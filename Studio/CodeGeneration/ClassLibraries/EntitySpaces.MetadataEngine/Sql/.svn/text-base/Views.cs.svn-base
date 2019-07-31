using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlViews : Views
	{
		public SqlViews()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM VIEW" : "VIEW";
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Tables, new Object[] {this.Database.Name, null, null, type});

				PopulateArray(metaData);

				LoadDescriptions();
			}
			catch {}
		}

		private void LoadDescriptions()
		{
			try
			{
				string select = @"SELECT objName, value FROM ::fn_listextendedproperty ('MS_Description', 'user', 'dbo', 'view', null, null, null)";

                DataTable dataTable = new DataTable();
                using (OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString))
                {
                    cn.Open();
                    cn.ChangeDatabase("[" + this.Database.Name + "]");

                    OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
                    adapter.Fill(dataTable);
                }

				View v;

				foreach(DataRow row in dataTable.Rows)
				{
					v = this[row["objName"] as string] as View;

					if(null != v)
					{
						v._row["DESCRIPTION"] = row["value"] as string;
					}
				}
			}
			catch
			{
			
			}
		}
	}
}
