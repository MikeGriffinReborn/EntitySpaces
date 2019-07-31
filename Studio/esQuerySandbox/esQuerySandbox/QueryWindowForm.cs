using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using ActiproSoftware.SyntaxEditor;
using ActiproSoftware.SyntaxEditor.Addons.CSharp;

using gudusoft.gsqlparser;
using Microsoft.Win32;
using System.Resources;

using EntitySpaces.QuerySandbox.Properties;
using gudusoft.gsqlparser.Units;
 

namespace EntitySpaces.QuerySandbox
{
    public partial class QueryWindowForm : DevExpress.XtraEditors.XtraForm
    {
        private ReferenceList referenceList;
        private TGSqlParser parser = new TGSqlParser(TDbVendor.DbVMssql);
        private SyntaxEditor headerEditor = new SyntaxEditor();
        private SyntaxEditor footerEditor = new SyntaxEditor();
        private List<string> esAssemblies = new List<string>();

        public QueryWindowForm(ReferenceList referenceList)
        {
            InitializeComponent();

            this.Text = Resources.SelectAllExcept;

            this.cbxProviders.Properties.Items.Add("MSAccessProvider");
            this.cbxProviders.Properties.Items.Add("MySqlClientProvider");
            this.cbxProviders.Properties.Items.Add("Npgsql2Provider");
            this.cbxProviders.Properties.Items.Add("OracleClientProvider");
            this.cbxProviders.Properties.Items.Add("SqlClientProvider");
            this.cbxProviders.Properties.Items.Add("SQLiteProvider");
            this.cbxProviders.Properties.Items.Add("SqlServerCe4Provider");
            this.cbxProviders.Properties.Items.Add("SqlServerCeProvider");
            this.cbxProviders.Properties.Items.Add("SybaseSqlAnywhereProvider");
            this.cbxProviders.Properties.Items.Add("VistaDB4Provider");
            this.cbxProviders.Properties.Items.Add("VistaDBProvider");
            this.cbxProviders.SelectedIndex = 4;

            this.referenceList = referenceList;

            esAssemblies.Add("EntitySpaces.Core.dll");
            esAssemblies.Add("EntitySpaces.Interfaces.dll");
            esAssemblies.Add("EntitySpaces.DynamicQuery.dll");
            esAssemblies.Add("EntitySpaces.Loader.dll");
            esAssemblies.Add("EntitySpaces.MSAccessProvider.dll");
            esAssemblies.Add("EntitySpaces.MySqlClientProvider.dll");
            esAssemblies.Add("EntitySpaces.Npgsql2Provider.dll");
            esAssemblies.Add("EntitySpaces.OracleClientProvider.dll");
            esAssemblies.Add("EntitySpaces.SqlClientProvider.dll");
            esAssemblies.Add("EntitySpaces.SQLiteProvider.dll");
            esAssemblies.Add("EntitySpaces.SqlServerCe4Provider.dll");
            esAssemblies.Add("EntitySpaces.SqlServerCeProvider.dll");
            esAssemblies.Add("EntitySpaces.SybaseSqlAnywhereProvider.dll");
            esAssemblies.Add("EntitySpaces.VistaDB4Provider.dll");
            esAssemblies.Add("EntitySpaces.VistaDBProvider.dll");
            esAssemblies.Add("EntitySpaces.DebuggerVisualizer.dll");
          //esAssemblies.Add("EntitySpaces.Profiler.dll");
          //esAssemblies.Add("XDMessaging.dll");

            AssignHeaderEditorText();

            footerEditor.Document.Text = @"
    }
};";

            CSharpSyntaxLanguage cSharpLanguage = new CSharpSyntaxLanguage();
            syntaxEditor.Document.Filename = "Query.cs";
            syntaxEditor.Document.Language = cSharpLanguage;
            syntaxEditor.Document.LanguageData = referenceList.ProjectResolver;
            syntaxEditor.Document.HeaderText = headerEditor.Document.Text;
            syntaxEditor.Document.FooterText = footerEditor.Document.Text;

            syntaxEditor.LineNumberMarginVisible = true;
            syntaxEditor.LineNumberMarginWidth = 20;

            //define color of user customized token
            lzbasetype.gFmtOpt.HighlightingElements[(int)TLzHighlightingElement.sfkUserCustomized].SetForegroundInRGB("#FF00FF");

            foreach (var obj in lzbasetype.gFmtOpt.HighlightingElements)
            {
                if (obj.Foreground.ToString().ToLower() == "12632256")
                {
                    obj.SetForegroundInRGB("#000000");
                }
            }
            lzbasetype.gFmtOpt.AlignAliasInSelectList = false;
        }

        private void AssignHeaderEditorText()
        {
            headerEditor.Document.Text = @"using System;
using System.Windows.Forms;
using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;
using EntitySpaces.Core;";

            foreach (string theNamespace in txtNamespaces.Lines)
            {
                headerEditor.Document.Text += @"
using " + theNamespace + ";";
            }

            headerEditor.Document.Text += @"

public class MyClass
{
    static public object Execute()
    {";

            syntaxEditor.Document.HeaderText = headerEditor.Document.Text;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                txtSQL.Text = "";
                txtSQL.Rtf = "";
                txtSQL.Update();

                CSharpExecuter executer = new CSharpExecuter(referenceList);
                object[] result = executer.Execute(syntaxEditor.Text, txtConnectionString.Text, cbxProviders.Text, txtProviderMetadataKey.Text, this.txtNamespaces.Text, false);

                if (result != null)
                {
                    gridView1.Columns.Clear();
                    grid.DataSource = null;
                    grid.DataSource = result[0];
                    grid.RefreshDataSource();
                    grid.Update();

                    txtSQL.Text = (string)result[1];
                    SetParserSyntax(cbxProviders.Text);
                    parser.SqlText.Text = txtSQL.Text;
                    txtSQL.Rtf = parser.ToRTF(TOutputFmt.ofrtf);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReferences_Click(object sender, EventArgs e)
        {
            ReferencesForm form = new ReferencesForm(referenceList);
            DialogResult result = form.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void bnLoad_35_EsAssemblies_Click(object sender, EventArgs e)
        {
            string path = "";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\" + cbxEsVersion.Text, false);
            if (key != null)
            {
                path = (string)key.GetValue("Install_Dir");
            }

            path = Path.Combine(path, "Runtimes\\.NET 3.5");

            LoadEsAssemblies(path);
        }

        private void bnLoad_40_EsAssemblies_Click(object sender, EventArgs e)
        {
            string path = "";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\" + cbxEsVersion.Text, false);
            if (key != null)
            {
                path = (string)key.GetValue("Install_Dir");
            }

            path = Path.Combine(path, "Runtimes\\.NET 4.0");

            LoadEsAssemblies(path);
        }

        private void LoadEsAssemblies(string srcPath)
        {
            try
            {
                string dstPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                dstPath = dstPath.Replace("esQuerySandbox.exe", "");

                string currentAssemblyFileName = null;

                try
                {
                    foreach (string assemblyFileName in this.esAssemblies)
                    {
                        currentAssemblyFileName = assemblyFileName;

                        string sPath = Path.Combine(srcPath, assemblyFileName);
                        string dPath = Path.Combine(dstPath, assemblyFileName);

                        File.Delete(dPath);
                        File.Copy(sPath, dPath);

                        if (referenceList.AssemblyPaths.Contains(dPath))
                        {
                            referenceList.AssemblyPaths.Remove(dPath);
                            referenceList.ProjectResolver.RemoveExternalReference(Path.GetFileName(dPath));
                        }

                        referenceList.AssemblyPaths.Add(dPath);
                    }
                }
                catch (Exception ex)
                {

                }

                foreach (string assembly in referenceList.AssemblyPaths)
                {
                    try
                    {
                        referenceList.ProjectResolver.AddExternalReference(assembly);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetParserSyntax(string syntax)
        {
            try
            {
                switch (syntax)
                {
                    case "SqlClientProvider":
                    case "SqlServerCe4Provider":
                    case "SqlServerCeProvider":
                    case "VistaDB4Provider":
                    case "VistaDBProvider":

                        if (parser.DbVendor != TDbVendor.DbVMssql)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVMssql);
                        }
                        break;

                    case "Npgsql2Provider":
                    case "OracleClientProvider":

                        if (parser.DbVendor != TDbVendor.DbVOracle)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVOracle);
                        }
                        break;

                    case "MySqlClientProvider":

                        if (parser.DbVendor != TDbVendor.DbVMysql)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVMysql);
                        }
                        break;

                    //case "SybaseSqlAnywhereProvider":

                    //    if (parser.DbVendor != TDbVendor.DbVSybase)
                    //    {
                    //        parser = new TGSqlParser(TDbVendor.DbVSybase);
                    //    }
                    //    break;

                    case "MSAccessProvider":

                        if (parser.DbVendor != TDbVendor.DbVAccess)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVAccess);
                        }
                        break;

                    default:
                        parser = new TGSqlParser(TDbVendor.DbVGeneric);
                        break;
                }
            }
            catch { }
        }

        private void textEdit1_Enter(object sender, EventArgs e)
        {
            txtConnectionString.Properties.PasswordChar = '\0';
        }

        private void textEdit1_Leave(object sender, EventArgs e)
        {
            txtConnectionString.Properties.PasswordChar = '*';
        }

        private void txtNamespaces_Leave(object sender, EventArgs e)
        {
            AssignHeaderEditorText();
        }

        private void cbxNorthwindSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbxNorthwindSamples.Text)
            {
                case "SelectAllExcept":
                    this.syntaxEditor.Text = Resources.SelectAllExcept;
                    break;

                case "SelectSubQuery":
                    this.syntaxEditor.Text = Resources.SelectSubQuery;
                    break;

                case "SelectSubQueryAllOrderColumns":
                    this.syntaxEditor.Text = Resources.SelectSubQueryAllOrderColumns;
                    break;

                case "FromSubQuery":
                    this.syntaxEditor.Text = Resources.FromSubQuery;
                    break;

                case "WhereExists":
                    this.syntaxEditor.Text = Resources.WhereExists;
                    break;

                case "JoinOnSubquery":
                    this.syntaxEditor.Text = Resources.JoinOnSubquery;
                    break;

                case "CorrelatedSubQuery":
                    this.syntaxEditor.Text = Resources.CorrelatedSubQuery;
                    break;

                case "TypicalJoin":
                    this.syntaxEditor.Text = Resources.TypicalJoin;
                    break;

                case "PagingSample":
                    this.syntaxEditor.Text = Resources.PagingSample;
                    break;

                case "NativeLanguageSyntax":
                    this.syntaxEditor.Text = Resources.NativeLanguageSyntax;
                    break;

                case "TraditionalSqlStyle":
                    this.syntaxEditor.Text = Resources.TraditionalSqlStyle;
                    break;

                case "ArithmeticExpression":
                    this.syntaxEditor.Text = Resources.ArithmeticExpression;
                    break;

                case "CaseThenWhen1":
                    this.syntaxEditor.Text = Resources.CaseThenWhen1;
                    break;

                case "CaseThenWhen2":
                    this.syntaxEditor.Text = Resources.CaseThenWhen2;
                    break;

                case "SubQueryWithAnyOperator":
                    this.syntaxEditor.Text = Resources.SubQueryWithAnyOperator;
                    break;

                case "SubQueryWithAllOperator":
                    this.syntaxEditor.Text = Resources.SubQueryWithAllOperator;
                    break;
            }
        }
    }
}