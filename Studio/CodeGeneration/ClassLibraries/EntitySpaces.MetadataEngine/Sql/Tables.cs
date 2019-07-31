using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlTables : Tables
	{
		public SqlTables()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM TABLE" : "TABLE";
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Tables, new Object[] {this.Database.Name, null, null, type});

                foreach(DataRow row in metaData.Rows)
                {
                    object o = row["TABLE_NAME"];
                    if (o != null)
                    {
                        string name = (string)o;

                        if (name.ToLower() == "sysdiagrams")
                        {
                            row.Delete();
                            break;
                        }
                    }
                }

				PopulateArray(metaData);

				LoadDescriptions();
			}
			catch {}
		}

		private void LoadDescriptions()
		{
			try
			{
				string select = @"SELECT objName, value FROM ::fn_listextendedproperty ('MS_Description', 'user', 'dbo', 'table', null, null, null)";

                DataTable dataTable = new DataTable();
                using (OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString))
                {
                    cn.Open();
                    cn.ChangeDatabase("[" + this.Database.Name + "]");

                    OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
                    adapter.Fill(dataTable);
                }

				Table t;

				foreach(DataRow row in dataTable.Rows)
				{
					t = this[row["objName"] as string] as Table;

					if(null != t)
					{
						t._row["DESCRIPTION"] = row["value"] as string;
					}
				}
			}
			catch
			{
			
			}
		}
	}
}
