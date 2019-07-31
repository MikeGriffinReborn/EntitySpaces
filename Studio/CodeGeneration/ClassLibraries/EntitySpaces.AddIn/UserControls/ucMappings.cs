using System;
using System.Data;
using System.IO;
using System.Xml;

namespace EntitySpaces.AddIn
{
    internal partial class ucMappings : esUserControl
    {
        XmlDataDocument settings = new XmlDataDocument();

        public ucMappings()
        {
            InitializeComponent();
        }

        public override void OnSettingsChanged()
        {
            LoadData();            
        }

        private void ucMappings_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (!DesignMode)
                {
                    using (StreamReader reader = File.OpenText(Settings.LanguageMappingFile))
                    {
                        settings.LoadXml(reader.ReadToEnd());
                        if (settings != null)
                        {
                            PopulateProviders();
                        }
                    }
                }

                int index = ProviderComboBox.FindString(Settings.Driver);

                if (index != -1)
                {
                    ProviderComboBox.SelectedIndex = index;
                }

            }
            catch { }
        }

        private void PopulateProviders()
        {
            ProviderComboBox.Items.Clear();
            ProviderComboBox.Items.Insert(0, "Select One");

            XmlNodeList nodeList = settings.SelectNodes("Languages/Language");
            if (nodeList != null && nodeList.Count > 0)
            {
                foreach (XmlNode node in nodeList)
                {
                    ProviderComboBox.Items.Add(node.Attributes["From"].Value);
                }
            }

            ProviderComboBox.SelectedIndex = 0;
        }

        private void ProviderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProviderComboBox.SelectedItem.ToString() != "Select One")
            {
                PopulateMappings(ProviderComboBox.SelectedItem.ToString());
            }
        }

        private void PopulateMappings(string providerName)
        {            
            DataTable table = new DataTable();
            table.Columns.Add("Database");
            table.Columns.Add(".NET");

            XmlNode provider = settings.SelectSingleNode(string.Format("Languages/Language[@From='{0}']", providerName));
            if (provider != null)
            {
                foreach(XmlNode node in provider.ChildNodes)
                {
                    DataRow row = table.NewRow();

                    row["Database"] = node.Attributes["From"].Value;
                    row[".NET"] = node.Attributes["To"].Value;

                    table.Rows.Add(row);
                }
            }

            MappingsDataGridView.DataSource = table;   
        }

        private void ToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            switch (e.Button.Tag as string)
            {
                case "save":

                    try
                    {
                        string provider = ProviderComboBox.SelectedItem.ToString();

                        // First Lets Remove the current node for this entire database
                        XmlNode node = settings.SelectSingleNode(string.Format("Languages/Language[@From='{0}']", provider));
                        node.ParentNode.RemoveChild(node);

                        // Grab the parent node of the entire xml file
                        XmlNode parentNode = settings.SelectSingleNode(@"//Languages");

                        // Create our new Language Node entry for this database
                        XmlNode langNode = settings.CreateNode(XmlNodeType.Element, "Language", null);
                        parentNode.AppendChild(langNode);

                        // Fill in it's To/From Attributes
                        XmlAttribute attr = settings.CreateAttribute("From");
                        attr.Value = provider;
                        langNode.Attributes.Append(attr);

                        attr = settings.CreateAttribute("To");
                        attr.Value = "C#";
                        langNode.Attributes.Append(attr);

                        // Lets sort them in order of the database column type 
                        DataTable dt = (DataTable)MappingsDataGridView.DataSource;
                        dt.DefaultView.Sort = "Database ASC";

                        // Now let's add all of the mappings
                        foreach (DataRowView row in dt.DefaultView)
                        {
                            XmlNode typeNode = settings.CreateNode(XmlNodeType.Element, "Type", null);
                            langNode.AppendChild(typeNode);

                            attr = settings.CreateAttribute("From");
                            attr.Value = (string)row.Row["Database"];
                            typeNode.Attributes.Append(attr);

                            attr = settings.CreateAttribute("To");
                            attr.Value = (string)row.Row[".NET"];
                            typeNode.Attributes.Append(attr);
                        }

                        settings.Save(this.Settings.LanguageMappingFile);
                    }
                    catch (Exception ex)
                    {
                        this.MainWindow.ShowError(ex);
                    }
                    break;
            }
        }
    }
}
