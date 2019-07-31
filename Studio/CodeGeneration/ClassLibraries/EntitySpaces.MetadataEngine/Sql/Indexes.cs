using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlIndexes : Indexes
	{
		public SqlIndexes()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Indexes, 
					new object[]{this.Table.Database.Name, this.Table.Schema, null, null, this.Table.Name});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
