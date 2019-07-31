using System;
using System.Data;
using Provider.VistaDB;

namespace esMetadataEngine.VistaDB
{
	/// <summary>
	/// Summary description for MetaHelper.
	/// </summary>
	public class MetaHelper
	{
		public MetaHelper()
		{

		}

		public DataTable LoadColumns(string cn, string tableName)
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int32"));
			metaData.Columns.Add("IS_NULLABLE", Type.GetType("System.Boolean"));
			metaData.Columns.Add("COLUMN_HASDEFAULT", Type.GetType("System.Boolean"));
			metaData.Columns.Add("COLUMN_DEFAULT", Type.GetType("System.String"));
			metaData.Columns.Add("IS_AUTO_KEY", Type.GetType("System.Boolean"));
			metaData.Columns.Add("AUTO_KEY_SEED", Type.GetType("System.Int32"));
			metaData.Columns.Add("AUTO_KEY_INCREMENT", Type.GetType("System.Int32"));
			metaData.Columns.Add("DATA_TYPE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("NUMERIC_PRECISION", Type.GetType("System.Int32"));
			metaData.Columns.Add("NUMERIC_SCALE", Type.GetType("System.Int32"));
			metaData.Columns.Add("CHARACTER_MAXIMUM_LENGTH", Type.GetType("System.Int32"));
			metaData.Columns.Add("CHARACTER_OCTET_LENGTH", Type.GetType("System.Int32"));
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			metaData.Columns.Add("IS_PRIMARY_KEY", Type.GetType("System.Boolean"));

			try
			{
				Provider.VistaDB.VistaDBDatabase db = OpenDatabase(cn);
				db.Connect();

				Provider.VistaDB.VistaDBTable table = new Provider.VistaDB.VistaDBTable(db, tableName);
				table.Open();

				for(int ordinal = 0; ordinal < table.ColumnCount(); ordinal++)
				{
					Provider.VistaDB.VistaDBColumn c = table.Columns[ordinal];

					bool b = false;
					string colName = c.Name;

					string def		= table.GetDefaultValue(colName, out b);
					int width		= c.ColumnWidth;
					int dec			= c.ColumnDecimals;
					int length      = 0;
					int octLength   = width;

					if(c.Identity)
					{
						// While I'll see their point this is not typically how it is done
						def = "";
					}

					string type = c.VistaDBType.ToString();

					switch(type)
					{
						case "Character":
						case "Varchar":
							length    = width;
							width     = 0;
							dec       = 0;
							break;

						case "Currency":
						case "Double":
							break;

						default:
							width = 0;
							dec   = 0;
							break;
					}

					metaData.Rows.Add(new object[] 
					{ 
						table.TableName, 
						c.Name,
						ordinal,
						c.AllowNull,
						def == string.Empty ? false : true,
						def,
						c.Identity,
						1,
						(int)c.IdentityStep,
						c.VistaDBType.ToString(),
						width,
						dec,
						length,
						octLength,
						c.ColumnDescription,
						c.PrimaryKey
					} );
				}

				table.Close();
				db.Close();
			}
			catch {}

			return metaData;
		}

		public DataTable LoadTables(string cn)
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));

			try
			{
				Provider.VistaDB.VistaDBDatabase db = OpenDatabase(cn);
				db.Connect();

				string[] list = new string[0];
				db.EnumTables(ref list);

				if(list != null)
				{
					foreach(string tableName in list)
					{
						Provider.VistaDB.VistaDBTable table = new Provider.VistaDB.VistaDBTable(db, tableName);
						table.Open();
						metaData.Rows.Add(new object[] { table.TableName, "" } );
						table.Close();
					}
				}

				db.Close();
			}
			catch {}

			return metaData;
		}

		public DataTable LoadForeignKeys(string cn, string databaseName, string tableName)
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("PK_TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("PK_TABLE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("FK_TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("FK_TABLE_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("FK_TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("PK_TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("ORDINAL", Type.GetType("System.Int32"));
			metaData.Columns.Add("FK_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("PK_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("PK_COLUMN_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("FK_COLUMN_NAME", Type.GetType("System.String"));

			try
			{
				Provider.VistaDB.VistaDBDatabase db = OpenDatabase(cn);
				db.Connect();

				Provider.VistaDB.VistaDBTable table = new Provider.VistaDB.VistaDBTable(db, tableName);
				table.Open();

				string[] fkeys = new string[0];
				table.EnumForeignKeys(out fkeys);

				string foreignKey   = "";
				string primaryTable = "";
				string primaryKey   = "";

				if(fkeys != null)
				{
					foreach(string fkey in fkeys)
					{
						table.GetForeignKey(fkey, out foreignKey, out primaryTable, out primaryKey);

						string[] fColumns = foreignKey.Split(new char[] {';'});
						string[] pColumns = primaryKey.Split(new char[] {';'});

						for(int i = 0; i < fColumns.GetLength(0); i++)
						{
							metaData.Rows.Add(new object[] 
							{ 
								databaseName,
								DBNull.Value,
								DBNull.Value,
								DBNull.Value,
								tableName,
								primaryTable, 
								0,
								fkey,
								"PKEY",
								pColumns[i],
								fColumns[i]}
									);
							}
					}
		
					table.Close();
					db.Close();
				}
			}
			catch {}

			return metaData;
		}

		public DataTable LoadIndexes(string cn, string databaseName, string tableName)
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("INDEX_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("INDEX_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("UNIQUE", Type.GetType("System.Boolean"));
			metaData.Columns.Add("COLLATION", Type.GetType("System.Int16"));
			metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));

			try
			{
				Provider.VistaDB.VistaDBDatabase db = OpenDatabase(cn);
				db.Connect();

				Provider.VistaDB.VistaDBTable table = new Provider.VistaDB.VistaDBTable(db, tableName);
				table.Open();

				string[] indexes = new string[0];
				table.EnumIndexes(out indexes);

				bool active;
				int orderIndex;
				bool unique;
				bool primary;
				bool descending;
				string keyExp;

				if(indexes != null)
				{
					foreach(string index in indexes)
					{
						table.GetIndex(index, out active, out orderIndex, out unique, out primary, out descending, out keyExp);

						if(orderIndex != 0) // && keyExp != "PrimaryKey" && keyExp != "PRIMARY_KEY")
						{
							if(keyExp != null && keyExp != string.Empty)
							{
								string[] columns = keyExp.Split(new char[] { ';' } );

								foreach(string colName in columns)
								{
									metaData.Rows.Add(new object[] 
								{ 
									databaseName,
									tableName, 
									databaseName, 
									index,
									unique,
									descending ? 2 : 1,
									colName
								} );
								}
							}
						}
					}
				}

				table.Close();
				db.Close();
			}
			catch {}

			return metaData;
		}

		public IDbConnection GetConnection(string cn)
		{
			Provider.VistaDB.VistaDBConnection conn = new Provider.VistaDB.VistaDBConnection(cn);
			return conn;
		}

		public string LoadDatabases(string cn)
		{
			string dbName = "";

			try
			{
				Provider.VistaDB.VistaDBConnection conn = new Provider.VistaDB.VistaDBConnection(cn);
				conn.Open();
				dbName = conn.Database;
				conn.Close();

				try
				{
					int index = dbName.LastIndexOfAny(new char[]{'\\'});
					if (index >= 0)
					{
						dbName = dbName.Substring(index + 1);
					}
				}
				catch {}
			}
			catch
			{
		
			}

			return dbName;
		}

		private Provider.VistaDB.VistaDBDatabase OpenDatabase(string cn)
		{
			Provider.VistaDB.VistaDBConnection conn = new Provider.VistaDB.VistaDBConnection();
			conn.ConnectionString = cn;
			conn.ReadOnly = true;
			conn.Open();
			string dbName = conn.Database;
			conn.Close();

			return new Provider.VistaDB.VistaDBDatabase(dbName, false, true);
		}
	}
}
