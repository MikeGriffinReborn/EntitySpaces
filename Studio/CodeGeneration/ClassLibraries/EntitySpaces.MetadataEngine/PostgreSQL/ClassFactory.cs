using System;

using EntitySpaces.MetadataEngine;

namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class ClassFactory : IClassFactory
	{
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new PostgreSQL.PostgreSQLTables();
		}

		public ITable CreateTable()
		{
			return new PostgreSQL.PostgreSQLTable();
		}

		public IColumn CreateColumn()
		{
			return new PostgreSQL.PostgreSQLColumn();
		}

		public IColumns CreateColumns()
		{
			return new PostgreSQL.PostgreSQLColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new PostgreSQL.PostgreSQLDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new PostgreSQL.PostgreSQLDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new PostgreSQL.PostgreSQLProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new PostgreSQL.PostgreSQLProcedures();
		}

		public IView CreateView()
		{
			return new PostgreSQL.PostgreSQLView();
		}

		public IViews CreateViews()
		{
			return new PostgreSQL.PostgreSQLViews();
		}

		public IParameter CreateParameter()
		{
			return new PostgreSQL.PostgreSQLParameter();
		}

		public IParameters CreateParameters()
		{
			return new PostgreSQL.PostgreSQLParameters();
		}

		public IForeignKey  CreateForeignKey()
		{
			return new PostgreSQL.PostgreSQLForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new PostgreSQL.PostgreSQLForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new PostgreSQL.PostgreSQLIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new PostgreSQL.PostgreSQLIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new PostgreSQL.PostgreSQLResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new PostgreSQL.PostgreSQLResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new PostgreSQLDomain();
		}

		public IDomains CreateDomains()
		{
			return new PostgreSQLDomains();
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
            return PostgreSQLDatabases.CreateConnection("");
        }
    }
}
