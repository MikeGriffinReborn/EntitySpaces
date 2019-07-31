#if !IGNORE_VISTA

using System;
using System.Data;
using System.ComponentModel;

namespace Provider.VistaDB
{

	/// <summary>
	/// A helper object that automatically generates and populates command properties of a VistaDBDataAdapter.
	/// </summary>
	[ToolboxItem(false)]
	public sealed class VistaDBCommandBuilder: Component
	{
		private VistaDBDataAdapter dataAdapter;
		private VistaDBCommand insertCommand;
		private VistaDBCommand updateCommand;
		private VistaDBCommand deleteCommand;

		private void RefreshSchema()
		{
			string commandText, tableName;
			int pos, i, k, textLen;
			string text, primaryKey, s, s2;
			VistaDBType fieldType;
			VistaDBParameter parameter;
			VistaDBDataReader reader;
			bool needClose;
			string columnName;

			if (dataAdapter.SelectCommand == null)
				return;

			//'Getting table name
			commandText = dataAdapter.SelectCommand.CommandText.ToUpper();

			tableName = "";
			pos = commandText.IndexOf("FROM");
			if (pos > 0)
			{
				pos += 5;
				textLen = commandText.Length;

				for(i = pos; i < textLen; i++)
				{
					if(commandText[i] != ' ')
					{
						for(k = i; k < textLen; k++)
						{
							if(commandText[k] == ' ' || commandText[k] == ';')
								break;
						}

						tableName = commandText.Substring(i, k - i);
						break;
					}
				}

				if(tableName == "")
					return;
			}
			else
				return;
			
			//Create data reader
			if( dataAdapter.SelectCommand.Connection.State != ConnectionState.Open )
			{
				needClose = true;
				dataAdapter.SelectCommand.Connection.Open();
			}
			else
				needClose = false;

			try
			{
				reader = dataAdapter.SelectCommand.ExecuteReader(CommandBehavior.KeyInfo);

				try
				{
					//Creating where expression
					primaryKey = "";
					for(i = 0; i < reader.Columns.Count; i++)
					{
						if( reader.Columns[i].Unique )
						{
							if(primaryKey != "")
								primaryKey += " and ";

							columnName = reader.Columns[i].Name;
							if(reader.Columns[i].ReservedWord)
								columnName = "[" + columnName + "]";

							primaryKey += columnName + " = @" + reader.Columns[i].Name;
						}
					}

					if(primaryKey == "")
					{
						throw new VistaDBException(VistaDBErrorCodes.TableDoesNotHaveUniqueColumn);
					}

					//Creation of VistaDBCommand's

					///////////////////////////////////////////////////////
					///////////////////////Update Command//////////////////
					///////////////////////////////////////////////////////
					text = "update " + tableName + " set ";

					//Create set expression
					s = "";

					for(i = 0; i < reader.Columns.Count; i++)
					{
						fieldType = reader.Columns[i].VistaDBType;

						if(!reader.Columns[i].Unique && !reader.Columns[i].Identity)
						{
							columnName = reader.Columns[i].Name;
							if(reader.Columns[i].ReservedWord)
								columnName = "[" + columnName + "]";

							text = text + s + columnName + " = @" + reader.Columns[i].Name;
							s = ",";
						}
					}

					//Create where expression
					text = text + " where " + primaryKey;

					updateCommand = new VistaDBCommand(text);
					updateCommand.Connection = dataAdapter.SelectCommand.Connection;
					updateCommand.CommandTimeout = dataAdapter.SelectCommand.CommandTimeout;
					updateCommand.Transaction = dataAdapter.SelectCommand.Transaction;

					//Create parameters
					for(i = 0; i < reader.Columns.Count; i++)
					{
						fieldType = reader.Columns[i].VistaDBType;

						parameter = new VistaDBParameter("@" + reader.Columns[i].Name, fieldType, reader.Columns[i].Name);
						parameter.SourceVersion = DataRowVersion.Current;
						updateCommand.Parameters.Add(parameter);
					}

					/////////////////////////////////////////////////////////
					//////////////////////Insert(Command)////////////////////
					/////////////////////////////////////////////////////////
					text = "insert into " + tableName + "(";

					//Create into and values expression
					s = "";
					s2 = " VALUES (";

					for(i = 0; i < reader.Columns.Count; i++)
					{
						fieldType = reader.Columns[i].VistaDBType;

						if (!reader.Columns[i].Identity)
						{
							columnName = reader.Columns[i].Name;
							if(reader.Columns[i].ReservedWord)
								columnName = "[" + columnName + "]";

							text = text + s + columnName;
							s2 = s2 + s + " @" + reader.Columns[i].Name;
							s = ",";
						}
					}

					text = text + ")" + s2 + ")";

					insertCommand = new VistaDBCommand(text,
						(VistaDBConnection)(dataAdapter.SelectCommand.Connection));
					insertCommand.Connection = dataAdapter.SelectCommand.Connection;
					insertCommand.CommandTimeout = dataAdapter.SelectCommand.CommandTimeout;
					insertCommand.Transaction = dataAdapter.SelectCommand.Transaction;

					//'CREATE(PARAMETERS)
					for(i = 0; i < reader.Columns.Count; i++)
					{
						fieldType = reader.Columns[i].VistaDBType;

						if(!reader.Columns[i].Identity)
						{
							parameter = new VistaDBParameter("@" + reader.Columns[i].Name, fieldType, reader.Columns[i].Name);
							parameter.SourceVersion = DataRowVersion.Current;
							insertCommand.Parameters.Add(parameter);
						}
					}

					// Delete Command
					text = "delete from " + tableName + " where " + primaryKey;
					deleteCommand = new VistaDBCommand(text);

					// Create parameters
					for(i = 0; i < reader.Columns.Count; i++)
					{
						if( reader.Columns[i].Unique )
						{
							fieldType = reader.Columns[i].VistaDBType;
							parameter = new VistaDBParameter("@" + reader.Columns[i].Name, fieldType, reader.Columns[i].Name);
							parameter.SourceVersion = DataRowVersion.Original;
							deleteCommand.Parameters.Add(parameter);
						}
					}

					deleteCommand.Connection = dataAdapter.SelectCommand.Connection;
					deleteCommand.CommandTimeout = dataAdapter.SelectCommand.CommandTimeout;
					deleteCommand.Transaction = dataAdapter.SelectCommand.Transaction;
				}
				finally
				{
					reader.Close();
				}
			}
			finally
			{
				if( needClose )
					dataAdapter.SelectCommand.Connection.Close();
			}
		}

		private void OnRowUpdating(object sender, VistaDBRowUpdatingEventArgs e)
		{
			StatementType type1;

			type1 = e.StatementType;

			switch(type1)
			{
				case StatementType.Insert:
					if (e.Command != null)
						return;

					e.Command = insertCommand;
					RetrieveParameterValues(e.Command, e.Row);
					break;
				case StatementType.Update:
					if (e.Command != null)
						return;

					e.Command = updateCommand;
					RetrieveParameterValues(e.Command, e.Row);
					break;

				case StatementType.Delete:
					if (e.Command != null)
						return;

					e.Command = deleteCommand;
					RetrieveParameterValues(e.Command, e.Row);
					break;
			}
		}

		private void RegisterDataAdapter(VistaDBDataAdapter adapter)
		{
			dataAdapter = adapter;
			dataAdapter.RowUpdating += new VistaDBRowUpdatingEventHandler(OnRowUpdating);
			RefreshSchema();
		}

		private void UnregisterDataAdapter()
		{
			dataAdapter.RowUpdating -= new VistaDBRowUpdatingEventHandler(OnRowUpdating);

			dataAdapter = null;
			insertCommand = null;
			updateCommand = null;
			deleteCommand = null;
		}

		private void RetrieveParameterValues(VistaDBCommand command, DataRow row)
		{
			object o;

			foreach(VistaDBParameter parameter1 in command.Parameters)
			{
				o = row[parameter1.SourceColumn, parameter1.SourceVersion];
				if(o is System.DBNull)
				{
					parameter1.Value = null;
				}
				else
					parameter1.Value = o;
			}

		}

		/// <summary>
		/// Overloaded constructor.
		/// </summary>
		public VistaDBCommandBuilder(VistaDBDataAdapter adapter)
		{
			VistaDBErrorMsgs.SetErrorFunc();

			dataAdapter = null;
			insertCommand = null;
			updateCommand = null;
			deleteCommand = null;

			RegisterDataAdapter(adapter);
		}

		/// <summary>
		/// Overloaded constructor.
		/// </summary>
		public VistaDBCommandBuilder()
		{
			VistaDBErrorMsgs.SetErrorFunc();

			//'This call is required by the Component Designer.
			InitializeComponent();

			dataAdapter = null;
			insertCommand = null;
			updateCommand = null;
			deleteCommand = null;
		}

		/// <summary>
		/// VistaDBCommandBuilder destructor
		/// </summary>
		~VistaDBCommandBuilder()
		{
			Dispose(false);
		}

		//'Required by the Component Designer
		private IContainer components;

		//'NOTE: The following procedure is required by the Component Designer
		//'It can be modified using the Component Designer.
		//'Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			components = new Container();
		}
		/// <summary>
		///  Gets or sets a VistaDBDataAdapter object for which V-SQL statements are automatically generated.
		/// </summary>
		public VistaDBDataAdapter DataAdapter
		{
			get
			{
				return dataAdapter;
			}

			set
			{
				if(dataAdapter != null)
					UnregisterDataAdapter();

				if (value == null)
				{
					dataAdapter = null;
					return;
				}

				RegisterDataAdapter(value);

			}
		}
		/// <summary>
		/// Gets the automatically generated VistaDBCommand object required to perform insertions on the database.
		/// </summary>
		public VistaDBCommand InsertCommand
		{
			get
			{
				return insertCommand;
			}
		}

		/// <summary>
		/// Gets the automatically generated VistaDBCommand object required to perform updates on the database.
		/// </summary>
		public VistaDBCommand UpdateCommand
		{
			get
			{
				return updateCommand;
			}
		}

		/// <summary>
		/// Gets the automatically generated VistaDBCommand object required to perform deletions on the database.
		/// </summary>
		public VistaDBCommand DeleteCommand
		{
			get
			{
				return deleteCommand;
			}
		}

		/// <summary>
		/// Overloaded. Releases the resources used by the component.
		/// </summary>
		/// <param name="disposing">True for external disposing</param>
		protected override void Dispose(bool disposing)
		{
			if (dataAdapter != null)
				UnregisterDataAdapter();
			base.Dispose(disposing);
		}

		/// <summary>
		/// Retrieves parameter information from the stored procedure specified in the VistaDBCommand and populates the Parameters collection of the specified VistaDBCommand object. Now do nothing.
		/// </summary>
		/// <param name="command">The VistaDBCommand referencing the stored procedure from which the parameter information is to be derived. The derived parameters are added to the Parameters collection of the VistaDBCommand.</param>
		public static void DeriveParameters(VistaDBCommand command)
		{
			if( command.parameters.Count != 0 )
				command.parameters = new VistaDBParameterCollection();
		}

		/// <summary>
		/// Gets the automatically generated VistaDBCommand object required to perform insertions on the database.
		/// </summary>
		public VistaDBCommand GetInsertCommand()
		{
			return insertCommand;
		}

		/// <summary>
		/// Gets the automatically generated VistaDBCommand object required to perform updates on the database.
		/// </summary>
		public VistaDBCommand GetUpdateCommand()
		{
			return updateCommand;
		}

		/// <summary>
		/// Gets the automatically generated VistaDBCommand object required to perform deletions on the database.
		/// </summary>
		public VistaDBCommand GetDeleteCommand()
		{
			return deleteCommand;
		}
	}
}
#endif
