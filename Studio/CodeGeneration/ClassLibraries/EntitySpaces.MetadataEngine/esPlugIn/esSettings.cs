using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Win32;

namespace EntitySpaces.MetadataEngine
{
    [Serializable]
    public class esSettings : ISettings
    {
        public esSettings()
        {
            // this.SetDefaultSettings();
        }

        #region Properties

        #region Connection

        private string _driver;
        //[XmlElement(Order=1)]
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
        //[XmlElement(Order = 2)]
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        #endregion

        #region File Locations


        static public string AppDataPath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                path += @"\EntitySpaces\ES2019";
                return path;
            }
        }

        static public string ES2010AppDataPath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                path += @"\EntitySpaces\ES2010";
                return path;
            }
        }

        static public string TemplateCachePath
        {
            get
            {
                string path = AppDataPath;
                path += @"\TemplateCache";
                return path;
            }
        }

        //[XmlElement(Order = 3)]
        public string InstallPath
        {
            get
            {
                string path = @"C:\Program Files\EntitySpaces 2019\";

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", false);
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
        //[XmlElement(Order = 4)]
        public string TemplatePath
        {
            get { return _templatePath; }
            set { _templatePath = value; }
        }

        private string _outputPath;
        //[XmlElement(Order = 5)]
        public string OutputPath
        {
            get { return _outputPath; }
            set { _outputPath = value; }
        }

        private string _uiAssemblyPath;
        //[XmlElement(Order = 6)]
        public string UIAssemblyPath
        {
            get { return _uiAssemblyPath; }
            set { _uiAssemblyPath = value; }
        }

        private string _compilerAssemblyPath;
        //[XmlElement(Order = 7)]
        public string CompilerAssemblyPath
        {
            get { return _compilerAssemblyPath; }
            set { _compilerAssemblyPath = value; }
        }

        private string _languageMappingFile;
        //[XmlElement(Order = 8)]
        public string LanguageMappingFile
        {
            get { return _languageMappingFile; }
            set { _languageMappingFile = value; }
        }

        private string _userMetadataFile;
        //[XmlElement(Order = 9)]
        public string UserMetadataFile
        {
            get { return _userMetadataFile; }
            set { _userMetadataFile = value; }
        }

        #endregion

        #region Class Names

        private string _abstractPrefix;
        //[XmlElement(Order = 10)]
        public string AbstractPrefix
        {
            get { return _abstractPrefix; }
            set { _abstractPrefix = value; }
        }

        private string _entitySuffix;
        //[XmlElement(Order = 11)]
        public string EntitySuffix
        {
            get { return _entitySuffix; }
            set { _entitySuffix = value; }
        }

        private string _collectionSuffix;
        //[XmlElement(Order = 12)]
        public string CollectionSuffix
        {
            get { return _collectionSuffix; }
            set { _collectionSuffix = value; }
        }

        private string _querySuffix;
        //[XmlElement(Order = 13)]
        public string QuerySuffix
        {
            get { return _querySuffix; }
            set { _querySuffix = value; }
        }

        private string _metadataSuffix;
        //[XmlElement(Order = 14)]
        public string MetadataSuffix
        {
            get { return _metadataSuffix; }
            set { _metadataSuffix = value; }
        }

        private string _proxyStubSuffix;
        //[XmlElement(Order = 15)]
        public string ProxyStubSuffix
        {
            get { return _proxyStubSuffix; }
            set { _proxyStubSuffix = value; }
        }

        private bool _prefixWithSchema;
        //[XmlElement(Order = 16)]
        public bool PrefixWithSchema
        {
            get { return _prefixWithSchema; }
            set { _prefixWithSchema = value; }
        }

        #endregion

        #region Stored Procedure Names

        private string _procPrefix;
        //[XmlElement(Order = 17)]
        public string ProcPrefix
        {
            get { return _procPrefix; }
            set { _procPrefix = value; }
        }

        private string _procInsert;
        //[XmlElement(Order = 18)]
        public string ProcInsert
        {
            get { return _procInsert; }
            set { _procInsert = value; }
        }

        private string _procUpdate;
        //[XmlElement(Order = 19)]
        public string ProcUpdate
        {
            get { return _procUpdate; }
            set { _procUpdate = value; }
        }

        private string _procDelete;
        //[XmlElement(Order = 20)]
        public string ProcDelete
        {
            get { return _procDelete; }
            set { _procDelete = value; }
        }

        private string _procLoadAll;
        //[XmlElement(Order = 21)]
        public string ProcLoadAll
        {
            get { return _procLoadAll; }
            set { _procLoadAll = value; }
        }

        private string _procLoadByPK;
        //[XmlElement(Order = 22)]
        public string ProcLoadByPK
        {
            get { return _procLoadByPK; }
            set { _procLoadByPK = value; }
        }

        private string _procSuffix;
        //[XmlElement(Order = 23)]
        public string ProcSuffix
        {
            get { return _procSuffix; }
            set { _procSuffix = value; }
        }

        private bool _procVerbFirst;
        //[XmlElement(Order = 24)]
        public bool ProcVerbFirst
        {
            get { return _procVerbFirst; }
            set { _procVerbFirst = value; }
        }

        #endregion

        #region Hierarchical Names

        private string _onePrefix;
        //[XmlElement(Order = 25)]
        public string OnePrefix
        {
            get { return _onePrefix; }
            set { _onePrefix = value; }
        }

        private string _oneSeparator;
        //[XmlElement(Order = 26)]
        public string OneSeparator
        {
            get { return _oneSeparator; }
            set { _oneSeparator = value; }
        }

        private string _oneSuffix;
        //[XmlElement(Order = 27)]
        public string OneSuffix
        {
            get { return _oneSuffix; }
            set { _oneSuffix = value; }
        }

        private string _manyPrefix;
        //[XmlElement(Order = 28)]
        public string ManyPrefix
        {
            get { return _manyPrefix; }
            set { _manyPrefix = value; }
        }

        private string _manySeparator;
        //[XmlElement(Order = 29)]
        public string ManySeparator
        {
            get { return _manySeparator; }
            set { _manySeparator = value; }
        }

        private string _manySuffix;
        //[XmlElement(Order = 30)]
        public string ManySuffix
        {
            get { return _manySuffix; }
            set { _manySuffix = value; }
        }

        private bool _selfOnly;
        //[XmlElement(Order = 31)]
        public bool SelfOnly
        {
            get { return _selfOnly; }
            set { _selfOnly = value; }
        }

        private bool _swapNames;
        //[XmlElement(Order = 32)]
        public bool SwapNames
        {
            get { return _swapNames; }
            set { _swapNames = value; }
        }

        private bool _useAssociativeName;
        //[XmlElement(Order = 33)]
        public bool UseAssociativeName
        {
            get { return _useAssociativeName; }
            set { _useAssociativeName = value; }
        }

        private bool _useUpToPrefix;
        //[XmlElement(Order = 34)]
        public bool UseUpToPrefix
        {
            get { return _useUpToPrefix; }
            set { _useUpToPrefix = value; }
        }

        #endregion

        #region Miscellaneous

        private bool _preserveUnderscores;
        //[XmlElement(Order = 35)]
        public bool PreserveUnderscores
        {
            get { return _preserveUnderscores; }
            set { _preserveUnderscores = value; }
        }

        private bool _useRawNames;
        //[XmlElement(Order = 36)]
        public bool UseRawNames
        {
            get { return _useRawNames; }
            set { _useRawNames = value; }
        }

        #endregion

        #region Other

        private bool _useNullableTypesAlways;
        //[XmlElement(Order = 37)]
        public bool UseNullableTypesAlways
        {
            get { return _useNullableTypesAlways; }
            set { _useNullableTypesAlways = value; }
        }

        private bool _turnOffDateTimeInClassHeaders;
        //[XmlElement(Order = 38)]
        public bool TurnOffDateTimeInClassHeaders
        {
            get { return _turnOffDateTimeInClassHeaders; }
            set { _turnOffDateTimeInClassHeaders = value; }
        }

        private string _defaultTemplateDoubleClickAction;
        //[XmlElement(Order = 39)]
        public string DefaultTemplateDoubleClickAction
        {
            get { return _defaultTemplateDoubleClickAction; }
            set { _defaultTemplateDoubleClickAction = value; }
        }

        #endregion

        #region License

        private bool _licenseProxyEnable;
        //[XmlElement(Order = 40)]
        public bool LicenseProxyEnable
        {
            get { return _licenseProxyEnable; }
            set { _licenseProxyEnable = value; }
        }

        private string _licenseProxyUrl;
        //[XmlElement(Order = 41)]
        public string LicenseProxyUrl
        {
            get { return _licenseProxyUrl; }
            set { _licenseProxyUrl = value; }
        }

        private string _licenseProxyUserName;
        //[XmlElement(Order = 42)]
        public string LicenseProxyUserName
        {
            get { return _licenseProxyUserName; }
            set { _licenseProxyUserName = value; }
        }

        private string _licenseProxyPassword;
        //[XmlElement(Order = 43)]
        public string LicenseProxyPassword
        {
            get { return _licenseProxyPassword; }
            set { _licenseProxyPassword = value; }
        }

        private string _licenseProxyDomainName;
        //[XmlElement(Order = 44)]
        public string LicenseProxyDomainName
        {
            get { return _licenseProxyDomainName; }
            set { _licenseProxyDomainName = value; }
        }

        #endregion

        #region Driver Settings

        private List<esSettingsDriverInfo> driverInfoCollection;
        //[XmlElement(Order = 45)]
        public List<esSettingsDriverInfo> DriverInfoCollection
        {
            get
            {
                if (driverInfoCollection == null)
                {
                    driverInfoCollection = new List<esSettingsDriverInfo>();
                }

                return driverInfoCollection;
            }

            set
            {
                driverInfoCollection = value;
            }
        }

        public esSettingsDriverInfo FindDriverInfoCollection(string driver)
        {
            esSettingsDriverInfo info = null;

            foreach (esSettingsDriverInfo driverInfo in DriverInfoCollection)
            {
                if (driverInfo.Driver == driver)
                {
                    info = driverInfo;
                    break;
                }
            }

            return info;
        }

        #endregion

        #endregion

        /// <summary>
        /// Loads the user's default settings.
        /// If none exist, a file is created with the EntitySpaces default settings.
        /// </summary>
        static public esSettings Load()
        {
            esSettings settings = null;

            string pathAndFileName = AppDataPath + @"\esSettings.xml";

            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }

            if (!File.Exists(pathAndFileName))
            {
                settings = SetDefaultSettings();
                settings.Save();
            }

            using (TextReader rdr = new StreamReader(pathAndFileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(esSettings));
                settings = (esSettings)serializer.Deserialize(rdr);
                rdr.Close();
            }

            if (!Directory.Exists(TemplateCachePath))
                Directory.CreateDirectory(TemplateCachePath);
            
            if (settings.DriverInfoCollection == null || settings.driverInfoCollection.Count == 0)
            {
                settings.DriverInfoCollection.Add(new esSettingsDriverInfo() { Driver = settings.Driver, ConnectionString = settings.ConnectionString });
            }

            return settings;
        }

        static public esSettings Load(string pathAndFileName)
        {
            esSettings settings = null;

            string version = GetFileVersion(pathAndFileName);

            if (version.Substring(0, 4) != "2011" && version.Substring(0, 4) != "2012")
            {
                // Convert the old project file in place
                ConvertSettings(pathAndFileName);
            }

            using (TextReader rdr = new StreamReader(pathAndFileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(esSettings));
                settings = (esSettings)serializer.Deserialize(rdr);
                rdr.Close();
            }

            if (!Directory.Exists(TemplateCachePath))
                Directory.CreateDirectory(TemplateCachePath);

            if (settings.DriverInfoCollection == null || settings.driverInfoCollection.Count == 0)
            {
                settings.DriverInfoCollection.Add(new esSettingsDriverInfo() { Driver = settings.Driver, ConnectionString = settings.ConnectionString });
            }

            return settings;
        }

        static public esSettings Load(XmlReader reader)
        {
            esSettings settings = null;

            XmlSerializer serializer = new XmlSerializer(typeof(esSettings));
            settings = (esSettings)serializer.Deserialize(reader);

            if (settings.DriverInfoCollection == null || settings.driverInfoCollection.Count == 0)
            {
                settings.DriverInfoCollection.Add(new esSettingsDriverInfo() { Driver = settings.Driver, ConnectionString = settings.ConnectionString });
            }

            return settings;
        }

        static private string GetFileVersion(string fileNameAndFilePath)
        {
            string version = "0000.0.0000.0";

            try
            {
                using (XmlTextReader reader = new XmlTextReader(fileNameAndFilePath))
                {
                    reader.WhitespaceHandling = WhitespaceHandling.None;

                    reader.Read();
                    reader.Read();

                    version = reader[0];
                }
            }
            catch { }

            return version;
        }

        private static void ConvertSettings(string fileNameAndFilePath)
        {
            esSettings2010 settings2010 = new esSettings2010();
            settings2010.Load(fileNameAndFilePath);

            esSettings settings = settings2010.To2011();

            esSettingsDriverInfo info = new esSettingsDriverInfo();
            info.ConnectionString = settings2010.ConnectionString;
            info.Driver = settings2010.Driver;

            settings.DriverInfoCollection.Add(info);

            try
            {
                AdjustPathsBasedOnPriorVersions(settings, @"Software\EntitySpaces 2009", "ES2009", false);
                AdjustPathsBasedOnPriorVersions(settings, @"Software\EntitySpaces 2010", "ES2010", false);
                AdjustPathsBasedOnPriorVersions(settings, @"Software\EntitySpaces 2011", "ES2011", false);
                AdjustPathsBasedOnPriorVersions(settings, @"Software\EntitySpaces 2011", "ES2012", false);
                AdjustPathsBasedOnPriorVersions(settings, @"Software\EntitySpaces 2012", "ES2019", true);
            }
            catch { }

            FileInfo fileInfo = new FileInfo(fileNameAndFilePath);

            string backup = fileInfo.Name.Replace(fileInfo.Extension, "");
            backup += "_original" + fileInfo.Extension;
            backup = fileInfo.DirectoryName + "\\" + backup;

            File.Copy(fileNameAndFilePath, backup, true);

            settings.Save(fileNameAndFilePath);
        }

        public static void AdjustPathsBasedOnPriorVersions(esSettings settings, string registry, string version, bool currentVersion)
        {
            // File Locations
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registry, false);
            if (key != null)
            {
                string basePath = (string)key.GetValue("Install_Dir");

                if (!basePath.EndsWith(@"\"))
                {
                    basePath += @"\";
                }

                if (!currentVersion)
                {
                    if (settings.TemplatePath == basePath + @"CodeGeneration\Templates\")
                    {
                        settings.TemplatePath = string.Empty;
                    }

                    if (settings.UIAssemblyPath == basePath + @"CodeGeneration\Bin\UIAddIns\")
                    {
                        settings.UIAssemblyPath = string.Empty;
                    }

                    if (settings.CompilerAssemblyPath == basePath + @"CodeGeneration\Bin\")
                    {
                        settings.CompilerAssemblyPath = string.Empty;
                    }

                    if (settings.LanguageMappingFile == basePath + @"CodeGeneration\esLanguages.xml")
                    {
                        settings.LanguageMappingFile = string.Empty;
                    }

                    string appPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\EntitySpaces\" + version;

                    if (settings.UserMetadataFile == appPath + @"\esUserData.xml")
                    {
                        settings.UserMetadataFile = string.Empty;
                    }
                }
                else
                {
                    if (settings.TemplatePath == string.Empty)
                    {
                        settings.TemplatePath = basePath + @"CodeGeneration\Templates\";
                    }

                    if (settings.UIAssemblyPath == string.Empty)
                    {
                        settings.UIAssemblyPath = basePath + @"CodeGeneration\Bin\UIAddIns\";
                    }

                    if (settings.CompilerAssemblyPath == string.Empty)
                    {
                        settings.CompilerAssemblyPath = basePath + @"CodeGeneration\Bin\";
                    }

                    if (settings.LanguageMappingFile == string.Empty)
                    {
                        settings.LanguageMappingFile = basePath + @"CodeGeneration\esLanguages.xml";
                    }

                    if (settings.UserMetadataFile == string.Empty)
                    {
                        settings.UserMetadataFile = AppDataPath + @"\esUserData.xml";
                    }
                }
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

            if (pathAndFileName == String.Empty)
            {
                pathAndFileName = AppDataPath + @"\esSettings.xml";
            }

            string xml = String.Empty;

            DiscardUnverifiedConnections();

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlSerializer serializer = new XmlSerializer(typeof(esSettings));
                serializer.Serialize(stream, this, ns);

                xml = ASCIIEncoding.UTF8.GetString(stream.ToArray());
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlAttribute attr = doc.CreateAttribute("Version");
            attr.Value = "2019.1.0725.0";

            doc.DocumentElement.Attributes.Append(attr);
            doc.Save(pathAndFileName);
        }

        /// <summary>
        /// Saves the user's default settings to the given filename
        /// </summary>
        public void Save(string pathAndFileName)
        {
            string xml = String.Empty;

            DiscardUnverifiedConnections();

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlSerializer serializer = new XmlSerializer(typeof(esSettings));
                serializer.Serialize(stream, this, ns);

                xml = ASCIIEncoding.UTF8.GetString(stream.ToArray());
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlAttribute attr = doc.CreateAttribute("Version");
            attr.Value = "2019.1.0725.0";

            doc.DocumentElement.Attributes.Append(attr);
            doc.Save(pathAndFileName);
        }

        /// <summary>
        /// This can be called to embed the settings in an existing XmlDocument
        /// </summary>
        /// <param name="xwriter"></param>
        public void Save(XmlTextWriter xwriter)
        {
            // Save a copy off
            List<esSettingsDriverInfo> driverCollection = DriverInfoCollection;

            try
            {
                // We only want the connection for this settings file 
                DriverInfoCollection = new List<esSettingsDriverInfo>();

                if (driverCollection != null && driverCollection.Count > 0)
                {
                    foreach (esSettingsDriverInfo info in driverCollection)
                    {
                        if (info.Driver == Driver)
                        {
                            DriverInfoCollection.Add(info);
                            break;
                        }
                    }
                }
                else
                {
                    esSettingsDriverInfo info = new esSettingsDriverInfo();
                    info.ConnectionString = ConnectionString;
                    info.Driver = Driver;

                    DriverInfoCollection.Add(info);
                }

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlSerializer serializer = new XmlSerializer(typeof(esSettings));
                serializer.Serialize(xwriter, this, ns);
            }
            finally
            {
                DriverInfoCollection = driverCollection;
            }
        }

        private void DiscardUnverifiedConnections()
        {
            List<esSettingsDriverInfo> goodConnections  = new List<esSettingsDriverInfo>();

            if (DriverInfoCollection != null && DriverInfoCollection.Count > 0)
            {
                foreach (esSettingsDriverInfo info in DriverInfoCollection)
                {
                    if (info.HasConnected)
                    {
                        goodConnections.Add(info);
                    }
                }
            }

            DriverInfoCollection = goodConnections;
        }

        static public esSettings SetDefaultSettings()
        {
            esSettings settings = new esSettings();

            // Connection
            settings.Driver = "SQL";
            settings.ConnectionString = GetDefaultConnectionString(settings.Driver);

            // File Locations
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", false);
            if (key != null)
            {
                string basePath = (string)key.GetValue("Install_Dir");

                if (!basePath.EndsWith(@"\"))
                {
                    basePath += @"\";
                }

                settings.TemplatePath = basePath + @"CodeGeneration\Templates\";     
                settings.UIAssemblyPath = basePath + @"CodeGeneration\Bin\UIAddIns\";
                settings.CompilerAssemblyPath = basePath + @"CodeGeneration\Bin\";
                settings.LanguageMappingFile = basePath + @"CodeGeneration\esLanguages.xml";
            }
            else
            {
                //=========================================================================================
                // Ultimately this branch of the "if" statement will never be used as there will always be
                // a registry setting
                //=========================================================================================

                settings.TemplatePath = @"C:\svn\architecture\ES2019\CodeGeneration\Templates";
                settings.UIAssemblyPath = @"C:\SVN\architecture\ES2019\CodeGeneration\ClassLibraries\EntitySpaces.TemplateUI\bin\x86\Debug";
                settings.CompilerAssemblyPath = @"C:\SVN\architecture\ES2019\CodeGeneration\StandAlone\bin\x86\Debug";
                settings.LanguageMappingFile = @"C:\svn\architecture\ES2019\CodeGeneration\esLanguages.xml";
            }

            settings.OutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            settings.UserMetadataFile = AppDataPath + @"\esUserData.xml";

            // Class Names
            settings.AbstractPrefix = "es";
            settings.EntitySuffix = "";
            settings.CollectionSuffix = "Collection";
            settings.QuerySuffix = "Query";
            settings.MetadataSuffix = "Metadata";
            settings.ProxyStubSuffix = "ProxyStub";
            settings.PrefixWithSchema = false;

            // Stored Procedure Names
            settings.ProcPrefix = "proc_";
            settings.ProcInsert = "Insert";
            settings.ProcUpdate = "Update";
            settings.ProcDelete = "Delete";
            settings.ProcLoadAll = "LoadAll";
            settings.ProcLoadByPK = "LoadByPrimaryKey";
            settings.ProcSuffix = "";
            settings.ProcVerbFirst = false;

            // Hierarchical Names
            settings.OnePrefix = "";
            settings.OneSeparator = "By";
            settings.OneSuffix = "";
            settings.ManyPrefix = "";
            settings.ManySeparator = "By";
            settings.ManySuffix = "Collection";
            settings.SelfOnly = false;
            settings.SwapNames = false;
            settings.UseAssociativeName = false;
            settings.UseUpToPrefix = true;

            // Miscellaneous
            settings.PreserveUnderscores = false;
            settings.UseRawNames = false;

            // Licensing
            settings.LicenseProxyUrl = string.Empty;
            settings.LicenseProxyUserName = string.Empty;
            settings.LicenseProxyPassword = string.Empty;
            settings.LicenseProxyDomainName = string.Empty;

            // Other
            settings.UseNullableTypesAlways = true;
            settings.TurnOffDateTimeInClassHeaders = false;
            settings.DefaultTemplateDoubleClickAction = "Execute";

            return settings;
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

        static public string GetDefaultConnectionString(string driver)
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
    }

    [Serializable]
    //[XmlRoot(ElementName="DriverInfo")]
    public class esSettingsDriverInfo
    {
        //[XmlElement(Order = 1)]
        public string Driver;
        //[XmlElement(Order = 2)]
        public string ConnectionString;

        //[XmlElement(Order = 3)]
        public SpecialDate DateAdded;
        //[XmlElement(Order = 4)]
        public SpecialDate DateModified;
        //[XmlElement(Order = 5)]
        public AuditingInfo AddedBy;
        //[XmlElement(Order = 6)]
        public AuditingInfo ModifiedBy;

        //[XmlElement(Order = 7)]
        public bool ConcurrencyColumnEnabled;
        //[XmlElement(Order = 8)]
        public string ConcurrencyColumn;

        [Serializable]
        public struct SpecialDate
        {
            //[XmlElement(Order = 1)]
            public bool IsEnabled;
            //[XmlElement(Order = 2)]
            public string ColumnName;
            //[XmlElement(Order = 3)]
            public DateType Type;
            //[XmlElement(Order = 4)]
            public ClientType ClientType;
            //[XmlElement(Order = 5)]
            public string ServerSideText;
        }

        [Serializable]
        public struct AuditingInfo
        {
            //[XmlElement(Order = 1)]
            public bool IsEnabled;
            //[XmlElement(Order = 2)]
            public bool UseEventHandler;
            //[XmlElement(Order = 3)]
            public string ColumnName;
            //[XmlElement(Order = 4)]
            public string ServerSideText;
        }

        public enum DateType
        {
            Unassigned = 0,
            ClientSide = 1,
            ServerSide = 2
        };

        public enum ClientType
        {
            Unassigned = 0,
            Now = 1,
            UtcNow = 2
        };

        [XmlIgnore]
        public bool HasConnected = true;
     }
}
