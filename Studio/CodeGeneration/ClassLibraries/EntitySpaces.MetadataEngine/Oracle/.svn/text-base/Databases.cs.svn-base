using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class OracleDatabases : Databases
	{
		public OracleDatabases()
		{

		}

		override internal void LoadAll()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Schemata, new Object[] {null});

			PopulateArray(metaData);
		}

		internal override void PopulateSchemaData()
		{
			// we do nothing here
		}
	}
}
