//'''''''''''''''''''''''''''''''''''''
using System;
using System.ComponentModel;
using System.Text;
using System.Collections;

namespace Provider.VistaDB
{
	/// <summary>
	/// VistaDBDatabase class provides a live connection object into a VistaDB database
	/// </summary>	
#if EVAL
	[LicenseProviderAttribute(typeof(VistaDBLicenseProvider))]
#endif
	public class VistaDBDatabase: Component, ISupportInitialize
	{
		//////////////////////////////////////////////
		///////////////////////CONSTRUCTOR\DESTRUCTOR/////////////////////////////
		//////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Constructor
		/// </summary>
		public VistaDBDatabase()
		{
			InitClass();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="databasename">Database name</param>
		public VistaDBDatabase(string databasename)
		{
			InitClass();

			databaseName = databasename;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="DatabaseName">Database name</param>
		/// <param name="Exclusive">True for exclusive mode</param>
		public VistaDBDatabase(string DatabaseName, bool Exclusive)
		{
			InitClass();

			databaseName = DatabaseName;
			exclusive = Exclusive;
		}

		/// <summary>
		/// Constructor. Set DatabaseName, Exclusive and ReadOnly property
		/// </summary>
		/// <param name="databasename">Database name</param>
		/// <param name="exclusive_">True for exclusive mode</param>
		/// <param name="readonly_">True for read only mode</param>
		public VistaDBDatabase(string databasename, bool exclusive_, bool readonly_)
		{
			InitClass();

			databaseName = databasename;
			exclusive = exclusive_;
			readOnly = readonly_;
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~VistaDBDatabase()
		{
			Dispose();
		}


		///////////////////////////////////////////////////////////////////////
		////////////////////////////////METHODS////////////////////////////////
		///////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Activates space recycling mechanism
		/// </summary>
		public void ActivateRecycling()
		{
			if(databaseId <= 0)
				return;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				VistaDBAPI.ivdb_ActivateRecycling();
			}
		}

		/// <summary>
		/// Space recycling mechanism status
		/// </summary>
		/// <returns>Return True if activated space recycling mechanism</returns>
		public bool ActivatedRecycling()
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_ActivatedRecycling();
			}
		}

		/// <summary>
		/// Add table to XML export\import list
		/// </summary>
		/// <param name="cpTableName">Table name</param>
		/// <returns>Return true if success else false</returns>
		public bool AddToExportList( string cpTableName )
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_AddToExportList(cpTableName);
			}
		}

		internal void AddToInitList(IVistaDBDataSet dataSet)
		{
			if(this.initList != null)
				this.initList.Add(dataSet);
		}

		void ISupportInitialize.BeginInit()
		{
			this.initStarted = true;
			this.needConnect = false;
			this.initList = new ArrayList();
		}
		
		/// <summary>
		/// Begin transaction
		/// </summary>
		/// <returns></returns>
		public bool BeginTransaction()
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_BeginTransaction();
			}
		}

		/// <summary>
		/// Clear export\import list
		/// </summary>
		/// <returns>Return true if success else false</returns>
		public bool ClearExportList()
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_ClearExportList();
			}
		}

		/// <summary>
		/// Clear low level data caching made by engine for database.
		/// </summary>
		public void ClearMemoryCache()
		{
			if(this.databaseId <= 0)
				return;

			lock(this.SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(this.databaseId);
				VistaDBAPI.ivdb_ClearMemoryCache();
			}
		}

		/// <summary>
		/// Close database
		/// </summary>
		public void Close()
		{
			lock(SyncRoot)
			{
				CloseWithoutSync();
			}
		}

		internal void CloseWithoutSync()
		{
			if(databaseId > 0)
			{
				for(int i = 0; i < tableList.Count; i++)
					((VistaDBTable)tableList[i]).Close();

				VistaDBAPI.ivdb_SelectDb(databaseId);
				VistaDBAPI.ivdb_CloseDatabase();

				databaseId = 0;
			}
		}

		internal void FreeDatabase()
		{
			if(databaseId <= 0)
				return;

			lock(SyncRoot)
			{
				for(int i = 0; i < tableList.Count; i++)
					((VistaDBTable)tableList[i]).Close();

				this.databaseId = 0;
			}
		}

		/// <summary>
		/// Commit transaction
		/// </summary>
		/// <returns>Return true for success</returns>
		public bool CommitTransaction()
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_CommitTransaction();
			}
		}

		//////////////Methods//////////////////////

		/// <summary>
		/// Connect to database
		/// </summary>
		/// <returns>True if opened</returns>
		public bool Connect()
		{
			lock(SyncRoot)
			{
				return ConnectWithoutSync();
			}
		}

		/// <summary>
		/// Connect to database, but without synchronization
		/// </summary>
		/// <returns>True if opened</returns>
		internal bool ConnectWithoutSync()
		{
#if EVAL
			if (license == null)
				license = LicenseManager.Validate(typeof(VistaDBDatabase), this);
#endif

			if( databaseId > 0 )
			{
				return VistaDBAPI.ivdb_SelectDb( databaseId ); 
			}

			try
			{		
				databaseId = VistaDBAPI.ivdb_OpenDatabase(databaseName, "", exclusive, readOnly, 
					(uint)parameters, password, (uint)cypher, false);
				clusterSize = VistaDBAPI.ivdb_GetClusterLength();
			}
			catch(VistaDBException e)
			{
				if(!e.Critical)
					databaseId = (int)(e.Result);

				throw;
			}

			return databaseId > 0;
		}

		/// <summary>
		/// Create new database without case sensitivity for indexes and searching (overloaded)
		/// </summary>
		/// <param name="iCultureId">Culture ID for database</param>
		/// <param name="bOpenAfterCreation">If true then database opens after creation</param>
		/// <returns>return true for success</returns>
		public bool CreateDatabase(int iCultureId, bool bOpenAfterCreation )
		{
			return CreateDatabase( iCultureId, bOpenAfterCreation, false );
		}
		
		/// <summary>
		/// Create new database
		/// </summary>
		/// <param name="iCultureId">Culture ID for database</param>
		/// <param name="bOpenAfterCreation">If true then database opens after creation</param>
		/// <param name="bCaseSensitivity">Sets case-sensitivity flag for character data in run-time searching and filtering operations. Used as respective flag in primary and FTS indexes</param>
		/// <returns>return true for success</returns>
		public bool CreateDatabase(int iCultureId, bool bOpenAfterCreation, bool bCaseSensitivity)
		{
			bool res;
			int tmp;

			if(bOpenAfterCreation && databaseId > 0)
				throw new VistaDBException(VistaDBErrorCodes.DatabaseMustBeClosedBeforeCreate);

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SetClusterLength(clusterSize);
				//Parameters bExclusive and bReadOnly are not used by Engine
				tmp = VistaDBAPI.ivdb_CreateDatabase(databaseName, "", true, false, (uint) iCultureId, (uint)parameters, 
					password, (uint)cypher, bCaseSensitivity);
				VistaDBAPI.ivdb_SetClusterLength(1);

				if(tmp > 0)
				{

					if (parameters != VDBDatabaseParam.InMemory)
					{

						VistaDBAPI.ivdb_CloseDatabase();

						if(bOpenAfterCreation)
						{
							res = ConnectWithoutSync();
						}
						else
						{
							res = true;
						}
					}
					else
					{
						databaseId = tmp;
						res = true;
					}
				}
				else
					res = false;

				return res;
			}

		}

		/// <summary>
		/// Deactivates space recycling mechanism
		/// </summary>
		public void DeactivateRecycling()
		{
			if(databaseId <= 0)
				return;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				VistaDBAPI.ivdb_DeactivateRecycling();
			}
		}

		/// <summary>
		/// Dispose method
		/// </summary>
		protected override void Dispose(bool disposing)
		{
#if EVAL
			if (license != null) 
			{
				license.Dispose();
				license = null;
			}
#endif
			this.Close();
			if(this.tableList != null)
			{
				for(int i = 0; i < tableList.Count; i++)
					((VistaDBTable)tableList[i]).Database = null;
				this.tableList = null;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Drop table from database
		/// </summary>
		/// <param name="sTableName">Table name</param>
		/// <returns>Return true for success</returns>
		public bool DropTable(string sTableName)
		{
			if(databaseId > 0)
			{
				lock(SyncRoot)
				{
					VistaDBAPI.ivdb_SelectDb(databaseId);
					VistaDBAPI.ivdb_DropTable(sTableName);

					return true;
				}
			}
			else
				return false;
		}

		/// <summary>
		/// Drop table from database without using recycling.
		/// </summary>
		/// <param name="tableName">Table name</param>
		/// <returns>Return true for success</returns>
		public bool DropTableInstantly(string tableName)
		{
			if(databaseId > 0)
			{
				lock(SyncRoot)
				{
					VistaDBAPI.ivdb_SelectDb(databaseId);
					VistaDBAPI.ivdb_DropTableInstantly(tableName);

					return true;
				}
			}
			else
				return false;
		}

		void ISupportInitialize.EndInit()
		{
			this.initStarted = false;

			if(this.needConnect)
				this.Connect();

			for(int i = 0; i < this.initList.Count; i++)
			{
				((IVistaDBDataSet)initList[i]).OpenAfterInit();
			}

			this.initList = null;
		}

		/// <summary>
		/// Enum tables in the database
		/// </summary>
		/// <param name="list">Table names list</param>
		public void EnumTables(ref string[] list)
		{
			uint i;
			string sTableName;
			StringBuilder buf;
			string[] listTemp;

			if(databaseId <= 0)
				return;

			list = null;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);

				buf = new StringBuilder(64);

				i = 0;

				do
				{
					i++;

					i = VistaDBAPI.ivdb_EnumTables(i, buf, 63);

					if(i != 0)
					{
						sTableName = VistaDBAPI.CutString(buf);

						if(list == null)
							list = new string[1];
						else
						{
							listTemp = new string[list.Length + 1];
							list.CopyTo(listTemp, 0);
							list = listTemp;
						}

						list[list.GetUpperBound(0)] = sTableName;

					}

				}
				while(i != 0);
			}

		}

		/// <summary>
		/// Export data from tables (in list) to xml-file
		/// </summary>
		/// <param name="cpXmlFileName">Xml-file name</param>
		/// <param name="OnlySchema">If true, then exports only schema else schema and data</param>
		/// <returns>Return true if success else false</returns>
		public bool ExportData(string cpXmlFileName, bool OnlySchema)
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_ExportData(cpXmlFileName, OnlySchema);
			}
		}

		/// <summary>
		/// Flush database data buffers to disk
		/// </summary>
		public void FlushFileBuffers()
		{
			if(databaseId <= 0)
				return;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				VistaDBAPI.ivdb_FlushFileBuffers();
			}
		}
	
		/// <summary>
		/// Check database case sensitive flag
		/// </summary>
		/// <returns>Return true if database is set to support case sensitive, else false</returns>
		[Browsable(false)]
		public bool GetCaseSensitive()
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_GetCaseSensitive();
			}
		}

		/// <summary>
		/// Import data to tables (in list)
		/// </summary>
		/// <param name="cpXmlFileName">Xml-file name</param>
		/// <returns>Return true if success else false</returns>
		public bool ImportData(string cpXmlFileName)
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_ImportData(cpXmlFileName);
			}
		}

		/// <summary>
		/// Import schema to current database
		/// </summary>
		/// <param name="cpXmlFileName">Xml-file name</param>
		/// <returns>Return true if success else false</returns>
		public bool ImportSchema(string cpXmlFileName)
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_ImportSchema(cpXmlFileName);
			}
		}
		
		/// <summary>
		/// Import schema and data to current database
		/// </summary>
		/// <param name="cpXmlFileName">Xml-file name</param>
		/// <returns>Return true if success else false</returns>
		public bool ImportSchemaAndData(string cpXmlFileName)
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_ImportSchemaAndData(cpXmlFileName);
			}
		}

		///////////////////////////////////////////
		
		/// <summary>
		/// Class object initialiser
		/// </summary>
		public void InitClass()
		{
			VistaDBErrorMsgs.SetErrorFunc();
			databaseName = "";
			databaseId = 0;
			exclusive = false;
			readOnly = false;
			parameters = 0;
			tableList = new ArrayList();
		}
	
		/// <summary>
		/// Check table existence
		/// </summary>
		/// <param name="tableName">Table name</param>
		/// <returns>Return true, if table exist, else false</returns>
		public bool IsTableExist(string tableName)
		{
			if(databaseId <= 0)
				return false;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_IsTableExist(tableName);
			}
		}

		/// <summary>
		/// Pack database
		/// </summary>
		/// <returns>True for success</returns>
		public bool PackDatabase()
		{
			return PackDatabase(password, cypher, password, cypher, 
				CaseSensitive, Locale, false);
		}

		/// <summary>
		/// Pack database with option 'save file permission'
		/// </summary>
		/// <param name="SaveFilePermission">Save file permission option</param>
		/// <returns></returns>
		public bool PackDatabase(bool SaveFilePermission)
		{
			return PackDatabase(password, cypher, password, cypher, 
				CaseSensitive, Locale, false, SaveFilePermission);
		}

		/// <summary>
		/// Pack database
		/// <param name="newPassword">New password</param>
		/// <param name="newCypher">New cypher</param>
		/// <param name="MarkFullEncryption">Mark full encryption option</param>
		/// </summary>
		/// <returns>True for success</returns>
		public bool PackDatabase(string newPassword, CypherType newCypher, bool MarkFullEncryption)
		{
			return PackDatabase(password, cypher, newPassword, newCypher, 
				CaseSensitive, Locale, MarkFullEncryption);
		}
		
		/// <summary>
		/// Pack database
		/// <param name="newPassword">New password</param>
		/// <param name="newCypher">New cypher</param>
		/// <param name="newCaseSensitive">New case sensitive option</param>
		/// <param name="newLocale">New locale</param>
		/// <param name="MarkFullEncryption">Mark full encryption option</param>
		/// </summary>
		/// <returns>True for success</returns>
		public bool PackDatabase(string newPassword, CypherType newCypher, 
			bool newCaseSensitive, int newLocale, bool MarkFullEncryption)
		{
			return PackDatabase(password, cypher, newPassword, newCypher, 
				newCaseSensitive, newLocale, MarkFullEncryption);
		}

		/// <summary>
		/// Pack database
		/// <param name="oldPassword">Old password</param>
		/// <param name="oldCypher">Old cypher</param>
		/// <param name="newPassword">New password</param>
		/// <param name="newCypher">New cypher</param>
		/// <param name="newCaseSensitive">New case sensitive option</param>
		/// <param name="newLocale">New locale</param>
		/// <param name="MarkFullEncryption">Mark full encryption option</param>
		/// </summary>
		/// <returns>True for success</returns>
		public bool PackDatabase(string oldPassword, CypherType oldCypher, 
			string newPassword, CypherType newCypher, bool newCaseSensitive,
			int newLocale, bool MarkFullEncryption)
		{
			return PackDatabase(oldPassword, oldCypher, newPassword, newCypher, newCaseSensitive,
				newLocale, MarkFullEncryption, false);
		}

		/// <summary>
		/// Pack database with option 'save file permission'
		/// </summary>
		/// <param name="oldPassword">Old password</param>
		/// <param name="oldCypher">Old cypher</param>
		/// <param name="newPassword">New password</param>
		/// <param name="newCypher">New cypher</param>
		/// <param name="newCaseSensitive">New case sensitive option</param>
		/// <param name="newLocale">New locale</param>
		/// <param name="MarkFullEncryption">Mark full encryption option</param>
		/// <param name="SaveFilePermission">Save file permission option</param>
		/// <returns>True for success</returns>
		public bool PackDatabase(string oldPassword, CypherType oldCypher, 
			string newPassword, CypherType newCypher, bool newCaseSensitive,
			int newLocale, bool MarkFullEncryption, bool SaveFilePermission)
		{
			bool needOpen, res;

			lock(SyncRoot)
			{
				if(databaseId > 0)
				{
					needOpen = true;
					CloseWithoutSync();
				}
				else
					needOpen = false;

				res = VistaDBAPI.ivdb_PackDatabase(databaseName, oldPassword, (uint)oldCypher,
					newPassword, (uint)newCypher, newCaseSensitive, newLocale, MarkFullEncryption,
					SaveFilePermission);

				password = newPassword;
				cypher = newCypher;

				if( needOpen )
					ConnectWithoutSync();

				return res;
			}
		}
			
		/// <summary>
		/// Register table in list
		/// </summary>
		/// <param name="table">Table object</param>
		internal void RegisterTable(VistaDBTable table)
		{
			if( tableList.IndexOf(table) >= 0 )
				return;

			tableList.Add(table);
		}

		/// <summary>
		/// Unregister table in list
		/// </summary>
		/// <param name="table"></param>
		internal void UnregisterTable(VistaDBTable table)
		{
			if(this.tableList != null)
			{
				int index = tableList.IndexOf(table); 

				if( index < 0 )
					return;

				tableList.RemoveAt(index);
			}
		}

		/// <summary>
		/// Rollback transaction
		/// </summary>
		/// <param name="AllLevels">True for rollback all levels</param>
		public void RollbackTransaction(bool AllLevels)
		{
			if(databaseId <= 0)
				return;

			lock(SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(databaseId);
				VistaDBAPI.ivdb_RollbackTransaction(AllLevels);
			}
		}

		/// <summary>
		/// Saves in-memory table into 
		/// </summary>
		/// <param name="databaseName">Database name</param>
		/// <returns></returns>
		public bool SaveToFile(string databaseName)
		{
			if(this.databaseId <= 0 || this.parameters != VDBDatabaseParam.InMemory)
				return false;

			lock(this.SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(this.databaseId);
				return VistaDBAPI.ivdb_SaveToDatabase(databaseName);
			}
		}



		////////////////////////////////////////////////////////////////////
		/////////////////////////////PROPERTIES/////////////////////////////
		////////////////////////////////////////////////////////////////////

    
		/// <summary>
		/// Get case sensitivity for database
		/// </summary>
		[Browsable(false)]
		public bool CaseSensitive
		{
			get
			{
				if(databaseId <= 0)
					return false;

				lock(SyncRoot)
				{
					VistaDBAPI.ivdb_SelectDb(databaseId);
					return VistaDBAPI.ivdb_GetCaseSensitive();
				}
			}
		}

		/// <summary>
		/// Cluster size
		/// </summary>
		[Browsable(false)]
		public int ClusterSize
		{
			get
			{
				return clusterSize;
			}
			set
			{
				if(databaseId > 0)
					return;

				clusterSize = value;
			}
		}

		/// <summary>
		/// Database connected state
		/// </summary>
		public bool Connected
		{
			get
			{
				return this.databaseId > 0;
			}
			set
			{
				if(this.initStarted)
					this.needConnect = value;
				else
				{
					if(value)
						this.Connect();
					else
						this.Close();
				}
			}
		}

		/// <summary>
		/// Gets or sets the database encryption, or Cypher type
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
		/// Gets the Database ID
		/// </summary>
		[Browsable(false)]
		public int DatabaseId
		{
			get
			{
				return databaseId;
			}
		}

		////////////////////////Properties/////////////////////
		/// <summary>
		/// Gets or sets the Database filename
		/// </summary>
		public string DatabaseName
		{
			get
			{
				return databaseName;
			}
			set
			{
				if(databaseId > 0)
				{
					throw new VistaDBException(VistaDBErrorCodes.DatabaseNameCanNotBeChanged);
				}

				databaseName = value;
			}
		}

		/// <summary>
		/// Database description
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Description
		{
			get
			{
				if(databaseId <= 0)
					return "";

				VistaDBAPI.ivdb_SelectDb(databaseId);
				return VistaDBAPI.ivdb_GetDatabaseDescription();
			}
			set
			{
				if(databaseId <= 0)
					return;

				VistaDBAPI.ivdb_SelectDb(databaseId);
				VistaDBAPI.ivdb_SetDatabaseDescription(value);
			}
		}

		/// <summary>
		/// Gets or sets the database exclusive mode property.
		/// </summary>
		public bool Exclusive
		{
			get
			{
				return exclusive;
			}
			set
			{
				if(databaseId > 0)
				{
					throw new VistaDBException(VistaDBErrorCodes.ExclusiveCanNotBeChanged);
				}

				exclusive = value;
			}
		}

		/// <summary>
		/// Get Locale for database
		/// </summary>
		[Browsable(false)]
		public int Locale
		{
			get
			{
				if(databaseId <= 0)
					return 0;

				lock(SyncRoot)
				{
					VistaDBAPI.ivdb_SelectDb(databaseId);
					return (int)VistaDBAPI.ivdb_GetDatabaseCultureId();
				}
			}
		}
		
		/// <summary>
		/// Database connection parameters
		/// </summary>
		public VDBDatabaseParam Parameters
		{
			get
			{
				return parameters;
			}
			set
			{
				if(databaseId > 0)
				{
					throw new VistaDBException(VistaDBErrorCodes.ParametersCanNotBeChanged);
				}

				parameters = value;
			}
		}

		/// <summary>
		/// Database read only mode property
		/// </summary>
		public bool ReadOnly
		{
			get
			{
				return readOnly;
			}
			set
			{
				if(databaseId > 0)
				{
					throw new VistaDBException(VistaDBErrorCodes.ReadOnlyCanNotBeChanged);
				}

				readOnly = value;
			}
		}

		/// <summary>
		/// Password for database
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
		/// Internal object for synchronization 
		/// </summary>
		internal object SyncRoot
		{
			get
			{
				return syncRoot;
			}
		}

		/// <summary>
		/// VistaDB engine version
		/// </summary>
		public static string Version
		{
			get
			{
				return VistaDBAPI.ivdb_Version();
			}
		}

		/// <summary>
		/// Database transaction level
		/// </summary>
		[Browsable(false)]
		public int TransactionLevel
		{
			get
			{
				lock(SyncRoot)
				{
					VistaDBAPI.ivdb_SelectDb(databaseId);
					return (int) VistaDBAPI.ivdb_GetTransactionLevel();
				}
			}
		}

		/////////////////////////////////////////////////////////////////////
		/////////////////////////FIELDS//////////////////////////////////////
		/////////////////////////////////////////////////////////////////////

		private int clusterSize = 1;
		private CypherType cypher = CypherType.None;
		private int databaseId = 0;
		private string databaseName = null;
		private bool exclusive = false;
#if EVAL
		private License license = null;
#endif
		private string password = null;
		private VDBDatabaseParam parameters = VDBDatabaseParam.None;
		private ArrayList tableList = null;
		private bool readOnly = false;
		private static object syncRoot = new object();
		private bool initStarted = false;
		private bool needConnect = false;
		private ArrayList initList = null;
	}
}