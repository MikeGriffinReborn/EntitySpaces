using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class OracleParameters : Parameters
	{
		public OracleParameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Parameters, 
					new object[]{null, this.Procedure.Database.Name, this.Procedure.Name});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
