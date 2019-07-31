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
    public partial class MultiProviderMap_Advanced : UserControl, ITemplateUI
    {
        private Root esMeta;
        private bool UseCachedSettings;
        private object applicationObject;

        public MultiProviderMap_Advanced()
        {
            InitializeComponent();
        }
        #region ITemplateUI Members

        esTemplateInfo ITemplateUI.Init()
        {
            esTemplateInfo info = new esTemplateInfo();
            info.UserInterface = this;
            info.UserInterfaceId = new Guid("DF64D8BE-865C-449c-B43A-FB8B6A1DC3B9");
            info.TabTitle = "Advanced";
            info.TabOrder = 1;
            return info;
        }

        UserControl ITemplateUI.CreateInstance(Root esMeta, bool cachedSettings, object applicationObject)
        {
            MultiProviderMap_Advanced window = new MultiProviderMap_Advanced();
            window.esMeta = esMeta;
            window.applicationObject = applicationObject;
            window.UseCachedSettings = cachedSettings;
            return window;
        }

        bool ITemplateUI.OnExecute()
        {
            try
            {
                esMeta.Input["MetadataClassShouldIgnoreSchema"] = chkIgnoreSchema.Checked;
                esMeta.Input["MetadataClassShouldIgnoreCatalog"] = chkIgnoreCatalog.Checked;
            }
            catch { }

            return true;
        }

        void ITemplateUI.OnCancel()
        {

        }

        #endregion

        private void MultiProviderMap_Advanced_Load(object sender, EventArgs e)
        {
            if (!UseCachedSettings) return;

            try
            {
                if (esMeta.Input.ContainsKey("MetadataClassShouldIgnoreSchema"))
                {
                    chkIgnoreSchema.Checked = (bool)esMeta.Input["MetadataClassShouldIgnoreSchema"];
                }

                if (esMeta.Input.ContainsKey("MetadataClassShouldIgnoreCatalog"))
                {
                    chkIgnoreCatalog.Checked = (bool)esMeta.Input["MetadataClassShouldIgnoreCatalog"];
                }
            }
            catch { }
        }
    }
}