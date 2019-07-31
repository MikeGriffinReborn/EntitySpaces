using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlProcedures : Procedures
	{
		public SqlProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedures, 
					new Object[] {this.Database.Name, null, null});

				PopulateArray(metaData);

				LoadDescriptions();
			}
			catch {}
		}

		override public IProcedure this[object name]
		{
			get
			{
				return base[name];
			}
		}

		private void LoadDescriptions()
		{
			try
			{
				string select = @"SELECT objName, value FROM ::fn_listextendedproperty ('MS_Description', 'user', 'dbo', 'procedure', null, null, null)";

                DataTable dataTable = new DataTable();
                using (OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString))
                {
                    cn.Open();
                    cn.ChangeDatabase("[" + this.Database.Name + "]");

                    OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
                    adapter.Fill(dataTable);
                }

				Procedure p;

				foreach(DataRow row in dataTable.Rows)
				{
					p = this[row["objName"] as string] as Procedure;

					if(null != p)
					{
						p._row["DESCRIPTION"] = row["value"] as string;
					}
				}
			}
			catch
			{
			
			}
		}
	}
}
