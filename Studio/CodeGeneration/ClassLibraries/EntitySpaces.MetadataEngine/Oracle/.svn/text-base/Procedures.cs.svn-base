using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class OracleProcedures : Procedures
	{
		public OracleProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedures, 
					new Object[] {null, this.Database.Name});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
