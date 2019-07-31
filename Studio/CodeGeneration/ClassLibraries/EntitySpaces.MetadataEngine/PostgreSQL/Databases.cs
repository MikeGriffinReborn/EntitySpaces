using System;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class PostgreSQLDatabases : Databases
	{
        static internal string nameSpace = "Npgsql.";
        static internal Assembly asm = null;
        static internal Module mod = null;

        static internal ConstructorInfo IDbConnectionCtor = null;
        static internal ConstructorInfo IDbDataAdapterCtor = null;
        static internal ConstructorInfo IDbDataAdapterCtor2 = null;

        internal string Version = "";

		public PostgreSQLDatabases()
		{

		}

        static PostgreSQLDatabases()
		{
			LoadAssembly();
		}

        static public void LoadAssembly()
        {
            try
            {
                if (asm == null)
                {
                    try
                    {
                        asm = Assembly.LoadWithPartialName("Npgsql");
                        Module[] mods = asm.GetModules(false);
                        mod = mods[0];
                    }
                    catch
                    {
                        throw new Exception("Make sure the Npgsql.dll is registered in the Gac or is located in the MyGeneration folder.");
                    }
                }
            }
            catch { }
        }

		override internal void LoadAll()
		{
            string query =
                "select datname as CATALOG_NAME, s.usename as SCHEMA_OWNER, current_schema() as SCHEMA_NAME from pg_database d " +
                "INNER JOIN pg_user s on d.datdba = s.usesysid where datistemplate = 'f' ORDER BY datname";

            DbDataAdapter adapter = PostgreSQLDatabases.CreateAdapter(query, this.dbRoot.ConnectionString);
			DataTable metaData = new DataTable();

			adapter.Fill(metaData);
		
			PopulateArray(metaData);
		}

        static internal IDbConnection CreateConnection(string connStr)
        {
            if (IDbConnectionCtor == null)
            {
                Type type = mod.GetType(nameSpace + "NpgsqlConnection");

                IDbConnectionCtor = type.GetConstructor(new Type[] { typeof(string) });
            }

            object obj = IDbConnectionCtor.Invoke(BindingFlags.CreateInstance | BindingFlags.OptionalParamBinding,
                null, new object[] { connStr }, null);

            return obj as IDbConnection;
        }

        static internal DbDataAdapter CreateAdapter(string query, string connStr)
        {
            if (IDbDataAdapterCtor == null)
            {
                Type type = mod.GetType(nameSpace + "NpgsqlDataAdapter");

                IDbDataAdapterCtor = type.GetConstructor(new Type[] { typeof(string), typeof(string) });
            }

            object obj = IDbDataAdapterCtor.Invoke
                (BindingFlags.CreateInstance | BindingFlags.OptionalParamBinding, null,
                new object[] { query, connStr }, null);

            return obj as DbDataAdapter;
        }

        static internal DbDataAdapter CreateAdapter(string query, IDbConnection conn)
        {
            if (IDbDataAdapterCtor2 == null)
            {
                Type type = mod.GetType(nameSpace + "NpgsqlDataAdapter");

                ConstructorInfo[] ctrs = type.GetConstructors();

                IDbDataAdapterCtor2 = ctrs[2];
            }

            object obj = IDbDataAdapterCtor2.Invoke
                (BindingFlags.CreateInstance | BindingFlags.OptionalParamBinding, null,
                new object[] { query, conn }, null);

            return obj as DbDataAdapter;
        }
	}
}
