using System;
using System.Data;
using System.Data.Common;

namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class PostgreSQLTables : Tables
	{
		public PostgreSQLTables()
		{

		}

		override internal void LoadAll()
		{
			try
            {
                string query = "select * from information_schema.tables where table_type = 'BASE TABLE' " +
                                      " and table_schema = '" + this.Database.SchemaName + "' ORDER BY table_name;";

				IDbConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

				DataTable metaData = new DataTable();
                DbDataAdapter adapter = PostgreSQLDatabases.CreateAdapter(query, cn);

				adapter.Fill(metaData);
				cn.Close();
		
				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
