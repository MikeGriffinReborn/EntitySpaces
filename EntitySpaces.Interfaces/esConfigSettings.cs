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
using System.Collections;
using System.Collections.Generic;
#if DOTNET4||DOTNET5
using System.Configuration;
#endif

namespace EntitySpaces.Interfaces
{
    public sealed class esConfigSettings 
    {
        private esConfigSettings()
        {

        }

        static public esConfigSettings ConnectionInfo
        {
            get
            {
#if DOTNET4 || DOTNET5
                if (esConfigSettings.connectionInfo == null || esConfigSettings.connectionInfo.Connections == null)
                {
                    esConfigSection.InitializeFromConfigSection();
                }
#endif
                return connectionInfo;
            }
        }

        /// <summary>
        /// Returns the default as esConnectionElement.
        /// represented by <connectionInfo default="SQLDynamic">
        /// </summary>
        /// <remarks>
        /// For example, if the EntitySpaces config section contained <connectionInfo default="SQLDynamic"> 
        /// the <see cref="esConnectionElement"/> for "SQLDynamic" would be returned.
        /// </remarks>
        static public esConnectionElement DefaultConnection
        {
            get
            {
                if (esConfigSettings.defaultConnection == null)
                {
                    esConfigSettings ConnectionInfoSettings = esConfigSettings.ConnectionInfo;
#if DOTNET4 || DOTNET5
                    if (ConnectionInfoSettings == null || ConnectionInfoSettings.Connections == null)
                    {
                        esConfigSection.InitializeFromConfigSection();
                    }
#endif

                    foreach (esConnectionElement connection in ConnectionInfoSettings.Connections)
                    {
                        if (connection.Name == ConnectionInfoSettings.Default)
                        {
                            esConfigSettings.defaultConnection = connection;
                            break;
                        }
                    }
                }

                return esConfigSettings.defaultConnection;
            }
        }

        /// <summary>
        /// Unless otherwise specified, the connection information defined by this property
        /// is used by all EntitySpaces object to read and write to the database.
        /// </summary>
        /// <remarks>
        /// The default can be overridden at runtime as follows:
        /// <code>
        /// Employees emp = new Employees();
        /// emp.es.Connection.Name = "Oracle";
        /// emp.LoadByPrimaryKey(1);
        /// </code>
        /// </remarks>
        public String Default
        {
            get { return _default;  }
            set 
            { 
                _default = value;
                esConfigSettings.defaultConnection = null;
            }
        }
        private string _default = String.Empty;

        /// <summary>
        /// This can be used to iterate over all of the Connections in the EntitySpaces configuration section.
        /// </summary>
        /// <remarks>
        /// The example below demonstrates how to loop through all of the Connection information stored
        /// in the EntitySpaces configuration section.
        /// <code>
        /// esConfigSettings ConnectionInfoSettings = esConfigSettings.ConnectionInfo;
        /// foreach (esConnectionElement connection in ConnectionInfoSettings.Connections)
        /// {
        ///		Console.WriteLine(connection.ConnectionString);
        /// }
        /// </code>
        /// </remarks>
        public esConnectionsCollection Connections
        {
            get
            {
                return connections;
            }
        }

        private esConnectionsCollection connections = new esConnectionsCollection();

        static private esConfigSettings connectionInfo = new esConfigSettings();
        static private esConnectionElement defaultConnection;
    }


    /// <summary>
    /// This class serves as a collection for all of the connections in the 
    /// EntitySpaces configuration section. See the .NET Framework class
    /// <see cref="ConfigurationElementCollection"/>
    /// </summary>
    /// <seealso cref="esConfigSettings"/>
    /// <seealso cref="esConnectionElement"/>
    /// <seealso cref="ConfigurationElementCollection"/>
    public class esConnectionsCollection : IEnumerable
    {
        public esConnectionsCollection()
        {

        }

        public esConnectionElement this[int index]
        {
            get
            {
                return this.list[index];
            }
        }

        public esConnectionElement this[string name]
        {
            get
            {
                esConnectionElement element = null;
                if(this.hash.ContainsKey(name))
                {
                    element = this.hash[name];
                }

                return element;
            }
        }

        public void Add(esConnectionElement connection)
        {
            if(!this.hash.ContainsKey(connection.Name))
            {
                this.hash[connection.Name] = connection;
                this.list.Add(connection);
            }
        }

        public void Remove(esConnectionElement connection)
        {
            if(!this.hash.ContainsKey(connection.Name))
            {
                this.list.Remove(connection);
                this.hash.Remove(connection.Name);
            }
        }

        public void Remove(string name)
        {
            if(!this.hash.ContainsKey(name))
            {
                this.hash.Remove(name);
            }
        }

        public void Clear()
        {
            this.hash.Clear();
            this.list.Clear();
        }

    #region IEnumerable Members

        /// <summary>
        /// Supports the foreach() syntax over the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

    #endregion

        private Dictionary<string, esConnectionElement> hash = new Dictionary<string, esConnectionElement>();
        private List<esConnectionElement> list = new List<esConnectionElement>();
    }

    /// <summary>
    /// This contains the detail connection information for an entry in the
    /// EntitySpaces configuration section.
    /// </summary>
    public class esConnectionElement 
    {
        public esConnectionElement()
        {
            Name = "RemoteDb";
            ProviderMetadataKey = "esDefault";
            ProviderClass = "DataProvider";
            SqlAccessType = esSqlAccessType.DynamicSQL;

            esConfigSettings.ConnectionInfo.Default = "RemoteDb";
        }

        /// <summary>
        /// Constructor used internally by EntitySpaces
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        public esConnectionElement(String provider, String connectionString)
        {
            this.Provider = provider;
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// The name moniker of the connection.
        /// </summary>
        /// <remarks>
        /// The name of a connection can be used to override the default connection
        /// as follows.
        /// <code>
        /// Employees emp = new Employees();
        /// emp.es.Connection.Name = "Oracle";
        /// emp.LoadByPrimaryKey(1);
        /// </code>
        /// </remarks>
        public string Name
        {
            get { return _name;  }
            set { _name = value; }
        }
        private string _name;

        /// <summary>
        /// The EntitySpaces DataProvider to which all commands using this connection are routed.
        /// </summary>
        public string Provider
        {
            get { return _provider;  }
            set { _provider = value; }
        }
        private string _provider;

        /// <summary>
        /// Either "DataProvider" or "DataProviderEnterprise"
        /// </summary>
        /// <remarks>
        /// When using "DataProvider" the esTransactionScope class is used internally by
        /// EntitySpaces and uses ADO.NET connection based transactions. When "DataProviderEnterprise"
        /// is used the TransactionScope classes is used thereby using true distributed transactions.
        /// </remarks>
        public string ProviderClass
        {
            get { return _providerClass;  }
            set { _providerClass = value; }
        }
        private string _providerClass;

        /// <summary>
        /// Set to either "DynamicSQL" or "StoredProcedure". This can be overridden at
        /// the command level if needed, but this isn't usually overridden.
        /// </summary>
        /// <remarks>
        /// An example of explicity declaring the sqlAccessType.
        /// <code>
        /// Employees emp = new Employees();
        ///	emp.es.Connection.Name = "Oracle";
        /// emp.LoadByPrimaryKey(esSqlAccessType.StoredProcedure, 1);
        /// emp.FirstName = "Joe";
        /// emp.Save(esSqlAccessType.DynamicSQL);
        /// </code>
        /// </remarks>
        public esSqlAccessType SqlAccessType
        {
            get { return _esSqlAccessType;  }
            set { _esSqlAccessType = value; }
        }
        private esSqlAccessType _esSqlAccessType;

        /// <summary>
        /// This is normally created as "esDefault" and should only be explicitly
        /// provided during code generation if you want the same
        /// exact binary to run on two different DBMS systems.
        /// </summary>
        public string ProviderMetadataKey
        {
            get { return _providerMetadataKey;  }
            set { _providerMetadataKey = value; }
        }
        private string _providerMetadataKey;

        /// <summary>
        /// The actually connection string. This can be encrypted. 
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString;  }
            set { _connectionString = value; }
        }
        private string _connectionString;

        /// <summary>
        /// There are certain cases where a provider might use different logic
        /// based on the database version. For instance, Microsoft SQL 2005
        /// depreciated the TSEQUAL function that was used in SQL 2000 with
        /// timestamp columns. For SQL Server we recommend you provide EntitySpaces
        /// with either 2000 or 2005.
        /// </summary>
        public string DatabaseVersion
        {
            get { return _databaseVersion;  }
            set { _databaseVersion = value; }
        }
        private string _databaseVersion;

        /// <summary>
        /// This is optional. You can use this to override the default timeout
        /// for all commands issued agains this database.
        /// </summary>
        public int? CommandTimeout
        {
            get { return _commandTimeout;  }
            set { _commandTimeout = value; }
        }
        private int? _commandTimeout;

        /// <summary>
        /// This is optional. You can use this to indiate the catalog for your 
        /// database access
        /// </summary>
        public string Catalog
        {
            get { return catalog; }
            set { catalog = value; }
        }
        private string catalog;

        /// <summary>
        /// This is optional. You can use this to indiate the schema for your 
        /// database access
        /// </summary>
        public string Schema
        {
            get { return schema; }
            set { schema = value; }
        }
        private string schema;
    }
}