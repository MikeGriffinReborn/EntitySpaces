using System;
using System.Data;
using System.Windows.Forms;

using EntitySpaces.MetadataEngine;

namespace EntitySpaces.AddIn
{
    internal partial class ucMetadata : esUserControl
    {
        private Root root = null;
        private bool isDirty = false;
        private EntitySpaces.MetadataEngine.Single single = null;

        private ucMetadataProperties metadataProperties = null;

        public ucMetadata()
        {
            try
            {
                if (!this.DesignMode)
                {
                    InitializeComponent();
                }

                this.metadataProperties = this.ucMetadataProperties;
            }
            catch { }
        }

        public override void OnSettingsChanged()
        {
            try
            {
                this.root = null;

                if (Settings.Driver != "")
                {
                    this.root = esMetaCreator.Create(Settings);
                }
            }
            catch
            {

            }
            finally
            {
                InitializeTree();
            }
        }

        public bool IsConnected
        {
            get
            {
                if (root == null)
                    return false;
                else
                    return root.IsConnected;
            }
        }

        private void ucMetadata_Load(object sender, EventArgs e)
        {
            OnSettingsChanged();
        }

        private void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Cursor cursor = Cursor.Current;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                NodeData data = (NodeData)e.Node.Tag;

                if (null != data)
                {
                    switch (data.Type)
                    {
                        case NodeType.DATABASE:

                            ExpandDatabase(data.Meta as IDatabase, e.Node);
                            break;

                        case NodeType.TABLES:
                        case NodeType.SUBTABLES:

                            ExpandTables(data.Meta as ITables, e.Node);
                            break;

                        case NodeType.TABLE:

                            ExpandTable(data.Meta as ITable, e.Node);
                            break;

                        case NodeType.VIEWS:
                        case NodeType.SUBVIEWS:

                            ExpandViews(data.Meta as IViews, e.Node);
                            break;

                        case NodeType.VIEW:

                            ExpandView(data.Meta as IView, e.Node);
                            break;

                        case NodeType.PROCEDURES:

                            ExpandProcedures(data.Meta as IProcedures, e.Node);
                            break;

                        case NodeType.PROCEDURE:

                            ExpandProcedure(data.Meta as IProcedure, e.Node);
                            break;

                        case NodeType.COLUMNS:
                        case NodeType.PRIMARYKEYS:

                            ExpandColumns(data.Meta as IColumns, e.Node);
                            break;

                        case NodeType.PARAMETERS:

                            ExpandParameters(data.Meta as IParameters, e.Node);
                            break;

                        case NodeType.RESULTCOLUMNS:

                            ExpandResultColumns(data.Meta as IResultColumns, e.Node);
                            break;

                        case NodeType.INDEXES:

                            ExpandIndexes(data.Meta as IIndexes, e.Node);
                            break;

                        case NodeType.FOREIGNKEYS:

                            this.ExpandForeignKeys(data.Meta as IForeignKeys, e.Node);
                            break;

                        case NodeType.DOMAINS:

                            this.ExpandDomains(data.Meta as IDomains, e.Node);
                            break;

                        case NodeType.PROVIDERTYPES:

                            //this.ExpandProviderTypes(data.Meta as IProviderTypes, e.Node);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MainWindow.ShowError(ex);
            }
            finally
            {
                Cursor.Current = cursor;
            }
        }

        private void ExpandDatabase(IDatabase database, TreeNode dbNode)
        {
            if (HasBlankNode(dbNode))
            {
                IDatabase db = root.Databases[database.Name];

                TreeNode node;

                if (db.Tables.Count > 0)
                {
                    node = new TreeNode("Tables");
                    node.Tag = new NodeData(NodeType.TABLES, database.Tables);
                    node.SelectedImageIndex = node.ImageIndex = 2;
                    dbNode.Nodes.Add(node);
                    node.Nodes.Add(this.BlankNode);
                }

                if (db.Views.Count > 0)
                {
                    node = new TreeNode("Views");
                    node.Tag = new NodeData(NodeType.VIEWS, database.Views);
                    node.SelectedImageIndex = node.ImageIndex = 5;
                    dbNode.Nodes.Add(node);
                    node.Nodes.Add(this.BlankNode);
                }

                if (db.Procedures.Count > 0)
                {
                    node = new TreeNode("Procedures");
                    node.Tag = new NodeData(NodeType.PROCEDURES, database.Procedures);
                    node.SelectedImageIndex = node.ImageIndex = 7;
                    dbNode.Nodes.Add(node);
                    node.Nodes.Add(this.BlankNode);
                }

                if (db.Domains.Count > 0)
                {
                    node = new TreeNode("Domains");
                    node.Tag = new NodeData(NodeType.DOMAINS, database.Domains);
                    node.SelectedImageIndex = node.ImageIndex = 24;
                    dbNode.Nodes.Add(node);
                    node.Nodes.Add(this.BlankNode);
                }
            }
        }

        private void ExpandTables(ITables tables, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (ITable table in tables)
                {
                    TreeNode n = new TreeNode(table.Name);
                    n.Tag = new NodeData(NodeType.TABLE, table);
                    n.SelectedImageIndex = n.ImageIndex = 3;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }
            }
        }

        private void ExpandTable(ITable table, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                TreeNode n;

                if (table.Columns.Count > 0)
                {
                    n = new TreeNode("Columns");
                    n.Tag = new NodeData(NodeType.COLUMNS, table.Columns);
                    n.SelectedImageIndex = n.ImageIndex = 9;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }

                if (table.ForeignKeys.Count > 0)
                {
                    n = new TreeNode("ForeignKeys");
                    n.Tag = new NodeData(NodeType.FOREIGNKEYS, table.ForeignKeys);
                    n.SelectedImageIndex = n.ImageIndex = 11;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }

                if (table.Indexes.Count > 0)
                {
                    n = new TreeNode("Indexes");
                    n.Tag = new NodeData(NodeType.INDEXES, table.Indexes);
                    n.SelectedImageIndex = n.ImageIndex = 14;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }

                if (table.PrimaryKeys.Count > 0)
                {
                    n = new TreeNode("PrimaryKeys");
                    n.Tag = new NodeData(NodeType.PRIMARYKEYS, table.PrimaryKeys);
                    n.SelectedImageIndex = n.ImageIndex = 16;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }
            }
        }

        private void ExpandViews(IViews views, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (IView view in views)
                {
                    TreeNode n = new TreeNode(view.Name);
                    n.Tag = new NodeData(NodeType.VIEW, view);
                    n.SelectedImageIndex = n.ImageIndex = 6;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }
            }
        }

        private void ExpandView(IView view, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                TreeNode n;

                if (view.Columns.Count > 0)
                {
                    n = new TreeNode("Columns");
                    n.Tag = new NodeData(NodeType.COLUMNS, view.Columns);
                    n.SelectedImageIndex = n.ImageIndex = 9;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }

                if (view.SubTables.Count > 0)
                {
                    n = new TreeNode("SubTables");
                    n.Tag = new NodeData(NodeType.SUBTABLES, view.SubTables);
                    n.SelectedImageIndex = n.ImageIndex = 2;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }

                if (view.SubViews.Count > 0)
                {
                    n = new TreeNode("SubViews");
                    n.Tag = new NodeData(NodeType.SUBVIEWS, view.SubViews);
                    n.SelectedImageIndex = n.ImageIndex = 5;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }
            }
        }

        private void ExpandProcedures(IProcedures procedures, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (IProcedure procedure in procedures)
                {
                    TreeNode n = new TreeNode(procedure.Name);
                    n.Tag = new NodeData(NodeType.PROCEDURE, procedure);
                    n.SelectedImageIndex = n.ImageIndex = 8;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }
            }
        }

        private void ExpandProcedure(IProcedure procedure, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                TreeNode n;

                if (procedure.Parameters.Count > 0)
                {
                    n = new TreeNode("Parameters");
                    n.Tag = new NodeData(NodeType.PARAMETERS, procedure.Parameters);
                    n.SelectedImageIndex = n.SelectedImageIndex = n.ImageIndex = 17;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }

                if (procedure.ResultColumns.Count > 0)
                {
                    n = new TreeNode("ResultColumns");
                    n.Tag = new NodeData(NodeType.RESULTCOLUMNS, procedure.ResultColumns);
                    n.SelectedImageIndex = n.ImageIndex = 19;
                    node.Nodes.Add(n);
                    n.Nodes.Add(this.BlankNode);
                }
            }
        }

        private void ExpandColumns(IColumns columns, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (IColumn column in columns)
                {
                    TreeNode n = new TreeNode(column.Name);
                    n.Tag = new NodeData(NodeType.COLUMN, column);

                    if (!column.IsInPrimaryKey)
                        n.SelectedImageIndex = n.ImageIndex = 10;
                    else
                        n.SelectedImageIndex = n.ImageIndex = 26;

                    node.Nodes.Add(n);

                    if (column.ForeignKeys.Count > 0)
                    {
                        TreeNode nn = new TreeNode("ForeignKeys");
                        nn.Tag = new NodeData(NodeType.FOREIGNKEYS, column.ForeignKeys);
                        nn.SelectedImageIndex = nn.ImageIndex = 11;
                        n.Nodes.Add(nn);
                        nn.Nodes.Add(this.BlankNode);
                    }
                }
            }
        }

        private void ExpandParameters(IParameters parameters, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (IParameter parameter in parameters)
                {
                    TreeNode n = new TreeNode(parameter.Name);
                    n.Tag = new NodeData(NodeType.PARAMETER, parameter);
                    n.SelectedImageIndex = n.ImageIndex = 18;
                    node.Nodes.Add(n);
                }
            }
        }

        private void ExpandResultColumns(IResultColumns resultColumns, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (IResultColumn resultColumn in resultColumns)
                {
                    TreeNode n = new TreeNode(resultColumn.Name);
                    n.Tag = new NodeData(NodeType.RESULTCOLUMN, resultColumn);
                    n.SelectedImageIndex = n.ImageIndex = 20;
                    node.Nodes.Add(n);
                }
            }
        }

        private void ExpandIndexes(IIndexes indexes, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (IIndex index in indexes)
                {
                    TreeNode indexNode = new TreeNode(index.Name);
                    indexNode.Tag = new NodeData(NodeType.INDEX, index);
                    indexNode.SelectedImageIndex = indexNode.ImageIndex = 15;
                    node.Nodes.Add(indexNode);

                    if (index.Columns.Count > 0)
                    {
                        TreeNode n = new TreeNode("Columns");
                        n.Tag = new NodeData(NodeType.COLUMNS, index.Columns);
                        n.SelectedImageIndex = n.ImageIndex = 9;
                        indexNode.Nodes.Add(n);
                        n.Nodes.Add(this.BlankNode);
                    }
                }
            }
        }

        private void ExpandForeignKeys(IForeignKeys foreignKeys, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (IForeignKey foreignKey in foreignKeys)
                {
                    TreeNode n;
                    TreeNode fkNode = new TreeNode(foreignKey.Name);
                    fkNode.Tag = new NodeData(NodeType.FOREIGNKEY, foreignKey);
                    fkNode.SelectedImageIndex = fkNode.ImageIndex = 12;
                    node.Nodes.Add(fkNode);

                    if (foreignKey.PrimaryColumns.Count > 0)
                    {
                        n = new TreeNode("PrimaryColumns");
                        n.Tag = new NodeData(NodeType.COLUMNS, foreignKey.PrimaryColumns);
                        n.SelectedImageIndex = n.ImageIndex = 9;
                        fkNode.Nodes.Add(n);
                        n.Nodes.Add(this.BlankNode);
                    }

                    if (foreignKey.ForeignColumns.Count > 0)
                    {
                        n = new TreeNode("ForeignColumns");
                        n.Tag = new NodeData(NodeType.COLUMNS, foreignKey.ForeignColumns);
                        n.SelectedImageIndex = n.ImageIndex = 9;
                        fkNode.Nodes.Add(n);
                        n.Nodes.Add(this.BlankNode);
                    }
                }
            }
        }

        private void ExpandDomains(IDomains domains, TreeNode node)
        {
            if (HasBlankNode(node))
            {
                foreach (IDomain domain in domains)
                {
                    TreeNode n = new TreeNode(domain.Name);
                    n.Tag = new NodeData(NodeType.DOMAIN, domain);
                    n.SelectedImageIndex = n.ImageIndex = 25;
                    node.Nodes.Add(n);
                }
            }
        }

        private bool HasBlankNode(TreeNode node)
        {
            if (node.Nodes.Count == 1 && "Blank" == node.Nodes[0].Tag as string)
            {
                node.Nodes.Clear();
                return true;
            }
            else
                return false;
        }

        private void tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                BeforeNodeSelected(e.Node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region esMetadataEngine.Collection Logic

        public void EditNiceNames(Collection coll)
        {
            try
            {
                DataTable dt = ProLogForNiceNames(coll);
                DataRowCollection rows = dt.Rows;

                foreach (EntitySpaces.MetadataEngine.Single o in coll)
                {
                    rows.Add(new object[] { o.Name, o.Alias });
                }

                EpilogForNiceNames(dt);

                dt.ExtendedProperties["Collection"] = coll;
            }
            catch { }
        }

        private DataTable ProLogForNiceNames(Collection coll)
        {
            DataTable dt = new DataTable();
            try
            {
                DataColumn k = dt.Columns.Add("Name", typeof(string));
                k.AllowDBNull = false;
                k.ReadOnly = true;
                DataColumn v = dt.Columns.Add("Alias", typeof(string));
                v.AllowDBNull = false;
            }
            catch { }

            return dt;
        }

        private void EpilogForNiceNames(DataTable dt)
        {
            try
            {
                dt.DefaultView.AllowNew = false;
                dt.DefaultView.AllowDelete = false;

                this.Grid.DataSource = null;
                this.Grid.DataSource = dt.DefaultView;

                dt.RowChanged += new DataRowChangeEventHandler(CollectionGridRowChanged);
            }
            catch { }
        }

        private void CollectionGridRowChanged(object sender, DataRowChangeEventArgs e)
        {
            try
            {
                Collection coll = e.Row.Table.ExtendedProperties["Collection"] as Collection;

                if (coll != null)
                {
                    foreach (EntitySpaces.MetadataEngine.Single o in coll)
                    {
                        if (o.Name == e.Row[0] as string)
                        {
                            this.isDirty = true;
                            o.Alias = e.Row[1] as string;
                        }
                    }
                }
            }
            catch { }
        }

        public void EditSingle(EntitySpaces.MetadataEngine.Single obj, string niceName)
        {
            try
            {
                this.single = obj;

                DataTable dt = new DataTable();

                DataColumn k = dt.Columns.Add("Key", typeof(string));
                k.Unique = true;
                DataColumn v = dt.Columns.Add("Value", typeof(string));
                k.AllowDBNull = false;

                IPropertyCollection properties = obj.Properties;
                DataRowCollection rows = dt.Rows;

                foreach (IProperty prop in properties)
                {
                    rows.Add(new object[] { prop.Key, prop.Value });
                }

                dt.AcceptChanges();

                this.Grid.DataSource = null;
                this.Grid.DataSource = dt.DefaultView;

                dt.ExtendedProperties["EntitySpaces.MetadataEngine.Single"] = obj;

                dt.RowChanged += new DataRowChangeEventHandler(PropertyGridRowChanged);
                dt.RowDeleting += new DataRowChangeEventHandler(PropertyGridRowDeleting);
                dt.RowDeleted += new DataRowChangeEventHandler(PropertyGridRowDeleted);
            }
            catch { }
        }

        private void PropertyGridRowChanged(object sender, DataRowChangeEventArgs e)
        {
            try
            {
                EntitySpaces.MetadataEngine.Single o = e.Row.Table.ExtendedProperties["EntitySpaces.MetadataEngine.Single"] as EntitySpaces.MetadataEngine.Single;

                if (o != null)
                {
                    try
                    {
                        string sKey = e.Row["Key"] as string;
                        if (sKey == null || sKey.Length == 0)
                        {
                            MessageBox.Show("Requires a 'Key' or this record won't be saved");
                            return;
                        }

                        string sValue = e.Row["Value"] as string;
                        if (sValue == null || sValue.Length == 0)
                        {
                            MessageBox.Show("Requires a 'Value' or this record won't be saved");
                            return;
                        }

                        IProperty p = o.Properties[sKey];

                        if (p != null)
                        {
                            p.Value = sValue;
                        }
                        else
                        {
                            o.Properties.AddKeyValue(sKey, sValue);
                        }

                        this.isDirty = true;
                    }
                    catch { }
                }
            }
            catch { }
        }

        private void PropertyGridRowDeleting(object sender, DataRowChangeEventArgs e)
        {
            try
            {
                EntitySpaces.MetadataEngine.Single o = e.Row.Table.ExtendedProperties["EntitySpaces.MetadataEngine.Single"] as EntitySpaces.MetadataEngine.Single;

                if (o != null)
                {
                    try
                    {
                        this.isDirty = true;
                        o.Properties.RemoveKey(e.Row["Key"] as string);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private void PropertyGridRowDeleted(object sender, DataRowChangeEventArgs e)
        {
            try
            {
                EntitySpaces.MetadataEngine.Single o = e.Row.Table.ExtendedProperties["EntitySpaces.MetadataEngine.Single"] as EntitySpaces.MetadataEngine.Single;

                if (o != null)
                {
                    try
                    {
                        DataTable dt = e.Row.Table;

                        dt.RowChanged -= new DataRowChangeEventHandler(this.PropertyGridRowChanged);
                        dt.RowDeleting -= new DataRowChangeEventHandler(this.PropertyGridRowDeleting);
                        dt.RowDeleted -= new DataRowChangeEventHandler(this.PropertyGridRowDeleted);

                        dt.AcceptChanges();

                        dt.RowChanged += new DataRowChangeEventHandler(this.PropertyGridRowChanged);
                        dt.RowDeleting += new DataRowChangeEventHandler(this.PropertyGridRowDeleting);
                        dt.RowDeleted += new DataRowChangeEventHandler(this.PropertyGridRowDeleted);

                    }
                    catch { }
                }
            }
            catch { }
        }

        #endregion

        public void InitializeTree()
        {
            Cursor cursor = Cursor.Current;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                this.tree.Nodes.Clear();

                TreeNode rootNode = null;

                if (root == null)
                {
                    rootNode = new TreeNode("Invalid Connection ...");
                    rootNode.SelectedImageIndex = rootNode.ImageIndex = 21;
                    this.tree.Nodes.Add(rootNode);
                    return;
                }

                this.tree.BeginUpdate();

                // ROOT NODE
                rootNode = new TreeNode("esMeta (" + root.DriverString + ")");
                rootNode.Tag = new NodeData(NodeType.ESMETADATAENGINE, this.root);
                rootNode.SelectedImageIndex = rootNode.ImageIndex = 21;
                this.tree.Nodes.Add(rootNode);

                // DATABASES
                TreeNode databasesNode = new TreeNode("Databases");
                databasesNode.Tag = new NodeData(NodeType.DATABASES, root.Databases);
                databasesNode.SelectedImageIndex = databasesNode.ImageIndex = 0;

                rootNode.Nodes.Add(databasesNode);

                foreach (IDatabase database in root.Databases)
                {
                    TreeNode dbNode = new TreeNode(database.Name);
                    dbNode.Tag = new NodeData(NodeType.DATABASE, database);
                    dbNode.SelectedImageIndex = dbNode.ImageIndex = 1;
                    dbNode.Nodes.Add(this.BlankNode);
                    databasesNode.Nodes.Add(dbNode);
                }

                rootNode.Expand();

                this.tree.EndUpdate();
            }
            catch
            {

            }
            finally
            {
                Cursor.Current = cursor;
            }
        }

        private TreeNode BlankNode
        {
            get
            {
                TreeNode blankNode = new TreeNode("");
                blankNode.Tag = "Blank";
                return blankNode;
            }
        }

        private void esUserData_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (this.isDirty)
                {
                    DialogResult result = MessageBox.Show("Save your Changes?", "Unsaved Changes", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        this.Grid.EndEdit();
                        root.SaveUserMetaData();
                        this.isDirty = false;
                    }
                }
            }
            catch { }
        }

        private void Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.Cancel = true;
        }

        private void esUserDataDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.isDirty)
                {
                    DialogResult result = MessageBox.Show("Save your Changes?", "Unsaved Changes", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        this.Grid.EndEdit();
                        root.SaveUserMetaData();
                        this.isDirty = false;
                    }
                }
            }
            catch { }
        }

        private void BeforeNodeSelected(TreeNode node)
        {
            if (node != null)
            {
                if (node.Tag == null) return;

                NodeData data = node.Tag as NodeData;
                MetaObject obj = null;

                if (data.Type != NodeType.ESMETADATAENGINE)
                {
                    obj = data.Meta as MetaObject;
                }

                if (data != null)
                {
                    switch (data.Type)
                    {
                        case NodeType.COLUMNS:
                            this.EditNiceNames(data.Meta as Columns);
                            break;

                        case NodeType.DATABASES:
                            this.EditNiceNames(data.Meta as Databases);
                            break;

                        case NodeType.TABLES:
                        case NodeType.SUBTABLES:
                            this.EditNiceNames(data.Meta as Tables);
                            break;

                        case NodeType.VIEWS:
                        case NodeType.SUBVIEWS:
                            this.EditNiceNames(data.Meta as Views);
                            break;

                        case NodeType.FOREIGNKEYS:
                        case NodeType.INDIRECTFOREIGNKEYS:
                            this.EditNiceNames(data.Meta as ForeignKeys);
                            break;

                        case NodeType.PARAMETERS:
                            this.EditNiceNames(data.Meta as Parameters);
                            break;

                        case NodeType.RESULTCOLUMNS:
                            this.EditNiceNames(data.Meta as ResultColumns);
                            break;

                        case NodeType.INDEXES:
                            this.EditNiceNames(data.Meta as Indexes);
                            break;

                        case NodeType.PROCEDURES:
                            this.EditNiceNames(data.Meta as Procedures);
                            break;

                        case NodeType.DOMAINS:
                            this.EditNiceNames(data.Meta as Domains);
                            break;

                        default:
                            this.Grid.DataSource = null;
                            break;
                    }

                    switch (data.Type)
                    {
                        case NodeType.DATABASE:
                            {
                                Database o = obj as Database;
                                metadataProperties.DisplayDatabaseProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.COLUMN:
                            {
                                Column o = obj as Column;
                                metadataProperties.DisplayColumnProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.TABLE:
                            {

                                Table o = obj as Table;
                                metadataProperties.DisplayTableProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.VIEW:
                            {

                                EntitySpaces.MetadataEngine.View o = obj as EntitySpaces.MetadataEngine.View;
                                metadataProperties.DisplayViewProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.PARAMETER:
                            {

                                Parameter o = obj as Parameter;
                                metadataProperties.DisplayParameterProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.RESULTCOLUMN:
                            {

                                ResultColumn o = obj as ResultColumn;
                                metadataProperties.DisplayResultColumnProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.FOREIGNKEY:
                            {
                                ForeignKey o = obj as ForeignKey;
                                metadataProperties.DisplayForeignKeyProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.INDEX:
                            {
                                Index o = obj as Index;
                                metadataProperties.DisplayIndexProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.PROCEDURE:
                            {
                                Procedure o = obj as Procedure;
                                metadataProperties.DisplayProcedureProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        case NodeType.DOMAIN:
                            {
                                Domain o = obj as Domain;
                                metadataProperties.DisplayDomainProperties(o, node);
                                this.EditSingle(o, o.Alias);
                            }
                            break;

                        default:
                            metadataProperties.Clear();
                            break;
                    }
                }
            }
        }

        private void tree_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                TreeNode node = tree.GetNodeAt(e.X, e.Y) as TreeNode;
                if ((node != null) && (node == tree.SelectedNode))
                {
                    try
                    {
                        BeforeNodeSelected(node);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private void ToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            Cursor origCursor = this.Cursor;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                object data = this.Grid.DataSource;

                try
                {
                    this.PropertyNiceNameLeave(null, null);
                    this.Grid.DataSource = null;
                    this.root.SaveUserMetaData();
                }
                catch (Exception ex)
                {
                    MainWindow.ShowError(ex);
                }
                finally
                {
                    this.Grid.DataSource = data;
                }
            }
            finally
            {
                this.Cursor = origCursor;
            }
        }

        private void PropertyNiceNameLeave(object sender, EventArgs e)
        {
            try
            {
                this.single = null;
            }
            catch { }
        }
    }

    class NodeData
    {
        public NodeData(NodeType type, object meta)
        {
            this.Type = type;
            this.Meta = meta;
        }

        public NodeType Type = NodeType.BLANK;
        public object Meta = null;
    }

    enum NodeType
    {
        BLANK,
        ESMETADATAENGINE,
        DATABASES,
        DATABASE,
        TABLES,
        TABLE,
        VIEWS,
        VIEW,
        SUBVIEWS,
        SUBTABLES,
        PROCEDURES,
        PROCEDURE,
        COLUMNS,
        COLUMN,
        FOREIGNKEYS,
        INDIRECTFOREIGNKEYS,
        FOREIGNKEY,
        INDEXES,
        INDEX,
        PRIMARYKEYS,
        PRIMARKYKEY,
        PARAMETERS,
        PARAMETER,
        RESULTCOLUMNS,
        RESULTCOLUMN,
        DOMAINS,
        DOMAIN,
        PROVIDERTYPE,
        PROVIDERTYPES
    }
}

