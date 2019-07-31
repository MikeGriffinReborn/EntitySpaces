using System;
using System.Text;
using System.Net.Sockets;
using System.Globalization;

namespace Provider.VistaDB
{
	internal class ActionList
	{
		public const string sc_Logout               = "0465";
		public const string sc_Login                = "0464";
		public const string sc_sql_PREPARE          = "0800";
		public const string sc_sql_UNPREPARE        = "0801";
		public const string sc_sql_OPENSQL          = "0802";
		public const string sc_sql_CLOSESQL         = "0804";
		public const string sc_sql_GETSTRUCTURE     = "0806";
		public const string sc_sql_READRECORD       = "0807";
		public const string sc_sql_READBLOBDATA     = "0808";
		public const string sc_sql_EXECSQL          = "0803";

		public const string sc_SRV_GETLOGFILE       = "0900";
		public const string sc_SRV_UPDATEALIASLIST  = "0901";
		public const string sc_SRV_UPDATEUSERLIST   = "0902";
		public const string sc_SRV_GETACTIVEUSERLIST= "0903";
		public const string sc_SRV_ISADMINCONNECTION= "0904";
		public const string sc_SRV_ENUMTABLES       = "0905";
		public const string sc_SRV_GETALIASLIST     = "0906";
	}

	internal class VistaDBRemoteConnection: VistaDBSQLConnection, IVistaDBRemoteService
	{
		#region Fields
		private NetworkStream stream;
		private TcpClient client;
		private int timeout;
		private string serial;
		private bool secureConnection;
		private string user;
		private string loginPassword;
		#endregion Fields

		#region Constructor
		public VistaDBRemoteConnection(): base()
		{
			this.stream              = null;
			this.client              = null;
			this.timeout             = 10000;
			this.serial              = "0000";
			this.secureConnection    = false;
			this.user                = "";
			this.cultureID           = 0;
			this.clusterSize         = 0;
			this.caseSensitivity     = false;
			this.databaseDescription = null;
			this.loginPassword       = "";
		}
		#endregion Constructor

		#region Server access methods

		private void Connect()
		{
			if(this.opened)
				return;

			string host;
			int port;
			int index;

			index = this.dataSource.IndexOf(":");
			if(index >= 0)
			{
				host = this.dataSource.Substring(0, index).Trim();
				port = Int32.Parse(this.dataSource.Substring(index + 1, this.dataSource.Length - index - 1));
			}
			else
			{
				host = this.dataSource.Trim();
				port = 20211;
			}

			this.client = new TcpClient(host, port);
			this.client.ReceiveTimeout = this.timeout;

			this.stream = this.client.GetStream();
		}

		private void Disconnect()
		{
			this.stream.Close();
			this.stream = null;
			this.client.Close();
			this.client = null;
		}

		private string GetMachineID()
		{
			return " ".PadRight(255);
		}

		private void SendFromBuffer(byte[] buffer)
		{
			int      messagePosition = 0;
			int      messageLen;
			byte     needEncryption;
			Encoding encoding        = Encoding.Default;
			int      encLen;
			byte[]   parsedEncLen;

			//Check if connect not logged then need to send secureConnection
			if(!this.opened)
			{
				messagePosition++;
				needEncryption = secureConnection ? (byte)'Y': (byte)'N';
			}
			else
				needEncryption = 0;
				
			//Get length of message if it will be encoded
			messagePosition += 8;
			messageLen = messagePosition + Encode(buffer, 0, true);

			//If this required then get new buffer with wided length
			if(messageLen != buffer.Length)
			{
				byte[] newBuffer = new byte[messageLen];

				buffer.CopyTo(newBuffer, messagePosition);
				buffer = newBuffer;
			}

			//Encode message
			Encode(buffer, messagePosition, false);			

			//Put total message len
			encLen = messageLen - messagePosition;
			parsedEncLen = encoding.GetBytes(encLen.ToString("X8"));
			parsedEncLen.CopyTo(buffer, 0);

			//Put needEncryption if this required
			if(needEncryption != 0)
				buffer[messagePosition - 1] = needEncryption;

			//Send message
			stream.Write(buffer, 0, buffer.Length);
		}

		private byte[] ReceiveToBuffer()
		{
			byte[] buffer;
			byte[] result;
			int len, readLen;
			Encoding encoding = Encoding.Default;

			//Read message length
			buffer = new byte[8];
			stream.Read(buffer, 0, buffer.Length);
			len = Int32.Parse(encoding.GetString(buffer, 0, 8), NumberStyles.AllowHexSpecifier);

			//Read message body
			buffer  = new byte[len];
			readLen = 0;
			while(readLen < len)
				readLen += stream.Read(buffer, readLen, buffer.Length - readLen);

			//Decode message
			Decode(buffer);

			//Get real message length
			len = Int32.Parse(encoding.GetString(buffer, 0, 8), NumberStyles.AllowHexSpecifier) - 12;
			result = new byte[len];
			Array.Copy(buffer, 20, result, 0, len);

			return result;
		}

		internal void SendMessage(string action, string packet)
		{
			int pos;
			Encoding encoding = Encoding.Default;
			//Getting message len
			int len = this.serial.Length + action.Length + packet.Length + 4;
			//Getting buffer for message + space for message length
			StringBuilder buffer = new StringBuilder(len + 8);

			//Put message length to buffer
			buffer.Insert(0, len.ToString("X8"));
			//Put message body to buffer
			buffer.Insert(8, "0000");
			buffer.Insert(12, this.serial);
			pos = 12 + this.serial.Length;
			buffer.Insert(pos, action);
			pos += action.Length;
			buffer.Insert(pos, packet);

			//Send message to server
			SendFromBuffer(encoding.GetBytes(buffer.ToString()));
		}

		public void SendMessage(string action, byte[] packet)
		{
			int pos;
			Encoding encoding = Encoding.Default;
			//Getting message len
			int len = this.serial.Length + action.Length + packet.Length + 4;
			//Getting buffer for message + space for message length
			StringBuilder buffer = new StringBuilder(len - packet.Length + 8);
			byte[] message;

			//Put message length to buffer
			buffer.Insert(0, len.ToString("X8"));
			//Put message header to the buffer
			buffer.Insert(8, "0000");
			buffer.Insert(12, this.serial);
			pos = 12 + this.serial.Length;
			buffer.Insert(pos, action);
			//Put message body to the buffer
			message = new byte[buffer.Capacity + packet.Length];
			encoding.GetBytes(buffer.ToString()).CopyTo(message, 0);
			packet.CopyTo(message, buffer.Capacity);

			//Send message to the server
			SendFromBuffer(message);
		}

		public void ReceiveMessage(out byte[] buffer)
		{
			buffer = ReceiveToBuffer();
		}

		public void ReceiveMessage(out string message)
		{
			byte[] buffer     = ReceiveToBuffer();
			Encoding encoding = Encoding.Default;

			message = encoding.GetString(buffer);
		}

		private void Login()
		{
			if(this.opened || this.client == null)
				return;

			StringBuilder message = new StringBuilder(255 + 255 + 255 + 255 + 1 + 1);
			string answer;

			//Prepare message to request login
			message.Insert(0, this.loginPassword.PadRight(255));
			message.Insert(255, this.user.PadRight(255));
			message.Insert(510, GetMachineID());
			message.Insert(765, this.database.PadRight(255));
			message.Insert(1020, this.exclusive ? 'Y': 'N');
			message.Insert(1021, this.readOnly ? 'Y': 'N');

			//Send message
			SendMessage(ActionList.sc_Login, message.ToString());

			//Receive answer
			ReceiveMessage(out answer);

			if(answer[0] == 'Y')
			{
				//Parse answer
				this.opened              = true;
				this.cultureID           = Int32.Parse(answer.Substring(1, 4), NumberStyles.AllowHexSpecifier);
				this.clusterSize         = Int32.Parse(answer.Substring(5, 4), NumberStyles.AllowHexSpecifier);
				this.caseSensitivity     = answer[9] == 'Y';
				this.databaseDescription = answer.Substring(10, answer.Length - 10);
			}
			else
			{
				Logout();
				string error = answer.Substring(1, answer.Length - 1);
				throw new VistaDBException(error, null, true, null);
			}
		}

		private void Logout()
		{
			if(!this.opened)
				return;

			byte[] answer;

			//Send message
			SendMessage(ActionList.sc_Logout, "");

			//Receive answer
			ReceiveMessage(out answer);

			this.opened = false;
		}

		#endregion Server access methods

		#region Crypting methods

		private int Encode(byte[] buffer, int position, bool getLength)
		{
			return buffer.Length;
		}

		private void Decode(byte[] buffer)
		{
		}

		#endregion Crypting methods
	
		#region Overriden methods

		public override void Dispose()
		{
			CloseDatabaseConnection();
		}


		public override void OpenDatabaseConnection()
		{
			if(this.opened)
				return;

			//Connect to server
			Connect();

			try
			{
				//Login to server
				Login();
			}
			catch
			{
				Disconnect();
				throw;
			}
		}

		public override void CloseDatabaseConnection()
		{
			if(!this.opened)
				return;

			base.CloseDatabaseConnection();

			lock(this.syncRoot)
			{
				//Logout
				Logout();
				//Disconnect from server
				Disconnect();
			}
		}

		protected override VistaDBSQLQuery CreateSQLQuery()
		{
			return new VistaDBRemoteQuery(this);
		}

		/// <summary>
		/// Begin a transaction. Transactions may be nested.
		/// </summary>
		public override bool BeginTransaction()
		{
			VistaDBSQLQuery query = CreateSQLQuery();
			query.SQL             = "BEGIN TRANSACTION";
			query.ExecSQL();
			query.DropQuery();
			return true;
		}

		/// <summary>
		/// Commit an active transaction. Transactions may be nested.
		/// </summary>
		public override bool CommitTransaction()
		{
			VistaDBSQLQuery query = CreateSQLQuery();
			query.SQL             = "COMMIT TRANSACTION";
			query.ExecSQL();
			query.DropQuery();
			return true;
		}

		/// <summary>
		/// Rollback the active transaction. Transactions may be nested. 
		/// </summary>
		public override void RollbackTransaction()
		{
			VistaDBSQLQuery query = CreateSQLQuery();
			query.SQL             = "ROLLBACK TRANSACTION";
			query.ExecSQL();
			query.DropQuery();
		}

		#endregion Overriden methods

		#region Overriden properties
		
		/// <summary>
		/// Gets or sets the data source.
		/// </summary>
		public override string DataSource
		{
			set
			{
				if(this.opened)
					throw new VistaDBException(VistaDBErrorCodes.PropertyCannotBeChanged);
				
				this.dataSource = value;
			}
		}

		#endregion Overriden properties

		#region Properties

		public int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if(this.opened)
					throw new VistaDBException(VistaDBErrorCodes.PropertyCannotBeChanged);

				this.timeout = value;
			}
		}

		public bool SecureConnection
		{
			get
			{
				return this.secureConnection;
			}
			set
			{
				if(this.opened)
					throw new VistaDBException(VistaDBErrorCodes.PropertyCannotBeChanged);

				this.secureConnection = value;
			}
		}

		public string User
		{
			get
			{
				return this.user;
			}
			set
			{
				if(this.opened)
					throw new VistaDBException(VistaDBErrorCodes.PropertyCannotBeChanged);

				this.user = value;
			}
		}

		public string LoginPassword
		{
			get
			{
				return this.loginPassword;
			}
			set
			{
				this.loginPassword  = value;
			}
		}
		
		#endregion Properties

		#region Server service methods

		public string GetLogFile()
		{
			if(!this.opened)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionNotOpened);

			string answer;
			int errorCode;
			string message;

			//Send request
			SendMessage(ActionList.sc_SRV_GETLOGFILE, "");

			//Receive answer
			ReceiveMessage(out answer);

			//Get error code
			errorCode = Int32.Parse(answer.Substring(0, 4), NumberStyles.AllowHexSpecifier);

			//Get message (error message or log file body)
			message   = answer.Substring(4, answer.Length - 4);

			//Check if here occurred error then raise exception
			if(errorCode != 0)
				throw new VistaDBException(message, VistaDBErrorCodes.ServerError);

			return message;
		}

		public void UpdateAliasList()
		{
			if(!this.opened)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionNotOpened);

			string answer;
			int errorCode;

			//Send request
			SendMessage(ActionList.sc_SRV_UPDATEUSERLIST, "");

			//Receive answer
			ReceiveMessage(out answer);

			//Get error code
			errorCode = Int32.Parse(answer.Substring(0, 4), NumberStyles.AllowHexSpecifier);

			//Check if here error occured then raise exception
			if(errorCode != 0)
			{
				string message = answer.Substring(4, answer.Length - 4);
				throw new VistaDBException(message, VistaDBErrorCodes.ServerError);
			}
		}

		public void UpdateUserList()
		{
			if(!this.opened)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionNotOpened);

			string answer;
			int errorCode;

			//Send request
			SendMessage(ActionList.sc_SRV_UPDATEUSERLIST, "");

			//Receive answer
			ReceiveMessage(out answer);

			//Get error code
			errorCode = Int32.Parse(answer.Substring(0, 4), NumberStyles.AllowHexSpecifier);

			//Check if here error occured then raise exception
			if(errorCode != 0)
			{
				string message = answer.Substring(4, answer.Length - 4);
				throw new VistaDBException(message, VistaDBErrorCodes.ServerError);
			}
		}

		public string[] GetActiveUserList()
		{
			if(!this.opened)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionNotOpened);

			string answer;
			string[] result;
			int errorCode;
			int index, endIndex;
			int usersCount;
			int len;

			//Send request
			SendMessage(ActionList.sc_SRV_GETACTIVEUSERLIST, "");

			//Receive answer
			ReceiveMessage(out answer);

			//Get error code
			errorCode = Int32.Parse(answer.Substring(0, 4), NumberStyles.AllowHexSpecifier);

			//Check if here occurred error then raise exception
			if(errorCode != 0)
			{
				string message = answer.Substring(4, answer.Length - 4);
				throw new VistaDBException(message, VistaDBErrorCodes.ServerError);
			}

			//If there are no any users then return null
			if(answer.Length == 4)
				return null;

			//Calculate users count
			usersCount = 1;
			index      = 4;
			while(true)
			{
				index = answer.IndexOf(Environment.NewLine, index);
				if(index >= 0)
					usersCount++;
				else
					break;
				index += Environment.NewLine.Length;
			}

			//Break line
			result = new string[usersCount];
			index  = 4;
			for(int i = 0; i < usersCount; i++)
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

		public bool IsAdminConnection()
		{
			if(!this.opened)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionNotOpened);

			string answer;
			int errorCode;
			string message;

			//Send request
			SendMessage(ActionList.sc_SRV_ISADMINCONNECTION, "");

			//Receive answer
			ReceiveMessage(out answer);

			//Get error code
			errorCode = Int32.Parse(answer.Substring(0, 4), NumberStyles.AllowHexSpecifier);

			//Get message (error message or log file body)
			message   = answer.Substring(4, answer.Length - 4);

			//Check if here occurred error then raise exception
			if(errorCode != 0)
				throw new VistaDBException(message, VistaDBErrorCodes.ServerError);

			return message[0] == 'Y';
		}

		public VistaDBMetaDataReader EnumTables()
		{
			if(!this.opened)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionNotOpened);

			int errorCode;
			byte[] answer;
			byte[] buffer;
			VistaDBMetaDataReader metaDataReader;
			ASCIIEncoding encoding = new ASCIIEncoding();

			//Send request
			SendMessage(ActionList.sc_SRV_ENUMTABLES, this.database);

			//Receive answer
			ReceiveMessage(out answer);
		
			//Get error code
			errorCode = Int32.Parse(encoding.GetString(answer, 0, 4), NumberStyles.AllowHexSpecifier);

			//Check if here occurred error then raise exception
			if(errorCode != 0)
			{
				//Get error message
				string message         = encoding.GetString(answer, 4, answer.Length - 4);
				throw new VistaDBException(message, VistaDBErrorCodes.ServerError);
			}

			//Prepare meta data reader
			buffer = new byte[answer.Length - 4];
			Array.Copy(answer, 4, buffer, 0, buffer.Length);
			metaDataReader = new VistaDBMetaDataReader(buffer);

			return metaDataReader;
		}

		public string[] GetAliasList()
		{
			if(!this.opened)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionNotOpened);

			bool needClose;
			string answer;
			string[] aliases;
			int index, endIndex;
			int len, aliasesCount;
			int errorCode;

			if(!this.opened)
			{
				Connect();
				needClose = true;
			}
			else
				needClose = false;

			try
			{
				//Send request
				SendMessage(ActionList.sc_SRV_GETALIASLIST, "");

				//Receive answer
				ReceiveMessage(out answer);

				//Get error code
				errorCode = Int32.Parse(answer.Substring(0, 4), NumberStyles.AllowHexSpecifier);


				//Check if here occurred error then raise exception
				if(errorCode != 0)
				{
					string message   = answer.Substring(4, answer.Length - 4);
					throw new VistaDBException(message, VistaDBErrorCodes.ServerError);
				}

				//Calculate aliases count
				aliasesCount = 0;
				index        = 4;
				while(true)
				{
					index = answer.IndexOf(Environment.NewLine, index);
					if(index >= 0)
						aliasesCount++;
					else
						break;
					index += Environment.NewLine.Length;
				}

				//Break line
				aliases = new string[aliasesCount];
				index  = 4;
				for(int i = 0; i < aliasesCount; i++)
				{
					endIndex = answer.IndexOf(Environment.NewLine, index);

					if(endIndex < 0)
						len = answer.Length - index;
					else
						len = endIndex - index;						

					aliases[i] = answer.Substring(index, len);
					index = endIndex + Environment.NewLine.Length;
				}
			}
			finally
			{
				if(needClose)
					Disconnect();
			}

			return aliases;
		}

		#endregion Server service methods
	}
}