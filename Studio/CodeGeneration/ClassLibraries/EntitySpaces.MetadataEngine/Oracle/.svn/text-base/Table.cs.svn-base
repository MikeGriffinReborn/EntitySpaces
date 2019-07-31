using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class OracleTable : Table
	{
		public OracleTable()
		{

		}

		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					DataTable metaData = this.LoadData(OleDbSchemaGuid.Primary_Keys, 
						new Object[] {null, this.Tables.Database.Name, this.Name});

					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					Columns cols = (Columns)this.Columns;

					string colName = "";

					int count = metaData.Rows.Count;
					for(int i = 0; i < count; i++)
					{
						colName = metaData.Rows[i]["COLUMN_NAME"] as string;
						_primaryKeys.AddColumn((Column)cols.GetByPhysicalName(colName));
					}
				}

				return _primaryKeys;
			}
		}
	}
}
