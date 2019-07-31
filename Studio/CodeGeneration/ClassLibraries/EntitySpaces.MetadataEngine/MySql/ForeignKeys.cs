using System;
using System.Data;
using System.Data.Common;

namespace EntitySpaces.MetadataEngine.MySql
{
	public class MySqlForeignKeys : ForeignKeys
	{
		public MySqlForeignKeys()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				MySqlDatabase db = (MySqlDatabase)this.Table.Database;

				if(!db._FKsInLoad)
				{
					db._FKsInLoad = true;

					MySqlForeignKeys fks = null;

					foreach(Table table in this.Table.Tables)
					{
						fks = table.ForeignKeys as MySqlForeignKeys;
					}

					AddMyHalf();

					foreach(Table table in this.Table.Tables)
					{
						fks = table.ForeignKeys as MySqlForeignKeys;
						fks.AddTheOtherHalf();
					}

					AddTheOtherHalf();

					db._FKsInLoad = false;
				}
				else
				{
					AddMyHalf();
				}
			}
			catch {}
		}

		internal void AddMyHalf()
		{
			string query = @"SHOW CREATE TABLE `" + this.Table.Name + "`";

			DataTable dt = new DataTable();
			DbDataAdapter adapter = MySqlDatabases.CreateAdapter(query, this.dbRoot.ConnectionString);

			adapter.Fill(dt);

			string text = dt.Rows[0][1] as string;

			// CONSTRAINT `FK_mastertypes_3` FOREIGN KEY (`TheINT1`, `TheINT2`) REFERENCES `employee` (`TheInt1`, `TheInt2`),
			// CONSTRAINT `FK_mastertypes_1` FOREIGN KEY (`MyPK`) REFERENCES `employee` (`EmployeeID`),
			// CONSTRAINT `FK_mastertypes_2` FOREIGN KEY (`TheVARCHAR`) REFERENCES `employee` (`LastName`)
			// CONSTRAINT `ShippersOrders` FOREIGN KEY (`ShipVia`) REFERENCES `shippers` (`ShipperID`) ON DELETE NO ACTION ON UPDATE NO ACTION\n)

			DataTable metaData = new DataTable();
			metaData.Columns.Add("PK_TABLE_CATALOG");
			metaData.Columns.Add("PK_TABLE_SCHEMA");
			metaData.Columns.Add("PK_TABLE_NAME");	
			metaData.Columns.Add("FK_TABLE_CATALOG");
			metaData.Columns.Add("FK_TABLE_SCHEMA");
			metaData.Columns.Add("FK_TABLE_NAME");	
			metaData.Columns.Add("ORDINAL");		
			metaData.Columns.Add("UPDATE_RULE");	
			metaData.Columns.Add("DELETE_RULE");	
			metaData.Columns.Add("PK_NAME");		
			metaData.Columns.Add("FK_NAME");		
			metaData.Columns.Add("DEFERRABILITY");	
			metaData.Columns.Add("PK_COLUMN_NAME"); 
			metaData.Columns.Add("FK_COLUMN_NAME");

			string s = "";
			string[] fkRec = null;
			string[] pkColumns = null;
			string[] fkColumns = null;

			int iStart = 0;

			while(true)
			{
				iStart = text.IndexOf("CONSTRAINT", iStart);
				if(iStart == -1) break;
				int iEnd   = text.IndexOf('\n', iStart);

				string fk = text.Substring(iStart, iEnd - iStart);

				iStart = iEnd + 2;

				if(-1 != fk.IndexOf("FOREIGN KEY"))
				{
					// MySQL 5.0 trick !!
					int index = fk.IndexOf(")");
					index = fk.IndexOf(")", index + 1);
					s = fk.Substring(0, index);
					//


					// Munge it down it a record I can split with a ',' seperator
					s = s.Replace("`", "");
					s = s.Replace(" ", "");
					s = s.Replace("),", "");
					s = s.Replace(",", "|");
					s = s.Replace("CONSTRAINT", "");
					s = s.Replace("FOREIGNKEY", "");
					s = s.Replace("REFERENCES", ",");
					s = s.Replace("(", ",");
					s = s.Replace(")", "");

					fkRec = s.Split(',');

					fkColumns = fkRec[1].Split('|');
					pkColumns = fkRec[3].Split('|');

					for(int i = 0; i < pkColumns.Length; i++)
					{
						DataRow row = metaData.NewRow();
						metaData.Rows.Add(row);

						row["PK_TABLE_CATALOG"] = this.Table.Database.Name;
						row["FK_TABLE_CATALOG"] = this.Table.Database.Name;

						row["PK_TABLE_NAME"] = fkRec[2];
						row["FK_TABLE_NAME"] = this.Table.Name;
						row["FK_NAME"] = fkRec[0];
						row["PK_COLUMN_NAME"] = pkColumns[i];
						row["FK_COLUMN_NAME"] = fkColumns[i];

						row["ORDINAL"] = i;

						//  ON DELETE NO ACTION ON UPDATE NO ACTION\n)
						try
						{
							row["DELETE_RULE"] = "RESTRICT";
							int ond = fk.IndexOf("ON DELETE");
							if(-1 != ond)
							{
								char c = fk[ond + 10];

								switch(c)
								{
									case 'R':
									case 'r':

										row["DELETE_RULE"] = "RESTRICT";
										break;

									case 'C':
									case 'c':

										row["DELETE_RULE"] = "CASCADE";
										break;

									case 'S':
									case 's':

										row["DELETE_RULE"] = "SET NULL";
										break;

									case 'N':
									case 'n':

										row["DELETE_RULE"] = "NO ACTION";
										break;
								}
							}


							row["UPDATE_RULE"] = "RESTRICT";
							int onu = fk.IndexOf("ON UPDATE");
							if(-1 != onu)
							{
								char c = fk[onu + 10];

								switch(c)
								{
									case 'R':
									case 'r':

										row["UPDATE_RULE"] = "RESTRICT";
										break;

									case 'C':
									case 'c':

										row["UPDATE_RULE"] = "CASCADE";
										break;

									case 'S':
									case 's':

										row["UPDATE_RULE"] = "SET NULL";
										break;

									case 'N':
									case 'n':

										row["UPDATE_RULE"] = "NO ACTION";
										break;
								}
							}
						}
						catch {}

					}
				}
			}

			PopulateArray(metaData);
		}

		internal void AddTheOtherHalf()
		{
			string myName = this.Table.Name;

			foreach(Table table in this.Table.Tables)
			{
				if(table.Name != myName)
				{
					foreach(MySqlForeignKey fkey in table.ForeignKeys)
					{
						if(fkey.ForeignTable.Name == myName || fkey.PrimaryTable.Name == myName)
						{
							this.AddForeignKey(fkey);
						}
					}
				}
			}
		}
	}
}
