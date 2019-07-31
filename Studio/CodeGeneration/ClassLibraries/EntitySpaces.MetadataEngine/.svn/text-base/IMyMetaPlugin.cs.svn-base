using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace EntitySpaces.MetadataEngine
{
	public interface IPlugin
    {
        /// <summary>
        /// Called when the plugin is loaded - 1 time only
        /// </summary>
        bool OnLoad();

        /// <summary>
        /// Called when the plugin is unloaded - 1 time only
        /// </summary>
        void OnUnload();
        /// <summary>
        /// Initialize the plugin
        /// </summary>
        /// <param name="context"></param>
        void Initialize(IContext context);

        /// <summary>
        /// The providers unique key
        /// </summary>
        string ProviderUniqueKey { get; }

        /// <summary>
        /// A name for the MyMeta provider.
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Information about the plugin author
        /// </summary>
        string ProviderAuthorInfo { get; }

        /// <summary>
        /// Author's Uri
        /// </summary>
        Uri ProviderAuthorUri { get; }

        /// <summary>
        /// String trailing nulls when fetching meta data? This is currently only true for the MySql providers.
        /// </summary>
        bool StripTrailingNulls { get; }

        /// <summary>
        /// Is the DatabaseName required to make a connection?
        /// </summary>
        bool RequiredDatabaseName { get; }

        /// <summary>
        /// A Sample connection string
        /// </summary>
        string SampleConnectionString { get; }

        /// <summary>
        /// Get a IDbConnection for the database that is not opend yet
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IDbConnection NewConnection { get; }

        /// <summary>
        /// Name of the default database.
        /// </summary>
        string DefaultDatabase { get; }

        /*
        if(metaData.Columns.Contains("CATALOG_NAME"))	f_Catalog		 = metaData.Columns["CATALOG_NAME"];
		if(metaData.Columns.Contains("DESCRIPTION"))	f_Description	 = metaData.Columns["DESCRIPTION"];
		if(metaData.Columns.Contains("SCHEMA_NAME"))	f_SchemaName	 = metaData.Columns["SCHEMA_NAME"];
		if(metaData.Columns.Contains("SCHEMA_OWNER"))	f_SchemaOwner	 = metaData.Columns["SCHEMA_OWNER"];
		if(metaData.Columns.Contains("DEFAULT_CHARACTER_SET_CATALOG"))	f_DefCharSetCat		= metaData.Columns["DEFAULT_CHARACTER_SET_CATALOG"];
		if(metaData.Columns.Contains("DEFAULT_CHARACTER_SET_SCHEMA"))	f_DefCharSetSchema	= metaData.Columns["DEFAULT_CHARACTER_SET_SCHEMA"];
		if(metaData.Columns.Contains("DEFAULT_CHARACTER_SET_NAME"))		f_DefCharSetName	= metaData.Columns["DEFAULT_CHARACTER_SET_NAME"];
        */
        DataTable Databases { get; }

        /*
		if(metaData.Columns.Contains("TABLE_CATALOG"))		f_Catalog		 = metaData.Columns["TABLE_CATALOG"];
		if(metaData.Columns.Contains("TABLE_SCHEMA"))		f_Schema		 = metaData.Columns["TABLE_SCHEMA"];
		if(metaData.Columns.Contains("TABLE_NAME"))			f_Name			 = metaData.Columns["TABLE_NAME"];
		if(metaData.Columns.Contains("TABLE_TYPE"))			f_Type			 = metaData.Columns["TABLE_TYPE"];
		if(metaData.Columns.Contains("TABLE_GUID"))			f_Guid			 = metaData.Columns["TABLE_GUID"];
		if(metaData.Columns.Contains("DESCRIPTION"))		f_Description	 = metaData.Columns["DESCRIPTION"];
		if(metaData.Columns.Contains("TABLE_PROPID"))		f_PropID		 = metaData.Columns["TABLE_PROPID"];
		if(metaData.Columns.Contains("DATE_CREATED"))		f_DateCreated	 = metaData.Columns["DATE_CREATED"];
		if(metaData.Columns.Contains("DATE_MODIFIED"))		f_DateModified	 = metaData.Columns["DATE_MODIFIED"];
        */
        DataTable GetTables(string database);

        /*
        if(metaData.Columns.Contains("TABLE_CATALOG"))		f_Catalog		 = metaData.Columns["TABLE_CATALOG"];
		if(metaData.Columns.Contains("TABLE_SCHEMA"))		f_Schema		 = metaData.Columns["TABLE_SCHEMA"];
		if(metaData.Columns.Contains("TABLE_NAME"))			f_Name			 = metaData.Columns["TABLE_NAME"];
		if(metaData.Columns.Contains("TABLE_TYPE"))			f_Type			 = metaData.Columns["TABLE_TYPE"];
		if(metaData.Columns.Contains("VIEW_DEFINITION"))	f_ViewDefinition = metaData.Columns["VIEW_DEFINITION"];
		if(metaData.Columns.Contains("CHECK_OPTION"))		f_CheckOption	 = metaData.Columns["CHECK_OPTION"];
		if(metaData.Columns.Contains("IS_UPDATABLE"))		f_IsUpdateable	 = metaData.Columns["IS_UPDATABLE"];
		if(metaData.Columns.Contains("TABLE_GUID"))			f_Guid			 = metaData.Columns["TABLE_GUID"];
		if(metaData.Columns.Contains("DESCRIPTION"))		f_Description	 = metaData.Columns["DESCRIPTION"];
		if(metaData.Columns.Contains("TABLE_PROPID"))		f_PropID		 = metaData.Columns["TABLE_PROPID"];
		if(metaData.Columns.Contains("DATE_CREATED"))		f_DateCreated	 = metaData.Columns["DATE_CREATED"];
		if(metaData.Columns.Contains("DATE_MODIFIED"))		f_DateModified	 = metaData.Columns["DATE_MODIFIED"];
        */
        DataTable GetViews(string database);

        /*
        if(metaData.Columns.Contains("PROCEDURE_CATALOG"))		f_Catalog				= metaData.Columns["PROCEDURE_CATALOG"];
        if(metaData.Columns.Contains("PROCEDURE_SCHEMA"))		f_Schema				= metaData.Columns["PROCEDURE_SCHEMA"];
        if(metaData.Columns.Contains("PROCEDURE_NAME"))			f_Name					= metaData.Columns["PROCEDURE_NAME"];
        if(metaData.Columns.Contains("PROCEDURE_TYPE"))			f_Type					= metaData.Columns["PROCEDURE_TYPE"];
        if(metaData.Columns.Contains("PROCEDURE_DEFINITION"))	f_ProcedureDefinition	= metaData.Columns["PROCEDURE_DEFINITION"];
        if(metaData.Columns.Contains("DESCRIPTION"))			f_Description			= metaData.Columns["DESCRIPTION"];
        if(metaData.Columns.Contains("DATE_CREATED"))			f_DateCreated			= metaData.Columns["DATE_CREATED"];
        if(metaData.Columns.Contains("DATE_MODIFIED"))			f_DateModified			= metaData.Columns["DATE_MODIFIED"];
        */
        DataTable GetProcedures(string database);

        /*
        if(metaData.Columns.Contains("DOMAIN_CATALOG"))						f_DomainCatalog		= metaData.Columns["DOMAIN_CATALOG"];
		if(metaData.Columns.Contains("DOMAIN_SCHEMA"))						f_DomainSchema		= metaData.Columns["DOMAIN_SCHEMA"];
		if(metaData.Columns.Contains("DOMAIN_NAME"))						f_DomainName		= metaData.Columns["DOMAIN_NAME"];
		if(metaData.Columns.Contains("DATA_TYPE"))							f_DataType			= metaData.Columns["DATA_TYPE"];
		if(metaData.Columns.Contains("CHARACTER_MAXIMUM_LENGTH"))			f_MaxLength			= metaData.Columns["CHARACTER_MAXIMUM_LENGTH"];
		if(metaData.Columns.Contains("CHARACTER_OCTET_LENGTH"))				f_OctetLength		= metaData.Columns["CHARACTER_OCTET_LENGTH"];
		if(metaData.Columns.Contains("COLLATION_CATALOG"))					f_CollationCatalog	= metaData.Columns["COLLATION_CATALOG"];
		if(metaData.Columns.Contains("COLLATION_SCHEMA"))					f_CollationSchema	= metaData.Columns["COLLATION_SCHEMA"];
		if(metaData.Columns.Contains("COLLATION_NAME"))						f_CollationName		= metaData.Columns["COLLATION_NAME"];
		if(metaData.Columns.Contains("CHARACTER_SET_CATALOG"))				f_CharSetCatalog    = metaData.Columns["CHARACTER_SET_CATALOG"];
		if(metaData.Columns.Contains("CHARACTER_SET_SCHEMA"))				f_CharSetSchema     = metaData.Columns["CHARACTER_SET_SCHEMA"];
		if(metaData.Columns.Contains("CHARACTER_SET_NAME"))					f_CharSetName       = metaData.Columns["CHARACTER_SET_NAME"];
		if(metaData.Columns.Contains("NUMERIC_PRECISION"))					f_NumericPrecision	= metaData.Columns["NUMERIC_PRECISION"];
		if(metaData.Columns.Contains("NUMERIC_SCALE"))						f_NumericScale		= metaData.Columns["NUMERIC_SCALE"];
		if(metaData.Columns.Contains("DATETIME_PRECISION"))					f_DatetimePrecision = metaData.Columns["DATETIME_PRECISION"];
		if(metaData.Columns.Contains("DOMAIN_DEFAULT"))						f_Default			= metaData.Columns["DOMAIN_DEFAULT"];
		if(metaData.Columns.Contains("IS_NULLABLE"))						f_IsNullable		= metaData.Columns["IS_NULLABLE"];
        */
        DataTable GetDomains(string database);

        /*
        if(metaData.Columns.Contains("PROCEDURE_CATALOG"))			f_Catalog			 = metaData.Columns["PROCEDURE_CATALOG"];
        if(metaData.Columns.Contains("PROCEDURE_SCHEMA"))			f_Schema			 = metaData.Columns["PROCEDURE_SCHEMA"];
        if(metaData.Columns.Contains("PROCEDURE_NAME"))				f_ProcedureName		 = metaData.Columns["PROCEDURE_NAME"];
        if(metaData.Columns.Contains("PARAMETER_NAME"))				f_ParameterName		 = metaData.Columns["PARAMETER_NAME"];
        if(metaData.Columns.Contains("ORDINAL_POSITION"))			f_Ordinal			 = metaData.Columns["ORDINAL_POSITION"];
        if(metaData.Columns.Contains("PARAMETER_TYPE"))				f_Type				 = metaData.Columns["PARAMETER_TYPE"];
        if(metaData.Columns.Contains("PARAMETER_HASDEFAULT"))		f_HasDefault		 = metaData.Columns["PARAMETER_HASDEFAULT"];
        if(metaData.Columns.Contains("PARAMETER_DEFAULT"))			f_Default			 = metaData.Columns["PARAMETER_DEFAULT"];
        if(metaData.Columns.Contains("IS_NULLABLE"))				f_IsNullable		 = metaData.Columns["IS_NULLABLE"];
        if(metaData.Columns.Contains("DATA_TYPE"))					f_DataType			 = metaData.Columns["DATA_TYPE"];
        if(metaData.Columns.Contains("CHARACTER_MAXIMUM_LENGTH"))	f_CharMaxLength		 = metaData.Columns["CHARACTER_MAXIMUM_LENGTH"];
        if(metaData.Columns.Contains("CHARACTER_OCTET_LENGTH"))		f_CharOctetLength	 = metaData.Columns["CHARACTER_OCTET_LENGTH"];
        if(metaData.Columns.Contains("NUMERIC_PRECISION"))			f_NumericPrecision   = metaData.Columns["NUMERIC_PRECISION"];
        if(metaData.Columns.Contains("NUMERIC_SCALE"))				f_NumericScale		 = metaData.Columns["NUMERIC_SCALE"];
        if(metaData.Columns.Contains("DESCRIPTION"))				f_Description		 = metaData.Columns["DESCRIPTION"];
        if(metaData.Columns.Contains("FULL_TYPE_NAME"))				f_FullTypeName		 = metaData.Columns["FULL_TYPE_NAME"];
        if(metaData.Columns.Contains("TYPE_NAME"))					f_TypeName			 = metaData.Columns["TYPE_NAME"];
        if(metaData.Columns.Contains("LOCAL_TYPE_NAME"))			f_LocalTypeName		 = metaData.Columns["LOCAL_TYPE_NAME"];
         */
        DataTable GetProcedureParameters(string database, string procedure);

        /*
        if (metaData.Columns.Contains("COLUMN_NAME")) f_Name = metaData.Columns["COLUMN_NAME"];
        if (metaData.Columns.Contains("ORDINAL_POSITION")) f_Ordinal = metaData.Columns["ORDINAL_POSITION"];
        if (metaData.Columns.Contains("DATA_TYPE")) f_DataType = metaData.Columns["DATA_TYPE"];
        if (metaData.Columns.Contains("DATA_TYPE_NAME")) f_DataType = metaData.Columns["DATA_TYPE_NAME"];
        if (metaData.Columns.Contains("DATA_TYPE_NAME_COMPLETE")) f_DataType = metaData.Columns["DATA_TYPE_NAME_COMPLETE"];
        */
        DataTable GetProcedureResultColumns(string database, string procedure);

        /*
        if(metaData.Columns.Contains("TABLE_CATALOG"))						f_TableCatalog		= metaData.Columns["TABLE_CATALOG"];
        if(metaData.Columns.Contains("TABLE_SCHEMA"))						f_TableSchema		= metaData.Columns["TABLE_SCHEMA"];
        if(metaData.Columns.Contains("TABLE_NAME"))							f_TableName			= metaData.Columns["TABLE_NAME"];
        if(metaData.Columns.Contains("COLUMN_NAME"))						f_Name				= metaData.Columns["COLUMN_NAME"];
        if(metaData.Columns.Contains("COLUMN_GUID"))						f_Guid				= metaData.Columns["COLUMN_GUID"];
        if(metaData.Columns.Contains("COLUMN_PROPID"))						f_PropID            = metaData.Columns["COLUMN_PROPID"];
        if(metaData.Columns.Contains("ORDINAL_POSITION"))					f_Ordinal           = metaData.Columns["ORDINAL_POSITION"];
        if(metaData.Columns.Contains("COLUMN_HASDEFAULT"))					f_HasDefault        = metaData.Columns["COLUMN_HASDEFAULT"];
        if(metaData.Columns.Contains("COLUMN_DEFAULT"))						f_Default			= metaData.Columns["COLUMN_DEFAULT"];
        if(metaData.Columns.Contains("COLUMN_FLAGS"))						f_Flags				= metaData.Columns["COLUMN_FLAGS"];
        if(metaData.Columns.Contains("IS_NULLABLE"))						f_IsNullable		= metaData.Columns["IS_NULLABLE"];
        if(metaData.Columns.Contains("DATA_TYPE"))							f_DataType			= metaData.Columns["DATA_TYPE"];
        if(metaData.Columns.Contains("TYPE_GUID"))							f_TypeGuid			= metaData.Columns["TYPE_GUID"];
        if(metaData.Columns.Contains("CHARACTER_MAXIMUM_LENGTH"))			f_MaxLength			= metaData.Columns["CHARACTER_MAXIMUM_LENGTH"];
        if(metaData.Columns.Contains("CHARACTER_OCTET_LENGTH"))				f_OctetLength		= metaData.Columns["CHARACTER_OCTET_LENGTH"];
        if(metaData.Columns.Contains("NUMERIC_PRECISION"))					f_NumericPrecision	= metaData.Columns["NUMERIC_PRECISION"];
        if(metaData.Columns.Contains("NUMERIC_SCALE"))						f_NumericScale		= metaData.Columns["NUMERIC_SCALE"];
        if(metaData.Columns.Contains("DATETIME_PRECISION"))					f_DatetimePrecision = metaData.Columns["DATETIME_PRECISION"];
        if(metaData.Columns.Contains("CHARACTER_SET_CATALOG"))				f_CharSetCatalog    = metaData.Columns["CHARACTER_SET_CATALOG"];
        if(metaData.Columns.Contains("CHARACTER_SET_SCHEMA"))				f_CharSetSchema     = metaData.Columns["CHARACTER_SET_SCHEMA"];
        if(metaData.Columns.Contains("CHARACTER_SET_NAME"))					f_CharSetName       = metaData.Columns["CHARACTER_SET_NAME"];
        if(metaData.Columns.Contains("COLLATION_CATALOG"))					f_CollationCatalog	= metaData.Columns["COLLATION_CATALOG"];
        if(metaData.Columns.Contains("COLLATION_SCHEMA"))					f_CollationSchema	= metaData.Columns["COLLATION_SCHEMA"];
        if(metaData.Columns.Contains("COLLATION_NAME"))						f_CollationName		= metaData.Columns["COLLATION_NAME"];
        if(metaData.Columns.Contains("DOMAIN_CATALOG"))						f_DomainCatalog		= metaData.Columns["DOMAIN_CATALOG"];
        if(metaData.Columns.Contains("DOMAIN_SCHEMA"))						f_DomainSchema		= metaData.Columns["DOMAIN_SCHEMA"];
        if(metaData.Columns.Contains("DOMAIN_NAME"))						f_DomainName		= metaData.Columns["DOMAIN_NAME"];
        if(metaData.Columns.Contains("DESCRIPTION"))						f_Description		= metaData.Columns["DESCRIPTION"];
        if(metaData.Columns.Contains("COLUMN_LCID"))						f_LCID				= metaData.Columns["COLUMN_LCID"];
        if(metaData.Columns.Contains("COLUMN_COMPFLAGS"))					f_CompFlags			= metaData.Columns["COLUMN_COMPFLAGS"];
        if(metaData.Columns.Contains("COLUMN_SORTID"))						f_SortID			= metaData.Columns["COLUMN_SORTID"];
        if(metaData.Columns.Contains("COLUMN_TDSCOLLATION"))				f_TDSCollation		= metaData.Columns["COLUMN_TDSCOLLATION"];
        if(metaData.Columns.Contains("IS_COMPUTED"))						f_IsComputed		= metaData.Columns["IS_COMPUTED"];
        if(metaData.Columns.Contains("IS_AUTO_KEY"))						f_IsAutoKey			= metaData.Columns["IS_AUTO_KEY"];
        if(metaData.Columns.Contains("AUTO_KEY_SEED"))						f_AutoKeySeed		= metaData.Columns["AUTO_KEY_SEED"];
        if(metaData.Columns.Contains("AUTO_KEY_INCREMENT"))					f_AutoKeyIncrement	= metaData.Columns["AUTO_KEY_INCREMENT"];
         */
        DataTable GetViewColumns(string database, string view);
        DataTable GetTableColumns(string database, string table);

        /// <summary>
        /// Returns a list of column names that are primary keys for a given table
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        List<string> GetPrimaryKeyColumns(string database, string table);
        
        /// <summary>
        /// Returns a list of view names that make up the parent view
        /// </summary>
        /// <param name="database"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        List<string> GetViewSubViews(string database, string view);
        
        /// <summary>
        /// Returns a list of table names that make up the parent view
        /// </summary>
        /// <param name="database"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        List<string> GetViewSubTables(string database, string view);

        /*
        if(metaData.Columns.Contains("TABLE_CATALOG"))			f_Catalog			 = metaData.Columns["TABLE_CATALOG"];
        if(metaData.Columns.Contains("TABLE_SCHEMA"))			f_Schema			 = metaData.Columns["TABLE_SCHEMA"];
        if(metaData.Columns.Contains("TABLE_NAME"))				f_TableName			 = metaData.Columns["TABLE_NAME"];
        if(metaData.Columns.Contains("INDEX_CATALOG"))			f_IndexCatalog		 = metaData.Columns["INDEX_CATALOG"];
        if(metaData.Columns.Contains("INDEX_SCHEMA"))			f_IndexSchema		 = metaData.Columns["INDEX_SCHEMA"];
        if(metaData.Columns.Contains("INDEX_NAME"))				f_IndexName			 = metaData.Columns["INDEX_NAME"];
        if(metaData.Columns.Contains("UNIQUE"))					f_Unique			 = metaData.Columns["UNIQUE"];
        if(metaData.Columns.Contains("CLUSTERED"))				f_Clustered			 = metaData.Columns["CLUSTERED"];
        if(metaData.Columns.Contains("TYPE"))					f_Type				 = metaData.Columns["TYPE"];
        if(metaData.Columns.Contains("FILL_FACTOR"))			f_FillFactor		 = metaData.Columns["FILL_FACTOR"];
        if(metaData.Columns.Contains("INITIAL_SIZE"))			f_InitializeSize	 = metaData.Columns["INITIAL_SIZE"];
        if(metaData.Columns.Contains("NULLS"))					f_Nulls				 = metaData.Columns["NULLS"];
        if(metaData.Columns.Contains("SORT_BOOKMARKS"))			f_SortBookmarks		 = metaData.Columns["SORT_BOOKMARKS"];
        if(metaData.Columns.Contains("AUTO_UPDATE"))			f_AutoUpdate		 = metaData.Columns["AUTO_UPDATE"];
        if(metaData.Columns.Contains("NULL_COLLATION"))			f_NullCollation		 = metaData.Columns["NULL_COLLATION"];
        if(metaData.Columns.Contains("COLLATION"))				f_Collation			 = metaData.Columns["COLLATION"];
        if(metaData.Columns.Contains("CARDINALITY"))			f_Cardinality		 = metaData.Columns["CARDINALITY"];
        if(metaData.Columns.Contains("PAGES"))					f_Pages				 = metaData.Columns["PAGES"];
        if(metaData.Columns.Contains("FILTER_CONDITION"))		f_FilterCondition	 = metaData.Columns["FILTER_CONDITION"];
        if(metaData.Columns.Contains("INTEGRATED"))				f_Integrated		 = metaData.Columns["INTEGRATED"];
        */
        DataTable GetTableIndexes(string database, string table);

        /*
        if(metaData.Columns.Contains("PK_TABLE_CATALOG"))    f_PKTableCatalog = metaData.Columns["PK_TABLE_CATALOG"];
        if(metaData.Columns.Contains("PK_TABLE_SCHEMA"))	 f_PKTableSchema = metaData.Columns["PK_TABLE_SCHEMA"];
        if(metaData.Columns.Contains("PK_TABLE_NAME"))		 f_PKTableName = metaData.Columns["PK_TABLE_NAME"];
        if(metaData.Columns.Contains("FK_TABLE_CATALOG"))	 f_FKTableCatalog = metaData.Columns["FK_TABLE_CATALOG"];
        if(metaData.Columns.Contains("FK_TABLE_SCHEMA"))	 f_FKTableSchema = metaData.Columns["FK_TABLE_SCHEMA"];
        if(metaData.Columns.Contains("FK_TABLE_NAME"))		 f_FKTableName = metaData.Columns["FK_TABLE_NAME"];
        if(metaData.Columns.Contains("ORDINAL"))			 f_Ordinal = metaData.Columns["ORDINAL"];
        if(metaData.Columns.Contains("UPDATE_RULE"))		 f_UpdateRule = metaData.Columns["UPDATE_RULE"];
        if(metaData.Columns.Contains("DELETE_RULE"))		 f_DeleteRule = metaData.Columns["DELETE_RULE"];
        if(metaData.Columns.Contains("PK_NAME"))			 f_PKName = metaData.Columns["PK_NAME"];
        if(metaData.Columns.Contains("FK_NAME"))			 f_FKName= metaData.Columns["FK_NAME"];
        if(metaData.Columns.Contains("DEFERRABILITY"))		 f_Deferrability = metaData.Columns["DEFERRABILITY"];
        */
        DataTable GetForeignKeys(string database, string table);

        /// <summary>
        /// since MyGeneration 1.2.0.7
        /// This is a way to return proprietary meta data specific to the Database engine. 
        /// An example would be: 
        ///     sqlServerPlugin.GetDatabaseSpecificMetaData(userITable, "ExtendedProperties");
        /// 
        /// The myMetaObject match a specific MyMeta object of a type like IColumn, ITable, IView, IProcedure, etc.
        /// The key is something that describes the kind of meta data requested.
        ///  
        /// special keys (since MyGeneration 1.2.0.8)
        /// 
        /// GetDatabaseSpecificMetaData(null,"CanBrowseDatabase")
        ///     - if not null the plugin supports Browse for Database
        /// 
        /// GetDatabaseSpecificMetaData(null,"BrowseDatabase")
        ///     - Asks the plugin to display a dialog to browse for the database
        ///     - returns a new connectionstring or null
        /// 
        /// </summary>
        object GetDatabaseSpecificMetaData(object myMetaObject, string key);
    }
}