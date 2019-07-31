using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlParameters : Parameters
	{
		public SqlParameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Parameters, 
					new object[]{this.Procedure.Database.Name, this.Procedure.Schema, this.Procedure.Name});

                PopulateArray(metaData);

                LoadDescriptions();
			}
			catch {}
		}

        private void LoadDescriptions()
        {
            try
            {
                string select = @"SELECT objName, value FROM ::fn_listextendedproperty (default, 'user', 'dbo', 'procedure', '" + this.Procedure.Name + "', 'parameter', default)";

                OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString);
                cn.Open();
                cn.ChangeDatabase("[" + this.Procedure.Database.Name + "]");

                OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                cn.Close();

                Parameter p;

                foreach (DataRow row in dataTable.Rows)
                {
                    p = this[row["objName"] as string] as Parameter;

                    if (null != p)
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
