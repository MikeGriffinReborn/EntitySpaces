using System;
using System.Text;

namespace Provider.VistaDB
{
	/// <summary>
	/// Summary description for VistaDBLocalSQL.
	/// </summary>
	internal class VistaDBLocalQuery: VistaDBSQLQuery
	{
		private int queryID = 0;
		private bool startTraversing = true;

		public VistaDBLocalQuery(VistaDBLocalConnection p): base(p)
		{
			this.queryID = VistaDBAPI.ivsql_CreateQuery(p.ConnectionID);
		}

		public override void CreateQuery()
		{
			if( this.queryID != 0 )
				return;

			this.queryID = VistaDBAPI.ivsql_CreateQuery(((VistaDBLocalConnection)parent).ConnectionID);
		}

		public override void FreeQuery()
		{
			if( this.queryID == 0 )
				return;

			try
			{
				VistaDBAPI.ivsql_FreeQuery( this.queryID );
			}
			finally
			{
				this.queryID = 0;
			}
		}

		/// <summary>
		/// Open a V-SQL query. Open is used with SELECT statements only.
		/// </summary>
		public override void Open()
		{
			//Open SQL Query
			//Set rowsAffected
			//Fill 'columns' (meta data structure)

			if( this.queryID == 0 )
				return;

			lock(syncRoot)
			{
				this.rowsAffected = 0;

				VistaDBAPI.ivsql_SetSQL(this.queryID, this.commandText);

				VistaDBAPI.ivsql_Open(this.queryID, ref this.rowsAffected);

				InternalInitFieldDefs();

				this.recordCount = VistaDBAPI.ivsql_RecCount(this.queryID);

				this.opened = true;
				this.startTraversing = true;
			}
		}

		/// <summary>
		/// Close a V-SQL query.
		/// </summary>
		public override void Close()
		{
			//Call SQL API function for close query
			
			if( this.opened )
			{
				lock(syncRoot)
				{
					if( this.queryID == 0 )
						return;

					VistaDBAPI.ivsql_Close(this.queryID);

					this.columns = null;
					this.opened = false;
				}
			}
		}

		/// <summary>
		/// Execute a V-SQL query that does not return a result set. These include INSERT, DELETE and UPDATE.
		/// </summary>
		public override void ExecSQL()
		{
			//Execute SQL command
			if( this.queryID == 0 )
				return;

			if( this.opened )
				throw new InvalidOperationException("Query already opened");

			lock(syncRoot)
			{
				this.rowsAffected = 0;

				VistaDBAPI.ivsql_SetSQL(this.queryID, this.commandText);			
				VistaDBAPI.ivsql_ExecSQL(this.queryID, ref this.rowsAffected);

				this.startTraversing = true;
			}
		}
    
		/// <summary>
		/// Set a V-SQL parameter.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="dataType">Parameter data type.</param>
		/// <param name="value">Data value</param>
		public override void SetParameter(string paramName, VistaDBType dataType, object value)
		{
			//Set parameter value
			//Depending on 'dataType' this function call different
			//API SQL functions for this 'dataType'
			//If 'value' is null then set parameter to null value
			long tickCount;

			if( this.queryID == 0 )
				return;

			switch(dataType)
			{
				case VistaDBType.Character:
					VistaDBAPI.ivsql_SetParamString(this.queryID, paramName, (string)value);
					break;
				case VistaDBType.Date:
					tickCount = ((DateTime)value).Ticks;
					VistaDBAPI.ivsql_SetParamDate(this.queryID, paramName, (long)tickCount);
					break;
				case VistaDBType.DateTime:
					tickCount = ((DateTime)value).Ticks;
					VistaDBAPI.ivsql_SetParamDateTime(this.queryID, paramName, (long)tickCount);
					break;
				case VistaDBType.Boolean:
					VistaDBAPI.ivsql_SetParamBoolean(this.queryID, paramName, (bool)value);
					break;
				case VistaDBType.Memo:
					VistaDBAPI.ivsql_SetParamMemo(this.queryID, paramName, (string)value);
					break;
				case VistaDBType.Picture:
					VistaDBAPI.ivsql_SetParamBlob(this.queryID, paramName, (byte[])value, ((byte[])value).Length ); 
					break;
				case VistaDBType.Blob:
					VistaDBAPI.ivsql_SetParamBlob(this.queryID, paramName, (byte[])value, ((byte[])value).Length ); 
					break;
				case VistaDBType.Currency:

					decimal dVal = (decimal)value;
					long longValue;
					longValue = (long)(dVal * 10000);

					VistaDBAPI.ivsql_SetParamCurrency(this.queryID, paramName, longValue);
					break;
				case VistaDBType.Int32:
					VistaDBAPI.ivsql_SetParamInt32(this.queryID, paramName, (int)value);
					break;
				case VistaDBType.Int64:
					VistaDBAPI.ivsql_SetParamInt64(this.queryID, paramName, (long)value);
					break;
				case VistaDBType.Double:
					VistaDBAPI.ivsql_SetParamDouble(this.queryID, paramName, (double)value);
					break;
				case VistaDBType.Varchar:
					VistaDBAPI.ivsql_SetParamVarchar(this.queryID, paramName, (string)value);
					break;
				case VistaDBType.Guid:
					VistaDBAPI.ivsql_SetParamGuid(this.queryID, paramName, (Guid)value);
					break;
			}		
		}

		/// <summary>
		/// Returns True if the V-SQL parameter value is NULL.
		/// </summary>
		public override bool ParamIsNull(string pName)
		{
			return this.queryID != 0 && VistaDBAPI.ivsql_ParamIsNull(this.queryID, pName);
		}

		/// <summary>
		/// Set a V-SQL parameter value to NULL.
		/// </summary>
		public override void SetParamNull(string pName, VistaDBType type)
		{
			if( this.queryID == 0 )
				return;

			VistaDBAPI.ivsql_SetParamNull(queryID, pName, (short)type);
		}

		//////////////////////////////////////////////////////////
		///////////////Navigation functions///////////////////////
		//////////////////////////////////////////////////////////

		/// <summary>
		/// Go to the first row in the dataset.
		/// </summary>
		/// <returns>False if current position doesn't change</returns>
		public override bool First()
		{
			if( this.queryID == 0 )
				return false;

			//Call SQL API function to move to the first position
			VistaDBAPI.ivsql_First(this.queryID);

			return true;
		}

		/// <summary>
		/// Go to the next row in dataset
		/// </summary>
		/// <returns>False if current position doesn't change</returns>
		public override bool Next()
		{
			if( this.queryID == 0 )
				return false;

			if( this.startTraversing )
			{
				this.startTraversing = !First();
				return this.startTraversing;
			}

			//Call SQL API function to move to the next position
			
			VistaDBAPI.ivsql_Next(this.queryID);

			return true;
		}

		/// <summary>
		/// End of file. Tests if a row movement function has placed the row pointer beyond the last row in the dataset.
		/// </summary>
		public override bool Eof
		{
			get
			{
				return this.queryID == 0 || VistaDBAPI.ivsql_Eof(this.queryID) != 0;
			}
		}

		/// <summary>
		/// Returns the value of the column at the given position in the table schema. The first column is 1.
		/// </summary>
		public override object GetValue(int fieldNo)
		{
			DateTime		dt;
			int				dataLen;
			StringBuilder	tmpstr;

			if( this.queryID == 0 )
				return null;

			object res = null;

			if( ! IsNull(fieldNo) )
			{
				switch(columns[fieldNo].VistaDBType)
				{
					case VistaDBType.Character:
						dataLen = columns[fieldNo].DataSize;
						tmpstr	= new StringBuilder( dataLen );
						dataLen = VistaDBAPI.ivsql_GetString(this.queryID, fieldNo + 1, tmpstr, dataLen);
						tmpstr.Length = dataLen;
						res = tmpstr.ToString();
						break;
					case VistaDBType.Date:
						long longdate = VistaDBAPI.ivsql_GetDate(this.queryID, fieldNo + 1);
						dt = new DateTime(longdate);
						res = dt;
						break;
					case VistaDBType.DateTime:
						long longdatetime = VistaDBAPI.ivsql_GetDateTime(this.queryID, fieldNo + 1);

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
						res = VistaDBAPI.ivsql_GetBoolean(this.queryID, fieldNo + 1);
						break;
					case VistaDBType.Int32:
						res = VistaDBAPI.ivsql_GetInt32(this.queryID, fieldNo + 1);
						break;
					case VistaDBType.Int64:
						res = VistaDBAPI.ivsql_GetInt64(this.queryID, fieldNo + 1);
						break;
					case VistaDBType.Currency:
						long longValue;		
						longValue = VistaDBAPI.ivsql_GetCurrency(this.queryID, fieldNo + 1);
						res = (decimal)(longValue) / (decimal)10000;
						break;
					case VistaDBType.Double:
						res = VistaDBAPI.ivsql_GetDouble(this.queryID, fieldNo + 1);
						break;
					case VistaDBType.Memo:
						dataLen	= VistaDBAPI.ivsql_GetBlobLength(this.queryID, fieldNo + 1);
						tmpstr	= new StringBuilder(dataLen);
						dataLen = VistaDBAPI.ivsql_GetMemo(this.queryID, fieldNo + 1, tmpstr, dataLen);

						tmpstr.Length = dataLen;
						res = tmpstr.ToString();
						break;
					case VistaDBType.Picture:
					case VistaDBType.Blob:
						int blobLen = 0;
						byte[] blobContent;
						blobContent = null;
						blobLen = VistaDBAPI.ivsql_GetBlobLength(this.queryID, fieldNo + 1);
						blobContent = new byte[blobLen];
						VistaDBAPI.ivsql_GetBlob(this.queryID, fieldNo + 1, blobContent, blobLen);
						res = blobContent;
						break;
					case VistaDBType.Varchar:
						dataLen = columns[fieldNo].DataSize;
						tmpstr	= new StringBuilder(dataLen);
						dataLen = VistaDBAPI.ivsql_GetVarchar(this.queryID, fieldNo + 1, tmpstr, dataLen);

						tmpstr.Length = dataLen;
						res = tmpstr.ToString();
						break;
					case VistaDBType.Guid:
						res = VistaDBAPI.ivsql_GetGuid(this.queryID, fieldNo + 1);
						break;
				}
			}

			return res;
		}

		/// <summary>
		/// Return True if a column value is NULL at the given position in the table schema. The first column is 1.
		/// </summary>
		public override bool IsNull(int columnNumber)
		{
			return this.queryID == 0 || VistaDBAPI.ivsql_IsNull(this.queryID, columnNumber + 1 );
		}

		private int InternalInitFieldDefs()
		{
			if( this.queryID == 0 )
				return 0;

			lock(syncRoot)
			{
				this.recordCount = VistaDBAPI.ivsql_RecCount(this.queryID);
				this.columnCount = VistaDBAPI.ivsql_ColumnCount(this.queryID);
				this.columns = new VistaDBColumn[this.columnCount];

				int columnCaptionWidth = 128;
				StringBuilder columnCaption = new StringBuilder(columnCaptionWidth);

				int columnNameWidth = 128;
				StringBuilder columnName = new StringBuilder(columnNameWidth);

				VistaDBType columntype;
				bool allowNull, readOnly, autoIncrement, primaryKey, unique, reservedWord;
				int dataSize, columnWidth;

				for( int i = 1; i <= columnCount; i ++ )
				{

					int len = VistaDBAPI.ivsql_ColumnName(this.queryID, i, columnName, columnNameWidth );
					columnName.Length = len;

					len = VistaDBAPI.ivsql_ColumnCaption(this.queryID, i, columnCaption, columnCaptionWidth);
					columnCaption.Length = len;

					columntype    = VistaDBAPI.NetDataType( VistaDBAPI.ivsql_ColumnType(this.queryID, i).ToString() );

					dataSize      = 0; // default
					columnWidth   = 0; // default

					allowNull     = !VistaDBAPI.ivsql_ColumnRequired(this.queryID, i);
					readOnly      = VistaDBAPI.ivsql_ColumnReadOnly(this.queryID, i);
					autoIncrement = VistaDBAPI.ivsql_ColumnIsIdentity(this.queryID, i);
					primaryKey    = VistaDBAPI.ivsql_ColumnIsPrimaryKey(this.queryID, i);
					unique        = VistaDBAPI.ivsql_ColumnIsUnique(this.queryID, i);
					reservedWord  = VistaDBAPI.ivsql_IsReservedWord(columnName.ToString());

					switch(columntype)
					{
						case VistaDBType.Character:
							columnWidth = VistaDBAPI.ivsql_ColumnWidth(this.queryID, i);
							dataSize = columnWidth;
							break;
						case VistaDBType.Varchar:
							columnWidth = VistaDBAPI.ivsql_ColumnWidth(this.queryID, i);
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

					this.columns[i - 1] = new VistaDBColumn( columnName.ToString(), columntype, dataSize, 
						(short)columnWidth, 0, allowNull, readOnly, primaryKey, unique, autoIncrement, 0, "", 
						columnCaption.ToString(), "", reservedWord, false, false, false, false);
				}
			}

			return errorNumber;
		}
	}
}