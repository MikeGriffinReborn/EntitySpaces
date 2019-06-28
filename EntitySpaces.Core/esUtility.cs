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
using System.Data;

using EntitySpaces.Interfaces;
using EntitySpaces.DynamicQuery;

namespace EntitySpaces.Core
{
    /// <summary>
    /// Provides all of the protected methods available to you in your Custom classes that query the database.
    /// This class can be useful to query the database when certain calls don't seem to fit in your business
    /// objects.
    /// </summary>
    public class esUtility
    {
        private esPrivateCollection coll;

        public esUtility()
        {
            coll = new esPrivateCollection(this);
        }

        public IEntityCollection es
        {
            get
            {
                return coll.es;
            }
        }

        public string Source = "esUtility";
        public string Destination = "esUtility";


        #region ExecuteNonQuery

        public int ExecuteNonQuery(esQueryType queryType, string query)
        {
            return coll.ExecuteNonQuery(queryType, query);
        }

        public int ExecuteNonQuery(esQueryType queryType, string query, params object[] parameters)
        {
            return coll.ExecuteNonQuery(queryType, query, parameters);
        }

        public int ExecuteNonQuery(esQueryType queryType, string query, esParameters parms)
        {
            return coll.ExecuteNonQuery(queryType, query, parms);
        }

        public int ExecuteNonQuery(string schema, string storedProcedure)
        {
            return coll.ExecuteNonQuery(schema, storedProcedure);
        }

        public int ExecuteNonQuery(string schema, string storedProcedure, params object[] parameters)
        {
            return coll.ExecuteNonQuery(schema, storedProcedure, parameters);
        }

        public int ExecuteNonQuery(string schema, string storedProcedure, esParameters parms)
        {
            return coll.ExecuteNonQuery(schema, storedProcedure, parms);
        }

        public int ExecuteNonQuery(string catalog, string schema, string storedProcedure, esParameters parameters)
        {
            return coll.ExecuteNonQuery(catalog, schema, storedProcedure,parameters);
        }

        #endregion

        #region ExecuteReader

        public IDataReader ExecuteReader(esQueryType queryType, string query)
        {
            return coll.ExecuteReader(queryType, query);
        }

        public IDataReader ExecuteReader(esQueryType queryType, string query, params object[] parameters)
        {
            return coll.ExecuteReader(queryType, query, parameters);
        }

        public IDataReader ExecuteReader(esQueryType queryType, string query, esParameters parms)
        {
            return coll.ExecuteReader(queryType, query, parms);
        }

        public IDataReader ExecuteReader(string schema, string storedProcedure)
        {
            return coll.ExecuteReader(schema, storedProcedure);
        }

        public IDataReader ExecuteReader(string schema, string storedProcedure, params object[] parameters)
        {
            return coll.ExecuteReader(schema, storedProcedure, parameters);
        }

        public IDataReader ExecuteReader(string schema, string storedProcedure, esParameters parms)
        {
            return coll.ExecuteReader(schema, storedProcedure, parms);
        }

        public IDataReader ExecuteReader(string catalog, string schema, string storedProcedure, esParameters parameters)
        {
            return coll.ExecuteReader(catalog, schema, storedProcedure, parameters);
        }

        #endregion

        #region ExecuteScalar

        public object ExecuteScalar(esQueryType queryType, string query)
        {
            return coll.ExecuteScalar(queryType, query);
        }

        public object ExecuteScalar(esQueryType queryType, string query, params object[] parameters)
        {
            return coll.ExecuteScalar(queryType, query, parameters);
        }

        public object ExecuteScalar(esQueryType queryType, string query, esParameters parms)
        {
            return coll.ExecuteScalar(queryType, query, parms);
        }

        public object ExecuteScalar(string schema, string storedProcedure)
        {
            return coll.ExecuteScalar(schema, storedProcedure);
        }

        public object ExecuteScalar(string schema, string storedProcedure, params object[] parameters)
        {
            return coll.ExecuteScalar(schema, storedProcedure, parameters);
        }

        public object ExecuteScalar(string schema, string storedProcedure, esParameters parms)
        {
            return coll.ExecuteScalar(schema, storedProcedure, parms);
        }

        public object ExecuteScalar(string catalog, string schema, string storedProcedure, esParameters parameters)
        {
            return coll.ExecuteScalar(catalog, schema, storedProcedure, parameters);
        }

        #region Generic Versions

        public T ExecuteScalar<T>(esQueryType queryType, string query)
        {
            return (T)coll.ExecuteScalar<T>(queryType, query);
        }

        public T ExecuteScalar<T>(esQueryType queryType, string query, params object[] parameters)
        {
            return (T)coll.ExecuteScalar<T>(queryType, query, parameters);
        }

        public T ExecuteScalar<T>(esQueryType queryType, string query, esParameters parms)
        {
            return (T)coll.ExecuteScalar<T>(queryType, query, parms);
        }

        public T ExecuteScalar<T>(string schema, string storedProcedure)
        {
            return (T)coll.ExecuteScalar<T>(schema, storedProcedure);
        }

        public T ExecuteScalar<T>(string schema, string storedProcedure, params object[] parameters)
        {
            return (T)coll.ExecuteScalar<T>(schema, storedProcedure, parameters);
        }

        public T ExecuteScalar<T>(string schema, string storedProcedure, esParameters parms)
        {
            return (T)coll.ExecuteScalar<T>(schema, storedProcedure, parms);
        }

        public T ExecuteScalar<T>(string catalog, string schema, string storedProcedure, esParameters parameters)
        {
            return (T)coll.ExecuteScalar<T>(catalog, schema, storedProcedure, parameters);
        }

        #endregion

        #endregion

        #region FillDataTable

        public DataTable FillDataTable(esQueryType queryType, string query)
        {
            return coll.FillDataTable(queryType, query);
        }

        public DataTable FillDataTable(esQueryType queryType, string query, params object[] parameters)
        {
            return coll.FillDataTable(queryType, query, parameters);
        }

        public DataTable FillDataTable(esQueryType queryType, string query, esParameters parms)
        {
            return coll.FillDataTable(queryType, query, parms);
        }

        public DataTable FillDataTable(string schema, string storedProcedure)
        {
            return coll.FillDataTable(schema, storedProcedure);
        }

        public DataTable FillDataTable(string schema, string storedProcedure, params object[] parameters)
        {
            return coll.FillDataTable(schema, storedProcedure, parameters);
        }

        public DataTable FillDataTable(string schema, string storedProcedure, esParameters parms)
        {
            return coll.FillDataTable(schema, storedProcedure, parms);
        }

        public DataTable FillDataTable(string catalog, string schema, string storedProcedure, esParameters parameters)
        {
            return coll.FillDataTable(catalog, schema, storedProcedure, parameters);
        }

        #endregion

        #region FillDataSet

        public DataSet FillDataSet(esQueryType queryType, string query)
        {
            return coll.FillDataSet(queryType, query);
        }

        public DataSet FillDataSet(esQueryType queryType, string query, params object[] parameters)
        {
            return coll.FillDataSet(queryType, query, parameters);
        }

        public DataSet FillDataSet(esQueryType queryType, string query, esParameters parms)
        {
            return coll.FillDataSet(queryType, query, parms);
        }

        public DataSet FillDataSet(string schema, string storedProcedure)
        {
            return coll.FillDataSet(schema, storedProcedure);
        }

        public DataSet FillDataSet(string schema, string storedProcedure, params object[] parameters)
        {
            return coll.FillDataSet(schema, storedProcedure, parameters);
        }

        public DataSet FillDataSet(string schema, string storedProcedure, esParameters parms)
        {
            return coll.FillDataSet(schema, storedProcedure, parms);
        }

        public DataSet FillDataSet(string catalog, string schema, string storedProcedure, esParameters parameters)
        {
            return coll.FillDataSet(catalog, schema, storedProcedure, parameters);
        }

        #endregion

        #region Virtual Methods

        virtual public esProviderSpecificMetadata GetProviderMetadata(string mapName)
        {
            esProv.Schema = this.es.Connection.Schema;
            esProv.Catalog = this.es.Connection.Catalog;
            esProv.Source = this.Source;
            esProv.Destination = this.Destination;
            return esProv;
        }

        virtual public Guid DataID
        {
            get { return dataID; }
        }

        virtual public bool MultiProviderMode
        {
            get { return false; }
        }

        virtual public esColumnMetadataCollection Columns
        {
            get { return null; }
        }

        #endregion

        #region Nested 'esPrivateCollection' class

        private class esPrivateCollection : esEntityCollection<esUtilityEntity>
        {
            private esPrivateCollection() { }

            public esPrivateCollection(esUtility util)
            {
                md = new MetaData(util);
            }

            protected override IMetadata Meta
            {
                get
                {
                    return md;
                }
            }

            #region Nested 'MetaData' class
            protected class MetaData : IMetadata
            {
                public MetaData(esUtility esUtil)
                {
                    this.esUtil = esUtil;
                }

                public Guid DataID
                {
                    get { return esUtil.DataID; }
                }
                public bool MultiProviderMode
                {
                    get { return esUtil.MultiProviderMode; }
                }
                public esColumnMetadataCollection Columns
                {
                    get { return esUtil.Columns; }
                }

                public esProviderSpecificMetadata GetProviderMetadata(string mapName)
                {
                    return esUtil.GetProviderMetadata(mapName);
                }

                private esUtility esUtil;
            }
            #endregion

            MetaData md;
        }

        #endregion

        protected Guid dataID = Guid.NewGuid();
        protected esProviderSpecificMetadata esProv = new esProviderSpecificMetadata();
    }
}
