using System;
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
    public partial class GeneratedMaster_ProxyStub : UserControl, ITemplateUI
    {
        private Root esMeta;
        private bool UseCachedSettings;
        private object applicationObject;
        private bool WCFSupport;

        public GeneratedMaster_ProxyStub()
        {
            InitializeComponent();
        }

        #region ITemplateUI Members

        esTemplateInfo ITemplateUI.Init()
        {
            esTemplateInfo info = new esTemplateInfo();
            info.UserInterface = this;
            info.UserInterfaceId = new Guid("2216AB4F-BDB4-47de-8412-8560C1F2F420");
            info.TabTitle = "Proxy/Stub";
            info.TabOrder = 2;
            return info;
        }

        UserControl ITemplateUI.CreateInstance(Root esMeta, bool cachedSettings, object applicationObject)
        {
            GeneratedMaster_ProxyStub window = new GeneratedMaster_ProxyStub();
            window.esMeta = esMeta;
            window.applicationObject = applicationObject;
            window.UseCachedSettings = cachedSettings;

            return window;
        }

        bool ITemplateUI.OnExecute()
        {
            try
            {
                esMeta.Input["GenerateProxyStub"] = chkProxyStub.Checked;
                esMeta.Input["IncludeRowStateInXml"] = chkManageState.Checked;
                esMeta.Input["WcfSupport"] = chkWCFSupport.Checked;
                esMeta.Input["WcfDataContract"] = txtDataContract.Text;
                esMeta.Input["WcfEmitDefaultValue"] = chkWCFEmitDefault.Checked;
                esMeta.Input["WcfOrder"] = chkWCFOrder.Checked;
                esMeta.Input["WcfIsRequired"] = chkWCFIsRequired.Checked;
                esMeta.Input["CompactXML"] = chkCompactXML.Checked;
            }
            catch { }

            return true;
        }

        void ITemplateUI.OnCancel()
        {
            // Nothing to do really
        }

        #endregion

        private void chkProxyStub_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkProxyStub.Checked)
            {
                chkManageState.Enabled = true;
                chkWCFSupport.Enabled = true;
                chkWCFSupport.Checked = WCFSupport;
            }
            else
            {
                chkManageState.Enabled = false;
                chkWCFSupport.Enabled = false;
                WCFSupport = chkWCFSupport.Checked;
                chkWCFSupport.Checked = false;
            }
        }

        private void chkWCFSupport_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkWCFSupport.Enabled)
            {
                if (chkWCFSupport.Checked)
                {
                    txtDataContract.Enabled = true;
                    chkWCFEmitDefault.Enabled = true;
                    chkWCFIsRequired.Enabled = true;
                    chkWCFOrder.Enabled = true;
                    chkCompactXML.Enabled = true;
                }
                else
                {
                    txtDataContract.Enabled = false;
                    chkWCFEmitDefault.Enabled = false;
                    chkWCFIsRequired.Enabled = false;
                    chkWCFOrder.Enabled = false;
                    chkCompactXML.Enabled = false;
                }
            }
        }

        private void GeneratedMaster_ProxyStub_Load(object sender, EventArgs e)
        {
            if (!UseCachedSettings) return;

            try
            {
                if (esMeta.Input.ContainsKey("GenerateProxyStub"))
                {
                    chkProxyStub.Checked = (bool)esMeta.Input["GenerateProxyStub"];
                }

                if (esMeta.Input.ContainsKey("IncludeRowStateInXml"))
                {
                    chkManageState.Checked = (bool)esMeta.Input["IncludeRowStateInXml"];
                }

                if (esMeta.Input.ContainsKey("WcfSupport"))
                {
                    chkWCFSupport.Checked = (bool)esMeta.Input["WcfSupport"];
                }

                if (esMeta.Input.ContainsKey("WcfDataContract"))
                {
                    txtDataContract.Text = (string)esMeta.Input["WcfDataContract"];
                }

                if (esMeta.Input.ContainsKey("WcfEmitDefaultValue"))
                {
                    chkWCFEmitDefault.Checked = (bool)esMeta.Input["WcfEmitDefaultValue"];
                }

                if (esMeta.Input.ContainsKey("WcfOrder"))
                {
                    chkWCFOrder.Checked = (bool)esMeta.Input["WcfOrder"];
                }

                if (esMeta.Input.ContainsKey("WcfIsRequired"))
                {
                    chkWCFIsRequired.Checked = (bool)esMeta.Input["WcfIsRequired"];
                }

                if (esMeta.Input.ContainsKey("CompactXML"))
                {
                    chkCompactXML.Checked = (bool)esMeta.Input["CompactXML"];
                }
            }
            catch { }
        }
    }
}
