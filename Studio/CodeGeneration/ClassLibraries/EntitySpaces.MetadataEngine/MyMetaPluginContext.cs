using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace EntitySpaces.MetadataEngine
{
    public class PluginContext : IContext
    {
        private bool includeSystemEntities;
        private string connectionString = string.Empty;
        private string providerName = string.Empty;

        public PluginContext(string providerName, string connectionString) 
        {
            this.providerName = providerName;
            this.connectionString = connectionString;
        }

        public bool IncludeSystemEntities
        {
            get { return includeSystemEntities; }
            set { includeSystemEntities = value; }
        }

        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }


		public DataTable CreateDatabasesDataTable()
		{
			DataTable table = new DataTable();
			table.Columns.Add("CATALOG_NAME", Type.GetType("System.String"));
			table.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			table.Columns.Add("SCHEMA_NAME", Type.GetType("System.String"));
			table.Columns.Add("SCHEMA_OWNER", Type.GetType("System.String"));
			table.Columns.Add("DEFAULT_CHARACTER_SET_CATALOG", Type.GetType("System.String"));
			table.Columns.Add("DEFAULT_CHARACTER_SET_SCHEMA", Type.GetType("System.String"));
			table.Columns.Add("DEFAULT_CHARACTER_SET_NAME", Type.GetType("System.String"));
			return table;
		}

        public DataTable CreateForeignKeysDataTable()
        {
            DataTable metaData = new DataTable();
			metaData.Columns.Add("PK_TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("PK_TABLE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("PK_TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("PK_COLUMN_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("PK_COLUMN_GUID", Type.GetType("System.Guid"));
			metaData.Columns.Add("PK_COLUMN_PROPID", Type.GetType("System.Int64"));
			metaData.Columns.Add("FK_TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("FK_TABLE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("FK_TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("FK_COLUMN_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("FK_COLUMN_GUID", Type.GetType("System.Guid"));
			metaData.Columns.Add("FK_COLUMN_PROPID", Type.GetType("System.Int64"));
			metaData.Columns.Add("ORDINAL", Type.GetType("System.Int64"));
			metaData.Columns.Add("UPDATE_RULE", Type.GetType("System.String"));
			metaData.Columns.Add("DELETE_RULE", Type.GetType("System.String"));
			metaData.Columns.Add("PK_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("FK_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("DEFERRABILITY", Type.GetType("System.Int16"));
            return metaData;
        }

		public DataTable CreateTablesDataTable()
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_TYPE", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_GUID", Type.GetType("System.Guid"));
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_PROPID", Type.GetType("System.Int64"));
			metaData.Columns.Add("DATE_CREATED", Type.GetType("System.DateTime"));
			metaData.Columns.Add("DATE_MODIFIED", Type.GetType("System.DateTime"));
			return metaData;
		}

		public DataTable CreateViewsDataTable()
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_TYPE", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_GUID", Type.GetType("System.Guid"));
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			metaData.Columns.Add("VIEW_TEXT", Type.GetType("System.String"));
			metaData.Columns.Add("IS_UPDATABLE", Type.GetType("System.Boolean"));
			metaData.Columns.Add("TABLE_PROPID", Type.GetType("System.Int64"));
			metaData.Columns.Add("DATE_CREATED", Type.GetType("System.DateTime"));
			metaData.Columns.Add("DATE_MODIFIED", Type.GetType("System.DateTime"));
			return metaData;
		}

		public DataTable CreateColumnsDataTable()
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("COLUMN_GUID", Type.GetType("System.Guid"));
			metaData.Columns.Add("COLUMN_PROPID", Type.GetType("System.Int64"));
			metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int64"));
			metaData.Columns.Add("COLUMN_HASDEFAULT", Type.GetType("System.Boolean"));
			metaData.Columns.Add("COLUMN_DEFAULT", Type.GetType("System.String"));
			metaData.Columns.Add("COLUMN_FLAGS", Type.GetType("System.Int64"));
			metaData.Columns.Add("IS_NULLABLE", Type.GetType("System.Boolean"));
			metaData.Columns.Add("DATA_TYPE", Type.GetType("System.Int32"));
			metaData.Columns.Add("TYPE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("TYPE_NAME_COMPLETE", Type.GetType("System.String"));
			metaData.Columns.Add("TYPE_GUID", Type.GetType("System.Guid"));
			metaData.Columns.Add("CHARACTER_MAXIMUM_LENGTH", Type.GetType("System.Int64"));
			metaData.Columns.Add("CHARACTER_OCTET_LENGTH", Type.GetType("System.Int64"));
			metaData.Columns.Add("NUMERIC_PRECISION", Type.GetType("System.Int32"));
			metaData.Columns.Add("NUMERIC_SCALE", Type.GetType("System.Int16"));
			metaData.Columns.Add("DATETIME_PRECISION", Type.GetType("System.Int64"));
			metaData.Columns.Add("CHARACTER_SET_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("CHARACTER_SET_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("CHARACTER_SET_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("COLLATION_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("COLLATION_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("COLLATION_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("DOMAIN_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("DOMAIN_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("DOMAIN_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			metaData.Columns.Add("COLUMN_LCID", Type.GetType("System.Int32"));
			metaData.Columns.Add("COLUMN_COMPFLAGS", Type.GetType("System.Int32"));
			metaData.Columns.Add("COLUMN_SORTID", Type.GetType("System.Int32"));
			metaData.Columns.Add("IS_COMPUTED", Type.GetType("System.Boolean"));
			metaData.Columns.Add("IS_AUTO_KEY", Type.GetType("System.Boolean"));				
			metaData.Columns.Add("AUTO_KEY_SEED", Type.GetType("System.Int32"));				
			metaData.Columns.Add("AUTO_KEY_INCREMENT", Type.GetType("System.Int32"));
            metaData.Columns.Add("IS_CONCURRENCY", Type.GetType("System.Boolean"));
			return metaData;
		}

		public DataTable CreateIndexesDataTable()
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("INDEX_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("INDEX_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("INDEX_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("PRIMARY_KEY", Type.GetType("System.Boolean"));
			metaData.Columns.Add("UNIQUE", Type.GetType("System.Boolean"));
			metaData.Columns.Add("CLUSTERED", Type.GetType("System.Boolean"));
			metaData.Columns.Add("TYPE", Type.GetType("System.Int32"));
			metaData.Columns.Add("FILL_FACTOR", Type.GetType("System.Int32"));
			metaData.Columns.Add("INITIAL_SIZE", Type.GetType("System.Int32"));
			metaData.Columns.Add("NULLS", Type.GetType("System.Int32"));
			metaData.Columns.Add("SORT_BOOKMARKS", Type.GetType("System.Boolean"));
			metaData.Columns.Add("AUTO_UPDATE", Type.GetType("System.Boolean"));
			metaData.Columns.Add("NULL_COLLATION", Type.GetType("System.Int32"));
			metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int64"));
			metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("COLUMN_GUID", Type.GetType("System.Guid"));
			metaData.Columns.Add("COLUMN_PROPID", Type.GetType("System.Int64"));
			metaData.Columns.Add("COLLATION", Type.GetType("System.Int16"));
			metaData.Columns.Add("CARDINALITY", Type.GetType("System.Decimal"));
			metaData.Columns.Add("PAGES", Type.GetType("System.Int32"));
			metaData.Columns.Add("FILTER_CONDITION", Type.GetType("System.String"));
			metaData.Columns.Add("INTEGRATED", Type.GetType("System.Boolean"));
			return metaData;
		}

		public DataTable CreateProceduresDataTable()
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("PROCEDURE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("PROCEDURE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("PROCEDURE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("PROCEDURE_TYPE", Type.GetType("System.Int16"));
			metaData.Columns.Add("PROCEDURE_DEFINITION", Type.GetType("System.String"));
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			metaData.Columns.Add("DATE_CREATED", Type.GetType("System.DateTime"));
			metaData.Columns.Add("DATE_MODIFIED", Type.GetType("System.DateTime"));
			return metaData;
		}

		public DataTable CreateParametersDataTable()
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("PROCEDURE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("PROCEDURE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("PROCEDURE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("PARAMETER_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int32"));
			metaData.Columns.Add("PARAMETER_TYPE", Type.GetType("System.Int32"));
			metaData.Columns.Add("PARAMETER_HASDEFAULT", Type.GetType("System.Boolean"));
			metaData.Columns.Add("PARAMETER_DEFAULT", Type.GetType("System.String"));
			metaData.Columns.Add("IS_NULLABLE", Type.GetType("System.Boolean"));
			metaData.Columns.Add("DATA_TYPE", Type.GetType("System.Int32"));
			metaData.Columns.Add("CHARACTER_MAXIMUM_LENGTH", Type.GetType("System.Int64"));
			metaData.Columns.Add("CHARACTER_OCTET_LENGTH", Type.GetType("System.Int64"));
			metaData.Columns.Add("NUMERIC_PRECISION", Type.GetType("System.Int32"));
			metaData.Columns.Add("NUMERIC_SCALE", Type.GetType("System.Int16"));
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			metaData.Columns.Add("TYPE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("LOCAL_TYPE_NAME", Type.GetType("System.String"));
			return metaData;
		}

		public DataTable CreateResultColumnsDataTable()
		{
			DataTable metaData = new DataTable();
            // Fix k3b 20070709: PluginResultColumns.BindToColumns and 
            //      MyMetaPluginContext.CreateResultColumnsDataTable
            //      ColumnNames were different
            metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));
			// metaData.Columns.Add("Alias", Type.GetType("System.String"));
            metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int64"));
            metaData.Columns.Add("TYPE_NAME", Type.GetType("System.String"));
            metaData.Columns.Add("TYPE_NAME_COMPLETE", Type.GetType("System.String"));
			// metaData.Columns.Add("LanguageType", Type.GetType("System.String"));
            // metaData.Columns.Add("DbTargetType", Type.GetType("System.String"));
			return metaData;
		}

		public DataTable CreateDomainsDataTable()
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("DOMAIN_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("DOMAIN_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("DOMAIN_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("DATA_TYPE", Type.GetType("System.String"));
			metaData.Columns.Add("CHARACTER_MAXIMUM_LENGTH", Type.GetType("System.Int32"));
			metaData.Columns.Add("CHARACTER_OCTET_LENGTH", Type.GetType("System.Int32"));
			metaData.Columns.Add("COLLATION_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("COLLATION_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("COLLATION_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("CHARACTER_SET_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("CHARACTER_SET_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("CHARACTER_SET_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("NUMERIC_PRECISION", Type.GetType("System.Byte"));
			metaData.Columns.Add("NUMERIC_PRECISION_RADIX", Type.GetType("System.Int16"));
			metaData.Columns.Add("NUMERIC_SCALE", Type.GetType("System.Int32"));
			metaData.Columns.Add("DATETIME_PRECISION", Type.GetType("System.Int16"));
			metaData.Columns.Add("DOMAIN_DEFAULT", Type.GetType("System.String"));
			return metaData;
		}
    }
}
