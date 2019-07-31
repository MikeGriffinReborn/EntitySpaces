namespace EntitySpaces.QuerySandbox
{
    partial class QueryWindowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ActiproSoftware.SyntaxEditor.Document document1 = new ActiproSoftware.SyntaxEditor.Document();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryWindowForm));
            this.cSharpSyntaxLanguage1 = new ActiproSoftware.SyntaxEditor.Addons.CSharp.CSharpSyntaxLanguage(this.components);
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.syntaxEditor = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.txtSQL = new System.Windows.Forms.RichTextBox();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtConnectionString = new DevExpress.XtraEditors.TextEdit();
            this.btnExecute = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.bnLoad_40_EsAssemblies = new DevExpress.XtraEditors.SimpleButton();
            this.bnLoad_35_EsAssemblies = new DevExpress.XtraEditors.SimpleButton();
            this.btnReferences = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cbxProviders = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtProviderMetadataKey = new DevExpress.XtraEditors.TextEdit();
            this.cbxEsVersion = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtNamespaces = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cbxNorthwindSamples = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtConnectionString.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxProviders.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProviderMetadataKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxEsVersion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamespaces.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxNorthwindSamples.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(3, 43);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.grid);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(845, 432);
            this.splitContainerControl1.SplitterPosition = 531;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.syntaxEditor);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.txtSQL);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(531, 432);
            this.splitContainerControl2.SplitterPosition = 196;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // syntaxEditor
            // 
            this.syntaxEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEditor.Document = document1;
            this.syntaxEditor.Location = new System.Drawing.Point(0, 0);
            this.syntaxEditor.Name = "syntaxEditor";
            this.syntaxEditor.Size = new System.Drawing.Size(531, 196);
            this.syntaxEditor.TabIndex = 0;
            // 
            // txtSQL
            // 
            this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQL.Location = new System.Drawing.Point(0, 0);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(531, 231);
            this.txtSQL.TabIndex = 0;
            this.txtSQL.Text = "";
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MainView = this.gridView1;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(309, 432);
            this.grid.TabIndex = 0;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.grid;
            this.gridView1.Name = "gridView1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainerControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelControl2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(851, 518);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtConnectionString);
            this.panelControl1.Controls.Add(this.btnExecute);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(845, 34);
            this.panelControl1.TabIndex = 1;
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.EditValue = "Data Source=localhost;Initial Catalog=Northwind;Integrated Security=SSPI;";
            this.txtConnectionString.Location = new System.Drawing.Point(90, 7);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Properties.PasswordChar = '*';
            this.txtConnectionString.Size = new System.Drawing.Size(750, 20);
            this.txtConnectionString.TabIndex = 1;
            this.txtConnectionString.ToolTip = "Enter your connection string here";
            this.txtConnectionString.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.txtConnectionString.ToolTipTitle = "Connection String";
            this.txtConnectionString.Enter += new System.EventHandler(this.textEdit1_Enter);
            this.txtConnectionString.Leave += new System.EventHandler(this.textEdit1_Leave);
            // 
            // btnExecute
            // 
            this.btnExecute.Image = ((System.Drawing.Image)(resources.GetObject("btnExecute.Image")));
            this.btnExecute.Location = new System.Drawing.Point(9, 5);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "&Execute";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(3, 481);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(845, 34);
            this.panelControl2.TabIndex = 2;
            // 
            // bnLoad_40_EsAssemblies
            // 
            this.bnLoad_40_EsAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bnLoad_40_EsAssemblies.Location = new System.Drawing.Point(3, 29);
            this.bnLoad_40_EsAssemblies.Name = "bnLoad_40_EsAssemblies";
            this.bnLoad_40_EsAssemblies.Size = new System.Drawing.Size(164, 23);
            this.bnLoad_40_EsAssemblies.TabIndex = 2;
            this.bnLoad_40_EsAssemblies.Text = "Load .NET 4.0 Assemblies";
            this.bnLoad_40_EsAssemblies.Click += new System.EventHandler(this.bnLoad_40_EsAssemblies_Click);
            // 
            // bnLoad_35_EsAssemblies
            // 
            this.bnLoad_35_EsAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bnLoad_35_EsAssemblies.Location = new System.Drawing.Point(3, 58);
            this.bnLoad_35_EsAssemblies.Name = "bnLoad_35_EsAssemblies";
            this.bnLoad_35_EsAssemblies.Size = new System.Drawing.Size(164, 23);
            this.bnLoad_35_EsAssemblies.TabIndex = 2;
            this.bnLoad_35_EsAssemblies.Text = "Load .NET 3.5 Assemblies";
            this.bnLoad_35_EsAssemblies.Click += new System.EventHandler(this.bnLoad_35_EsAssemblies_Click);
            // 
            // btnReferences
            // 
            this.btnReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReferences.Location = new System.Drawing.Point(3, 87);
            this.btnReferences.Name = "btnReferences";
            this.btnReferences.Size = new System.Drawing.Size(164, 23);
            this.btnReferences.TabIndex = 1;
            this.btnReferences.Text = "References";
            this.btnReferences.Click += new System.EventHandler(this.btnReferences_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1033, 524);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.bnLoad_35_EsAssemblies, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.bnLoad_40_EsAssemblies, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnReferences, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.cbxProviders, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.txtProviderMetadataKey, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.cbxEsVersion, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtNamespaces, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.labelControl1, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.cbxNorthwindSamples, 0, 10);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(860, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 13;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(170, 430);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // cbxProviders
            // 
            this.cbxProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxProviders.Location = new System.Drawing.Point(3, 142);
            this.cbxProviders.Name = "cbxProviders";
            this.cbxProviders.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxProviders.Size = new System.Drawing.Size(164, 20);
            this.cbxProviders.TabIndex = 4;
            this.cbxProviders.ToolTip = "The EntitySpaces DataProvider, you can choose any one you desire and hit \"Parse\" " +
    "and it doesn\'t require a valid connection string";
            this.cbxProviders.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cbxProviders.ToolTipTitle = "EntitySpaces DataProvider";
            // 
            // txtProviderMetadataKey
            // 
            this.txtProviderMetadataKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProviderMetadataKey.EditValue = "esDefault";
            this.txtProviderMetadataKey.Location = new System.Drawing.Point(3, 168);
            this.txtProviderMetadataKey.Name = "txtProviderMetadataKey";
            this.txtProviderMetadataKey.Size = new System.Drawing.Size(164, 20);
            this.txtProviderMetadataKey.TabIndex = 5;
            this.txtProviderMetadataKey.ToolTip = "The Metadata Provider Map - don\'t change unless you know what this means";
            this.txtProviderMetadataKey.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.txtProviderMetadataKey.ToolTipTitle = "Metadata Provider Map";
            // 
            // cbxEsVersion
            // 
            this.cbxEsVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxEsVersion.EditValue = "EntitySpaces 2012";
            this.cbxEsVersion.Location = new System.Drawing.Point(3, 3);
            this.cbxEsVersion.Name = "cbxEsVersion";
            this.cbxEsVersion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxEsVersion.Properties.Items.AddRange(new object[] {
            "EntitySpaces 2011",
            "EntitySpaces 2012"});
            this.cbxEsVersion.Size = new System.Drawing.Size(164, 20);
            this.cbxEsVersion.TabIndex = 7;
            // 
            // txtNamespaces
            // 
            this.txtNamespaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNamespaces.EditValue = "BusinessObjects";
            this.txtNamespaces.Location = new System.Drawing.Point(3, 194);
            this.txtNamespaces.Name = "txtNamespaces";
            this.txtNamespaces.Size = new System.Drawing.Size(164, 114);
            this.txtNamespaces.TabIndex = 8;
            this.txtNamespaces.ToolTip = "Add one or more namespaces to enable intellisense for those assemblies";
            this.txtNamespaces.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.txtNamespaces.ToolTipTitle = "Namespaces";
            this.txtNamespaces.Leave += new System.EventHandler(this.txtNamespaces_Leave);
            // 
            // labelControl1
            // 
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl1.Location = new System.Drawing.Point(3, 334);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(91, 13);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "Northwind Samples";
            // 
            // cbxNorthwindSamples
            // 
            this.cbxNorthwindSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxNorthwindSamples.Location = new System.Drawing.Point(3, 353);
            this.cbxNorthwindSamples.Name = "cbxNorthwindSamples";
            this.cbxNorthwindSamples.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxNorthwindSamples.Properties.Items.AddRange(new object[] {
            "SelectAllExcept",
            "SelectSubQuery",
            "SelectSubQueryAllOrderColumns",
            "FromSubQuery",
            "WhereExists",
            "JoinOnSubquery",
            "CorrelatedSubQuery",
            "TypicalJoin",
            "PagingSample",
            "NativeLanguageSyntax",
            "TraditionalSqlStyle",
            "ArithmeticExpression",
            "CaseThenWhen1",
            "CaseThenWhen2",
            "SubQueryWithAnyOperator",
            "SubQueryWithAllOperator"});
            this.cbxNorthwindSamples.Size = new System.Drawing.Size(164, 20);
            this.cbxNorthwindSamples.TabIndex = 10;
            this.cbxNorthwindSamples.SelectedIndexChanged += new System.EventHandler(this.cbxNorthwindSamples_SelectedIndexChanged);
            // 
            // QueryWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 524);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "QueryWindowForm";
            this.Text = "EntitySpaces Query Sandbox";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtConnectionString.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxProviders.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProviderMetadataKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxEsVersion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamespaces.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxNorthwindSamples.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ActiproSoftware.SyntaxEditor.Addons.CSharp.CSharpSyntaxLanguage cSharpSyntaxLanguage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private ActiproSoftware.SyntaxEditor.SyntaxEditor syntaxEditor;
        private System.Windows.Forms.RichTextBox txtSQL;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnReferences;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnExecute;
        private DevExpress.XtraEditors.SimpleButton bnLoad_40_EsAssemblies;
        private DevExpress.XtraEditors.SimpleButton bnLoad_35_EsAssemblies;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.ComboBoxEdit cbxProviders;
        private DevExpress.XtraEditors.TextEdit txtProviderMetadataKey;
        private DevExpress.XtraEditors.ComboBoxEdit cbxEsVersion;
        private DevExpress.XtraEditors.TextEdit txtConnectionString;
        private DevExpress.XtraEditors.MemoEdit txtNamespaces;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cbxNorthwindSamples;
    }
}