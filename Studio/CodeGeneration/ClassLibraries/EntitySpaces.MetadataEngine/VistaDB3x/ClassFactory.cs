using System;

using MyMeta;

namespace MyMeta.VistaDB3x
{
#if ENTERPRISE
	using System.EnterpriseServices;
	using System.Runtime.InteropServices;
	[ComVisible(false)]
#endif
	public class ClassFactory : IClassFactory
	{
		public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new VistaDB3x.VistaDBTables();
		}

		public ITable CreateTable()
		{
			return new VistaDB3x.VistaDBTable();
		}

		public IColumn CreateColumn()
		{
			return new VistaDB3x.VistaDBColumn();
		}

		public IColumns CreateColumns()
		{
			return new VistaDB3x.VistaDBColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new VistaDB3x.VistaDBDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new VistaDB3x.VistaDBDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new VistaDB3x.VistaDBProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new VistaDB3x.VistaDBProcedures();
		}

		public IView CreateView()
		{
			return new VistaDB3x.VistaDBView();
		}

		public IViews CreateViews()
		{
			return new VistaDB3x.VistaDBViews();
		}

		public IParameter CreateParameter()
		{
			return new VistaDB3x.VistaDBParameter();
		}

		public IParameters CreateParameters()
		{
			return new VistaDB3x.VistaDBParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new VistaDB3x.VistaDBForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new VistaDB3x.VistaDBForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new VistaDB3x.VistaDBIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new VistaDB3x.VistaDBIndexes();
		}

		public IDomain CreateDomain()
		{
			return new VistaDBDomain();
		}

		public IDomains CreateDomains()
		{
			return new VistaDBDomains();
		}

		public IResultColumn CreateResultColumn()
		{
			return new VistaDB3x.VistaDBResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new VistaDB3x.VistaDBResultColumns();
		}


		public IProviderType CreateProviderType()
		{
			return new ProviderType();
		}

		public IProviderTypes CreateProviderTypes()
		{
			return new ProviderTypes();
		}

        public System.Data.IDbConnection CreateConnection()
        {
            return new Provider.VistaDB.VistaDBConnection();
        }

    }
}
