using System;
using System.Data;
using System.Data.Common;

namespace EntitySpaces.MetadataEngine.MySql
{
	public class MySqlViews : Views
	{
		public MySqlViews()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				MySqlDatabases db = this.Database.Databases as MySqlDatabases;
				if(db.Version.StartsWith("5"))
				{
					string query = @"SHOW FULL TABLES WHERE Table_type = 'VIEW'";

					DataTable metaData = new DataTable();
					DbDataAdapter adapter = MySqlDatabases.CreateAdapter(query, this.dbRoot.ConnectionString);

					adapter.Fill(metaData);

					metaData.Columns[0].ColumnName = "TABLE_NAME";

					PopulateArray(metaData);
				}
			}
			catch {}
		}
	}
}
