using System;
using System.Text;
using System.Globalization;

namespace Provider.VistaDB
{
	/// <summary>
	/// Summary description for VistaDBRemoteQuery.
	/// </summary>
	internal class VistaDBRemoteQuery: VistaDBSQLQuery
	{
		private const byte FLD_NULL    = 0;
		private const byte FLD_NOTNULL = 1;

		private int queryID;
		private RemoteParameterCollection parameterCollection;
		private int recordSize;
		private int fetchCount;
		private int currentRow;
		private int lastRowNo, firstRowNo;
		private object[,] values;

		public VistaDBRemoteQuery(VistaDBSQLConnection parent): base(parent)
		{
			this.fetchCount          = 100;
			this.parameterCollection = new RemoteParameterCollection();
			this.queryID             = 0;//srv_Prepare();
		}

		private VistaDBRemoteConnection Parent
		{
			get
			{
				return (VistaDBRemoteConnection)this.parent;
			}
		}

		#region Remote access function
		private string[] srv_GetStructure()
		{
			string answer;
			string[] result;
			int linesCount;
			int index, endIndex;
			int len;
			
			//Send request
			Parent.SendMessage(ActionList.sc_sql_GETSTRUCTURE, queryID.ToString("X8"));

			//Receive answer
			Parent.ReceiveMessage(out answer);

			//Calculate line count
			linesCount = 1;
			index      = 0;
			while(true)
			{
				index = answer.IndexOf(Environment.NewLine, index);
				if(index >= 0)
					linesCount++;
				else
					break;
				index += Environment.NewLine.Length;
			}

			//Break line
			result = new string[linesCount];
			index  = 0;
			for(int i = 0; i < linesCount; i++)
			{
				endIndex = answer.IndexOf(Environment.NewLine, index);

				if(endIndex < 0)
					len = answer.Length - index;
				else
					len = endIndex - index;

				result[i] = answer.Substring(index, len);
				index = endIndex + Environment.NewLine.Length;
			}

			return result;
		}

		private int srv_Prepare()
		{
			int size = 0;
			byte[] buffer;
			int queryID;
			int errorCode;
			string answer;
			Encoding encoding = Encoding.Default;

			//Calc buffer size and create buffer
			size   = 255 + this.parameterCollection.GetParameters(null, 0, true);
			buffer = new byte[size];

			//Put info into the buffer
			encoding.GetBytes(Parent.Database.PadRight(255)).CopyTo(buffer, 0);
			this.parameterCollection.GetParameters(buffer, 255, false);

			//Send request
			Parent.SendMessage(ActionList.sc_sql_PREPARE, buffer);

			//Get answer
			Parent.ReceiveMessage(out answer);

			//Parse answer
			queryID   = Int32.Parse(answer.Substring(0, 8), NumberStyles.AllowHexSpecifier);
			errorCode = Int32.Parse(answer.Substring(8, 8), NumberStyles.AllowHexSpecifier);

			if(errorCode != 0)
			{
				string message = answer.Substring(16, answer.Length - 16);
				throw new VistaDBException(message, null, true, null);
			}

			return queryID;
		}

		private void srv_OpenSQL()
		{
			int errorCode;
			Encoding encoding = Encoding.Default;
			string answer;

			//Send messege
			Parent.SendMessage(ActionList.sc_sql_OPENSQL, this.queryID.ToString("X8") + this.commandText.Trim());

			//Receive answer
			Parent.ReceiveMessage(out answer);

			//Get error code
			errorCode = Int32.Parse(answer.Substring(24, 8), NumberStyles.AllowHexSpecifier);

			if(errorCode != 0)
			{
				string message = answer.Substring(32, answer.Length - 32);
				throw new VistaDBException(message, null, true, null);
			}

			this.recordCount = Int32.Parse(answer.Substring(0, 8), NumberStyles.AllowHexSpecifier);
			this.columnCount = Int32.Parse(answer.Substring(8, 8), NumberStyles.AllowHexSpecifier);
			this.recordSize  = Int32.Parse(answer.Substring(16, 8), NumberStyles.AllowHexSpecifier);
		}

		private void srv_UnPrepare(int sqlID)
		{
			byte[] buffer;

			//Send request
			Parent.SendMessage(ActionList.sc_sql_UNPREPARE, sqlID.ToString("X8"));

			//Receive answer
			Parent.ReceiveMessage(out buffer);
		}

		private byte[] srv_GetRecord(int recNo)
		{

			int bufferSize;
			int curFetchCount;
			StringBuilder buffer = new StringBuilder(8 + 8 + 8 + 8);
			byte[] answer;

			if(this.fetchCount > this.recordCount - recNo + 1)
				curFetchCount = this.recordCount - recNo + 1;
			else
				curFetchCount = this.fetchCount;

			bufferSize = (this.recordSize + 8) * curFetchCount + 8;

			//Put info into the buffer
			buffer.Insert(0, this.queryID.ToString("X8"));
			buffer.Insert(8, recNo.ToString("X8"));
			buffer.Insert(16, curFetchCount.ToString("X8"));
			buffer.Insert(24, bufferSize.ToString("X8"));

			//Send request
			Parent.SendMessage(ActionList.sc_sql_READRECORD, buffer.ToString());

			//Receive answer
			Parent.ReceiveMessage(out answer);

			return answer;
		}

		private byte[] srv_ReadBlobData(int recNo, int fieldIndex)
		{
			byte[] answer;
			StringBuilder buffer = new StringBuilder(8 + 8 + 8);

			//Put info into the buffer
			buffer.Insert(0, this.queryID.ToString("X8"));
			buffer.Insert(8, recNo.ToString("X8"));
			buffer.Insert(16, fieldIndex.ToString("X8"));

			//Send message
			Parent.SendMessage(ActionList.sc_sql_READBLOBDATA, buffer.ToString());

			//Receive answer
			Parent.ReceiveMessage(out answer);

			return answer;
		}

		private void srv_ExecSQL()
		{
			int len = this.commandText.Length;
			byte[] buffer;
			int size;
			Encoding encoding = Encoding.Default;
			string answer;
			int errorCode;

			//Create buffer
			size = 16 + len + this.parameterCollection.GetParameters(null, 0, true);
			buffer = new byte[size];

			//Put data into the buffer
			encoding.GetBytes(this.queryID.ToString("X8")).CopyTo(buffer, 0);
			encoding.GetBytes(len.ToString("X8")).CopyTo(buffer, 8);
			encoding.GetBytes(this.commandText).CopyTo(buffer, 16);
			this.parameterCollection.GetParameters(buffer, 16 + len, false);

			//Send message
			Parent.SendMessage(ActionList.sc_sql_EXECSQL, buffer);

			//Receive answer
			Parent.ReceiveMessage(out answer);

			//Check if here error occured
			errorCode = Int32.Parse(answer.Substring(0, 8), NumberStyles.AllowHexSpecifier);
			if(errorCode != 0)
			{
				string message = answer.Substring(8, answer.Length - 8);
				throw new VistaDBException(message, null, true, null);
			}
		}

		private void srv_Close()
		{
			byte[] answer;

			//Send request
			Parent.SendMessage(ActionList.sc_sql_CLOSESQL, this.queryID.ToString("X8"));

			//Receive answer
			Parent.ReceiveMessage(out answer);
		}

		#endregion Remote access function

		#region Overriden functions
		public override void CreateQuery()
		{
		}

		public override void FreeQuery()
		{
		}

		public override void Open()
		{
			if( this.queryID != 0 )
				return;

			lock(this)
			{
				this.queryID = srv_Prepare();
				srv_OpenSQL();
				InternalInitFieldDefs();
				First();
				this.opened     = true;
				this.values     = null;
				this.lastRowNo  = -1;
				this.firstRowNo = -1;
			}
		}

		public override void Close()
		{
			if(this.opened)
			{
				lock(this)
				{
					srv_Close();
					srv_UnPrepare(this.queryID);
					this.columns = null;
					this.values  = null;
					this.opened  = false;
				}
			}
			
			if(this.queryID != 0)
			{
				try
				{
					srv_UnPrepare(this.queryID);
				}
				finally
				{
					this.queryID = 0;
				}
			}
		}

		public override void ExecSQL()
		{
			if(this.opened)
				throw new InvalidOperationException("Query opened");

			lock(this)
			{
				if(this.queryID == 0)
					this.queryID = srv_Prepare();

				srv_ExecSQL();
				
				this.rowsAffected = 1;
			}
		}

		public override void SetParameter(string paramName, VistaDBType dataType, object value)
		{
			lock(this)
			{
				this.parameterCollection.SetParameter(paramName, dataType, value);
			}
		}

		public override bool ParamIsNull(string pName)
		{
			return false;
		}

		public override void SetParamNull(string pName, VistaDBType type)
		{
			lock(this)
			{
				this.parameterCollection.SetParameter(pName, type, null);
			}
		}

		public override bool First()
		{
			lock(this)
			{
				this.currentRow = 0; // set before prior position
			}

			return true;
		}

		public override bool Next()
		{
			lock(this)
			{
				if(this.currentRow < this.recordCount)
					this.currentRow++;
			}
			return true;
		}

		public override bool Eof
		{
			get
			{
				return this.currentRow == this.recordCount;
			}
		}

		public override object GetValue(int fieldNo)
		{
			lock(this)
			{
				if(FetchRow(this.currentRow))
					return this.values[this.currentRow - this.firstRowNo, fieldNo];
				else
					return null;
			}
		}

		public override bool IsNull(int columnNumber)
		{
			return GetValue(columnNumber) == null;
		}

		#endregion Overriden functions

		#region Private Methods
		private void InternalInitFieldDefs()
		{
			if( this.queryID == 0 )
				return;

			string[] buffer;
			string columnName, columnCaption;
			VistaDBType columnType;
			bool allowNull, readOnly, autoIncrement, primaryKey, unique, reservedWord;
			int dataSize, columnWidth;
			int index;

			lock(this)
			{
				this.columns = new VistaDBColumn[this.columnCount];
				buffer       = srv_GetStructure();
				index        = 0;

				for(int i = 0; i < this.columnCount; i++)
				{
					columnName    = buffer[index];
					columnType    = VistaDBAPI.NetDataType(buffer[index + 1]);
					columnWidth   = Int32.Parse(buffer[index + 2], NumberStyles.AllowHexSpecifier);
					columnCaption = buffer[index + 3];
					allowNull     = buffer[index + 4] != "Y";
					readOnly      = buffer[index + 5] == "Y";
					autoIncrement = buffer[index + 6] == "Y";
					primaryKey    = buffer[index + 7] == "Y";
					unique        = buffer[index + 8] == "Y";
					reservedWord  = buffer[index + 9] == "Y";

					dataSize      = 0; // default

					switch(columnType)
					{
						case VistaDBType.Character:
							dataSize = columnWidth;
							break;
						case VistaDBType.Varchar:
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

					this.columns[i] = new VistaDBColumn( columnName, columnType, dataSize, (short)columnWidth, 0, allowNull, readOnly, primaryKey, unique, autoIncrement, 0, "", columnCaption, "", reservedWord, false, false, false, false);

					index += 10;
				}
			}
		}

		private bool FetchRow(int rowNo)
		{
			if(rowNo < 0 || rowNo >= this.recordCount)
				return false;

			//Create data buffer
			if(this.values == null)
			{
				if(this.recordCount < this.fetchCount)
					this.values = new object[this.recordCount, this.columnCount];
				else
					this.values = new object[this.fetchCount, this.columnCount];
			}
			else if(rowNo <= this.lastRowNo && rowNo >= this.firstRowNo)
				return true;

			byte[] buffer;
			Encoding encoding = Encoding.Default;
			int rowToRead;
			int ptr;

			//Get rows from the server
			buffer   = srv_GetRecord(rowNo + 1);

			//Parse result

			//Get read row
			rowToRead = Int32.Parse(encoding.GetString(buffer, 0, 8), NumberStyles.AllowHexSpecifier);

			if(rowToRead == 0)
				throw new VistaDBException("Server send wrong row count", VistaDBErrorCodes.ServerError);

			//Parse rows
			ptr = 8;
			for(int i = 0; i < rowToRead; i++)
			{
				if(i == rowToRead - 1)
					this.lastRowNo = Int32.Parse(encoding.GetString(buffer, ptr, 8), NumberStyles.AllowHexSpecifier) - 1;
				GetRowFromRawData(i, buffer, ref ptr);
			}

			this.firstRowNo = Int32.Parse(encoding.GetString(buffer, 8, 8), NumberStyles.AllowHexSpecifier) - 1;

			return true;
		}

		private void GetRowFromRawData(int index, byte[] buffer, ref int ptr)
		{
			Encoding encoding = Encoding.Default;
			long i64;
			int rowNo;
			bool isNull;

			rowNo = Int32.Parse(encoding.GetString(buffer, ptr, 8), NumberStyles.AllowHexSpecifier);
			ptr += 8;

			for(int i = 0; i < this.columnCount; i++)
			{
				isNull = buffer[ptr] == FLD_NULL;

				if(isNull)
					this.values[index, i] = null;

				ptr++;

				switch(this.columns[i].VistaDBType)
				{
					case VistaDBType.Blob:
					case VistaDBType.Picture:
						if(!isNull)
							this.values[index, i] = srv_ReadBlobData(rowNo, i + 1);
						break;
					case VistaDBType.Memo:
						if(!isNull)
							this.values[index, i] = encoding.GetString(srv_ReadBlobData(rowNo, i + 1));
						break;
					case VistaDBType.Boolean:
						if(!isNull)
							this.values[index, i] = (buffer[ptr] != 0) || (buffer[ptr + 1] != 0);
						ptr += 2;
						break;
					case VistaDBType.Character:
					case VistaDBType.Varchar:
						if(!isNull)
							this.values[index, i] = encoding.GetString(buffer, ptr, columns[i].ColumnWidth).Trim();
						ptr += columns[i].ColumnWidth + 1;
						break;
					case VistaDBType.Currency:
						if(!isNull)
							this.values[index, i] = BcdToCurr(buffer, ptr);
						ptr += 34;
						break;
					case VistaDBType.Date:
						if(!isNull)
						{
							i64 = (long)(BitConverter.ToInt32(buffer, ptr) - 1) * VistaDBAPI.MSecsPerDay * (long)10000;
							this.values[index, i] = new DateTime(i64);
						}
						ptr += 4;
						break;
					case VistaDBType.DateTime:
						if(!isNull)
						{
							i64 = (long)(BitConverter.ToDouble(buffer, ptr) * 10000);
							this.values[index, i] = new DateTime(i64);
						}
						ptr += 8;
						break;
					case VistaDBType.Double:
						if(!isNull)
							this.values[index, i] = BitConverter.ToDouble(buffer, ptr);
						ptr += 8;
						break;
					case VistaDBType.Guid:
						if(!isNull)
							this.values[index, i] = new Guid(encoding.GetString(buffer, ptr, 38).Substring(1,36));
						ptr += 39;
						break;
					case VistaDBType.Int32:
						if(!isNull)
							this.values[index, i] = BitConverter.ToInt32(buffer, ptr);
						ptr += 4;
						break;
					case VistaDBType.Int64:
						if(!isNull)
							this.values[index, i] = BitConverter.ToInt64(buffer, ptr);
						ptr += 8;
						break;
				}
			}
		}

		private decimal BcdToCurr(byte[] buffer, int ptr)
		{
			//Convert TBcd type to decimal
			long power = 1;
			int first  = buffer[ptr] / 2;
			long i64   = 0;
			byte nibble;

			for(int i = first - 1; i >= 0; i--)
			{
				nibble = (byte)((buffer[i + ptr + 2] & 0x0F) + 10 * (buffer[i + ptr + 2] >> 4 & 0x0F));
				i64   += power * nibble;
				power  = power * 100;
			}

			if((buffer[ptr + 1] & 0x80) == 0x80)
				i64 = -i64;

			return (decimal)i64 / (decimal)10000;
		}

		#endregion Private Methods
	}
}