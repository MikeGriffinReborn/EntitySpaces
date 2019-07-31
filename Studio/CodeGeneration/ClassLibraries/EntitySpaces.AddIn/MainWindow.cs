using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Management;

using Microsoft.Win32;

using EntitySpaces;
using EntitySpaces.Common;
using EntitySpaces.CodeGenerator;
using EntitySpaces.MetadataEngine;

using EntitySpaces.AddIn;

namespace EntitySpaces.AddIn.ES2019
{
    public partial class MainWindow : UserControl
    {
        private Licensing licensing = new Licensing();
        private object applicationObject;
        private List<esUserControl> userControlCollection = new List<esUserControl>();
        private esSettings settings = new esSettings();
        internal string esVersion = "2019.1.0725.0";

        internal OnTemplateExecute OnTemplateExecuteCallback;
        internal OnTemplateCancel OnTemplateCancelCallback;
        internal TemplateDisplaySurface CurrentTemplateDisplaySurface;

        public MainWindow()
        {
            InitializeComponent();

            NotAConstructor();
        }

        internal void NotAConstructor()
        {
            try
            {
                if (!this.DesignMode)
                {
                    TemplateDisplaySurface.Initialize(this);

                    this.Settings = esSettings.Load();
                    esPlugIn plugin = new esPlugIn(settings);

//#if TRIAL
//                    Licensing licensing = new Licensing();
//                    string id = licensing.getUniqueID("C");

//                    bool canRunOffline = false;

//                    int result = licensing.ValidateLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, esVersion, GetProxySettings(Settings));

//                    if (1 != result)
//                    {
//                        result = licensing.RegisterLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, esVersion, GetProxySettings(Settings));
//                    }
//#else
//                    Crypto crypto = new Crypto();

//                    string id = licensing.getUniqueID("C");
//                    string serialNumber = "";
//                    bool canRunOffline = false;

//                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", true);
//                    if (key != null)
//                    {
//                        try
//                        {
//                            serialNumber = (string)key.GetValue("Serial_Number");
//                        }
//                        catch { }
//                    }


//                    string offlinePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
//                    offlinePath += @"\EntitySpaces\ES2019\Interop.ADODBX.dll";

//                    // See if we have registered our license
//                    int result = licensing.ValidateLicense("developer", serialNumber, System.Environment.MachineName, id, plugin.esVersion, GetProxySettings(Settings));

//                    switch (result)
//                    {
//                        case 0:

//                            // Try Registering it ...
//                            int newResult = licensing.RegisterLicense("developer", serialNumber, System.Environment.MachineName, id, plugin.esVersion, GetProxySettings(Settings));

//                            if (newResult == 1)
//                            {
//                                licensing.CreateSerialNumber2Key(key, "Serial_Number2", id, false);
//                                result = 1;
//                            }
//                            else 
//                            {
//                                result = 0;
//                            }
//                            break;

//                        case 1:

//                            licensing.CreateSerialNumber2Key(key, "Serial_Number2", id, false);
//                            try
//                            {
//                                File.Delete(offlinePath);
//                            }
//                            catch { }

//                            break;

//                        case -1:

//                            bool isOffLine = false;
//                            DateTime offLineDate = DateTime.MinValue;
//                            if (licensing.ReadSerialNumber2Key(key, "Serial_Number2", id, out isOffLine, out offLineDate))
//                            {
//                                if (isOffLine)
//                                {
//                                    if (File.Exists(offlinePath))
//                                    {
//                                        DateTime fileDate = licensing.OpenOfflineFile(offlinePath);

//                                        if (DateTime.Now > offLineDate)
//                                        {
//                                            TimeSpan ts = DateTime.Now.Subtract(offLineDate);
//                                            if (ts.Days < licensing.DaysTheyCanRunOffline)
//                                            {
//                                                if (fileDate < DateTime.Now)
//                                                {
//                                                    canRunOffline = true;
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    licensing.CreateSerialNumber2Key(key, "Serial_Number2", id, true);
//                                    licensing.CreateOfflineFile(offlinePath);
//                                    canRunOffline = true;
//                                }
//                            }
//                            break;
//                    }
//#endif 

                    this.ucSettings.Settings = this.Settings;

                    //if (result == 1 || (canRunOffline && result == -1))
                    //{
                    userControlCollection.Add(ucProjects);
                    userControlCollection.Add(ucTemplates);
                    userControlCollection.Add(ucMetadata);
                    userControlCollection.Add(ucMappings);

                    this.ucProjects.MainWindow = this;
                    this.ucTemplates.MainWindow = this;
                    this.ucMetadata.MainWindow = this;
                    this.ucMappings.MainWindow = this;
                    //}
                    //else
                    //{
                    //    this.tabControl.TabPages.Remove(this.tabProjects);
                    //    this.tabControl.TabPages.Remove(this.tabMetadata);
                    //    this.tabControl.TabPages.Remove(this.tabProjects);
                    //    this.tabControl.TabPages.Remove(this.tabTemplates);
                    //    this.tabControl.TabPages.Remove(this.tabLanguageMappings);
                    //}
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            finally
            {
                //userControlCollection.Add(ucWhatsNew);
                //this.ucWhatsNew.MainWindow = this;

                userControlCollection.Add(ucSettings);
                this.ucSettings.MainWindow = this;
            }
        }

        public object ApplicationObject
        {
            get { return applicationObject; }
            set { applicationObject = value; }
        }

        public esSettings Settings
        {
            get { return settings; }
            set 
            { 
                settings = value; 
            }
        }

        public void NofityControlsThatSettingsChanged()
        {
            try
            {
                Root.UnLoadPlugins();

                foreach (esUserControl control in this.userControlCollection)
                {
                    control.OnSettingsChanged();
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        public void ShowError(Exception ex)
        {
            try
            {
                string errorText = string.Empty;
                string callStack = string.Empty;

                CompilerException cex = ex as CompilerException;

                if (cex != null)
                {
                    foreach (CompilerError error in cex.Results.Errors)
                    {
                        errorText += "Error Found in " + cex.Template.Header.FullFileName + " on line " +
                            cex.Template.TemplateLineFromErrorLine(error.Line) +
                            Environment.NewLine + error.ErrorText + Environment.NewLine + Environment.NewLine;
                    }
                }
                else
                {
                    Exception rootCause = ex;

                    while (rootCause.InnerException != null)
                    {
                        if (rootCause.Equals(ex.InnerException)) break;

                        rootCause = ex.InnerException;
                    }

                    errorText = rootCause.Message;
                }

                this.splitContainer.Panel2Collapsed = false;
                this.pictureBoxError.Image = Resource.error;
                this.textBoxError.Text = errorText;
                this.textBoxError.Text += Environment.NewLine + Environment.NewLine;
                this.textBoxError.Text += ex.StackTrace;
                this.textBoxError.ScrollToCaret();
            }
            catch (Exception exx)
            {
                this.ShowError(exx);
            }
        }

        public void HideErrorOrStatusMessage()
        {
            try
            {
                this.splitContainer.Panel2Collapsed = true;
                this.textBoxError.Text = string.Empty;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        public void ShowStatusMessage(string message)
        {
            this.splitContainer.Panel2Collapsed = false;
            this.pictureBoxError.Image = Resource.info;

            this.textBoxError.Text = message;
            this.textBoxError.Text += Environment.NewLine + Environment.NewLine;
            this.textBoxError.ScrollToCaret();
        }

        public void ShowTemplateUIControl()
        {
            try
            {
                this.splitContainerTabControl.Panel1Collapsed = true;
                this.splitContainerTabControl.Panel2Collapsed = false;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void buttonExecuteTemplateOk_Click(object sender, EventArgs e)
        {
            Cursor origCursor = this.Cursor;

            try
            {
                if (this.OnTemplateExecuteCallback == null) return;

                this.HideErrorOrStatusMessage();

//#if TRIAL
//                Licensing license = new Licensing();
//                string id = license.getUniqueID("C");

//                if (1 != licensing.ValidateLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, "2019.1.0725.0", GetProxySettings(Settings)))
//                {
//                    return;
//                }
//#else
//                Licensing licensing = new Licensing();
//                licensing.ReplaceMeLater("developer", esVersion, "Serial_Number", "Serial_Number2", "Interop.ADODBX.dll", GetProxySettings(Settings));
//#endif

                this.Cursor = Cursors.WaitCursor;

                if (this.OnTemplateExecuteCallback(this.CurrentTemplateDisplaySurface))
                {
                    this.tabControlTemplateUI.TabPages.Clear();

                    this.splitContainerTabControl.Panel1Collapsed = false;
                    this.splitContainerTabControl.Panel2Collapsed = true;

                    OnTemplateExecuteCallback = null;
                    OnTemplateCancelCallback = null;
                    CurrentTemplateDisplaySurface = null;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            finally
            {
                this.Cursor = origCursor;
            }
        }

        private void buttonExecuteTemplateCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.OnTemplateExecuteCallback == null) return;

                this.OnTemplateCancelCallback(this.CurrentTemplateDisplaySurface);

                this.HideErrorOrStatusMessage();

                this.tabControlTemplateUI.TabPages.Clear();

                this.splitContainerTabControl.Panel1Collapsed = false;
                this.splitContainerTabControl.Panel2Collapsed = true;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            finally
            {
                OnTemplateExecuteCallback = null;
                OnTemplateCancelCallback = null;
                CurrentTemplateDisplaySurface = null;
            }
        }

        private void pictureBoxError_Click(object sender, EventArgs e)
        {
            try
            {
                this.splitContainer.Panel2Collapsed = true;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void TabPage_Enter(object sender, EventArgs e)
        {
            try
            {
                this.HideErrorOrStatusMessage();

                TabPage tabPage = sender as TabPage;

                if (tabPage.Name != "Projects")
                {
                    this.ucProjects.PromptForSave();
                }
            }
            catch { }
        }

        internal static ProxySettings GetProxySettings(esSettings settings)
        {
            ProxySettings proxy = new ProxySettings();
            proxy.UseProxy = settings.LicenseProxyEnable;
            if (proxy.UseProxy)
            {
                proxy.Url = settings.LicenseProxyUrl;
                proxy.UserName = settings.LicenseProxyUserName;
                proxy.Password = settings.LicenseProxyPassword;
                proxy.DomainName = settings.LicenseProxyDomainName;
            }

            return proxy;
        }
    }
}
