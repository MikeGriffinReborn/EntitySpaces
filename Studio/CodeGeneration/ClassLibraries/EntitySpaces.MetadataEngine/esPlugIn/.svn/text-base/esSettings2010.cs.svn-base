using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Win32;

namespace EntitySpaces.MetadataEngine
{
    [Serializable]
    public class esSettings2010 : ISettings
    {
        public esSettings2010()
        {
            this.SetDefaultSettings();
        }

        #region Properties

        #region Connection

        private string _driver;
        public string Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        public string DriverName(string driver)
        {
            switch (driver)
            {
                case "Access":
                    return "Access";
                case "EffiProzDB":
                    return "EffiProzDB";
                case "MySql":
                    return "MySQL";
                case "Oracle":
                    return "Oracle";
                case "PostgreSQL":
                    return "PostgreSQL";
                case "SQL":
                    return "SQL Server";
                case "SQLAzure":
                    return "SQL Azure";
                case "SQLCE":
                    return "SQL Server CE";
                case "SQLite":
                    return "SQLite";
                case "VistaDB":
                    return "VistaDB";
                case "VistaDB4":
                    return "VistaDB4";
                case "Sybase":
                    return "Sybase";
                default:
                    return String.Empty;
            }
        }

        private string _connectionString;
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        #endregion

        #region File Locations

        public string AppDataPath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                path += @"\EntitySpaces\ES2010";
                return path;
            }
        }

        public string TemplateCachePath
        {
            get
            {
                string path = this.AppDataPath;
                path += @"\TemplateCache";
                return path;
            }
        }

        public string InstallPath
        {
            get
            {
                string path = @"C:\Program Files\EntitySpaces 2010\";

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2010", false);
                if (key != null)
                {
                    path = (string)key.GetValue("Install_Dir");

                    if (!path.EndsWith(@"\"))
                    {
                        path += @"\";
                    }
                }

                return path;
            }
        }

        private string _templatePath;
        public string TemplatePath
        {
            get { return _templatePath; }
            set { _templatePath = value; }
        }

        private string _outputPath;
        public string OutputPath
        {
            get { return _outputPath; }
            set { _outputPath = value; }
        }

        private string _uiAssemblyPath;
        public string UIAssemblyPath
        {
            get { return _uiAssemblyPath; }
            set { _uiAssemblyPath = value; }
        }

        private string _compilerAssemblyPath;
        public string CompilerAssemblyPath
        {
            get { return _compilerAssemblyPath; }
            set { _compilerAssemblyPath = value; }
        }

        private string _languageMappingFile;
        public string LanguageMappingFile
        {
            get { return _languageMappingFile; }
            set { _languageMappingFile = value; }
        }

        private string _userMetadataFile;
        public string UserMetadataFile
        {
            get { return _userMetadataFile; }
            set { _userMetadataFile = value; }
        }

        #endregion

        #region Class Names

        private string _abstractPrefix;
        public string AbstractPrefix
        {
            get { return _abstractPrefix; }
            set { _abstractPrefix = value; }
        }

        private string _entitySuffix;
        public string EntitySuffix
        {
            get { return _entitySuffix; }
            set { _entitySuffix = value; }
        }

        private string _collectionSuffix;
        public string CollectionSuffix
        {
            get { return _collectionSuffix; }
            set { _collectionSuffix = value; }
        }

        private string _querySuffix;
        public string QuerySuffix
        {
            get { return _querySuffix; }
            set { _querySuffix = value; }
        }

        private string _metadataSuffix;
        public string MetadataSuffix
        {
            get { return _metadataSuffix; }
            set { _metadataSuffix = value; }
        }

        private string _proxyStubSuffix;
        public string ProxyStubSuffix
        {
            get { return _proxyStubSuffix; }
            set { _proxyStubSuffix = value; }
        }

        private bool _prefixWithSchema;
        public bool PrefixWithSchema
        {
            get { return _prefixWithSchema; }
            set { _prefixWithSchema = value; }
        }

        #endregion

        #region Stored Procedure Names

        private string _procPrefix;
        public string ProcPrefix
        {
            get { return _procPrefix; }
            set { _procPrefix = value; }
        }

        private string _procInsert;
        public string ProcInsert
        {
            get { return _procInsert; }
            set { _procInsert = value; }
        }

        private string _procUpdate;
        public string ProcUpdate
        {
            get { return _procUpdate; }
            set { _procUpdate = value; }
        }

        private string _procDelete;
        public string ProcDelete
        {
            get { return _procDelete; }
            set { _procDelete = value; }
        }

        private string _procLoadAll;
        public string ProcLoadAll
        {
            get { return _procLoadAll; }
            set { _procLoadAll = value; }
        }

        private string _procLoadByPK;
        public string ProcLoadByPK
        {
            get { return _procLoadByPK; }
            set { _procLoadByPK = value; }
        }

        private string _procSuffix;
        public string ProcSuffix
        {
            get { return _procSuffix; }
            set { _procSuffix = value; }
        }

        private bool _procVerbFirst;
        public bool ProcVerbFirst
        {
            get { return _procVerbFirst; }
            set { _procVerbFirst = value; }
        }

        #endregion

        #region Hierarchical Names

        private string _onePrefix;
        public string OnePrefix
        {
            get { return _onePrefix; }
            set { _onePrefix = value; }
        }

        private string _oneSeparator;
        public string OneSeparator
        {
            get { return _oneSeparator; }
            set { _oneSeparator = value; }
        }

        private string _oneSuffix;
        public string OneSuffix
        {
            get { return _oneSuffix; }
            set { _oneSuffix = value; }
        }

        private string _manyPrefix;
        public string ManyPrefix
        {
            get { return _manyPrefix; }
            set { _manyPrefix = value; }
        }

        private string _manySeparator;
        public string ManySeparator
        {
            get { return _manySeparator; }
            set { _manySeparator = value; }
        }

        private string _manySuffix;
        public string ManySuffix
        {
            get { return _manySuffix; }
            set { _manySuffix = value; }
        }

        private bool _selfOnly;
        public bool SelfOnly
        {
            get { return _selfOnly; }
            set { _selfOnly = value; }
        }

        private bool _swapNames;
        public bool SwapNames
        {
            get { return _swapNames; }
            set { _swapNames = value; }
        }

        private bool _useAssociativeName;
        public bool UseAssociativeName
        {
            get { return _useAssociativeName; }
            set { _useAssociativeName = value; }
        }

        private bool _useUpToPrefix;
        public bool UseUpToPrefix
        {
            get { return _useUpToPrefix; }
            set { _useUpToPrefix = value; }
        }

        #endregion

        #region Miscellaneous

        private bool _preserveUnderscores;
        public bool PreserveUnderscores
        {
            get { return _preserveUnderscores; }
            set { _preserveUnderscores = value; }
        }

        private bool _useRawNames;
        public bool UseRawNames
        {
            get { return _useRawNames; }
            set { _useRawNames = value; }
        }

        #endregion

        #region Other

        private bool _useNullableTypesAlways;
        public bool UseNullableTypesAlways
        {
            get { return _useNullableTypesAlways; }
            set { _useNullableTypesAlways = value; }
        }

        private bool _turnOffDateTimeInClassHeaders;
        public bool TurnOffDateTimeInClassHeaders
        {
            get { return _turnOffDateTimeInClassHeaders; }
            set { _turnOffDateTimeInClassHeaders = value; }
        }

        private string _defaultTemplateDoubleClickAction;
        public string DefaultTemplateDoubleClickAction
        {
            get { return _defaultTemplateDoubleClickAction; }
            set { _defaultTemplateDoubleClickAction = value; }
        }

        #endregion

        #region License

        private bool _licenseProxyEnable;
        public bool LicenseProxyEnable
        {
            get { return _licenseProxyEnable; }
            set { _licenseProxyEnable = value; }
        }

        private string _licenseProxyUrl;
        public string LicenseProxyUrl
        {
            get { return _licenseProxyUrl; }
            set { _licenseProxyUrl = value; }
        }

        private string _licenseProxyUserName;
        public string LicenseProxyUserName
        {
            get { return _licenseProxyUserName; }
            set { _licenseProxyUserName = value; }
        }

        private string _licenseProxyPassword;
        public string LicenseProxyPassword
        {
            get { return _licenseProxyPassword; }
            set { _licenseProxyPassword = value; }
        }

        private string _licenseProxyDomainName;
        public string LicenseProxyDomainName
        {
            get { return _licenseProxyDomainName; }
            set { _licenseProxyDomainName = value; }
        }

        #endregion

        #endregion

        /// <summary>
        /// Loads the user's default settings.
        /// If none exist, a file is created with the EntitySpaces default settings.
        /// </summary>
        public void Load()
        {
            string pathAndFileName = string.Empty;

            //Module executingAssembly = System.Reflection.Assembly.GetEntryAssembly().ManifestModule;
            //string exe = executingAssembly.Name;

            ////--------------------------------------------------------
            //// Memory Stick override
            ////--------------------------------------------------------
            //if (exe.Equals("EntitySpaces.exe", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    FileInfo info = new FileInfo(executingAssembly.FullyQualifiedName);
            //    pathAndFileName = info.DirectoryName + @"\esSettings.xml";

            //    if (File.Exists(pathAndFileName))
            //    {
            //        this.Load(pathAndFileName);

            //        this.AdjustPathsForTravelMode(executingAssembly);

            //        if (!Directory.Exists(this.TemplateCachePath))
            //            Directory.CreateDirectory(this.TemplateCachePath);

            //        return;
            //    }
            //}

            //--------------------------------------------------------
            // Okay, we didn't load the Memory Stick settings file
            //--------------------------------------------------------
            pathAndFileName = this.AppDataPath + @"\esSettings.xml";

            if (!Directory.Exists(this.AppDataPath))
            {
                Directory.CreateDirectory(this.AppDataPath);
                this.SetDefaultSettings();
                this.Save();
            }
            else if (!File.Exists(pathAndFileName))
            {
                this.SetDefaultSettings();
                this.Save();
            }
            else
            {
                this.Load(pathAndFileName);
            }

            if (!Directory.Exists(this.TemplateCachePath))
                Directory.CreateDirectory(this.TemplateCachePath);
        }

        public void Load(string pathAndFileName)
        {
            XmlTextReader reader = null;
            this.SetDefaultSettings();

            if (File.Exists(pathAndFileName))
            {
                try
                {
                    Hashtable h = new Hashtable();

                    reader = new XmlTextReader(pathAndFileName);
                    reader.WhitespaceHandling = WhitespaceHandling.None;

                    reader.Read();
                    reader.Read();

                    if (reader.Name != "esSettings")
                    {
                        throw new Exception("Invalid Settings File: " + pathAndFileName + "'");
                    }

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }
                        else
                        {
                            h.Add(reader.GetAttribute("Name"), reader.GetAttribute("Value"));
                        }
                    }

                    // Connection
                    this.Driver = h.Contains("Driver") ? h["Driver"].ToString() : this.Driver;
                    this.ConnectionString = h.Contains("ConnectionString") ? h["ConnectionString"].ToString() : this.ConnectionString;

                    // File Locations
                    this.TemplatePath = h.Contains("TemplatePath") ? h["TemplatePath"].ToString() : this.TemplatePath;
                    this.OutputPath = h.Contains("OutputPath") ? h["OutputPath"].ToString() : this.OutputPath;
                    this.UIAssemblyPath = h.Contains("UIAssemblyPath") ? h["UIAssemblyPath"].ToString() : this.UIAssemblyPath;
                    this.CompilerAssemblyPath = h.Contains("CompilerAssemblyPath") ? h["CompilerAssemblyPath"].ToString() : this.CompilerAssemblyPath;
                    this.LanguageMappingFile = h.Contains("LanguageMappingFile") ? h["LanguageMappingFile"].ToString() : this.LanguageMappingFile;
                    this.UserMetadataFile = h.Contains("UserMetadataFile") ? h["UserMetadataFile"].ToString() : this.UserMetadataFile;

                    // Class Names
                    this.AbstractPrefix = h.Contains("AbstractPrefix") ? h["AbstractPrefix"].ToString() : this.AbstractPrefix;
                    this.EntitySuffix = h.Contains("EntitySuffix") ? h["EntitySuffix"].ToString() : this.EntitySuffix;
                    this.CollectionSuffix = h.Contains("CollectionSuffix") ? h["CollectionSuffix"].ToString() : this.CollectionSuffix;
                    this.QuerySuffix = h.Contains("QuerySuffix") ? h["QuerySuffix"].ToString() : this.QuerySuffix;
                    this.MetadataSuffix = h.Contains("MetadataSuffix") ? h["MetadataSuffix"].ToString() : this.MetadataSuffix;
                    this.ProxyStubSuffix = h.Contains("ProxyStubSuffix") ? h["ProxyStubSuffix"].ToString() : this.ProxyStubSuffix;
                    this.PrefixWithSchema = h.Contains("PrefixWithSchema") ? Convert.ToBoolean(h["PrefixWithSchema"].ToString()) : this.PrefixWithSchema;

                    // Stored Procedure names
                    this.ProcDelete = h.Contains("ProcDelete") ? h["ProcDelete"].ToString() : this.ProcDelete;
                    this.ProcInsert = h.Contains("ProcInsert") ? h["ProcInsert"].ToString() : this.ProcInsert;
                    this.ProcLoadAll = h.Contains("ProcLoadAll") ? h["ProcLoadAll"].ToString() : this.ProcLoadAll;
                    this.ProcLoadByPK = h.Contains("ProcLoadByPK") ? h["ProcLoadByPK"].ToString() : this.ProcLoadByPK;
                    this.ProcPrefix = h.Contains("ProcPrefix") ? h["ProcPrefix"].ToString() : this.ProcPrefix;
                    this.ProcSuffix = h.Contains("ProcSuffix") ? h["ProcSuffix"].ToString() : this.ProcSuffix;
                    this.ProcUpdate = h.Contains("ProcUpdate") ? h["ProcUpdate"].ToString() : this.ProcUpdate;
                    this.ProcVerbFirst = h.Contains("ProcVerbFirst") ? Convert.ToBoolean(h["ProcVerbFirst"].ToString()) : this.ProcVerbFirst;

                    // Hierarchical Names
                    this.OnePrefix = h.Contains("OneToOnePrefix") ? h["OneToOnePrefix"].ToString() : this.OnePrefix;
                    this.OneSeparator = h.Contains("OneToOneSeparator") ? h["OneToOneSeparator"].ToString() : this.OneSeparator;
                    this.OneSuffix = h.Contains("OneToOneSuffix") ? h["OneToOneSuffix"].ToString() : this.OneSuffix;
                    this.ManyPrefix = h.Contains("ManyToManyPrefix") ? h["ManyToManyPrefix"].ToString() : this.ManyPrefix;
                    this.ManySeparator = h.Contains("ManyToManySeparator") ? h["ManyToManySeparator"].ToString() : this.ManySeparator;
                    this.ManySuffix = h.Contains("ManyToManySuffix") ? h["ManyToManySuffix"].ToString() : this.ManySuffix;
                    this.SelfOnly = h.Contains("SelfOnly") ? Convert.ToBoolean(h["SelfOnly"].ToString()) : this.SelfOnly;
                    this.SwapNames = h.Contains("SwapNames") ? Convert.ToBoolean(h["SwapNames"].ToString()) : this.SwapNames;
                    this.UseAssociativeName = h.Contains("UseAssociativeName") ? Convert.ToBoolean(h["UseAssociativeName"].ToString()) : this.UseAssociativeName;
                    this.UseUpToPrefix = h.Contains("UseUpToPrefix") ? Convert.ToBoolean(h["UseUpToPrefix"].ToString()) : this.UseUpToPrefix;

                    // Miscellaneous
                    this.PreserveUnderscores = h.Contains("PreserveUnderscores") ? Convert.ToBoolean(h["PreserveUnderscores"].ToString()) : this.PreserveUnderscores;
                    this.UseRawNames = h.Contains("UseRawNames") ? Convert.ToBoolean(h["UseRawNames"].ToString()) : this.UseRawNames;

                    // Other
                    this.UseNullableTypesAlways = h.Contains("UseNullableTypesAlways") ? Convert.ToBoolean(h["UseNullableTypesAlways"].ToString()) : this.UseNullableTypesAlways;
                    this.TurnOffDateTimeInClassHeaders = h.Contains("TurnOffDateTimeInClassHeaders") ? Convert.ToBoolean(h["TurnOffDateTimeInClassHeaders"].ToString()) : this.TurnOffDateTimeInClassHeaders;
                    this.DefaultTemplateDoubleClickAction = h.Contains("DefaultTemplateDoubleClickAction") ? h["DefaultTemplateDoubleClickAction"].ToString() : this.DefaultTemplateDoubleClickAction;

                    // License 
                    this.LicenseProxyEnable = h.Contains("LicenseProxyEnable") ? Convert.ToBoolean(h["LicenseProxyEnable"].ToString()) : this.LicenseProxyEnable;
                    this.LicenseProxyUrl = h.Contains("LicenseProxyUrl") ? h["LicenseProxyUrl"].ToString() : this.LicenseProxyUrl;
                    this.LicenseProxyUserName = h.Contains("LicenseProxyUserName") ? h["LicenseProxyUserName"].ToString() : this.LicenseProxyUserName;
                    this.LicenseProxyPassword = h.Contains("LicenseProxyPassword") ? h["LicenseProxyPassword"].ToString() : this.LicenseProxyPassword;
                    this.LicenseProxyDomainName = h.Contains("LicenseProxyDomainName") ? h["LicenseProxyDomainName"].ToString() : this.LicenseProxyDomainName;

                    if ((this.LicenseProxyUrl != null && this.LicenseProxyUrl.Length > 0) ||
                        (this.LicenseProxyUserName != null && this.LicenseProxyUserName.Length > 0) ||
                        (this.LicenseProxyPassword != null && this.LicenseProxyPassword.Length > 0) ||
                        (this.LicenseProxyDomainName != null && this.LicenseProxyDomainName.Length > 0))
                    {
                        Licensing licensing = new Licensing();
                        string id = licensing.getUniqueID("C");

                        Crypto crypto = new Crypto();
                        this.LicenseProxyUrl = crypto.DecryptStringAES(this.LicenseProxyUrl, id);
                        this.LicenseProxyUserName = crypto.DecryptStringAES(this.LicenseProxyUserName, id);
                        this.LicenseProxyPassword = crypto.DecryptStringAES(this.LicenseProxyPassword, id);
                        this.LicenseProxyDomainName = crypto.DecryptStringAES(this.LicenseProxyDomainName, id);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        public void Load(XmlReader reader)
        {
            this.SetDefaultSettings();

            Hashtable h = new Hashtable();

            while (reader.Read())
            {
                if (reader.GetAttribute("Name") == "esCustom")
                {
                    break;
                }
            }
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
                else
                {
                    h.Add(reader.GetAttribute("Name"), reader.GetAttribute("Value"));
                }
            }

            // Connection
            this.Driver = h.Contains("Driver") ? h["Driver"].ToString() : this.Driver;
            this.ConnectionString = h.Contains("ConnectionString") ? h["ConnectionString"].ToString() : this.ConnectionString;

            // File Locations
            this.TemplatePath = h.Contains("TemplatePath") ? h["TemplatePath"].ToString() : this.TemplatePath;
            this.OutputPath = h.Contains("OutputPath") ? h["OutputPath"].ToString() : this.OutputPath;
            this.UIAssemblyPath = h.Contains("UIAssemblyPath") ? h["UIAssemblyPath"].ToString() : this.UIAssemblyPath;
            this.CompilerAssemblyPath = h.Contains("CompilerAssemblyPath") ? h["CompilerAssemblyPath"].ToString() : this.CompilerAssemblyPath;
            this.LanguageMappingFile = h.Contains("LanguageMappingFile") ? h["LanguageMappingFile"].ToString() : this.LanguageMappingFile;
            this.UserMetadataFile = h.Contains("UserMetadataFile") ? h["UserMetadataFile"].ToString() : this.UserMetadataFile;

            // Class Names
            this.AbstractPrefix = h.Contains("AbstractPrefix") ? h["AbstractPrefix"].ToString() : this.AbstractPrefix;
            this.EntitySuffix = h.Contains("EntitySuffix") ? h["EntitySuffix"].ToString() : this.EntitySuffix;
            this.CollectionSuffix = h.Contains("CollectionSuffix") ? h["CollectionSuffix"].ToString() : this.CollectionSuffix;
            this.QuerySuffix = h.Contains("QuerySuffix") ? h["QuerySuffix"].ToString() : this.QuerySuffix;
            this.MetadataSuffix = h.Contains("MetadataSuffix") ? h["MetadataSuffix"].ToString() : this.MetadataSuffix;
            this.ProxyStubSuffix = h.Contains("ProxyStubSuffix") ? h["ProxyStubSuffix"].ToString() : this.ProxyStubSuffix;
            this.PrefixWithSchema = h.Contains("PrefixWithSchema") ? Convert.ToBoolean(h["PrefixWithSchema"].ToString()) : this.PrefixWithSchema;

            // Stored Procedure names
            this.ProcDelete = h.Contains("ProcDelete") ? h["ProcDelete"].ToString() : this.ProcDelete;
            this.ProcInsert = h.Contains("ProcInsert") ? h["ProcInsert"].ToString() : this.ProcInsert;
            this.ProcLoadAll = h.Contains("ProcLoadAll") ? h["ProcLoadAll"].ToString() : this.ProcLoadAll;
            this.ProcLoadByPK = h.Contains("ProcLoadByPK") ? h["ProcLoadByPK"].ToString() : this.ProcLoadByPK;
            this.ProcPrefix = h.Contains("ProcPrefix") ? h["ProcPrefix"].ToString() : this.ProcPrefix;
            this.ProcSuffix = h.Contains("ProcSuffix") ? h["ProcSuffix"].ToString() : this.ProcSuffix;
            this.ProcUpdate = h.Contains("ProcUpdate") ? h["ProcUpdate"].ToString() : this.ProcUpdate;
            this.ProcVerbFirst = h.Contains("ProcVerbFirst") ? Convert.ToBoolean(h["ProcVerbFirst"].ToString()) : this.ProcVerbFirst;

            // Hierarchical Names
            this.OnePrefix = h.Contains("OneToOnePrefix") ? h["OneToOnePrefix"].ToString() : this.OnePrefix;
            this.OneSeparator = h.Contains("OneToOneSeparator") ? h["OneToOneSeparator"].ToString() : this.OneSeparator;
            this.OneSuffix = h.Contains("OneToOneSuffix") ? h["OneToOneSuffix"].ToString() : this.OneSuffix;
            this.ManyPrefix = h.Contains("ManyToManyPrefix") ? h["ManyToManyPrefix"].ToString() : this.ManyPrefix;
            this.ManySeparator = h.Contains("ManyToManySeparator") ? h["ManyToManySeparator"].ToString() : this.ManySeparator;
            this.ManySuffix = h.Contains("ManyToManySuffix") ? h["ManyToManySuffix"].ToString() : this.ManySuffix;
            this.SelfOnly = h.Contains("SelfOnly") ? Convert.ToBoolean(h["SelfOnly"].ToString()) : this.SelfOnly;
            this.SwapNames = h.Contains("SwapNames") ? Convert.ToBoolean(h["SwapNames"].ToString()) : this.SwapNames;
            this.UseAssociativeName = h.Contains("UseAssociativeName") ? Convert.ToBoolean(h["UseAssociativeName"].ToString()) : this.UseAssociativeName;
            this.UseUpToPrefix = h.Contains("UseUpToPrefix") ? Convert.ToBoolean(h["UseUpToPrefix"].ToString()) : this.UseUpToPrefix;

            // Miscellaneous
            this.PreserveUnderscores = h.Contains("PreserveUnderscores") ? Convert.ToBoolean(h["PreserveUnderscores"].ToString()) : this.UseRawNames;
            this.UseRawNames = h.Contains("UseRawNames") ? Convert.ToBoolean(h["UseRawNames"].ToString()) : this.UseRawNames;

            // Other
            this.UseNullableTypesAlways = h.Contains("UseNullableTypesAlways") ? Convert.ToBoolean(h["UseNullableTypesAlways"].ToString()) : this.UseNullableTypesAlways;
            this.TurnOffDateTimeInClassHeaders = h.Contains("TurnOffDateTimeInClassHeaders") ? Convert.ToBoolean(h["TurnOffDateTimeInClassHeaders"].ToString()) : this.TurnOffDateTimeInClassHeaders;
            this.DefaultTemplateDoubleClickAction = h.Contains("DefaultTemplateDoubleClickAction") ? h["DefaultTemplateDoubleClickAction"].ToString() : this.DefaultTemplateDoubleClickAction;

            // License 
            this.LicenseProxyEnable = h.Contains("LicenseProxyEnable") ? Convert.ToBoolean(h["LicenseProxyEnable"].ToString()) : this.LicenseProxyEnable;
            this.LicenseProxyUrl = h.Contains("LicenseProxyUrl") ? h["LicenseProxyUrl"].ToString() : this.LicenseProxyUrl;
            this.LicenseProxyUserName = h.Contains("LicenseProxyUserName") ? h["LicenseProxyUserName"].ToString() : this.LicenseProxyUserName;
            this.LicenseProxyPassword = h.Contains("LicenseProxyPassword") ? h["LicenseProxyPassword"].ToString() : this.LicenseProxyPassword;
            this.LicenseProxyDomainName = h.Contains("LicenseProxyDomainName") ? h["LicenseProxyDomainName"].ToString() : this.LicenseProxyDomainName;

            if ((this.LicenseProxyUrl != null && this.LicenseProxyUrl.Length > 0) ||
                (this.LicenseProxyUserName != null && this.LicenseProxyUserName.Length > 0) ||
                (this.LicenseProxyPassword != null && this.LicenseProxyPassword.Length > 0) ||
                (this.LicenseProxyDomainName != null && this.LicenseProxyDomainName.Length > 0))
            {
                Licensing licensing = new Licensing();
                string id = licensing.getUniqueID("C");

                Crypto crypto = new Crypto();
                this.LicenseProxyUrl = crypto.DecryptStringAES(this.LicenseProxyUrl, id);
                this.LicenseProxyUserName = crypto.DecryptStringAES(this.LicenseProxyUserName, id);
                this.LicenseProxyPassword = crypto.DecryptStringAES(this.LicenseProxyPassword, id);
                this.LicenseProxyDomainName = crypto.DecryptStringAES(this.LicenseProxyDomainName, id);
            }
        }

        public void AdjustPathsForTravelMode(Module executingAssembly)
        {
            FileInfo info = new FileInfo(executingAssembly.FullyQualifiedName);

            int index = info.DirectoryName.IndexOf("CodeGeneration") + 14;

            string basePath = info.DirectoryName.Substring(0, index);

            this.CompilerAssemblyPath = basePath + @"\Bin\";
            this.UIAssemblyPath = basePath + @"\Bin\UIAddIns\";
            this.TemplatePath = basePath + @"\Templates\";
            this.LanguageMappingFile = basePath + @"\esLanguages.xml";
        }

        /// <summary>
        /// Saves the user's default settings.
        /// </summary>
        public void Save()
        {
            string pathAndFileName = string.Empty;

            //Module executingAssembly = System.Reflection.Assembly.GetEntryAssembly().ManifestModule;
            //string exe = executingAssembly.Name;

            ////--------------------------------------------------------
            //// Memory Stick override
            ////--------------------------------------------------------
            //if (exe.Equals("EntitySpaces.exe", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    FileInfo info = new FileInfo(executingAssembly.FullyQualifiedName);
            //    pathAndFileName = info.DirectoryName + @"\esSettings.xml";

            //    if (!File.Exists(pathAndFileName))
            //    {
            //        pathAndFileName = String.Empty;
            //    }
            //}

            if (pathAndFileName == String.Empty)
            {
                pathAndFileName = this.AppDataPath + @"\esSettings.xml";
            }

            XmlTextWriter xwriter = new XmlTextWriter(pathAndFileName, System.Text.Encoding.UTF8);
            xwriter.Formatting = Formatting.Indented;
            xwriter.Indentation = 4;

            xwriter.WriteStartDocument();
            this.Save(xwriter);
            xwriter.WriteEndDocument();

            //Flush the xml document to the underlying stream and
            //close the underlying stream. The data will not be
            //written out to the stream until either the Flush()
            //method is called or the Close() method is called.
            xwriter.Flush();
            xwriter.Close();
        }

        /// <summary>
        /// Saves the user's default settings to the given filename
        /// </summary>
        public void Save(string pathAndFileName)
        {
            XmlTextWriter xwriter = new XmlTextWriter(pathAndFileName, System.Text.Encoding.UTF8);
            xwriter.Formatting = Formatting.Indented;
            xwriter.Indentation = 4;

            xwriter.WriteStartDocument();
            this.Save(xwriter);
            xwriter.WriteEndDocument();

            //Flush the xml document to the underlying stream and
            //close the underlying stream. The data will not be
            //written out to the stream until either the Flush()
            //method is called or the Close() method is called.
            xwriter.Flush();
            xwriter.Close();
        }

        /// <summary>
        /// This can be called to embed the settings in an existing XmlDocument
        /// </summary>
        /// <param name="xwriter"></param>
        public void Save(XmlTextWriter xwriter)
        {
            xwriter.WriteStartElement("esSettings");
            xwriter.WriteAttributeString("Name", "esCustom");

            // Connection
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "Driver");
            xwriter.WriteAttributeString("Value", this.Driver);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ConnectionString");
            xwriter.WriteAttributeString("Value", this.ConnectionString);
            xwriter.WriteEndElement();

            // File Locations
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "TemplatePath");
            xwriter.WriteAttributeString("Value", this.TemplatePath);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "OutputPath");
            xwriter.WriteAttributeString("Value", this.OutputPath);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "UIAssemblyPath");
            xwriter.WriteAttributeString("Value", this.UIAssemblyPath);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "CompilerAssemblyPath");
            xwriter.WriteAttributeString("Value", this.CompilerAssemblyPath);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "LanguageMappingFile");
            xwriter.WriteAttributeString("Value", this.LanguageMappingFile);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "UserMetadataFile");
            xwriter.WriteAttributeString("Value", this.UserMetadataFile);
            xwriter.WriteEndElement();

            // Class Names
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "AbstractPrefix");
            xwriter.WriteAttributeString("Value", this.AbstractPrefix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "EntitySuffix");
            xwriter.WriteAttributeString("Value", this.EntitySuffix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "CollectionSuffix");
            xwriter.WriteAttributeString("Value", this.CollectionSuffix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "QuerySuffix");
            xwriter.WriteAttributeString("Value", this.QuerySuffix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "MetadataSuffix");
            xwriter.WriteAttributeString("Value", this.MetadataSuffix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProxyStubSuffix");
            xwriter.WriteAttributeString("Value", this.ProxyStubSuffix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "PrefixWithSchema");
            xwriter.WriteAttributeString("Value", this.PrefixWithSchema.ToString());
            xwriter.WriteEndElement();

            // Stored Procedure Names
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProcPrefix");
            xwriter.WriteAttributeString("Value", this.ProcPrefix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProcSuffix");
            xwriter.WriteAttributeString("Value", this.ProcSuffix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProcInsert");
            xwriter.WriteAttributeString("Value", this.ProcInsert);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProcUpdate");
            xwriter.WriteAttributeString("Value", this.ProcUpdate);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProcDelete");
            xwriter.WriteAttributeString("Value", this.ProcDelete);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProcLoadAll");
            xwriter.WriteAttributeString("Value", this.ProcLoadAll);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProcLoadByPK");
            xwriter.WriteAttributeString("Value", this.ProcLoadByPK);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ProcVerbFirst");
            xwriter.WriteAttributeString("Value", this.ProcVerbFirst.ToString());
            xwriter.WriteEndElement();

            // Hierarchical Names
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "OneToOnePrefix");
            xwriter.WriteAttributeString("Value", this.OnePrefix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "OneToOneSeparator");
            xwriter.WriteAttributeString("Value", this.OneSeparator);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "OneToOneSuffix");
            xwriter.WriteAttributeString("Value", this.OneSuffix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ManyToManyPrefix");
            xwriter.WriteAttributeString("Value", this.ManyPrefix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ManyToManySeparator");
            xwriter.WriteAttributeString("Value", this.ManySeparator);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "ManyToManySuffix");
            xwriter.WriteAttributeString("Value", this.ManySuffix);
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "SelfOnly");
            xwriter.WriteAttributeString("Value", this.SelfOnly.ToString());
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "SwapNames");
            xwriter.WriteAttributeString("Value", this.SwapNames.ToString());
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "UseAssociativeName");
            xwriter.WriteAttributeString("Value", this.UseAssociativeName.ToString());
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "UseUpToPrefix");
            xwriter.WriteAttributeString("Value", this.UseUpToPrefix.ToString());
            xwriter.WriteEndElement();

            // Miscellaneous
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "PreserveUnderscores");
            xwriter.WriteAttributeString("Value", this.PreserveUnderscores.ToString());
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "UseRawNames");
            xwriter.WriteAttributeString("Value", this.UseRawNames.ToString());
            xwriter.WriteEndElement();

            // Other
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "UseNullableTypesAlways");
            xwriter.WriteAttributeString("Value", this.UseNullableTypesAlways.ToString());
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "TurnOffDateTimeInClassHeaders");
            xwriter.WriteAttributeString("Value", this.TurnOffDateTimeInClassHeaders.ToString());
            xwriter.WriteEndElement();
            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "DefaultTemplateDoubleClickAction");
            xwriter.WriteAttributeString("Value", this.DefaultTemplateDoubleClickAction.ToString());
            xwriter.WriteEndElement();

            if ((this.LicenseProxyUrl != null && this.LicenseProxyUrl.Length > 0) ||
                (this.LicenseProxyUserName != null && this.LicenseProxyUserName.Length > 0) ||
                (this.LicenseProxyPassword != null && this.LicenseProxyPassword.Length > 0) ||
                (this.LicenseProxyDomainName != null && this.LicenseProxyDomainName.Length > 0))
            {
                Licensing licensing = new Licensing();
                string id = licensing.getUniqueID("C");

                Crypto crypto = new Crypto();
                this.LicenseProxyUrl = crypto.EncryptStringAES(this.LicenseProxyUrl, id);
                this.LicenseProxyUserName = crypto.EncryptStringAES(this.LicenseProxyUserName, id);
                this.LicenseProxyPassword = crypto.EncryptStringAES(this.LicenseProxyPassword, id);
                this.LicenseProxyDomainName = crypto.EncryptStringAES(this.LicenseProxyDomainName, id);
            }

            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "LicenseProxyEnable");
            xwriter.WriteAttributeString("Value", this.LicenseProxyEnable.ToString());
            xwriter.WriteEndElement();

            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "LicenseProxyUrl");
            xwriter.WriteAttributeString("Value", this.LicenseProxyUrl.ToString());
            xwriter.WriteEndElement();

            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "LicenseProxyUserName");
            xwriter.WriteAttributeString("Value", this.LicenseProxyUserName.ToString());
            xwriter.WriteEndElement();

            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "LicenseProxyPassword");
            xwriter.WriteAttributeString("Value", this.LicenseProxyPassword.ToString());
            xwriter.WriteEndElement();

            xwriter.WriteStartElement("esSetting");
            xwriter.WriteAttributeString("Name", "LicenseProxyDomainName");
            xwriter.WriteAttributeString("Value", this.LicenseProxyDomainName.ToString());
            xwriter.WriteEndElement();

            xwriter.WriteEndElement(); //End the "esSettings" element.
        }

        public void SetDefaultSettings()
        {
            // Connection
            this.Driver = "SQL";
            this.ConnectionString = GetDefaultConnectionString(this.Driver);

            // File Locations
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2010", false);
            if (key != null)
            {
                string basePath = (string)key.GetValue("Install_Dir");

                if (!basePath.EndsWith(@"\"))
                {
                    basePath += @"\";
                }

                this.TemplatePath = basePath + @"CodeGeneration\Templates\";
                this.UIAssemblyPath = basePath + @"CodeGeneration\Bin\UIAddIns\";
                this.CompilerAssemblyPath = basePath + @"CodeGeneration\Bin\";
                this.LanguageMappingFile = basePath + @"CodeGeneration\esLanguages.xml";
            }
            else
            {
                //=========================================================================================
                // Ultimately this branch of the "if" statement will never be used as there will always be
                // a registry setting
                //=========================================================================================

                this.TemplatePath = @"C:\svn\architecture\ES2010\CodeGeneration\Templates";
                this.UIAssemblyPath = @"C:\SVN\architecture\ES2010\CodeGeneration\ClassLibraries\EntitySpaces.TemplateUI\bin\x86\Debug";
                this.CompilerAssemblyPath = @"C:\SVN\architecture\ES2010\CodeGeneration\StandAlone\bin\x86\Debug";
                this.LanguageMappingFile = @"C:\svn\architecture\ES2010\CodeGeneration\esLanguages.xml";
            }

            this.OutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.UserMetadataFile = this.AppDataPath + @"\esUserData.xml";

            // Class Names
            this.AbstractPrefix = "es";
            this.EntitySuffix = "";
            this.CollectionSuffix = "Collection";
            this.QuerySuffix = "Query";
            this.MetadataSuffix = "Metadata";
            this.ProxyStubSuffix = "ProxyStub";
            this.PrefixWithSchema = false;

            // Stored Procedure Names
            this.ProcPrefix = "proc_";
            this.ProcInsert = "Insert";
            this.ProcUpdate = "Update";
            this.ProcDelete = "Delete";
            this.ProcLoadAll = "LoadAll";
            this.ProcLoadByPK = "LoadByPrimaryKey";
            this.ProcSuffix = "";
            this.ProcVerbFirst = false;

            // Hierarchical Names
            this.OnePrefix = "";
            this.OneSeparator = "By";
            this.OneSuffix = "";
            this.ManyPrefix = "";
            this.ManySeparator = "By";
            this.ManySuffix = "Collection";
            this.SelfOnly = false;
            this.SwapNames = false;
            this.UseAssociativeName = false;
            this.UseUpToPrefix = true;

            // Miscellaneous
            this.PreserveUnderscores = false;
            this.UseRawNames = false;

            // Licensing
            this.LicenseProxyUrl = string.Empty;
            this.LicenseProxyUserName = string.Empty;
            this.LicenseProxyPassword = string.Empty;
            this.LicenseProxyDomainName = string.Empty;

            // Other
            this.UseNullableTypesAlways = true;
            this.TurnOffDateTimeInClassHeaders = false;
            this.DefaultTemplateDoubleClickAction = "Execute";
        }

        public esSettings Clone()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();

            formatter.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            esSettings clone = (esSettings)formatter.Deserialize(stream);
            stream.Flush();
            stream.Close();

            return clone;
        }

        public string GetDefaultConnectionString(string driver)
        {
            if (String.IsNullOrEmpty(driver))
            {
                return "";
            }

            switch (driver.ToUpper())
            {
                case "SQL":
                    return @"Provider=SQLOLEDB.1;Password=;User ID=sa;Persist Security Info=True;Initial Catalog=Northwind;Data Source=localhost";

                case "SQLAZURE":
                    return @"Server=tcp:server.database.windows.net;Database=SomeDatabase;User ID=Groovey@cool;Password=pass;Trusted_Connection=False;Encrypt=True;";

                case "ORACLE":
                    return @"Provider=OraOLEDB.Oracle.1;Password=Password;Persist Security Info=True;User ID=UserID;Data Source=DataSource";

                case "ACCESS":
                    return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=northwind.mdb;Persist Security Info=True;User Id=;Password=";

                case "MYSQL":
                    return @"Database=test;Data Source=localhost;User Id=anonymous;Password=;";

                case "POSTGRESQL":
                    return @"Server=127.0.0.1;Port=5432;User Id=postgres;Password=;Database=MyDatabase;";

                case "VISTADB":
                    return @"Data Source=C:\Program Files\VistaDB 3\Data\Northwind.vdb3;Cypher= None;Password=;Exclusive=False;Readonly=False;";

                case "VISTADB4":
                    return @"Data Source=C:\Program Files\VistaDB 4.0\Data\Northwind.vdb4;Open Mode=NonexclusiveReadWrite;";

                case "SQLCE":
                    return "Data Source=C:\\SomeDatabase.sdf;Version=\"3.5.1.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91\";";

                case "SQLITE":
                    return @"Data Source=C:\SQLiteAdmin\Northwind.db3;Version=3;";

                case "SYBASE":
                    return @"ENG=demo11;UID=dba;PWD=sql;DBN=demo";

                case "EFFIPROZDB":
                    return "Connection Type=File; Initial Catalog=C:\\effiproz\\AdventureWorks;User=sa;Password=;Version=\"1.4.3926.39651, Culture=neutral, PublicKeyToken=9c147f7358eea142\";";
                //return @"Connection Type=File; Initial Catalog=C:\effiproz\AdventureWorks;User=sa;Password=;";

                default:
                    return "";
            }
        }

        public esSettings To2011()
        {
            esSettings settings = new esSettings();

            settings.AbstractPrefix = this.AbstractPrefix;
            settings.CollectionSuffix = this.CollectionSuffix;
            settings.CompilerAssemblyPath = this.CompilerAssemblyPath;
            settings.ConnectionString = this.ConnectionString;
            settings.DefaultTemplateDoubleClickAction = this.DefaultTemplateDoubleClickAction;
            settings.Driver = this.Driver;
            settings.EntitySuffix = this.EntitySuffix;
//          settings.InstallPath { get; }
            settings.LanguageMappingFile = this.LanguageMappingFile;
            settings.LicenseProxyDomainName = this.LicenseProxyDomainName;
            settings.LicenseProxyEnable = this.LicenseProxyEnable;
            settings.LicenseProxyPassword = this.LicenseProxyPassword;
            settings.LicenseProxyUrl = this.LicenseProxyUrl;
            settings.LicenseProxyUserName = this.LicenseProxyUserName;
            settings.ManyPrefix = this.ManyPrefix;
            settings.ManySeparator = this.ManySeparator;
            settings.ManySuffix = this.ManySuffix;
            settings.MetadataSuffix = this.MetadataSuffix;
            settings.OnePrefix = this.OnePrefix;
            settings.OneSeparator = this.OneSeparator;
            settings.OneSuffix = this.OneSuffix;
            settings.OutputPath = this.OutputPath;
            settings.PrefixWithSchema = this.PrefixWithSchema;
            settings.PreserveUnderscores = this.PreserveUnderscores;
            settings.ProcDelete = this.ProcDelete;
            settings.ProcInsert = this.ProcInsert;
            settings.ProcLoadAll = this.ProcLoadAll;
            settings.ProcLoadByPK = this.ProcLoadByPK;
            settings.ProcPrefix = this.ProcPrefix;
            settings.ProcSuffix = this.ProcSuffix;
            settings.ProcUpdate = this.ProcUpdate;
            settings.ProcVerbFirst = this.ProcVerbFirst;
            settings.ProxyStubSuffix = this.ProxyStubSuffix;
            settings.QuerySuffix = this.QuerySuffix;
            settings.SelfOnly = this.SelfOnly;
            settings.SwapNames = this.SwapNames;
            settings.TemplatePath = this.TemplatePath;
            settings.TurnOffDateTimeInClassHeaders = this.TurnOffDateTimeInClassHeaders;
            settings.UIAssemblyPath = this.UIAssemblyPath;
            settings.UseAssociativeName = this.UseAssociativeName;
            settings.UseNullableTypesAlways = this.UseNullableTypesAlways;
            settings.UseRawNames = this.UseRawNames;
            settings.UserMetadataFile = this.UserMetadataFile;
            settings.UseUpToPrefix = this.UseUpToPrefix;

            return settings;
        }
    }
}
