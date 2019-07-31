using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntitySpaces;
using EntitySpaces.MetadataEngine;
using EntitySpaces.CodeGenerator;
using EntitySpaces.Common;

namespace EntitySpaces.CommandLine
{
    internal class ProjectExecuter
    {
        private esProject project = null;
        private esSettings userSettings;
        private List<Exception> errors = new List<Exception>();

        private ProjectExecuter() 
        {
       
        }

        public ProjectExecuter(string projectFile, esSettings mainSettings)
        {
            userSettings = mainSettings;
            project = new esProject();
            project.Load(projectFile, userSettings);
        }

        public List<Exception> ExecuteFromNode(string nodeName)
        {
            try
            {
                esProjectNode startNode = project.RootNode;

                if (nodeName != null && startNode.Name != nodeName)
                {
                    string[] nodes = nodeName.Split(new char[] { '\\' });

                    if (startNode.Name != nodes[0])
                    {
                        throw new Exception("Node '" + nodes[0] + "' " + "Not Found in Project File");
                    }

                    for (int i = 1; i < nodes.Length; i++)
                    {
                        esProjectNode nextNode = null;
                        string node = nodes[i];

                        foreach (esProjectNode childNode in startNode.Children)
                        {
                            if (node == childNode.Name)
                            {
                                nextNode = childNode;
                                break;
                            }
                        }

                        if (nextNode == null)
                        {
                            throw new Exception("Node '" + node + "' " + "Not Found in Project File");
                        }

                        startNode = nextNode;
                    }
                }

                ExecuteRecordedTemplates(startNode);
            }
            catch(Exception ex)
            {
                errors.Add(ex);
            }

            return errors;
        }

        private void ExecuteRecordedTemplates(esProjectNode node)
        {
            try
            {
                ExecuteRecordedTemplate(node);

                foreach (esProjectNode childNode in node.Children)
                {
                    ExecuteRecordedTemplates(childNode);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex); 
            }
        }

        private void ExecuteRecordedTemplate(esProjectNode node)
        {
            try
            {
                if (node != null && !node.IsFolder)
                {
                    Root esMeta = Create(node.Settings as esSettings);
                    esMeta.Input = node.Input;

                    Template template = new Template();
                    template.Execute(esMeta, node.Template.Header.FullFileName);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }

        internal Root Create(esSettings settings)
        {
            Root esMeta = new Root(settings);
            if (!esMeta.Connect(settings.Driver, settings.ConnectionString))
            {
                throw new Exception("Unable to Connect to Database: " + settings.Driver);
            }
            esMeta.Language = "C#";

//#if !TRIAL
//            Licensing licensing = new Licensing();
//            licensing.ReplaceMeLater("developer", esMeta.esPlugIn.esVersion, "Serial_Number", "Serial_Number2", "Interop.ADODBX.dll", GetProxySettings(userSettings));
//#else
//            Licensing licensing = new Licensing();
//            string id = licensing.getUniqueID("C");

//            int result = licensing.ValidateLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, "2019.1.0725.0", GetProxySettings(userSettings));

//            if (1 != result)
//            {
//                result = licensing.RegisterLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, "2019.1.0725.0", GetProxySettings(userSettings));
//            }

//            if (result != 1)
//            {
//                throw new Exception("Trial Expired");
//            }
//#endif

            return esMeta;
        }

        internal static ProxySettings GetProxySettings(esSettings settings)
        {
            ProxySettings proxy = new ProxySettings();
            proxy.UseProxy = settings.LicenseProxyEnable;
            if (proxy.UseProxy)
            {
                proxy.Url = settings.LicenseProxyUrl;
                proxy.UserName = settings.LicenseProxyUserName;
                proxy.Password = settings.LicenseProxyPassword;
                proxy.DomainName = settings.LicenseProxyDomainName;
            }

            return proxy;
        }
    }
}
