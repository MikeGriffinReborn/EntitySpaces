using System;
using System.Data;
using System.Data.Common;

namespace EntitySpaces.MetadataEngine.MySql
{
	public class MySqlIndexes : Indexes
	{
		public MySqlIndexes()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string query = @"SHOW INDEX FROM `" + this.Table.Name + "`";

				DataTable metaData = new DataTable();
				DbDataAdapter adapter = MySqlDatabases.CreateAdapter(query, this.dbRoot.ConnectionString);

				adapter.Fill(metaData);

				metaData.Columns["Key_name"].ColumnName		= "INDEX_NAME";
				metaData.Columns["Index_type"].ColumnName	= "TYPE";
				metaData.Columns["Non_unique"].ColumnName   = "UNIQUE";
			
				PopulateArray(metaData);


			}
			catch {}
		}
	}
}
