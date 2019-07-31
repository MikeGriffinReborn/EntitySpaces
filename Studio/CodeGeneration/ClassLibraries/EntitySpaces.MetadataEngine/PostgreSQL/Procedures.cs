using System;
using System.Data;
using System.Data.Common;


namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class PostgreSQLProcedures : Procedures
	{
		internal string _specific_name = "";

		public PostgreSQLProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string query = "SELECT routine_definition as PROCEDURE_DEFINITION, specific_name, " +
					"routine_name as PROCEDURE_NAME, routine_schema as PROCEDURE_SCHEMA, routine_catalog as PROCEDURE_CATALOG " +
					"from information_schema.routines where routine_schema = '" + this.Database.SchemaName + 
					"' and routine_catalog = '" + this.Database.Name + "'";

				IDbConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

				DataTable metaData = new DataTable();
                DbDataAdapter adapter = PostgreSQLDatabases.CreateAdapter(query, cn);

				adapter.Fill(metaData);
				cn.Close();
		
				PopulateArray(metaData);
			}
			catch {}
		}

		override public IProcedure this[object name]
		{
			get
			{
				return base[name];
			}
		}
	}
}
