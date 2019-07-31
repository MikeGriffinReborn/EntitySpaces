using System;
using System.Data;
using System.Reflection;

namespace esMetadataEngine.VistaDB
{
	public class VistaDBTable : Table
	{
		public VistaDBTable()
		{

		}

		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					foreach(IColumn col in this.Columns)
					{
						if(col.IsInPrimaryKey)
						{
							_primaryKeys.AddColumn((Column)this.Columns[col.Name]);
						}
					}
				}
				return _primaryKeys;
			}
		}
	}
}
