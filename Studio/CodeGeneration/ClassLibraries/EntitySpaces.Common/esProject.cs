using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

using EntitySpaces.CodeGenerator;
using EntitySpaces.MetadataEngine;

namespace EntitySpaces.Common
{
    public class esProject
    {
        public esProjectNode RootNode;
        public esSettings userSettings;
        private string projectFilePath = "";

        public void Load(string fileNameAndFilePath, esSettings mainSettings)
        {
            userSettings = mainSettings;

            string version = GetFileVersion(fileNameAndFilePath);

            if (version?.Length >= 4 && version.Substring(0, 4) != "0000" && string.Compare(version.Substring(0, 4), "2011") < 0)
            {
                // Convert the old project file in place
                ConvertProject(fileNameAndFilePath, mainSettings);
            }

            RootNode = null;

            Dictionary<int, esProjectNode> parents = new Dictionary<int, esProjectNode>();

            using (XmlTextReader reader = new XmlTextReader(fileNameAndFilePath))
            {
                projectFilePath = fileNameAndFilePath;
                reader.WhitespaceHandling = WhitespaceHandling.None;

                esProjectNode currentNode = null;

                reader.Read();
                reader.Read();

                if (reader.Name != "EntitySpacesProject")
                {
                    throw new Exception("Invalid Project File: '" + fileNameAndFilePath + "'");
                }

                reader.Read();

                currentNode = new esProjectNode();
                currentNode.Name = reader.GetAttribute("Name");
                RootNode = currentNode;

                parents[reader.Depth] = currentNode;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "Folder":

                                currentNode = new esProjectNode();
                                currentNode.Name = reader.GetAttribute("Name");

                                parents[reader.Depth - 1].Children.Add(currentNode);
                                parents[reader.Depth] = currentNode;
                                break;

                            case "RecordedTemplate":

                                currentNode = new esProjectNode();
                                currentNode.Name = reader.GetAttribute("Name");
                                currentNode.IsFolder = false;

                                int depth = reader.Depth;

                                // <Template>
                                reader.Read();
                                currentNode.Template = new Template();

                                // Path fixup to the template
                                string path = reader.GetAttribute("Path");
                                path = path.Replace("{fixup}", userSettings.TemplatePath);
                                path = path.Replace("\\\\", "\\");

                                currentNode.Template.Parse(path);

                                // <Input>
                                reader.Read();
                                XmlReader input = reader.ReadSubtree();
                                input.Read();

                                currentNode.Input = new Hashtable();

                                while (input.Read())
                                {
                                    string type = input.GetAttribute("Type");
                                    string key = input.GetAttribute("Key");
                                    string value = input.GetAttribute("Value");

                                    if (key == "OutputPath")
                                    {
                                        value = FixupTheFixup(this.projectFilePath, value);
                                    }

                                    switch (type)
                                    {
                                        case "(null)":
                                            currentNode.Input[key] = null;
                                            break;

                                        case "System.String":
                                            currentNode.Input[key] = value;
                                            break;

                                        case "System.Char":
                                            currentNode.Input[key] = Convert.ToChar(value);
                                            break;

                                        case "System.DateTime":
                                            currentNode.Input[key] = Convert.ToDateTime(value);
                                            break;

                                        case "System.Decimal":
                                            currentNode.Input[key] = Convert.ToDecimal(value);
                                            break;

                                        case "System.Double":
                                            currentNode.Input[key] = Convert.ToDouble(value);
                                            break;

                                        case "System.Boolean":
                                            currentNode.Input[key] = Convert.ToBoolean(value);
                                            break;

                                        case "System.Int16":
                                            currentNode.Input[key] = Convert.ToInt16(value);
                                            break;

                                        case "System.Int32":
                                            currentNode.Input[key] = Convert.ToInt32(value);
                                            break;

                                        case "System.Int64":
                                            currentNode.Input[key] = Convert.ToInt64(value);
                                            break;

                                        case "System.Collections.ArrayList":

                                            ArrayList list = new ArrayList();
                                            string[] items = value.Split(',');

                                            foreach (string item in items)
                                            {
                                                list.Add(item);
                                            }

                                            currentNode.Input[key] = list;
                                            break;
                                    }
                                }

                                // <Settings>
                                reader.Read();
                                XmlReader settings = reader.ReadSubtree();

                                currentNode.Settings = new esSettings();
                                currentNode.Settings = esSettings.Load(settings);

                                // Fixup Settings ...
                                currentNode.Settings.TemplatePath = userSettings.TemplatePath;
                                currentNode.Settings.OutputPath = userSettings.OutputPath;
                                currentNode.Settings.UIAssemblyPath = userSettings.UIAssemblyPath;
                                currentNode.Settings.CompilerAssemblyPath = userSettings.CompilerAssemblyPath;
                                currentNode.Settings.LanguageMappingFile = userSettings.LanguageMappingFile;
                                currentNode.Settings.UserMetadataFile = userSettings.UserMetadataFile;

                                parents[depth - 1].Children.Add(currentNode);
                                break;
                        }
                    }
                }
            }
        }

        private string GetFileVersion(string fileNameAndFilePath)
        {
            string version = "0000.0.0000.0";

            try
            {
                using (XmlTextReader reader = new XmlTextReader(fileNameAndFilePath))
                {
                    projectFilePath = fileNameAndFilePath;
                    reader.WhitespaceHandling = WhitespaceHandling.None;

                    reader.Read();
                    reader.Read();

                    version = reader[0];
                }
            }
            catch { }

            return version;
        }

        private void ConvertProject(string fileNameAndFilePath, esSettings settings)
        {
            esProject2010 project2010 = new esProject2010();
            project2010.Load(fileNameAndFilePath, settings);

            ConvertProjectNodeSettings(project2010.RootNode);

            esProject project = new esProject();

            // Manually copy root node of tree into new project
            project.RootNode = new esProjectNode();
            project.RootNode.Name = project2010.RootNode.Name;
            project.RootNode.IsFolder = project2010.RootNode.IsFolder;
            project.RootNode.Children = project2010.RootNode.Children;
            project.RootNode.Settings = project2010.RootNode.Settings;

            FileInfo fileInfo = new FileInfo(fileNameAndFilePath);

            string backup = fileInfo.Name.Replace(fileInfo.Extension, "");
            backup += "_original" + fileInfo.Extension;
            backup = fileInfo.DirectoryName + "\\" + backup;

            File.Copy(fileNameAndFilePath, backup, true);

            // Now Save it in our new format
            project.Save(fileNameAndFilePath, settings);
        }

        private void ConvertProjectNodeSettings(IProjectNode node)
        {
            foreach (IProjectNode childNode in node.Children)
            {
                if (childNode.Settings != null)
                {
                    childNode.Settings = ((esSettings2010)childNode.Settings).To2011();

                    try
                    {
                        esSettings.AdjustPathsBasedOnPriorVersions(childNode.Settings as esSettings, @"Software\EntitySpaces 2009", "ES2009", false);
                        esSettings.AdjustPathsBasedOnPriorVersions(childNode.Settings as esSettings, @"Software\EntitySpaces 2010", "ES2010", false);
                        esSettings.AdjustPathsBasedOnPriorVersions(childNode.Settings as esSettings, @"Software\EntitySpaces 2011", "ES2011", false);
                        esSettings.AdjustPathsBasedOnPriorVersions(childNode.Settings as esSettings, @"Software\EntitySpaces 2012", "ES2012", true);
                    }
                    catch { }
                }

                ConvertProjectNodeSettings(childNode);
            }
        }

        public void Save(string fileNameAndFilePath, esSettings mainSettings)
        {
            projectFilePath = fileNameAndFilePath;
            userSettings = mainSettings;

            XmlTextWriter writer = new XmlTextWriter(fileNameAndFilePath, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();
            writer.WriteStartElement("EntitySpacesProject");
            writer.WriteAttributeString("Version", "2019.1.0708.0");
            Save(this.RootNode, writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        private void Save(esProjectNode node, XmlTextWriter writer)
        {
            BeginWriteNode(node, writer);

            foreach (esProjectNode childNode in node.Children)
            {
                Save(childNode, writer);
            }

            EndWriteNode(node, writer);
        }

        private void BeginWriteNode(esProjectNode node, XmlTextWriter writer)
        {
            if (node.IsFolder)
            {
                writer.WriteStartElement("Folder");
                writer.WriteAttributeString("Name", node.Name);
            }
            else
            {
                writer.WriteStartElement("RecordedTemplate");
                writer.WriteAttributeString("Name", node.Name);

                writer.WriteStartElement("Template");
                writer.WriteAttributeString("Name", node.Template.Header.Title);
                writer.WriteAttributeString("Path", node.Template.Header.FullFileName.Replace(userSettings.TemplatePath, "{fixup}"));
                writer.WriteAttributeString("Version", node.Template.Header.Version);
                writer.WriteEndElement();

                writer.WriteStartElement("Input");
                if (node.Input.Count > 0)
                {
                    foreach (string key in node.Input.Keys)
                    {
                        object value = node.Input[key];

                        if (key == "OutputPath")
                        {
                             value = this.CreateFixup(this.projectFilePath, (string)value);
                        }

                        if (value == null)
                        {
                            writer.WriteStartElement("Item");
                            writer.WriteAttributeString("Type", "(null)");
                            writer.WriteAttributeString("Key", key);
                            writer.WriteEndElement();
                            continue;
                        }

                        string typeName = value.GetType().FullName;

                        writer.WriteStartElement("Item");
                        writer.WriteAttributeString("Type", typeName);
                        writer.WriteAttributeString("Key", key);

                        switch (typeName)
                        {
                            case "System.Collections.ArrayList":

                                ArrayList list = value as ArrayList;

                                string values = "";
                                string comma = "";

                                foreach (string s in list)
                                {
                                    values += comma;
                                    values += s;
                                    comma = ",";
                                }

                                writer.WriteAttributeString("Value", values);
                                break;

                            default:

                                writer.WriteAttributeString("Value", value.ToString());
                                break;
                        }

                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();

                // Save these off so we can restore them
                string bakTemplatePath = node.Settings.TemplatePath;
                string bakOutputPath = node.Settings.OutputPath;
                string bakUIAssemblyPath = node.Settings.UIAssemblyPath;
                string bakCompilerAssemblyPath = node.Settings.CompilerAssemblyPath;
                string bakLanguageMappingFile = node.Settings.LanguageMappingFile;
                string bakUserMetadataFile = node.Settings.UserMetadataFile;

                // Remove Hard coded Paths
                node.Settings.TemplatePath = "{fixup}";
                node.Settings.OutputPath = "{fixup}";
                node.Settings.UIAssemblyPath = "{fixup}";
                node.Settings.CompilerAssemblyPath = "{fixup}";
                node.Settings.LanguageMappingFile = "{fixup}";
                node.Settings.UserMetadataFile = "{fixup}";

                // Now write it
                node.Settings.Save(writer);
                writer.WriteEndElement();

                // Restore the original values
                node.Settings.TemplatePath = bakTemplatePath;
                node.Settings.OutputPath = bakOutputPath;
                node.Settings.UIAssemblyPath = bakUIAssemblyPath;
                node.Settings.CompilerAssemblyPath = bakCompilerAssemblyPath;
                node.Settings.LanguageMappingFile = bakLanguageMappingFile;
                node.Settings.UserMetadataFile = bakUserMetadataFile;
            }
        }

        private void EndWriteNode(esProjectNode node, XmlWriter writer)
        {
            if (node.IsFolder)
            {
                writer.WriteEndElement();
            }
        }

        private string CreateFixup(string projectFile, string outputDir)
        {
            string prjPath = Path.GetDirectoryName(projectFile.ToLower());
            string outPath = outputDir.ToLower();

            char sep = Path.DirectorySeparatorChar;

            string[] prjPathParts = prjPath.Split(sep);
            string[] outPathParts = outPath.Split(sep);

            int i = 0;
            while (true)
            {
                if (prjPathParts.Length > i && outPathParts.Length > i)
                {
                    if (prjPathParts[i] == outPathParts[i])
                    {
                        i++;
                    }
                    else break;
                }
                else break;
            }

            if (i > 1)
            {
                int i_backup = i;

                // At this point "i" is where the paths deviate

                //=====================================================
                // Do We need any \.. path relative stuff?
                //=====================================================
                string fixup = "{fixup";
                while (true)
                {
                    if (prjPathParts.Length > i)
                    {
                        fixup += @"\..";
                        i++;
                    }
                    else break;
                }

                i = i_backup;
                while (true)
                {
                    if (outPathParts.Length > i)
                    {
                        fixup += sep;
                        fixup += outPathParts[i];
                        i++;
                    }
                    else break;
                }

                return fixup + "}";
            }
            else
            {
                return outputDir;
            }
        }

        private string FixupTheFixup(string projectFile, string outputDir)
        {
            if (!outputDir.StartsWith("{")) return outputDir;

            string outputPath = outputDir.Replace("{fixup", "").Replace("}", "");
            string prjPath = Path.GetDirectoryName(projectFile.ToLower());

            char sep = Path.DirectorySeparatorChar;

            string[] prjPathParts = prjPath.Split(sep);
            int index = prjPathParts.Length;

            int loc = 0;
            while (true)
            {
                loc = outputPath.IndexOf(@"\..", loc);

                if (loc != -1)
                {
                    prjPathParts[--index] = "";
                    loc += 3;
                }
                else break;
            }

            outputPath = outputPath.Replace(@"\..", "");

            string basePath = "";
            foreach (string part in prjPathParts)
            {
                if (part != string.Empty)
                {
                    basePath += part;
                    basePath += sep;
                }
            }

            basePath += outputPath;
            basePath = basePath.Replace(@"\\", @"\");
            if (!basePath.EndsWith(@"\"))
            {
                basePath += @"\";
            }
            return basePath;
        }
    }
}
