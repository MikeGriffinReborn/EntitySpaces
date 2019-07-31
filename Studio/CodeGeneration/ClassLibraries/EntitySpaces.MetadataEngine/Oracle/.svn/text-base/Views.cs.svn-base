using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class OracleViews : Views
	{
		public OracleViews()
		{

		}

		internal override void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM VIEW" : "VIEW";
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Views, new Object[] {null, this.Database.Name, null, null, type});

				base.PopulateArray(metaData);

				LoadExtraData(this.Database.SchemaName);
			}
			catch {}
		}

		private void LoadExtraData(string schema)
		{
			try
			{
				string select = "SELECT DISTINCT C.TABLE_NAME, C.COMMENTS AS DESCRIPTION FROM SYS.ALL_TAB_COMMENTS C, SYS.ALL_VIEWS V " +
					"WHERE V.OWNER = '" + schema + "' AND V.OWNER = C.OWNER AND V.VIEW_NAME = C.TABLE_NAME	AND C.COMMENTS IS NOT NULL";

				OleDbDataAdapter adapter = new OleDbDataAdapter(select, this.dbRoot.ConnectionString);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);

				DataRowCollection rows = dataTable.Rows;

				if(rows.Count > 0)
				{
					View v;
					foreach(DataRow row in rows)
					{
						v = this[row["TABLE_NAME"]] as View;
						v._row["DESCRIPTION"] = row["DESCRIPTION"];
					}
				}
			}
			catch {}
		}
	}
}
