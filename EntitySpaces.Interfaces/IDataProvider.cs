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

using System.Data;

namespace EntitySpaces.Interfaces
{
    public delegate void TraceEventHandler(ITraceArguments packet);

    /// <summary>
    /// The EntitySpaces DataProviders such as EntitySpaces.SqlClientProvider implement this interface. 
    /// </summary>
    /// <remarks>
    /// This interface helps to de-couple an EntitySpaces application from the actual provider such as
    /// SqlClientProvider, OracleClientProvider and so on. The providers take an esDataRequest as the 
    /// input and return an esDataResponse. EntitySpaces developers don't have to worry about loading providers, 
    /// instead, you access these methods through the <see cref="esDataProvider"/> which will carry out the
    /// call.
    /// </remarks>
    /// <seealso cref="esDataProvider"/>
    public interface IDataProvider
    {
        /// <summary>
        /// Used when Profiling this Provider
        /// </summary>
        event TraceEventHandler TraceHandler;

        /// <summary>
        /// Can be used to determine if this provider is already tracing
        /// </summary>
        bool IsTracing { get; }

        /// <summary>
        /// The Channel to use for this provider when Profiling.
        /// </summary>
        string TraceChannel { get; set; }

        /// <summary>
        /// Used to populate an esEntity or an esEntityCollection with data. Whatever type of query is
        /// in the esDataRequest must return a resultset (DataTable) in order to the properly load
        /// the EntitySpaces object.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Any exception thrown by the provider will be in the returned esDataResponse</returns>
        esDataResponse esLoadDataTable(esDataRequest request);

        /// <summary>
        /// This method is used to save an esEntity or esEntityCollection to the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        esDataResponse esSaveDataTable(esDataRequest request);

        /// <summary>
        /// This method can be used to execute any query which does not return a result set. See 
        /// the help for the ADO.NET SqlCommand.ExecuteNonQuery method. This method utlimately maps to the 
        /// the low level ADO.NET data provider's Command.ExecuteNonQuery method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        esDataResponse ExecuteNonQuery(esDataRequest request);

        /// <summary>
        /// This method is used to return an IDataReader which will be found in the returned esDataResponse.
        /// See the help for the ADO.NET SqlCommand.ExecuteReader method. This method utlimately maps to the 
        /// the low level ADO.NET data provider's Command.ExecuteReader method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>See the esDataResponse.DataReader property</returns>
        esDataResponse ExecuteReader(esDataRequest request);

        /// <summary>
        /// This method is used to return a single value from the database. 
        /// See the help for the ADO.NET SqlCommand.ExecuteScalar method. This method utlimately maps to the 
        /// the low level ADO.NET data provider's Command.ExecuteScalar method.
        /// </summary>
        /// <param name="request">See the esDataResponse.Scalar property</param>
        /// <returns></returns>
        esDataResponse ExecuteScalar(esDataRequest request);

        /// <summary>
        /// This method can be used to execute a query that returns multiple resultsets.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        esDataResponse FillDataSet(esDataRequest request);

        /// <summary>
        /// This method can be used to return a DataTable. The returned DataTable is not used to 
        /// populate an esEntity or esEntityCollection. See <see cref="esLoadDataTable"/> if your
        /// desire is to populate an EntitySpaces object with data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        esDataResponse FillDataTable(esDataRequest request);
    }
}
