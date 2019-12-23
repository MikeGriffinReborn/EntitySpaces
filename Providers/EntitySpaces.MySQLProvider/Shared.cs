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

using MySql.Data.MySqlClient;

namespace EntitySpaces.MySQLProvider
{
    class Shared
    {
        static public MySqlCommand BuildDynamicInsertCommand(esDataRequest request, List<string> modifiedColumns)
        {
            string sql = String.Empty;
            string defaults = String.Empty;
            string into = String.Empty;
            string values = String.Empty;
            string comma = String.Empty;
            string defaultComma = String.Empty;
            string where = String.Empty;
            string whereComma = String.Empty;

            PropertyCollection props = new PropertyCollection();
            MySqlParameter p = null;

            Dictionary<string, MySqlParameter> types = Cache.GetParameters(request);

            MySqlCommand cmd = new MySqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            esColumnMetadataCollection cols = request.Columns;
            foreach (esColumnMetadata col in cols)
            {
                bool isModified = modifiedColumns == null ? false : modifiedColumns.Contains(col.Name);

                if (isModified && (!col.IsAutoIncrement && !col.IsConcurrency && !col.IsEntitySpacesConcurrency))
                {
                    p = types[col.Name];
                    cmd.Parameters.Add(CloneParameter(p));

                    into += comma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    values += comma + p.ParameterName;
                    comma = ", ";
                }
                else if (col.IsAutoIncrement)
                {
                    props["AutoInc"] = col.Name;
                    props["Source"] = request.ProviderMetadata.Source;

                    p = CloneParameter(types[col.Name]);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(p);
                }
                else if (col.IsConcurrency)
                {
                    props["Timestamp"] = col.Name;
                    props["Source"] = request.ProviderMetadata.Source;

                    p = CloneParameter(types[col.Name]);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(p);
                }
                else if (col.IsEntitySpacesConcurrency)
                {
                    into += comma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    values += comma + "1";
                    comma = ", ";

                    p = CloneParameter(types[col.Name]);
                    p.Direction = ParameterDirection.Output;
                    p.Value = 1; // Seems to work, We'll take it ...
                    cmd.Parameters.Add(p);
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
                    defaults += defaultComma + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose;
                    defaultComma = ",";
                }

                if (col.IsInPrimaryKey)
                {
                    where += whereComma + col.Name;
                    whereComma = ",";
                }
            }

            #region Special Columns
            if (cols.DateAdded != null && cols.DateAdded.IsServerSide)
            {
                into += comma + Delimiters.ColumnOpen + cols.DateAdded.ColumnName + Delimiters.ColumnClose;
                values += comma + request.ProviderMetadata["DateAdded.ServerSideText"];
                comma = ", ";

                defaults += defaultComma + Delimiters.ColumnOpen + cols.DateAdded.ColumnName + Delimiters.ColumnClose;
                defaultComma = ",";
            }

            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                into += comma + Delimiters.ColumnOpen + cols.DateModified.ColumnName + Delimiters.ColumnClose;
                values += comma + request.ProviderMetadata["DateModified.ServerSideText"];
                comma = ", ";

                defaults += defaultComma + Delimiters.ColumnOpen + cols.DateModified.ColumnName + Delimiters.ColumnClose;
                defaultComma = ",";
            }

            if (cols.AddedBy != null && cols.AddedBy.IsServerSide)
            {
                into += comma + Delimiters.ColumnOpen + cols.AddedBy.ColumnName + Delimiters.ColumnClose;
                values += comma + request.ProviderMetadata["AddedBy.ServerSideText"];
                comma = ", ";

                defaults += defaultComma + Delimiters.ColumnOpen + cols.AddedBy.ColumnName + Delimiters.ColumnClose;
                defaultComma = ",";
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                into += comma + Delimiters.ColumnOpen + cols.ModifiedBy.ColumnName + Delimiters.ColumnClose;
                values += comma + request.ProviderMetadata["ModifiedBy.ServerSideText"];
                comma = ", ";

                defaults += defaultComma + Delimiters.ColumnOpen + cols.ModifiedBy.ColumnName + Delimiters.ColumnClose;
                defaultComma = ",";
            }
            #endregion

            if (defaults.Length > 0)
            {
                comma = String.Empty;
                props["Defaults"] = defaults;
                props["Where"] = where;
            }

            sql += " INSERT INTO " + CreateFullName(request);

            if (into.Length != 0)
            {
                sql += "(" + into + ") VALUES (" + values + ")";
            }
            else
            {
                sql += "DEFAULT VALUES";
            }

            request.Properties = props;

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public MySqlCommand BuildDynamicUpdateCommand(esDataRequest request, List<string> modifiedColumns)
        {
            string where = String.Empty;
            string scomma = String.Empty;
            string wcomma = String.Empty;
            string defaults = String.Empty;
            string defaultsWhere = String.Empty;
            string defaultsComma = String.Empty;
            string defaultsWhereComma = String.Empty;

            string sql = "UPDATE " + CreateFullName(request) + " SET ";

            PropertyCollection props = new PropertyCollection();
            MySqlParameter p = null;

            Dictionary<string, MySqlParameter> types = Cache.GetParameters(request);

            MySqlCommand cmd = new MySqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            esColumnMetadataCollection cols = request.Columns;
            foreach (esColumnMetadata col in cols)
            {
                bool isModified = modifiedColumns == null ? false : modifiedColumns.Contains(col.Name);

                if (isModified && (!col.IsAutoIncrement && !col.IsConcurrency && !col.IsEntitySpacesConcurrency))
                {
                    p = CloneParameter(types[col.Name]);
                    cmd.Parameters.Add(p);

                    sql += scomma;
                    sql += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    scomma = ", ";
                }
                else if (col.IsAutoIncrement)
                {
                    // Nothing to do but leave this here
                }
                else if (col.IsConcurrency)
                {
                    props["Timestamp"] = col.Name;
                    props["Source"] = request.ProviderMetadata.Source;

                    p = CloneParameter(types[col.Name]);
                    p.SourceVersion = DataRowVersion.Original;
                    p.Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add(p);

                    where += wcomma;
                    where += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    wcomma = " AND ";
                }
                else if (col.IsEntitySpacesConcurrency)
                {
                    props["EntitySpacesConcurrency"] = col.Name;

                    p = CloneParameter(types[col.Name]);
                    p.SourceVersion = DataRowVersion.Original;
                    p.Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add(p);

                    sql += scomma;
                    sql += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " + 1";
                    scomma = ", ";

                    where += wcomma;
                    where += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    wcomma = " AND ";
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
                    p.SourceVersion = DataRowVersion.Original;
                    cmd.Parameters.Add(p);

                    where += wcomma;
                    where += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    wcomma = " AND ";

                    defaultsWhere += defaultsWhereComma + col.Name;
                    defaultsWhereComma = ",";
                }
            }

            #region Special Columns
            if (cols.DateModified != null && cols.DateModified.IsServerSide)
            {
                sql += scomma;
                sql += Delimiters.ColumnOpen + cols.DateModified.ColumnName + Delimiters.ColumnClose + " = " + request.ProviderMetadata["DateModified.ServerSideText"];
                scomma = ", ";

                defaults += defaultsComma + Delimiters.ColumnOpen + cols.DateModified.ColumnName + Delimiters.ColumnClose;
                defaultsComma = ",";
            }

            if (cols.ModifiedBy != null && cols.ModifiedBy.IsServerSide)
            {
                sql += scomma;
                sql += Delimiters.ColumnOpen + cols.ModifiedBy.ColumnName + Delimiters.ColumnClose + " = " + request.ProviderMetadata["ModifiedBy.ServerSideText"];
                scomma = ", ";

                defaults += defaultsComma + Delimiters.ColumnOpen + cols.ModifiedBy.ColumnName + Delimiters.ColumnClose;
                defaultsComma = ",";
            }
            #endregion

            if (defaults.Length > 0)
            {
                props["Defaults"] = defaults;
                props["Where"] = defaultsWhere;
            }

            sql += " WHERE " + where + ";";

            request.Properties = props;

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public MySqlCommand BuildDynamicDeleteCommand(esDataRequest request, List<string> modifiedColumns)
        {
            Dictionary<string, MySqlParameter> types = Cache.GetParameters(request);

            MySqlCommand cmd = new MySqlCommand();
            if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            string sql = "DELETE FROM " + CreateFullName(request) + " ";

            string comma = String.Empty;
            comma = String.Empty;
            sql += " WHERE ";
            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsInPrimaryKey || col.IsEntitySpacesConcurrency)
                {
                    MySqlParameter p = types[col.Name];
                    cmd.Parameters.Add(CloneParameter(p));

                    sql += comma;
                    sql += Delimiters.ColumnOpen + col.Name + Delimiters.ColumnClose + " = " + p.ParameterName;
                    comma = " AND ";
                }
            }

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        static public MySqlCommand BuildStoredProcInsertCommand(esDataRequest request)
        {
            MySqlCommand cmd = new MySqlCommand();
            if(request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = Delimiters.StoredProcNameOpen + request.ProviderMetadata.spInsert + Delimiters.StoredProcNameClose;

            PopulateStoredProcParameters(cmd, request);

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsComputed || col.IsAutoIncrement || col.IsEntitySpacesConcurrency)
                {
                    MySqlParameter p = cmd.Parameters["?p" + (col.Name).Replace(" ", String.Empty)];
                    p.Direction = ParameterDirection.Output;
                }
            }

            return cmd;
        }

        static public MySqlCommand BuildStoredProcUpdateCommand(esDataRequest request)
        {
            MySqlCommand cmd = new MySqlCommand();
            if(request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = Delimiters.StoredProcNameOpen + request.ProviderMetadata.spUpdate + Delimiters.StoredProcNameClose;

            PopulateStoredProcParameters(cmd, request);

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsComputed || col.IsEntitySpacesConcurrency)
                {
                    MySqlParameter p = cmd.Parameters["?p" + (col.Name).Replace(" ", String.Empty)];
                    p.SourceVersion = DataRowVersion.Original;
                    p.Direction = ParameterDirection.InputOutput;
                }
            }

            return cmd;
        }

        static public MySqlCommand BuildStoredProcDeleteCommand(esDataRequest request)
        {
            MySqlCommand cmd = new MySqlCommand();
            if(request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = Delimiters.StoredProcNameOpen + request.ProviderMetadata.spDelete + Delimiters.StoredProcNameClose;

            Dictionary<string, MySqlParameter> types = Cache.GetParameters(request);

            MySqlParameter p;

            foreach (esColumnMetadata col in request.Columns)
            {
                if (col.IsInPrimaryKey || col.IsEntitySpacesConcurrency)
                {
                    p = types[col.Name];
                    p = CloneParameter(p);
                    p.ParameterName = p.ParameterName.Replace("?", "?p");
                    p.SourceVersion = DataRowVersion.Current;
                    cmd.Parameters.Add(p);
                }
            }

            return cmd;
        }

        static public void PopulateStoredProcParameters(MySqlCommand cmd, esDataRequest request)
        {
            Dictionary<string, MySqlParameter> types = Cache.GetParameters(request);

            MySqlParameter p;

            foreach (esColumnMetadata col in request.Columns)
            {
                p = types[col.Name];
                p = CloneParameter(p);
                p.ParameterName = p.ParameterName.Replace("?", "?p");
                p.SourceVersion = DataRowVersion.Current;

                if (p.MySqlDbType == MySqlDbType.Timestamp)
                {
                    p.Direction = ParameterDirection.InputOutput;
                }
                cmd.Parameters.Add(p);
            }
        }

        static private MySqlParameter CloneParameter(MySqlParameter p)
        {
            ICloneable param = p as ICloneable;
            return param.Clone() as MySqlParameter;
        }

        static public bool HasUpdates(DataRowCollection rows, out bool insert, out bool update, out bool delete)
        {
            insert = false;
            update = false;
            delete = false;

            for (int i = 0; i < rows.Count; i++)
            {
                switch (rows[i].RowState)
                {
                    case DataRowState.Added:
                        insert = true;
                        break;
                    case DataRowState.Modified:
                        update = true;
                        break;
                    case DataRowState.Deleted:
                        delete = true;
                        break;
                }
            }

            return (insert || update || delete) ? true : false;
        }

        static public bool HasUpdate(DataRow row, out bool insert, out bool update, out bool delete)
        {
            insert = false;
            update = false;
            delete = false;

            switch (row.RowState)
            {
                case DataRowState.Added:
                    insert = true;
                    break;
                case DataRowState.Modified:
                    update = true;
                    break;
                case DataRowState.Deleted:
                    delete = true;
                    break;
            }

            return (insert || update || delete) ? true : false;
        }

        static public string CreateFullName(esDataRequest request, esDynamicQuery query)
        {
            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            esProviderSpecificMetadata providerMetadata = iQuery.ProviderMetadata as esProviderSpecificMetadata;

            string name = String.Empty;

            string catalog = iQuery.Catalog ?? request.Catalog ?? providerMetadata.Catalog;
            string schema = iQuery.Schema ?? request.Schema ?? providerMetadata.Schema;

            if (catalog != null)
            {
                name += Delimiters.TableOpen + catalog + Delimiters.TableClose + ".";
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

            if (catalog != null)
            {
                name += Delimiters.TableOpen + catalog + Delimiters.TableClose + ".";
            }

            name += Delimiters.TableOpen;
            if(request.DynamicQuery != null && request.DynamicQuery.querySource != null)
                name += request.DynamicQuery.querySource;
            else
                name += request.QueryText != null ? request.QueryText : request.ProviderMetadata.Destination;
            name += Delimiters.TableClose;

            return name;
        }

        static public esConcurrencyException CheckForConcurrencyException(MySqlException ex)
        {
            esConcurrencyException ce = null;

            if (ex != null)
            {
                if (ex.Number == 532)
                {
                    ce = new esConcurrencyException(ex.Message, ex);
                    ce.Source = ex.Source;
                }
            }

            return ce;
        }

        static public void AddParameters(MySqlCommand cmd, esDataRequest request)
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
                MySqlParameter param;

                string paramPrefix = request.ProviderMetadata.spLoadByPrimaryKey == cmd.CommandText ? Delimiters.Param + "p" : Delimiters.Param;

                foreach (esParameter esParam in request.Parameters)
                {
                    param = cmd.Parameters.AddWithValue(paramPrefix + esParam.Name, esParam.Value);

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
                            param.Scale = esParam.Scale;
                            param.Precision = esParam.Precision;
                            break;

                        case esParameterDirection.ReturnValue:
                            param.Direction = ParameterDirection.ReturnValue;
                            break;
                    }
                }
            }
        }

        static public void GatherReturnParameters(MySqlCommand cmd, esDataRequest request, esDataResponse response)
        {
            if (cmd.Parameters.Count > 0)
            {
                if (request.Parameters != null && request.Parameters.Count > 0)
                {
                    string paramPrefix = request.ProviderMetadata.spLoadByPrimaryKey == cmd.CommandText ? Delimiters.Param + "p" : Delimiters.Param;

                    response.Parameters = new esParameters();

                    foreach (esParameter esParam in request.Parameters)
                    {
                        if (esParam.Direction != esParameterDirection.Input)
                        {
                            response.Parameters.Add(esParam);
                            MySqlParameter p = cmd.Parameters[paramPrefix + esParam.Name];
                            esParam.Value = p.Value;
                        }
                    }
                }
            }
        }
    }
}
