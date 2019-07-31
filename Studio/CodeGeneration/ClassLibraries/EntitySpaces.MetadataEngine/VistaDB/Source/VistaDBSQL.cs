#if !IGNORE_VISTA

using System;
using System.Data;
// using System.Windows.Forms;
using System.Text;
using System.ComponentModel;

namespace VistaDB
{

	/// <summary>
	/// Table column definition.
	/// </summary>
	public class VistaDBColumn
	{
		private string name;
		private VistaDBType vistaDBType;
		private Type type;
		private int dataSize;
		private short columnWidth;
		private short columnDecimals;//
		private bool allowNull;
		private bool readOnly;
		private bool primaryKey;
		private bool unique;
		private bool identity;
		private double identityStep;//
		private string identityValue;//
		private string columnCaption;
		private string columnDescription;//

		private bool packed;
		private bool hidden;
		private bool encrypted;
		private bool unicode;

		private bool reservedWord;
		
		internal VistaDBColumn(string _name, 
			VistaDBType _vistaDBType,
			int _dataSize,
			short _columnWidth,
			short _columnDecimals,
			bool _allowNull, 
			bool _readOnly, 
			bool _primaryKey,
			bool _unique,
			bool _identity,
			double _identityStep,
			string _identityValue,
			string _columnCaption,
			string _columnDescription,
			bool _reservedWord,
			bool _packed,
			bool _hidden,
			bool _encrypted,
			bool _unicode)
		{
			name = _name;
			vistaDBType = _vistaDBType;
			type = VistaDBAPI.GetFieldType(vistaDBType);
			dataSize = _dataSize;
			columnWidth = _columnWidth;
			columnDecimals = _columnDecimals;
			allowNull = _allowNull;
			readOnly = _readOnly;
			primaryKey = _primaryKey;
			unique = _unique;
			identity = _identity;
			identityStep = _identityStep;
			identityValue = _identityValue;
			columnCaption = _columnCaption;
			columnDescription = _columnDescription;
			reservedWord = _reservedWord;
			packed = _packed;
			hidden = _hidden;
			encrypted = _encrypted;
			unicode = _unicode;
		}

		/// <summary>
		/// Column name.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
			}
		}

		/// <summary>
		/// VistaDBType for the column.
		/// </summary>
		public VistaDBType VistaDBType
		{
			get
			{
				return vistaDBType;
			}
			set
			{
			}
		}

		/// <summary>
		/// Column type.
		/// </summary>
		public Type Type
		{
			get
			{
				return type;
			}
			set
			{
			}
		}

		/// <summary>
		/// Physical size of column. 
		/// </summary>
		public int DataSize
		{
			get
			{
				return dataSize;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns the column width. Useful for Varchar and Character column types.
		/// </summary>
		public short ColumnWidth
		{
			get
			{
				return columnWidth;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns the column decimals.
		/// </summary>
		public short ColumnDecimals
		{
			get
			{
				return columnDecimals;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if column can store NULL values.
		/// </summary>
		public bool AllowNull
		{
			get
			{
				return allowNull;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if the column is readonly.
		/// </summary>
		public bool ReadOnly
		{
			get
			{
				return readOnly;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if the column participates in the Primary Key index.
		/// </summary>
		public bool PrimaryKey
		{
			get
			{
				return primaryKey;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if the column participates in the Unique index
		/// </summary>
		public bool Unique
		{
			get
			{
				return unique;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if the column is an Identity field.
		/// </summary>
		public bool Identity
		{
			get
			{
				return identity;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns increment step for identity
		/// </summary>
		public double IdentityStep
		{
			get
			{
				return identityStep;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns current value for identity
		/// </summary>
		public string IdentityValue
		{
			get
			{
				return identityValue;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns the Caption for the column.
		/// </summary>
		public string ColumnCaption
		{
			get
			{
				return columnCaption;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns the column description
		/// </summary>
		public string ColumnDescription
		{
			get
			{
				return columnDescription;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if column name is reserved word
		/// </summary>
		public bool ReservedWord
		{
			get
			{
				return reservedWord;
			}
			set
			{
			}
		}

		/// <summary>
		/// Return true if column is packed
		/// </summary>
		public bool Packed
		{
			get
			{
				return this.packed;
			}
			set
			{
			}
		}

		/// <summary>
		/// Return true if column marked as hidden
		/// </summary>
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
			set
			{
			}
		}

		/// <summary>
		/// Return encrypted if column encrypted
		/// </summary>
		public bool Encrypted
		{
			get
			{
				return this.encrypted;
			}
			set
			{
			}
		}

		/// <summary>
		/// Not supported property. Always return false.
		/// </summary>
		public bool Unicode
		{
			get
			{
				return this.unicode;
			}
			set
			{
			}
		}
	}

	/// <summary>
	/// V-SQL query class
	/// </summary>
	public class VistaDBSQLQuery
	{
		private int rowsAffected;
		private int sqlID;
		private int queryID;
		private int recordCount;
		private string commandText;
		private bool opened;

		private int columnCount;
		private string password;

		private int errorNumber;

		private VistaDBColumn[] columns;
		private VistaDBSQL parent;

		private static object syncRoot = new object();

		internal VistaDBSQLQuery(VistaDBSQL p)
		{
			rowsAffected = 0;
			sqlID = 0;
			queryID = 0;
			recordCount = 0;
			commandText = "";
			columnCount = 0;
			password = "";
			errorNumber = 0;
			parent = p;
			opened = false;

			Columns = new VistaDBColumnCollection(this);

			queryID = VistaDBAPI.ivsql_CreateQuery(p.ConnectionID);
		}

		internal void CreateQuery()
		{
			if( queryID > 0 )
				return;

			queryID = VistaDBAPI.ivsql_CreateQuery(parent.ConnectionID);
		}

		internal void FreeQuery()
		{
			if( queryID <= 0 )
				return;

			VistaDBAPI.ivsql_FreeQuery(queryID);

			queryID = 0;
		}

		/// <summary>
		/// Used internally.
		/// </summary>
		public void DropQuery()
		{
			parent.DropQuery(this);
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~VistaDBSQLQuery()
		{
			Close();

			columns = null;

			if( queryID > 0 )
			{
				VistaDBAPI.ivsql_FreeQuery(queryID);
			}
		}


		///////////////////////////////////////////////////////////
		////////////////Open and prepare SQL functions/////////////
		///////////////////////////////////////////////////////////

		/// <summary>
		/// Prepare V-SQL statement for opening or executing a query.
		/// </summary>
		public void Prepare()
		{
			//Prepare SQL Query
			//Take SQL ID
			sqlID = VistaDBAPI.ivsql_Prepare(queryID);
		}

		/// <summary>
		/// Unprepare a V-SQL statement from a previous call to Prepare.
		/// </summary>
		public void UnPrepare()
		{
			//Un prepare SQL
			VistaDBAPI.ivsql_UnPrepare(queryID);
		}

		/// <summary>
		/// Open a V-SQL query. Open is used with SELECT statements only.
		/// </summary>
		public void Open()
		{
			//Open SQL Query
			//Set rowsAffected
			//Fill 'columns' (meta data structure)

			if( queryID <= 0 )
				return;

			int rowsaffected = 0;

			lock(syncRoot)
			{
				VistaDBAPI.ivsql_SetSQL(queryID, commandText);

				sqlID = VistaDBAPI.ivsql_Open(queryID, ref rowsaffected);

				InternalInitFieldDefs();

				recordCount = VistaDBAPI.ivsql_RecCount(queryID);

				rowsAffected = rowsaffected;

				opened = true;
			}
		}

		/// <summary>
		/// Close a V-SQL query.
		/// </summary>
		public void Close()
		{
			//Call SQL API function for close query
			
			if( opened )
			{
				lock(syncRoot)
				{
					if( queryID <= 0 )
						return;

					VistaDBAPI.ivsql_Close(queryID);
					columns = null;
					opened = false;
				}
			}
		}

		/// <summary>
		/// Execute a V-SQL query that does not return a result set. These include INSERT, DELETE and UPDATE.
		/// </summary>
		public void ExecSQL()
		{
			//Execute SQL command
			//Set rowsAffected
			int rowsaffected = 0;

			if( queryID <= 0 )
				return;

			if(opened)
				throw new InvalidOperationException("Query opened");

			lock(syncRoot)
			{
				VistaDBAPI.ivsql_SetSQL(queryID, commandText);			
				sqlID = VistaDBAPI.ivsql_ExecSQL(queryID, ref rowsaffected);
				rowsAffected = rowsaffected;
			}
		}


		////////////////////////////////////////////////////////////
		////////////////Data and parameter functions////////////////
		////////////////////////////////////////////////////////////

		/// <summary>
		/// Set a V-SQL parameter.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="dataType">Parameter data type.</param>
		/// <param name="value">Data value</param>
		public void SetParameter(string paramName, VistaDBType dataType, object value)
		{
			//Set parameter value
			//Depending on 'dataType' this function call different
			//API SQL functions for this 'dataType'
			//If 'value' is null then set parameter to null value
			long tickCount;

			if( queryID <= 0 )
				return;

			switch(dataType)
			{
				case VistaDBType.Character:
					VistaDBAPI.ivsql_SetParamString(queryID, paramName, (string)value);
					break;
				case VistaDBType.Date:
					tickCount = ((DateTime)value).Ticks;
					VistaDBAPI.ivsql_SetParamDate(queryID, paramName, (long)tickCount);
					break;
				case VistaDBType.DateTime:
					tickCount = ((DateTime)value).Ticks;
					VistaDBAPI.ivsql_SetParamDateTime(queryID, paramName, (long)tickCount);
					break;
				case VistaDBType.Boolean:
					VistaDBAPI.ivsql_SetParamBoolean(queryID, paramName, (bool)value);
					break;
				case VistaDBType.Memo:
					VistaDBAPI.ivsql_SetParamMemo(queryID, paramName, (string)value);
					break;
				case VistaDBType.Picture:
					VistaDBAPI.ivsql_SetParamBlob(queryID, paramName, (byte[])value, ((byte[])value).Length ); 
					break;
				case VistaDBType.Blob:
					VistaDBAPI.ivsql_SetParamBlob(queryID, paramName, (byte[])value, ((byte[])value).Length ); 
					break;
				case VistaDBType.Currency:

					decimal dVal = (decimal)value;
					long longValue;
					longValue = (long)(dVal * 10000);

					VistaDBAPI.ivsql_SetParamCurrency(queryID, paramName, longValue);
					break;
				case VistaDBType.Int32:
					VistaDBAPI.ivsql_SetParamInt32(queryID, paramName, (int)value);
					break;
				case VistaDBType.Int64:
					VistaDBAPI.ivsql_SetParamInt64(queryID, paramName, (long)value);
					break;
				case VistaDBType.Double:
					VistaDBAPI.ivsql_SetParamDouble(queryID, paramName, (double)value);
					break;
				case VistaDBType.Varchar:
					VistaDBAPI.ivsql_SetParamVarchar(queryID, paramName, (string)value);
					break;
				case VistaDBType.Guid:
					VistaDBAPI.ivsql_SetParamGuid(queryID, paramName, (Guid)value);
					break;
			}
			
		}
		/// <summary>
		/// Returns True if the V-SQL parameter value is NULL.
		/// </summary>
		public bool ParamIsNull(string pName)
		{
			bool res;

			if( queryID <= 0 )
				return false;

			res = VistaDBAPI.ivsql_ParamIsNull(queryID, pName);

			return res;
		}

		/// <summary>
		/// Set a V-SQL parameter value to NULL.
		/// </summary>
		public void SetParamNull(string pName, VistaDBType type)
		{
			if( queryID <= 0 )
				return;

			VistaDBAPI.ivsql_SetParamNull(queryID, pName, (short)type);
		}

		/// <summary>
		/// Return parameter value. Not implemented.
		/// </summary>
		/// <param name="paramName">Parameter name</param>
		/// <returns>Parameter value</returns>
		public object GetParameter(string paramName)
		{
			//Return parameter value
			return null;
		}

		/// <summary>
		/// Returns if the V-SQL query has been opened. Applies to SELECT statements only.
		/// </summary>
		public bool Opened
		{
			get
			{
				return opened;
			}
		}

		//////////////////////////////////////////////////////////
		///////////////Navigation functions///////////////////////
		//////////////////////////////////////////////////////////

		/// <summary>
		/// Go to the first row in the dataset.
		/// </summary>
		/// <returns>False if current position doesn't change</returns>
		public bool First()
		{
			if( queryID <= 0 )
				return false;

			//Call SQL API function to move to the first position
			VistaDBAPI.ivsql_First(queryID);

			return true;
		}

		/// <summary>
		/// Go to the last row in the dataset.
		/// </summary>
		/// <returns>False if current position doesn't change</returns>
		public bool Last()
		{
			if( queryID <= 0 )
				return false;

			//Call SQL API function to move to the last position
			VistaDBAPI.ivsql_Last(queryID);

			return true;
		}

		/// <summary>
		/// Go to the next row in dataset
		/// </summary>
		/// <returns>False if current position doesn't change</returns>
		public bool Next()
		{
			if( queryID <= 0 )
				return false;

			//Call SQL API function to move to the next position
			// VistaDBAPI.ivsql_MoveBy(queryID, 1);
			VistaDBAPI.ivsql_Next(queryID);

			return true;
		}

		/// <summary>
		/// Go to previous row in the dataset
		/// </summary>
		/// <returns>False if current position doesn't change</returns>
		public bool Prior()
		{
			if( queryID <= 0 )
				return false;

			//Call SQL API function to move to the previous position
			// VistaDBAPI.ivsql_MoveBy(queryID, -1);
			VistaDBAPI.ivsql_Prior(queryID);

			return false;
		}

		/// <summary>
		/// End of file. Tests if a row movement function has placed the row pointer beyond the last row in the dataset.
		/// </summary>
		public bool Eof
		{
			get
			{
				bool res;

				if( queryID <= 0 )
					return false;

				res = VistaDBAPI.ivsql_Eof(queryID) != 0;

				return res;
			}
		}

		/// <summary>
		/// Begining of file. Tests if a row movement function has placed the row pointer before the first row in the dataset.
		/// </summary>
		public bool Bof
		{
			get
			{
				bool res;

				if( queryID <= 0 )
					return false;

				res = VistaDBAPI.ivsql_Bof(queryID) != 0;

				return res;
			}
		}


		//////////////////////////////////////////////////////////
		///////////////////Meta data functions////////////////////
		//////////////////////////////////////////////////////////
		
		/// <summary>
		/// Return the number of rows affected.
		/// </summary>
		public int RowsAffected
		{
			get
			{
				return rowsAffected;
			}
		}

		/// <summary>
		/// Return the column count for the V-SQL query.
		/// </summary>
		public int ColumnCount
		{
			get
			{
				return columnCount;
			}
		}

		/// <summary>
		/// The collection of column objects in the V-SQL query.
		/// </summary>
		public class VistaDBColumnCollection
		{
			private VistaDBSQLQuery parent;

			internal VistaDBColumnCollection(VistaDBSQLQuery p)
			{
				parent = p;
			}

			/// <summary>
			/// Gets the column object at the specified offset.
			/// </summary>
			public VistaDBColumn this[int i]
			{
				get
				{
					if( i < 0 || i > parent.columns.GetUpperBound(0) )
						return  null;
					else
						return parent.columns[i];
				}
			}

			/// <summary>
			/// Gets the number of columns in the collection.
			/// </summary>
			public int Count
			{
				get
				{
					return parent.columns.Length;
				}
			}

		}
		
		/// <summary>
		/// Column objects in the collection.
		/// </summary>
		public readonly VistaDBColumnCollection Columns;

		/// <summary>
		/// Returns the value of the column at the given position in the table schema. The first column is 1.
		/// </summary>
		public object GetValue(int fieldNo)
		{
			DateTime		dt;
			int				dataLen;
			StringBuilder	tmpstr;

			object res;
			res = null;

			if( queryID <= 0 )
				return null;

			if( IsNull(fieldNo) )
				res = null;
			else
			{
				switch(columns[fieldNo].VistaDBType)
				{
					case VistaDBType.Character:
						dataLen = columns[fieldNo].DataSize;
						tmpstr	= new StringBuilder( dataLen );
						dataLen = VistaDBAPI.ivsql_GetString(queryID, fieldNo + 1, tmpstr, dataLen);
						tmpstr.Length = dataLen;
						res = tmpstr.ToString();
						break;
					case VistaDBType.Date:
						long longdate = VistaDBAPI.ivsql_GetDate(queryID, fieldNo + 1);
						dt = new DateTime(longdate);
						res = dt;
						break;
					case VistaDBType.DateTime:
						long longdatetime = VistaDBAPI.ivsql_GetDateTime(queryID, fieldNo + 1);

						if (longdatetime==0)
						{
							res = null;
						}
						else
						{
							if ((longdatetime>=DateTime.MinValue.Ticks) && (longdatetime<=DateTime.MaxValue.Ticks))
							{
								dt = new DateTime(longdatetime);
								res = dt;
							}
							else
								res = null;
						}
						break;
					case VistaDBType.Boolean:
						res = VistaDBAPI.ivsql_GetBoolean(queryID, fieldNo + 1);
						break;
					case VistaDBType.Int32:
						res = VistaDBAPI.ivsql_GetInt32(queryID, fieldNo + 1);
						break;
					case VistaDBType.Int64:
						res = VistaDBAPI.ivsql_GetInt64(queryID, fieldNo + 1);
						break;
					case VistaDBType.Currency:
						long longValue;		
						longValue = VistaDBAPI.ivsql_GetCurrency(queryID, fieldNo + 1);
						res = (decimal)((double)(longValue/10000d));
						break;
					case VistaDBType.Double:
						res = VistaDBAPI.ivsql_GetDouble(queryID, fieldNo + 1);
						break;
					case VistaDBType.Memo:
						dataLen	= VistaDBAPI.ivsql_GetBlobLength(queryID, fieldNo + 1);
						tmpstr	= new StringBuilder(dataLen);
						dataLen = VistaDBAPI.ivsql_GetMemo(queryID, fieldNo + 1, tmpstr, dataLen);

						tmpstr.Length = dataLen;
						res = tmpstr.ToString();
						break;
					case VistaDBType.Picture:
					case VistaDBType.Blob:
						int blobLen = 0;
						byte[] blobContent;
						blobContent = null;
						blobLen = VistaDBAPI.ivsql_GetBlobLength(queryID, fieldNo + 1);
						blobContent = new byte[blobLen];
						VistaDBAPI.ivsql_GetBlob(queryID, fieldNo + 1, blobContent, blobLen);
						res = blobContent;
						break;
					case VistaDBType.Varchar:
						dataLen = columns[fieldNo].DataSize;
						tmpstr	= new StringBuilder(dataLen);
						dataLen = VistaDBAPI.ivsql_GetVarchar(queryID, fieldNo + 1, tmpstr, dataLen);

						tmpstr.Length = dataLen;
						res = tmpstr.ToString();
						break;
					case VistaDBType.Guid:
						res = VistaDBAPI.ivsql_GetGuid(queryID, fieldNo + 1);
						break;
				}

			}

			return res;
		}

		/// <summary>
		/// Returns the number of records retrieved by the last V-SQL query statement. Applies to SELECT.
		/// </summary>
		public int RecordCount
		{
			get
			{
				return recordCount;
			}

		}

		/// <summary>
		/// Gets or sets the database password. This value must be set when accessing a Password-protected database.
		/// </summary>
		public string Password
		{
			get
			{
				return password;
			}

			set
			{
				password = value;
			}
		}

		/// <summary>
		/// Gets or sets the V-SQL query statement to be executed.
		/// </summary>
		public string SQL
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
		

		// public bool IsNull(string columnName)
		/// <summary>
		/// Return True if a column value is NULL at the given position in the table schema. The first column is 1.
		/// </summary>
		public bool IsNull(int columnNumber)
		{
			bool res;

			if( queryID <= 0 )
				return false;

			res = VistaDBAPI.ivsql_IsNull(queryID, columnNumber+1);

			return res;
		}

		private int InternalInitFieldDefs()
		{
			if( queryID <= 0 )
				return 0;

			lock(syncRoot)
			{
				recordCount = VistaDBAPI.ivsql_RecCount(queryID);
				columnCount = VistaDBAPI.ivsql_ColumnCount(queryID);
				columns = new VistaDBColumn[columnCount];
				char colType;
				int len;

				int columnCaptionWidth = 128;
				StringBuilder columnCaption = new StringBuilder(columnCaptionWidth);

				int columnNameWidth = 128;
				StringBuilder columnName = new StringBuilder(columnNameWidth);

				VistaDBType columntype;
				bool allowNull, readOnly, autoIncrement, primaryKey, unique, reservedWord;
				int dataSize, columnWidth;

				string tempS;
				for(int i = 0; i<columnCount; i++)
				{

					len = VistaDBAPI.ivsql_ColumnName(queryID, i + 1, columnName, columnNameWidth );
					columnName.Length = len;

					len = VistaDBAPI.ivsql_ColumnCaption(queryID, i+1, columnCaption, columnCaptionWidth);
					columnCaption.Length = len;

					colType = VistaDBAPI.ivsql_ColumnType(queryID, i + 1);
					tempS = colType.ToString();

					columntype    = VistaDBAPI.NetDataType( tempS );

					dataSize      = 0; // default
					columnWidth   = 0; // default
					allowNull     = !VistaDBAPI.ivsql_ColumnRequired(queryID, i+1);
					readOnly      = VistaDBAPI.ivsql_ColumnReadOnly(queryID, i+1);
					autoIncrement = VistaDBAPI.ivsql_ColumnIsIdentity(queryID, i+1);
					primaryKey    = VistaDBAPI.ivsql_ColumnIsPrimaryKey(queryID, i+1);
					unique				= VistaDBAPI.ivsql_ColumnIsUnique(queryID, i+1);
					reservedWord	= VistaDBAPI.ivsql_IsReservedWord(columnName.ToString());

					switch(columntype)
					{
						case VistaDBType.Character:
							columnWidth = VistaDBAPI.ivsql_ColumnWidth(queryID, i + 1);
							dataSize = columnWidth;
							break;
						case VistaDBType.Varchar:
							columnWidth = VistaDBAPI.ivsql_ColumnWidth(queryID, i + 1);
							dataSize = columnWidth;
							break;
						case VistaDBType.Date:
							dataSize = 8;
							break;
						case VistaDBType.DateTime:
							dataSize = 8;
							break;
						case VistaDBType.Boolean:
							dataSize = 2;
							break;
						case VistaDBType.Int32:
							dataSize = 4;
							break;
						case VistaDBType.Int64:
							dataSize = 8;
							break;
						case VistaDBType.Currency:
							dataSize = 8;
							break;
						case VistaDBType.Double:
							dataSize = 8;
							break;
						case VistaDBType.Memo:
						case VistaDBType.Blob:
						case VistaDBType.Picture:
							dataSize = 2147483647;
							break;
						case VistaDBType.Guid:
							dataSize = 16;
							break;
					}

					columns[i] = new VistaDBColumn( columnName.ToString(), columntype, dataSize, (short)columnWidth, 0, allowNull, readOnly, primaryKey, unique, autoIncrement, 0, "", columnCaption.ToString(), "", reservedWord, false, false, false, false);
				}
			}
	
			return errorNumber;
		}
	}

	/// <summary>
	/// VistaDBSQL class for managing V-SQL query statements.
	/// </summary>
	public class VistaDBSQL
	{
		private string dataSource; 
		private string dataBase;
		private CypherType cypher;
		private string password;
		private bool exclusive;
		private bool readOnly;
		private bool opened;
		private VistaDBSQLQuery[] queries;
		private int connectionID;
		private object syncRoot = new object();

		/// <summary>
		/// Constructor.
		/// </summary>
		public VistaDBSQL()
		{
			dataSource = "";
			dataBase = "";
			cypher = CypherType.None;
			password = "";
			exclusive = false;
			readOnly = false;
			opened = false;
			queries = new VistaDBSQLQuery[0];

			connectionID = VistaDBAPI.ivsql_CreateDatabaseConnection();
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~VistaDBSQL()
		{
			CloseDatabaseConnection();

			VistaDBAPI.ivsql_FreeDatabaseConnection(connectionID);
		}

		/// <summary>
		/// Open a database connection to a VistaDB database.
		/// </summary>
		/// <returns></returns>
		public void OpenDatabaseConnection()
		{
			if( opened )
				return;

			bool success = false;
			CypherType _cypher;
			string _password;

			lock(syncRoot)
			{
				try
				{
					if(cypher == CypherType.None)
					{
						_cypher = CypherType.Blowfish;
						_password = "";
					}
					else
					{
						_cypher = cypher;
						_password = password;
					}

					success = VistaDBAPI.ivsql_OpenDatabaseConnection(connectionID, dataSource, exclusive, readOnly, _cypher, _password, false);
				}
				catch(VistaDBException e)
				{
					if(!e.Critical)
					{
						for(int i = 0; i < queries.Length; i++)
							queries[i].CreateQuery();

						opened = true;
					}

					throw;
				}

				if (!success )
				{
					throw new VistaDBException(VistaDBErrorCodes.SQLDatabaseCouldNotBeFound);
				}

				for(int i = 0; i < queries.Length; i++)
					queries[i].CreateQuery();

				opened = true;
			}
		}

		/// <summary>
		/// Close an active database connection.
		/// </summary>
		public void CloseDatabaseConnection()
		{
			if( ! opened )
				return;

			lock(syncRoot)
			{
				for(int i = 0; i < queries.Length; i++)
					queries[i].FreeQuery();

				VistaDBAPI.ivsql_CloseDatabaseConnection( connectionID);
				opened = false;
			}
		}

		/// <summary>
		/// Internal. Create new query for this connection.
		/// </summary>
		/// <returns></returns>
		internal VistaDBSQLQuery NewSQLQuery()
		{
			VistaDBSQLQuery query;
			VistaDBSQLQuery[] newQueries;

			query = new VistaDBSQLQuery(this);

			lock(syncRoot)
			{
				newQueries = new VistaDBSQLQuery[queries.Length + 1];

				for(int i = 0; i < queries.Length; i++ )
				{
					newQueries[i] = queries[i];
				}

				queries = newQueries;

				queries[queries.GetUpperBound(0)] = query;
			}

			return query;
		}

		internal bool DropQuery(VistaDBSQLQuery query)
		{
			int indexQuery = -1;
			VistaDBSQLQuery[] newQueries;

			lock(syncRoot)
			{
				for(int i = 0; i < queries.Length; i++)
				{
					if( queries[i] == query )
					{
						indexQuery = i;
						break;
					}
				}

				if(indexQuery < 0)
					return false;

				queries[indexQuery].FreeQuery();

				newQueries = new VistaDBSQLQuery[queries.Length - 1];

				for(int i = 0; i < newQueries.Length; i++)
				{
					if( i < indexQuery )
						newQueries[i] = queries[i];
					else
						newQueries[i] = queries[i + 1];
				}

				queries = newQueries;
			}

			return true;
		}

		/// <summary>
		/// Return a unique connection ID to the opened database.
		/// </summary>
		public int ConnectionID
		{
			get
			{
				return connectionID;
			}
		}

		/// <summary>
		/// Begin a transaction. Transactions may be nested.
		/// </summary>
		public bool BeginTransaction()
		{
			bool res;

			res = VistaDBAPI.ivsql_BeginTransaction(connectionID);

			return res;
		}

		/// <summary>
		/// Commit an active transaction. Transactions may be nested.
		/// </summary>
		public bool CommitTransaction()
		{
			bool res;

			res = VistaDBAPI.ivsql_CommitTransaction(connectionID);

			return res;
		}

		/// <summary>
		/// Rollback the active transaction. Transactions may be nested. 
		/// </summary>
		public void RollbackTransaction()
		{
			VistaDBAPI.ivsql_RollbackTransaction(connectionID);
		}

		/// <summary>
		/// Gets or sets the data source.
		/// </summary>
		public string DataSource
		{
			get
			{
				return dataSource;
			}
			set
			{
				dataSource = value;
				Database = value; 
			}
		}

		/// <summary>
		/// Gets or sets the database name
		/// </summary>
		public string Database
		{
			get
			{
				return dataBase;
			}
			set
			{
				dataBase= value;
			}
		}

		/// <summary>
		/// Gets or sets the database password. 
		/// </summary>
		public string Password
		{
			get
			{
				return password;
			}

			set
			{
				password = value;
			}
		}

		/// <summary>
		/// Gets or sets the database encryption type, or Cypher type.
		/// </summary>
		public CypherType Cypher
		{
			get
			{
				return cypher;
			}

			set
			{
				cypher = value;
			}
		}

		/// <summary>
		/// Gets or sets if a database is to be opened in exclusive mode. Required for altering the database schema.
		/// </summary>
		public bool Exclusive
		{
			get
			{
				return exclusive;
			}
			set
			{
				exclusive = value;
			}
		}

		/// <summary>
		/// Gets or sets if a database is to be opened in readonly mode.
		/// </summary>
		public bool ReadOnly
		{
			get
			{
				return readOnly;
			}
			set
			{
				readOnly = value;
			}
		}
	}
}
#endif
