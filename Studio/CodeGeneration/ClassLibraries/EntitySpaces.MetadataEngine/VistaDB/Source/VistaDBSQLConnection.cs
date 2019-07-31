using System;
using System.Data;
//using System.Windows.Forms;
using System.Text;
using System.ComponentModel;

namespace Provider.VistaDB
{

	/// <summary>
	/// VistaDBSQL class for managing V-SQL query statements.
	/// </summary>
	internal abstract class VistaDBSQLConnection: IDisposable
	{
		protected string dataSource; 
		protected string database;
		protected CypherType cypher;
		protected string password;
		protected bool exclusive;
		protected bool readOnly;
		protected bool opened;
		protected VistaDBSQLQuery[] queries;
		protected int cultureID;
		protected int clusterSize;
		protected bool caseSensitivity;
		protected string databaseDescription;
		protected object syncRoot = new object();

		/// <summary>
		/// Constructor.
		/// </summary>
		public VistaDBSQLConnection()
		{
			this.dataSource          = "";
			this.database            = "";
			this.cypher              = CypherType.None;
			this.password            = "";
			this.exclusive           = false;
			this.readOnly            = false;
			this.opened              = false;
			this.queries             = new VistaDBSQLQuery[0];
			this.cultureID           = 0;
			this.clusterSize         = 0;
			this.caseSensitivity     = false;
			this.databaseDescription = "";
		}

		public abstract void Dispose();

		/// <summary>
		/// Open a database connection to a VistaDB database.
		/// </summary>
		/// <returns></returns>
		public abstract void OpenDatabaseConnection();

		/// <summary>
		/// Close an active database connection.
		/// </summary>
		public virtual void CloseDatabaseConnection()
		{
			lock(this.syncRoot)
			{
				for(int i = 0; i < this.queries.Length; i++)
				{
					this.queries[i].Close();
					this.queries[i].FreeQuery();
				}
			}
		}

		protected abstract VistaDBSQLQuery CreateSQLQuery();

		/// <summary>
		/// Create new query for this connection.
		/// </summary>
		/// <returns></returns>
		public VistaDBSQLQuery NewSQLQuery()
		{
			VistaDBSQLQuery query;
			VistaDBSQLQuery[] newQueries;

			query = CreateSQLQuery();

			lock(this.syncRoot)
			{
				newQueries = new VistaDBSQLQuery[queries.Length + 1];

				for(int i = 0; i < queries.Length; i++ )
				{
					newQueries[i] = this.queries[i];
				}

				this.queries = newQueries;

				this.queries[queries.GetUpperBound(0)] = query;
			}

			return query;
		}

		public bool DropQuery(VistaDBSQLQuery query)
		{
			int indexQuery = -1;
			VistaDBSQLQuery[] newQueries;

			lock(this.syncRoot)
			{
				for(int i = 0; i < queries.Length; i++)
				{
					if( this.queries[i] == query )
					{
						indexQuery = i;
						break;
					}
				}

				if(indexQuery < 0)
					return false;

				this.queries[indexQuery].Close();
				this.queries[indexQuery].FreeQuery();

				newQueries = new VistaDBSQLQuery[this.queries.Length - 1];

				for(int i = 0; i < newQueries.Length; i++)
				{
					if( i < indexQuery )
						newQueries[i] = this.queries[i];
					else
						newQueries[i] = this.queries[i + 1];
				}

				this.queries = newQueries;
			}

			return true;
		}

		/// <summary>
		/// Begin a transaction. Transactions may be nested.
		/// </summary>
		public abstract bool BeginTransaction();

		/// <summary>
		/// Commit an active transaction. Transactions may be nested.
		/// </summary>
		public abstract bool CommitTransaction();

		/// <summary>
		/// Rollback the active transaction. Transactions may be nested. 
		/// </summary>
		public abstract void RollbackTransaction();

		/// <summary>
		/// Gets or sets the data source.
		/// </summary>
		public virtual string DataSource
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

		/// <summary>
		/// Gets or sets the database name
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

		public int CultureID
		{
			get
			{
				return this.cultureID;
			}
		}

		public int ClusterSize
		{
			get
			{
				return this.clusterSize;
			}
		}

		public bool CaseSensitivity
		{
			get
			{
				return this.caseSensitivity;
			}
		}

		public string DatabaseDescription
		{
			get
			{
				return this.databaseDescription;
			}
		}
	}
}

