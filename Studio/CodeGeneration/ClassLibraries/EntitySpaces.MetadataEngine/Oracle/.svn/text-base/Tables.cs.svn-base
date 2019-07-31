using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class OracleTables : Tables
	{
		public OracleTables()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM TABLE" : "TABLE";
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Tables, new Object[] {null, this.Database.Name, null, null, type});

				// Oracle returns VIEWS in Addition to when you ask for TABLES, however, if you just ask for VIEWS that works fine
				metaData.DefaultView.RowFilter = "TABLE_TYPE = '" + type + "'";

				base.PopulateArray(metaData);

				LoadExtraData(this.Database.SchemaName);
			}
			catch {}
		}

		private void LoadExtraData(string schema)
		{
			try
			{
				string select = "SELECT DISTINCT C.TABLE_NAME, C.COMMENTS AS DESCRIPTION FROM SYS.ALL_TAB_COMMENTS C, SYS.ALL_TABLES T " +
					"WHERE T.OWNER = '" + schema + "' AND T.OWNER = C.OWNER	AND T.TABLE_NAME = C.TABLE_NAME	AND C.COMMENTS IS NOT NULL";

				OleDbDataAdapter adapter = new OleDbDataAdapter(select, this.dbRoot.ConnectionString);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);

				DataRowCollection rows = dataTable.Rows;

				if(rows.Count > 0)
				{
					Table t;
					foreach(DataRow row in rows)
					{
						t = this[row["TABLE_NAME"]] as Table;
						t._row["DESCRIPTION"] = row["DESCRIPTION"];
					}
				}
			}
			catch {}
		}
	}
}
