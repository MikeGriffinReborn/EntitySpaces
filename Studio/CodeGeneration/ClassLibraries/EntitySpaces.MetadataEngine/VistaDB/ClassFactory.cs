using System;

using esMetadataEngine;

namespace esMetadataEngine.VistaDB
{
	public class ClassFactory : IClassFactory
	{
        public static void Register()
        {
            InternalDriver.Register("VISTADB",
                new FileDbDriver
                (typeof(ClassFactory)
                , @"DataSource=", @"C:\Program Files\VistaDB 2.0\Data\Northwind.vdb", @";Cypher= None;Password=;Exclusive=False;Readonly=False;"
                , "VistaDB (*.vbd)|*.vbd|all files (*.*)|*.*"));
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new VistaDB.VistaDBTables();
		}

		public ITable CreateTable()
		{
			return new VistaDB.VistaDBTable();
		}

		public IColumn CreateColumn()
		{
			return new VistaDB.VistaDBColumn();
		}

		public IColumns CreateColumns()
		{
			return new VistaDB.VistaDBColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new VistaDB.VistaDBDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new VistaDB.VistaDBDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new VistaDB.VistaDBProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new VistaDB.VistaDBProcedures();
		}

		public IView CreateView()
		{
			return new VistaDB.VistaDBView();
		}

		public IViews CreateViews()
		{
			return new VistaDB.VistaDBViews();
		}

		public IParameter CreateParameter()
		{
			return new VistaDB.VistaDBParameter();
		}

		public IParameters CreateParameters()
		{
			return new VistaDB.VistaDBParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new VistaDB.VistaDBForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new VistaDB.VistaDBForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new VistaDB.VistaDBIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new VistaDB.VistaDBIndexes();
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
			return new VistaDB.VistaDBResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new VistaDB.VistaDBResultColumns();
		}


		public IProviderType CreateProviderType()
		{
			return new ProviderType();
		}

		public IProviderTypes CreateProviderTypes()
		{
			return new ProviderTypes();
		}

        #region IClassFactory Members

        public System.Data.IDbConnection CreateConnection()
        {
            return new Provider.VistaDB.VistaDBConnection();
        }

        #endregion
    }
}
