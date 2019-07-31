using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Access
{
	public class AccessForeignKey : ForeignKey
	{
		public AccessForeignKey()
		{

		}

		override public ITable ForeignTable
		{
			get
			{
				string catalog = this.ForeignKeys.Table.Database.Name;
				string schema  = this.GetString(ForeignKeys.f_FKTableSchema);

				return this.dbRoot.Databases[0].Tables[this.GetString(ForeignKeys.f_FKTableName)];
			}
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
