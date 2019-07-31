using System;
using System.Data;
using System.Data.Common;

namespace EntitySpaces.MetadataEngine.MySql
{
	public class MySqlTable : Table
	{
		public MySqlTable()
		{

		}

		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					string query = @"SHOW INDEX FROM `" + this.Name + "`";

					DataTable metaData = new DataTable();
					DbDataAdapter adapter = MySqlDatabases.CreateAdapter(query, this.dbRoot.ConnectionString);

					adapter.Fill(metaData);

					DataRowCollection rows = metaData.Rows;

					string s = "";
					for(int i = 0; i < rows.Count; i++)
					{
						s = rows[i]["Key_Name"] as string;

						if(s == "PRIMARY")
						{
							s = metaData.Rows[i]["Column_name"] as string;
							_primaryKeys.AddColumn((Column)this.Columns[s]);
						}
					}
				}

				return _primaryKeys;
			}
		}

//		public override IColumns PrimaryKeys
//		{
//			get
//			{
//				if(null == _primaryKeys)
//				{
//					OleDbConnection cn = new OleDbConnection(this.dbRoot.ConnectionString); 
//					cn.Open(); 
//					DataTable metaData = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, 
//						new Object[] {null, this.Tables.Database.Name, this.Name});
//					cn.Close();
//
//					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
//
//					Columns cols = (Columns)this.Columns;
//
//					string colName = "";
//
//					int count = metaData.Rows.Count;
//					for(int i = 0; i < count; i++)
//					{
//						colName = metaData.Rows[i]["COLUMN_NAME"] as string;
//						_primaryKeys.AddColumn((Column)cols[colName]);
//					}
//				}
//
//				return _primaryKeys;
//			}
//		}
	}
}

