using System;
using System.Data;

namespace EntitySpaces.MetadataEngine.MySql
{
	public class MySqlParameters : Parameters
	{
		public MySqlParameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
//				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Parameters, 
//					new object[]{this.Procedure.Database.Name, null, this.Procedure.Name});
//
//				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
