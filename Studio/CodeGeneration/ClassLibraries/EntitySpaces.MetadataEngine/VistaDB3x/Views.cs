using System;
using System.Data;

namespace MyMeta.VistaDB3x
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IViews))]
#endif 
	public class VistaDBViews : Views
	{
		public VistaDBViews()
		{

		}

		override internal void LoadAll()
		{
			try
			{
//				string type = this.dbRoot.ShowSystemData ? "SYSTEM VIEW" : "VIEW";
//				DataTable metaData = this.LoadData(OleDbSchemaGuid.Tables, new Object[] {null, null, null, type});
//
//				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
