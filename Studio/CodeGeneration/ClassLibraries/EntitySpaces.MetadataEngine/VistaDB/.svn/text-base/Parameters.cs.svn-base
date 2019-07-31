using System;
using System.Data;

namespace esMetadataEngine.VistaDB
{
	public class VistaDBParameters : Parameters
	{
		public VistaDBParameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
//				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Parameters, new object[]{null, null, this.Procedure.Name});
//
//				PopulateArray(metaData);
//
//				LoadExtraData();
			}
			catch {}
		}

//		private void LoadExtraData()
//		{
//			try
//			{
//				string select = "SELECT PARMNAME, TYPENAME, CODEPAGE FROM SYSCAT.PROCPARMS WHERE PROCNAME = '" + this.Procedure.Name + "' ORDER BY ORDINAL";
//
//				OleDbDataAdapter adapter = new OleDbDataAdapter(select, this.dbRoot.ConnectionString);
//				DataTable dataTable = new DataTable();
//
//				adapter.Fill(dataTable);
//
//				if(this._array.Count > 0)
//				{
//					DataRowCollection rows = dataTable.Rows;
//					string paramName = "";
//
//					int count = this._array.Count;
//					Parameter p = null;
//
//					foreach(DataRow row in rows)
//					{
//						paramName = row["PARMNAME"] as string;
//						p = this[paramName] as Parameter;
//
//						p._row["TYPE_NAME"] = (row["TYPENAME"] as string).Trim();
//
//						int codepage = -1;
//						try
//						{
//							codepage = (short)row["CODEPAGE"];
//						}
//						catch{}
//
//						if(codepage == 0)
//						{
//							// Check for "bit data"
//							switch(p.TypeName)
//							{
//								case "CHARACTER":
//								case "VARCHAR":
//								case "LONG VARCHAR":
//
//									p._row["TYPE_NAME"] = p.TypeName + " FOR BIT DATA";
//									break;
//							}
//						}
//					}
//				}
//			}
//			catch {}
//		}
	}
}
