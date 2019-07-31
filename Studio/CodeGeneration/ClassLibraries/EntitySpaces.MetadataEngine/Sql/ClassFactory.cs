using System;

using EntitySpaces.MetadataEngine;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class ClassFactory : IClassFactory
	{
		public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new Sql.SqlTables();
		}

		public ITable CreateTable()
		{
			return new Sql.SqlTable();
		}

		public IColumn CreateColumn()
		{
			return new Sql.SqlColumn();
		}

		public IColumns CreateColumns()
		{
			return new Sql.SqlColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new Sql.SqlDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new Sql.SqlDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new Sql.SqlProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new Sql.SqlProcedures();
		}

		public IView CreateView()
		{
			return new Sql.SqlView();
		}

		public IViews CreateViews()
		{
			return new Sql.SqlViews();
		}

		public IParameter CreateParameter()
		{
			return new Sql.SqlParameter();
		}

		public IParameters CreateParameters()
		{
			return new Sql.SqlParameters();
		}

		public IForeignKey  CreateForeignKey()
		{
			return new Sql.SqlForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new Sql.SqlForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new Sql.SqlIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new Sql.SqlIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new Sql.SqlResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new Sql.SqlResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new Sql.SqlDomain();
		}

		public IDomains CreateDomains()
		{
			return new Sql.SqlDomains();
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
            return new System.Data.OleDb.OleDbConnection();
        }
    }
}
