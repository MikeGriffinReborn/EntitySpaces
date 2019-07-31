using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class OracleForeignKeys : ForeignKeys
	{
		public OracleForeignKeys()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData1 = this.LoadData(OleDbSchemaGuid.Foreign_Keys, 
					new object[]{null, this.Table.Database.Name, this.Table.Name});

				DataTable metaData2 = this.LoadData(OleDbSchemaGuid.Foreign_Keys, 
					new object[]{null, null, null, null, this.Table.Database.Name, this.Table.Name});

				DataRowCollection rows = metaData2.Rows;
				int count = rows.Count;
				for(int i = 0; i < count; i++)
				{
        			metaData1.ImportRow(rows[i]);
				}

				PopulateArray(metaData1);
			}
			catch {}
		}
	}
}
