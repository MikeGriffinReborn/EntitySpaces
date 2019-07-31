using System;
using System.Data;
using System.Data.Common;

namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class PostgreSQLIndexes : Indexes
	{
		public PostgreSQLIndexes()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string select = @"SELECT current_database() as table_catalog, tab.relname AS table_name, " +
					"n.nspname as TABLE_NAMESPACE, cls.relname as INDEX_NAME, idx.indisunique as UNIQUE, " +
					"idx.indisclustered as CLUSTERED, a.amname as TYPE, indkey AS columns FROM pg_index idx " +
					"JOIN pg_class cls ON cls.oid=indexrelid " +
					"JOIN pg_class tab ON tab.oid=indrelid AND tab.relname = '" + this.Table.Name + "' " +
					"JOIN pg_namespace n ON n.oid=tab.relnamespace AND n.nspname = '" + this.Table.Schema + "' " +
					"JOIN pg_am a ON a.oid = cls.relam " +
					"LEFT JOIN pg_depend dep ON (dep.classid = cls.tableoid AND dep.objid = cls.oid AND dep.refobjsubid = '0') " +
					"LEFT OUTER JOIN pg_constraint con ON (con.tableoid = dep.refclassid AND con.oid = dep.refobjid) " +
					"WHERE con.conname IS NULL ORDER BY cls.relname;";

                IDbConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Table.Tables.Database.Name);

                DbDataAdapter adapter = PostgreSQLDatabases.CreateAdapter(select, cn);
				DataTable metaData = new DataTable();

				adapter.Fill(metaData);
				cn.Close();
		
				PopulateArrayNoHookup(metaData);

				for(int i = 0; i < this.Count; i++)
				{
					Index index = this[i] as Index;

					if(null != index)
					{
						string s = index._row["columns"] as string;
						string[] colIndexes = s.Split(' ');

						foreach(string colIndex in colIndexes)
						{
							if(colIndex != "0")
							{
								int id = Convert.ToInt32(colIndex);

								Column column  = this.Table.Columns[id-1] as Column;
								index.AddColumn(column.Name);
							}
						}
					}
				}
			}
			catch {}
		}
	}
}
