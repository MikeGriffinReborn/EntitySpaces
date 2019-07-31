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
    public partial class ClientProxyStub_ProxyStub : UserControl, ITemplateUI
    {
        private Root esMeta;
        private bool UseCachedSettings;
        private object applicationObject;
        private bool WCFSupport;

        public ClientProxyStub_ProxyStub()
        {
            InitializeComponent();
        }

        #region ITemplateUI Members

        esTemplateInfo ITemplateUI.Init()
        {
            esTemplateInfo info = new esTemplateInfo();
            info.UserInterface = this;
            info.UserInterfaceId = new Guid("2E918CFD-D2A7-44a8-BED7-CEFC1EDCC4EC");
            info.TabTitle = "Proxy/Stub";
            info.TabOrder = 1;
            return info;
        }

        UserControl ITemplateUI.CreateInstance(Root esMeta, bool cachedSettings, object applicationObject)
        {
            ClientProxyStub_ProxyStub window = new ClientProxyStub_ProxyStub();
            window.esMeta = esMeta;
            window.applicationObject = applicationObject;
            window.UseCachedSettings = cachedSettings;

            return window;
        }

        bool ITemplateUI.OnExecute()
        {
            try
            {
                esMeta.Input["EnableDynamicQueryAPI"] = chkEnableDynamicQueryAPI.Checked;
                esMeta.Input["IncludeRowStateInXml"] = chkManageState.Checked;
                esMeta.Input["WcfSupport"] = chkWCFSupport.Checked;
                esMeta.Input["WcfDataContract"] = txtDataContract.Text;
                esMeta.Input["WcfEmitDefaultValue"] = chkWCFEmitDefault.Checked;
                esMeta.Input["WcfOrder"] = chkWCFOrder.Checked;
                esMeta.Input["WcfIsRequired"] = chkWCFIsRequired.Checked;
                esMeta.Input["INotifyPropertyChanged"] = chkINotifyPropertyChanged.Checked;
                esMeta.Input["CompactXML"] = chkCompactXML.Checked;
                esMeta.Input["TargetSilverlight"] = chkTargetSilverlight.Checked;
                esMeta.Input["ObservableCollection"] = chkObservableCollections.Checked;
                esMeta.Input["BindingList"] = chkBindingList.Checked;
            }
            catch { }

            return true;
        }

        void ITemplateUI.OnCancel()
        {
            // Nothing to do really
        }

        #endregion

        private void chkWCFSupport_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkWCFSupport.Checked && chkWCFSupport.Enabled)
            {
                txtDataContract.Enabled = true;
                chkWCFEmitDefault.Enabled = true;
                chkWCFIsRequired.Enabled = true;
                chkWCFOrder.Enabled = true;
                chkINotifyPropertyChanged.Enabled = true;
                chkCompactXML.Enabled = true;
                chkTargetSilverlight.Enabled = true;
                chkObservableCollections.Enabled = true;
                chkBindingList.Enabled = true;
            }
            else
            {
                txtDataContract.Enabled = false;
                chkWCFEmitDefault.Enabled = false;
                chkWCFIsRequired.Enabled = false;
                chkWCFOrder.Enabled = false;
                chkINotifyPropertyChanged.Enabled = false;
                chkCompactXML.Enabled = false;
                chkTargetSilverlight.Enabled = false;
                chkObservableCollections.Enabled = false;
                chkBindingList.Enabled = false;
            }
        }

        private void ClientProxyStub_ProxyStub_Load(object sender, EventArgs e)
        {
            try
            {
                if (!UseCachedSettings) return;

                if (esMeta.Input.ContainsKey("EnableDynamicQueryAPI"))
                {
                    chkManageState.Checked = (bool)esMeta.Input["EnableDynamicQueryAPI"];
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

                if (esMeta.Input.ContainsKey("INotifyPropertyChanged"))
                {
                    chkINotifyPropertyChanged.Checked = (bool)esMeta.Input["INotifyPropertyChanged"];
                }

                if (esMeta.Input.ContainsKey("CompactXML"))
                {
                    chkCompactXML.Checked = (bool)esMeta.Input["CompactXML"];
                }

                if (esMeta.Input.ContainsKey("TargetSilverlight"))
                {
                    chkTargetSilverlight.Checked = (bool)esMeta.Input["TargetSilverlight"];
                }

                if (esMeta.Input.ContainsKey("ObservableCollection"))
                {
                    chkObservableCollections.Checked = (bool)esMeta.Input["ObservableCollection"];
                }

                if (esMeta.Input.ContainsKey("BindingList"))
                {
                    chkBindingList.Checked = (bool)esMeta.Input["BindingList"];
                }
            }
            catch { }
        }
    }
}
