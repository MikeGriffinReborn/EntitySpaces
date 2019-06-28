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

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// This class is used by EntitySpaces to issue loosely coupled calls to the actual EntitySpaces
    /// data provider as defined in the web.config or app.config file. 
    /// </summary>
    public class esDataProvider 
    {
        /// <summary>
        /// Used to populate an esEntity or esEntityCollection with data.
        /// </summary>
        /// <param name="request">Contains all of the information necessary to issue and carry out the request</param>
        /// <param name="sig">Contains the required information to locate the EntitySpaces DataProvider</param>
        /// <returns></returns>
        public esDataResponse esLoadDataTable(esDataRequest request, esProviderSignature sig)
        {
            request.DatabaseVersion = sig.DatabaseVersion;
            esDataResponse response = esProviderFactory.GetDataProvider(sig.DataProviderName, sig.DataProviderClass).esLoadDataTable(request);
            if(request.DynamicQuery != null)
            {
                request.DynamicQuery.es.LastQuery = response.LastQuery;
            }

            if (response.IsException)
            {
                throw response.Exception;
            }

            return response;
        }

        /// <summary>
        /// Used to issue a Save command on an esEntity or esEntityCollection.
        /// </summary>
        /// <param name="request">Contains all of the information necessary to issue and carry out the request</param>
        /// <param name="sig">Contains the required information to locate the EntitySpaces DataProvider</param>
        /// <returns></returns>
        public esDataResponse esSaveDataTable(esDataRequest request, esProviderSignature sig)
        {
            request.DatabaseVersion = sig.DatabaseVersion;
            esDataResponse response = esProviderFactory.GetDataProvider(sig.DataProviderName, sig.DataProviderClass).esSaveDataTable(request);

            // NOTE: New to 1.6.0. We do not rethrow the exception here, we do rethrow it in 
            // esEntityCollection.SaveToProviderInsertsUpdates after we assign the errors to the proper rows.

            return response;
        }

        /// <summary>
        /// Used to execute a non-data return query through the EntitySpaces DataProvider
        /// </summary>
        /// <param name="request">Contains all of the information necessary to issue and carry out the request</param>
        /// <param name="sig">Contains the required information to locate the EntitySpaces DataProvider</param>
        /// <returns></returns>
        public esDataResponse ExecuteNonQuery(esDataRequest request, esProviderSignature sig)
        {
            request.DatabaseVersion = sig.DatabaseVersion;
            esDataResponse response = esProviderFactory.GetDataProvider(sig.DataProviderName, sig.DataProviderClass).ExecuteNonQuery(request);

            if (response.IsException)
            {
                throw response.Exception;
            }

            if (response.Parameters != null && response.Parameters.Count > 0)
            {
                request.Parameters.Merge(response.Parameters);
            }

            return response;
        }

        /// <summary>
        /// Used to execute a command against the EntitySpaces DataProvider that returns data in the form of
        /// a IDataReader.
        /// </summary>
        /// <param name="request">Contains all of the information necessary to issue and carry out the request</param>
        /// <param name="sig">Contains the required information to locate the EntitySpaces DataProvider</param>
        /// <returns></returns>
        public esDataResponse ExecuteReader(esDataRequest request, esProviderSignature sig)
        {
            request.DatabaseVersion = sig.DatabaseVersion;
            esDataResponse response = esProviderFactory.GetDataProvider(sig.DataProviderName, sig.DataProviderClass).ExecuteReader(request);

            if (response.IsException)
            {
                throw response.Exception;
            }

            return response;
        }

        /// <summary>
        /// Used to issue a command against the EntitySpaces DataProvider that returns a single scalar value.
        /// </summary>
        /// <param name="request">Contains all of the information necessary to issue and carry out the request</param>
        /// <param name="sig">Contains the required information to locate the EntitySpaces DataProvider</param>
        /// <returns></returns>
        public esDataResponse ExecuteScalar(esDataRequest request, esProviderSignature sig)
        {
            request.DatabaseVersion = sig.DatabaseVersion;
            esDataResponse response = esProviderFactory.GetDataProvider(sig.DataProviderName, sig.DataProviderClass).ExecuteScalar(request);

            if (response.IsException)
            {
                throw response.Exception;
            }

            return response;
        }

        /// <summary>
        /// Not used by EntitySpaces but provided so that you can issue commands against the EntitySpaces DataProvider
        /// that return mult-resultsets (many DataTables).
        /// </summary>
        /// <param name="request">Contains all of the information necessary to issue and carry out the request</param>
        /// <param name="sig">Contains the required information to locate the EntitySpaces DataProvider</param>
        /// <returns></returns>
        public esDataResponse FillDataSet(esDataRequest request, esProviderSignature sig)
        {
            request.DatabaseVersion = sig.DatabaseVersion;
            esDataResponse response = esProviderFactory.GetDataProvider(sig.DataProviderName, sig.DataProviderClass).FillDataSet(request);

            if (response.IsException)
            {
                throw response.Exception;
            }

            return response;
        }

        /// <summary>
        /// Similiar to esLoadDataTable only this method merely returns a DataTable and does not 
        /// actually populate an esEntity or esEntityCollection.
        /// </summary>
        /// <param name="request">Contains all of the information necessary to issue and carry out the request</param>
        /// <param name="sig">Contains the required information to locate the EntitySpaces DataProvider</param>
        /// <returns></returns>
        public esDataResponse FillDataTable(esDataRequest request, esProviderSignature sig)
        {
            request.DatabaseVersion = sig.DatabaseVersion;
            esDataResponse response = esProviderFactory.GetDataProvider(sig.DataProviderName, sig.DataProviderClass).FillDataTable(request);

            if (response.IsException)
            {
                throw response.Exception;
            }

            return response;
        }
    }
}
