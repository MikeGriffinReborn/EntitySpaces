using System;
using System.Data;
using System.Data.Common;

namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class PostgreSQLDomains : Domains
	{
		public PostgreSQLDomains()
		{

		}

		internal DataColumn f_TypeNameComplete	= null;

		internal override void LoadAll()
		{
			string query = "select * from information_schema.domains where domain_catalog = '" + this.Database.Name + 
				"' and domain_schema = '" + this.Database.SchemaName + "'";

			IDbConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

			DataTable metaData = new DataTable();
            DbDataAdapter adapter = PostgreSQLDatabases.CreateAdapter(query, cn);

			adapter.Fill(metaData);
			cn.Close();

			metaData.Columns["udt_name"].ColumnName = "DATA_TYPE";
			metaData.Columns["data_type"].ColumnName = "TYPE_NAMECOMPLETE";

			if(metaData.Columns.Contains("TYPE_NAMECOMPLETE"))
				f_TypeNameComplete = metaData.Columns["TYPE_NAMECOMPLETE"];
		
			PopulateArray(metaData);
		}
	}
}
