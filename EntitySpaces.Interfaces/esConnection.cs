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
    /// Configured by the EntitySpaces section from the app or web config file.
    /// </summary>
    /// <remarks>
    /// Below is an example EntitySpaces configuration section as would be stored in the
    /// web.config or app.config file. Notice below on the "connectionInfo" element that
    /// the default is "MyMicrosoftDB". The default, in this case, "MyMicrosoftDB" will
    /// be used for all of your EntitySpaces provider calls. However, this can be easily
    /// overridden by overriding the connection name property. See the Name property of this
    /// class for more information. Notice that the "MyOracleDB" connection uses the
    /// providerMetadataKey of "esOracle". You SHOULD always use "esDefault" unless you
    /// are building a binary compatible, multi-database application.<BR></BR>
    /// Connection information is not serialized.
    /// <code>
    ///	&lt;EntitySpaces&gt;
    ///		&lt;connectionInfo default="MyMicrosoftDB"&gt;
    ///			&lt;connections&gt;
    ///			
    ///			   &lt;add name="MyMicrosoftDB"
    ///				   providerMetadataKey="esDefault"
    ///				   sqlAccessType="DynamicSQL"
    ///				   provider="EntitySpaces.SqlClientProvider"
    ///				   providerClass="DataProvider"
    ///				   connectionString="User ID=sa;Password=secret;Initial Catalog=Northwind;Data Source=localhost;" 
    ///				   commandTimeout="90"
    ///				   databaseVersion="2005"/&gt;
    ///				   
    ///				&lt;add name="MyOracleDB" 
    ///				  providerMetadataKey="esOracle" 
    ///				  sqlAccessType="DynamicSQL" 
    ///				  provider="EntitySpaces.OracleClientProvider" 
    ///				  providerClass="DataProvider"           
    ///				  connectionString="Password=retina;Persist Security Info=True;User ID=MyDB;Data Source=127.0.0.1;" /&gt;
    ///				  
    ///			   &lt;/connections&gt;
    ///		&lt;/connectionInfo&gt;
    ///	&lt;/EntitySpaces&gt;
    /// </code>
    /// </remarks>
    public class esConnection
    {
        static private IConnectionNameService connectionService = null;

        /// <summary>
        /// If assigned this interface is responsible for service up all connection names.
        /// <seealso cref="IConnectionNameService"/>
        /// </summary>
        static public IConnectionNameService ConnectionService
        {
            get { return connectionService; }
            set { connectionService = value; }
        }
          

        /// <summary>
        /// This nested class is used to extract the information from the EntitySpaces configuration section
        /// of a connection in order to invoke the provider. 
        /// </summary>
        public esProviderSignature ProviderSignature
        {
            get
            {
                if (this.providerSignature == null)
                {
                    this.providerSignature = new esProviderSignature();
                    this.providerSignature.DataProviderName = esConfigSettings.DefaultConnection.Provider;
                    this.providerSignature.DataProviderClass = esConfigSettings.DefaultConnection.ProviderClass;
                    this.providerSignature.DatabaseVersion = esConfigSettings.DefaultConnection.DatabaseVersion;
                }
                return this.providerSignature;
            }
        }

        /// <summary>
        /// Maps to the "connectionString" in the EntitySpaces configuration section. It is possible for 
        /// this connection string to be encrypted.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                string cnString = String.Empty;

                if (this.connectionString == null)
                    cnString = esConfigSettings.DefaultConnection.ConnectionString;
                else
                {
                    cnString = this.connectionString;

                    if (!this.converted)
                    {
                        // Check for 'AppSettings:' as the leading string in the ConnectionString in the config
                        // section, if present the the characters following "AppSettings:" are the 'name' of an
                        // entry in the <connectionStrings> section (the standard .NET area to store connection string)
                        if (cnString != null && cnString.StartsWith("AppSettings:"))
                        {
                            int index = cnString.IndexOf(':');
                            string name = cnString.Substring(index + 1);
                            this.connectionString = cnString = "";
                                //System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;

                            esConfigSettings.DefaultConnection.ConnectionString = cnString;

                        }

                        this.converted = true;
                    }
                }

                return cnString;
            }

            set { this.connectionString = value;  }
        }

        /// <summary>
        /// Maps to the "providerMetadataKey" in the EntitySpaces configuration section.
        /// </summary>
        public string ProviderMetadataKey
        {
            get
            {
                string key = null;

                if (this.providerMetadataKey == null)
                {
                    if (esConfigSettings.DefaultConnection != null)
                    {
                        key = esConfigSettings.DefaultConnection.ProviderMetadataKey;
                    }
                }
                else
                {
                    key = this.providerMetadataKey;
                }

                return key;
            }
            set { this.providerMetadataKey = value; }
        }

        /// <summary>
        /// Maps to the "sqlAccessType" in the EntitySpaces configuration section. See <see cref="esSqlAccessType"/>
        /// </summary>
        public esSqlAccessType SqlAccessType
        {
            get
            {
                if (this.sqlAccessType == esSqlAccessType.Unassigned)
                    return esConfigSettings.DefaultConnection.SqlAccessType;
                else
                    return this.sqlAccessType;
            }
            set { this.sqlAccessType = value; }
        }

        /// <summary>
        /// Optional. Maps to the "databaseVersion" in the EntitySpaces configuration section.
        /// </summary>
        public string DatabaseVersion
        {
            get
            {
                if (this.databaseVersion == null && esConfigSettings.DefaultConnection != null)
                    return esConfigSettings.DefaultConnection.DatabaseVersion;
                else
                    return this.databaseVersion;
            }
            set { this.databaseVersion = value; }
        }

        /// <summary>
        /// Optional. Maps to the "commandTimeout" in the EntitySpaces configuration section.
        /// </summary>
        public int? CommandTimeout
        {
            get
            {
                if (this.commandTimeout == null && esConfigSettings.DefaultConnection != null)
                    return esConfigSettings.DefaultConnection.CommandTimeout;
                else
                    return this.commandTimeout;
            }
            set { this.commandTimeout = value; }
        }

        /// <summary>
        /// Optional. Allows you to set the Schema for you database access.
        /// </summary>
        public string Schema
        {
            get
            {
                if (this.schema == null && esConfigSettings.DefaultConnection != null)
                    return esConfigSettings.DefaultConnection.Schema;
                else
                    return this.schema;
            }
            set { this.schema = value; }
        }

        /// <summary>
        /// Optional. Allows you to set the Catalog for you database access.
        /// </summary>
        public string Catalog
        {
            get
            {
                if (this.catalog == null && esConfigSettings.DefaultConnection != null)
                    return esConfigSettings.DefaultConnection.Catalog;
                else
                    return this.catalog;
            }
            set { this.catalog = value; }
        }

        /// <summary>
        /// Maps to the "name" in the EntitySpaces configuration section. 
        /// </summary>
        /// <remarks>
        /// The default "name" in the EntitySpaces configuration section 
        /// can be overridden at runtime as follows:
        /// <code>
        /// Employees emp = new Employees();
        /// emp.es.Connection.Name = "Oracle";
        /// emp.LoadByPrimaryKey(1);
        /// </code>
        /// </remarks>
        public string Name
        {
            get
            {
                if (this.name == null && esConfigSettings.DefaultConnection != null)
                    return esConfigSettings.DefaultConnection.Name;
                else
                    return this.name;
            }
            set 
            {
                foreach(esConnectionElement conn in esConfigSettings.ConnectionInfo.Connections)
                {
                    if (conn.Name == value)
                    {
                        this.name = value;
                        this.providerSignature = new esProviderSignature();
                        this.providerSignature.DataProviderName = conn.Provider;
                        this.providerSignature.DataProviderClass = conn.ProviderClass;
                        this.providerSignature.DatabaseVersion = conn.DatabaseVersion;
                        this.connectionString = conn.ConnectionString;
                        this.providerMetadataKey = conn.ProviderMetadataKey;
                        this.sqlAccessType = conn.SqlAccessType;
                        this.commandTimeout = conn.CommandTimeout;
                        this.catalog = conn.Catalog;
                        this.schema = conn.Schema;

                        break;
                    }
                }
            }
        }

        private string name;

        private string catalog;
        private string schema;

        private bool converted = false;

        [NonSerialized]
        private esProviderSignature providerSignature;

        [NonSerialized]
        private string connectionString;
        [NonSerialized]
        private string providerMetadataKey;
        [NonSerialized]
        private string databaseVersion;
        [NonSerialized]
        private int? commandTimeout;
        [NonSerialized]
        private esSqlAccessType sqlAccessType;
    }
}
