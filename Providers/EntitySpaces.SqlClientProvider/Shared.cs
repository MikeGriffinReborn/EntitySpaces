/*  New BSD License
-------------------------------------------------------------------------------
Copyright (c) 2006-2012, EntitySpaces, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the EntitySpaces, LLC nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL EntitySpaces, LLC BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;

namespace EntitySpaces.SqlClientProvider
{
    class Shared
    {
        static public SqlCommand BuildDynamicInsertCommand(esDataRequest request, esEntitySavePacket packet)
        {
            string into = String.Empty;
            string values = String.Empty;
            string computed = String.Empty;
            string computedComma = String.Empty;
            string autoInc = String.Empty;
            string comma = String.Empty;
            string where = String.Empty;

            // newsequentialid variables
            int seqCount = 0;
            string seqDeclare = " DECLARE @table_ids TABLE (";
            string seqOutput = " OUTPUT ";
            string seqSelect = " SELECT ";

            List<string> modifiedColumns = packet.ModifiedColumns;

            Dictionary<string, SqlParameter> types = Cache.GetParameters(request);

            SqlCommand cmd = new SqlCommand();
            SqlParameter p = null;
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            string sql = "SET NOCOUNT OFF";

            foreach (esColumnMetadata col in request.Columns)
            {
                string colName = col.Name;

                if (request.SelectedColumns != null && !request.SelectedColumns.ContainsKey(colName)) continue;

                bool isModified = modifiedColumns == null ? false : modifiedColumns.Contains(col.Name);

                if (isModified && !col.IsComputed && !col.IsConcurrency && !col.IsAutoIncrement)
                {
                    p = types[colName];
                    p = cmd.Parameters.Add(CloneParameter(p));

                    object value = packet.CurrentValues[colName];
                    p.Value = value != null ? value : DBNull.Value;

                    CreateInsertSQLSnippet(colName, p, ref into, ref values, ref comma);
                }
                else
                {
                    bool needOutputParam = false;
                    bool needsFetchedAfterSave = false;
                    SqlParameter clone = null;

                    if (col.HasDefault)
                    {
                        p = types[colName];

                        if (col.esType == esSystemType.Guid)
                        {
                            if (col.Default.ToLower().Contains("newid"))
                            {
                                // Special logic for newid()'s that weren't supplied with a value, they
                                // go into the SELECT INTO as well
                                sql += " SET " + p.ParameterName + " = NEWID(); ";
                                CreateInsertSQLSnippet(colName, p, ref into, ref values, ref comma);
                                needOutputParam = true;
                            }
                            else if (col.Default.ToLower().Contains("newsequentialid"))
                            {
                                if (seqCount > 0)
                                {
                                    seqDeclare += ", ";
                                    seqOutput += ", ";
                                    seqSelect += ", ";
                                }
                                seqCount++;

                                seqDeclare += col.Name + "  uniqueidentifier";
                                seqOutput += "INSERTED." + col.Name;
                                seqSelect += Delimiters.Param + col.PropertyName + "=" + col.Name;

                                needOutputParam = true;
                            }
                        }
                        else
                        {
                            // 11/15/2009 Let's return all default values
                            needOutputParam = true;
                            needsFetchedAfterSave = true;
                        }
                    }
                    else if (col.IsEntitySpacesConcurrency)
                    {
                        p = types[colName];
                        sql += " SET " + p.ParameterName + " = 1; ";

                        into += comma;
                        into += Delimiters.ColumnOpen + colName + Delimiters.ColumnClose;
                        values += comma;
                        values += "1";
                        comma = ", ";

                        needOutputParam = true;
                    }
                    else if (col.IsAutoIncrement)
                    {
                        p = types[colName];
                        autoInc += " SELECT " + p.ParameterName + " = SCOPE_IDENTITY() ";
                        needOutputParam = true;
                    }
                    else if (col.IsComputed || col.IsConcurrency)
                    {
                        p = types[colName];
                        needOutputParam = true;
                        needsFetchedAfterSave = true;
                    }

                    if (needOutputParam)
                    {
                        clone = CloneParameter(p);
                        clone.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(clone);
                    }

                    if (needsFetchedAfterSave)
                    {
                        computed += computedComma;
                        computed += p.ParameterName + " = " + Delimiters.ColumnOpen + colName + Delimiters.ColumnClose;
                        computedComma = ", ";

                        if (col.CharacterMaxLength > 0)
                        {
                            clone.Size = (int)col.CharacterMaxLength;
                        }
                    }
                }
            }

            esColumnMetadataCollection cols = request.Columns;

            #region Special Column Logic
            if (cols.DateAdded != null && cols.DateAdded.IsServerSide)
            {
                p = CloneParameter(types[cols.DateAdded.ColumnName]);
                sql += " SET " + p.ParameterName + " = " + request.ProviderMetadata["DateAdded.ServerSideText"] + ";";
                CreateInsertSQLSnippet(cols.DateAdded.ColumnName, p, ref into, ref values, ref comma);

                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
            }

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                p = CloneParameter(types[cols.DateModified.ColumnName]);
                sql += " SET " + p.ParameterName + " = " + request.ProviderMetadata["DateModified.ServerSideText"] + ";";
                CreateInsertSQLSnippet(cols.DateModified.ColumnName, p, ref into, ref values, ref comma);

                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
            }

            if (cols.AddedBy != null && cols.AddedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.AddedBy.ColumnName]);
                p.Size = (int)cols.FindByColumnName(cols.AddedBy.ColumnName).CharacterMaxLength;
                sql += " SET " + p.ParameterName + " = " + request.ProviderMetadata["AddedBy.ServerSideText"] + ";";

                CreateInsertSQLSnippet(cols.AddedBy.ColumnName, p, ref into, ref values, ref comma);

                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.ModifiedBy.ColumnName]);
                p.Size = (int)cols.FindByColumnName(cols.ModifiedBy.ColumnName).CharacterMaxLength;
                sql += " SET " + p.ParameterName + " = " + request.ProviderMetadata["ModifiedBy.ServerSideText"] + ";";

                CreateInsertSQLSnippet(cols.ModifiedBy.ColumnName, p, ref into, ref values, ref comma);

                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
            }
            #endregion

            seqDeclare += ")";
            seqOutput += " INTO @table_ids";
            seqSelect += " FROM @table_ids";

            if (computed.Length > 0)
            {
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
            }

            string fullName = CreateFullName(request);

            if (seqCount > 0)
            {
                sql += seqDeclare;
            }

            sql += " INSERT INTO " + fullName;

            if (into.Length != 0 && seqCount > 0)
            {
                sql += "(" + into + ") " + seqOutput + " VALUES (" + values + ")";
            }
            else if (into.Length != 0)
            {
                sql += "(" + into + ") VALUES (" + values + ")";
            }
            else
            {
                sql += "DEFAULT VALUES";
            }

            sql += autoInc;

            if (seqCount > 0)
            {
                sql += seqSelect;
            }

            if (computed.Length > 0)
            {
                sql += " SELECT " + computed + " FROM " + fullName + " WHERE (" + where + ")";
            }

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static private void CreateInsertSQLSnippet(string colName, SqlParameter p, ref string into, ref string values, ref string comma)
        {
            into += comma;
            into += Delimiters.ColumnOpen + colName + Delimiters.ColumnClose;
            values += comma;
            values += p.ParameterName;
            comma = ", ";
        }

        static public SqlCommand BuildDynamicUpdateCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, SqlParameter> types = Cache.GetParameters(request);

            SqlCommand cmd = new SqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            string set = string.Empty;
            string sql = "SET NOCOUNT OFF ";
            sql += "UPDATE " + CreateFullName(request) + " SET ";

            string where = String.Empty;
            string conncur = String.Empty;
            string computed = String.Empty;
            string comma = String.Empty;
            string and = String.Empty;
            string prolog = String.Empty;

            SqlParameter p = null;

            List<string> modifiedColumns = packet.ModifiedColumns;

            foreach (string colName in modifiedColumns)
            {
                esColumnMetadata col = request.Columns[colName];

                if (col == null) continue;

                if (!col.IsInPrimaryKey && !col.IsComputed)
                {
                    p = CloneParameter(types[colName]);
                    p = cmd.Parameters.Add(p);

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
                else if (col.IsConcurrency)
                {
                    p = CloneParameter(types[col.Name]);
                    p.Value = packet.OriginalValues[col.Name];
                    p.Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add(p);

                    if (request.DatabaseVersion == "2005" || request.DatabaseVersion == "2008")
                        conncur += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    else
                        conncur += "TSEQUAL(" + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + "," + p.ParameterName + ")";

                    if (computed.Length > 0) computed += ", ";
                    computed += " " + p.ParameterName + " = " + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
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
                else if (col.IsEntitySpacesConcurrency)
                {
                    if (packet.OriginalValues != null && packet.OriginalValues.ContainsKey(col.Name))
                    {
                        p = CloneParameter(types[col.Name]);
                        p.Direction = ParameterDirection.InputOutput;
                        p.Value = packet.OriginalValues[col.Name];
                        cmd.Parameters.Add(p);

                        sql += comma.Length > 0 ? ", " : string.Empty;
                        sql += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " +
                            Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " + 1";

                        conncur += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;

                        prolog += " SET " + p.ParameterName + " = " + p.ParameterName + " + 1";
                    }
                }
            }

            esColumnMetadataCollection cols = request.Columns;

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                p = CloneParameter(types[cols.DateModified.ColumnName]);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                sql += comma;
                sql += Delimiters.ColumnOpen + cols.DateModified.ColumnName + Delimiters.ColumnClose + " = " + p.ParameterName;
                comma = ", ";

                set += " SET " + p.ParameterName + " = " + request.ProviderMetadata["DateModified.ServerSideText"] + ";";
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.ModifiedBy.ColumnName]);
                p.Size = (int)cols.FindByColumnName(cols.ModifiedBy.ColumnName).CharacterMaxLength;
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                sql += comma;
                sql += Delimiters.ColumnOpen + cols.ModifiedBy.ColumnName + Delimiters.ColumnClose + " = " + p.ParameterName;
                comma = ", ";

                set += " SET " + p.ParameterName + " = " + request.ProviderMetadata["ModifiedBy.ServerSideText"] + ";";
            }


            sql = set + sql + " WHERE (" + where + ")";
            if (conncur.Length > 0)
            {
                sql += " AND " + conncur;
            }

            if (computed.Length > 0)
            {
                sql += " SELECT " + computed + " FROM " + CreateFullName(request) + " WHERE (" + where + ")";
            }

            if (prolog.Length > 0)
            {
                sql += prolog;
            }

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public SqlCommand BuildDynamicDeleteCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, SqlParameter> types = Cache.GetParameters(request);

            SqlCommand cmd = new SqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            string sql = "SET NOCOUNT OFF; ";
            sql += "DELETE FROM " + CreateFullName(request) + " ";

            string comma = String.Empty;
            string concur = String.Empty;
            comma = String.Empty;
            sql += " WHERE ";
            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsInPrimaryKey)
                {
                    SqlParameter p = CloneParameter(types[col.Name]);
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);

                    sql += comma;
                    sql += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    comma = " AND ";
                }
                else if (col.IsConcurrency || col.IsEntitySpacesConcurrency)
                {
                    SqlParameter p = CloneParameter(types[col.Name]);
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);

                    if (request.DatabaseVersion == "2005" || request.DatabaseVersion == "2008" || col.IsEntitySpacesConcurrency)
                        concur += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    else
                        concur += "TSEQUAL(" + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + "," + p.ParameterName + ")";
                }
            }

            if (concur.Length > 0)
            {
                sql += " AND " + concur;
            }

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public SqlCommand BuildStoredProcInsertCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, SqlParameter> types = Cache.GetParameters(request);

            SqlCommand cmd = new SqlCommand();
            if(request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = CreateFullSPName(request, request.ProviderMetadata.spInsert);

            PopulateStoredProcParameters(cmd, request, packet);

            esColumnMetadataCollection cols = request.Columns;

            foreach (esColumnMetadata col in cols)
            {
                if (col.HasDefault &&
                    (col.Default.ToLower().Contains("newid") || col.Default.ToLower().Contains("newsequentialid")))
                {
                    // They could pre-assign this even though it has a default
                    SqlParameter p = types[col.Name];
                    p = cmd.Parameters[p.ParameterName];

                    if (packet.ModifiedColumns.Contains(col.Name))
                    {
                        p.Direction = ParameterDirection.InputOutput;
                    }
                    else
                    {
                        p.Direction = ParameterDirection.Output;
                    }
                }
                else if (col.IsComputed || col.IsAutoIncrement || col.IsEntitySpacesConcurrency)
                {
                    SqlParameter p = types[col.Name];
                    p = cmd.Parameters[p.ParameterName];
                    p.Direction = ParameterDirection.Output;
                }
            }

            if (cols.DateAdded != null && cols.DateAdded.IsServerSide)
            {
                SqlParameter p = cmd.Parameters[types[cols.DateAdded.ColumnName].ParameterName];
                p = cmd.Parameters[p.ParameterName];
                p.Direction = ParameterDirection.Output;
            }

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                SqlParameter p = cmd.Parameters[types[cols.DateModified.ColumnName].ParameterName];
                p = cmd.Parameters[p.ParameterName];
                p.Direction = ParameterDirection.Output;
            }

            if (cols.AddedBy != null && cols.AddedBy.IsServerSide)
            {
                SqlParameter p = cmd.Parameters[types[cols.AddedBy.ColumnName].ParameterName];
                p.Size = (int)cols.FindByColumnName(cols.AddedBy.ColumnName).CharacterMaxLength;
                p = cmd.Parameters[p.ParameterName];
                p.Direction = ParameterDirection.Output;
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                SqlParameter p = cmd.Parameters[types[cols.ModifiedBy.ColumnName].ParameterName];
                p.Size = (int)cols.FindByColumnName(cols.ModifiedBy.ColumnName).CharacterMaxLength;
                p = cmd.Parameters[p.ParameterName];
                p.Direction = ParameterDirection.Output;
            }

            return cmd;
        }

        static public SqlCommand BuildStoredProcUpdateCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, SqlParameter> types = Cache.GetParameters(request);

            SqlCommand cmd = new SqlCommand();
            if(request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = CreateFullSPName(request, request.ProviderMetadata.spUpdate);

            PopulateStoredProcParameters(cmd, request, packet);

            esColumnMetadataCollection cols = request.Columns;

            foreach (esColumnMetadata col in cols)
            {
                if (col.IsComputed || col.IsEntitySpacesConcurrency)
                {
                    SqlParameter p = types[col.Name];
                    p = cmd.Parameters[p.ParameterName];
                    p.Direction = ParameterDirection.InputOutput;
                }
            }

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                SqlParameter p = cmd.Parameters[types[cols.DateModified.ColumnName].ParameterName];
                p = cmd.Parameters[p.ParameterName];
                p.Value = null;
                p.Direction = ParameterDirection.Output;
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                SqlParameter p = cmd.Parameters[types[cols.ModifiedBy.ColumnName].ParameterName];
                p.Size = (int)cols.FindByColumnName(cols.ModifiedBy.ColumnName).CharacterMaxLength;
                p = cmd.Parameters[p.ParameterName];
                p.Value = null;
                p.Direction = ParameterDirection.Output;
            }

            return cmd;
        }

        static public SqlCommand BuildStoredProcDeleteCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, SqlParameter> types = Cache.GetParameters(request);

            SqlCommand cmd = new SqlCommand();
            if(request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = CreateFullSPName(request, request.ProviderMetadata.spDelete);

            SqlParameter p;

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsInPrimaryKey)
                {
                    p = types[col.Name];
                    p = CloneParameter(p);
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);
                }
                else if (col.IsConcurrency || col.IsEntitySpacesConcurrency)
                {
                    p = types[col.Name];
                    p = CloneParameter(p);
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);
                }
            }

            return cmd;
        }

        static public void PopulateStoredProcParameters(SqlCommand cmd, esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, SqlParameter> types = Cache.GetParameters(request);

            SqlParameter p;

            foreach (esColumnMetadata col in request.Columns)
            {
                p = types[col.Name];
                p = CloneParameter(p);

                if (packet.CurrentValues.ContainsKey(col.Name))
                {
                    p.Value = packet.CurrentValues[col.Name];
                }

                if (p.SqlDbType == SqlDbType.Timestamp)
                {
                    p.Direction = ParameterDirection.InputOutput;
                }

                if (col.IsComputed && col.CharacterMaxLength > 0)
                {
                    p.Size = (int)col.CharacterMaxLength;
                }

                cmd.Parameters.Add(p);
            }
        }

        static private SqlParameter CloneParameter(SqlParameter p)
        {
            ICloneable param = p as ICloneable;
            return param.Clone() as SqlParameter;
        }

        static public string CreateFullName(esDataRequest request, esDynamicQuerySerializable query)
        {
            IDynamicQuerySerializableInternal iQuery = query as IDynamicQuerySerializableInternal;

            esProviderSpecificMetadata providerMetadata = iQuery.ProviderMetadata as esProviderSpecificMetadata;

            string name = String.Empty;

            string catalog = iQuery.Catalog ?? request.Catalog ?? providerMetadata.Catalog;
            string schema = iQuery.Schema ?? request.Schema ?? providerMetadata.Schema;

            if (catalog != null && schema != null)
            {
                name += Delimiters.TableOpen + catalog + Delimiters.TableClose + ".";
            }

            if (schema != null)
            {
                name += Delimiters.TableOpen + schema + Delimiters.TableClose + ".";
            }

            name += Delimiters.TableOpen;

            if (query.es.QuerySource != null)
                name += query.es.QuerySource;
            else
                name += providerMetadata.Destination;
            name += Delimiters.TableClose;

            return name;
        }

        static public string CreateFullName(esDataRequest request)
        {
            string name = String.Empty;

            string catalog = request.Catalog ?? request.ProviderMetadata.Catalog;
            string schema = request.Schema ?? request.ProviderMetadata.Schema;

            if (catalog != null && schema != null)
            {
                name += Delimiters.TableOpen + catalog + Delimiters.TableClose + ".";
            }

            if (schema != null)
            {
                name += Delimiters.TableOpen + schema + Delimiters.TableClose + ".";
            }

            name += Delimiters.TableOpen;

            if (request.DynamicQuery != null && request.DynamicQuery.es.QuerySource != null)
                name += request.DynamicQuery.es.QuerySource;
            else
                name += request.QueryText != null ? request.QueryText : request.ProviderMetadata.Destination;
            name += Delimiters.TableClose;

            return name;
        }

        static public string CreateFullSPName(esDataRequest request, string spName)
        {
            string name = String.Empty;

            if ( (request.Catalog != null || request.ProviderMetadata.Catalog != null) &&
                 (request.Schema != null || request.ProviderMetadata.Schema != null) )
            {
                name += Delimiters.TableOpen;
                name += request.Catalog != null ? request.Catalog : request.ProviderMetadata.Catalog;
                name += Delimiters.TableClose + ".";
            }

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

        static public esConcurrencyException CheckForConcurrencyException(SqlException ex)
        {
            esConcurrencyException ce = null;

            if (ex.Errors != null)
            {
                foreach (SqlError err in ex.Errors)
                {
                    if (err.Number == 532)
                    {
                        ce = new esConcurrencyException(err.Message, ex);
                        ce.Source = err.Source;
                        break;
                    }
                }
            }

            return ce;
        }

        static public void AddParameters(SqlCommand cmd, esDataRequest request)
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

                    SqlParameter p = cmd.Parameters.AddWithValue(Delimiters.Param + esParam.Name, esParam.Value);

                    if (esParam.UdtTypeName != null)
                    {
                        p.UdtTypeName = esParam.UdtTypeName;
                    }
                }
            }
            else
            {
                SqlParameter param;

                foreach (esParameter esParam in request.Parameters)
                {
                    param = cmd.Parameters.AddWithValue(Delimiters.Param + esParam.Name, esParam.Value);

                    switch (esParam.Direction)
                    {
                        case esParameterDirection.InputOutput:
                            param.Direction = ParameterDirection.InputOutput;
                            break;

                        case esParameterDirection.Output:
                            param.Direction = ParameterDirection.Output;
                            param.DbType = esParam.DbType;
                            param.Size = esParam.Size;
                            param.Scale = esParam.Scale;
                            param.Precision = esParam.Precision;
                            break;

                        case esParameterDirection.ReturnValue:
                            param.Direction = ParameterDirection.ReturnValue;
                            break;

                        // The default is ParameterDirection.Input;
                    }

                    if (esParam.UdtTypeName != null)
                    {
                        param.UdtTypeName = esParam.UdtTypeName;
                    }
                }
            }
        }

        static public void GatherReturnParameters(SqlCommand cmd, esDataRequest request, esDataResponse response)
        {
            if (cmd.Parameters.Count > 0)
            {
                if (request.Parameters != null && request.Parameters.Count > 0)
                {
                    response.Parameters = new esParameters();

                    foreach (esParameter esParam in request.Parameters)
                    {
                        if (esParam.Direction != esParameterDirection.Input)
                        {
                            response.Parameters.Add(esParam);
                            SqlParameter p = cmd.Parameters[Delimiters.Param + esParam.Name];
                            esParam.Value = p.Value;
                        }
                    }
                }
            }
        }
    }
}
