using System;
using System.Data;
using Provider.VistaDB;
using System.ComponentModel;

namespace Provider.VistaDB
{
	/// <summary>
	/// Connection object.
	/// </summary>
#if EVAL
	[LicenseProviderAttribute(typeof(VistaDBLicenseProvider))]
#endif
	public class VistaDBConnection: Component, IDbConnection
	{
		ConnectionState connectionState;
		VistaDBSQLConnection vistaDBSQL;
		AccessMode accessMode;
#if EVAL
		private License license = null;
#endif
		private int connectionTimeOut = 0;
		private string dataSource;
		private string database;
		private CypherType cypher;
		private string password;
		private bool exclusive;
		private bool readOnly;
		private string loginUser;
		private string loginPassword;

		/// <summary>
		/// Constructor.
		/// </summary>
		public VistaDBConnection()
		{
			VistaDBErrorMsgs.SetErrorFunc();
			this.connectionState = ConnectionState.Closed;
			this.accessMode      = AccessMode.Local;
			this.vistaDBSQL      = null;

			this.dataSource      = null;
			this.database        = null;
			this.cypher          = CypherType.None;
			this.password        = null;
			this.exclusive       = false;
			this.readOnly        = false;
			this.loginUser       = "";
			this.loginPassword   = "";
		}

		/// <summary>
		/// Constructor with a connection string.
		/// </summary>
		public VistaDBConnection(string sConnString): base()
		{
			ConnectionString = sConnString;
		}

		internal VistaDBConnection(int cultureID, bool caseSensitive): this()
		{
			this.vistaDBSQL      = new VistaDBLocalConnection(cultureID, caseSensitive);
			this.exclusive       = true;
			this.connectionState = ConnectionState.Open;
		}

		internal VistaDBConnection(VistaDBDatabase db): this()
		{
			if(db.Parameters != VDBDatabaseParam.InMemory)
				throw new VistaDBException(VistaDBErrorCodes.DatabaseMustBeTemporary);

			this.vistaDBSQL      = new VistaDBLocalConnection(db);
			this.exclusive       = true;
			this.connectionState = ConnectionState.Open;
		}

		/// <summary>
		/// VistaDBConnection destructor
		/// </summary>
		~VistaDBConnection()
		{
			Dispose(false);
		}

		/// <summary>
		/// Overloaded. Releases the resources used by the component.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
#if EVAL
			if (license != null) 
			{
				license.Dispose();
				license = null;
			}
#endif
			try
			{
				Close();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		/// <summary>
		/// Returns remote service object
		/// </summary>
		/// <returns>Remote service object</returns>
		public IVistaDBRemoteService GetRemoteService()
		{
			if(this.connectionState != ConnectionState.Open || !(this.vistaDBSQL is VistaDBRemoteConnection))
				return null;

			return (IVistaDBRemoteService)this.vistaDBSQL;
		}

		private bool GetValueForProp(string expr, string propName, out string value)
		{
			int len = propName.Length;
			int limit = expr.Length - len;
			propName = propName.ToUpper();
			int i, eqNo, previous;
			string s;
			string curPropName;

			value = "";

			//Find property position
			previous = 0;
			i = expr.IndexOf(';', 0);

			while( (i >= 0 && i <= limit) || (previous <= limit) )
			{
				if (i < 0)
					i = expr.Length;

				s = expr.Substring(previous, i - previous).TrimStart();

				eqNo = s.IndexOf('=');

				if (eqNo < 0)
					curPropName = "";
				else
					curPropName = s.Substring(0, eqNo).Trim().ToUpper();

				if( curPropName == propName )
				{
					value = s.Substring(eqNo + 1, s.Length - eqNo - 1).TrimStart();
					return true;
				}

				previous = i + 1;

				if ( i < expr.Length )
					i = expr.IndexOf(';', i + 1);
			}

			return false;

		}

		/// <summary>
		/// Gets the current state of the connection.
		/// </summary>
		public ConnectionState State	//IDbConnection.State
		{
			get
			{
				return connectionState;
			}
		}

		IDbTransaction IDbConnection.BeginTransaction ()
		{
			return BeginTransaction ();
		}

		/// <summary>
		/// Overloaded. Begins a database transaction. Transactions may be nested.
		/// </summary>
		/// <remarks>
		/// VistaDB always uses snapshot isolation, where each new transaction receives 
		/// its own view of the database at the moment the transaction is started. 
		/// Uncommitted and committed changes by other connections and transactions are 
		/// not seen until this transaction is committed or rolled back.
		/// </remarks>
		/// <returns>Transaction ID</returns>
		public VistaDBTransaction BeginTransaction() 
		{
			return new VistaDBTransaction(this);
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public IDbTransaction BeginTransaction(IsolationLevel level) 
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		public void ChangeDatabase(string dbName)//IDbConnection.ChangeDatabase
		{
			//		' Change the database setting on the back-end. Note that it is a method
			//		' and not a property because the operation requires an expensive
			//		' round trip.
		}

		/// <summary>
		/// Open the connection.
		/// </summary>
		public void Open()//IDbConnection.Open
		{
#if EVAL
			if(license == null)
				license = LicenseManager.Validate(typeof(VistaDBConnection), this);
#endif

			if(this.connectionState == ConnectionState.Open)
				return;

			if (DataSource=="")
				throw new VistaDBException(VistaDBErrorCodes.ConnectionDataSourceIsEmpty);

			switch(this.accessMode)
			{
				case AccessMode.Local:
					this.vistaDBSQL = new VistaDBLocalConnection();
					break;
				case AccessMode.Remote:
					this.vistaDBSQL = new VistaDBRemoteConnection();
					((VistaDBRemoteConnection)this.vistaDBSQL).User     = this.loginUser;
					((VistaDBRemoteConnection)this.vistaDBSQL).LoginPassword = this.loginPassword;
					((VistaDBRemoteConnection)this.vistaDBSQL).Timeout  = this.connectionTimeOut;
					break;
				default:
					throw new VistaDBException(VistaDBErrorCodes.UnusableAccessMode);
			}

			this.vistaDBSQL.DataSource = this.dataSource;
			this.vistaDBSQL.Database   = this.database;
			this.vistaDBSQL.Cypher     = this.cypher;
			this.vistaDBSQL.Password   = this.password;
			this.vistaDBSQL.Exclusive  = this.exclusive;
			this.vistaDBSQL.ReadOnly   = this.readOnly;

			vistaDBSQL.OpenDatabaseConnection();

			connectionState = ConnectionState.Open;
		}

		/// <summary>
		/// Close the connection.
		/// </summary>
		public void Close()//IDbConnection.Close
		{
			if(this.connectionState == ConnectionState.Open)
			{
				try
				{
					this.vistaDBSQL.Dispose();
				}
				finally
				{
					this.vistaDBSQL      = null;
					this.connectionState = ConnectionState.Closed;
				}
			}
		}

		IDbCommand IDbConnection.CreateCommand()//IDbConnection.CreateCommand
		{
			return this.CreateCommand();
		}

		/// <summary>
		/// Creates and returns a VistaDBCommand object associated with the VistaDBConnection.
		/// </summary>
		public VistaDBCommand CreateCommand()
		{
			//' Return a new instance of a command object.
			VistaDBCommand comm = new VistaDBCommand();
			comm.Connection     = this;

			return comm;
		}

		/// <summary>
		/// Creates temporary database and connects to it
		/// </summary>
		/// <param name="cultureID">Database culture ID</param>
		/// <param name="caseSensitive">True if database is case sensitive</param>
		/// <returns>VistaDBConnection object</returns>
		public static VistaDBConnection CreateTemporaryDatabase(int cultureID, bool caseSensitive)
		{
			return new VistaDBConnection(cultureID, caseSensitive);
		}

		/// <summary>
		/// Connects to temporary database.
		/// After connection created VistaDBDatabase object lose control to database.
		/// </summary>
		/// <param name="db">VistaDBDatabase object</param>
		/// <returns>VistaDBConnection object</returns>
		public static VistaDBConnection CreateTemporaryDatabase(VistaDBDatabase db)
		{
			return new VistaDBConnection(db);
		}

		internal VistaDBSQLConnection VistaDBSQL
		{
			get
			{
				return vistaDBSQL;
			}
		}

		/// <summary>
		/// Returns the connection time-out value set in the connection
		///	string. Zero indicates an indefinite time-out period.
		/// </summary>
		int IDbConnection.ConnectionTimeout
		{
			get
			{
				return connectionTimeOut;
			}
		}

		/// <summary>
		/// Gets or sets the connection string. 
		/// </summary>
		/// <remarks>
		/// The ConnectionString is similar to an OLE DB connection string, but is not identical. Unlike OLE DB or ADO, 
		/// the connection string that is returned is the same as the user-set ConnectionString.
		/// You can use the ConnectionString property to connect to a database.
		/// </remarks>
		[Browsable(true), Editor("VistaDB.Designer.VistaDBDatabasePathEditor, VistaDB.Designer.VS2003", "System.Drawing.Design.UITypeEditor")]
		public string ConnectionString //IDbConnection.ConnectionString
		{
			get
			{
				string tmp = "";

				tmp += "AccessMode=" + this.accessMode.ToString() + ";";

				if (this.dataSource != "")
					tmp += "DataSource=" + this.dataSource + ";";

				if (this.database != "")
					tmp += "Database=" + this.database + ";";

				if(this.loginUser != "")
					tmp += "LoginUser=" + this.loginUser + ";";

				if(this.loginPassword != "")
					tmp += "LoginPassword=" + this.loginPassword + ";";

				tmp += "Cypher=" + this.cypher.ToString() + ";";

				if (this.password != "")
					tmp += "Password=" + this.password + ";";

				tmp += "Exclusive=" + this.exclusive.ToString() + ";";

				tmp += "ReadOnly=" + this.readOnly.ToString();

				return tmp; 
			}

			set
			{
				if (connectionState == ConnectionState.Open)
					throw new VistaDBException(VistaDBErrorCodes.ConnectionCannotBeChanged);

				string s;

				value = value.TrimStart(' ');

				if (GetValueForProp(value, "AccessMode", out s))
				{
					s = s.ToUpper();

					switch(s)
					{
						case "LOCAL":
							this.accessMode = AccessMode.Local;
							break;
						case "REMOTE":
							this.accessMode = AccessMode.Remote;
							break;
					}
				}
				else
				{
					this.accessMode = AccessMode.Local;
				}

				if (GetValueForProp(value, "DataSource", out s))
				{
					this.dataSource = s;
				}
				else
				{
					this.dataSource = string.Empty;
				}

				if( GetValueForProp(value, "Database", out s) )
				{
					this.database = s;
				}
				else
				{
					this.database = this.dataSource; 
				}

				//Find login user
				if( GetValueForProp(value, "LoginUser", out s) )
				{
					this.loginUser = s;
				}
				else
				{
					this.loginUser = "";
				}

				//Find login password
				if( GetValueForProp(value, "LoginPassword", out s) )
				{
					this.loginPassword = s;
				}
				else
				{
					this.loginPassword = "";
				}


				//Find Cypher
				if( GetValueForProp(value, "Cypher", out s) )
				{
					s = s.ToUpper();

					switch( s )
					{
						case "NONE":
							this.cypher = CypherType.None;
							break;
						case "BLOWFISH":
							this.cypher = CypherType.Blowfish;
							break;
						case "DES":
							this.cypher = CypherType.DES;
							break;
					}
				}
				else
					this.cypher = CypherType.None;

				//Find Password
				if( GetValueForProp(value, "Password", out s) )
				{
					this.password = s;
				}
				else
					this.password = string.Empty;


				//Find Exclusive
				if( GetValueForProp(value, "Exclusive", out s) )
				{
					this.exclusive = Convert.ToBoolean(s);
				}
				else
					this.exclusive = false;


				//Find ReadOnly
				if( GetValueForProp(value, "ReadOnly", out s) )
				{
					this.readOnly = Convert.ToBoolean(s);
				}
				else
					this.readOnly = false;
			}
		}
			
		/// <summary>
		/// Returns the connection time-out value set in the connection
		///	string. Zero indicates an indefinite time-out period.
		/// </summary>
		public int ConnectionTimeout
		{
			get
			{
				return connectionTimeOut;
			}
			set
			{
				connectionTimeOut = value;
			}
		}

    /// <summary>
		/// Gets or sets the name of the instance of the VistaDB database to connect to.
		/// </summary>
		//[Browsable(true), Editor("VistaDB.Designer.VistaDBDatabasePathEditor, VistaDB.Designer.VS2003", "System.Drawing.Design.UITypeEditor")]
		public string DataSource
		{
			get
			{
				return this.dataSource;
			}

			set
			{
				this.dataSource = value;
			}
		}

		string IDbConnection.Database
		{
			get
			{
				return this.database;
			}
		}
		/// <summary>
		/// Gets or sets the name of the current database or the database to be used after a connection is opened.
		/// </summary>
		public string Database
		{
			get
			{
				return this.database;
			}
			set
			{
				this.database = value;
			}
		}

		/// <summary>
		/// Gets or sets the database password.
		/// </summary>
		public string Password
		{
			get
			{
				return this.password;
			}

			set
			{
				this.password = value;
			}
		}

		/// <summary>
		/// Gets or sets the database encryption type, or Cypher type.
		/// </summary>
		public CypherType Cypher
		{
			get
			{
				return this.cypher;
			}

			set
			{
				this.cypher = value;
			}
		}
		/// <summary>
		/// Gets or sets if a database is to be opened in exclusive mode. Required for altering the database schema.
		/// </summary>
		public bool Exclusive
		{
			get
			{
				return this.exclusive;
			}
			set
			{
				this.exclusive = value;
			}
		}

		/// <summary>
		/// Gets or sets if a database is to be opened in readonly mode.
		/// </summary>
		public bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				this.readOnly = value;
			}
		}

		/// <summary>
		/// User login name for server connect
		/// </summary>
		public string LoginUser
		{
			get
			{
				return this.loginUser;
			}
			set
			{
				this.loginUser = value;
			}
		}

		/// <summary>
		/// Login password for server connect
		/// </summary>
		public string LoginPassword
		{
			get
			{
				return this.loginPassword;
			}
			set
			{
				this.loginPassword = value;
			}
		}

		/// <summary>
		/// Connection access mode
		/// </summary>
		public AccessMode AccessMode
		{
			get
			{
				return this.accessMode;
			}
			set
			{
				this.accessMode = value;
			}
		}

		/// <summary>
		/// Opened database culture id
		/// </summary>
		[Browsable(false)]
		public int CultureID
		{
			get
			{
				return this.vistaDBSQL == null ? -1: this.vistaDBSQL.CultureID;
			}
		}

		/// <summary>
		/// Opened database cluster size
		/// </summary>
		[Browsable(false)]
		public int ClusterSize
		{
			get
			{
				return this.vistaDBSQL == null ? -1: this.vistaDBSQL.ClusterSize;
			}
		}

		/// <summary>
		/// Returns True if opened database is case sensitive
		/// </summary>
		[Browsable(false)]
		public bool CaseSensitivity
		{
			get
			{
				return this.vistaDBSQL == null ? false: this.vistaDBSQL.CaseSensitivity;
			}
		}

		/// <summary>
		/// Opened database description
		/// </summary>
		[Browsable(false)]
		public string DatabaseDescription
		{
			get
			{
				return this.vistaDBSQL == null ? "": this.vistaDBSQL.DatabaseDescription;
			}
		}
	}
}