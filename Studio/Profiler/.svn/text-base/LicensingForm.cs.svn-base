using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Microsoft.Win32;
using System.IO;

namespace EntitySpaces.ProfilerApplication
{
    public partial class LicensingForm : DevExpress.XtraEditors.XtraForm
    {
        internal  ProxySettings proxySettings = null;

        public LicensingForm()
        {
            InitializeComponent();
        }

        private void LicensingForm_Load(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2012", true);
            if (key != null)
            {
                try
                {
                    this.txtSerialNumber.Text = (string)key.GetValue("Profiler_Number");
                }
                catch { }
            }

            if (proxySettings != null)
            {
                this.chkUseProxyServer.Checked = proxySettings.UseProxy;
                this.txtProxyServerURL.Text = proxySettings.Url;
                this.txtProxyServerUserName.Text = proxySettings.UserName;
                this.txtProxyServerPassword.Text = proxySettings.Password;
                this.txtProxyServerDomainName.Text = proxySettings.DomainName;
            }
        }

        private void chkUseProxyServer_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = !txtProxyServerURL.Enabled;

            txtProxyServerURL.Enabled = enable;
            txtProxyServerUserName.Enabled = enable;
            txtProxyServerPassword.Enabled = enable;
            txtProxyServerDomainName.Enabled = enable;
        }

        private void buttonActivate_Click(object sender, EventArgs e)
        {
            string originalKey = null;

            try
            {
                string serialNum = txtSerialNumber.Text != null ? txtSerialNumber.Text.Trim() : "";
                Guid serialNumber = new Guid(serialNum);
            }
            catch
            {
                MessageBox.Show("Invalid Serial Number");
                return;
            }

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2012", true);
            if (key != null)
            {
                originalKey = (string)key.GetValue("Profiler_Number");
                key.SetValue("Profiler_Number", txtSerialNumber.Text.Trim());
            }

            try
            {
                Licensing licensing = new Licensing();
                licensing.ReplaceMeLater("profiler", "2012.1.0000.0", "Profiler_Number", "Profiler_Number2", "Interop.ADODB64X.dll", proxySettings);

                MessageBox.Show("You must close the Profiler and restart it for the activation to take effect.");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to Validate License");

                if (originalKey != null && originalKey.Length > 0)
                {
                    key.SetValue("Profiler_Number", originalKey);
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2012", true);
                if (key != null)
                {
                    key.DeleteValue("Profiler_Number2");

                    string installDir = (string)key.GetValue("Profiler_Install_Dir");
                    if (installDir.EndsWith("\\"))
                    {
                        installDir = installDir.TrimEnd('\\');
                    }
                    installDir += @"\CodeGeneration\Bin\Interop.ADODB64X.dll";

                    try
                    {
                        File.Delete(installDir);
                    }
                    catch { }
                }
            }
            catch { }

            try
            {
                string offlinePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                offlinePath += @"\EntitySpaces\ES2011\Interop.ADODB64X.dll";

                File.Delete(offlinePath);
            }
            catch { }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            proxySettings.UseProxy = this.chkUseProxyServer.Checked;
            proxySettings.Url = this.txtProxyServerURL.Text;
            proxySettings.UserName = this.txtProxyServerUserName.Text;
            proxySettings.Password = this.txtProxyServerPassword.Text;
            proxySettings.DomainName = this.txtProxyServerDomainName.Text;

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2012", true);

            string installDir = (string)key.GetValue("Profiler_Install_Dir");
            if (installDir.EndsWith("\\"))
            {
                installDir = installDir.TrimEnd('\\');
            }

            proxySettings.Save();

            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}