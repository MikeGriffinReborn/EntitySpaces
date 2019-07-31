using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlColumns : Columns
	{
		public SqlColumns()
		{

		}

		internal DataColumn f_TypeName	= null;

		override internal void LoadForTable()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {this.Table.Database.Name, this.Table.Schema, this.Table.Name});
			PopulateArray(metaData);

			LoadExtraData(this.Table.Name, "T");
			LoadAutoKeyInfo();
			LoadDescriptions();
	   }

		override internal void LoadForView()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {this.View.Database.Name, this.View.Schema, this.View.Name});
			PopulateArray(metaData);

			LoadExtraData(this.View.Name, "V");
		}

		private void LoadExtraData(string name, string type)
		{
			try
			{
				string dbName = ("T" == type) ? this.Table.Database.Name : this.View.Database.Name;
				string schema = ("T" == type) ? this.Table.Schema : this.View.Schema;
				string select = "EXEC [" + dbName + "].dbo.sp_columns '" + name + "', '" + schema + "'";

                using (OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString))
                {
                    cn.Open();
                    cn.ChangeDatabase("[" + dbName + "]");

                    OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    if (this._array.Count > 0)
                    {
                        Column col = this._array[0] as Column;

                        f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
                        col._row.Table.Columns.Add(f_TypeName);

                        string typeName = "";
                        DataRowCollection rows = dataTable.Rows;

                        int count = this._array.Count;
                        Column c = null;

                        for (int index = 0; index < count; index++)
                        {
                            c = (Column)this[index];

                            typeName = rows[index]["TYPE_NAME"] as string;

                            if (typeName.EndsWith(" identity"))
                            {
                                typeName = typeName.Replace(" identity", "");
                                typeName = typeName.Replace("()", "");
                                c._row["TYPE_NAME"] = typeName;
                            }
                            else
                            {
                                c._row["TYPE_NAME"] = typeName;
                            }
                        }
                    }

                    select = @"select COLUMN_NAME, DATA_TYPE from 
INFORMATION_SCHEMA.COLUMNS 
where table_schema = '" + schema + @"' 
and table_catalog='" + dbName + @"' and table_name='" + name + @"' 
and DATA_TYPE in ('nvarchar', 'varchar', 'varbinary') 
and character_maximum_length=-1 and character_octet_length=-1;";

                    adapter = new OleDbDataAdapter(select, cn);
                    dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    Column colz = null;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        colz = this[row["COLUMN_NAME"] as string] as Column;

                        if (null != colz)
                        {
                            colz._row["TYPE_NAME"] = row["DATA_TYPE"] as string;
                        }
                    }

                    cn.Close();
                }
			}
			catch {}
		}

		private void LoadDescriptions()
		{
			try
			{
				string select = @"SELECT objName, value 
FROM ::fn_listextendedproperty ('MS_Description', 'user', '" + this.Table.Schema + @"', 'table', '" + this.Table.Name + @"', 'column', null)
UNION
SELECT objName, value 
FROM ::fn_listextendedproperty ('MS_Description', 'schema', '" + this.Table.Schema + @"', 'table', '" + this.Table.Name + @"', 'column', null)";
                DataTable dataTable = new DataTable();
                using (OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString))
                {
                    cn.Open();
                    cn.ChangeDatabase("[" + this.Table.Database.Name + "]");

                    OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
                    adapter.Fill(dataTable);
                }

				Column c;

				foreach(DataRow row in dataTable.Rows)
				{
					c = this[row["objName"] as string] as Column;

					if(null != c)
					{
						c._row["DESCRIPTION"] = row["value"] as string;
					}
				}
			}
			catch(Exception ex)
			{
				string s = ex.Message;
			}
		}

		private void LoadAutoKeyInfo()
		{
			try
			{
				string select =
@"SELECT TABLE_NAME, COLUMN_NAME, IDENT_SEED('[" + this.Table.Name + "]') AS AUTO_KEY_SEED, IDENT_INCR('[" + this.Table.Name + "]') AS AUTO_KEY_INCREMENT " +
"FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + this.Table.Name + "' AND TABLE_SCHEMA = '" + this.Table.Schema + "' AND " + 
"columnproperty(object_id('[" + this.Table.Schema + "].[" + this.Table.Name + "]'), COLUMN_NAME, 'IsIdentity') = 1";

				OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString);
				cn.Open();
				cn.ChangeDatabase("[" + this.Table.Database.Name + "]");

				OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);
				cn.Close();

				if(dataTable.Rows.Count > 0)
				{
					f_AutoKeySeed		= new DataColumn("AUTO_KEY_SEED", typeof(System.Int32));
					f_AutoKeyIncrement  = new DataColumn("AUTO_KEY_INCREMENT", typeof(System.Int32));
					f_IsAutoKey         = new DataColumn("AUTO_INCREMENT", typeof(Boolean));

					Column col = this._array[0] as Column;
					col._row.Table.Columns.Add(f_AutoKeySeed);
					col._row.Table.Columns.Add(f_AutoKeyIncrement);
					col._row.Table.Columns.Add(f_IsAutoKey);

					DataRowCollection rows = dataTable.Rows;
					DataRow row;

					for(int i = 0; i < rows.Count; i++)
					{
						row = rows[i];

						col = this[row["COLUMN_NAME"]] as Column;

						col._row["AUTO_KEY_SEED"]	   = row["AUTO_KEY_SEED"];
						col._row["AUTO_KEY_INCREMENT"] = row["AUTO_KEY_INCREMENT"];
						col._row["AUTO_INCREMENT"]     = true;
					}
				}
			}
			catch(Exception ex)
			{
				string s = ex.Message;
			}
		}
	}
}
