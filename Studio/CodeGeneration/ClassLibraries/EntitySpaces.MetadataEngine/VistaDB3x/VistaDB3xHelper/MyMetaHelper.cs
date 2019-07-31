#if !IGNORE_VISTA

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using VistaDB;
using VistaDB.DDA;
using VistaDB.Provider;
using VistaDB.Diagnostic;

namespace Vista
{
	class MyMetaHelper
	{
		private static readonly IVistaDBDDA DDA = VistaDBEngine.Connections.OpenDDA();

		private string dbName = @"C:\Program Files\VistaDB 3.0 CTP\Data\Northwind.vdb3";
		private string password = "";


		public MyMetaHelper()
		{

		}

		public MyMetaHelper(string dbName, string password)
		{

		}

		public string GetDatabaseName()
		{
			string name  = "";
			int index = dbName.LastIndexOfAny(new char[]{'\\'});
			if (index >= 0)
			{
				name = dbName.Substring(index + 1);
			}
			return name;
		}


		public DataTable GetDatabases()
		{
			DataTable metaData = new DataTable();
			metaData.Columns.Add("CATALOG_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			metaData.Columns.Add("SCHEMA_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("SCHEMA_OWNER", Type.GetType("System.String"));
			metaData.Columns.Add("DEFAULT_CHARACTER_SET_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("DEFAULT_CHARACTER_SET_SCHEMA", Type.GetType("System.String"));
			metaData.Columns.Add("DEFAULT_CHARACTER_SET_NAME", Type.GetType("System.String"));

			DataRow row = metaData.NewRow();
			metaData.Rows.Add(row);

			IVistaDBDatabase db = DDA.OpenDatabase(dbName, VistaDBDatabaseOpenMode.NonexclusiveReadOnly, password);
			row["CATALOG_NAME"] = GetDatabaseName();
			row["DESCRIPTION"] = db.Description;
			
			return metaData;
		}

		public DataTable GetTables()
		{
			DataTable metaData = new DataTable();
			//metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));		
			//metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));		
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));		
			//metaData.Columns.Add("TABLE_TYPE", Type.GetType("System.String"));		
			//metaData.Columns.Add("TABLE_GUID", Type.GetType("System.String"));		
			metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));		
			//metaData.Columns.Add("TABLE_PROPID", Type.GetType("System.String"));		
			//metaData.Columns.Add("DATE_CREATED", Type.GetType("System.String"));		
			//metaData.Columns.Add("DATE_MODIFIED", Type.GetType("System.String"));		

			IVistaDBDatabase db = DDA.OpenDatabase(dbName, VistaDBDatabaseOpenMode.NonexclusiveReadOnly, "");
			ArrayList tables = db.EnumTables(); 

			foreach (string table in tables) 
			{ 
				IVistaDBTableStructure tblStructure = db.TableStructure(table);

				DataRow row = metaData.NewRow();
				metaData.Rows.Add(row);

				row["TABLE_NAME"]  = tblStructure.Name;
				row["DESCRIPTION"] = tblStructure.Description;
			}

			return metaData;
		}

		public DataTable GetColumns(string tableName)
		{
			DataTable metaData = new DataTable();
			//metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));				
			//metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));				
			//metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));					
			//metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));				
			//metaData.Columns.Add("COLUMN_GUID", Type.GetType("System.String"));				
			//metaData.Columns.Add("COLUMN_PROPID", Type.GetType("System.String"));				
			//metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.String"));			
			//metaData.Columns.Add("COLUMN_HASDEFAULT", Type.GetType("System.String"));			
			//metaData.Columns.Add("COLUMN_DEFAULT", Type.GetType("System.String"));				
			//metaData.Columns.Add("COLUMN_FLAGS", Type.GetType("System.String"));				
			//metaData.Columns.Add("IS_NULLABLE", Type.GetType("System.String"));				
			//metaData.Columns.Add("DATA_TYPE", Type.GetType("System.String"));					
			//metaData.Columns.Add("TYPE_GUID", Type.GetType("System.String"));					
			//metaData.Columns.Add("CHARACTER_MAXIMUM_LENGTH", Type.GetType("System.String"));	
			//metaData.Columns.Add("CHARACTER_OCTET_LENGTH", Type.GetType("System.String"));		
			//metaData.Columns.Add("NUMERIC_PRECISION", Type.GetType("System.String"));			
			//metaData.Columns.Add("NUMERIC_SCALE", Type.GetType("System.String"));				
			//metaData.Columns.Add("DATETIME_PRECISION", Type.GetType("System.String"));			
			//metaData.Columns.Add("CHARACTER_SET_CATALOG", Type.GetType("System.String"));		
			//metaData.Columns.Add("CHARACTER_SET_SCHEMA", Type.GetType("System.String"));		
			//metaData.Columns.Add("CHARACTER_SET_NAME", Type.GetType("System.String"));			
			//metaData.Columns.Add("COLLATION_CATALOG", Type.GetType("System.String"));			
			//metaData.Columns.Add("COLLATION_SCHEMA", Type.GetType("System.String"));			
			//metaData.Columns.Add("COLLATION_NAME", Type.GetType("System.String"));				
			//metaData.Columns.Add("DOMAIN_CATALOG", Type.GetType("System.String"));				
			//metaData.Columns.Add("DOMAIN_SCHEMA", Type.GetType("System.String"));				
			//metaData.Columns.Add("DOMAIN_NAME", Type.GetType("System.String"));				
			//metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));				
			//metaData.Columns.Add("COLUMN_LCID", Type.GetType("System.String"));				
			//metaData.Columns.Add("COLUMN_COMPFLAGS", Type.GetType("System.String"));			
			//metaData.Columns.Add("COLUMN_SORTID", Type.GetType("System.String"));				
			//metaData.Columns.Add("COLUMN_TDSCOLLATION", Type.GetType("System.String"));		
			//metaData.Columns.Add("IS_COMPUTED", Type.GetType("System.String"));				
			//metaData.Columns.Add("IS_AUTO_KEY", Type.GetType("System.String"));				
			//metaData.Columns.Add("AUTO_KEY_SEED", Type.GetType("System.String"));				
			//metaData.Columns.Add("AUTO_KEY_INCREMENT", Type.GetType("System.String"));	
		
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

			IVistaDBDatabase db = DDA.OpenDatabase(dbName, VistaDBDatabaseOpenMode.NonexclusiveReadOnly, "");
			ArrayList tables = db.EnumTables(); 

			IVistaDBTableStructure tblStructure = db.TableStructure(tableName);

			foreach (IVistaDBColumnAttributes c in tblStructure) 
			{ 
				string colName = c.Name;

				string def = "";
				if(tblStructure.Defaults.Contains(colName))
				{
					def = tblStructure.Defaults[colName].Expression;
				}
				int width		= c.MaxLength; //c.ColumnWidth;
				int dec			= 0; //c.ColumnDecimals;
				int length      = 0;
				int octLength   = width;

				IVistaDBIdentityInformation identity = null;
				if(tblStructure.Identities.Contains(colName))
				{
					identity = tblStructure.Identities[colName];
				}

				string[] pks = null;
				if(tblStructure.Indexes.Contains("PrimaryKey"))
				{
					pks = tblStructure.Indexes["PrimaryKey"].KeyExpression.Split(',');
				}
				else
				{
					foreach(IVistaDBIndexInformation pk in tblStructure.Indexes)
					{
						if(pk.Primary)
						{
							pks = pk.KeyExpression.Split(',');
							break;
						}
					}
				}

				System.Collections.Hashtable pkCols = null;
				if(pks != null)
				{
					pkCols = new Hashtable();
					foreach(string pkColName in pks)
					{
						pkCols[pkColName] = true;
					}
				}

				switch(c.Type)
				{
					case VistaDBType.Char:
					case VistaDBType.NChar:
					case VistaDBType.NText:
					case VistaDBType.NVarchar:
					case VistaDBType.Text:
					case VistaDBType.Varchar:
						length    = width;
						width     = 0;
						dec       = 0;
						break;

					case VistaDBType.Currency:
					case VistaDBType.Double:
					case VistaDBType.Decimal:
					case VistaDBType.Single:
						break;

					default:
						width = 0;
						dec   = 0;
						break;
				}

				metaData.Rows.Add(new object[] 
				{ 
					tblStructure.Name, 
					c.Name,
					c.RowIndex,
					c.AllowNull,
					def == string.Empty ? false : true,
					def,
					identity == null ? false : true,
					1,
					identity == null ? 0 : Convert.ToInt32(identity.StepExpression),
					c.Type.ToString(),
					width,
					dec,
					length,
					octLength,
					c.Description,
					pkCols == null ? false : pkCols.Contains(colName)
				} );
			} 

			return metaData;
		}

		public DataTable GetIndexes(string tableName)
		{
			DataTable metaData = new DataTable();
			//metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));		
			//metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));		
			//metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));			
			//metaData.Columns.Add("INDEX_CATALOG", Type.GetType("System.String"));		
			//metaData.Columns.Add("INDEX_SCHEMA", Type.GetType("System.String"));		
			//metaData.Columns.Add("INDEX_NAME", Type.GetType("System.String"));			
			//metaData.Columns.Add("UNIQUE", Type.GetType("System.String"));				
			//metaData.Columns.Add("CLUSTERED", Type.GetType("System.String"));			
			//metaData.Columns.Add("TYPE", Type.GetType("System.String"));				
			//metaData.Columns.Add("FILL_FACTOR", Type.GetType("System.String"));		
			//metaData.Columns.Add("INITIAL_SIZE", Type.GetType("System.String"));		
			//metaData.Columns.Add("NULLS", Type.GetType("System.String"));				
			//metaData.Columns.Add("SORT_BOOKMARKS", Type.GetType("System.String"));		
			//metaData.Columns.Add("AUTO_UPDATE", Type.GetType("System.String"));		
			//metaData.Columns.Add("NULL_COLLATION", Type.GetType("System.String"));		
			//metaData.Columns.Add("COLLATION", Type.GetType("System.String"));			
			//metaData.Columns.Add("CARDINALITY", Type.GetType("System.String"));		
			//metaData.Columns.Add("PAGES", Type.GetType("System.String"));				
			//metaData.Columns.Add("FILTER_CONDITION", Type.GetType("System.String"));	
			//metaData.Columns.Add("INTEGRATED", Type.GetType("System.String"));		
	
			metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("INDEX_CATALOG", Type.GetType("System.String"));
			metaData.Columns.Add("INDEX_NAME", Type.GetType("System.String"));
			metaData.Columns.Add("UNIQUE", Type.GetType("System.Boolean"));
			metaData.Columns.Add("COLLATION", Type.GetType("System.Int16"));
			metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));

			IVistaDBDatabase db = DDA.OpenDatabase(dbName, VistaDBDatabaseOpenMode.NonexclusiveReadOnly, "");
			ArrayList tables = db.EnumTables(); 

			IVistaDBTableStructure tblStructure = db.TableStructure(tableName);

            foreach (IVistaDBIndexInformation indexInfo in tblStructure.Indexes) 
			{ 
				string[] pks = indexInfo.KeyExpression.Split(',');

				int index = 0;
				foreach(string colName in pks)
				{
					metaData.Rows.Add(new object[] 
					{ 
						GetDatabaseName(),
						tblStructure.Name, 
						GetDatabaseName(), 
						indexInfo.Name,
						indexInfo.Unique,
						indexInfo.KeyStructure[index++].Descending ? 2 : 1,
						colName});
				}
			} 

			return metaData;
		}

		public DataTable GetForeignKeys(string tableName)
		{
			DataTable metaData = new DataTable();
			//metaData.Columns.Add("PK_TABLE_CATALOG", Type.GetType("System.String"));   
			//metaData.Columns.Add("PK_TABLE_SCHEMA", Type.GetType("System.String"));	
			//metaData.Columns.Add("PK_TABLE_NAME", Type.GetType("System.String"));		
			//metaData.Columns.Add("FK_TABLE_CATALOG", Type.GetType("System.String"));	
			//metaData.Columns.Add("FK_TABLE_SCHEMA", Type.GetType("System.String"));	
			//metaData.Columns.Add("FK_TABLE_NAME", Type.GetType("System.String"));		
			//metaData.Columns.Add("ORDINAL", Type.GetType("System.String"));			
			//metaData.Columns.Add("UPDATE_RULE", Type.GetType("System.String"));		
			//metaData.Columns.Add("DELETE_RULE", Type.GetType("System.String"));		
			//metaData.Columns.Add("PK_NAME", Type.GetType("System.String"));			
			//metaData.Columns.Add("FK_NAME", Type.GetType("System.String"));			
			//metaData.Columns.Add("DEFERRABILITY", Type.GetType("System.String"));	
	
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

			IVistaDBDatabase db = DDA.OpenDatabase(dbName, VistaDBDatabaseOpenMode.NonexclusiveReadOnly, "");
			ArrayList tables = db.EnumTables(); 

			IVistaDBTableStructure tblStructure = db.TableStructure(tableName);

            foreach (IVistaDBRelationshipInformation relInfo in tblStructure.ForeignKeys) 
			{ 
				//string[] fColumns = relInfo.ForeignKey.Split(new char[] {';'});
				//string[] pColumns = relInfo.p.ForeignKey.pprimaryKey.Split(new char[] {';'});

				//for(int i = 0; i < fColumns.GetLength(0); i++)
				{
					metaData.Rows.Add(new object[] 
					{ 
						GetDatabaseName(),
						DBNull.Value,
						DBNull.Value,
						DBNull.Value,
						tblStructure.Name,
						relInfo.PrimaryTable, 
						0,
						relInfo.Name,
						"PKEY",
						"", //pColumns[i],
						"" }); //fColumns[i]});
				}
			} 

			return metaData;
		}



		public void GetDBCompexInfo(string dbName) 
        { 
            try 
            { 
                using (IVistaDBDDA conn = VistaDBEngine.Connections.OpenDDA()) 
                { 
                    using (IVistaDBDatabase db = conn.OpenDatabase(dbName, VistaDBDatabaseOpenMode.ExclusiveReadOnly, null)) 
                    { 
                        Console.WriteLine("METAINFORMATION FOR " + dbName + " DATABASE"); 
                        Console.WriteLine("-------------------------------------------"); 
                        Console.WriteLine("Table Description: " + db.Description); 
                        Console.WriteLine("Row count: " + db.RowCount.ToString()); 
                        Console.WriteLine("PageSize:  " + db.PageSize.ToString()); 
                        Console.WriteLine("Open mode: " + db.Mode.ToString()); 
                        Console.WriteLine("Culture: " + db.Culture.ToString()); 
                        Console.WriteLine("Case Sensitive: " + db.CaseSensitive.ToString()); 

                        ArrayList tables = db.EnumTables(); 
                                    
                        foreach (string table in tables) 
                        { 
                            IVistaDBTableStructure tblStructure = db.TableStructure(table); 
                            Console.WriteLine("============================================"); 
                            Console.WriteLine("Table " + table); 
                            Console.WriteLine("============================================"); 
                            
                            //columns 
                            Console.WriteLine("COLUMNS:"); 
                            foreach (IVistaDBColumnAttributes colInfo in tblStructure) 
                            { 
                                Console.WriteLine("\t" + colInfo.Name); 
                                //use colInfo for getting columns metadata 
                            } 

                            //indexes 
                            Console.WriteLine("INDEXES:"); 
                            foreach (IVistaDBIndexInformation indexInfo in tblStructure.Indexes) 
                            { 
                                Console.WriteLine("\t" + indexInfo.Name); 
                                //use indexInfo for getting columns metadata 
                            } 

                            //constraints 
                            Console.WriteLine("CONSTRAINTS:"); 
                            foreach (IVistaDBConstraintInformation constrInfo in tblStructure.Constraints) 
                            { 
                                Console.WriteLine("\t" + constrInfo.Name); 
                                //use constrInfo for getting columns metadata 
                            } 

                            //foreignKeys 
                            Console.WriteLine("FOREIGN KEYS:"); 
                            foreach (IVistaDBRelationshipInformation relInfo in tblStructure.ForeignKeys) 
                            { 
                                Console.WriteLine("\t" + relInfo.Name); 
                                //use foreignKeys for getting columns metadata 
                            } 
                        }    
                    } 
                } 
            } 
            catch (VistaDBException ex) 
            { 
               
            } 
            catch 
            { 
               
            } 
        } 

	}
}
#endif
