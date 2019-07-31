using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using EntitySpaces.Common;
using EntitySpaces.AddIn.TemplateUI;
using EntitySpaces.CodeGenerator;
using EntitySpaces.MetadataEngine;

namespace EntitySpaces.AddIn
{
    internal class ucTemplateControl : TreeView
    {
        private TreeNode rootNode = null;
        private TemplateUICollection coll = new TemplateUICollection();
        private SortedList<int, UserControl> currentUIControls = new SortedList<int, UserControl>();
        private Dictionary<Guid, Hashtable> cachedSettings = new Dictionary<Guid, Hashtable>();
        private esSettings Settings;

        private ImageList imageList;
        private ContextMenuStrip folderMenu;
        private ToolStripMenuItem showallexecutable;
        private ToolStripMenuItem showall;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem collapseAll;
        private System.ComponentModel.IContainer components;

        public void LoadTemplates(ContextMenuStrip templateMenu, ContextMenuStrip subTemplateMenu, esSettings settings)
        {
            try
            {
                this.Settings = settings;

                if (this.TreeViewNodeSorter == null)
                {
                    this.TreeViewNodeSorter = new NodeSorter();
                    InitializeComponent();

                    Template.SetTemplateCachePath(esSettings.TemplateCachePath);
                    Template.SetCompilerAssemblyPath(Settings.CompilerAssemblyPath);
                }

                this.Nodes.Clear();
                rootNode = this.Nodes.Add("Templates");
                rootNode.ImageIndex = 2;
                rootNode.SelectedImageIndex = 2;
                rootNode.ContextMenuStrip = this.folderMenu;

                this.currentUIControls.Clear();
                this.coll.Clear();

                string[] files = Directory.GetFiles(Settings.TemplatePath, "*.est", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    Template template = new Template();
                    TemplateHeader header = null;
                    string[] nspace = null;

                    try
                    {
                        // If this doesn't meet the criteria skip it and move on to the next file
                        template.Parse(file);

                        header = template.Header;
                        if (header.Namespace == string.Empty) continue;

                        nspace = header.Namespace.Split('.');
                        if (nspace == null || nspace.Length == 0) continue;
                    }
                    catch { continue; }

                    // Okay, we have a valid template with a namespace ...
                    TreeNode node = rootNode;
                    TreeNode[] temp = null;

                    // This foreach loop adds all of the folder entries based on 
                    // the namespace
                    foreach (string entry in nspace)
                    {
                        temp = node.Nodes.Find(entry, true);

                        if (temp == null || temp.Length == 0)
                        {
                            node = node.Nodes.Add(entry);
                            node.Name = entry;
                        }
                        else
                        {
                            node = temp[0];
                        }

                        node.ImageIndex = 2;
                        node.SelectedImageIndex = 2;
                        node.ContextMenuStrip = this.folderMenu;
                    }

                    // Now we add the final node, with the template icon and stash the Template
                    // in the node's "Tag" property for later use when they execute it.
                    node = node.Nodes.Add(template.Header.Title);
                    node.Tag = template;
                    node.ToolTipText = header.Description + " : " + header.Author + " (" + header.Version + ")" + Environment.NewLine;


                    if (header.IsSubTemplate)
                    {
                        node.ImageIndex = 0;
                        node.SelectedImageIndex = 0;
                        node.ContextMenuStrip = subTemplateMenu;
                    }
                    else
                    {
                        node.ImageIndex = 1;
                        node.SelectedImageIndex = 1;
                        node.ContextMenuStrip = templateMenu;
                    }
                }

                // Now, let's sort it so it all makes sense ...
                this.Sort();
            }
            catch { }
        }

        public bool IsExecuteableTemplateSelected()
        {
            bool isSelected = false;

            try
            {
                TreeNode node = this.SelectedNode;

                if (node != null && node.Tag != null)
                {
                    Template template = node.Tag as Template;
                    isSelected = !template.Header.IsSubTemplate;
                }
            }
            catch { }

            return isSelected;
        }

        private void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (e.Node.Tag == null)
                {
                    e.Node.ImageIndex = 3;
                    e.Node.SelectedImageIndex = 3;
                }

                e.Cancel = false;
            }
            catch { }
        }

        private void tree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (e.Node.Tag == null)
                {
                    e.Node.ImageIndex = 2;
                    e.Node.SelectedImageIndex = 2;
                }

                e.Cancel = false;
            }
            catch { }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucTemplateControl));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.folderMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showallexecutable = new System.Windows.Forms.ToolStripMenuItem();
            this.showall = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.collapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.folderMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "template.png");
            this.imageList.Images.SetKeyName(1, "template_selected.png");
            this.imageList.Images.SetKeyName(2, "folder_closed.png");
            this.imageList.Images.SetKeyName(3, "folder_open.png");
            // 
            // folderMenu
            // 
            this.folderMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showallexecutable,
            this.showall,
            this.toolStripSeparator6,
            this.collapseAll});
            this.folderMenu.Name = "folderMenu";
            this.folderMenu.Size = new System.Drawing.Size(234, 76);
            this.folderMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.folderMenu_ItemClicked);
            // 
            // showallexecutable
            // 
            this.showallexecutable.Image = ((System.Drawing.Image)(resources.GetObject("showallexecutable.Image")));
            this.showallexecutable.Name = "showallexecutable";
            this.showallexecutable.Size = new System.Drawing.Size(233, 22);
            this.showallexecutable.Text = "Show All Executable Templates";
            // 
            // showall
            // 
            this.showall.Image = ((System.Drawing.Image)(resources.GetObject("showall.Image")));
            this.showall.Name = "showall";
            this.showall.Size = new System.Drawing.Size(233, 22);
            this.showall.Text = "Show All Templates";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(230, 6);
            // 
            // collapseAll
            // 
            this.collapseAll.Image = ((System.Drawing.Image)(resources.GetObject("collapseAll.Image")));
            this.collapseAll.Name = "collapseAll";
            this.collapseAll.Size = new System.Drawing.Size(233, 22);
            this.collapseAll.Text = "Collapse All";
            // 
            // ucTemplateControl
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HideSelection = false;
            this.ImageIndex = 0;
            this.ImageList = this.imageList;
            this.ItemHeight = 18;
            this.LineColor = System.Drawing.Color.Black;
            this.PathSeparator = ".";
            this.SelectedImageIndex = 0;
            this.ShowNodeToolTips = true;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ucTemplateControl_MouseClick);
            this.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeExpand);
            this.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeCollapse);
            this.folderMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ucTemplateControl_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    TreeNode node = this.GetNodeAt(e.X, e.Y);

                    if (node != null)
                    {
                        this.SelectedNode = node;
                    }
                }
            }
            catch { }
        }

        private void folderMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Name.ToLower())
                {
                    case "showallexecutable":

                        this.SelectedNode.Collapse(false);
                        this.ShowAllExecutableTemplates(this.SelectedNode);
                        this.SelectedNode.EnsureVisible();
                        break;

                    case "showall":

                        this.SelectedNode.ExpandAll();
                        this.SelectedNode.EnsureVisible();
                        break;

                    case "collapseall":

                        this.SelectedNode.Collapse();
                        break;
                }
            }
            catch { }
        }

        private void ShowAllExecutableTemplates(TreeNode node)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Tag != null)
                {
                    Template template = childNode.Tag as Template;

                    if (!template.Header.IsSubTemplate)
                    {
                        ExpandNode(node);
                        break;
                    }
                }
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                ShowAllExecutableTemplates(childNode);
            }
        }

        private void ExpandNode(TreeNode node)
        {
            if (node == null) return;

            node.Expand();

            ExpandNode(node.Parent);
        }
    }

    // Create a node sorter that implements the IComparer interface.
    internal class NodeSorter : IComparer
    {
        // Compare the length of the strings, or the strings
        // themselves, if they are the same length.
        public int Compare(object x, object y)
        {
            try
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;

                if ((tx.Tag == null && ty.Tag == null) ||
                    (tx.Tag != null && ty.Tag != null))
                {
                    // We compare folder to folder or tempate to template
                    return string.Compare(tx.Text, ty.Text);
                }
                else
                {
                    if (tx.Tag == null)
                    {
                        // This is a folder, we want it before templates
                        return -1;
                    }
                    else
                    {
                        // This is a template, we want it at the bottom, after folders
                        return 1;
                    }
                }
            }
            catch
            {
                return -1;
            }
        }
    }
}
