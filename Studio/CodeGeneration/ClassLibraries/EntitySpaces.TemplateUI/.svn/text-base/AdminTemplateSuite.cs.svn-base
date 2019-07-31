using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EntitySpaces.AddIn.TemplateUI;
using EntitySpaces.MetadataEngine;

namespace EntitySpaces.TemplateUI
{
    public partial class AdminTemplateSuite : UserControl, ITemplateUI
    {
        private Root esMeta;
        private bool UseCachedSettings;
        private object applicationObject;

        private LookupColumns lookupColumns = new LookupColumns();
        private DetailGridSortInfoCollection detailGridSortInfo = new DetailGridSortInfoCollection();
        private ArrayList detailTitleColumns = new ArrayList();
        private DetailGridInfo detailGridsinfo = new DetailGridInfo();

        private IDatabase database = null;
        private ITable table = null;

        public AdminTemplateSuite()
        {
            InitializeComponent();
        }

        #region ITemplateUI Members

        esTemplateInfo ITemplateUI.Init()
        {
            esTemplateInfo info = new esTemplateInfo();
            info.UserInterface = this;
            info.UserInterfaceId = new Guid("D4CCF86A-2911-4598-AB11-B4B39A0ABF9A");
            info.TabTitle = "Web Admin Grids";
            info.TabOrder = 0;
            return info;
        }

        UserControl ITemplateUI.CreateInstance(Root esMeta, bool cachedSettings, object applicationObject)
        {
            AdminTemplateSuite window = new AdminTemplateSuite();
            window.esMeta = esMeta;
            window.applicationObject = applicationObject;
            window.UseCachedSettings = cachedSettings;

            return window;
        }

        bool ITemplateUI.OnExecute()
        {
            try
            {
                string sep;
                string columns;

                esMeta.Input["playback"] = "yes";
                esMeta.Input["database"] = database.Name;
                esMeta.Input["table"] = table.Name;
                esMeta.Input["chkRawNames"] = this.chkRawNames.Checked.ToString();
                esMeta.Input["IsForDnn"] = this.chkIsForDnn.Checked.ToString();

                //--------------------------------------------
                // Browse Columns
                //--------------------------------------------
                string browseView = this.cboxBrowseViews.SelectedItem as String;
                if (browseView == null || browseView == "<None>")
                {
                    browseView = table.Name;
                }
                esMeta.Input["browseView"] = browseView;

                sep = ""; columns = "";
                foreach (string s in this.lboxBrowseColumns.SelectedItems)
                {
                    int index = s.IndexOf(',');

                    columns += sep;
                    columns += s.Substring(7, index - 7);
                    sep = "|";
                }
                esMeta.Input["browseColumns"] = columns;

                if (this.cboxBrowseSortCol.SelectedItem != null)
                    esMeta.Input["browseSortCol"] = this.cboxBrowseSortCol.SelectedItem as string;
                else
                    esMeta.Input["browseSortCol"] = "";

                if (this.cboxBrowseSortDir.SelectedItem != null)
                    esMeta.Input["browseSortDir"] = this.cboxBrowseSortDir.SelectedItem as string;
                else
                    esMeta.Input["browseSortDir"] = "";

                //--------------------------------------------
                // Detail Columns
                //--------------------------------------------
                sep = ""; columns = "";
                foreach (string s in this.lboxDetailColumns.SelectedItems)
                {
                    int index = s.IndexOf(',');

                    columns += sep;
                    columns += s.Substring(7, index - 7);
                    sep = "|";
                }
                esMeta.Input["detailColumns"] = columns;

                //--------------------------------------------
                // Edit Columns
                //--------------------------------------------
                sep = ""; columns = "";
                foreach (string s in this.lboxEditColumns.SelectedItems)
                {
                    int index = s.IndexOf(',');

                    columns += sep;
                    columns += s.Substring(7, index - 7);
                    sep = "|";
                }
                esMeta.Input["editColumns"] = columns;

                //--------------------------------------------
                // Search Columns
                //--------------------------------------------
                sep = ""; columns = "";
                foreach (string s in this.lboxSearchColumns.SelectedItems)
                {
                    int index = s.IndexOf(',');

                    columns += sep;
                    columns += s.Substring(7, index - 7);
                    sep = "|";
                }
                esMeta.Input["searchColumns"] = columns;


                sep = ""; columns = "";
                for (int i = 0; i < detailTitleColumns.Count; i++)
                {
                    columns += sep;
                    columns += detailTitleColumns[i] as string;
                    sep = "|";
                }
                esMeta.Input["detailTitleColumns"] = columns;

                esMeta.Input["outputPath"] = this.txtOutputPath.Text;
                esMeta.Input["namespace"] = this.txtNamespace.Text;
                esMeta.Input["lookupColumns"] = this.lookupColumns.ToString();

                if (this.txtPageSize.Text.Length > 0)
                    esMeta.Input["pageSize"] = this.txtPageSize.Text;
                else
                    esMeta.Input["pageSize"] = "20";

                // We gotta filter the detail grids to only those 'checked' and in
                // the specific order
                ArrayList grids = new ArrayList();
                for (int i = 0; i < this.chklistDetailGrids.Items.Count; i++)
                {
                    bool isChecked = this.chklistDetailGrids.GetItemChecked(i);
                    if (isChecked)
                    {
                        string grid = this.chklistDetailGrids.Items[i].ToString();
                        grids.Add(grid);
                    }
                }

                // Does the filter.
                this.detailGridsinfo.TheseGridsOnly(grids);
                esMeta.Input["detailGridsinfo"] = this.detailGridsinfo.ToString();
                esMeta.Input["detailGridSortInfo"] = this.detailGridSortInfo.ToString();
            }
            catch { }

            return true;
        }

        void ITemplateUI.OnCancel()
        {

        }

        #endregion

        protected void BindFromProjectData()
        {
            // Selected tab Doesn't Matter on these !!
            string temp = esMeta.Input["lookupColumns"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.lookupColumns.FromString(temp);
            }

            temp = esMeta.Input["detailGridSortInfo"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.detailGridSortInfo.FromString(temp);
            }

            // Must select tabs for these

            this.tabs.SelectedTab = this.tabSettings;
            temp = esMeta.Input["outputPath"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.txtOutputPath.Text = temp;
            }

            temp = esMeta.Input["namespace"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.txtNamespace.Text = temp;
            }

            temp = esMeta.Input["pageSize"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.txtPageSize.Text = temp;
            }

            temp = esMeta.Input["chkRawNames"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.chkRawNames.Checked = Convert.ToBoolean(temp);
            }

            temp = esMeta.Input["IsForDnn"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.chkIsForDnn.Checked = Convert.ToBoolean(temp);
            }

            temp = esMeta.Input["table"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.lboxTables.SelectedIndex = this.lboxTables.FindStringExact(temp);
            }

            ArrayList tempList = new ArrayList();

            temp = esMeta.Input["browseView"] as string;
            if (temp != null && temp.Length > 0)
            {
                int selIndex = this.cboxBrowseViews.FindStringExact(temp);
                if (selIndex != -1)
                {
                    this.cboxBrowseViews.SelectedIndex = selIndex;
                    this.cboxBrowseViews_SelectionChangeCommitted(null, null);
                }
            }

            temp = esMeta.Input["browseColumns"] as string;
            tempList.Clear();
            PopulateColumns(temp, tempList);

            for (int i = 0; i < this.lboxBrowseColumns.Items.Count; i++)
            {
                string s = this.lboxBrowseColumns.Items[i] as string;
                int index = s.IndexOf(',');
                s = s.Substring(7, index - 7);

                if (tempList.Contains(s))
                {
                    this.lboxBrowseColumns.SetSelected(i, true);
                }
            }

            temp = esMeta.Input["browseSortCol"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.cboxBrowseSortCol.SelectedItem = temp;
            }

            temp = esMeta.Input["browseSortDir"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.cboxBrowseSortDir.SelectedItem = temp;
            }

            temp = esMeta.Input["detailColumns"] as string;
            tempList.Clear();
            PopulateColumns(temp, tempList);

            for (int i = 0; i < this.lboxDetailColumns.Items.Count; i++)
            {
                string s = this.lboxDetailColumns.Items[i] as string;
                int index = s.IndexOf(',');
                s = s.Substring(7, index - 7);

                if (tempList.Contains(s))
                {
                    this.lboxDetailColumns.SetSelected(i, true);
                }
            }

            temp = esMeta.Input["detailTitleColumns"] as string;
            detailTitleColumns = new ArrayList();
            PopulateColumns(temp, detailTitleColumns);

            string data = "";
            for (int i = 0; i < detailTitleColumns.Count; i++)
            {
                data += detailTitleColumns[i] as string;
                data += "\r\n";
            }
            this.txtDetailEditTitle.Text = data;


            temp = esMeta.Input["editColumns"] as string;
            tempList.Clear();
            PopulateColumns(temp, tempList);

            for (int i = 0; i < this.lboxEditColumns.Items.Count; i++)
            {
                string s = this.lboxEditColumns.Items[i] as string;
                int index = s.IndexOf(',');
                s = s.Substring(7, index - 7);

                if (tempList.Contains(s))
                {
                    this.lboxEditColumns.SetSelected(i, true);
                }
            }

            temp = esMeta.Input["detailGridsinfo"] as string;
            if (temp != null && temp.Length > 0)
            {
                this.detailGridsinfo.FromString(temp);
            }

            for (int i = 0; i < this.chklistDetailGrids.Items.Count; i++)
            {
                string grid = this.chklistDetailGrids.Items[i] as string;

                if (detailGridsinfo.Grids.Contains(grid))
                {
                    this.chklistDetailGrids.SetItemChecked(i, true);
                }
            }

            foreach (EntitySpaces.MetadataEngine.IForeignKey fk in table.ForeignKeys)
            {
                EntitySpaces.MetadataEngine.TableRelation tr = new EntitySpaces.MetadataEngine.TableRelation(table, fk);
                if (tr.IsZeroToMany)
                {
                    string grid = tr.ForeignTable.Name + "->" + fk.ForeignColumns[0].Name;

                    if (!detailGridsinfo.Grids.Contains(grid))
                    {
                        this.detailGridsinfo.AddGrid(grid, fk.Name);
                    }
                }
            }

            temp = esMeta.Input["searchColumns"] as string;

            if (temp != null && temp.Length > 0)
            {
                tempList.Clear();
                PopulateColumns(temp, tempList);

                for (int i = 0; i < this.lboxSearchColumns.Items.Count; i++)
                {
                    string s = this.lboxSearchColumns.Items[i] as string;
                    int index = s.IndexOf(',');
                    s = s.Substring(7, index - 7);

                    if (tempList.Contains(s))
                    {
                        this.lboxSearchColumns.SetSelected(i, true);
                    }
                }
            }
        }

        private void PopulateColumns(string data, ArrayList columns)
        {
            if (data.Length != 0)
            {
                string[] array = data.Split(new char[] { '|' });
                foreach (string col in array)
                {
                    columns.Add(col.Trim());
                }
            }
        }

        private void AdminTemplateSuite_Load(object sender, EventArgs e)
        {
            //-----------------------------------------------------------
            // OutputPath
            //-----------------------------------------------------------
            this.txtOutputPath.Text = (string)esMeta.Input["OutputPath"];

            BindDatabase();
            BindTables();

            if (UseCachedSettings)
            {
                BindFromProjectData();
            }
        }

        private void BindDatabase()
        {
            this.cboxDatabases.Items.Clear();
            foreach (IDatabase db in esMeta.Databases)
            {
                this.cboxDatabases.Items.Add(db.Name);
            }

            if (esMeta.DefaultDatabase != null)
            {
                this.database = esMeta.DefaultDatabase;
                this.cboxDatabases.SelectedIndex = this.cboxDatabases.FindStringExact(esMeta.DefaultDatabase.Name);
            }
        }

        private void PopulateColumns(ListBox lbox)
        {
            ITable table = this.lboxTables.SelectedValue as ITable;
            PopulateColumns(lbox, table);
        }

        private void PopulateColumns(ListBox lbox, ITable theTable)
        {
            int maxColLen = 0;

            lbox.Items.Clear();
            if (theTable != null)
            {
                string name;

                foreach (IColumn col in theTable.Columns)
                {
                    maxColLen = Math.Max(col.Name.Length, maxColLen);
                }

                foreach (IColumn col in theTable.Columns)
                {
                    name = "";
                    name += !col.IsNullable ? 'R' : ' ';
                    name += col.IsInPrimaryKey ? 'P' : ' ';
                    name += col.IsAutoKey ? 'A' : ' ';

                    if (!col.IsInPrimaryKey)
                    {
                        if (col.IsInForeignKey)
                        {
                            foreach (IForeignKey fk in theTable.ForeignKeys)
                            {
                                TableRelation tr = new TableRelation(theTable, fk);
                                if (tr.IsLookup)
                                {
                                    name += 'F';
                                    break;
                                }
                            }
                        }
                    }

                    name = name.PadRight(7, ' ');
                    name += col.Name + ",";
                    name = name.PadRight(maxColLen + 9, ' ');
                    name += col.DataTypeNameComplete;

                    lbox.Items.Add(name);
                }
            }
        }

        private void PopulateColumns(ListBox lbox, IView theView)
        {
            int maxColLen = 0;

            lbox.Items.Clear();
            if (theView != null)
            {
                string name;

                foreach (IColumn col in theView.Columns)
                {
                    maxColLen = Math.Max(col.Name.Length, maxColLen);
                }

                foreach (IColumn col in theView.Columns)
                {
                    name = "";
                    name += !col.IsNullable ? 'R' : ' ';
                    name += col.IsInPrimaryKey ? 'P' : ' ';
                    name += col.IsAutoKey ? 'A' : ' ';

                    name = name.PadRight(7, ' ');
                    name += col.Name + ",";
                    name = name.PadRight(maxColLen + 9, ' ');
                    name += col.DataTypeNameComplete;

                    lbox.Items.Add(name);
                }
            }
        }

        private void cboxDatabases_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            string databaseName = this.cboxDatabases.SelectedItem as string;

            if (database == null || database.Name != databaseName)
            {
                database = this.esMeta.Databases[databaseName];
                if (database != null)
                {
                    this.cboxDatabases.SelectedText = database.Name;
                }

                BindTables();
            }
        }

        #region Tables 'Tab'

        private void BindTables()
        {
            if (database != null)
            {
                this.lboxTables.BindingContext = new BindingContext();
                this.lboxTables.DataSource = database.Tables;
                this.lboxTables.DisplayMember = "Name";
            }
        }

        private void lboxTables_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ITable newTable = this.lboxTables.SelectedItem as ITable;

            if (table == null || table.Name != newTable.Name)
            {
                table = newTable;

                BindBrowseColumns();
                BindBrowseSortColumns();
                BindBrowseViews();
                BindDetailColumns();
                BindDetailLookups();
                BindDetailGrids();
                BindEditColumns();
                BindSearchColumns();
            }
        }

        #endregion

        #region Browse 'Tab'

        private void BindBrowseColumns()
        {
            PopulateColumns(this.lboxBrowseColumns);
        }

        private void btnBrowseDN_Click(object sender, System.EventArgs e)
        {
            btnDn(this.lboxBrowseColumns);
        }

        private void btnBrowseUP_Click(object sender, System.EventArgs e)
        {
            btnUp(this.lboxBrowseColumns);
        }

        private void BindBrowseViews()
        {
            this.cboxBrowseViews.Items.Clear();
            string pkName = this.table.PrimaryKeys[0].Name;

            foreach (IView view in database.Views)
            {
                IColumn col = view.Columns[pkName];
                if (col != null)
                {
                    this.cboxBrowseViews.Items.Add(view.Name);
                }
            }
            this.cboxBrowseViews.Items.Insert(0, "<None>");
        }

        private void cboxBrowseViews_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            string view = this.cboxBrowseViews.SelectedItem as string;
            if (view != "<None>")
            {
                this.PopulateColumns(this.lboxBrowseColumns, this.database.Views[view]);
                this.PopulateColumns(this.lboxSearchColumns, this.database.Views[view]);
            }
            else
            {
                this.PopulateColumns(this.lboxBrowseColumns);
                this.PopulateColumns(this.lboxSearchColumns);
            }

            this.BindBrowseSortColumns();
        }

        #endregion

        #region Detail 'Tab'

        private void BindDetailColumns()
        {
            PopulateColumns(this.lboxDetailColumns);
        }

        private void btnDetailUP_Click(object sender, System.EventArgs e)
        {
            btnUp(this.lboxDetailColumns);
        }

        private void btnDetailDN_Click(object sender, System.EventArgs e)
        {
            btnDn(this.lboxDetailColumns);
        }

        #endregion

        #region Edit 'Tab'

        private void BindEditColumns()
        {
            PopulateColumns(this.lboxEditColumns);
        }

        private void btnEditUP_Click(object sender, System.EventArgs e)
        {
            btnUp(this.lboxEditColumns);
        }

        private void btnEditDN_Click(object sender, System.EventArgs e)
        {
            btnDn(this.lboxEditColumns);
        }

        #endregion

        #region Settings 'Tab'

        private void BindBrowseSortColumns()
        {
            IColumns cols = null;

            string view = this.cboxBrowseViews.SelectedItem as string;
            if (view != null && view != "<None>")
                cols = this.database.Views[view].Columns;
            else
                cols = table.Columns;

            this.cboxBrowseSortCol.Items.Clear();
            foreach (IColumn col in cols)
            {
                this.cboxBrowseSortCol.Items.Add(col.Name);
            }

            this.cboxBrowseSortDir.SelectedItem = "Ascending";
        }

        private void btnPath_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.pathFinder.SelectedPath = this.txtOutputPath.Text;
                if (this.pathFinder.ShowDialog(this) == DialogResult.OK)
                {
                    this.txtOutputPath.Text = this.pathFinder.SelectedPath;
                }
            }
            catch { }
        }

        #endregion

        #region Detail Lookups 'Tab'

        private void BindDetailLookups()
        {
            this.lboxDetailLookupColumns.Items.Clear();
            this.lboxDetailLookups.Items.Clear();

            foreach (IForeignKey fk in table.ForeignKeys)
            {
                TableRelation tr = new TableRelation(table, fk);
                if (tr.IsLookup)
                {
                    this.lboxDetailLookups.Items.Add(tr.PrimaryColumns[0].Name);
                }
            }
        }

        private void lboxDetailLookups_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.txtDetailLookupColumns.Clear();
            this.lboxDetailLookupColumns.Items.Clear();

            string columnName = this.lboxDetailLookups.SelectedItem as string;

            ITable lookupTable = this.GetLookupTable(columnName);

            foreach (IColumn col in lookupTable.Columns)
            {
                this.lboxDetailLookupColumns.Items.Add(col.Name);
            }

            this.DisplayLookupColumnsChosen();
        }

        private void btnAddLookupColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                string columnName = this.lboxDetailLookups.SelectedItem as string;
                if (columnName != null)
                {
                    string col = this.lboxDetailLookupColumns.SelectedItem as string;
                    if (col != null)
                    {
                        ArrayList list = this.lookupColumns[columnName];
                        if (list == null)
                        {
                            IForeignKey fk = GetLookupForeignKey(columnName);
                            this.lookupColumns.Add(columnName, fk.Name);
                            list = this.lookupColumns[columnName];
                        }

                        if (this.lookupColumns.Add(columnName, col))
                        {
                            DisplayLookupColumnsChosen(list);
                        }
                    }
                }
            }
            catch { }
        }

        private void btnRemoveLookupColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                string columnName = this.lboxDetailLookups.SelectedItem as string;
                if (columnName != null)
                {
                    string col = this.lboxDetailLookupColumns.SelectedItem as string;
                    if (col != null)
                    {
                        this.lookupColumns.Remove(columnName, col);

                        ArrayList list = this.lookupColumns[columnName];
                        if (list.Count == 1)
                        {
                            this.lookupColumns[columnName] = null;
                            this.txtDetailLookupColumns.Clear();
                        }
                        else
                        {
                            DisplayLookupColumnsChosen(list);
                        }
                    }
                }
            }
            catch { }
        }

        private void DisplayLookupColumnsChosen()
        {
            string columnName = this.lboxDetailLookups.SelectedItem as string;
            ArrayList columns = this.lookupColumns[columnName];

            DisplayLookupColumnsChosen(columns);
        }

        private void DisplayLookupColumnsChosen(ArrayList columns)
        {
            this.txtDetailLookupColumns.Clear();

            if (columns != null)
            {
                string data = "";
                for (int i = 1; i < columns.Count; i++)
                {
                    data += columns[i] as string;
                    data += "\r\n";
                }
                this.txtDetailLookupColumns.Text = data;
            }
        }

        private void btnLookupColumnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                string columnName = this.lboxDetailLookups.SelectedItem as string;
                if (columnName != null)
                {
                    this.lookupColumns[columnName] = null;
                }

                this.txtDetailLookupColumns.Clear();
            }
            catch { }
        }

        private void btnDetailTitleAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                string columnName = this.lboxDetailColumns.SelectedItem as string;

                if (columnName != null)
                {
                    int index = columnName.IndexOf(',');
                    columnName = columnName.Substring(7, index - 7);

                    if (!detailTitleColumns.Contains(columnName))
                    {
                        this.detailTitleColumns.Add(columnName);

                        string data = "";
                        for (int i = 0; i < detailTitleColumns.Count; i++)
                        {
                            data += detailTitleColumns[i] as string;
                            data += "\r\n";
                        }
                        this.txtDetailEditTitle.Text = data;
                    }
                }
            }
            catch { }
        }

        private void btnDetailTitleRemove_Click(object sender, System.EventArgs e)
        {
            try
            {
                string columnName = this.lboxDetailColumns.SelectedItem as string;

                if (columnName != null)
                {
                    int index = columnName.IndexOf(',');
                    columnName = columnName.Substring(7, index - 7);

                    if (detailTitleColumns.Contains(columnName))
                    {
                        detailTitleColumns.Remove(columnName);

                        string data = "";
                        for (int i = 0; i < detailTitleColumns.Count; i++)
                        {
                            data += detailTitleColumns[i] as string;
                            data += "\r\n";
                        }
                        this.txtDetailEditTitle.Text = data;
                    }
                }
            }
            catch { }
        }

        #endregion

        #region Detail Grids 'Tab'

        private void BindDetailGrids()
        {
            this.detailGridsinfo.Clear();
            this.chklistDetailGrids.Items.Clear();
            this.lboxDetailGridColumns.Items.Clear();
            this.cboxDetailGridSortCol.Items.Clear();
            this.cboxViews.Items.Clear();

            foreach (IForeignKey fk in table.ForeignKeys)
            {
                TableRelation tr = new TableRelation(table, fk);
                if (tr.IsZeroToMany)
                {
                    string text = tr.ForeignTable.Name + "->" + fk.ForeignColumns[0].Name;
                    this.chklistDetailGrids.Items.Add(text);
                    this.detailGridsinfo.AddGrid(text, fk.Name);
                }
            }

            foreach (IView view in this.database.Views)
            {
                this.cboxViews.Items.Add(view);
            }
            this.cboxViews.Items.Insert(0, "<None>");
            this.cboxViews.DisplayMember = "Name";


        }

        private string GridNameToFkName(string gridName)
        {
            foreach (IForeignKey fk in table.ForeignKeys)
            {
                TableRelation tr = new TableRelation(table, fk);
                if (tr.IsZeroToMany)
                {
                    string text = tr.ForeignTable.Name + "->" + fk.ForeignColumns[0].Name;
                    if (text == gridName) return fk.Name;
                }
            }

            return "";
        }

        private void chklistDetailGrids_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.txtDetailGridColumns.Clear();
                this.lboxDetailGridColumns.Items.Clear();
                this.cboxDetailGridSortCol.Items.Clear();
                this.cboxDetailGridSortDir.Items.Clear();
                this.cboxViewKey.Items.Clear();

                this.cboxViews.Enabled = false;
                this.cboxViewKey.Enabled = false;

                if (this.chklistDetailGrids.SelectedIndex != -1)
                {
                    if (this.chklistDetailGrids.GetItemChecked(this.chklistDetailGrids.SelectedIndex))
                    {
                        this.cboxDetailGridSortDir.Items.Add("Ascending");
                        this.cboxDetailGridSortDir.Items.Add("Descending");

                        this.cboxViews.Enabled = true;

                        string grid = this.chklistDetailGrids.SelectedItem as string;
                        string sourceName = this.detailGridsinfo.GridToSourceName(grid);

                        if (this.detailGridsinfo.IsGridSourceNameView(grid))
                        {
                            string viewName = this.detailGridsinfo.GridToViewName(grid);
                            this.cboxViews.SelectedIndex = this.cboxViews.FindStringExact(viewName);
                            PopulateDetailGridPageFromView(viewName);
                            this.cboxViewKey.SelectedIndex = this.cboxViewKey.FindStringExact(this.detailGridsinfo.GridToViewPrimaryKey(grid));
                        }
                        else
                        {
                            this.cboxViews.SelectedIndex = 0;
                            string fkName = this.detailGridsinfo.GridToForeignKeyName(grid);
                            PopulateDetailGridPageFromForiegnKey(fkName);
                        }
                    }
                    else
                    {
                        this.cboxViews.SelectedIndex = 0;
                    }
                }
                else
                {
                    this.cboxViews.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void PopulateDetailGridPageFromForiegnKey(string fkName)
        {
            IForeignKey fk = table.ForeignKeys[fkName];
            ITable theTable = this.database.Tables[fk.ForeignTable.Name];

            this.PopulateColumns(this.lboxDetailGridColumns, theTable);

            this.DisplayGridDetailLookupColumnsChosen();

            this.PopulateGridDetailSortColumns(theTable.Columns);

            DetailGridSortInfoItem sortItem = this.detailGridSortInfo[fk.Name];
            if (sortItem != null)
            {
                this.cboxDetailGridSortCol.SelectedItem = sortItem.Column;
                this.cboxDetailGridSortDir.SelectedItem = sortItem.Direction;
            }
        }

        private void PopulateDetailGridPageFromView(string viewName)
        {
            string grid = this.chklistDetailGrids.SelectedItem as string;
            string sourceName = this.detailGridsinfo.GridToSourceName(grid);

            IView theView = this.database.Views[viewName];

            this.PopulateColumns(this.lboxDetailGridColumns, theView);

            this.DisplayGridDetailLookupColumnsChosen();

            this.PopulateGridDetailSortColumns(theView.Columns);

            this.cboxViewKey.Enabled = true;
            this.cboxViewKey.Items.Clear();
            foreach (IColumn col in theView.Columns)
            {
                this.cboxViewKey.Items.Add(col);
            }
            this.cboxViewKey.DisplayMember = "Name";

            DetailGridSortInfoItem sortItem = this.detailGridSortInfo[sourceName];
            if (sortItem != null)
            {
                this.cboxDetailGridSortCol.SelectedItem = sortItem.Column;
                this.cboxDetailGridSortDir.SelectedItem = sortItem.Direction;
            }
        }

        private void cboxViews_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            string grid = this.chklistDetailGrids.SelectedItem as string;

            IView theView = this.cboxViews.SelectedItem as IView;
            if (theView != null)
            {
                this.cboxViewKey.Enabled = true;
                string currentSourceName = this.detailGridsinfo.GridToSourceName(grid);
                string newSourceName = "[v]" + theView.Name;
                if (currentSourceName != newSourceName)
                {
                    this.detailGridsinfo.SwapGrid(grid, newSourceName);
                }

                PopulateDetailGridPageFromView(theView.Name);
            }
            else
            {
                this.cboxViewKey.Items.Clear();
                this.cboxViewKey.Enabled = false;
                string fkName = GridNameToFkName(grid);
                this.detailGridsinfo.SwapGrid(grid, fkName);
                PopulateDetailGridPageFromForiegnKey(fkName);
            }
        }

        private void cboxViewKey_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            try
            {
                string grid = this.chklistDetailGrids.SelectedItem as string;
                IColumn col = this.cboxViewKey.SelectedItem as IColumn;
                this.detailGridsinfo.SetViewPrimaryKey(grid, col.Name);
            }
            catch { }
        }

        private void cboxDetailGridSortCol_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            cboxDetailGridSortDirCol_Changed();
        }

        private void cboxDetailGridSortDir_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            cboxDetailGridSortDirCol_Changed();
        }

        private void cboxDetailGridSortDirCol_Changed()
        {
            try
            {
                string grid = this.chklistDetailGrids.SelectedItem as string;
                string sourceName = this.detailGridsinfo.GridToSourceName(grid);

                DetailGridSortInfoItem sortItem = this.detailGridSortInfo[sourceName];
                if (sortItem == null)
                {
                    sortItem = new DetailGridSortInfoItem();
                    this.detailGridSortInfo[sourceName] = sortItem;
                }

                sortItem.Column = this.cboxDetailGridSortCol.SelectedItem as string;
                sortItem.Direction = this.cboxDetailGridSortDir.SelectedItem as string;
            }
            catch { }
        }

        private void btnDetailGridUP_Click(object sender, System.EventArgs e)
        {
            btnUp(this.chklistDetailGrids);
        }

        private void btnDetailGridDN_Click(object sender, System.EventArgs e)
        {
            btnDn(this.chklistDetailGrids);
        }

        private void PopulateGridDetailSortColumns(IColumns columns)
        {
            this.cboxDetailGridSortCol.Items.Clear();
            this.cboxDetailGridSortCol.Items.Insert(0, string.Empty);

            foreach (IColumn col in columns)
            {
                this.cboxDetailGridSortCol.Items.Add(col.Name);
            }
        }

        private void btnAddDetailGridColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                string text = this.chklistDetailGrids.SelectedItem as string;
                if (text != null)
                {
                    string col = this.lboxDetailGridColumns.SelectedItem as string;
                    if (col != null)
                    {
                        int index = col.IndexOf(',');
                        col = col.Substring(7, index - 7);

                        this.detailGridsinfo.AddColumn(text, col);
                        DisplayGridDetailLookupColumnsChosen(this.detailGridsinfo.GridToColumns(text));
                    }
                }
            }
            catch { }
        }

        private void btnRemoveDetailGridColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                string text = this.chklistDetailGrids.SelectedItem as string;
                if (text != null)
                {
                    string col = this.lboxDetailGridColumns.SelectedItem as string;
                    if (col != null)
                    {
                        int index = col.IndexOf(',');
                        col = col.Substring(7, index - 7);

                        this.detailGridsinfo.RemoveColumn(text, col);
                        DisplayGridDetailLookupColumnsChosen(this.detailGridsinfo.GridToColumns(text));
                    }
                }
            }
            catch { }
        }

        private void btnDetailGridColumnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                string text = this.chklistDetailGrids.SelectedItem as string;
                if (text != null)
                {
                    this.detailGridsinfo.RemoveGrid(text);
                }

                this.txtDetailGridColumns.Clear();
            }
            catch { }
        }

        private void DisplayGridDetailLookupColumnsChosen()
        {
            string text = this.chklistDetailGrids.SelectedItem as string;
            ArrayList columns = this.detailGridsinfo.GridToColumns(text);

            DisplayGridDetailLookupColumnsChosen(columns);
        }

        private void DisplayGridDetailLookupColumnsChosen(ArrayList columns)
        {
            this.txtDetailGridColumns.Clear();

            if (columns != null)
            {
                string data = "";
                for (int i = 0; i < columns.Count; i++)
                {
                    data += columns[i] as string;
                    data += "\r\n";
                }
                this.txtDetailGridColumns.Text = data;
            }
        }

        #endregion

        #region Search 'Tab'

        private void BindSearchColumns()
        {
            PopulateColumns(this.lboxSearchColumns);
        }

        private void btnSearchDN_Click(object sender, System.EventArgs e)
        {
            btnDn(this.lboxSearchColumns);
        }

        private void btnSearchUP_Click(object sender, System.EventArgs e)
        {
            btnUp(this.lboxSearchColumns);
        }

        #endregion


        private void btnDn(ListBox lbox)
        {
            try
            {
                if (lbox.SelectedItems.Count != 1) return;

                int index = lbox.SelectedIndex;

                bool newBottomState = false;
                bool newTopState = false;

                if (index != -1 && index < lbox.Items.Count - 1)
                {
                    object newBottom = lbox.Items[index];
                    object newTop = lbox.Items[index + 1];

                    CheckedListBox chklstBox = lbox as CheckedListBox;
                    if (chklstBox != null)
                    {
                        newBottomState = chklstBox.GetItemChecked(index);
                        newTopState = chklstBox.GetItemChecked(index + 1);
                    }

                    lbox.Items.RemoveAt(index + 1);
                    lbox.Items.RemoveAt(index);

                    lbox.Items.Insert(index, newTop);
                    lbox.Items.Insert(index + 1, newBottom);

                    if (chklstBox != null)
                    {
                        chklstBox.SetItemChecked(index, newTopState);
                        chklstBox.SetItemChecked(index + 1, newBottomState);
                    }

                    lbox.SelectedIndex = index + 1;
                }
            }
            catch { }
        }

        private void btnUp(ListBox lbox)
        {
            if (lbox.SelectedItems.Count != 1) return;

            int index = lbox.SelectedIndex;

            bool newBottomState = false;
            bool newTopState = false;

            if (index != -1 && index > 0)
            {
                object newBottom = lbox.Items[index - 1];
                object newTop = lbox.Items[index];

                CheckedListBox chklstBox = lbox as CheckedListBox;
                if (chklstBox != null)
                {
                    newBottomState = chklstBox.GetItemChecked(index - 1);
                    newTopState = chklstBox.GetItemChecked(index);
                }

                lbox.Items.RemoveAt(index);
                lbox.Items.RemoveAt(index - 1);

                lbox.Items.Insert(index - 1, newTop);
                lbox.Items.Insert(index, newBottom);

                if (chklstBox != null)
                {
                    chklstBox.SetItemChecked(index - 1, newTopState);
                    chklstBox.SetItemChecked(index, newBottomState);
                }

                lbox.SelectedIndex = index - 1;
            }
        }

        private ITable GetLookupTable(string columnName)
        {
            IColumn theColumn = table.Columns[columnName];
            foreach (IForeignKey fk in theColumn.ForeignKeys)
            {
                TableRelation tr = new TableRelation(table, fk);
                if (tr.IsLookup)
                {
                    return tr.ForeignTable;
                }
            }

            return null;
        }

        private IForeignKey GetLookupForeignKey(string columnName)
        {
            IColumn theColumn = table.Columns[columnName];
            foreach (IForeignKey fk in theColumn.ForeignKeys)
            {
                TableRelation tr = new TableRelation(table, fk);
                if (tr.IsLookup)
                {
                    return fk;
                }
            }

            return null;
        }

        //private void btnRebind_Click(object sender, System.EventArgs e)
        //{
        //    BindFromProjectData();
        //}
    }

    #region Helper Classes

    public class DetailGridSortInfoCollection
    {
        public DetailGridSortInfoItem this[string key]
        {
            get
            {
                return sortInfo[key] as DetailGridSortInfoItem;
            }

            set
            {
                sortInfo[key] = value;
            }
        }

        public override string ToString()
        {
            string sep = "";
            string data = "";
            DetailGridSortInfoItem item;

            foreach (string key in sortInfo.Keys)
            {
                item = sortInfo[key] as DetailGridSortInfoItem;

                if (item.Column != "")  // Filter out the user change of mind
                {
                    data += sep + " |" + key + "|" + item.Column + "|" + item.Direction;
                    sep = "^";
                }
            }

            return data;
        }

        public void FromString(string data)
        {
            if (data != null && data.Length > 0)
            {
                sortInfo = new Hashtable();
                string[] records = data.Split(new char[] { '^' });

                foreach (string rec in records)
                {
                    string[] fields = rec.Split(new char[] { '|' });

                    DetailGridSortInfoItem item = new DetailGridSortInfoItem();
                    item.Column = fields[2];
                    item.Direction = fields[3];

                    this.sortInfo[fields[1]] = item;
                }
            }
        }

        private Hashtable sortInfo = new Hashtable();
    }

    public class DetailGridSortInfoItem
    {
        public string Column;
        public string Direction;
    }

    public class LookupColumns
    {
        public ICollection Keys
        {
            get
            {
                return this.lookupColumns.Keys;
            }
        }

        public bool ContainsLookup(string key)
        {
            bool contains = false;

            if (lookupColumns.Contains(key))
            {
                contains = true;
            }

            return contains;
        }

        public ArrayList this[string key]
        {
            get
            {
                return lookupColumns[key] as ArrayList;
            }

            set
            {
                if (value == null)
                {
                    if (lookupColumns.Contains(key))
                    {
                        lookupColumns.Remove(key);
                    }
                }
            }
        }

        public bool Add(string key, string column)
        {
            bool wasAdded = false;

            ArrayList list = this.lookupColumns[key] as ArrayList;
            if (list == null)
            {
                list = new ArrayList();
                this.lookupColumns[key] = list;
            }

            if (!list.Contains(column))
            {
                list.Add(column);
                wasAdded = true;
            }

            return wasAdded;
        }

        public bool Remove(string key, string column)
        {
            bool wasRemoved = false;

            ArrayList list = this.lookupColumns[key] as ArrayList;
            if (list == null) return false;

            if (list.Contains(column))
            {
                list.Remove(column);
                wasRemoved = true;
            }

            return wasRemoved;
        }


        public override string ToString()
        {
            string sep = "";
            string data = "";
            ArrayList list;

            foreach (string key in this.lookupColumns.Keys)
            {
                list = this.lookupColumns[key] as ArrayList;

                if (list.Count > 0)
                {
                    data += sep + "|" + key;
                    foreach (string colName in list)
                    {
                        data += "|" + colName;
                    }

                    sep = "^";
                }
            }

            return data;
        }

        public void FromString(string data)
        {
            if (data != null && data.Length > 0)
            {
                lookupColumns = new Hashtable();
                string[] records = data.Split(new char[] { '^' });

                foreach (string rec in records)
                {
                    string[] fields = rec.Split(new char[] { '|' });

                    ArrayList list = new ArrayList();

                    for (int i = 2; i < fields.Length; i++)
                    {
                        list.Add(fields[i]);
                    }

                    this.lookupColumns[fields[1]] = list;
                }
            }
        }

        private Hashtable lookupColumns = new Hashtable();
    }


    public class DetailGridInfo
    {
        public ArrayList Grids
        {
            get
            {
                return this.gridList;
            }
        }

        public void Clear()
        {
            gridList.Clear();
            gridHash.Clear();
            columns.Clear();
        }

        public string GridToSourceName(string grid)
        {
            return this.gridHash[grid] as string;
        }

        public bool IsGridSourceNameView(string grid)
        {
            string sourceName = this.gridHash[grid] as string;

            if (sourceName.StartsWith("[v]"))
                return true;
            else
                return false;
        }

        public ArrayList GridToColumns(string grid)
        {
            return this.columns[grid] as ArrayList;
        }

        public string GridToViewName(string grid)
        {
            string sourceName = this.gridHash[grid] as string;

            if (sourceName.StartsWith("[v]"))
            {
                int index = sourceName.IndexOf(',', 3);
                if (index != -1)
                {
                    sourceName = sourceName.Substring(3, index - 3);
                }
            }

            return sourceName;
        }

        public string GridToForeignKeyName(string grid)
        {
            string sourceName = this.gridHash[grid] as string;

            if (sourceName.StartsWith("[v]"))
            {
                int index = sourceName.IndexOf(',', 3);
                if (index != -1)
                {
                    sourceName = sourceName.Substring(index + 1);
                }
            }

            return sourceName;
        }

        public string GridToViewPrimaryKey(string grid)
        {
            return this.viewPK[grid] as string;
        }

        public bool AddGrid(string grid, string sourceName)
        {
            // sourceName prefix "[v]" = view, otherwise it's a foriegnkey
            bool wasAdded = false;

            if (!this.gridList.Contains(grid))
            {
                this.gridList.Add(grid);
                this.gridHash[grid] = sourceName;
                wasAdded = true;
            }

            return wasAdded;
        }

        public bool RemoveGrid(string grid)
        {
            bool wasRemoved = false;

            if (this.gridList.Contains(grid))
            {
                this.gridList.Remove(grid);
                this.gridHash.Remove(grid);

                if (this.columns.ContainsKey(grid))
                {
                    this.columns.Remove(grid);
                }
                wasRemoved = true;
            }

            return wasRemoved;
        }

        public bool SwapGrid(string grid, string sourceName)
        {
            // sourceName prefix "[v]" = view, otherwise it's a foriegnkey
            bool wasSwapped = false;

            if (this.gridList.Contains(grid))
            {
                ArrayList list = this.columns[grid] as ArrayList;
                if (list != null)
                {
                    list.Clear();
                }

                this.viewPK[grid] = "";

                if (sourceName.StartsWith("[v]"))
                {
                    string currentSourceName = this.gridHash[grid] as string;

                    if (currentSourceName.StartsWith("[v]"))
                    {
                        string[] fkInfo = currentSourceName.Split(new char[] { ',' });
                        sourceName += "," + fkInfo[1];
                    }
                    else
                    {
                        // We tack on the fk.Name
                        sourceName += "," + this.gridHash[grid];
                    }
                }
                this.gridHash[grid] = sourceName;
                wasSwapped = true;
            }

            return wasSwapped;
        }

        public void SetViewPrimaryKey(string grid, string primarykey)
        {
            this.viewPK[grid] = primarykey;
        }

        public bool AddColumn(string grid, string column)
        {
            bool wasAdded = false;

            ArrayList list = this.columns[grid] as ArrayList;
            if (list == null)
            {
                list = new ArrayList();
                this.columns[grid] = list;
            }

            if (!list.Contains(column))
            {
                list.Add(column);
                wasAdded = true;
            }

            return wasAdded;
        }

        public bool RemoveColumn(string grid, string column)
        {
            bool wasRemoved = false;

            ArrayList list = this.columns[grid] as ArrayList;
            if (list == null) return false;

            if (list.Contains(column))
            {
                list.Remove(column);
                wasRemoved = true;
            }

            return wasRemoved;
        }

        public void TheseGridsOnly(ArrayList list)
        {
            this.gridList = list;
        }

        public override string ToString()
        {
            string sep = "";
            string data = "";
            string sourceName = "";
            string viewPK = "";
            ArrayList list;

            foreach (string grid in this.gridList)
            {
                sourceName = this.gridHash[grid] as string;
                list = this.columns[grid] as ArrayList;
                viewPK = this.viewPK[grid] as string;

                if (list != null && list.Count > 0)
                {
                    data += sep + "|" + grid + "|" + sourceName;
                    if (viewPK != null && viewPK.Length > 0)
                    {
                        data += "+" + viewPK;
                    }

                    foreach (string colName in list)
                    {
                        data += "|" + colName;
                    }

                    sep = "^";
                }
            }

            return data;
        }

        public void FromString(string data)
        {
            if (data != null && data.Length > 0)
            {
                gridList = new ArrayList();
                gridHash = new Hashtable();
                columns = new Hashtable();

                string grid = "";

                string[] records = data.Split(new char[] { '^' });

                foreach (string rec in records)
                {
                    string[] fields = rec.Split(new char[] { '|' });

                    grid = fields[1];

                    if (fields[2].StartsWith("[v]"))
                    {
                        string[] viewInfo = fields[2].Split(new char[] { '+' });

                        this.AddGrid(grid, viewInfo[0]);
                        this.SetViewPrimaryKey(grid, viewInfo[1]);
                    }
                    else
                    {
                        this.AddGrid(grid, fields[2]);
                    }

                    for (int i = 3; i < fields.Length; i++)
                    {
                        this.AddColumn(grid, fields[i]);
                    }
                }
            }
        }

        private ArrayList gridList = new ArrayList();
        private Hashtable gridHash = new Hashtable();
        private Hashtable columns = new Hashtable();
        private Hashtable viewPK = new Hashtable();
    }
    #endregion
}
