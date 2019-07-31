using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace EntitySpaces.MetadataEngine
{
    public class esXmlUserDataMigration
    {
        private string fullPathToConfigFile;
        private string driver;
        private string version;
        private XmlDocument Xml;

        public esXmlUserDataMigration(string fullPathToConfigFile, string driver, string version)
        {
            this.fullPathToConfigFile = fullPathToConfigFile;
            this.driver = driver;
            this.version = version;
        }

        public void PerformMigration()
        {
            try
            {
                if (!File.Exists(fullPathToConfigFile)) return;

                Xml = new XmlDocument();
                Xml.Load(fullPathToConfigFile);

                // If the version numbers match, bail ...
                if (DoesTheVersionNumberMatch()) return;

                BackupConfigFile();

                AddVersionNumberAttribute();
                AddDriversAndDriverElement();
                AddDriverNameAttribute();
                RenamePAttributeToName();
                RenameNAttributeToAlias();

                MoveTableProperties();
                DeletePropertiesSection();

                SaveMigration();
            }
            catch { }
        }

        private bool DoesTheVersionNumberMatch()
        {
            bool match = false;

            XmlNode rootNode = Xml.SelectSingleNode("//esUserData");
            if (rootNode != null)
            {
                foreach (XmlAttribute attribute in rootNode.Attributes)
                {
                    if(attribute.Name == "Version")
                    {
                        if( attribute.Value == this.version || 
                            attribute.Value == "2009.2.0831.0" ||
                            attribute.Value == "2009.2.0928.0" ||
                            attribute.Value == "2010.1.0628.0" ||
                            attribute.Value == "2010.1.1122.0" ||
                            attribute.Value == "2019.1.0725.0" ||
                            attribute.Value == "2019.1.0725.0")
                        {
                            match = true;
                            break;
                        }
                    }
                }
            }

            return match;
        }

        private void AddVersionNumberAttribute()
        {
            XmlNode rootNode = Xml.SelectSingleNode("//esUserData");
            if (rootNode != null)
            {
                AddAttribute(rootNode, "Version", this.version);
            }
        }

        private void AddDriversAndDriverElement()
        {
            XmlNode rootNode = Xml.SelectSingleNode("//esUserData");
            if (rootNode != null)
            {
                if (!rootNode.InnerXml.StartsWith("<Drivers><Driver"))
                {
                    string xml = "<Drivers><Driver>" + rootNode.InnerXml + "</Driver></Drivers>";
                    rootNode.InnerXml = xml;
                }
            }
        }

        private void AddDriverNameAttribute()
        {
            XmlNode driverNode = Xml.SelectSingleNode("//Driver");
            if (driverNode != null)
            {
                AddAttribute(driverNode, "Name", this.driver);
            }
        }

        private void RenamePAttributeToName()
        {
            XmlNode rootNode = Xml.SelectSingleNode("//esUserData");
            if (rootNode != null)
            {
                RenameAttribute(rootNode, 0, "p", "Name");
            }
        }

        private void RenameNAttributeToAlias()
        {
            XmlNode rootNode = Xml.SelectSingleNode("//esUserData");
            if (rootNode != null)
            {
                RenameAttribute(rootNode, 0, "n", "Alias");
            }
        }

        private void MoveTableProperties()
        {
            XmlNodeList databases = GetDatabases();
            {
                if (databases != null)
                {
                    foreach (XmlNode database in databases)
                    {
                        foreach (XmlNode databaseChild in database)
                        {
                            if (databaseChild.Name == "Tables")
                            {
                                foreach (XmlElement table in databaseChild.ChildNodes)
                                {
                                    foreach (XmlNode node in table.ChildNodes)
                                    {
                                        //this table has a properties section lets migrate it
                                        if (node.Name == "Properties")
                                        {
                                            MoveTablePropertyToColumnAttribute(table);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MoveTablePropertyToColumnAttribute(XmlNode table)
        {
            XmlNodeList tableChildren = table.ChildNodes;
            foreach (XmlNode tableChild in tableChildren)
            {
                if (tableChild.Name == "Properties")
                {            
                    foreach (XmlNode property in tableChild.ChildNodes)
                    {
                        MigrateAutoKeyPropertySection(property);
                        MigrateConcurrencyPropertySection(property);
                    }
                }
            }
        }

        private void MigrateAutoKeyPropertySection(XmlNode propertyNode)
        {
            XmlNode tableNode = propertyNode.ParentNode.ParentNode;
            foreach (XmlAttribute attribute in propertyNode.Attributes)
            {
                if (attribute.Value.StartsWith("AUTOKEY:"))
                {
                    //found auto key column
                    EnsureTableHasColumnsSection(tableNode);
                    string autoKeyColumn = attribute.Value.Replace("AUTOKEY:", string.Empty);
                    string autoKeyTextValue = propertyNode.Attributes["v"].Value;

                    if (!string.IsNullOrEmpty(autoKeyColumn) && !string.IsNullOrEmpty(autoKeyTextValue))
                    {
                        List<XmlNode> tableColumns = GetTableColumns(tableNode);
                        if (tableColumns != null)
                        {
                            //see if that column was there already because it had an alias
                            foreach (XmlNode tableColumn in tableColumns)
                            {
                                if (tableColumn.Attributes["Name"] != null)
                                {
                                    if (tableColumn.Attributes["Name"].Value == autoKeyColumn)
                                    {
                                        AddAttribute(tableColumn, "IsAutoKey", "True");
                                        AddAttribute(tableColumn, "AutoKeyText", autoKeyTextValue);
                                        return;
                                    }
                                }
                            }
                        }

                        //nope need to add the column element with appropriate attributes
                        XmlNode columnNode = AddColumnToColumnsSection(tableNode);
                        if (columnNode != null)
                        {
                            AddAttribute(columnNode, "Name", autoKeyColumn);
                            AddAttribute(columnNode, "IsAutoKey", "True");
                            AddAttribute(columnNode, "AutoKeyText", autoKeyTextValue);
                        }
                    }
                }
            }
        }

        private void MigrateConcurrencyPropertySection(XmlNode propertyNode)
        {
            XmlNode tableNode = propertyNode.ParentNode.ParentNode;
            foreach (XmlAttribute attribute in propertyNode.Attributes)
            {
                if (attribute.Value.StartsWith("CONCURR:"))
                {
                    //found concurrency column
                    EnsureTableHasColumnsSection(tableNode);
                    string concurrencyColumn = attribute.Value.Replace("CONCURR:", string.Empty);
                    if (!string.IsNullOrEmpty(concurrencyColumn))
                    {
                        List<XmlNode> tableColumns = GetTableColumns(tableNode);
                        if (tableColumns != null)
                        {
                            //see if that column was there already because it had an alias
                            foreach (XmlNode tableColumn in tableColumns)
                            {
                                if (tableColumn.Attributes["Name"] != null)
                                {
                                    if (tableColumn.Attributes["Name"].Value == concurrencyColumn)
                                    {
                                        AddAttribute(tableColumn, "IsEntitySpacesConcurrency", "True");
                                        return;
                                    }
                                }
                            }
                        }

                        //nope need to add the column element with appropriate attributes
                        XmlNode columnNode = AddColumnToColumnsSection(tableNode);
                        if (columnNode != null)
                        {
                            AddAttribute(columnNode, "IsEntitySpacesConcurrency", "True");
                        }
                    }
                }
            }
        }

        private XmlNodeList GetDatabases()
        {
            XmlNodeList databases = Xml.SelectNodes("//Database");
            if (databases != null)
            {
                return databases;
            }

            return null;
        }

        private void DeletePropertiesSection()
        {
            XmlNodeList tables = Xml.SelectNodes("//Table");
            if (tables != null)
            {
                foreach (XmlNode table in tables)
                {
                    foreach (XmlNode tableElement in table.ChildNodes)
                    {
                        if (tableElement.Name == "Properties")
                        {
                            tableElement.ParentNode.RemoveChild(tableElement);
                        }
                    }
                }
            }

            XmlNodeList columns = Xml.SelectNodes("//Column");
            if (tables != null)
            {
                foreach (XmlNode column in columns)
                {
                    foreach (XmlNode columnElement in column.ChildNodes)
                    {
                        if (columnElement.Name == "Properties")
                        {
                            columnElement.ParentNode.RemoveChild(columnElement);
                        }
                    }
                }
            }
        }

        private List<XmlNode> GetTableColumns(XmlNode tableNode)
        {
            List<XmlNode> columns = new List<XmlNode>();
            foreach (XmlNode node in tableNode.ChildNodes)
            {
                if (node.Name == "Columns")
                {
                    foreach (XmlNode column in node.ChildNodes)
                    {
                        columns.Add(column);
                    }
                }
            }

            return columns;
        }

        private void EnsureTableHasColumnsSection(XmlNode tableNode)
        {
            List<XmlNode> columns = new List<XmlNode>();
            foreach (XmlNode node in tableNode.ChildNodes)
            {
                if (node.Name == "Columns")
                {
                    return;
                }
            }

            XmlNode columnsNode = Xml.CreateNode(XmlNodeType.Element, "Columns", string.Empty);
            tableNode.AppendChild(columnsNode);
        }

        private XmlNode AddColumnToColumnsSection(XmlNode tableNode)
        {
            List<XmlNode> columns = new List<XmlNode>();
            foreach (XmlNode node in tableNode.ChildNodes)
            {
                if (node.Name == "Columns")
                {
                    XmlNode columnNode = Xml.CreateNode(XmlNodeType.Element, "Column", string.Empty);
                    return node.AppendChild(columnNode);
                }
            }

            return null;
        }

        #region Xml Helpers

        private void AddAttribute(XmlNode node, string attributeName, string attributeValue)
        {
            node.Attributes.Append(Xml.CreateAttribute(attributeName));
            node.Attributes[attributeName].Value = attributeValue;
        }

        private void RenameAttribute(XmlNode node, int level, string oldAttributeName, string newAttributeName)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                XmlAttribute oldAttribute = child.Attributes[oldAttributeName];
                if (oldAttribute != null)
                {
                    if (oldAttribute.Value != String.Empty)
                    {
                        child.Attributes.Append(Xml.CreateAttribute(newAttributeName));
                        child.Attributes[newAttributeName].Value = oldAttribute.Value;
                    }

                    child.Attributes.Remove(oldAttribute);
                }

                RenameAttribute(child, level + 1, oldAttributeName, newAttributeName);
            }
        }

        private void ReplaceAttributeValue(XmlNode node, string attributeName, string attributeValue)
        {
            node.Attributes[attributeName].Value = attributeValue;
        }

        private void BackupConfigFile()
        {
            FileInfo sourceFile = new FileInfo(fullPathToConfigFile);
            if (sourceFile.Directory != null)
            {
                string destinationFileFullPath = Path.Combine(sourceFile.Directory.FullName, Guid.NewGuid().ToString() + "-esUserData.bak");
                sourceFile.CopyTo(destinationFileFullPath);
            }
        }

        private void SaveMigration()
        {
            Xml.Save(fullPathToConfigFile);
        }

        #endregion
    }
}
