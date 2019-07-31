using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Access
{
	public class AccessViews : Views
	{
		public AccessViews()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM VIEW" : "VIEW";
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Tables, new Object[] {null, null, null, type});

//				DataTable metaData = this.LoadData(OleDbSchemaGuid.Views, null, null, null, type);


				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
