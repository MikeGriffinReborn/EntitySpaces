using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

using EntitySpaces.AddIn.TemplateUI;
using EntitySpaces.MetadataEngine;

namespace EntitySpaces.TemplateUI
{
    public partial class WCFServiceJson_Basic : UserControl, ITemplateUI
    {
        private Root esMeta;
        private bool UseCachedSettings;
        private object applicationObject;

        public WCFServiceJson_Basic()
        {
            InitializeComponent();
        }

        #region ITemplateUI Members

        esTemplateInfo ITemplateUI.Init()
        {
            esTemplateInfo info = new esTemplateInfo();
            info.UserInterface = this;
            info.UserInterfaceId = new Guid("FB6B68B7-03E7-4fbe-8B6E-34A6274B37BB");
            info.TabTitle = "Basic Information";
            info.TabOrder = 0;
            return info;
        }

        UserControl ITemplateUI.CreateInstance(Root esMeta, bool cachedSettings, object applicationObject)
        {
            WCFServiceJson_Basic window = new WCFServiceJson_Basic();
            window.esMeta = esMeta;
            window.applicationObject = applicationObject;
            window.UseCachedSettings = cachedSettings;

            return window;
        }

        bool ITemplateUI.OnExecute()
        {
            try
            {
                esMeta.Input["OutputPath"] = txtOutputPath.Text;
                esMeta.Input["EntitySpacesNamespace"] = txtEntitySpacesNamespace.Text;
                esMeta.Input["WCFServiceClassNamespace"] = txtWCFServiceClassNamespace.Text;
                esMeta.Input["WCFServiceClassName"] = txtWCFServiceClassName.Text;
                esMeta.Input["ConnectionName"] = txtConnectionName.Text;
                esMeta.Input["Database"] = ((IDatabase)this.cboDatabase.SelectedItem).Name;

                ArrayList list = new ArrayList();

                esMeta.Input["EntityType"] = ShowTables() == true ? "Tables" : "Views";

                foreach (ITableView tv in this.lboxTablesViews.SelectedItems)
                {
                    list.Add(tv.FullName);
                }

                esMeta.Input["Entities"] = list;
            }
            catch { }

            return true;
        }

        void ITemplateUI.OnCancel()
        {

        }

        #endregion

        private void WCFService_Load(object sender, EventArgs e)
        {
            try
            {
                //-----------------------------------------------------------
                // OutputPath
                //-----------------------------------------------------------
                if (UseCachedSettings && esMeta.Input.ContainsKey("OutputPath"))
                {
                    this.txtOutputPath.Text = (string)esMeta.Input["OutputPath"];
                }
                else
                {
                    this.txtOutputPath.Text = (string)esMeta.Input["OutputPath"];

                    if (!this.txtOutputPath.Text.EndsWith(@"\"))
                        this.txtOutputPath.Text += @"\";
                }

                //-----------------------------------------------------------
                // EntitySpacesNamespace
                //-----------------------------------------------------------
                if (UseCachedSettings && esMeta.Input.ContainsKey("EntitySpacesNamespace"))
                {
                    this.txtEntitySpacesNamespace.Text = (string)esMeta.Input["EntitySpacesNamespace"];
                }

                //-----------------------------------------------------------
                // WCFServiceClassNamespace
                //-----------------------------------------------------------
                if (UseCachedSettings && esMeta.Input.ContainsKey("WCFServiceClassNamespace"))
                {
                    this.txtWCFServiceClassNamespace.Text = (string)esMeta.Input["WCFServiceClassNamespace"];
                }

                //-----------------------------------------------------------
                // WCFServiceClassName
                //-----------------------------------------------------------
                if (UseCachedSettings && esMeta.Input.ContainsKey("WCFServiceClassName"))
                {
                    this.txtWCFServiceClassName.Text = (string)esMeta.Input["WCFServiceClassName"];
                }

                //-----------------------------------------------------------
                // ConnectionName
                //-----------------------------------------------------------
                if (UseCachedSettings && esMeta.Input.ContainsKey("ConnectionName"))
                {
                    this.txtConnectionName.Text = (string)esMeta.Input["ConnectionName"];
                }

                //-----------------------------------------------------------
                // Database
                //-----------------------------------------------------------
                this.cboDatabase.DataSource = esMeta.Databases;
                this.cboDatabase.DisplayMember = "Name";

                string database = "";

                if (UseCachedSettings && esMeta.Input.ContainsKey("Database"))
                {
                    database = (string)esMeta.Input["Database"];
                }
                else if (esMeta.DefaultDatabase != null)
                {
                    database = esMeta.DefaultDatabase.Name;
                }

                int index = this.cboDatabase.FindString(database);
                if (index != -1)
                {
                    this.cboDatabase.SelectedIndex = index;
                }

                //-----------------------------------------------------------
                // EntityType
                //-----------------------------------------------------------
                index = this.cboTablesViews.Items.Add("Tables");
                this.cboTablesViews.Items.Add("Views");

                if (UseCachedSettings && esMeta.Input.ContainsKey("EntityType"))
                {
                    index = this.cboTablesViews.FindString((string)esMeta.Input["EntityType"]);
                }

                this.cboTablesViews.SelectedIndex = index;

                //-----------------------------------------------------------
                // Entities
                //-----------------------------------------------------------
                // NOTE: Setting the "cboTablesViews.SelectedIndex" causes the lboxTablesViews
                //       to be bound
                if (UseCachedSettings && esMeta.Input.ContainsKey("Entities"))
                {
                    this.lboxTablesViews.SelectedItems.Clear();

                    ArrayList entities = (ArrayList)esMeta.Input["Entities"];

                    foreach (string entity in entities)
                    {
                        index = this.lboxTablesViews.FindString(entity);

                        if (index != -1)
                        {
                            this.lboxTablesViews.SetSelected(index, true);
                        }
                    }
                }
            }
            catch { }
        }

        private void cboDatabase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BindTablesOrViews();
        }

        private void cboTablesViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTablesOrViews();
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                folderBrowserDialog1.Description = "Select the Output Folder";
                folderBrowserDialog1.SelectedPath = txtOutputPath.Text;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch { }
        }

        private bool ShowTables()
        {
            return this.cboTablesViews.SelectedIndex == 0;
        }

        private void BindTablesOrViews()
        {
            try
            {
                IDatabase database = this.cboDatabase.SelectedItem as IDatabase;

                if (database != null)
                {
                    this.lboxTablesViews.DataSource = null;

                    if (ShowTables())
                    {
                        this.lboxTablesViews.DataSource = database.Tables;
                        this.lboxTablesViews.DisplayMember = "FullName";
                    }
                    else
                    {
                        this.lboxTablesViews.DataSource = database.Views;
                        this.lboxTablesViews.DisplayMember = "FullName";
                    }
                }
            }
            catch { }
        }

        private void lboxTablesViews_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    for (int i = 0; i < this.lboxTablesViews.Items.Count; i++)
                    {
                        this.lboxTablesViews.SetSelected(i, true);
                    }
                }
            }
        }
    }
}