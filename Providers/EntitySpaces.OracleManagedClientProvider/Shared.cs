using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;

namespace EntitySpaces.OracleManagedClientProvider
{
    class Shared
    {
        static public OracleCommand BuildDynamicInsertCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, OracleParameter> types = Cache.GetParameters(request);

            OracleCommand cmd = new OracleCommand();
            cmd.BindByName = true;
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            string comma = String.Empty;
            string into = String.Empty;
            string values = String.Empty;
            string computedColumns = String.Empty;
            string computedParameters = String.Empty;
            string computedComma = String.Empty;

            List<string> modifiedColumns = packet.ModifiedColumns;

            string sql = "BEGIN ";

            OracleParameter p = null;

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsAutoIncrement)
                {
                    p = CloneParameter(types[col.Name]);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(p);

                    string AutoKeyText = request.ProviderMetadata["AutoKeyText"];

                    // SELECT EMPLOYEE_ID.NextVal INTO p_EMPLOYEE_ID FROM DUAL;
                    sql += "SELECT \"" + AutoKeyText + "\".NextVal INTO " + p.ParameterName + " FROM DUAL; ";

                    into += comma;
                    into += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    values += comma;
                    values += p.ParameterName;
                    comma = ", ";
                }
                else if (col.IsEntitySpacesConcurrency)
                {
                    p = CloneParameter(types[col.Name]);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(p);

                    sql += p.ParameterName + " := 1; ";

                    into += comma;
                    into += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    values += comma;
                    values += p.ParameterName;
                    comma = ", ";
                }
                else if (col.HasDefault && (modifiedColumns != null && !modifiedColumns.Contains(col.Name)))
                {
                    p = CloneParameter(types[col.Name]);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(p);

                    computedColumns += computedComma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    computedParameters += computedComma + p.ParameterName;
                    computedComma = ", ";

                    if (col.CharacterMaxLength > 0)
                    {
                        p.Size = (int)col.CharacterMaxLength;
                    }
                }
            }

            esColumnMetadataCollection cols = request.Columns;

            #region Special Column Logic
            if (cols.DateAdded != null && cols.DateAdded.IsServerSide)
            {
                p = CloneParameter(types[cols.DateAdded.ColumnName]);
                sql += p.ParameterName + " := " + request.ProviderMetadata["DateAdded.ServerSideText"] + ";";

                CreateInsertSQLSnippet(cols.DateAdded.ColumnName, p, ref into, ref values, ref comma);

                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
            }

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                p = CloneParameter(types[cols.DateModified.ColumnName]);
                sql += p.ParameterName + " := " + request.ProviderMetadata["DateModified.ServerSideText"] + ";";

                CreateInsertSQLSnippet(cols.DateModified.ColumnName, p, ref into, ref values, ref comma);

                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
            }

            if (cols.AddedBy != null && cols.AddedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.AddedBy.ColumnName]);
                p.Size = (int)cols.FindByColumnName(cols.AddedBy.ColumnName).CharacterMaxLength;
                sql += p.ParameterName + " := " + request.ProviderMetadata["AddedBy.ServerSideText"] + ";";

                CreateInsertSQLSnippet(cols.AddedBy.ColumnName, p, ref into, ref values, ref comma);

                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.ModifiedBy.ColumnName]);
                p.Size = (int)cols.FindByColumnName(cols.ModifiedBy.ColumnName).CharacterMaxLength;
                sql += p.ParameterName + " := " + request.ProviderMetadata["ModifiedBy.ServerSideText"] + ";";

                CreateInsertSQLSnippet(cols.ModifiedBy.ColumnName, p, ref into, ref values, ref comma);

                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
            }
            #endregion

            sql += "INSERT INTO " + CreateFullName(request) + " ";

            if (modifiedColumns != null)
            {
                foreach (string colName in modifiedColumns)
                {
                    esColumnMetadata col = request.Columns[colName];
                    if (col != null && !col.IsAutoIncrement)
                    {
                        p = types[colName];
                        p = cmd.Parameters.Add(CloneParameter(p));

                        object value = packet.CurrentValues[colName];
                        p.Value = value != null ? value : DBNull.Value;

                        into += comma;
                        into += Delimiters.ColumnOpen + colName + Delimiters.ColumnClose;
                        values += comma;
                        values += p.ParameterName;
                        comma = ", ";
                    }
                }
            }

            if (into.Length != 0)
            {
                sql += "(" + into + ") VALUES (" + values + ")";
                sql += "; ";
            }

            if (computedColumns.Length > 0)
            {
                string where = String.Empty;

                foreach (esColumnMetadata col in request.Columns)
                {
                    if (col.IsInPrimaryKey)
                    {
                        // We need the were clause if there are defaults to bring back
                        p = types[col.Name];

                        if (where.Length > 0) where += " AND ";
                        where += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;

                        if (!cmd.Parameters.Contains(p.ParameterName))
                        {
                            p = CloneParameter(p);
                            p.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(p);
                        }
                    }
                }

                sql += " SELECT " + computedColumns + " INTO " + computedParameters + " FROM " + CreateFullName(request) + " WHERE (" + where + ");";
            }

            sql += " END;";

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static private void CreateInsertSQLSnippet(string colName, OracleParameter p, ref string into, ref string values, ref string comma)
        {
            into += comma;
            into += Delimiters.ColumnOpen + colName + Delimiters.ColumnClose;
            values += comma;
            values += p.ParameterName;
            comma = ", ";
        }

        //CREATE PROCEDURE "MYGENERATION"."proc_SeqTestUpdate" ( 
        //    pID IN "SeqTest"."ID"%type, 
        //    pTimeStamp IN OUT "SeqTest"."TimeStamp"%type, 
        //    pData IN "SeqTest"."Data"%type 
        //) 
        //IS 
        //    pConncurrency "SeqTest"."TimeStamp"%type := pTimeStamp;      

        //BEGIN 
        //    UPDATE "SeqTest" 
        //    SET 
        //        "TimeStamp" = "TimeStamp" + 1, 
        //        "Data"  = pData 
        //    WHERE 
        //        "ID" = pID 
        //    AND "TimeStamp" = pConncurrency 
        //; 

        //    IF SQL%ROWCOUNT = 1 THEN 
        //     pTimeStamp := (pConncurrency + 1); 
        //    ELSE 
        //     Raise_application_error(01403, 'NO RECORDS WERE UPDATED'); 
        //    END IF;  

        //END ;

        static public OracleCommand BuildDynamicUpdateCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, OracleParameter> types = Cache.GetParameters(request);

            OracleCommand cmd = new OracleCommand();
            cmd.BindByName = true;
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            string sql = string.Empty;

            bool hasConcurrency = false;
            string concurrencyColumn = String.Empty;

            List<string> modifiedColumns = packet.ModifiedColumns;

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsEntitySpacesConcurrency)
                {
                    hasConcurrency = true;
                    concurrencyColumn = col.Name;

                    sql += "DECLARE pConncurrency " + request.ProviderMetadata.GetTypeMap(col.PropertyName).NativeType + "; ";
                    break;
                }
            }

            OracleParameter p = null;
            sql += " BEGIN ";

            esColumnMetadataCollection cols = request.Columns;

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                p = CloneParameter(types[cols.DateModified.ColumnName]);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                sql += p.ParameterName + " := " + request.ProviderMetadata["DateModified.ServerSideText"] + "; ";
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.ModifiedBy.ColumnName]);
                p.Size = (int)cols.FindByColumnName(cols.ModifiedBy.ColumnName).CharacterMaxLength;
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                sql += p.ParameterName + " := " + request.ProviderMetadata["ModifiedBy.ServerSideText"] + "; ";
            }


            if (hasConcurrency)
            {
                sql += "pConncurrency := " + Delimiters.Param + concurrencyColumn + "; ";
            }

            sql += "UPDATE " + CreateFullName(request) + " SET ";

            string computed = String.Empty;
            string comma = String.Empty;
            string and = String.Empty;
            string where = string.Empty;

            foreach (string colName in modifiedColumns)
            {
                esColumnMetadata col = request.Columns[colName];

                if (col != null && !col.IsInPrimaryKey && !col.IsEntitySpacesConcurrency)
                {
                    p = cmd.Parameters.Add(CloneParameter(types[colName]));

                    object value = packet.CurrentValues[colName];
                    p.Value = value != null ? value : DBNull.Value;

                    sql += comma;
                    sql += Delimiters.ColumnOpen + colName + Delimiters.ColumnClose + " = " + p.ParameterName;
                    comma = ", ";
                }
            }

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsInPrimaryKey)
                {
                    p = CloneParameter(types[col.Name]);
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);

                    where += and;
                    where += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    and = " AND ";
                }
                else if (col.IsComputed && !col.IsAutoIncrement)
                {
                    if (request.SelectedColumns != null && request.SelectedColumns.ContainsKey(col.Name))
                    {
                        p = CloneParameter(types[col.Name]);
                        p.Direction = ParameterDirection.Output;
                        if (col.CharacterMaxLength > 0)
                        {
                            p.Size = (int)col.CharacterMaxLength;
                        }
                        cmd.Parameters.Add(p);

                        if (computed.Length > 0) computed += ", ";
                        computed += " " + p.ParameterName + " = " + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    }
                }
                if (col.IsEntitySpacesConcurrency)
                {
                    sql += ", ";
                    sql += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " +
                      Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " + 1";

                    p = CloneParameter(types[col.Name]);
                    p.Direction = ParameterDirection.InputOutput;
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);
                    break;
                }
            }

            sql += " WHERE (" + where + ") ";
            if (hasConcurrency)
            {
                sql += " AND \"" + concurrencyColumn + "\" = pConncurrency; ";

                sql += "IF SQL%ROWCOUNT = 1 THEN ";
                sql += Delimiters.Param + concurrencyColumn + " := pConncurrency + 1; ELSE ";
                sql += "Raise_application_error(-20101, 'NO RECORDS WERE UPDATED'); END IF;";
            }
            else
            {
                sql += ";";
            }

            if (computed.Length > 0)
            {
                sql += " SELECT " + computed + " FROM " + CreateFullName(request) + " WHERE (" + where + ")";
            }

            sql += " END;";

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public OracleCommand BuildDynamicDeleteCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, OracleParameter> types = Cache.GetParameters(request);

            OracleCommand cmd = new OracleCommand();
            cmd.BindByName = true;
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            string sql = String.Empty;
            bool hasConcurrency = false;

            sql += "BEGIN ";

            sql += "DELETE FROM " + CreateFullName(request) + " ";

            string where = String.Empty;
            string comma = String.Empty;

            comma = String.Empty;
            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsInPrimaryKey || col.IsEntitySpacesConcurrency)
                {
                    OracleParameter p = CloneParameter(types[col.Name]);
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);

                    where += comma;
                    where += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    comma = " AND ";

                    if (col.IsEntitySpacesConcurrency) hasConcurrency = true;
                }
            }

            sql += " WHERE (" + where + "); ";


            if (hasConcurrency)
            {
                sql += "IF SQL%ROWCOUNT = 0 THEN ";
                sql += "Raise_application_error(-20101, 'NO RECORDS WERE DELETED'); END IF;";
            }

            sql += " END;";

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public OracleCommand BuildStoredProcInsertCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, OracleParameter> types = Cache.GetParameters(request);

            OracleCommand cmd = new OracleCommand();
            cmd.BindByName = true;
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = CreateFullSPName(request, request.ProviderMetadata.spInsert);

            PopulateStoredProcParameters(cmd, request, packet);

            esColumnMetadataCollection cols = request.Columns;
            OracleParameter p = null;

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsComputed || col.IsAutoIncrement || col.IsEntitySpacesConcurrency)
                {
                    p = cmd.Parameters["p" + (col.Name).Replace(" ", String.Empty)];
                    p.Direction = ParameterDirection.Output;
                }
            }

            if (cols.DateAdded != null && cols.DateAdded.IsServerSide)
            {
                p = cmd.Parameters[types[cols.DateAdded.ColumnName].ParameterName];
                p = cmd.Parameters[p.ParameterName];
                p.Direction = ParameterDirection.Output;
            }

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                p = cmd.Parameters[types[cols.DateModified.ColumnName].ParameterName];
                p = cmd.Parameters[p.ParameterName];
                p.Direction = ParameterDirection.Output;
            }

            if (cols.AddedBy != null && cols.AddedBy.IsServerSide)
            {
                p = cmd.Parameters[types[cols.AddedBy.ColumnName].ParameterName];
                p.Size = (int)cols.FindByColumnName(cols.AddedBy.ColumnName).CharacterMaxLength;
                p = cmd.Parameters[p.ParameterName];
                p.Direction = ParameterDirection.Output;
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                p = cmd.Parameters[types[cols.ModifiedBy.ColumnName].ParameterName];
                p.Size = (int)cols.FindByColumnName(cols.ModifiedBy.ColumnName).CharacterMaxLength;
                p = cmd.Parameters[p.ParameterName];
                p.Direction = ParameterDirection.Output;
            }

            return cmd;
        }

        static public OracleCommand BuildStoredProcUpdateCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, OracleParameter> types = Cache.GetParameters(request);

            OracleCommand cmd = new OracleCommand();
            cmd.BindByName = true;
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = CreateFullSPName(request, request.ProviderMetadata.spUpdate);

            PopulateStoredProcParameters(cmd, request, packet);

            esColumnMetadataCollection cols = request.Columns;
            OracleParameter p = null;

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsComputed || col.IsEntitySpacesConcurrency)
                {
                    p = cmd.Parameters["p" + (col.Name).Replace(" ", String.Empty)];
                    p.Direction = ParameterDirection.InputOutput;
                }
            }

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                p = cmd.Parameters[types[cols.DateModified.ColumnName].ParameterName];
                p = cmd.Parameters[p.ParameterName];
                p.Value = null;
                p.Direction = ParameterDirection.Output;
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                p = cmd.Parameters[types[cols.ModifiedBy.ColumnName].ParameterName];
                p.Size = (int)cols.FindByColumnName(cols.ModifiedBy.ColumnName).CharacterMaxLength;
                p = cmd.Parameters[p.ParameterName];
                p.Value = null;
                p.Direction = ParameterDirection.Output;
            }

            return cmd;
        }

        static public OracleCommand BuildStoredProcDeleteCommand(esDataRequest request, esEntitySavePacket packet)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.BindByName = true;
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = CreateFullSPName(request, request.ProviderMetadata.spDelete);

            Dictionary<string, OracleParameter> types = Cache.GetParameters(request);

            OracleParameter p;

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsInPrimaryKey || col.IsEntitySpacesConcurrency)
                {
                    p = types[col.Name];
                    p = CloneParameter(p);
                    p.ParameterName = p.ParameterName.Replace(":", "p");
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);
                }
            }

            return cmd;
        }

        static public void PopulateStoredProcParameters(OracleCommand cmd, esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, OracleParameter> types = Cache.GetParameters(request);

            OracleParameter p;

            foreach (esColumnMetadata col in request.Columns)
            {
                p = types[col.Name];
                p = CloneParameter(p);

                p.ParameterName = p.ParameterName.Replace(":", "p");

                if (packet.CurrentValues.ContainsKey(col.Name))
                {
                    p.Value = packet.CurrentValues[col.Name];
                }
                else
                {
                    p.Value = DBNull.Value;
                }

                if (p.OracleDbType == OracleDbType.TimeStamp)
                {
                    p.Direction = ParameterDirection.InputOutput;
                }
                cmd.Parameters.Add(p);
            }
        }

        static private OracleParameter CloneParameter(OracleParameter p)
        {
            ICloneable param = p as ICloneable;
            return param.Clone() as OracleParameter;
        }

        //static public string CreateFullName(esDataRequest request, esDynamicQuerySerializable query)
        static public string CreateFullName(esDataRequest request, esDynamicQuery query)
        {
            //IDynamicQuerySerializableInternal iQuery = query as IDynamicQuerySerializableInternal;
            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            esProviderSpecificMetadata providerMetadata = iQuery.ProviderMetadata as esProviderSpecificMetadata;

            string name = String.Empty;

            string schema = iQuery.Schema ?? request.Schema ?? providerMetadata.Schema;

            if (schema != null)
            {
                name += Delimiters.TableOpen + schema + Delimiters.TableClose + ".";
            }

            name += Delimiters.TableOpen;
            if (query.querySource != null)
                name += query.querySource;
            else
                name += providerMetadata.Destination;
            name += Delimiters.TableClose;

            return name;
        }

        static public string CreateFullName(esDataRequest request)
        {
            string name = String.Empty;

            string schema = request.Schema ?? request.ProviderMetadata.Schema;

            if (schema != null)
            {
                name += Delimiters.TableOpen + schema + Delimiters.TableClose + ".";
            }

            name += Delimiters.TableOpen;
            if (request.DynamicQuery != null && request.DynamicQuery.querySource != null)
                name += request.DynamicQuery.querySource;
            else
                name += request.QueryText != null ? request.QueryText : request.ProviderMetadata.Destination;


            name += Delimiters.TableClose;

            return name;
        }

        static public string CreateFullSPName(esDataRequest request, string spName)
        {
            string name = String.Empty;

            if (request.Schema != null || request.ProviderMetadata.Schema != null)
            {
                name += Delimiters.TableOpen;
                name += request.Schema != null ? request.Schema : request.ProviderMetadata.Schema;
                name += Delimiters.TableClose + ".";
            }

            name += Delimiters.StoredProcNameOpen;
            name += spName;
            name += Delimiters.StoredProcNameClose;

            return name;
        }

        static public esConcurrencyException CheckForConcurrencyException(OracleException ex)
        {
            esConcurrencyException ce = null;

            if (ex.ErrorCode == 20101)
            {
                ce = new esConcurrencyException(ex.Message, ex);
                ce.Source = ex.Source;
            }

            return ce;
        }

        static public void AddParameters(OracleCommand cmd, esDataRequest request)
        {
            if (request.QueryType == esQueryType.Text && request.QueryText != null && request.QueryText.Contains("{0}"))
            {
                int i = 0;
                string token = String.Empty;
                string sIndex = String.Empty;
                string param = String.Empty;

                foreach (esParameter esParam in request.Parameters)
                {
                    sIndex = i.ToString();
                    token = '{' + sIndex + '}';
                    param = Delimiters.Param + "p" + sIndex;
                    request.QueryText = request.QueryText.Replace(token, param);
                    i++;

                    OracleParameter p = new OracleParameter(Delimiters.Param + esParam.Name, esParam.Value);
                    cmd.Parameters.Add(p);
                }
            }
            else
            {
                OracleParameter param;

                string paramPrefix = request.QueryType == esQueryType.StoredProcedure ? String.Empty : Delimiters.Param;
                paramPrefix = request.ProviderMetadata.spLoadByPrimaryKey == request.QueryText ? "p" : paramPrefix;

                foreach (esParameter esParam in request.Parameters)
                {
                    param = new OracleParameter(paramPrefix + esParam.Name, esParam.Value);
                    cmd.Parameters.Add(param);

                    // The default is ParameterDirection.Input
                    switch (esParam.Direction)
                    {
                        case esParameterDirection.InputOutput:
                            param.Direction = ParameterDirection.InputOutput;
                            break;

                        case esParameterDirection.Output:
                            param.Direction = ParameterDirection.Output;
                            param.DbType = esParam.DbType;
                            param.Size = esParam.Size;
                            // Precision and Scale are obsolete for Oracle
                            // and are ignored.
                            //param.Scale = esParam.Scale;
                            //param.Precision = esParam.Precision;
                            break;

                        case esParameterDirection.ReturnValue:
                            param.Direction = ParameterDirection.ReturnValue;
                            break;
                    }
                }
            }
        }

        static public void GatherReturnParameters(OracleCommand cmd, esDataRequest request, esDataResponse response)
        {
            if (cmd.Parameters.Count > 0)
            {
                if (request.Parameters != null && request.Parameters.Count > 0)
                {
                    string paramPrefix = request.QueryType == esQueryType.StoredProcedure ? String.Empty : Delimiters.Param;
                    paramPrefix = request.ProviderMetadata.spLoadByPrimaryKey == request.QueryText ? "p" : paramPrefix;

                    response.Parameters = new esParameters();

                    foreach (esParameter esParam in request.Parameters)
                    {
                        if (esParam.Direction != esParameterDirection.Input)
                        {
                            response.Parameters.Add(esParam);
                            OracleParameter p = cmd.Parameters[paramPrefix + esParam.Name];
                            esParam.Value = p.Value;
                        }
                    }
                }
            }
        }
    }
}
