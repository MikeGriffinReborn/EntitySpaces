using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EntitySpaces.AddIn.TemplateUI;
using EntitySpaces.MetadataEngine;

namespace EntitySpaces.TemplateUI
{
    public partial class StoredProcedure_Oracle : UserControl, ITemplateUI
    {
        private Root esMeta;
        private bool UseCachedSettings;
        private object applicationObject;

        public StoredProcedure_Oracle()
        {
            InitializeComponent();
        }

        #region ITemplateUI Members

        esTemplateInfo ITemplateUI.Init()
        {
            esTemplateInfo info = new esTemplateInfo();
            info.UserInterface = this;
            info.UserInterfaceId = new Guid("BCB3B598-9264-40dc-B497-1D872D9E324E");
            info.TabTitle = "PostgreSQL";
            info.TabOrder = 1;
            return info;
        }

        UserControl ITemplateUI.CreateInstance(Root esMeta, bool cachedSettings, object applicationObject)
        {
            StoredProcedure_Oracle window = new StoredProcedure_Oracle();
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
                esMeta.Input["Database"] = ((IDatabase)this.cboDatabase.SelectedItem).Name;
                esMeta.Input["EntityType"] = "Tables";

                ArrayList list = new ArrayList();

                foreach (ITable table in this.lboxTablesViews.SelectedItems)
                {
                    list.Add(table.FullName);
                }

                esMeta.Input["Entities"] = list;
            }
            catch { }

            return true;
        }

        void ITemplateUI.OnCancel()
        {
            // Nothing to do really
        }

        #endregion

        private void StoredProcedure_Oracle_Load(object sender, EventArgs e)
        {
            try
            {
                //-----------------------------------------------------------
                // OutputPath
                //-----------------------------------------------------------
                this.txtOutputPath.Text = (string)esMeta.Input["OutputPath"];

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
                // Entities
                //-----------------------------------------------------------
                this.lboxTablesViews.DataSource = esMeta.Databases[database].Tables;
                this.lboxTablesViews.DisplayMember = "FullName";

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
            try
            {
                IDatabase database = this.cboDatabase.SelectedValue as IDatabase;

                this.lboxTablesViews.DataSource = database.Tables;
                this.lboxTablesViews.DisplayMember = "FullName";
            }
            catch { }
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
