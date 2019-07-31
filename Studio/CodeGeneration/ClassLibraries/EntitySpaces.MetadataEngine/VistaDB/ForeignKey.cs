using System;
using System.Data;

namespace esMetadataEngine.VistaDB
{
	public class VistaDBForeignKey : ForeignKey
	{
		public VistaDBForeignKey()
		{

		}

		internal override void AddForeignColumn(string catalog, string schema,
			string physicalTableName, string physicalColumnName, bool primary)
		{
			Column column = this.ForeignKeys.Table.Tables[physicalTableName].Columns[physicalColumnName] as Column;

			Column c = column.Clone();

			if(primary)
			{
				if(null == _primaryColumns)
				{
					_primaryColumns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryColumns.ForeignKey = this;
				}

				_primaryColumns.AddColumn(c);
			}
			else
			{
				if(null == _foreignColumns)
				{
					_foreignColumns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_foreignColumns.ForeignKey = this;
				}

				_foreignColumns.AddColumn(c);
			}

			column.AddForeignKey(this);
		}
	}
}
