using System;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace EntitySpaces.MetadataEngine.MySql
{
	public class MySqlColumns : Columns
	{
		public MySqlColumns()
		{

		}

		override internal void LoadForTable()
		{
			string query = @"SHOW COLUMNS FROM `" + this.Table.Name + "`";

			DataTable metaData = new DataTable();
			DbDataAdapter adapter = MySqlDatabases.CreateAdapter(query, this.dbRoot.ConnectionString);

			adapter.Fill(metaData);

			metaData.Columns["Field"].ColumnName   = "COLUMN_NAME";
			metaData.Columns["Type"].ColumnName    = "DATA_TYPE";
			metaData.Columns["Null"].ColumnName    = "IS_NULLABLE";
			metaData.Columns["Default"].ColumnName = "COLUMN_DEFAULT";

            if (metaData.Columns.Contains("Extra"))
            {
                if (!metaData.Columns.Contains("IS_AUTO_KEY"))
                {
                    f_IsAutoKey = metaData.Columns.Add("IS_AUTO_KEY", typeof(bool));
                }

                foreach (DataRow row in metaData.Rows)
                {
                    string extra = (string)row["extra"];

                    if (extra != null && extra.Contains("autoincrement"))
                    {
                        row["IS_AUTO_KEY"] = true;
                    }
                    else
                    {
                        row["IS_AUTO_KEY"] = false;
                    }
                }
            }
			
			PopulateArray(metaData);

			LoadTableColumnDescriptions();
		}

		override internal void LoadForView()
		{
			MySqlDatabase db   = this.View.Database as MySqlDatabase;
			MySqlDatabases dbs = db.Databases as MySqlDatabases;
			if(dbs.Version.StartsWith("5"))
			{
				string query = @"SHOW COLUMNS FROM `" + this.View.Name + "`";

				DataTable metaData = new DataTable();
				DbDataAdapter adapter = MySqlDatabases.CreateAdapter(query, this.dbRoot.ConnectionString);

				adapter.Fill(metaData);

				metaData.Columns["Field"].ColumnName   = "COLUMN_NAME";
				metaData.Columns["Type"].ColumnName    = "DATA_TYPE";
				metaData.Columns["Null"].ColumnName    = "IS_NULLABLE";
				metaData.Columns["Default"].ColumnName = "COLUMN_DEFAULT";

                if (metaData.Columns.Contains("IS_AUTO_KEY")) f_IsAutoKey = metaData.Columns["IS_AUTO_KEY"];
			
				PopulateArray(metaData);
			}
		}

		private void LoadTableColumnDescriptions()
		{
			try
			{
				string query = @"SELECT TABLE_NAME, COLUMN_NAME, COLUMN_COMMENT FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = '" + 
					this.Table.Database.Name + "' AND TABLE_NAME ='" + this.Table.Name + "'";

				DataTable metaData = new DataTable();
				DbDataAdapter adapter = MySqlDatabases.CreateAdapter(query, this.dbRoot.ConnectionString);

				adapter.Fill(metaData);

				if(metaData.Rows.Count > 0)
				{
					foreach(DataRow row in metaData.Rows)
					{
						Column c = this[row["COLUMN_NAME"] as string] as Column;

						if(!c._row.Table.Columns.Contains("DESCRIPTION"))
						{
							c._row.Table.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
							this.f_Description = c._row.Table.Columns["DESCRIPTION"];
						}

                        c._row["DESCRIPTION"] = row["COLUMN_COMMENT"] as string;

                        // We now set the AutoKey flag here ...
                        string extra = (string)c._row["Extra"];

                        if(extra != null && extra.Length > 0)
                        {
                            if (-1 != extra.IndexOf("auto_increment"))
                            {
                                c._row["IS_AUTO_KEY"] = true;
                            }
                        }
					}
				}
			}
			catch {}
		}
	}
}
