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
    public partial class EntitySpaces_js_Advanced : UserControl, ITemplateUI
    {
        private Root esMeta;
        private bool UseCachedSettings;
        private object applicationObject;

        public EntitySpaces_js_Advanced()
        {
            InitializeComponent();
        }

        #region ITemplateUI Members

        esTemplateInfo ITemplateUI.Init()
        {
            esTemplateInfo info = new esTemplateInfo();
            info.UserInterface = this;
            info.UserInterfaceId = new Guid("491990CE-9355-46c8-93FC-E2EC8956BC41");
            info.TabTitle = "Advanced";
            info.TabOrder = 1;
            return info;
        }

        UserControl ITemplateUI.CreateInstance(Root esMeta, bool cachedSettings, object applicationObject)
        {
            EntitySpaces_js_Advanced window = new EntitySpaces_js_Advanced();
            window.esMeta = esMeta;
            window.applicationObject = applicationObject;
            window.UseCachedSettings = cachedSettings;

            return window;
        }

        bool ITemplateUI.OnExecute()
        {
            try
            {
                esMeta.Input["GenerateSingleFile"] = chkSingleFile.Checked;
                esMeta.Input["GenerateHierarchicalModel"] = chkHierarchical.Checked;

                if (chkHierarchical.Checked)
                {
                    esMeta.Input["GenerateHierarchicalModelSelectedTablesOnly"] = chkHierarchicalSelectedOnly.Checked;
                    esMeta.Input["GenerateHierarchicalLazyLoadSupport"] = chkHierarchicalLazyLoad.Checked;
                }
                else
                {
                    esMeta.Input["GenerateHierarchicalModelSelectedTablesOnly"] = false;
                    esMeta.Input["GenerateHierarchicalLazyLoadSupport"] = false;
                }

                esMeta.Input["GenerateRestAPI"] = chkRestAPI.Checked;
                esMeta.Input["ServicePath"] = txtServicePath.Text;
            }
            catch { }

            return true;
        }

        void ITemplateUI.OnCancel()
        {

        }

        #endregion

        private void EntitySpaces_js_Advanced_Load(object sender, EventArgs e)
        {
            try
            {
                if (!UseCachedSettings) return;

                if (esMeta.Input.ContainsKey("GenerateSingleFile"))
                {
                    chkSingleFile.Checked = (bool)esMeta.Input["GenerateSingleFile"];
                }

                if (esMeta.Input.ContainsKey("GenerateHierarchicalModel"))
                {
                    chkHierarchical.Checked = (bool)esMeta.Input["GenerateHierarchicalModel"];
                }

                if (esMeta.Input.ContainsKey("GenerateHierarchicalModelSelectedTablesOnly"))
                {
                    chkHierarchicalSelectedOnly.Checked = (bool)esMeta.Input["GenerateHierarchicalModelSelectedTablesOnly"];
                }

                if (esMeta.Input.ContainsKey("GenerateHierarchicalLazyLoadSupport"))
                {
                    chkHierarchicalLazyLoad.Checked = (bool)esMeta.Input["GenerateHierarchicalLazyLoadSupport"];
                }

                if (esMeta.Input.ContainsKey("GenerateRestAPI"))
                {
                    chkRestAPI.Checked = (bool)esMeta.Input["GenerateRestAPI"];
                }

                if (esMeta.Input.ContainsKey("ServicePath"))
                {
                    txtServicePath.Text = (string)esMeta.Input["ServicePath"];
                }
            }
            catch { }
        }

        private void chkHierarchical_CheckedChanged(object sender, EventArgs e)
        {
            chkHierarchicalSelectedOnly.Enabled = chkHierarchical.Checked;
            chkHierarchicalLazyLoad.Enabled = chkHierarchical.Checked;
        }
    }
}