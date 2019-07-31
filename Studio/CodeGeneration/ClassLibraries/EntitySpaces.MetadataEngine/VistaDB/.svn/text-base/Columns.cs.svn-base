using System;
using System.Data;
using System.Reflection;

namespace esMetadataEngine.VistaDB
{
	public class VistaDBColumns : Columns
	{
		public VistaDBColumns()
		{

		}

		internal DataColumn f_TypeName		= null;
		internal DataColumn f_InPrimaryKey	= null;

		override internal void LoadForTable()
		{
			try
			{
				VistaDBDatabase db = (VistaDBDatabase)this.Table.Database;

				DataTable metaData = db._mh.LoadColumns(this.dbRoot.ConnectionString, this.Table.Name);

				f_TypeName		= metaData.Columns["DATA_TYPE_NAME"];
				f_InPrimaryKey	= metaData.Columns["IS_PRIMARY_KEY"];

				PopulateArray(metaData);
			}
			catch(Exception ex)
			{
				string s = ex.Message;
			}
		}

		override internal void LoadForView()
		{

		}
	}
}
