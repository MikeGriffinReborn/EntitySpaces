namespace EntitySpaces.AddIn
{
    partial class ucMetadata
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMetadata));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeImageList = new System.Windows.Forms.ImageList(this.components);
            this.tree = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ToolBar = new System.Windows.Forms.ToolBar();
            this.toolBarButton_Save = new System.Windows.Forms.ToolBarButton();
            this.ucMetadataProperties = new EntitySpaces.AddIn.ucMetadataProperties();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "save.png");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            // 
            // treeImageList
            // 
            this.treeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImageList.ImageStream")));
            this.treeImageList.TransparentColor = System.Drawing.Color.White;
            this.treeImageList.Images.SetKeyName(0, "database_many.png");
            this.treeImageList.Images.SetKeyName(1, "database_single.png");
            this.treeImageList.Images.SetKeyName(2, "table_many.png");
            this.treeImageList.Images.SetKeyName(3, "table_single.png");
            this.treeImageList.Images.SetKeyName(4, "");
            this.treeImageList.Images.SetKeyName(5, "view_many.png");
            this.treeImageList.Images.SetKeyName(6, "view_single.png");
            this.treeImageList.Images.SetKeyName(7, "gear_many.png");
            this.treeImageList.Images.SetKeyName(8, "gear_single.png");
            this.treeImageList.Images.SetKeyName(9, "column_many.png");
            this.treeImageList.Images.SetKeyName(10, "column_single.png");
            this.treeImageList.Images.SetKeyName(11, "fk_many.png");
            this.treeImageList.Images.SetKeyName(12, "fk_single.png");
            this.treeImageList.Images.SetKeyName(13, "key_single.png");
            this.treeImageList.Images.SetKeyName(14, "index_many.png");
            this.treeImageList.Images.SetKeyName(15, "index_single.png");
            this.treeImageList.Images.SetKeyName(16, "key_many.png");
            this.treeImageList.Images.SetKeyName(17, "parameter_many.png");
            this.treeImageList.Images.SetKeyName(18, "parameter_single.png");
            this.treeImageList.Images.SetKeyName(19, "result_column_many.png");
            this.treeImageList.Images.SetKeyName(20, "result_column_single.png");
            this.treeImageList.Images.SetKeyName(21, "");
            this.treeImageList.Images.SetKeyName(22, "");
            this.treeImageList.Images.SetKeyName(23, "");
            this.treeImageList.Images.SetKeyName(24, "");
            this.treeImageList.Images.SetKeyName(25, "");
            this.treeImageList.Images.SetKeyName(26, "column_single_with_key.png");
            // 
            // tree
            // 
            this.tree.BackColor = System.Drawing.SystemColors.Window;
            this.tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tree.FullRowSelect = true;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.treeImageList;
            this.tree.Indent = 20;
            this.tree.ItemHeight = 18;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new System.Drawing.Size(202, 589);
            this.tree.TabIndex = 0;
            this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeExpand);
            this.tree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeSelect);
            this.tree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tree_MouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(458, 589);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.AutoScroll = true;
            this.splitContainer2.Panel1.Controls.Add(this.Grid);
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.AutoScroll = true;
            this.splitContainer2.Panel2.Controls.Add(this.ucMetadataProperties);
            this.splitContainer2.Size = new System.Drawing.Size(252, 589);
            this.splitContainer2.SplitterDistance = 215;
            this.splitContainer2.TabIndex = 15;
            // 
            // Grid
            // 
            this.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 30);
            this.Grid.MultiSelect = false;
            this.Grid.Name = "Grid";
            this.Grid.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Grid.ShowCellErrors = false;
            this.Grid.ShowCellToolTips = false;
            this.Grid.ShowEditingIcon = false;
            this.Grid.ShowRowErrors = false;
            this.Grid.Size = new System.Drawing.Size(250, 183);
            this.Grid.TabIndex = 19;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.ToolBar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 30);
            this.panel1.TabIndex = 23;
            // 
            // ToolBar
            // 
            this.ToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.ToolBar.AutoSize = false;
            this.ToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton_Save});
            this.ToolBar.Divider = false;
            this.ToolBar.DropDownArrows = true;
            this.ToolBar.ImageList = this.imageList1;
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.ShowToolTips = true;
            this.ToolBar.Size = new System.Drawing.Size(250, 26);
            this.ToolBar.TabIndex = 8;
            this.ToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ToolBar_ButtonClick);
            // 
            // toolBarButton_Save
            // 
            this.toolBarButton_Save.ImageIndex = 0;
            this.toolBarButton_Save.Name = "toolBarButton_Save";
            this.toolBarButton_Save.Tag = "save";
            this.toolBarButton_Save.ToolTipText = "Save User Metadata";
            // 
            // ucMetadataProperties
            // 
            this.ucMetadataProperties.AutoScroll = true;
            this.ucMetadataProperties.BackColor = System.Drawing.SystemColors.Window;
            this.ucMetadataProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMetadataProperties.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucMetadataProperties.Location = new System.Drawing.Point(0, 0);
            this.ucMetadataProperties.Name = "ucMetadataProperties";
            this.ucMetadataProperties.Size = new System.Drawing.Size(250, 368);
            this.ucMetadataProperties.TabIndex = 0;
            // 
            // ucMetadata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucMetadata";
            this.Size = new System.Drawing.Size(458, 589);
            this.Load += new System.EventHandler(this.ucMetadata_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.ImageList treeImageList;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.SplitContainer splitContainer2;
        public ucMetadataProperties ucMetadataProperties;
        private System.Windows.Forms.DataGridView Grid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolBar ToolBar;
        private System.Windows.Forms.ToolBarButton toolBarButton_Save;
    }
}
