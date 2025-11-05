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

using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;

using Npgsql;
using NpgsqlTypes;

namespace EntitySpaces.Npgsql2Provider
{
    class Shared
    {
        static public NpgsqlCommand BuildDynamicInsertCommand(esDataRequest request, esEntitySavePacket packet)
        {
            string sql = String.Empty;
            string defaults = String.Empty;
            string into = String.Empty;
            string values = String.Empty;
            string comma = String.Empty;
            string defaultComma = String.Empty;
            string where = String.Empty;
            string autoInc = String.Empty;

            NpgsqlParameter p = null;

            Dictionary<string, NpgsqlParameter> types = Cache.GetParameters(request);

            NpgsqlCommand cmd = new NpgsqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            esColumnMetadataCollection cols = request.Columns;
            foreach (esColumnMetadata col in cols)
            {
                bool isModified = packet.ModifiedColumns == null ? false : packet.ModifiedColumns.Contains(col.Name);

                if (isModified && (!col.IsAutoIncrement && !col.IsConcurrency && !col.IsEntitySpacesConcurrency))
                {
                    p = cmd.Parameters.Add(CloneParameter(types[col.Name]));

                    object value = packet.CurrentValues[col.Name];
                    p.Value = value != null ? value : DBNull.Value;

                    into += comma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    values += comma + p.ParameterName;
                    comma = ", ";
                }
                else if (col.IsAutoIncrement && request.ProviderMetadata.ContainsKey("AutoKeyText"))
                {
                    string sequence = request.ProviderMetadata["AutoKeyText"].Replace("nextval", "currval");

                    if (sequence != null && sequence.Length > 0)
                    {
                        autoInc += " RETURNING \"" + col.Name + "\";";
                    }
                    
                    p = CloneParameter(types[col.Name]);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(p);
                }
                else if (col.IsConcurrency)
                {
                    // These columns have defaults and they weren't supplied with values, so let's
                    // return them
                    p = cmd.Parameters.Add(CloneParameter(types[col.Name]));
                    p.Direction = ParameterDirection.InputOutput;

                    defaults += defaultComma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    defaultComma = ", ";

                    if (col.CharacterMaxLength > 0)
                    {
                        p.Size = (int)col.CharacterMaxLength;
                    }
                }
                else if (col.IsEntitySpacesConcurrency)
                {
                    p = cmd.Parameters.Add(CloneParameter(types[col.Name]));
                    p.Direction = ParameterDirection.Output;

                    into += comma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    values += comma + "1";
                    comma = ", ";

                    p.Value = 1; // Seems to work, We'll take it ...

                }
                else if (col.IsComputed)
                {
                    // Do nothing but leave this here
                }
                else if (cols.IsSpecialColumn(col))
                {
                    // Do nothing but leave this here
                }
                else if (col.HasDefault)
                {
                    // These columns have defaults and they weren't supplied with values, so let's
                    // return them
                    p = cmd.Parameters.Add(CloneParameter(types[col.Name]));
                    p.Direction = ParameterDirection.InputOutput;

                    defaults += defaultComma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    defaultComma = ",";

                    if (col.CharacterMaxLength > 0)
                    {
                        p.Size = (int)col.CharacterMaxLength;
                    }
                }

                if (col.IsInPrimaryKey)
                {
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

            #region Special Column Logic
            if (cols.DateAdded != null && cols.DateAdded.IsServerSide)
            {
                p = CloneParameter(types[cols.DateAdded.ColumnName]);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                into += comma + Delimiters.ColumnOpen + cols.DateAdded.ColumnName + Delimiters.ColumnClose;
                values += comma + request.ProviderMetadata["DateAdded.ServerSideText"];
                comma = ", ";

                defaults += defaultComma + cols.DateAdded.ColumnName;
                defaultComma = ",";
            }

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                p = CloneParameter(types[cols.DateModified.ColumnName]);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                into += comma + Delimiters.ColumnOpen + cols.DateModified.ColumnName + Delimiters.ColumnClose;
                values += comma + request.ProviderMetadata["DateModified.ServerSideText"];
                comma = ", ";

                defaults += defaultComma + cols.DateModified.ColumnName;
                defaultComma = ",";
            }

            if (cols.AddedBy != null && cols.AddedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.AddedBy.ColumnName]);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                into += comma + Delimiters.ColumnOpen + cols.AddedBy.ColumnName + Delimiters.ColumnClose;
                values += comma + request.ProviderMetadata["AddedBy.ServerSideText"];
                comma = ", ";

                defaults += defaultComma + cols.AddedBy.ColumnName;
                defaultComma = ",";

                esColumnMetadata col = request.Columns[cols.ModifiedBy.ColumnName];

                if (col.CharacterMaxLength > 0)
                {
                    p.Size = (int)col.CharacterMaxLength;
                }
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.ModifiedBy.ColumnName]);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                into += comma + Delimiters.ColumnOpen + cols.ModifiedBy.ColumnName + Delimiters.ColumnClose;
                values += comma + request.ProviderMetadata["ModifiedBy.ServerSideText"];
                comma = ", ";

                defaults += defaultComma + cols.ModifiedBy.ColumnName;
                defaultComma = ",";

                esColumnMetadata col = request.Columns[cols.ModifiedBy.ColumnName];

                if (col.CharacterMaxLength > 0)
                {
                    p.Size = (int)col.CharacterMaxLength;
                }
            }
            #endregion

            string fullName = CreateFullName(request);

            sql += " INSERT INTO " + fullName;

            if (into.Length != 0)
            {
                sql += " (" + into + ") VALUES (" + values + ")";
            }
            else
            {
                sql += " DEFAULT VALUES;";
            }

            sql += autoInc;

            if (defaults.Length > 0)
            {
                sql += " SELECT " + defaults + " FROM " + fullName + " WHERE (" + where + ")";
            }

            cmd.CommandText = sql + String.Empty;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public NpgsqlCommand BuildDynamicUpdateCommand(esDataRequest request, esEntitySavePacket packet)
        {
            string where = String.Empty;
            string conncur = String.Empty;
            string scomma = String.Empty;
            string defaults = String.Empty;
            string defaultsComma = String.Empty;
            string and = String.Empty;

            string sql = "UPDATE " + CreateFullName(request) + " SET ";

            PropertyCollection props = new PropertyCollection();
            NpgsqlParameter p = null;

            Dictionary<string, NpgsqlParameter> types = Cache.GetParameters(request);

            NpgsqlCommand cmd = new NpgsqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            esColumnMetadataCollection cols = request.Columns;
            foreach (esColumnMetadata col in cols)
            {
                bool isModified = packet.ModifiedColumns == null ? false : packet.ModifiedColumns.Contains(col.Name);

                if (isModified && (!col.IsAutoIncrement && !col.IsConcurrency && !col.IsEntitySpacesConcurrency))
                {
                    p = cmd.Parameters.Add(CloneParameter(types[col.Name]));

                    object value = packet.CurrentValues[col.Name];
                    p.Value = value != null ? value : DBNull.Value;

                    sql += scomma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    scomma = ", ";
                }
                else if (col.IsAutoIncrement)
                {
                    // Nothing to do but leave this here
                }
                else if (col.IsConcurrency)
                {
                    p = CloneParameter(types[col.Name]);
                    p.SourceVersion = DataRowVersion.Original;
                    p.Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add(p);

                    conncur += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                }
                else if (col.IsEntitySpacesConcurrency)
                {
                    p = CloneParameter(types[col.Name]);
                    p.Value = packet.OriginalValues[col.Name];
                    p.Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add(p);

                    sql += scomma;
                    sql += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName + " + 1";

                    conncur += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;

                    defaults += defaultsComma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    defaultsComma = ",";
                }
                else if (col.IsComputed)
                {
                    // Do nothing but leave this here
                }
                else if (cols.IsSpecialColumn(col))
                {
                    // Do nothing but leave this here
                }
                else if (col.HasDefault)
                {
                    // defaults += defaultsComma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    // defaultsComma = ",";
                }

                if (col.IsInPrimaryKey)
                {
                    p = CloneParameter(types[col.Name]);
                    p.Value = packet.OriginalValues[col.Name];
                    cmd.Parameters.Add(p);

                    where += and + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    and = " AND ";
                }
            }

            #region Special Column Logic
            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                p = CloneParameter(types[cols.DateModified.ColumnName]);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                sql += scomma + Delimiters.ColumnOpen + cols.DateModified.ColumnName + Delimiters.ColumnClose + " = " + request.ProviderMetadata["DateModified.ServerSideText"];
                scomma = ", ";

                defaults += defaultsComma + cols.DateModified.ColumnName;
                defaultsComma = ",";
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                p = CloneParameter(types[cols.ModifiedBy.ColumnName]);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                sql += scomma + Delimiters.ColumnOpen + cols.ModifiedBy.ColumnName + Delimiters.ColumnClose + " = " + request.ProviderMetadata["ModifiedBy.ServerSideText"];
                scomma = ", ";

                defaults += defaultsComma + cols.ModifiedBy.ColumnName;
                defaultsComma = ",";

                esColumnMetadata col = request.Columns[cols.ModifiedBy.ColumnName];

                if (col.CharacterMaxLength > 0)
                {
                    p.Size = (int)col.CharacterMaxLength;
                }
            }
            #endregion

            sql += " WHERE " + where + "";
            if (conncur.Length > 0)
            {
                sql += " AND " + conncur;
            }

            if (defaults.Length > 0)
            {
                sql += "; SELECT " + defaults + " FROM " + CreateFullName(request) + " WHERE (" + where + ")";
            }

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public NpgsqlCommand BuildDynamicDeleteCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, NpgsqlParameter> types = Cache.GetParameters(request);

            NpgsqlCommand cmd = new NpgsqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            string sql = "DELETE FROM " + CreateFullName(request) + " ";

            string comma = String.Empty;
            comma = String.Empty;
            sql += " WHERE ";
            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsInPrimaryKey || col.IsEntitySpacesConcurrency)
                {
                    NpgsqlParameter p = types[col.Name];
                    p = cmd.Parameters.Add(CloneParameter(p));
                    p.Value = packet.OriginalValues[col.Name];

                    sql += comma;
                    sql += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    comma = " AND ";
                }
            }

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public NpgsqlCommand BuildStoredProcInsertCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, NpgsqlParameter> types = Cache.GetParameters(request);

            NpgsqlCommand cmd = new NpgsqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = Delimiters.StoredProcNameOpen + request.ProviderMetadata.spInsert + Delimiters.StoredProcNameClose;

            PopulateStoredProcParameters(cmd, request, packet);

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.HasDefault && col.Default.ToLower().Contains("newid"))
                {
                    NpgsqlParameter p = types[col.Name];
                    p = cmd.Parameters[p.ParameterName];
                    p.Direction = ParameterDirection.InputOutput;
                }
                else if (col.IsComputed || col.IsAutoIncrement)
                {
                    NpgsqlParameter p = types[col.Name];
                    p = cmd.Parameters[p.ParameterName];
                    p.Direction = ParameterDirection.Output;
                }
            }

            return cmd;
        }

        static public NpgsqlCommand BuildStoredProcUpdateCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, NpgsqlParameter> types = Cache.GetParameters(request);

            NpgsqlCommand cmd = new NpgsqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = Delimiters.StoredProcNameOpen + request.ProviderMetadata.spUpdate + Delimiters.StoredProcNameClose;

            PopulateStoredProcParameters(cmd, request, packet);

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsComputed)
                {
                    NpgsqlParameter p = types[col.Name];
                    p = cmd.Parameters[p.ParameterName];
                    p.Direction = ParameterDirection.InputOutput;
                }
            }

            return cmd;
        }

        static public NpgsqlCommand BuildStoredProcDeleteCommand(esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, NpgsqlParameter> types = Cache.GetParameters(request);

            NpgsqlCommand cmd = new NpgsqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = Delimiters.StoredProcNameOpen + request.ProviderMetadata.spDelete + Delimiters.StoredProcNameClose;

            NpgsqlParameter p;

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

        static public void PopulateStoredProcParameters(NpgsqlCommand cmd, esDataRequest request, esEntitySavePacket packet)
        {
            Dictionary<string, NpgsqlParameter> types = Cache.GetParameters(request);

            NpgsqlParameter p;

            foreach (esColumnMetadata col in request.Columns)
            {
                p = types[col.Name];
                p = CloneParameter(p);

                if (packet.CurrentValues.ContainsKey(col.Name))
                {
                    p.Value = packet.CurrentValues[col.Name];
                }

                if (p.NpgsqlDbType == NpgsqlDbType.Timestamp)
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

        static private NpgsqlParameter CloneParameter(NpgsqlParameter p)
        {
            ICloneable param = p as ICloneable;
            return param.Clone() as NpgsqlParameter;
        }

        static public string CreateFullName(esDataRequest request, esDynamicQuery query)
        {
            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

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

            if ((request.Catalog != null || request.ProviderMetadata.Catalog != null) &&
                 (request.Schema != null || request.ProviderMetadata.Schema != null))
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

        static public esConcurrencyException CheckForConcurrencyException(NpgsqlException ex)
        {
            esConcurrencyException ce = null;
            return ce;
        }

        static public void AddParameters(NpgsqlCommand cmd, esDataRequest request)
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

                    cmd.Parameters.AddWithValue(Delimiters.Param + esParam.Name, esParam.Value);
                }
            }
            else
            {
                NpgsqlParameter param;

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
                }
            }
        }

        static public void GatherReturnParameters(NpgsqlCommand cmd, esDataRequest request, esDataResponse response)
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
                            NpgsqlParameter p = cmd.Parameters[Delimiters.Param + esParam.Name];
                            esParam.Value = p.Value;
                        }
                    }
                }
            }
        }
    }
}
