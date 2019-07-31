namespace EntitySpaces.AddIn
{
    partial class ucProjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucProjects));
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Project");
            this.menuFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.executeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPathToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.FolderMoveUpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderMoveDownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonMRU = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuMRU = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonProjectOpen = new System.Windows.Forms.ToolStripButton();
            this.ButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonSave = new System.Windows.Forms.ToolStripButton();
            this.ButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonOpenFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonMoveUp = new System.Windows.Forms.ToolStripButton();
            this.ButtonMoveDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonRecord = new System.Windows.Forms.ToolStripButton();
            this.ButtonExecute = new System.Windows.Forms.ToolStripButton();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.projectTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRecordCancel = new System.Windows.Forms.Button();
            this.buttonRecordOk = new System.Windows.Forms.Button();
            this.tree = new EntitySpaces.AddIn.ucTemplateControl();
            this.label1 = new System.Windows.Forms.Label();
            this.menuTemplate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.EditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.MoveUpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MoveDownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFolder.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuFolder
            // 
            this.menuFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeMenuItem,
            this.toolStripSeparator2,
            this.addFolderMenuItem,
            this.recordMenuItem,
            this.toolStripSeparator3,
            this.expandAllMenuItem,
            this.renameMenuItem,
            this.copyPathToClipboardToolStripMenuItem,
            this.toolStripSeparator6,
            this.deleteMenuItem,
            this.toolStripSeparator11,
            this.FolderMoveUpMenuItem,
            this.FolderMoveDownMenuItem});
            this.menuFolder.Name = "contextMenuStrip";
            this.menuFolder.Size = new System.Drawing.Size(199, 226);
            // 
            // executeMenuItem
            // 
            this.executeMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("executeMenuItem.Image")));
            this.executeMenuItem.Name = "executeMenuItem";
            this.executeMenuItem.Size = new System.Drawing.Size(198, 22);
            this.executeMenuItem.Text = "Execute";
            this.executeMenuItem.Click += new System.EventHandler(this.ExecuteMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(195, 6);
            // 
            // addFolderMenuItem
            // 
            this.addFolderMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addFolderMenuItem.Image")));
            this.addFolderMenuItem.Name = "addFolderMenuItem";
            this.addFolderMenuItem.Size = new System.Drawing.Size(198, 22);
            this.addFolderMenuItem.Text = "Add Folder";
            this.addFolderMenuItem.Click += new System.EventHandler(this.AddFolder_Click);
            // 
            // recordMenuItem
            // 
            this.recordMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("recordMenuItem.Image")));
            this.recordMenuItem.Name = "recordMenuItem";
            this.recordMenuItem.Size = new System.Drawing.Size(198, 22);
            this.recordMenuItem.Text = "Record Template";
            this.recordMenuItem.Click += new System.EventHandler(this.ButtonRecord_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(195, 6);
            // 
            // expandAllMenuItem
            // 
            this.expandAllMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("expandAllMenuItem.Image")));
            this.expandAllMenuItem.Name = "expandAllMenuItem";
            this.expandAllMenuItem.Size = new System.Drawing.Size(198, 22);
            this.expandAllMenuItem.Text = "Expand All";
            this.expandAllMenuItem.Click += new System.EventHandler(this.expandAllMenuItem_Click);
            // 
            // renameMenuItem
            // 
            this.renameMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameMenuItem.Image")));
            this.renameMenuItem.Name = "renameMenuItem";
            this.renameMenuItem.Size = new System.Drawing.Size(198, 22);
            this.renameMenuItem.Text = "Rename";
            this.renameMenuItem.Click += new System.EventHandler(this.RenameMenuItem_Click);
            // 
            // copyPathToClipboardToolStripMenuItem
            // 
            this.copyPathToClipboardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyPathToClipboardToolStripMenuItem.Image")));
            this.copyPathToClipboardToolStripMenuItem.Name = "copyPathToClipboardToolStripMenuItem";
            this.copyPathToClipboardToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.copyPathToClipboardToolStripMenuItem.Text = "Copy Path to Clipboard";
            this.copyPathToClipboardToolStripMenuItem.Click += new System.EventHandler(this.CopyNodePath);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(195, 6);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteMenuItem.Image")));
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(198, 22);
            this.deleteMenuItem.Text = "Delete";
            this.deleteMenuItem.Click += new System.EventHandler(this.DeleteMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(195, 6);
            // 
            // FolderMoveUpMenuItem
            // 
            this.FolderMoveUpMenuItem.Image = global::EntitySpaces.AddIn.Resource.nav_up;
            this.FolderMoveUpMenuItem.Name = "FolderMoveUpMenuItem";
            this.FolderMoveUpMenuItem.Size = new System.Drawing.Size(198, 22);
            this.FolderMoveUpMenuItem.Text = "Move Up";
            this.FolderMoveUpMenuItem.ToolTipText = "Move the Selected Node Up";
            this.FolderMoveUpMenuItem.Click += new System.EventHandler(this.FolderMoveUpMenuItem_Click);
            // 
            // FolderMoveDownMenuItem
            // 
            this.FolderMoveDownMenuItem.Image = global::EntitySpaces.AddIn.Resource.nav_down;
            this.FolderMoveDownMenuItem.Name = "FolderMoveDownMenuItem";
            this.FolderMoveDownMenuItem.Size = new System.Drawing.Size(198, 22);
            this.FolderMoveDownMenuItem.Text = "Move Down";
            this.FolderMoveDownMenuItem.ToolTipText = "Move the Selected Node Down";
            this.FolderMoveDownMenuItem.Click += new System.EventHandler(this.FolderMoveDownMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonMRU,
            this.toolStripSeparator8,
            this.ButtonProjectOpen,
            this.ButtonClear,
            this.toolStripSeparator7,
            this.ButtonSave,
            this.ButtonSaveAs,
            this.toolStripSeparator4,
            this.ButtonOpenFolder,
            this.toolStripSeparator9,
            this.ButtonMoveUp,
            this.ButtonMoveDown,
            this.toolStripSeparator12,
            this.ButtonRecord,
            this.ButtonExecute});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(371, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonMRU
            // 
            this.ButtonMRU.BackColor = System.Drawing.SystemColors.Window;
            this.ButtonMRU.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonMRU.DropDown = this.menuMRU;
            this.ButtonMRU.Image = ((System.Drawing.Image)(resources.GetObject("ButtonMRU.Image")));
            this.ButtonMRU.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonMRU.Name = "ButtonMRU";
            this.ButtonMRU.Size = new System.Drawing.Size(29, 22);
            this.ButtonMRU.Text = "Most Recently Used Projects";
            // 
            // menuMRU
            // 
            this.menuMRU.Name = "menuMRU";
            this.menuMRU.OwnerItem = this.ButtonMRU;
            this.menuMRU.Size = new System.Drawing.Size(61, 4);
            this.menuMRU.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuMRU_ItemClicked);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonProjectOpen
            // 
            this.ButtonProjectOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonProjectOpen.Image = ((System.Drawing.Image)(resources.GetObject("ButtonProjectOpen.Image")));
            this.ButtonProjectOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonProjectOpen.Name = "ButtonProjectOpen";
            this.ButtonProjectOpen.Size = new System.Drawing.Size(23, 22);
            this.ButtonProjectOpen.Text = "toolStripButton1";
            this.ButtonProjectOpen.ToolTipText = "Open a Project";
            this.ButtonProjectOpen.Click += new System.EventHandler(this.ButtonProjectOpen_Click);
            // 
            // ButtonClear
            // 
            this.ButtonClear.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClear.Image")));
            this.ButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonClear.Name = "ButtonClear";
            this.ButtonClear.Size = new System.Drawing.Size(23, 22);
            this.ButtonClear.Text = "Reset Project to New";
            this.ButtonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonSave
            // 
            this.ButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSave.Enabled = false;
            this.ButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSave.Image")));
            this.ButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(23, 22);
            this.ButtonSave.Text = "toolStripButton3";
            this.ButtonSave.ToolTipText = "Save";
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonSaveAs
            // 
            this.ButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSaveAs.Image")));
            this.ButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSaveAs.Name = "ButtonSaveAs";
            this.ButtonSaveAs.Size = new System.Drawing.Size(23, 22);
            this.ButtonSaveAs.Text = "toolStripButton2";
            this.ButtonSaveAs.ToolTipText = "Save As";
            this.ButtonSaveAs.Click += new System.EventHandler(this.ButtonSaveAs_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonOpenFolder
            // 
            this.ButtonOpenFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("ButtonOpenFolder.Image")));
            this.ButtonOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonOpenFolder.Name = "ButtonOpenFolder";
            this.ButtonOpenFolder.Size = new System.Drawing.Size(23, 22);
            this.ButtonOpenFolder.Text = "Open Default Output Folder";
            this.ButtonOpenFolder.Click += new System.EventHandler(this.ButtonOpenFolder_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonMoveUp
            // 
            this.ButtonMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonMoveUp.Image = global::EntitySpaces.AddIn.Resource.nav_up;
            this.ButtonMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonMoveUp.Name = "ButtonMoveUp";
            this.ButtonMoveUp.Size = new System.Drawing.Size(23, 22);
            this.ButtonMoveUp.ToolTipText = "Move the Selected Node Up";
            this.ButtonMoveUp.Click += new System.EventHandler(this.ButtonMoveUp_Click);
            // 
            // ButtonMoveDown
            // 
            this.ButtonMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonMoveDown.Image = global::EntitySpaces.AddIn.Resource.nav_down;
            this.ButtonMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonMoveDown.Name = "ButtonMoveDown";
            this.ButtonMoveDown.Size = new System.Drawing.Size(23, 22);
            this.ButtonMoveDown.ToolTipText = "Move the Selected Node Down";
            this.ButtonMoveDown.Click += new System.EventHandler(this.ButtonMoveDown_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonRecord
            // 
            this.ButtonRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonRecord.Enabled = false;
            this.ButtonRecord.Image = ((System.Drawing.Image)(resources.GetObject("ButtonRecord.Image")));
            this.ButtonRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonRecord.Name = "ButtonRecord";
            this.ButtonRecord.Size = new System.Drawing.Size(23, 22);
            this.ButtonRecord.Text = "toolStripButton2";
            this.ButtonRecord.ToolTipText = "Record a Template";
            this.ButtonRecord.Click += new System.EventHandler(this.ButtonRecord_Click);
            // 
            // ButtonExecute
            // 
            this.ButtonExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonExecute.Enabled = false;
            this.ButtonExecute.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExecute.Image")));
            this.ButtonExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonExecute.Name = "ButtonExecute";
            this.ButtonExecute.Size = new System.Drawing.Size(23, 22);
            this.ButtonExecute.Text = "toolStripButton1";
            this.ButtonExecute.ToolTipText = "Execute Project Starting at the Selected Node";
            this.ButtonExecute.Click += new System.EventHandler(this.ExecuteMenuItem_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.projectTree);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer.Panel2Collapsed = true;
            this.splitContainer.Size = new System.Drawing.Size(371, 520);
            this.splitContainer.SplitterDistance = 240;
            this.splitContainer.TabIndex = 8;
            // 
            // projectTree
            // 
            this.projectTree.AllowDrop = true;
            this.projectTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.projectTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.projectTree.ContextMenuStrip = this.menuFolder;
            this.projectTree.HideSelection = false;
            this.projectTree.ImageIndex = 0;
            this.projectTree.ImageList = this.imageList;
            this.projectTree.LabelEdit = true;
            this.projectTree.Location = new System.Drawing.Point(0, 10);
            this.projectTree.Name = "projectTree";
            treeNode2.ContextMenuStrip = this.menuFolder;
            treeNode2.Name = "Node1";
            treeNode2.Text = "Project";
            this.projectTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.projectTree.SelectedImageIndex = 0;
            this.projectTree.ShowNodeToolTips = true;
            this.projectTree.Size = new System.Drawing.Size(371, 520);
            this.projectTree.TabIndex = 0;
            this.projectTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProjectTree_DragDrop);
            this.projectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ProjectTree_AfterSelect);
            this.projectTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProjectTree_DragEnter);
            this.projectTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ProjectTree_NodeMouseClick);
            this.projectTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ProjectTree_ItemDrag);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "folder_closed.png");
            this.imageList.Images.SetKeyName(1, "folder_open.png");
            this.imageList.Images.SetKeyName(2, "microphone.png");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.buttonRecordCancel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonRecordOk, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tree, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(150, 46);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // buttonRecordCancel
            // 
            this.buttonRecordCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRecordCancel.Location = new System.Drawing.Point(3, 20);
            this.buttonRecordCancel.Name = "buttonRecordCancel";
            this.buttonRecordCancel.Size = new System.Drawing.Size(63, 23);
            this.buttonRecordCancel.TabIndex = 7;
            this.buttonRecordCancel.Text = "Cancel";
            this.buttonRecordCancel.UseVisualStyleBackColor = true;
            this.buttonRecordCancel.Click += new System.EventHandler(this.buttonRecordCancel_Click);
            // 
            // buttonRecordOk
            // 
            this.buttonRecordOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRecordOk.Enabled = false;
            this.buttonRecordOk.Location = new System.Drawing.Point(72, 20);
            this.buttonRecordOk.Name = "buttonRecordOk";
            this.buttonRecordOk.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonRecordOk.Size = new System.Drawing.Size(75, 23);
            this.buttonRecordOk.TabIndex = 6;
            this.buttonRecordOk.Text = "Ok";
            this.buttonRecordOk.UseVisualStyleBackColor = true;
            this.buttonRecordOk.Click += new System.EventHandler(this.buttonRecordOk_Click);
            // 
            // tree
            // 
            this.tree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.tree, 2);
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Location = new System.Drawing.Point(3, 33);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(144, 1);
            this.tree.TabIndex = 5;
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Tree_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.label1.Size = new System.Drawing.Size(144, 30);
            this.label1.TabIndex = 8;
            this.label1.Text = "Please Select a Template and Click Ok";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuTemplate
            // 
            this.menuTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.EditMenuItem,
            this.editSettingsToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripSeparator5,
            this.toolStripMenuItem5,
            this.toolStripSeparator10,
            this.MoveUpMenuItem,
            this.MoveDownMenuItem});
            this.menuTemplate.Name = "contextMenuStrip";
            this.menuTemplate.Size = new System.Drawing.Size(199, 220);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItem1.Text = "Execute";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.ExecuteMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
            // 
            // EditMenuItem
            // 
            this.EditMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("EditMenuItem.Image")));
            this.EditMenuItem.Name = "EditMenuItem";
            this.EditMenuItem.Size = new System.Drawing.Size(198, 22);
            this.EditMenuItem.Text = "Edit Template";
            this.EditMenuItem.Click += new System.EventHandler(this.EditTemplatesMenuItem_Click);
            // 
            // editSettingsToolStripMenuItem
            // 
            this.editSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editSettingsToolStripMenuItem.Image")));
            this.editSettingsToolStripMenuItem.Name = "editSettingsToolStripMenuItem";
            this.editSettingsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.editSettingsToolStripMenuItem.Text = "Edit Settings";
            this.editSettingsToolStripMenuItem.Click += new System.EventHandler(this.EditSettingsMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameToolStripMenuItem.Image")));
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.RenameMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem2.Image")));
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItem2.Text = "Copy Path to Clipboard";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.CopyNodePath);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(195, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem5.Image")));
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItem5.Text = "Delete";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.DeleteMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(195, 6);
            // 
            // MoveUpMenuItem
            // 
            this.MoveUpMenuItem.Image = global::EntitySpaces.AddIn.Resource.nav_up;
            this.MoveUpMenuItem.Name = "MoveUpMenuItem";
            this.MoveUpMenuItem.Size = new System.Drawing.Size(198, 22);
            this.MoveUpMenuItem.Text = "Move up";
            this.MoveUpMenuItem.ToolTipText = "Move the Selected Node Up";
            this.MoveUpMenuItem.Click += new System.EventHandler(this.MoveUpMenuItem_Click);
            // 
            // MoveDownMenuItem
            // 
            this.MoveDownMenuItem.Image = global::EntitySpaces.AddIn.Resource.nav_down;
            this.MoveDownMenuItem.Name = "MoveDownMenuItem";
            this.MoveDownMenuItem.Size = new System.Drawing.Size(198, 22);
            this.MoveDownMenuItem.Text = "Move Down";
            this.MoveDownMenuItem.ToolTipText = "Move the Selected Node Down";
            this.MoveDownMenuItem.Click += new System.EventHandler(this.MoveDownMenuItem_Click);
            // 
            // ucProjects
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ucProjects";
            this.Size = new System.Drawing.Size(371, 545);
            this.Load += new System.EventHandler(this.ucProjects_Load);
            this.menuFolder.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuTemplate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonExecute;
        private System.Windows.Forms.ToolStripButton ButtonRecord;
        private System.Windows.Forms.ToolStripButton ButtonSave;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView projectTree;
        private System.Windows.Forms.Button buttonRecordOk;
        private System.Windows.Forms.Button buttonRecordCancel;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip menuFolder;
        private System.Windows.Forms.ToolStripMenuItem executeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem addFolderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ButtonProjectOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton ButtonSaveAs;
        private ucTemplateControl tree;
        private System.Windows.Forms.ContextMenuStrip menuTemplate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem EditMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem renameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton ButtonClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton ButtonOpenFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem expandAllMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton ButtonMRU;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ContextMenuStrip menuMRU;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem copyPathToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem MoveUpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MoveDownMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem FolderMoveUpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FolderMoveDownMenuItem;
        private System.Windows.Forms.ToolStripButton ButtonMoveUp;
        private System.Windows.Forms.ToolStripButton ButtonMoveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem editSettingsToolStripMenuItem;
    }
}
