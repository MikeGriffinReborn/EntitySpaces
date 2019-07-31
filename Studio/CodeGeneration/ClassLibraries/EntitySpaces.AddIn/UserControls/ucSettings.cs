using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.Win32;

using EntitySpaces.MetadataEngine;
using System.Xml.Serialization;

namespace EntitySpaces.AddIn
{
    internal partial class ucSettings : esUserControl
    {
        private Root root;
        private MostRecentlyUsedList mru;

        public ucSettings()
        {
            if (!this.DesignMode)
            {
                InitializeComponent();
            }
        }

        public esSettings Settings 
        {
            get { return settings; }
            set { settings = value; }
        }
        private esSettings settings;


        private void ShowError(Exception ex)
        {
            if (MainWindow != null)
            {
                MainWindow.ShowError(ex);
            }
        }

        private void HideErrorOrStatusMessage()
        {
            if (MainWindow != null)
            {
                MainWindow.HideErrorOrStatusMessage();
            }
        }

        private void NofityControlsThatSettingsChanged()
        {
            if (MainWindow != null)
            {
                MainWindow.NofityControlsThatSettingsChanged();
            }
        }

        private void ucSettings_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                Cursor oldCursor = Cursor.Current;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    this.mru = new MostRecentlyUsedList();

                    comboBoxDriver.Items.Add(new DictionaryEntry("<None>", null));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("Access"), "Access"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("MySql"), "MySql"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("Oracle"), "Oracle"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("PostgreSQL"), "PostgreSQL"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("SQL"), "SQL"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("SQLAzure"), "SQLAzure"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("SQLCE"), "SQLCE"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("SQLite"), "SQLite"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("Sybase"), "Sybase"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("VistaDB"), "VistaDB"));
                    comboBoxDriver.Items.Add(new DictionaryEntry(Settings.DriverName("VistaDB4"), "VistaDB4"));

                    comboBoxDriver.DisplayMember = "Key";
                    comboBoxDriver.ValueMember = "Value";

                    comboBoxDriver.Focus();

                    PopulateUI();
                    SetOleDbButton();

                    DictionaryEntry de = (DictionaryEntry)comboBoxDriver.SelectedItem;
                    this.lblSelectedDriver.Text = (string)de.Value;

                    LoadMruList();
                }
                catch (Exception ex)
                {
                    this.ShowError(ex);
                }
                finally
                {
                    Cursor.Current = oldCursor;
                }
            }
        }

        public void PopulateUI()
        {
            try
            {
                // Connection
                comboBoxDriver.SelectedIndex = comboBoxDriver.FindStringExact(Settings.DriverName(Settings.Driver));
                textBoxConnectionString.Text = Settings.ConnectionString;

                // File Locations
                textBoxTemplatePath.Text = Settings.TemplatePath;
                textBoxOutputPath.Text = Settings.OutputPath;
                textBoxUIAssemblyPath.Text = Settings.UIAssemblyPath;
                textBoxCompilerAssemblyPath.Text = Settings.CompilerAssemblyPath;
                textBoxLanguageMap.Text = Settings.LanguageMappingFile;
                textBoxUserMetadata.Text = Settings.UserMetadataFile;

                // Class Names
                textBoxAbstractPrefix.Text = Settings.AbstractPrefix;
                textBoxEntitySuffix.Text = Settings.EntitySuffix;
                textBoxCollectionSuffix.Text = Settings.CollectionSuffix;
                textBoxQuerySuffix.Text = Settings.QuerySuffix;
                textBoxMetadataSuffix.Text = Settings.MetadataSuffix;
                textBoxProxyStubSuffix.Text = Settings.ProxyStubSuffix;
                checkboxPrefixWithSchema.Checked = Settings.PrefixWithSchema;

                // Stored Procedure Names
                textBoxProcPrefix.Text = Settings.ProcPrefix;
                textBoxProcInsert.Text = Settings.ProcInsert;
                textBoxProcUpdate.Text = Settings.ProcUpdate;
                textBoxProcDelete.Text = Settings.ProcDelete;
                textBoxProcLoadAll.Text = Settings.ProcLoadAll;
                textBoxProcLoadByPK.Text = Settings.ProcLoadByPK;
                textBoxProcSuffix.Text = Settings.ProcSuffix;
                checkBoxProcVerbFirst.Checked = Settings.ProcVerbFirst;

                BuildSampleProcs();

                // Hierarchical Names
                textBoxOnePrefix.Text = Settings.OnePrefix;
                textBoxOneSeparator.Text = Settings.OneSeparator;
                textBoxOneSuffix.Text = Settings.OneSuffix;
                textBoxManyPrefix.Text = Settings.ManyPrefix;
                textBoxManySeparator.Text = Settings.ManySeparator;
                textBoxManySuffix.Text = Settings.ManySuffix;
                checkBoxSelfOnly.Checked = Settings.SelfOnly;
                checkBoxSwapNames.Checked = Settings.SwapNames;
                checkBoxUseAssociativeName.Checked = Settings.UseAssociativeName;
                checkBoxUseUpToPrefix.Checked = Settings.UseUpToPrefix;

                // Miscellaneous
                checkBoxPreserveUnderscores.Checked = Settings.PreserveUnderscores;
                checkBoxUseRawNames.Checked = Settings.UseRawNames;

                // License
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", true);
                if (key != null)
                {
                    textBoxSerialNumber.Text = (string)key.GetValue("Serial_Number");
                }

                // Other
                checkBoxUseNullableTypes.Checked = Settings.UseNullableTypesAlways;
                checkBoxNoDatesInHeader.Checked = Settings.TurnOffDateTimeInClassHeaders;

                if (Settings.DefaultTemplateDoubleClickAction == "Edit")
                    radioButtonEditTemplate.Checked = true;
                if (Settings.DefaultTemplateDoubleClickAction == "Execute")
                    radioButtonExecute.Checked = true;
                if (Settings.DefaultTemplateDoubleClickAction == "ExecuteWithLastSettings")
                    radioButtonExecuteWithLastSettings.Checked = true;

                // Licensing
                chkUseProxyServer.Checked = Settings.LicenseProxyEnable;
                txtProxyServerURL.Text = Settings.LicenseProxyUrl;
                txtProxyServerUserName.Text = Settings.LicenseProxyUserName;
                txtProxyServerPassword.Text = Settings.LicenseProxyPassword;
                txtProxyServerDomainName.Text = Settings.LicenseProxyDomainName;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        public void PopulateSettings()
        {
            try
            {
                // Connection
                DictionaryEntry de = (DictionaryEntry)comboBoxDriver.SelectedItem;
                Settings.Driver = (string)de.Value;
                Settings.ConnectionString = textBoxConnectionString.Text;

                esSettingsDriverInfo driverInfo = Settings.FindDriverInfoCollection(Settings.Driver);

                if (driverInfo == null)
                {
                    driverInfo = new esSettingsDriverInfo();
                    driverInfo.Driver = Settings.Driver;
                    Settings.DriverInfoCollection.Add(driverInfo);
                }

                // Let's pull thier previous information
                driverInfo.ConnectionString = Settings.ConnectionString;

                driverInfo.DateAdded.IsEnabled = chkDateAddedEnabled.Checked;
                driverInfo.DateAdded.ColumnName = txtDateAddedColumnName.Text;
                driverInfo.DateAdded.ServerSideText = txtDateAddedServerSideText.Text;
                driverInfo.DateAdded.Type = chkDateAddedClientSideEnabled.Checked ?
                    esSettingsDriverInfo.DateType.ClientSide : esSettingsDriverInfo.DateType.ServerSide;
                driverInfo.DateAdded.ClientType = rdoDateAddedClientSideNow.Checked ?
                    esSettingsDriverInfo.ClientType.Now : esSettingsDriverInfo.ClientType.UtcNow;

                driverInfo.DateModified.IsEnabled = chkDateModifiedEnabled.Checked;
                driverInfo.DateModified.ColumnName = txtDateModifiedColumnName.Text;
                driverInfo.DateModified.ServerSideText = txtDateModifiedServerSideText.Text;
                driverInfo.DateModified.Type = chkDateModifiedClientSideEnabled.Checked ?
                    esSettingsDriverInfo.DateType.ClientSide : esSettingsDriverInfo.DateType.ServerSide;
                driverInfo.DateModified.ClientType = rdoDateModifiedClientSideNow.Checked ?
                    esSettingsDriverInfo.ClientType.Now : esSettingsDriverInfo.ClientType.UtcNow;

                driverInfo.AddedBy.IsEnabled = chkAddedByEnabled.Checked;
                driverInfo.AddedBy.UseEventHandler = chkAddedByEventHandler.Checked;
                driverInfo.AddedBy.ColumnName = txtAddedByColumnName.Text;
                driverInfo.AddedBy.ServerSideText = txtAddedByServerSideText.Text;

                driverInfo.ModifiedBy.IsEnabled = chkModifiedByEnabled.Checked;
                driverInfo.ModifiedBy.UseEventHandler = chkModifiedByEventHandler.Checked;
                driverInfo.ModifiedBy.ColumnName = txtModifiedByColumnName.Text;
                driverInfo.ModifiedBy.ServerSideText = txtModifiedByServerSideText.Text;

                driverInfo.ConcurrencyColumnEnabled = chkConcurrencyColumn.Checked;
                driverInfo.ConcurrencyColumn = txtConcurrencyColumnName.Text;
               

                // File Locations
                Settings.TemplatePath = textBoxTemplatePath.Text;
                Settings.OutputPath = textBoxOutputPath.Text;
                Settings.UIAssemblyPath = textBoxUIAssemblyPath.Text;
                Settings.CompilerAssemblyPath = textBoxCompilerAssemblyPath.Text;
                Settings.LanguageMappingFile = textBoxLanguageMap.Text;
                Settings.UserMetadataFile = textBoxUserMetadata.Text;

                // Class Names
                Settings.AbstractPrefix = textBoxAbstractPrefix.Text;
                Settings.EntitySuffix = textBoxEntitySuffix.Text;
                Settings.CollectionSuffix = textBoxCollectionSuffix.Text;
                Settings.QuerySuffix = textBoxQuerySuffix.Text;
                Settings.MetadataSuffix = textBoxMetadataSuffix.Text;
                Settings.ProxyStubSuffix = textBoxProxyStubSuffix.Text;

                // Stored Procedure Names
                Settings.ProcPrefix = textBoxProcPrefix.Text;
                Settings.ProcInsert = textBoxProcInsert.Text;
                Settings.ProcUpdate = textBoxProcUpdate.Text;
                Settings.ProcDelete = textBoxProcDelete.Text;
                Settings.ProcLoadAll = textBoxProcLoadAll.Text;
                Settings.ProcLoadByPK = textBoxProcLoadByPK.Text;
                Settings.ProcSuffix = textBoxProcSuffix.Text;
                Settings.ProcVerbFirst = checkBoxProcVerbFirst.Checked;
                Settings.PrefixWithSchema = checkboxPrefixWithSchema.Checked;

                // Hierarchical Names
                Settings.OnePrefix = textBoxOnePrefix.Text;
                Settings.OneSeparator = textBoxOneSeparator.Text;
                Settings.OneSuffix = textBoxOneSuffix.Text;
                Settings.ManyPrefix = textBoxManyPrefix.Text;
                Settings.ManySeparator = textBoxManySeparator.Text;
                Settings.ManySuffix = textBoxManySuffix.Text;
                Settings.SelfOnly = checkBoxSelfOnly.Checked;
                Settings.SwapNames = checkBoxSwapNames.Checked;
                Settings.UseAssociativeName = checkBoxUseAssociativeName.Checked;
                Settings.UseUpToPrefix = checkBoxUseUpToPrefix.Checked;

                // Miscellaneous
                Settings.PreserveUnderscores = checkBoxPreserveUnderscores.Checked;
                Settings.UseRawNames = checkBoxUseRawNames.Checked;

                // Other
                Settings.UseNullableTypesAlways = checkBoxUseNullableTypes.Checked;
                Settings.TurnOffDateTimeInClassHeaders = checkBoxNoDatesInHeader.Checked;

                if (radioButtonEditTemplate.Checked)
                    Settings.DefaultTemplateDoubleClickAction = "Edit";
                if (radioButtonExecute.Checked)
                    Settings.DefaultTemplateDoubleClickAction = "Execute";
                if (radioButtonExecuteWithLastSettings.Checked)
                    Settings.DefaultTemplateDoubleClickAction = "ExecuteWithLastSettings";

                // Licensing
                Settings.LicenseProxyEnable = chkUseProxyServer.Checked;
                Settings.LicenseProxyUrl = txtProxyServerURL.Text;
                Settings.LicenseProxyUserName = txtProxyServerUserName.Text;
                Settings.LicenseProxyPassword = txtProxyServerPassword.Text;
                Settings.LicenseProxyDomainName = txtProxyServerDomainName.Text;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void SetOleDbButton()
        {
            try
            {
                DictionaryEntry de = (DictionaryEntry)comboBoxDriver.SelectedItem;
                string driver = (string)de.Value;

                if (String.IsNullOrEmpty(driver))
                {
                    buttonOleDB.Enabled = false;
                    return;
                }

                switch (driver.ToUpper())
                {
                    case "SQL":
                    case "ORACLE":
                    case "ACCESS":
                        buttonOleDB.Enabled = true;
                        break;

                    default:
                        buttonOleDB.Enabled = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void comboBoxDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideErrorOrStatusMessage();

                if (comboBoxDriver.SelectedIndex != -1)
                {
                    DictionaryEntry de = (DictionaryEntry)comboBoxDriver.SelectedItem;
                    string driver = (string)de.Value;

                    lblSelectedDriver.Text = driver;

                    esSettingsDriverInfo driverInfo = Settings.FindDriverInfoCollection(driver);

                    if (driverInfo == null)
                    {
                        // There is no entry fro this driver
                        driverInfo = new esSettingsDriverInfo();
                        driverInfo.HasConnected = false;
                        driverInfo.Driver = driver;
                        Settings.DriverInfoCollection.Add(driverInfo);

                        textBoxConnectionString.Text = esSettings.GetDefaultConnectionString(driver);

                        // DateAdded
                        chkDateAddedEnabled.Checked = false;
                        txtDateAddedColumnName.Text = "DateAdded";
                        txtDateAddedServerSideText.Text = string.Empty;
                        chkDateAddedClientSideEnabled.Checked = true;
                        chkDateAddedClientSideEnabled.Checked = false;
                        rdoDateAddedClientSideNow.Checked = true;

                        // DateModified
                        chkDateModifiedEnabled.Checked = false;
                        txtDateModifiedColumnName.Text = "DateModified";
                        txtDateModifiedServerSideText.Text = string.Empty;
                        chkDateModifiedServerSideEnabled.Checked = true;
                        chkDateModifiedClientSideEnabled.Checked = false;
                        rdoDateModifiedClientSideNow.Checked = true;

                        txtConcurrencyColumnName.Text = string.Empty;
                    }
                    else
                    {
                        // Let's pull thier previous information
                        textBoxConnectionString.Text = driverInfo.ConnectionString;

                        // DateAdded
                        chkDateAddedEnabled.Checked = driverInfo.DateAdded.IsEnabled;
                        txtDateAddedColumnName.Text = driverInfo.DateAdded.ColumnName;
                        txtDateAddedServerSideText.Text = driverInfo.DateAdded.ServerSideText;
                        chkDateAddedClientSideEnabled.Checked =
                            driverInfo.DateAdded.Type == esSettingsDriverInfo.DateType.ServerSide ? true : false;
                        chkDateAddedClientSideEnabled.Checked =
                            driverInfo.DateAdded.Type == esSettingsDriverInfo.DateType.ServerSide ? false : true;
                        rdoDateAddedClientSideNow.Checked =
                            driverInfo.DateAdded.ClientType == esSettingsDriverInfo.ClientType.Now ? true : false;
                        rdoDateAddedClientSideUtcNow.Checked =
                            driverInfo.DateAdded.ClientType == esSettingsDriverInfo.ClientType.Now ? false : true;

                        // DateModified
                        chkDateModifiedEnabled.Checked = driverInfo.DateModified.IsEnabled;
                        txtDateModifiedColumnName.Text = driverInfo.DateModified.ColumnName;
                        txtDateModifiedServerSideText.Text = driverInfo.DateModified.ServerSideText;
                        chkDateModifiedServerSideEnabled.Checked =
                            driverInfo.DateModified.Type == esSettingsDriverInfo.DateType.ServerSide ? true : false;
                        chkDateModifiedClientSideEnabled.Checked =
                            driverInfo.DateModified.Type == esSettingsDriverInfo.DateType.ServerSide ? false : true;
                        rdoDateModifiedClientSideNow.Checked =
                            driverInfo.DateModified.ClientType == esSettingsDriverInfo.ClientType.Now ? true : false;
                        rdoDateModifiedClientSideUtcNow.Checked =
                            driverInfo.DateModified.ClientType == esSettingsDriverInfo.ClientType.Now ? false : true;

                        chkAddedByEnabled.Checked = driverInfo.AddedBy.IsEnabled;
                        chkAddedByEventHandler.Checked = driverInfo.AddedBy.UseEventHandler;
                        txtAddedByColumnName.Text = driverInfo.AddedBy.ColumnName;
                        txtAddedByServerSideText.Text = driverInfo.AddedBy.ServerSideText;

                        chkModifiedByEnabled.Checked = driverInfo.ModifiedBy.IsEnabled;
                        chkModifiedByEventHandler.Checked = driverInfo.ModifiedBy.UseEventHandler;
                        txtModifiedByColumnName.Text = driverInfo.ModifiedBy.ColumnName;
                        txtModifiedByServerSideText.Text = driverInfo.ModifiedBy.ServerSideText;

                        chkConcurrencyColumn.Checked = driverInfo.ConcurrencyColumnEnabled;
                        txtConcurrencyColumnName.Text = driverInfo.ConcurrencyColumn;

                    }

                    textBoxConnectionString.Focus();
                    SetOleDbButton();

                    textBoxConnectionHelp.Text = "";

                    if (String.IsNullOrEmpty(driver))
                    {
                        textBoxConnectionHelp.Text = "Select a driver from the dropdown above.";
                    }
                    else
                    {
                        switch (driver.ToLower())
                        {
                            case "access":
                                textBoxConnectionHelp.Text = "EntitySpaces uses OLEDB to pull the metadata from your database during the code generation process. You do not use OLEDB in your EntitySpaces application.";
                                textBoxConnectionHelp.Text += " If you encounter ADODB errors with the EntitySpaces Visual Studio Add-In, use the EntitySpaces StandAlone code generator instead.";
                                textBoxConnectionHelp.Text += " You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "sql":
                            case "oracle":
                                textBoxConnectionHelp.Text = "EntitySpaces uses OLEDB to pull the metadata from your database during the code generation process. You do not use OLEDB in your EntitySpaces application. You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "sqlazure":
                                textBoxConnectionHelp.Text = "EntitySpaces uses SqlClient to pull the metadata from your Azure database during the code generation process. You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "sqlce":
                                textBoxConnectionHelp.Text = "Because there are so many versions of the SqlCe ADO.NET provider, the EntitySpaces metadata engine requires you to build the version into the connection string (as shown below). You can determine the version by looking in your Global Assembly Cache (GAC).";
                                textBoxConnectionHelp.Text += "\r\n\r\nSqlCe 3.0 - Version=\"9.0.242.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91\"";
                                textBoxConnectionHelp.Text += "\r\nSqlCe 3.5 - Version=\"3.5.1.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91\"";
                                textBoxConnectionHelp.Text += " You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "postgresql":
                                textBoxConnectionHelp.Text = "The EntitySpaces Metadata Engine uses reflection to load Npgsql, you can either install a copy in the GAC or in the '" + Settings.InstallPath + "CodeGeneration\\Bin' folder. You must have a version in the GAC to use the Visual Studio Plugin, otherwise you will have to run the StandAlone application located on your Start -> Programs menu. You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "mysql":
                                textBoxConnectionHelp.Text = "The EntitySpaces Metadata Engine uses reflection to load the MySQL Connector/NET provider. You can either install a copy in the GAC or in the '" + Settings.InstallPath + "CodeGeneration\\Bin' folder. You must have a version in the GAC to use the Visual Studio Plugin, otherwise you will have to run the StandAlone application located on your Start -> Programs menu. You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "sqlite":
                                textBoxConnectionHelp.Text = "You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "sybase":
                                textBoxConnectionHelp.Text = "You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "effiprozdb":
                                textBoxConnectionHelp.Text = "You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "vistadb":
                                textBoxConnectionHelp.Text = "The EntitySpaces MetadataEngine PlugIn for VistaDB is bound to VistaDB version '3.5.1.84'.";
                                textBoxConnectionHelp.Text += " You can recompile the PlugIn if you are using a different version. It is located in your '" + Settings.InstallPath + "CodeGeneration\\EntitySpaces.MetadataEngine.VistaDB' folder. After you recompile it put the assembly in the '" + Settings.InstallPath + "CodeGeneration\\Bin' folder.";
                                textBoxConnectionHelp.Text += " You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            case "vistadb4":
                                textBoxConnectionHelp.Text = "The EntitySpaces MetadataEngine PlugIn for VistaDB is bound to VistaDB version '4.0.0.0'.";
                                textBoxConnectionHelp.Text += " You can recompile the PlugIn if you are using a different version. It is located in your '" + Settings.InstallPath + "CodeGeneration\\EntitySpaces.MetadataEngine.VistaDB4' folder. After you recompile it put the assembly in the '" + Settings.InstallPath + "CodeGeneration\\Bin' folder.";
                                textBoxConnectionHelp.Text += " You can save your settings as the default by clicking the 'Save Default Settings' icon on the toolbar.";
                                break;

                            default:
                                textBoxConnectionHelp.Text = "Select a driver from the dropdown above.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void toolStripButtonSetDefault_Click(object sender, EventArgs e)
        {
            Cursor oldCursor = Cursor.Current;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                PopulateSettings();
                Settings.Save();
                NofityControlsThatSettingsChanged();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            finally
            {
                Cursor.Current = oldCursor;
            }
        }

        private void toolStripButtonReloadDefault_Click(object sender, EventArgs e)
        {
            Cursor oldCursor = Cursor.Current;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Settings = esSettings.Load();
                PopulateUI();
                NofityControlsThatSettingsChanged();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            finally
            {
                Cursor.Current = oldCursor;
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            Cursor oldCursor = Cursor.Current;

            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save Settings File";
                saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    PopulateSettings();
                    Settings.Save(saveFileDialog1.FileName);
                    NofityControlsThatSettingsChanged();

                    mru.Push(saveFileDialog1.FileName);
                    SaveMruList();
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            finally
            {
                Cursor.Current = oldCursor;
            }
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            Cursor oldCursor = Cursor.Current;

            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Open Settings File";
                openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    OpenSettingsFile(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            finally
            {
                Cursor.Current = oldCursor;
            }
        }

        private void OpenSettingsFile(string filename)
        {
            Cursor oldCursor = Cursor.Current;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                this.MainWindow.Settings = Settings = esSettings.Load(filename);

                PopulateUI();
                NofityControlsThatSettingsChanged();

                mru.Push(filename);
                SaveMruList();
            }
            catch (Exception ex)
            {
                mru.Remove(filename);
                this.SaveMruList();
                this.ShowError(ex);
            }
            finally
            {
                Cursor.Current = oldCursor;
            }
        }

        private void buttonOleDB_Click(object sender, EventArgs e)
        {
            try
            {
                string driver = string.Empty;
                string connstr = string.Empty;

                DictionaryEntry de = (DictionaryEntry)comboBoxDriver.SelectedItem;
                if (null != de.Key)
                {
                    driver = (string)de.Value;
                }
                connstr = this.textBoxConnectionString.Text;

                if (String.IsNullOrEmpty(driver))
                {
                    throw new Exception("You Must Choose a Driver");
                }

                driver = driver.ToUpper();

                if (string.Empty == connstr)
                {
                    connstr = esSettings.GetDefaultConnectionString(driver);
                }

                Type adoType = Type.GetTypeFromProgID("ADODB.Connection");
                if (adoType == null) return;
                object oConn = Activator.CreateInstance(adoType);
                            
                object ret = adoType.InvokeMember("ConnectionString", BindingFlags.SetProperty, null, oConn,
                    new object[] { connstr });

                Type dataLinkType = Type.GetTypeFromProgID("DataLinks");
                if (dataLinkType == null) return;
                object oDialog = Activator.CreateInstance(dataLinkType);

                bool ok = (bool)dataLinkType.InvokeMember("PromptEdit", BindingFlags.InvokeMethod, null, oDialog, new object[] { oConn });

                if (ok)
                {
                    string connString = (string)adoType.InvokeMember("ConnectionString", BindingFlags.GetProperty, null, oConn, null);

                    Root.UnLoadPlugins();
                    Root tempRoot = new Root(Settings);
                    if (tempRoot.Connect(driver, connString))
                    {
                        this.textBoxConnectionString.Text = connString;

                        this.root = new Root(Settings);
                        this.root.Connect(driver, connString);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            catch { }
        }

        private void buttonTestConnection_Click(object sender, EventArgs e)
        {
            Cursor oldCursor = Cursor.Current;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Root tempRoot = new Root(Settings);
                Root.UnLoadPlugins();

                DictionaryEntry de = (DictionaryEntry)comboBoxDriver.SelectedItem;
                string driver = (string)de.Value;

                if (!String.IsNullOrEmpty(driver) &&
                    this.textBoxConnectionString.Text != String.Empty &&
                    tempRoot.Connect(driver, this.textBoxConnectionString.Text))
                {
                    esSettingsDriverInfo driverInfo = Settings.FindDriverInfoCollection(driver);
                    if (driverInfo != null)
                    {
                        driverInfo.HasConnected = true;
                    }
                    HideErrorOrStatusMessage();
                    MessageBox.Show("Connection Successful");
                }
                else
                {
                    throw new Exception("Unable to Connect");
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            finally
            {
                Cursor.Current = oldCursor;
            }
        }

        private void buttonAssign_Click(object sender, EventArgs e)
        {
            string originalKey = null;

            try
            {
                string serialNum = textBoxSerialNumber.Text != null ? textBoxSerialNumber.Text.Trim() : "";
                Guid serialNumber = new Guid(serialNum);
            }
            catch
            {
                MessageBox.Show("Invalid Serial Number");
                return;
            }

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", true);
            if (key != null)
            {
                originalKey = (string)key.GetValue("Serial_Number");
                key.SetValue("Serial_Number", textBoxSerialNumber.Text.Trim());
            }

            try
            {
//#if TRIAL
//                Licensing licensing = new Licensing();
//                string id = licensing.getUniqueID("C");

//                if (1 != licensing.ValidateLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, MainWindow.esVersion, GetProxySettings(Settings)))
//                {
//                    return;
//                }
//#else
//                if (MainWindow != null)
//                {
//                    Licensing licensing = new Licensing();
//                    licensing.ReplaceMeLater("developer", MainWindow.esVersion, "Serial_Number", "Serial_Number2", "Interop.ADODBX.dll", GetProxySettings(Settings));
//                }
//#endif

                MessageBox.Show("You must 'close' all instances of Visual Studio or the Stand Alone version and restart for changes to take place.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to Validate License");

                if (originalKey != null && originalKey.Length > 0)
                {
                    key.SetValue("Serial_Number", originalKey);
                }
            }
        }

        private void buttonTemplatePath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                folderBrowserDialog1.Description = "Select the Template Folder";
                folderBrowserDialog1.SelectedPath = textBoxTemplatePath.Text;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBoxTemplatePath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void buttonOutputPath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                folderBrowserDialog1.Description = "Select the Output Folder";
                folderBrowserDialog1.SelectedPath = textBoxOutputPath.Text;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBoxOutputPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void buttonUIAssemblyPath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                folderBrowserDialog1.Description = "Select the UI Assemblies Folder";
                folderBrowserDialog1.SelectedPath = textBoxUIAssemblyPath.Text;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBoxUIAssemblyPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void buttonCompilerAssemblyPath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                folderBrowserDialog1.Description = "Select the Compiler Assemblies Folder";
                folderBrowserDialog1.SelectedPath = textBoxCompilerAssemblyPath.Text;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBoxCompilerAssemblyPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void buttonLanguageMap_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.FileName = textBoxLanguageMap.Text;
                openFileDialog1.Title = "Select a Language Mapping File";
                openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBoxLanguageMap.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void buttonUserMetadata_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = textBoxUserMetadata.Text;
                openFileDialog1.Title = "Select a User Metadata File";
                openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBoxUserMetadata.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void ucSettings_Leave(object sender, EventArgs e)
        {
            try
            {
                PopulateSettings();
                NofityControlsThatSettingsChanged();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void ProcTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            BuildSampleProcs();
        }

        private void checkBoxProcVerbFirst_CheckedChanged(object sender, EventArgs e)
        {
            BuildSampleProcs();
        }

        private void BuildSampleProcs()
        {
            string sample = String.Empty;

            sample += BuildSampleProc(this.textBoxProcInsert);
            sample += BuildSampleProc(this.textBoxProcUpdate);
            sample += BuildSampleProc(this.textBoxProcDelete);
            sample += BuildSampleProc(this.textBoxProcLoadAll);
            sample += BuildSampleProc(this.textBoxProcLoadByPK);

            this.labelProcSample.Text = sample;
        }

        private string BuildSampleProc(TextBox textbox)
        {
            string sample = String.Empty;

            sample += this.textBoxProcPrefix.Text;

            if (!this.checkBoxProcVerbFirst.Checked)
            {
                sample += "Employees";
                sample += textbox.Text;
            }
            else
            {
                sample += textbox.Text;
                sample += "Employees";
            }

            sample += this.textBoxProcSuffix.Text;

            sample += Environment.NewLine;

            return sample;
        }

        private void HierarchicalTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            BuildSampleHierarchical();
        }

        private void checkBoxSelfOnly_CheckedChanged(object sender, EventArgs e)
        {
            BuildSampleHierarchical();
        }

        private void checkBoxSwapNames_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSwapNames.Checked)
            {
                labelHierTable1.Text = "Column";
                labelHierTable2.Text = "Column";
                labelHierColumn1.Text = "Table";
                labelHierColumn2.Text = "Table";
            }
            else
            {
                labelHierTable1.Text = "Table";
                labelHierTable2.Text = "Table";
                labelHierColumn1.Text = "Column";
                labelHierColumn2.Text = "Column";
            }

            BuildSampleHierarchical();
        }

        private void checkBoxUseAssociativeName_CheckedChanged(object sender, EventArgs e)
        {
            BuildSampleHierarchical();
        }

        private void checkBoxUseUpToPrefix_CheckedChanged(object sender, EventArgs e)
        {
            BuildSampleHierarchical();
        }

        private void BuildSampleHierarchical()
        {
            string sampleTypes = String.Empty;

            sampleTypes += " ManyToOne:" + Environment.NewLine;
            sampleTypes += " ZeroToMany:" + Environment.NewLine;

            this.labelHierTypes.Text = sampleTypes;

            string sample = String.Empty;

            sample += BuildSampleManyToOne();
            sample += BuildSampleZeroToMany();
            //sample += BuildSampleManyToMany();
            //sample += BuildSampleOneParent();
            //sample += BuildSampleOneChild();

            this.labelHierSample.Text = sample;
        }

        private string BuildSampleManyToOne()
        {
            string sample = String.Empty;
            string table = "Employees";
            string column = "ReportsTo";

            sample += "emp.";

            if (this.checkBoxUseUpToPrefix.Checked)
            {
                sample += "UpTo";
            }

            sample += this.textBoxOnePrefix.Text;

            if (this.checkBoxSwapNames.Checked)
            {
                sample += column;
            }
            else
            {
                sample += table;
            }

            sample += this.textBoxOneSuffix.Text;
            sample += this.textBoxOneSeparator.Text;

            if (this.checkBoxSwapNames.Checked)
            {
                sample += table;
            }
            else
            {
                sample += column;
            }

            sample += "()";

            sample += Environment.NewLine;

            return sample;
        }

        private string BuildSampleZeroToMany()
        {
            string sample = String.Empty;
            string table = "Employees";
            string column = "ReportsTo";

            sample += "emp.";
            sample += this.textBoxManyPrefix.Text;

            if (this.checkBoxSwapNames.Checked)
            {
                sample += column;
            }
            else
            {
                sample += table;
            }

            sample += this.textBoxManySuffix.Text;
            sample += this.textBoxManySeparator.Text;

            if (this.checkBoxSwapNames.Checked)
            {
                sample += table;
            }
            else
            {
                sample += column;
            }

            sample += "()";

            sample += Environment.NewLine;

            return sample;
        }

        private void LoadMruList()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", false))
            {
                if (key == null) return;

                mru.Load(key, "Settings_");
                PopulateMruMenu();
            }
        }

        private void SaveMruList()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", true))
            {
                if (key == null) return;

                mru.Save(key, "Settings_");
                PopulateMruMenu();
            }
        }

        private void PopulateMruMenu()
        {
            this.menuMRU.Items.Clear();

            foreach (string project in mru)
            {
                if (project == null) continue;

                try
                {
                    FileInfo info = new FileInfo(project);
                    ToolStripItem item = this.menuMRU.Items.Add(info.Name);
                    item.ToolTipText = project;
                    item.Image = Resource.check;
                }
                catch { }
            }
        }

        private void menuMRU_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                string settingsFile = e.ClickedItem.ToolTipText;
                this.mru.Remove(settingsFile);  // OpenProject will reinsert him at the top
                this.OpenSettingsFile(settingsFile);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", true);
                if (key != null)
                {
                    key.DeleteValue("Serial_Number2");

                    string installDir = (string)key.GetValue("Install_Dir");
                    if (installDir.EndsWith("\\"))
                    {
                        installDir = installDir.TrimEnd('\\');
                    }
                    installDir += @"\CodeGeneration\Bin\Interop.ADODBX.dll";

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
                offlinePath += @"\EntitySpaces\ES2019\Interop.ADODBX.dll";

                File.Delete(offlinePath);
            }
            catch { }

        }

        private void chkUseProxyServer_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = !txtProxyServerURL.Enabled;

            txtProxyServerURL.Enabled = enable;
            txtProxyServerUserName.Enabled = enable;
            txtProxyServerPassword.Enabled = enable;
            txtProxyServerDomainName.Enabled = enable;
        }

        private void chkDateAddedEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkDateAddedEnabled.Checked;

            this.txtDateAddedColumnName.Enabled = isChecked;
            this.chkDateAddedClientSideEnabled.Enabled = isChecked;
            this.chkDateAddedServerSideEnabled.Enabled = isChecked;
            this.txtDateAddedServerSideText.Enabled = isChecked;
            this.rdoDateAddedClientSideNow.Enabled = isChecked;
            this.rdoDateAddedClientSideUtcNow.Enabled = isChecked;
        }

        private bool inCheckChanged = false;

        private void chkDateAddedServerSideEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!inCheckChanged)
            {
                inCheckChanged = true;
                chkDateAddedClientSideEnabled.Checked = !chkDateAddedClientSideEnabled.Checked;
                inCheckChanged = false;
            }
        }

        private void chkDateAddedClientSideEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!inCheckChanged)
            {
                inCheckChanged = true;
                chkDateAddedServerSideEnabled.Checked = !chkDateAddedServerSideEnabled.Checked;
                inCheckChanged = false;
            }
        }

        private void chkDateModifiedEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkDateModifiedEnabled.Checked;

            this.txtDateModifiedColumnName.Enabled = isChecked;
            this.chkDateModifiedClientSideEnabled.Enabled = isChecked;
            this.chkDateModifiedServerSideEnabled.Enabled = isChecked;
            this.txtDateModifiedServerSideText.Enabled = isChecked;
            this.rdoDateModifiedClientSideNow.Enabled = isChecked;
            this.rdoDateModifiedClientSideUtcNow.Enabled = isChecked;
        }

        private void chkDateModifiedServerSideEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!inCheckChanged)
            {
                inCheckChanged = true;
                this.chkDateModifiedClientSideEnabled.Checked = !chkDateModifiedClientSideEnabled.Checked;
                inCheckChanged = false;
            }
        }

        private void chkDateModifiedClientSideEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!inCheckChanged)
            {
                inCheckChanged = true;
                chkDateModifiedServerSideEnabled.Checked = !chkDateModifiedClientSideEnabled.Checked;
                inCheckChanged = false;
            }
        }

        private void chkAddedByEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkAddedByEnabled.Checked;

            this.txtAddedByColumnName.Enabled = isChecked;
            this.chkAddedByServerSide.Enabled = isChecked;
            this.txtAddedByServerSideText.Enabled = isChecked;
            this.chkAddedByEventHandler.Enabled = isChecked;
        }

        private void chkModifiedByEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkModifiedByEnabled.Checked;

            this.txtModifiedByColumnName.Enabled = isChecked;
            this.chkModifiedByServerSide.Enabled = isChecked;
            this.txtModifiedByServerSideText.Enabled = isChecked;
            this.chkModifiedByEventHandler.Enabled = isChecked;
        }

        private void chkAddedByServerSide_CheckedChanged(object sender, EventArgs e)
        {
            if (!inCheckChanged)
            {
                inCheckChanged = true;
                this.chkAddedByEventHandler.Checked = !chkAddedByEventHandler.Checked;
                inCheckChanged = false;
            }
        }

        private void chkAddedByEventHandler_CheckedChanged(object sender, EventArgs e)
        {
            if (!inCheckChanged)
            {
                inCheckChanged = true;
                this.chkAddedByServerSide.Checked = !chkAddedByServerSide.Checked;
                inCheckChanged = false;
            }
        }

        private void chkModifiedByServerSide_CheckedChanged(object sender, EventArgs e)
        {
            if (!inCheckChanged)
            {
                inCheckChanged = true;
                this.chkModifiedByEventHandler.Checked = !chkModifiedByEventHandler.Checked;
                inCheckChanged = false;
            }
        }

        private void chkModifiedByEventHandler_CheckedChanged(object sender, EventArgs e)
        {
            if (!inCheckChanged)
            {
                inCheckChanged = true;
                this.chkModifiedByServerSide.Checked = !chkModifiedByServerSide.Checked;
                inCheckChanged = false;
            }
        }

        private void chkConcurrencyColumn_CheckedChanged(object sender, EventArgs e)
        {
            txtConcurrencyColumnName.Enabled = chkConcurrencyColumn.Checked;
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
