using System;
using System.Data;
using System.Reflection;

namespace MyMeta.VistaDB3x
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
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
