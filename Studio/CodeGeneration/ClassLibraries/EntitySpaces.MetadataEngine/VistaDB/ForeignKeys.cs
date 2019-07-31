using System;
using System.Data;
using System.Reflection;

namespace esMetadataEngine.VistaDB
{
	public class VistaDBForeignKeys : ForeignKeys
	{
		public VistaDBForeignKeys()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				VistaDBDatabase db = (VistaDBDatabase)this.Table.Database;

				if(!db._FKsInLoad)
				{
					db._FKsInLoad = true;

					VistaDBForeignKeys fks = null;

					foreach(Table table in this.Table.Tables)
					{
						fks = table.ForeignKeys as VistaDBForeignKeys;
					}

					DataTable metaData = db._mh.LoadForeignKeys(this.dbRoot.ConnectionString, this.Table.Database.Name, this.Table.Name);

					PopulateArray(metaData);

					ITables tables = this.Table.Tables;
					for(int i = 0; i < tables.Count; i++)
					{
						ITable table = tables[i];
						fks = table.ForeignKeys as VistaDBForeignKeys;
						fks.AddTheOtherHalf();
					}

					db._FKsInLoad = false;
				}
				else
				{
					DataTable metaData = db._mh.LoadForeignKeys(this.dbRoot.ConnectionString, this.Table.Database.Name, this.Table.Name);

					PopulateArray(metaData);
				}
			}
			catch {}
		}

		internal void AddTheOtherHalf()
		{
			string myName = this.Table.Name;

			foreach(Table table in this.Table.Tables)
			{
				if(table.Name != myName)
				{
					foreach(VistaDBForeignKey fkey in table.ForeignKeys)
					{
						if(fkey.ForeignTable.Name == myName || fkey.PrimaryTable.Name == myName)
						{
							this.AddForeignKey(fkey);
						}
					}
				}
			}
		}

		internal void AddForeignKey(VistaDBForeignKey fkey)
		{
			if(!this._array.Contains(fkey))
			{
				this._array.Add(fkey);
			}
		}
	}
}
