using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace EntitySpaces.OracleManagedClientProvider
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

            public esTraceArguments(esDataRequest request, IDbCommand cmd, string action, string callStack)
            {
                PacketOrder = Interlocked.Increment(ref esTraceArguments.packetOrder);

                this.command = cmd;

                TraceChannel = DataProvider.sTraceChannel;
                Syntax = "ORACLE";
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
                        OracleParameter param = parameters[i] as OracleParameter;

                        esTraceParameter p = new esTraceParameter()
                        {
                            Name = param.ParameterName,
                            Direction = param.Direction.ToString(),
                            ParamType = param.OracleDbType.ToString().ToUpper(),
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
            return new OracleConnection();
        }

        static private void CleanupCommand(OracleCommand cmd)
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
                        OracleCommand cmd = QueryBuilder.PrepareCommand(request);
                        cmd.BindByName = true;
                        LoadDataTableFromDynamicQuery(request, response, cmd);
                        break;

                    case esQueryType.DynamicQueryParseOnly:

                        response = new esDataResponse();
                        OracleCommand cmd1 = QueryBuilder.PrepareCommand(request);
                        cmd1.BindByName = true;
                        response.LastQuery = cmd1.CommandText;
                        break;


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
            catch (OracleException ex)
            {
                esException es = Shared.CheckForConcurrencyException(ex);
                if (es != null)
                    response.Exception = es;
                else
                    response.Exception = ex;
            }
            catch (DBConcurrencyException dbex)
            {
                response.Exception = new esConcurrencyException("Error in OracleClientProvider.esSaveDataTable", dbex);
            }

            response.Table = request.Table;
            return response;
        }

        esDataResponse IDataProvider.ExecuteNonQuery(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            OracleCommand cmd = null;

            try
            {
                cmd = new OracleCommand();
                cmd.BindByName = true;
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
            OracleCommand cmd = null;

            try
            {
                cmd = new OracleCommand();
                cmd.BindByName = true;
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

                        OracleParameter p = new OracleParameter("outCursor", OracleDbType.RefCursor);
                        p.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(p);
                        break;

                    case esQueryType.Text:
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = request.QueryText;
                        break;

                    case esQueryType.DynamicQuery:
                        cmd = QueryBuilder.PrepareCommand(request);
                        break;
                }

                cmd.Connection = new OracleConnection(request.ConnectionString);
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
            OracleCommand cmd = null;

            try
            {
                cmd = new OracleCommand();
                cmd.BindByName = true;
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
            OracleCommand cmd = null;

            try
            {
                DataSet dataSet = new DataSet();

                cmd = new OracleCommand();
                cmd.BindByName = true;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Shared.CreateFullName(request);

                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                OracleParameter p = new OracleParameter("outCursor", OracleDbType.RefCursor);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                OracleDataAdapter da = new OracleDataAdapter();
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
            catch (Exception)
            {
                CleanupCommand(cmd);
                throw;
            }
            finally
            {

            }

            return response;
        }

        static private esDataResponse LoadDataSetFromText(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            OracleCommand cmd = null;

            try
            {
                DataSet dataSet = new DataSet();

                cmd = new OracleCommand();
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                OracleDataAdapter da = new OracleDataAdapter();
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
            catch (Exception)
            {
                CleanupCommand(cmd);
                throw;
            }
            finally
            {

            }

            return response;
        }

        static private esDataResponse LoadDataTableFromStoredProcedure(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            OracleCommand cmd = null;

            try
            {
                DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

                cmd = new OracleCommand();
                cmd.BindByName = true;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Shared.CreateFullName(request);
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                OracleParameter p = new OracleParameter("outCursor", OracleDbType.RefCursor);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                OracleDataAdapter da = new OracleDataAdapter();
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
            catch (Exception)
            {
                CleanupCommand(cmd);
                throw;
            }
            finally
            {

            }

            return response;
        }

        static private esDataResponse LoadDataTableFromText(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            OracleCommand cmd = null;

            try
            {
                DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

                cmd = new OracleCommand();
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;
                if (request.Parameters != null) Shared.AddParameters(cmd, request);

                OracleDataAdapter da = new OracleDataAdapter();
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
            catch
            {
                CleanupCommand(cmd);
                throw;
            }
            finally
            {

            }

            return response;
        }

        static private esDataResponse LoadManyToMany(esDataRequest request)
        {
            esDataResponse response = new esDataResponse();
            OracleCommand cmd = null;

            try
            {
                DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

                cmd = new OracleCommand();
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;
                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

                string mmQuery = request.QueryText;

                string[] sections = mmQuery.Split('|');
                string[] tables = sections[0].Split(',');
                string[] columns = sections[1].Split(',');

                // We build the query, we don't use Delimiters to avoid tons of extra concatentation
                string sql = "SELECT * FROM \"" + tables[0] + "\" JOIN \"" + tables[1] + "\" ON \"" + tables[0] + "\".\"" + columns[0] + "\" = \"";
                sql += tables[1] + "\".\"" + columns[1] + "\" WHERE \"" + tables[1] + "\".\"" + sections[2] + "\" = :";

                if (request.Parameters != null)
                {
                    foreach (esParameter esParam in request.Parameters)
                    {
                        sql += esParam.Name;
                    }

                    Shared.AddParameters(cmd, request);
                }

                OracleDataAdapter da = new OracleDataAdapter();
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
            catch (Exception)
            {
                CleanupCommand(cmd);
                throw;
            }
            finally
            {

            }

            return response;
        }

        // This is used only to execute the Dynamic Query API
        static private void LoadDataTableFromDynamicQuery(esDataRequest request, esDataResponse response, OracleCommand cmd)
        {
            try
            {
                response.LastQuery = cmd.CommandText;

                if (request.CommandTimeout != null) cmd.CommandTimeout = request.CommandTimeout.Value;

                DataTable dataTable = new DataTable(request.ProviderMetadata.Destination);

                OracleDataAdapter da = new OracleDataAdapter();
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
            catch (Exception)
            {
                CleanupCommand(cmd);
                throw;
            }
            finally
            {

            }
        }

        static private DataTable SaveStoredProcCollection(esDataRequest request)
        {
            if (request.CollectionSavePacket == null) return null;

            OracleCommand cmdInsert = null;
            OracleCommand cmdUpdate = null;
            OracleCommand cmdDelete = null;

            try
            {
                using (esTransactionScope scope = new esTransactionScope())
                {
                    OracleCommand cmd = null;
                    bool exception = false;

                    foreach (esEntitySavePacket packet in request.CollectionSavePacket)
                    {
                        cmd = null;
                        exception = false;

                        #region Setup Commands
                        switch (packet.RowState)
                        {
                            case esDataRowState.Added:
                                if (cmdInsert == null)
                                {
                                    cmdInsert = Shared.BuildStoredProcInsertCommand(request, packet);
                                    esTransactionScope.Enlist(cmdInsert, request.ConnectionString, CreateIDbConnectionDelegate);
                                }
                                cmd = cmdInsert;
                                break;
                            case esDataRowState.Modified:
                                if (cmdUpdate == null)
                                {
                                    cmdUpdate = Shared.BuildStoredProcUpdateCommand(request, packet);
                                    esTransactionScope.Enlist(cmdUpdate, request.ConnectionString, CreateIDbConnectionDelegate);
                                }
                                cmd = cmdUpdate;
                                break;
                            case esDataRowState.Deleted:
                                if (cmdDelete == null)
                                {
                                    cmdDelete = Shared.BuildStoredProcDeleteCommand(request, packet);
                                    esTransactionScope.Enlist(cmdDelete, request.ConnectionString, CreateIDbConnectionDelegate);
                                }
                                cmd = cmdDelete;
                                break;

                            case esDataRowState.Unchanged:
                                continue;
                        }
                        #endregion

                        #region Preprocess Parameters
                        if (cmd.Parameters != null)
                        {
                            foreach (OracleParameter param in cmd.Parameters)
                            {
                                if (param.Direction == ParameterDirection.Output)
                                {
                                    param.Value = null;
                                }
                                else
                                {
                                    if (packet.CurrentValues.ContainsKey(param.SourceColumn))
                                    {
                                        param.Value = packet.CurrentValues[param.SourceColumn];
                                    }
                                    else
                                    {
                                        param.Value = null;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Execute Command
                        try
                        {
                            int count = 0;

                            #region Profiling
                            if (sTraceHandler != null)
                            {
                                using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "SaveCollectionStoredProcedure", System.Environment.StackTrace))
                                {
                                    try
                                    {
                                        count = cmd.ExecuteNonQuery();
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
                                count = cmd.ExecuteNonQuery();
                            }

                            if (count < 1)
                            {
                                throw new esConcurrencyException("Update failed to update any records");
                            }
                        }
                        catch (Exception ex)
                        {
                            exception = true;
                            request.FireOnError(packet, ex.Message);
                            if (!request.ContinueUpdateOnError)
                            {
                                throw;
                            }
                        }
                        #endregion

                        #region Postprocess Commands
                        if (!exception && packet.RowState != esDataRowState.Deleted && cmd.Parameters != null)
                        {
                            foreach (OracleParameter param in cmd.Parameters)
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
                        #endregion
                    }

                    scope.Complete();
                }
            }
            finally
            {
                if (cmdInsert != null) esTransactionScope.DeEnlist(cmdInsert);
                if (cmdUpdate != null) esTransactionScope.DeEnlist(cmdUpdate);
                if (cmdDelete != null) esTransactionScope.DeEnlist(cmdDelete);
            }

            return null;
        }

        static private DataTable SaveStoredProcEntity(esDataRequest request)
        {
            OracleCommand cmd = null;

            switch (request.EntitySavePacket.RowState)
            {
                case esDataRowState.Added:
                    cmd = Shared.BuildStoredProcInsertCommand(request, request.EntitySavePacket);
                    break;

                case esDataRowState.Modified:
                    cmd = Shared.BuildStoredProcUpdateCommand(request, request.EntitySavePacket);
                    break;

                case esDataRowState.Deleted:
                    cmd = Shared.BuildStoredProcDeleteCommand(request, request.EntitySavePacket);
                    break;

                case esDataRowState.Unchanged:
                    return null;
            }

            try
            {
                esTransactionScope.Enlist(cmd, request.ConnectionString, CreateIDbConnectionDelegate);
                int count = 0;

                #region Profiling
                if (sTraceHandler != null)
                {
                    using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "SaveEntityStoredProcedure", System.Environment.StackTrace))
                    {
                        try
                        {
                            count = cmd.ExecuteNonQuery(); ;
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
                    count = cmd.ExecuteNonQuery();
                }
                // hd, 19.03.2014: eingefügt, da immer -1 von ODP.Net zurückgegeben wird
                if (count < 0)
                {
                    count = count * -1;
                }
                if (count < 1)
                {
                    throw new esConcurrencyException("Update failed to update any records");
                }



                if (request.EntitySavePacket.RowState != esDataRowState.Deleted && cmd.Parameters != null)
                {
                    foreach (OracleParameter param in cmd.Parameters)
                    {
                        switch (param.Direction)
                        {
                            case ParameterDirection.Output:
                            case ParameterDirection.InputOutput:
                                if (param.OracleDbType == OracleDbType.Decimal)
                                {
                                    // 20.01.2016 791sd: Sonderbehandlung Decimal, da OracleDbType.Decimal nicht IConvertible implementiert
                                    request.EntitySavePacket.CurrentValues[param.SourceColumn] = Convert.ToDecimal(param.Value.ToString());
                                }
                                else
                                {
                                    request.EntitySavePacket.CurrentValues[param.SourceColumn] = param.Value;
                                }
                                break;

                        }
                    }
                }
            }
            finally
            {
                esTransactionScope.DeEnlist(cmd);
                cmd.Dispose();
            }
            return null;
        }

        static private DataTable SaveDynamicCollection(esDataRequest request)
        {
            if (request.CollectionSavePacket == null) return null;

            using (esTransactionScope scope = new esTransactionScope())
            {
                OracleCommand cmd = null;
                bool exception = false;

                foreach (esEntitySavePacket packet in request.CollectionSavePacket)
                {
                    exception = false;
                    cmd = null;

                    switch (packet.RowState)
                    {
                        case esDataRowState.Added:
                            cmd = Shared.BuildDynamicInsertCommand(request, packet);
                            break;

                        case esDataRowState.Modified:
                            cmd = Shared.BuildDynamicUpdateCommand(request, packet);
                            break;

                        case esDataRowState.Deleted:
                            cmd = Shared.BuildDynamicDeleteCommand(request, packet);
                            break;

                        case esDataRowState.Unchanged:
                            continue;
                    }

                    try
                    {
                        esTransactionScope.Enlist(cmd, request.ConnectionString, CreateIDbConnectionDelegate);
                        int count;

                        #region Profiling
                        if (sTraceHandler != null)
                        {
                            using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "SaveCollectionDynamic", System.Environment.StackTrace))
                            {
                                try
                                {
                                    count = cmd.ExecuteNonQuery(); ;
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
                            count = cmd.ExecuteNonQuery();
                        }

                        // hd, 19.03.2014: eingefügt, da immer -1 von ODP.Net zurückgegeben wird
                        if (count < 0)
                        {
                            count = count * -1;
                        }
                        if (count < 1)
                        {
                            throw new esConcurrencyException("Update failed to update any records");
                        }
                    }
                    catch (Exception ex)
                    {
                        exception = true;

                        request.FireOnError(packet, ex.Message);

                        if (!request.ContinueUpdateOnError)
                        {
                            throw;
                        }

                        if (!exception && packet.RowState != esDataRowState.Deleted && cmd.Parameters != null)
                        {
                            foreach (OracleParameter param in cmd.Parameters)
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

                    }
                    finally
                    {
                        esTransactionScope.DeEnlist(cmd);
                        cmd.Dispose();
                    }


                }

                scope.Complete();
            }

            return null;
        }

        static private DataTable SaveDynamicEntity(esDataRequest request)
        {
            OracleCommand cmd = null;

            switch (request.EntitySavePacket.RowState)
            {
                case esDataRowState.Added:
                    cmd = Shared.BuildDynamicInsertCommand(request, request.EntitySavePacket);
                    break;

                case esDataRowState.Modified:
                    cmd = Shared.BuildDynamicUpdateCommand(request, request.EntitySavePacket);
                    break;

                case esDataRowState.Deleted:
                    cmd = Shared.BuildDynamicDeleteCommand(request, request.EntitySavePacket);
                    break;
            }

            try
            {
                esTransactionScope.Enlist(cmd, request.ConnectionString, CreateIDbConnectionDelegate);
                int count = 0;

                #region Profiling
                if (sTraceHandler != null)
                {
                    using (esTraceArguments esTrace = new esTraceArguments(request, cmd, "SaveEntityDynamic", System.Environment.StackTrace))
                    {
                        try
                        {
                            count = cmd.ExecuteNonQuery();
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
                    count = cmd.ExecuteNonQuery();
                }
                // hd, 19.03.2014: eingefügt, da immer -1 von ODP.Net zurückgegeben wird
                if (count < 0)
                {
                    count = count * -1;
                }
                if (count < 1)
                {
                    throw new esConcurrencyException("Update failed to update any records");
                }


                if (request.EntitySavePacket.RowState != esDataRowState.Deleted && cmd.Parameters != null)
                {
                    foreach (OracleParameter param in cmd.Parameters)
                    {
                        switch (param.Direction)
                        {
                            case ParameterDirection.Output:
                            case ParameterDirection.InputOutput:
                                if (param.OracleDbType == OracleDbType.Decimal)
                                {
                                    // 20.01.2016 791sd: Sonderbehandlung Decimal, da OracleDbType.Decimal nicht IConvertible implementiert
                                    request.EntitySavePacket.CurrentValues[param.SourceColumn] = Convert.ToDecimal(param.Value.ToString());
                                }
                                else
                                {
                                    request.EntitySavePacket.CurrentValues[param.SourceColumn] = param.Value;
                                }
                                //request.EntitySavePacket.CurrentValues[param.SourceColumn] = param.Value;
                                break;
                        }
                    }
                }


            }
            finally
            {
                esTransactionScope.DeEnlist(cmd);
                cmd.Dispose();
            }


            return null;
        }
    }
}
