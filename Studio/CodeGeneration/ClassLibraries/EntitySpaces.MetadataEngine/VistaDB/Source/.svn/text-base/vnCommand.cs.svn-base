using System;
using System.Data;
using System.ComponentModel;

namespace Provider.VistaDB
{

	/// <summary>
	/// Command object.
	/// </summary>
	[DesignTimeVisible(true)]
	public class VistaDBCommand: Component, IDbCommand
	{		
		private VistaDBConnection connection;
		private VistaDBTransaction vistaDBTransaction;
		private string commandText;
		private VistaDBSQLQuery sqlQuery;
		private int commandTimeOut = 0;

		/// <summary>
		/// Gets or sets how command results are applied to the DataRow when used by the Update method of the VistaDBDataAdapter.
		/// </summary>
		public UpdateRowSource updatedRowSource = UpdateRowSource.None;

		/// <summary>
		/// Represents a collection of parameters relevant to a VistaDBCommand as well as their respective mappings to 
		/// columns in a DataSet. This class cannot be inherited.For a list of all members of this type, 
		/// see VistaDBParameterCollection Members.
		/// </summary>
		public VistaDBParameterCollection parameters = new VistaDBParameterCollection();


		/// <summary>
		/// Constructor.
		/// </summary>
		public VistaDBCommand()
		{
			InitClass();

			commandText = "";
		}

		//' Implement other constructors here.
		/// <summary>
		/// Overloaded. Initializes a new instance of the VistaDBCommand class.
		/// </summary>
		public VistaDBCommand(string cmdText)
		{
			InitClass();

			commandText = cmdText;
		}

		/// <summary>
		/// Overloaded. Initializes a new instance of the VistaDBCommand class with a connection.
		/// </summary>
		public VistaDBCommand(string cmdText, VistaDBConnection connection)
		{
			InitClass();

			commandText = cmdText;
			this.connection = connection;

		}

		/// <summary>
		/// Overloaded. Initializes a new instance of the VistaDBCommand class with a connection and transaction.
		/// </summary>
		public VistaDBCommand(string cmdText, VistaDBConnection connection, VistaDBTransaction txn)
		{
			InitClass();

			commandText = cmdText;
			this.connection = connection;
			vistaDBTransaction = txn;
		}

		/// <summary>
		/// VistaDBCommand destructor
		/// </summary>
		~VistaDBCommand()
		{
			Dispose(false);
		}

		/// <summary>
		/// Used internally to initialize the object.
		/// </summary>
		public void InitClass()
		{
			VistaDBErrorMsgs.SetErrorFunc();
		}

		/// <summary>
		/// Gets or sets the V-SQL statement to execute at the data source.
		/// </summary>		
		[Browsable(true), Editor("VistaDB.Designer.VistaDBQueryEditor, VistaDB.Designer.VS2003", "System.Drawing.Design.UITypeEditor")]
		public string CommandText//IDbCommand.CommandText
		{
			get
			{
				return commandText;
			}
			set
			{
				commandText = value;
			}
		}

		/// <summary>
		/// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
		/// </summary>		
		public int CommandTimeout//IDbCommand.CommandTimeout
		{
			get
			{
				return commandTimeOut;
			}
			set
			{
				commandTimeOut = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating how the CommandText property is to be interpreted.
		/// </summary>		
		public CommandType CommandType//IDbCommand.CommandType
		{
			get
			{
				return CommandType.Text;
			}
			set
			{
				if(value != CommandType.Text)
					throw new NotSupportedException();
			}
		}

		IDbConnection IDbCommand.Connection
		{
			get
			{
				return (IDbConnection)connection;
			}
			set
			{
				if(connection != value)
				{
					connection = (VistaDBConnection)value;
					sqlQuery = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the VistaDBConnection used by this instance of the VistaDBCommand.
		/// </summary>		
		[TypeConverter(typeof(ComponentConverter))]
		public VistaDBConnection Connection
		{
			get
			{
				return connection;
			}
			set
			{
				if(connection != value)
				{
					connection = value;
					sqlQuery = null;
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		IDataParameterCollection IDbCommand.Parameters
		{
			get
			{
				return (VistaDBParameterCollection)parameters;
			}
		}

		/// <summary>
		/// Gets the VistaDBParameterCollection.
		/// </summary>		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public VistaDBParameterCollection Parameters
		{
			get
			{
				return parameters;
			}
		}

		[Browsable(false)]
		IDbTransaction IDbCommand.Transaction
		{
			get
			{
				return vistaDBTransaction;
			}
			set
			{
				vistaDBTransaction = (VistaDBTransaction)value;
			}
		}

		/// <summary>
		/// Gets or sets the VistaDBTransaction within which the VistaDBCommand executes.
		/// </summary>		
		[Browsable(false)]
		public VistaDBTransaction Transaction
		{
			get
			{
				return vistaDBTransaction;
			}
			set
			{
				vistaDBTransaction = value;
			}
		}

		/// <summary>
		/// Gets or sets how command results are applied to the DataRow when used by the Update method of the VistaDBDataAdapter.
		/// </summary>		
		public UpdateRowSource UpdatedRowSource//IDbCommand.UpdatedRowSource
		{
			get
			{
				return updatedRowSource;
			}
			set
			{
				updatedRowSource = value;
			}
		}

		/// <summary>
		/// Attempts to cancel the execution of a SqlCommand.
		/// </summary>
		public void Cancel()//IDbCommand.Cancel
		{
			throw new NotSupportedException();
		}

		IDbDataParameter IDbCommand.CreateParameter()//IDbCommand.CreateParameter
		{
			return new VistaDBParameter();
		}
		/// <summary>
		/// Creates a new instance of a VistaDBParameter object.
		/// </summary>
		public VistaDBParameter CreateParameter()
		{
			return new VistaDBParameter();
		}

		/// <summary>
		/// Overloaded. Releases the resources used by the component.
		/// </summary>
		/// <param name="disposing">True for external disposing</param>
		protected override void Dispose(bool disposing)
		{
			if(sqlQuery != null)
				sqlQuery.DropQuery();
			sqlQuery = null;
			base.Dispose(disposing);
		}

		/// <summary>
		/// Executes a V-SQL statement against the connection and returns the number of rows affected.
		/// </summary>
		public int ExecuteNonQuery()//IDbCommand.ExecuteNonQuery
		{
			if((connection == null || connection.State != ConnectionState.Open)&&
				(commandText.Substring(0, 6).ToUpper() != "CREATE"))
				throw new VistaDBException(VistaDBErrorCodes.ConnectionInvalid);

			if( sqlQuery == null )
				sqlQuery = connection.VistaDBSQL.NewSQLQuery();

			sqlQuery.SQL = commandText;
			AddSQLParameter(sqlQuery);
			sqlQuery.ExecSQL();

			return sqlQuery.RowsAffected;
		}

		IDataReader IDbCommand.ExecuteReader()//IDbCommand.ExecuteReader
		{
			return this.ExecuteReader();
		}

		/// <summary>
		/// Overloaded. Sends the CommandText to the Connection and builds a VistaDBDataReader object.
		/// </summary>
		public VistaDBDataReader ExecuteReader()
		{
			VistaDBSQLQuery query;
			string s;

			s = commandText.TrimStart(' ');
			s = (s.Substring(0,6)).ToUpper();

			if( s == "INSERT" || s == "UPDATE" || s == "DELETE" )
			{
				ExecuteNonQuery();
				return null;
			}

			if(connection == null || connection.State != ConnectionState.Open)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionInvalid);

			query = connection.VistaDBSQL.NewSQLQuery();

			query.SQL = commandText;
			AddSQLParameter(query);
			query.Open();

			return new VistaDBDataReader(query, true, null);
		}

		IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)//IDbCommand.ExecuteReader
		{
			return this.ExecuteReader(behavior);
		}

		/// <summary>
		/// Overloaded. Sends the CommandText to the Connection and builds a VistaDBDataReader object, passing in CommandBehavior.
		/// </summary>
		public VistaDBDataReader ExecuteReader(System.Data.CommandBehavior behavior)
		{
			VistaDBSQLQuery query;
			string s;
			VistaDBConnection conn;
			bool fillData;

			s = commandText.TrimStart(' ');
			s = (s.Substring(0,6)).ToUpper();

			if( s == "INSERT" || s == "UPDATE" || s == "DELETE" )
			{
				int rowsAffected = ExecuteNonQuery();
				return new VistaDBDataReader(rowsAffected);
			}

			if(connection == null || connection.State != ConnectionState.Open)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionInvalid);

			query = connection.VistaDBSQL.NewSQLQuery();

			query.SQL = commandText;
			AddSQLParameter(query);
			query.Open();

			conn     = (int)(behavior & CommandBehavior.CloseConnection) != 0 ? this.connection: null;
			fillData = ((int)(behavior & CommandBehavior.KeyInfo) == 0) && ((int)(behavior & CommandBehavior.SchemaOnly) == 0);

			return new VistaDBDataReader(query, fillData, conn);
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.
		/// </summary>
		public object ExecuteScalar()//IDbCommand.ExecuteScalar
		{
			VistaDBSQLQuery query;
			object res;

			if(connection == null || connection.State != ConnectionState.Open)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionInvalid);

			query = connection.VistaDBSQL.NewSQLQuery();

			query.SQL = commandText;
			AddSQLParameter(query);
			query.Open();

			if(!query.Opened || query.Eof)
				return null;

			res = query.GetValue(0);

			query.Close();

			query.DropQuery();

			return res;
		}

		private void AddSQLParameter(VistaDBSQLQuery query)
		{
			VistaDBType vdbType;

			foreach(VistaDBParameter vp in Parameters)
			{
				vdbType = vp.VistaDBType;

				if( vp.Value == null || vp.Value == DBNull.Value )
				{
					query.SetParamNull(vp.ParameterName, vdbType);
				}
				else
				{
					switch(vdbType)
					{
						case VistaDBType.Character:
							query.SetParameter(vp.ParameterName, VistaDBType.Character, (string)vp.Value);
							break;
						case VistaDBType.Date:
							query.SetParameter(vp.ParameterName, VistaDBType.Date, (DateTime)vp.Value);
							break;
						case VistaDBType.DateTime:
							query.SetParameter(vp.ParameterName, VistaDBType.DateTime, (DateTime)vp.Value);
							break;
						case VistaDBType.Int32:
							query.SetParameter(vp.ParameterName, VistaDBType.Int32, (int)vp.Value);
							break;
						case VistaDBType.Int64:
							query.SetParameter(vp.ParameterName, VistaDBType.Int64, (long)vp.Value);
							break;
						case VistaDBType.Boolean:
							query.SetParameter(vp.ParameterName, VistaDBType.Boolean, (bool)vp.Value);
							break;
						case VistaDBType.Double:
							query.SetParameter(vp.ParameterName, VistaDBType.Double, (double)vp.Value);
							break;
						case VistaDBType.Varchar:
							query.SetParameter(vp.ParameterName, VistaDBType.Varchar, (string)vp.Value);
							break;
						case VistaDBType.Memo:
							query.SetParameter(vp.ParameterName, VistaDBType.Memo, (string)vp.Value);
							break;
						case VistaDBType.Blob:
							query.SetParameter(vp.ParameterName, VistaDBType.Blob, vp.Value);
							break;
						case VistaDBType.Picture:
							query.SetParameter(vp.ParameterName, VistaDBType.Picture, vp.Value);
							break;
						case VistaDBType.Currency:
							query.SetParameter(vp.ParameterName, VistaDBType.Currency, (decimal)vp.Value);
							break;
						case VistaDBType.Guid:
							query.SetParameter(vp.ParameterName, VistaDBType.Guid, (Guid)vp.Value);
							break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a prepared version of the command on an instance of VistaDB.
		/// </summary>
		public void Prepare()
		{
			if(connection == null || connection.State != ConnectionState.Open)
				throw new VistaDBException( VistaDBErrorCodes.ConnectionInvalid );
		}

	}
}

