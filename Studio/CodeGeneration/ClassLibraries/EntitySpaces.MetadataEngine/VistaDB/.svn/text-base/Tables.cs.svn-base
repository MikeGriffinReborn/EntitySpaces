using System;
using System.Data;
using System.Reflection;


namespace esMetadataEngine.VistaDB
{
	public class VistaDBTables : Tables
	{
		public VistaDBTables()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				VistaDBDatabase db = (VistaDBDatabase)this.Database;

				DataTable metaData = db._mh.LoadTables(this.dbRoot.ConnectionString);

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
