using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;

using EntitySpaces.MetadataEngine;

using iAnywhere.Data.SQLAnywhere;

namespace MyMeta.Plugins
{
    public class SybasePlugin : IPlugin
    {
        static private AppDomain _appDomain;

        #region IPlugin Interface

        private IContext context;

        bool IPlugin.OnLoad()
        {
            return true;
            //_appDomain = AppDomain.CreateDomain("EntitySpaces.MetadataEngine.Sybase");
            //return _appDomain != null;
        }

        void IPlugin.OnUnload()
        {
            //if (_appDomain != null)
            //{
            //    AppDomain.Unload(_appDomain);
            //    _appDomain = null;
            //}
        }

        void IPlugin.Initialize(IContext context)
        {
            this.context = context;
        }

        string IPlugin.ProviderName
        {
            get { return @"Sybase"; }
        }

        string IPlugin.ProviderUniqueKey
        {
            get { return @"SYBASE"; }
        }

        string IPlugin.ProviderAuthorInfo
        {
            get { return @"Enitityspaces"; }
        }

        Uri IPlugin.ProviderAuthorUri
        {
            get { return new Uri(@"http://www.entityspaces.net/"); }
        }

        bool IPlugin.StripTrailingNulls
        {
            get { return false; }
        }

        bool IPlugin.RequiredDatabaseName
        {
            get { return false; }
        }

        string IPlugin.SampleConnectionString
        {
            get { return @"ENG=demo11;UID=dba;PWD=sql;DBN=demo"; }
        }

        IDbConnection IPlugin.NewConnection
        {
            get
            {
                if (IsIntialized)
                {
                    SAConnection cn = new SAConnection(this.context.ConnectionString);
                    cn.Open();
                    return cn;
                }
                else
                    return null;
            }
        }


        string IPlugin.DefaultDatabase
        {
            get
            {
                SAConnection cn = new SAConnection(this.context.ConnectionString);
                return cn.Database;
            }
        }

        DataTable IPlugin.Databases
        {
            get
            {
                DataTable metaData = new DataTable();

                try
                {
                    metaData = context.CreateDatabasesDataTable();

                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    row["CATALOG_NAME"] = GetDatabaseName();
                  //row["DESCRIPTION"]  = db.Description;
                }
                finally
                {

                }

                return metaData;
            }
        }

        DataTable IPlugin.GetTables(string database)
        {
            DataTable metaData = new DataTable();

            try
            {
                using (SAConnection cn = new SAConnection(this.context.ConnectionString))
                {
                    DataTable dt = cn.GetSchema("Tables", new string[] { null, null, "BASE" });

                    metaData = context.CreateTablesDataTable();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = metaData.NewRow();
                        metaData.Rows.Add(row);

                        DataRow dtRow = dt.Rows[i];

                        row["TABLE_CATALOG"] = cn.Database;
                        row["TABLE_SCHEMA"] = dtRow["TABLE_SCHEMA"];
                        row["TABLE_NAME"] = dtRow["TABLE_NAME"];
                    }
                }
            }
            catch { }

            return metaData;
        }

        DataTable IPlugin.GetViews(string database)
        {
            DataTable metaData = new DataTable();

            try
            {
                using (SAConnection cn = new SAConnection(this.context.ConnectionString))
                {
                    DataTable dt = cn.GetSchema("Tables", new string[] { null, null, "VIEW" });

                    metaData = context.CreateTablesDataTable();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = metaData.NewRow();
                        metaData.Rows.Add(row);

                        DataRow dtRow = dt.Rows[i];

                        row["TABLE_CATALOG"] = cn.Database;
                        row["TABLE_SCHEMA"] = dtRow["TABLE_SCHEMA"];
                        row["TABLE_NAME"] = dtRow["TABLE_NAME"];
                    }
                }
            }
            catch { }

            return metaData;
        }

        DataTable IPlugin.GetProcedures(string database)
        {
            return new DataTable();
        }

        DataTable IPlugin.GetDomains(string database)
        {
            return new DataTable();
        }

        DataTable IPlugin.GetProcedureParameters(string database, string procedure)
        {
            return new DataTable();
        }

        DataTable IPlugin.GetProcedureResultColumns(string database, string procedure)
        {
            return new DataTable();
        }

        DataTable IPlugin.GetViewColumns(string database, string view)
        {
            return ((IPlugin)this).GetTableColumns(database, view);
        }

        DataTable IPlugin.GetTableColumns(string database, string table)
        {
            DataTable metaData = new DataTable();

            try
            {
                using (SAConnection cn = new SAConnection(this.context.ConnectionString))
                {
                    DataTable dt = cn.GetSchema("Columns", new string[] { null, table, null });

                    metaData = context.CreateColumnsDataTable();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = metaData.NewRow();
                        metaData.Rows.Add(row);

                        DataRow dtRow = dt.Rows[i];

                        row["TABLE_CATALOG"] = cn.Database;
                        row["TABLE_SCHEMA"] = dtRow["TABLE_SCHEMA"];
                        row["TABLE_NAME"] = dtRow["TABLE_NAME"];
                        row["COLUMN_NAME"] = dtRow["COLUMN_NAME"];
                        row["ORDINAL_POSITION"] = dtRow["ORDINAL_POSITION"];
                        row["TYPE_NAME"] = dtRow["DATA_TYPE"];

                        if (dtRow["IS_NULLABLE"] != DBNull.Value)
                        {
                            string isNullable = dtRow["IS_NULLABLE"].ToString().ToUpper();
                            row["IS_NULLABLE"] = (isNullable == "N") ? false : true;
                        }

                        if (dtRow["COLUMN_DEFAULT"] != DBNull.Value)
                        {
                            row["COLUMN_HASDEFAULT"] = true;
                            row["COLUMN_DEFAULT"] = dtRow["COLUMN_DEFAULT"];
                        }
                        else
                        {
                            row["COLUMN_HASDEFAULT"] = false;
                        }

                        if (row["COLUMN_DEFAULT"] != DBNull.Value)
                        {
                            switch(row["COLUMN_DEFAULT"].ToString().ToLower())
                            {
                                case "autoincrement":

                                    row["IS_AUTO_KEY"] = true;
                                    row["AUTO_KEY_SEED"] = 0;
                                    row["AUTO_KEY_INCREMENT"] = 1;
                                    break;

                                case "current timestamp":

                                    row["IS_COMPUTED"] = true;
                                    break;

                                case "timestamp":

                                    row["IS_COMPUTED"] = true;
                                    row["IS_CONCURRENCY"] = true;
                                    break;
                            }
                        }

                        long charMax = 0;
                        int precision = 0;
                        short scale = 0;

                        if (dt.Columns.Contains("CHARACTER_MAXIMUM_LENGTH") && dtRow["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                        {
                            charMax = Convert.ToInt64(dtRow["CHARACTER_MAXIMUM_LENGTH"]);
                        }

                        if (dt.Columns.Contains("PRECISION") && dtRow["PRECISION"] != DBNull.Value)
                        {
                            precision = Convert.ToInt32(dtRow["PRECISION"]);
                        }

                        if (dt.Columns.Contains("SCALE") && dtRow["SCALE"] != DBNull.Value)
                        {
                            scale = Convert.ToInt16(dtRow["SCALE"]);
                        }

                        string datatype = (string)row["TYPE_NAME"];

                        switch (datatype)
                        {
                            case "decimal":
                            case "money":
                            case "numeric":
                            case "smallmoney":

                                row["NUMERIC_PRECISION"] = precision;
                                row["NUMERIC_SCALE"] = scale;
                                row["CHARACTER_MAXIMUM_LENGTH"] = 0;
                                row["TYPE_NAME_COMPLETE"] = datatype + "(" + precision.ToString() + "," + scale.ToString() + ")";
                                break;

                            case "char":
                            case "nchar":
                            case "ntext":
                            case "nvarchar":
                            case "sysname":
                            case "text":
                            case "uniqueIdentifierstr":
                            case "varchar":
                            case "xml":

                                row["NUMERIC_PRECISION"] = 0;
                                row["NUMERIC_SCALE"] = 0;
                                row["CHARACTER_MAXIMUM_LENGTH"] = charMax;
                                row["TYPE_NAME_COMPLETE"] = datatype + "(" + charMax.ToString() + ")";
                                break;

                            default:

                                row["NUMERIC_PRECISION"] = 0;
                                row["NUMERIC_SCALE"] = 0;
                                row["CHARACTER_MAXIMUM_LENGTH"] = 0;
                                row["TYPE_NAME_COMPLETE"] = datatype;
                                break;
                        }
                    }
                }
            }
            catch { }

            return metaData;
        }

        List<string> IPlugin.GetPrimaryKeyColumns(string database, string table)
        {
            IDataReader reader = null;
            List<string> primaryKeys = new List<string>();

            try
            {
                using (SAConnection cn = new SAConnection(this.context.ConnectionString))
                {
                    DataTable theTable = cn.GetSchema("Tables", new string[] { null, table, "BASE" });

                    string schema = "DBA";

                    if (theTable != null && theTable.Rows.Count == 1)
                    {
                        schema = (string)theTable.Rows[0]["TABLE_SCHEMA"];
                    }

                    string query = @"select cname from sys.syscolumns WHERE creator = '{0}' AND tname = '{1}' AND in_primary_key = 'Y' ORDER BY TNAME, COLNO";
                    query = string.Format(query, schema, table);

                    using (SACommand cmd = new SACommand(query, cn))
                    {
                        cn.Open();
                        reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        
                        while (reader.Read())
                        {
                            primaryKeys.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return primaryKeys;
        }

        List<string> IPlugin.GetViewSubViews(string database, string view)
        {
            return new List<string>();
        }

        List<string> IPlugin.GetViewSubTables(string database, string view)
        {
            return new List<string>();
        }

        DataTable IPlugin.GetTableIndexes(string database, string table)
        {
            DataTable metaData = new DataTable();

            try
            {
                using (SAConnection cn = new SAConnection(this.context.ConnectionString))
                {
                    DataTable dt = new DataTable();

                    DataTable theTable = cn.GetSchema("Tables", new string[] { null, table, "BASE" });

                    string schema = "DBA";

                    if (theTable != null && theTable.Rows.Count == 1)
                    {
                        schema = (string)theTable.Rows[0]["TABLE_SCHEMA"];
                    }

                    string query = "select * from sys.sysindexes where creator = '{0}' and indextype <> 'Foreign Key' and tname = '{0}'";
                    query = string.Format(query, schema, table);
                    SADataAdapter ad = new SADataAdapter(query, cn);

                    ad.Fill(dt);

                    metaData = context.CreateIndexesDataTable();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dtRow = dt.Rows[i];

                        string cols = dtRow["colnames"].ToString();
                        string[] columns = cols.Split(',');

                        foreach (string column in columns)
                        {
                            DataRow row = metaData.NewRow();
                            metaData.Rows.Add(row);

                            row["TABLE_NAME"] = table;
                            row["INDEX_NAME"] = dtRow["iname"];
                            row["PRIMARY_KEY"] = false;

                            switch(dtRow["indextype"].ToString().ToLower())
                            {
                                case "primary key":

                                    row["PRIMARY_KEY"] = true;
                                    row["UNIQUE"] = true;
                                    break;

                                case "non-unique":

                                    row["UNIQUE"] = false;
                                    break;

                                case "unique":
                                    row["UNIQUE"] = true;
                                    break;
                            }

                            string[] columnData = column.Split(' ');
                            row["COLUMN_NAME"] = columnData[0];

                            if (columnData[1] == "ASC")
                            {
                                row["COLLATION"] = 1;
                            }
                            else
                            {
                                row["COLLATION"] = 2;
                            }
                        }
                    }
                }
            }
            catch { }

            return metaData;
        }

        DataTable IPlugin.GetForeignKeys(string database, string table)
        {
            DataTable metaData = new DataTable();

            try
            {
                using (SAConnection cn = new SAConnection(this.context.ConnectionString))
                {
                    DataTable theTable = cn.GetSchema("Tables", new string[] { null, table, "BASE" });

                    DataTable fks = cn.GetSchema(iAnywhere.Data.SQLAnywhere.SAMetaDataCollectionNames.MetaDataCollections);//, new string[] { table });

                    string schema = "DBA";

                    if (theTable != null && theTable.Rows.Count == 1)
                    {
                        schema = (string)theTable.Rows[0]["TABLE_SCHEMA"];
                    }

                    DataTable dt = new DataTable();

                    string query = "select * from sys.sysforeignkeys where primary_creator = '{0}' and (primary_tname = '{1}' OR foreign_tname = '{2}')";
                    query = string.Format(query, schema, table, table);
                    SADataAdapter ad = new SADataAdapter(query, cn);

                    ad.Fill(dt);

                    metaData = context.CreateForeignKeysDataTable();

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        string cols = (string)dtRow["columns"];
                        cols = cols.Replace(" IS ", ";");
                        string[] fkColumns = cols.Split(',');

                        foreach (string fkCol in fkColumns)
                        {
                            if (fkCol.Length == 0) break;

                            string[] fkCols = fkCol.Split(';');

                            DataRow row = metaData.NewRow();
                            metaData.Rows.Add(row);

                            row["FK_NAME"] = dtRow["role"];
                            row["PK_NAME"] = "Primary Key";

                            row["PK_TABLE_CATALOG"] = cn.Database;
                            row["PK_TABLE_SCHEMA"] = dtRow["primary_creator"];
                            row["PK_TABLE_NAME"] = dtRow["primary_tname"];

                            row["FK_TABLE_CATALOG"] = cn.Database;
                            row["FK_TABLE_SCHEMA"] = dtRow["foreign_creator"];
                            row["FK_TABLE_NAME"] = dtRow["foreign_tname"];

                            row["FK_COLUMN_NAME"] = fkCols[0];
                            row["PK_COLUMN_NAME"] = fkCols[1];

                            string pkQuery = "select iname from sys.sysindexes where creator = '{0}' and indextype = 'Primary key' and tname = '{1}'";
                            pkQuery = string.Format(pkQuery, schema, dtRow["primary_tname"]);

                            cn.Open();
                            using (SACommand pkCmd = new SACommand(pkQuery, cn))
                            {
                                row["PK_NAME"] = (string)pkCmd.ExecuteScalar();
                                cn.Close();
                            }
                        }
                    }
                }
            }
            catch { }

            return metaData;
        }

        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            return null;
        }

        #endregion

        #region Internal Methods

        private bool IsIntialized
        {
            get
            {
                return (context != null);
            }
        }

        public string GetDatabaseName()
        {
            IDbConnection cn = SybasePlugin.CreateConnection(context.ConnectionString);
            string dbName = cn.Database;

            int index = dbName.LastIndexOfAny(new char[] { '\\' });
            if (index >= 0)
            {
                dbName = dbName.Substring(index + 1);
            }

            return dbName;
        }

        public string GetFullDatabaseName()
        {
            IDbConnection cn = SybasePlugin.CreateConnection(context.ConnectionString);
            return cn.Database;
        }

        #endregion

        #region Other Methods

        private string GetDataTypeNameComplete(string dataType, int charMax, short precision, short scale)
        {
            switch (dataType)
            {
                case "binary":
                case "char":
                case "nchar":
                case "nvarchar":
                case "varchar":
                case "varbinary":
                    return dataType + "(" + charMax + ")";

                case "decimal":
                case "numeric":
                    return dataType + "(" + precision + "," + scale + ")";

                default:
                    return dataType;
            }
        }

        #endregion

        static internal string nameSpace = "iAnywhere.Data.SQLAnywhere.";

        #region Domain/Reflection

        static internal IDbConnection CreateConnection(string connStr)
        {
            return new SAConnection(connStr);

            //Assembly asm = Assembly.LoadWithPartialName("iAnywhere.Data.SQLAnywhere");

            //IDbConnection cn = _appDomain.CreateInstanceAndUnwrap
            //(
            //    "iAnywhere.Data.SQLAnywhere," + GetAssemblyVersion(connStr),
            //    "iAnywhere.Data.SQLAnywhere.SAConnection",
            //    false,
            //    BindingFlags.Default,
            //    null,
            //    new object[] { connStr }, //GetConnectionString(connStr) },
            //    null,
            //    null,
            //    null
            //) as IDbConnection;

            //return cn;
        }

        static internal DbDataAdapter CreateAdapter(string query, string connStr)
        {
            return new SADataAdapter(query, connStr);
            //DbDataAdapter adapter = _appDomain.CreateInstanceAndUnwrap
            //(
            //    "iAnywhere.Data.SQLAnywhere," + GetAssemblyVersion(connStr),
            //    "iAnywhere.Data.SQLAnywhere.SAAdapter",
            //    false,
            //    BindingFlags.OptionalParamBinding,
            //    null,
            //    new object[] { query, GetConnectionString(connStr) },
            //    null,
            //    null,
            //    null
            //) as DbDataAdapter;

            //return adapter;
        }

        static internal IDbCommand CreateCommand(string commandText, string connStr)
        {
            SAConnection cn = new SAConnection(connStr);
            return new SACommand(commandText, cn);

            //IDbCommand cmd = _appDomain.CreateInstanceAndUnwrap
            //(
            //    "iAnywhere.Data.SQLAnywhere," + GetAssemblyVersion(connStr),
            //    "iAnywhere.Data.SQLAnywhere.SACommand",
            //    false,
            //    BindingFlags.OptionalParamBinding,
            //    null,
            //    new object[] { commandText },
            //    null,
            //    null,
            //    null
            //) as IDbCommand;

            //IDbConnection cn = SybasePlugin.CreateConnection(connStr);

            //cmd.Connection = cn;
            //cn.Open();

            //return cmd;
        }

        // "System.Data.SqlServerCe, Version=3.5.1.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91",
        static public string GetAssemblyVersion(string connection)
        {
            try
            {
                string[] connInfo = connection.Split(';');

                foreach (string entry in connInfo)
                {
                    string[] parts = entry.Split('=');

                    if (parts[0].ToLower() == "version")
                    {
                        return entry;
                    }
                }
            }
            catch { }

            return "";
        }

        static internal string GetConnectionString(string connectionString)
        {
            string[] connInfo = connectionString.Split(';');

            foreach (string entry in connInfo)
            {
                string[] parts = entry.Split('=');

                if (parts[0].ToLower() == "data source")
                {
                    connectionString = entry;
                    break;
                }
            }

            return connectionString;
        }

        #endregion
    }
}
