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
    public interface ISettings
    {
        void AdjustPathsForTravelMode(Module executingAssembly);
        esSettings Clone();
        string DriverName(string driver);

        void Save();
        void Save(string pathAndFileName);
        void Save(XmlTextWriter xwriter);

        // Properties
        string AbstractPrefix { get; set; }
        string CollectionSuffix { get; set; }
        string CompilerAssemblyPath { get; set; }
        string ConnectionString { get; set; }
        string DefaultTemplateDoubleClickAction { get; set; }
        string Driver { get; set; }
        string EntitySuffix { get; set; }
        string InstallPath { get; }
        string LanguageMappingFile { get; set; }
        string LicenseProxyDomainName { get; set; }
        bool LicenseProxyEnable { get; set; }
        string LicenseProxyPassword { get; set; }
        string LicenseProxyUrl { get; set; }
        string LicenseProxyUserName { get; set; }
        string ManyPrefix { get; set; }
        string ManySeparator { get; set; }
        string ManySuffix { get; set; }
        string MetadataSuffix { get; set; }
        string OnePrefix { get; set; }
        string OneSeparator { get; set; }
        string OneSuffix { get; set; }
        string OutputPath { get; set; }
        bool PrefixWithSchema { get; set; }
        bool PreserveUnderscores { get; set; }
        string ProcDelete { get; set; }
        string ProcInsert { get; set; }
        string ProcLoadAll { get; set; }
        string ProcLoadByPK { get; set; }
        string ProcPrefix { get; set; }
        string ProcSuffix { get; set; }
        string ProcUpdate { get; set; }
        bool ProcVerbFirst { get; set; }
        string ProxyStubSuffix { get; set; }
        string QuerySuffix { get; set; }
        bool SelfOnly { get; set; }
        bool SwapNames { get; set; }
        string TemplatePath { get; set; }
        bool TurnOffDateTimeInClassHeaders { get; set; }
        string UIAssemblyPath { get; set; }
        bool UseAssociativeName { get; set; }
        bool UseNullableTypesAlways { get; set; }
        bool UseRawNames { get; set; }
        string UserMetadataFile { get; set; }
        bool UseUpToPrefix { get; set; }
    }
}
