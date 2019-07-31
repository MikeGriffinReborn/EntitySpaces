using System;
using System.Data;
using System.Data.Common;

namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class PostgreSQLTable : Table
	{
		public PostgreSQLTable()
		{

		}


		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
                    string query = "SELECT  c.column_name " +
                                   "FROM   information_schema.key_column_usage c " +
                                   "INNER JOIN  information_schema.table_constraints t " +
                                   "ON  c.constraint_name = t.constraint_name " +
                                   "WHERE  c.table_name = '" + this.Name + "' " +
                                   " AND c.table_schema = '" + this.Schema + "' " +
                                   " AND  t.constraint_type = 'PRIMARY KEY' ";

					IDbConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

					DataTable metaData = new DataTable();
                    DbDataAdapter adapter = PostgreSQLDatabases.CreateAdapter(query, cn);

					adapter.Fill(metaData);
					cn.Close();

					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					string colName = "";

					int count = metaData.Rows.Count;
					for(int i = 0; i < count; i++)
					{
						colName = metaData.Rows[i]["COLUMN_NAME"] as string;
						_primaryKeys.AddColumn((Column)this.Columns[colName]);
					}
				}

				return _primaryKeys;
			}
		}
	}
}
