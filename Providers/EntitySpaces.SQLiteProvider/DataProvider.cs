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
using System.Linq;
using System.Data;
using System.Data.SQLite;

using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace EntitySpaces.SQLiteProvider
{
    public class DataProvider : IDataProvider
    {
        public DataProvider()
        {

        }

        #region esTraceArguments

        private sealed class esTraceArguments : EntitySpaces.Interfaces.ITraceArguments, IDisposable
        {
            static private long packetOrder = 0;

            private sealed class esTraceParameter : ITraceParameter
            {
                public string Name { get; set; }
                public string Direction { get; set; }
                public string ParamType { get; set; }
                public string BeforeValue { get; set; }
                public string AfterValue { get; set; }
            }

            public esTraceArguments()
            {

            }

            public esTraceArguments(esDataRequest request, IDbCommand cmd, esEntitySavePacket packet, string action, string callStack)
            {
                PacketOrder = Interlocked.Increment(ref esTraceArguments.packetOrder);

                this.command = cmd;

                TraceChannel = DataProvider.sTraceChannel;
                Syntax = "SQLITE";
                Request = request;
                ThreadId = Thread.CurrentThread.ManagedThreadId;
                Action = action;
                CallStack = callStack;
                SqlCommand = cmd;
                ApplicationName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                IDataParameterCollection parameters = cmd.Parameters;

                if (parameters.Count > 0)
                {
                    Parameters = new List<ITraceParameter>(parameters.Count);

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SQLiteParameter param = parameters[i] as SQLiteParameter;

                        esTraceParameter p = new esTraceParameter()
                        {
                            Name = param.ParameterName,
                            Direction = param.Direction.ToString(),
                            ParamType = param.DbType.ToString().ToUpper(),
                            BeforeValue = param.Value != null && param.Value != DBNull.Value ? Convert.ToString(param.Value) : "null"
                        };

                        try
                        {
                            // Let's make it look like we're using parameters for the profiler
                            if (param.Value == null || param.Value == DBNull.Value)
                            {
                                if (param.SourceVersion == DataRowVersion.Current)
                                {
                                    object o = packet.CurrentValues[param.SourceColumn];
                                    if (o != null && o != DBNull.Value)
                                    {
                                        p.BeforeValue = Convert.ToString(o);
                                    }
                                }
                                else if (param.SourceVersion == DataRowVersion.Original)
                                {
                                    object o = packet.OriginalValues[param.SourceColumn];
                                    if (o != null && o != DBNull.Value)
                                    {
                                        p.BeforeValue = Convert.ToString(o);
                                    }
                                }
                            }
                        }
                        catch { }

                        this.Parameters.Add(p);
                    }
                }

                stopwatch = Stopwatch.StartNew();
            }

            public esTraceArguments(esDataRequest request, IDbCommand cmd, string action, string callStack)
            {
                PacketOrder = Interlocked.Increment(ref esTraceArguments.packetOrder);

                this.command = cmd;

                TraceChannel = DataProvider.sTraceChannel;
                Syntax = "SQLITE";
                Request = request;
                ThreadId = Thread.CurrentThread.ManagedThreadId;
                Action = action;
                CallStack = callStack;
                SqlCommand = cmd;
                ApplicationName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                IDataParameterCollection parameters = cmd.Parameters;

                if (parameters.Count > 0)
                {
                    Parameters = new List<ITraceParameter>(parameters.Count);

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SQLiteParameter param = parameters[i] as SQLiteParameter;

                        esTraceParameter p = new esTraceParameter()
                        {
                            Name = param.ParameterName,
                            Direction = param.Direction.ToString(),
                            ParamType = param.DbType.ToString().ToUpper(),
                            BeforeValue = param.Value != null ? Convert.ToString(param.Value) : "null"
                        };

                        this.Parameters.Add(p);
                    }
                }

                stopwatch = Stopwatch.StartNew();
            }

            // Temporary variable
            private IDbCommand command;

            public long PacketOrder { get; set; }
            public string Syntax { get; set; }
            public esDataRequest Request { get; set; }
            public int ThreadId { get; set; }
            public string Action { get; set; }
            public string CallStack { get; set; }
            public IDbCommand SqlCommand { get; set; }
            public string ApplicationName { get; set; }
            public string TraceChannel { get; set; }
            public long Duration { get; set; }
            public long Ticks { get; set; }
            public string Exception { get; set; }
            public List<ITraceParameter> Parameters { get; set; }

            private Stopwatch stopwatch;

            void IDisposable.Dispose()
            {
                stopwatch.Stop();
                Duration = stopwatch.ElapsedMilliseconds;
                Ticks = stopwatch.ElapsedTicks;

                // Gather Output Parameters
                if (this.Parameters != null && this.Parameters.Count > 0)
                {
                    IDataParameterCollection parameters = command.Parameters;

                    for (int i = 0; i < this.Parameters.Count; i++)
                    {
                        ITraceParameter esParam = this.Parameters[i];
                        IDbDataParameter param = parameters[esParam.Name] as IDbDataParameter;

                        if (param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output)
                        {
                            esParam.AfterValue = param.Value != null ? Convert.ToString(param.Value) : "null";
                        }
                    }
                }

                DataProvider.sTraceHandler(this);
            }
        }

        #endregion

        #region Profiling Logic

        /// <summary>
        /// The EventHandler used to decouple the profiling code from the core assemblies
        /// </summary>
        event TraceEventHandler IDataProvider.TraceHandler
        {
            add { DataProvider.sTraceHandler += value; }
            remove { DataProvider.sTraceHandler -= value; }
        }
        static private event TraceEventHandler sTraceHandler;

        /// <summary>
        /// Returns true if this Provider is current being profiled
        /// </summary>
        bool IDataProvider.IsTracing
        {
            get
            {
                return sTraceHandler != null ? true : false;
            }
        }

        /// <summary>
        /// Used to set the Channel this provider is to use during Profiling
        /// </summary>
        string IDataProvider.TraceChannel
        {
            get { return DataProvider.sTraceChannel; }
            set { DataProvider.sTraceChannel = value; }
        }
        static private string sTraceChannel = "Channel1";

        #endregion

        /// <summary>
        /// This method acts as a delegate for esTransactionScope
        /// </summary>
        /// <returns></returns>
        static private IDbConnection CreateIDbConnectionDelegate()
        {
            return new SQLiteConnection();
        }

        static private void CleanupCommand(SQLiteCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }

        #region IDataProvider Members

        esDataResponse IDataProvider.esLoadDataTable(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();

            try
            {
                switch (request.QueryType)
                {
                    case esQueryType.StoredProcedure:

                        response = LoadDataTableFromStoredProcedure(request);
                        break;

                    case esQueryType.Text:

                        response = LoadDataTableFromText(request);
                        break;

                    case esQueryType.DynamicQuery:

                        response = new esDataResponse();
                        SQLiteCommand cmd = QueryBuilder.PrepareCommand(request);
                        LoadDataTableFromDynamicQuery(request, response, cmd);
                        break;

                    case esQueryType.DynamicQueryParseOnly:

                        response = new esDataResponse();
                        SQLiteCommand cmd1 = QueryBuilder.PrepareCommand(request);
                        response.LastQuery = cmd1.CommandText;
                        break;

                    //case esQueryType.IQueryable:

                    //    response = new esDataResponse();
                    //    LoadDataTableForLinqToSql(request, response);
                    //    break;

                    case esQueryType.ManyToMany:

                        response = LoadManyToMany(request);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        esDataResponse IDataProvider.esSaveDataTable(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();

            try
            {
                if (request.SqlAccessType == esSqlAccessType.StoredProcedure)
                {
                    if (request.CollectionSavePacket != null)
                        SaveStoredProcCollection(request);
                    else
                        SaveStoredProcEntity(request);
                }
                else
                {
                    if (request.EntitySavePacket.CurrentValues == null)
                        SaveDynamicCollection(request);
                    else
                        SaveDynamicEntity(request);
                }
            }
            catch (SQLiteException ex)
            {
                esException es = Shared.CheckForConcurrencyException(ex);
                if (es != null)
                    response.Exception = es;
                else
                    response.Exception = ex;
            }
            catch (DBConcurrencyException dbex)
            {
                response.Exception = new esConcurrencyException("Error in SQLiteProvider.esSaveDataTable", dbex);
            }

            response.Table = request.Table;
            return response;
        }

        esDataResponse IDataProvider.ExecuteNonQuery(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand();
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null)	Shared.AddParameters(cmd, request);
                
                switch (request.QueryType)
                {
                    case esQueryType.TableDirect:
                        cmd.CommandType = CommandType.TableDirect;
                        cmd.CommandText = request.QueryText;
                        break;

                    case esQueryType.StoredProcedure:
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = Shared.CreateFullName(request);
                        break;

                    case esQueryType.Text:
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = request.QueryText;
                        break;
                }

                try
                {
                    esTransactionScope.Enlist(cmd, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "ExecuteNonQuery", System.Environment.StackTrace))
                        {
                            try
                            {
                                response.RowsEffected = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        response.RowsEffected = cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(cmd);
                }

                if (request.Parameters != null)
                {
                    Shared.GatherReturnParameters(cmd, request, response);
                }
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                response.Exception = ex;
            }

            return response;
        }

        esDataResponse IDataProvider.ExecuteReader(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand();
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                switch (request.QueryType)
                {
                    case esQueryType.TableDirect:
                        cmd.CommandType = CommandType.TableDirect;
                        cmd.CommandText = request.QueryText;
                        break;

                    case esQueryType.StoredProcedure:
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = Shared.CreateFullName(request);
                        break;

                    case esQueryType.Text:
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = request.QueryText;
                        break;

                    case esQueryType.DynamicQuery:
                        cmd = QueryBuilder.PrepareCommand(request);
                        break;
                }

                cmd.Connection = new SQLiteConnection(request.ConnectionString);
                cmd.Connection.Open();

                #region Profiling
                if (sTraceHandler != null)
                {
                    using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "ExecuteReader", System.Environment.StackTrace))
                    {
                        try
                        {
                            response.DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        }
                        catch (Exception ex)
                        {
                            esTrace.Exception = ex.Message;
                            throw;
                        }
                    }
                }
                else
                #endregion
                {
                    response.DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                response.Exception = ex;
            }

            return response;
        }

        esDataResponse IDataProvider.ExecuteScalar(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand();
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                switch (request.QueryType)
                {
                    case esQueryType.TableDirect:
                        cmd.CommandType = CommandType.TableDirect;
                        cmd.CommandText = request.QueryText;
                        break;

                    case esQueryType.StoredProcedure:
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = Shared.CreateFullName(request);
                        break;

                    case esQueryType.Text:
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = request.QueryText;
                        break;

                    case esQueryType.DynamicQuery:
                        cmd = QueryBuilder.PrepareCommand(request);
                        break;
                }

                try
                {
                    esTransactionScope.Enlist(cmd, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "ExecuteScalar", System.Environment.StackTrace))
                        {
                            try
                            {
                                response.Scalar = cmd.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        response.Scalar = cmd.ExecuteScalar();
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(cmd);
                }

                if (request.Parameters != null)
                {
                    Shared.GatherReturnParameters(cmd, request, response);
                }
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                response.Exception = ex;
            }

            return response;
        }

        esDataResponse IDataProvider.FillDataSet(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();

            try
            {
                switch (request.QueryType)
                {
                    case esQueryType.StoredProcedure:

                        response = LoadDataSetFromStoredProcedure(request);
                        break;

                    case esQueryType.Text:

                        response = LoadDataSetFromText(request);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        esDataResponse IDataProvider.FillDataTable(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();

            try
            {
                switch (request.QueryType)
                {
                    case esQueryType.StoredProcedure:

                        response = LoadDataTableFromStoredProcedure(request);
                        break;

                    case esQueryType.Text:

                        response = LoadDataTableFromText(request);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        #endregion

        static private esDataResponse LoadDataSetFromStoredProcedure(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            SQLiteCommand cmd = null;

            try
            {
                DataSet dataSet = new DataSet();

                cmd = new SQLiteCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Shared.CreateFullName(request);

                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                SQLiteDataAdapter da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;

                try
                {
                    esTransactionScope.Enlist(da.SelectCommand, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "LoadFromStoredProcedure", System.Environment.StackTrace))
                        {
                            try
                            {
                                da.Fill(dataSet);
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        da.Fill(dataSet);
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(da.SelectCommand);
                }

                response.DataSet = dataSet;

                if (request.Parameters != null)
                {
                    Shared.GatherReturnParameters(cmd, request, response);
                }
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                throw ex;
            }
            finally
            {

            }

            return response;
        }

        static private esDataResponse LoadDataSetFromText(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            SQLiteCommand cmd = null;

            try
            {
                DataSet dataSet = new DataSet();

                cmd = new SQLiteCommand();
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                SQLiteDataAdapter da = new SQLiteDataAdapter();
                cmd.CommandText = request.QueryText;
                da.SelectCommand = cmd;

                try
                {
                    esTransactionScope.Enlist(da.SelectCommand, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "LoadDataSetFromText", System.Environment.StackTrace))
                        {
                            try
                            {
                                da.Fill(dataSet);
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        da.Fill(dataSet);
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(da.SelectCommand);
                }

                response.DataSet = dataSet;

                if (request.Parameters != null)
                {
                    Shared.GatherReturnParameters(cmd, request, response);
                }
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                throw ex;
            }
            finally
            {

            }

            return response;
        }

        static private esDataResponse LoadDataTableFromStoredProcedure(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            SQLiteCommand cmd = null;

            try
            {
                DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

                cmd = new SQLiteCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Shared.CreateFullName(request);
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                SQLiteDataAdapter da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;

                try
                {
                    esTransactionScope.Enlist(da.SelectCommand, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "LoadFromStoredProcedure", System.Environment.StackTrace))
                        {
                            try
                            {
                                da.Fill(dataTable);
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        da.Fill(dataTable);
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(da.SelectCommand);
                }

                response.Table = dataTable;

                if (request.Parameters != null)
                {
                    Shared.GatherReturnParameters(cmd, request, response);
                }
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                throw ex;
            }
            finally
            {

            }

            return response;
        }

        static private esDataResponse LoadDataTableFromText(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            SQLiteCommand cmd = null;

            try
            {
                DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

                cmd = new SQLiteCommand();
                cmd.CommandType = CommandType.Text;
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                SQLiteDataAdapter da = new SQLiteDataAdapter();
                cmd.CommandText = request.QueryText;
                da.SelectCommand = cmd;

                try
                {
                    esTransactionScope.Enlist(da.SelectCommand, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "LoadFromText", System.Environment.StackTrace))
                        {
                            try
                            {
                                da.Fill(dataTable);
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        da.Fill(dataTable);
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(da.SelectCommand);
                }

                response.Table = dataTable;

                if (request.Parameters != null)
                {
                    Shared.GatherReturnParameters(cmd, request, response);
                }
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                throw ex;
            }
            finally
            {

            }

            return response;
        }

        static private esDataResponse LoadManyToMany(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            SQLiteCommand cmd = null;

            try
            {
                DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

                cmd = new SQLiteCommand();
                cmd.CommandType = CommandType.Text;
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

                string mmQuery = request.QueryText;

                string[] sections = mmQuery.Split('|');
                string[] tables  = sections[0].Split(',');
                string[] columns = sections[1].Split(',');

                // We build the query, we don't use Delimiters to avoid tons of extra concatentation
                string sql = "SELECT * FROM [" + tables[0];
                sql += "] JOIN [" + tables[1] + "] ON [" + tables[0] + "].[" + columns[0] + "] = [";
                sql += tables[1] + "].[" + columns[1];
                sql += "] WHERE [" + tables[1] + "].[" + sections[2] + "] = @";

                if (request.Parameters != null)
                {
                    foreach (esParameter esParam in request.Parameters)
                    {
                        sql += esParam.Name;
                    }

                    Shared.AddParameters(cmd, request);
                }

                SQLiteDataAdapter da = new SQLiteDataAdapter();
                cmd.CommandText = sql;

                da.SelectCommand = cmd;

                try
                {
                    esTransactionScope.Enlist(da.SelectCommand, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "LoadManyToMany", System.Environment.StackTrace))
                        {
                            try
                            {
                                da.Fill(dataTable);
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        da.Fill(dataTable);
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(da.SelectCommand);
                }

                response.Table = dataTable;
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                throw ex;
            }
            finally
            {

            }

            return response;
        }

        // This is used only to execute the Dynamic Query API
        static private void LoadDataTableFromDynamicQuery(esDataRequest request, esDataResponse response, SQLiteCommand cmd)
        {
            try
            {
                response.LastQuery = cmd.CommandText;

                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

                DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

                SQLiteDataAdapter da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;

                try
                {
                    esTransactionScope.Enlist(da.SelectCommand, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "LoadFromDynamicQuery", System.Environment.StackTrace))
                        {
                            try
                            {
                                da.Fill(dataTable);
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        da.Fill(dataTable);
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(da.SelectCommand);
                }

                response.Table = dataTable;
            }
            catch (Exception ex)
            {
                CleanupCommand(cmd);
                throw ex;
            }
            finally
            {

            }
        }

        // This is used only to execute the Dynamic Query API
        //static private void LoadDataTableForLinqToSql(esDataRequest request, esDataResponse response)
        //{
        //    SQLiteCommand cmd = null;

        //    try
        //    {
        //        DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

        //        cmd = request.LinqContext.GetCommand(request.LinqQuery) as SQLiteCommand;

        //        response.LastQuery = cmd.CommandText;

        //        if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

        //        SQLiteDataAdapter da = new SQLiteDataAdapter();
        //        da.SelectCommand = cmd;

        //        try
        //        {
        //            esTransactionScope.Enlist(da.SelectCommand, request.ConnectionString, CreateIDbConnectionDelegate);

        //            #region Profiling
        //            if (sTraceHandler != null)
        //            {
        //                using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "LoadForLinqToSql", System.Environment.StackTrace))
        //                {
        //                    try
        //                    {
        //                        da.Fill(dataTable);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        esTrace.Exception = ex.Message;
        //                        throw;
        //                    }
        //                }
        //            }
        //            else
        //            #endregion
        //            {
        //                da.Fill(dataTable);
        //            }
        //        }
        //        finally
        //        {
        //            esTransactionScope.DeEnlist(da.SelectCommand);
        //        }

        //        response.Table = dataTable;

        //        if (request.Parameters != null)
        //        {
        //            Shared.GatherReturnParameters(cmd, request, response);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        CleanupCommand(cmd);
        //        throw;
        //    }
        //    finally
        //    {

        //    }
        //}

        static private DataTable SaveStoredProcCollection(esDataRequest request)
        {
            throw new NotImplementedException("Stored Procedure Support Not Implemented");
        }

        static private DataTable SaveStoredProcEntity(esDataRequest request)
        {
            throw new NotImplementedException("Stored Procedure Support Not Implemented");
        }

        static private DataTable SaveDynamicCollection(esDataRequest request)
        {
            esEntitySavePacket pkt = request.CollectionSavePacket[0];

            if (pkt.RowState == esDataRowState.Deleted)
            {
                //============================================================================
                // We do all our deletes at once, so if the first one is a delete they all are
                //============================================================================
                return SaveDynamicCollection_Deletes(request);
            }
            else
            {
                //============================================================================
                // We do all our Inserts and Updates at once
                //============================================================================
                return SaveDynamicCollection_InsertsUpdates(request);
            }
        }

        static private DataTable SaveDynamicCollection_InsertsUpdates(esDataRequest request)
        {
            DataTable dataTable = CreateDataTable(request);

            using (esTransactionScope scope = new esTransactionScope())
            {
                using (SQLiteDataAdapter da = new SQLiteDataAdapter())
                {
                    da.AcceptChangesDuringUpdate = false;
                    da.ContinueUpdateOnError = request.ContinueUpdateOnError;

                    SQLiteCommand cmd = null;

                    if (!request.IgnoreComputedColumns)
                    {
                        da.RowUpdated += new EventHandler<System.Data.Common.RowUpdatedEventArgs>(OnRowUpdated);
                    }

                    foreach (esEntitySavePacket packet in request.CollectionSavePacket)
                    {
                        if (packet.RowState != esDataRowState.Added && packet.RowState != esDataRowState.Modified) continue;

                        DataRow row = dataTable.NewRow();
                        dataTable.Rows.Add(row);

                        switch (packet.RowState)
                        {
                            case esDataRowState.Added:
                                cmd = da.InsertCommand = Shared.BuildDynamicInsertCommand(request, packet);
                                SetModifiedValues(request, packet, row);
                                break;

                            case esDataRowState.Modified:
                                cmd = da.UpdateCommand = Shared.BuildDynamicUpdateCommand(request, packet);
                                SetOriginalValues(request, packet, row, false);
                                SetModifiedValues(request, packet, row);
                                row.AcceptChanges();
                                row.SetModified();
                                break;
                        }

                        request.Properties["esDataRequest"] = request;
                        request.Properties["esEntityData"] = packet;
                        dataTable.ExtendedProperties["props"] = request.Properties;
 
                        DataRow[] singleRow = new DataRow[1];
                        singleRow[0] = row;

                        try
                        {
                            esTransactionScope.Enlist(cmd, request.ConnectionString, CreateIDbConnectionDelegate);

                            #region Profiling
                            if (sTraceHandler != null)
                            {
                                using (esTraceArguments esTrace = new esTraceArguments(request, cmd, packet, "SaveCollectionDynamic", System.Environment.StackTrace))
                                {
                                    try
                                    {
                                        da.Update(singleRow);
                                    }
                                    catch (Exception ex)
                                    {
                                        esTrace.Exception = ex.Message;
                                        throw;
                                    }
                                }
                            }
                            else
                            #endregion
                            {
                                da.Update(singleRow);
                            }

                            if (row.HasErrors)
                            {
                                request.FireOnError(packet, row.RowError);
                            }
                        }
                        finally
                        {
                            esTransactionScope.DeEnlist(cmd);
                            dataTable.Rows.Clear();
                        }

                        if (!row.HasErrors && packet.RowState != esDataRowState.Deleted && cmd.Parameters != null)
                        {
                            foreach (SQLiteParameter param in cmd.Parameters)
                            {
                                switch (param.Direction)
                                {
                                    case ParameterDirection.Output:
                                    case ParameterDirection.InputOutput:

                                        packet.CurrentValues[param.SourceColumn] = param.Value;
                                        break;
                                }
                            }
                        }

                        cmd.Dispose();
                    }
                }

                scope.Complete();
            }

            return dataTable;
        }

        static private DataTable SaveDynamicCollection_Deletes(esDataRequest request)
        {
            SQLiteCommand cmd = null;

            DataTable dataTable = CreateDataTable(request);

            using (esTransactionScope scope = new esTransactionScope())
            {
                using (SQLiteDataAdapter da = new SQLiteDataAdapter())
                {
                    da.AcceptChangesDuringUpdate = false;
                    da.ContinueUpdateOnError = request.ContinueUpdateOnError;

                    try
                    {
                        cmd = da.DeleteCommand = Shared.BuildDynamicDeleteCommand(request);
                        esTransactionScope.Enlist(cmd, request.ConnectionString, CreateIDbConnectionDelegate);

                        DataRow[] singleRow = new DataRow[1];

                        // Delete each record
                        foreach (esEntitySavePacket packet in request.CollectionSavePacket)
                        {
                            DataRow row = dataTable.NewRow();
                            dataTable.Rows.Add(row);

                            SetOriginalValues(request, packet, row, true);
                            row.AcceptChanges();
                            row.Delete();

                            singleRow[0] = row;

                            #region Profiling
                            if (sTraceHandler != null)
                            {
                                using (esTraceArguments esTrace = new esTraceArguments(request, cmd, packet, "SaveCollectionDynamic", System.Environment.StackTrace))
                                {
                                    try
                                    {
                                        da.Update(singleRow);
                                    }
                                    catch (Exception ex)
                                    {
                                        esTrace.Exception = ex.Message;
                                        throw;
                                    }
                                }
                            }
                            else
                            #endregion
                            {
                                da.Update(singleRow);
                            }

                            if (row.HasErrors)
                            {
                                request.FireOnError(packet, row.RowError);
                            }

                            dataTable.Rows.Clear(); // ADO.NET won't let us reuse the same DataRow
                        }
                    }
                    finally
                    {
                        esTransactionScope.DeEnlist(cmd);
                        cmd.Dispose();
                    }
                }
                scope.Complete();
            }

            return request.Table;
        }

        static private DataTable SaveDynamicEntity(esDataRequest request)
        {
            bool needToDelete = request.EntitySavePacket.RowState == esDataRowState.Deleted;

            DataTable dataTable = CreateDataTable(request);

            using (SQLiteDataAdapter da = new SQLiteDataAdapter())
            {
                da.AcceptChangesDuringUpdate = false;

                DataRow row = dataTable.NewRow();
                dataTable.Rows.Add(row);

                SQLiteCommand cmd = null;

                switch (request.EntitySavePacket.RowState)
                {
                    case esDataRowState.Added:
                        cmd = da.InsertCommand = Shared.BuildDynamicInsertCommand(request, request.EntitySavePacket);
                        SetModifiedValues(request, request.EntitySavePacket, row);
                        break;

                    case esDataRowState.Modified:
                        cmd = da.UpdateCommand = Shared.BuildDynamicUpdateCommand(request, request.EntitySavePacket);
                        SetOriginalValues(request, request.EntitySavePacket, row, false);
                        SetModifiedValues(request, request.EntitySavePacket, row);
                        row.AcceptChanges();
                        row.SetModified();
                        break;

                    case esDataRowState.Deleted:
                        cmd = da.DeleteCommand = Shared.BuildDynamicDeleteCommand(request);
                        SetOriginalValues(request, request.EntitySavePacket, row, true);
                        row.AcceptChanges();
                        row.Delete();
                        break;
                }

                if (!needToDelete && request.Properties != null)
                {
                    request.Properties["esDataRequest"] = request;
                    request.Properties["esEntityData"] = request.EntitySavePacket;
                    dataTable.ExtendedProperties["props"] = request.Properties;
                }

                DataRow[] singleRow = new DataRow[1];
                singleRow[0] = row;

                try
                {
                    if (!request.IgnoreComputedColumns)
                    {
                        da.RowUpdated += new EventHandler<System.Data.Common.RowUpdatedEventArgs>(OnRowUpdated);
                    }
                    esTransactionScope.Enlist(cmd, request.ConnectionString, CreateIDbConnectionDelegate);

                    #region Profiling
                    if (sTraceHandler != null)
                    {
                        using (esTraceArguments esTrace = new esTraceArguments(request, cmd, request.EntitySavePacket, "SaveEntityDynamic", System.Environment.StackTrace))
                        {
                            try
                            {
                                da.Update(singleRow);
                            }
                            catch (Exception ex)
                            {
                                esTrace.Exception = ex.Message;
                                throw;
                            }
                        }
                    }
                    else
                    #endregion
                    {
                        da.Update(singleRow);
                    }
                }
                finally
                {
                    esTransactionScope.DeEnlist(cmd);
                }

                if (request.EntitySavePacket.RowState != esDataRowState.Deleted && cmd.Parameters != null)
                {
                    foreach (SQLiteParameter param in cmd.Parameters)
                    {
                        switch (param.Direction)
                        {
                            case ParameterDirection.Output:
                            case ParameterDirection.InputOutput:

                                request.EntitySavePacket.CurrentValues[param.SourceColumn] = param.Value;
                                break;
                        }
                    }
                }

                cmd.Dispose();
            }

            return dataTable;
        }

        static private DataTable CreateDataTable(esDataRequest request)
        {
            DataTable dataTable = new DataTable();
            DataColumnCollection dataColumns = dataTable.Columns;
            esColumnMetadataCollection cols = request.Columns;

            if (request.SelectedColumns == null)
            {
                esColumnMetadata col;
                for (int i = 0; i < cols.Count; i++)
                {
                    col = cols[i];
                    dataColumns.Add(new DataColumn(col.Name, col.Type));
                }
            }
            else
            {
                foreach (string col in request.SelectedColumns.Keys)
                {
                    dataColumns.Add(new DataColumn(col, cols[col].Type));
                }
            }

            return dataTable;
        }

        static void SetOriginalValues(esDataRequest request, esEntitySavePacket packet, DataRow row, bool primaryKeysAndConcurrencyOnly)
        {
            foreach (esColumnMetadata col in request.Columns)
            {
                if (primaryKeysAndConcurrencyOnly &&
                    (!col.IsInPrimaryKey && !col.IsConcurrency && !col.IsEntitySpacesConcurrency)) continue;

                string columnName = col.Name;

                if (packet.OriginalValues.ContainsKey(columnName))
                {
                    row[columnName] = packet.OriginalValues[columnName];
                }
            }
        }

        static void SetModifiedValues(esDataRequest request, esEntitySavePacket packet, DataRow row)
        {
            foreach (string column in packet.ModifiedColumns)
            {
                if (request.Columns.FindByColumnName(column) != null)
                {
                    row[column] = packet.CurrentValues[column];
                }
            }
        }

        // If it's an Insert we fetch the @@Identity value and stuff it in the proper column
        protected static void OnRowUpdated(object sender, System.Data.Common.RowUpdatedEventArgs e)
        {
            try
            {
                PropertyCollection props = e.Row.Table.ExtendedProperties;
                if(props.ContainsKey("props"))
                {
                    props = (PropertyCollection)props["props"];
                }

                if (e.Status == UpdateStatus.Continue && (e.StatementType == StatementType.Insert || e.StatementType == StatementType.Update))
                {
                    esDataRequest request = props["esDataRequest"] as esDataRequest;
                    esEntitySavePacket packet = (esEntitySavePacket)props["esEntityData"];

                    if (e.StatementType == StatementType.Insert)
                    {
                        if (props.Contains("AutoInc"))
                        {
                            string autoInc = props["AutoInc"] as string;

                            SQLiteCommand cmd = new SQLiteCommand();
                            cmd.Connection = e.Command.Connection as SQLiteConnection;
                            cmd.Transaction = e.Command.Transaction as SQLiteTransaction;

                            cmd.CommandText = "SELECT last_insert_rowid()";
                            cmd.CommandType = CommandType.Text;

                            object o = null;

                            #region Profiling
                            if (sTraceHandler != null)
                            {
                                using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "OnRowUpdated", System.Environment.StackTrace))
                                {
                                    try
                                    {
                                        o = cmd.ExecuteScalar();
                                    }
                                    catch (Exception ex)
                                    {
                                        esTrace.Exception = ex.Message;
                                        throw;
                                    }
                                }
                            }
                            else
                            #endregion
                            {
                                o = cmd.ExecuteScalar();
                            }

                            if (o != null)
                            {
                                packet.CurrentValues[autoInc] = o;
                                e.Row[autoInc] = o;
                            }
                        }

                        if (props.Contains("EntitySpacesConcurrency"))
                        {
                            string esConcurrencyColumn = props["EntitySpacesConcurrency"] as string;
                            packet.CurrentValues[esConcurrencyColumn] = 1;
                        }
                    }

                    //-------------------------------------------------------------------------------------------------
                    // Fetch any defaults, SQLite doesn't support output parameters so we gotta do this the hard way
                    //-------------------------------------------------------------------------------------------------
                    if (props.Contains("Defaults"))
                    {
                        // Build the Where parameter and parameters
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.Connection = e.Command.Connection as SQLiteConnection;
                        cmd.Transaction = e.Command.Transaction as SQLiteTransaction;

                        string select = (string)props["Defaults"];

                        string[] whereParameters = ((string)props["Where"]).Split(',');

                        string comma = String.Empty;
                        string where = String.Empty;
                        int i = 1;
                        foreach (string parameter in whereParameters)
                        {
                            SQLiteParameter p = new SQLiteParameter("@p" + i++.ToString(), e.Row[parameter]);
                            cmd.Parameters.Add(p);
                            where += comma + "[" + parameter + "]=" + p.ParameterName;
                            comma = " AND ";
                        }

                        // Okay, now we can execute the sql and get any values that have defaults that were
                        // null at the time of the insert and/or our timestamp
                        cmd.CommandText = "SELECT " + select + " FROM [" + request.ProviderMetadata.Source + "] WHERE " + where + ";";

                        SQLiteDataReader rdr = null;

                        try
                        {
                            #region Profiling
                            if (sTraceHandler != null)
                            {
                                using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "OnRowUpdated", System.Environment.StackTrace))
                                {
                                    try
                                    {
                                        rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                                    }
                                    catch (Exception ex)
                                    {
                                        esTrace.Exception = ex.Message;
                                        throw;
                                    }
                                }
                            }
                            else
                            #endregion
                            {
                                rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                            }

                            if (rdr.Read())
                            {
                                select = select.Replace("[", String.Empty).Replace("]", String.Empty);
                                string[] selectCols = select.Split(',');

                                for (int k = 0; k < selectCols.Length; k++)
                                {
                                    packet.CurrentValues[selectCols[k]] = rdr.GetValue(k);
                                }
                            }
                        }
                        finally
                        {
                            // Make sure we close the reader no matter what
                            if (rdr != null) rdr.Close();
                        }
                    }

                    if (e.StatementType == StatementType.Update)
                    {
                        string colName = props["EntitySpacesConcurrency"] as string;
                        object o = e.Row[colName];

                        switch (Type.GetTypeCode(o.GetType()))
                        {
                            case TypeCode.Int16: packet.CurrentValues[colName] = ((System.Int16)o) + 1; break;
                            case TypeCode.Int32: packet.CurrentValues[colName] = ((System.Int32)o) + 1; break;
                            case TypeCode.Int64: packet.CurrentValues[colName] = ((System.Int64)o) + 1; break;
                            case TypeCode.UInt16: packet.CurrentValues[colName] = ((System.UInt16)o) + 1; break;
                            case TypeCode.UInt32: packet.CurrentValues[colName] = ((System.UInt32)o) + 1; break;
                            case TypeCode.UInt64: packet.CurrentValues[colName] = ((System.UInt64)o) + 1; break;
                        }
                    }
                }
            }
            catch { }
        }
    }
}
