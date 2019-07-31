using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace EntitySpaces.MetadataEngine
{
    public interface IContext
    {
        /// <summary>
        /// Should the system tables, views, etc be included when calling the plugin for MetaData?
        /// </summary>
        bool IncludeSystemEntities { get; }

        /// <summary>
        /// Is the DatabaseName required to make a connection?
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Is the DatabaseName required to make a connection?
        /// </summary>
        string ConnectionString { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        DataTable CreateDatabasesDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        DataTable CreateForeignKeysDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        DataTable CreateTablesDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		DataTable CreateViewsDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		DataTable CreateColumnsDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		DataTable CreateIndexesDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		DataTable CreateProceduresDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		DataTable CreateParametersDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		DataTable CreateResultColumnsDataTable();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		DataTable CreateDomainsDataTable();
    }
}
