/*  New BSD License
-------------------------------------------------------------------------------
Copyright (c) 2021, Stefan D.
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

#if DOTNET4||DOTNET5
using System.Configuration;

namespace EntitySpaces.Interfaces
{
    public sealed class esConfigConnectionInfoElement : ConfigurationElement
    {
        [ConfigurationProperty("default", IsKey = false, IsRequired = false)]
        public string DefaultConnection
        {
            get { return (string)this["default"]; }
            set { this["default"] = value; }
        }

        // Declare the ConnectionInfoCollection collection property.
        [ConfigurationProperty("connections", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(esConfigConnectionElementCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public esConfigConnectionElementCollection Connections
        {
            get { return (esConfigConnectionElementCollection)this["connections"]; }
            set { this["connections"] = value; }
        }
    }

    [ConfigurationCollection(typeof(esConfigConnectionElementCollection))]
    public sealed class esConfigConnectionInfoCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new esConfigConnectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((esConfigConnectionElementCollection)element);
        }

        [ConfigurationProperty("default", IsKey = false, IsRequired = false)]
        public string DefaultConnection
        {
            get { return (string)this["default"]; }
            set { this["default"] = value; }
        }

    }

    public sealed class esConfigConnectionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("providerMetadataKey", IsRequired = true, DefaultValue = "esDefault")]
        public string ProviderMetadataKey
        {
            get { return (string)this["providerMetadataKey"]; }
            set { this["providerMetadataKey"] = value; }
        }

        [ConfigurationProperty("sqlAccessType", IsRequired = true, DefaultValue = "DynamicSQL")]
        public string SqlAccessType
        {
            get { return (string)this["sqlAccessType"]; }
            set { this["sqlAccessType"] = value; }
        }

        [ConfigurationProperty("provider", IsRequired = true)]
        public string Provider
        {
            get { return (string)this["provider"]; }
            set { this["provider"] = value; }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }

        [ConfigurationProperty("providerClass", IsRequired = true, DefaultValue = "DataProvider")]
        public string ProviderClass
        {
            get { return (string)this["providerClass"]; }
            set { this["providerClass"] = value; }
        }

        [ConfigurationProperty("schema", IsRequired = false)]
        public string Schema
        {
            get { return (string)this["schema"]; }
            set { this["schema"] = value; }
        }

        [ConfigurationProperty("catalog", IsRequired = false)]
        public string Catalog
        {
            get { return (string)this["catalog"]; }
            set { this["catalog"] = value; }
        }

        [ConfigurationProperty("databaseVersion", IsRequired = false)]
        public string DatabaseVersion
        {
            get { return (string)this["databaseVersion"]; }
            set { this["databaseVersion"] = value; }
        }

        [ConfigurationProperty("commandTimeout", IsRequired = false)]
        public int? CommandTimeout
        {
            get { return (int?)this["commandTimeout"]; }
            set { this["commandTimeout"] = value; }
        }
    }


    [ConfigurationCollection(typeof(esConfigConnectionElement))]
    public sealed class esConfigConnectionElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new esConfigConnectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((esConfigConnectionElement)element).Name;
        }
    }


    /// <summary>
    /// Class for Reading the configuration from app.config or web.config
    /// ...
    ///   <configSections>
    ///     <section name = "EntitySpaces" type="EntitySpaces.Interfaces.esConfigSection,TestEsConfigurationSection" />
    ///   </configSections>
    /// 
    ///   <EntitySpaces>
    ///      <connectionInfo default="APP">
    ///       <connections>
    ///         <!-- Define your DatabaseConnections here (Sample for Oracle managed driver) -->
    ///         <add name = "APP" providerMetadataKey="esDefault" sqlAccessType="DynamicSQL" provider="EntitySpaces.OracleManagedClientProvider" providerClass="DataProvider" connectionString="Password=xxxPASSWORDxxx;Persist Security Info=True;User ID=xxxUSERNAMExxx;Data Source=(DESCRIPTION=(SDU = 32767)(TDU = 32767)(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = xxxHOSTNAMExxx)(PORT = 1521))(ADDRESS = (PROTOCOL = TCP)(HOST = xxxHOSTNAMExxx)(PORT = 1526)))(CONNECT_DATA =(SERVICE_NAME = xxxSERVICE_NAMExxx)));" />
    ///         <add name = "APP_DEBUG" providerMetadataKey="esDefault" sqlAccessType="DynamicSQL" provider="EntitySpaces.OracleManagedClientProvider" providerClass="DataProvider" connectionString="Password=xxxPASSWORDxxx;Persist Security Info=True;User ID=xxxUSERNAMExxx;Data Source=(DESCRIPTION=(SDU = 32767)(TDU = 32767)(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = xxxHOSTNAMExxx)(PORT = 1521))(ADDRESS = (PROTOCOL = TCP)(HOST = xxxHOSTNAMExxx)(PORT = 1526)))(CONNECT_DATA =(SERVICE_NAME = xxxSERVICE_NAMExxx)));" />
    ///         <add name = "CONNECTION2" providerMetadataKey="esDefault" sqlAccessType="DynamicSQL" provider="EntitySpaces.OracleManagedClientProvider" providerClass="DataProvider" connectionString="Password=xxxPASSWORDxxx;Persist Security Info=True;User ID=xxxUSERNAMExxx;Data Source=(DESCRIPTION=(SDU = 32767)(TDU = 32767)(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = xxxHOSTNAMExxx)(PORT = 1521))(ADDRESS = (PROTOCOL = TCP)(HOST = xxxHOSTNAMExxx)(PORT = 1526)))(CONNECT_DATA =(SERVICE_NAME = xxxSERVICE_NAMExxx)));" />
    ///       </connections>
    ///      </connectionInfo>
    ///   </EntitySpaces>
    /// </summary>
    public sealed class esConfigSection : ConfigurationSection
    {
        static esConfigSection()
        {
            // Load at startup
            InitializeFromConfigSection();
        }

        [ConfigurationProperty("connectionInfo", IsDefaultCollection = true)]
        public esConfigConnectionInfoElement connectionInfo
        {
            get { return (esConfigConnectionInfoElement)this["connectionInfo"]; }
            set { this["connectionInfo"] = value; }
        }

        private static bool initalisationCompleted = false;
        public static void InitializeFromConfigSection()
        {
            // Initialize only once
            if (initalisationCompleted)
                return;
            esConfigSection configSection = (esConfigSection)System.Configuration.ConfigurationManager.GetSection("EntitySpaces");
            esConfigConnectionInfoElement connInfo = configSection.connectionInfo;
            if (connInfo == null)
            {
                return;
            }

            foreach (esConfigConnectionElement connElement in connInfo.Connections)
            {

                esConnectionElement esCon = new esConnectionElement();
                esCon.Name = connElement.Name;
                esCon.Provider = connElement.Provider;
                esCon.ProviderMetadataKey = connElement.ProviderMetadataKey;
                esCon.SqlAccessType = esSqlAccessType.DynamicSQL;
                esCon.Provider = connElement.Provider;
                esCon.ProviderClass = connElement.ProviderClass;
                esCon.Schema = connElement.Schema;
                esCon.ConnectionString = connElement.ConnectionString;
                esConfigSettings.ConnectionInfo.Connections.Add(esCon);
            }
            esConfigSettings.ConnectionInfo.Default = connInfo.DefaultConnection;
            initalisationCompleted = true;
        }
    }

}
#endif
