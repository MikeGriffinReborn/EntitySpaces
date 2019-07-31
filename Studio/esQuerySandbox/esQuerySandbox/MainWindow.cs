using System.IO;
using ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Ribbon;
using Microsoft.Win32;
using ActiproSoftware.SyntaxEditor;


namespace EntitySpaces.QuerySandbox
{
    public partial class MainWindow : RibbonForm
    {
        public MainWindow()
        {
            InitializeComponent();
            InitSkinGallery();

            SemanticParserService.Start();
        }

        private void LoadStandardAssemblies(ReferenceList referenceList)
        {
            referenceList.ProjectResolver.AddExternalReferenceForMSCorLib();
            referenceList.ProjectResolver.AddExternalReferenceForSystemAssembly("System");
            referenceList.ProjectResolver.AddExternalReferenceForSystemAssembly("System.Data");
            referenceList.ProjectResolver.AddExternalReferenceForSystemAssembly("System.Data.Linq");
            referenceList.ProjectResolver.AddExternalReferenceForSystemAssembly("System.Drawing");
            referenceList.ProjectResolver.AddExternalReferenceForSystemAssembly("System.Transactions");
            referenceList.ProjectResolver.AddExternalReferenceForSystemAssembly("System.Windows.Forms");
        }

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        private void iNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ReferenceList referenceList = new ReferenceList();

            LoadStandardAssemblies(referenceList);

            QueryWindowForm form = new QueryWindowForm(referenceList);
            form.Text = "Query";
            form.MdiParent = this;
            form.Show();
        }

        private void MainWindow_Load(object sender, System.EventArgs e)
        {
            ReferenceList referenceList = new ReferenceList();

            LoadStandardAssemblies(referenceList);

            QueryWindowForm form = new QueryWindowForm(referenceList);
            form.Text = "Query";
            form.MdiParent = this;
            form.Show();
        }

        private void iAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }
    }
}