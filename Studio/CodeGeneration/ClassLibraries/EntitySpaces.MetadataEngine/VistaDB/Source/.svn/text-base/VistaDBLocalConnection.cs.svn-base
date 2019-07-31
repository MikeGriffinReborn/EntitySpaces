using System;
using System.Text;

namespace Provider.VistaDB
{
	/// <summary>
	/// VistaDBSQL class for managing V-SQL query statements.
	/// </summary>
	internal class VistaDBLocalConnection: VistaDBSQLConnection
	{
		private int connectionID;

		public VistaDBLocalConnection(): base()
		{
			this.connectionID = VistaDBAPI.ivsql_CreateDatabaseConnection();
		}

		public VistaDBLocalConnection(int cultureID, bool caseSensitive): this()
		{
			int dbID = VistaDBAPI.ivdb_CreateDatabase(null, null, false, false, (uint)cultureID,
				(uint)VDBDatabaseParam.InMemory, null, 0, caseSensitive);

			try
			{
				if(!VistaDBAPI.ivsql_AssignDatabaseConnection(this.connectionID, dbID, null, true, false, 0, null, caseSensitive))
					throw new VistaDBException(VistaDBErrorCodes.DatabaseNotOpened);

				this.cultureID       = cultureID;
				this.caseSensitivity = caseSensitivity;
				this.opened          = true;
			}
			catch
			{
				VistaDBAPI.ivdb_SelectDb(dbID);
				VistaDBAPI.ivdb_CloseDatabase();
				throw;
			}
		}

		public VistaDBLocalConnection(VistaDBDatabase db): this()
		{
			if(!VistaDBAPI.ivsql_AssignDatabaseConnection(this.connectionID, db.DatabaseId, null, true, false, 0, null, db.CaseSensitive))
				throw new VistaDBException(VistaDBErrorCodes.DatabaseNotOpened);

			db.FreeDatabase();

			this.cultureID       = db.Locale;
			this.caseSensitivity = db.CaseSensitive;
			this.opened          = true;
		}

		~VistaDBLocalConnection()
		{
			Dispose();
		}

		/// <summary>
		/// Open a database connection to a VistaDB database.
		/// </summary>
		public override void OpenDatabaseConnection()
		{
			if( this.opened )
				return;

			bool success = false;
			CypherType _cypher;
			string _password;
			int dbID;

			lock(this.syncRoot)
			{
				try
				{
					if(this.cypher == CypherType.None)
					{
						_cypher = CypherType.Blowfish;
						_password = "";
					}
					else
					{
						_cypher = this.cypher;
						_password = this.password;
					}

					success = VistaDBAPI.ivsql_OpenDatabaseConnection(this.connectionID, this.dataSource, this.exclusive, this.readOnly, _cypher, _password, false);
				}
				catch(VistaDBException e)
				{
					if(!e.Critical)
					{
						for(int i = 0; i < this.queries.Length; i++)
							this.queries[i].CreateQuery();

						this.opened = true;
					}

					throw;
				}

				if (!success )
				{
					throw new VistaDBException(VistaDBErrorCodes.SQLDatabaseCouldNotBeFound);
				}

				this.opened = true;

				for(int i = 0; i < this.queries.Length; i++)
					this.queries[i].CreateQuery();

				dbID                     = VistaDBAPI.ivsql_GetCurrentDatabaseID(this.connectionID);

				this.cultureID           = (int)VistaDBAPI.ivdb_GetDatabaseCultureId();
				this.clusterSize         = VistaDBAPI.ivdb_GetClusterLength();
				this.caseSensitivity     = VistaDBAPI.ivdb_GetCaseSensitive();
				this.databaseDescription = VistaDBAPI.ivdb_GetDatabaseDescription();
			}
		}

		/// <summary>
		/// Close an active database connection.
		/// </summary>
		public override void CloseDatabaseConnection()
		{
			if( ! this.opened )
				return;

			base.CloseDatabaseConnection();

			lock(this.syncRoot)
			{
				VistaDBAPI.ivsql_CloseDatabaseConnection( this.connectionID);
				this.opened = false;
			}
		}

		protected override VistaDBSQLQuery CreateSQLQuery()
		{
			return new VistaDBLocalQuery(this);
		}

		/// <summary>
		/// Return a unique connection ID to the opened database.
		/// </summary>
		public int ConnectionID
		{
			get
			{
				return this.connectionID;
			}
		}

		/// <summary>
		/// Begin a transaction. Transactions may be nested.
		/// </summary>
		public override bool BeginTransaction()
		{
			bool res;

			res = VistaDBAPI.ivsql_BeginTransaction(this.connectionID);

			return res;
		}

		/// <summary>
		/// Commit an active transaction. Transactions may be nested.
		/// </summary>
		public override bool CommitTransaction()
		{
			bool res;

			res = VistaDBAPI.ivsql_CommitTransaction(this.connectionID);

			return res;
		}

		/// <summary>
		/// Rollback the active transaction. Transactions may be nested. 
		/// </summary>
		public override void RollbackTransaction()
		{
			VistaDBAPI.ivsql_RollbackTransaction(this.connectionID);
		}

		/// <summary>
		/// Gets or sets the data source.
		/// </summary>
		public override string DataSource
		{
			set
			{
				dataSource = value;
				Database = value; 
			}
		}

		public override void Dispose()
		{
			if(this.connectionID == 0)
				return;

			try
			{
				CloseDatabaseConnection();
			}
			finally
			{
				VistaDBAPI.ivsql_FreeDatabaseConnection(this.connectionID);
				this.connectionID = 0;
			}
		}
	}
}
