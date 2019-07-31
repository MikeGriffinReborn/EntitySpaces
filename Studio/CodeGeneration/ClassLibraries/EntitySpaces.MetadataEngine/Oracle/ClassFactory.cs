using System;

using EntitySpaces.MetadataEngine;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class ClassFactory : IClassFactory
	{
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new Oracle.OracleTables();
		}

		public ITable CreateTable()
		{
			return new Oracle.OracleTable();
		}

		public IColumn CreateColumn()
		{
			return new Oracle.OracleColumn();
		}

		public IColumns CreateColumns()
		{
			return new Oracle.OracleColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new Oracle.OracleDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new Oracle.OracleDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new Oracle.OracleProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new Oracle.OracleProcedures();
		}

		public IView CreateView()
		{
			return new Oracle.OracleView();
		}

		public IViews CreateViews()
		{
			return new Oracle.OracleViews();
		}

		public IParameter CreateParameter()
		{
			return new Oracle.OracleParameter();
		}

		public IParameters CreateParameters()
		{
			return new Oracle.OracleParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new Oracle.OracleForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new Oracle.OracleForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new Oracle.OracleIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new Oracle.OracleIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new Oracle.OracleResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new Oracle.OracleResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new OracleDomain();
		}

		public IDomains CreateDomains()
		{
			return new OracleDomains();
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
