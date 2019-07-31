using System;
using System.Data;
using System.Reflection;

namespace esMetadataEngine.VistaDB
{
	public class VistaDBDatabases : Databases
	{
		public VistaDBDatabases()
		{

		}

		internal override void LoadAll()
		{
			try
			{
				MetaHelper mh = new MetaHelper();
				string dbName = mh.LoadDatabases(this.dbRoot.ConnectionString);

				VistaDBDatabase database = (VistaDBDatabase)this.dbRoot.ClassFactory.CreateDatabase();

				database._name = dbName;
				database.dbRoot = this.dbRoot;
				database.Databases = this;

				this._array.Add(database);
			}
			catch {}
		}
	}
}
