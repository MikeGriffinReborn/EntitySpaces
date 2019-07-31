using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using EntitySpaces.MetadataEngine;

namespace EntitySpaces.AddIn
{
    internal partial class ucMetadataProperties : UserControl
    {
        private int _currentHashCode = Int32.MinValue;
 
        public ucMetadataProperties()
        {
            InitializeComponent();
        }

        private void MetaProperties_Load(object sender, System.EventArgs e)
        {

            this.ClearOrRefresh();
        }

        public void MetaBrowserRefresh()
        {
            this.Clear();
        }

        public void ClearOrRefresh()
        {
            if (_currentHashCode != Int32.MinValue)
            {
                this.Refresh();
            }
            else
            {
                Clear();
            }
        }

        public void Clear()
        {
            _currentHashCode = Int32.MinValue;
            this.propGrid.SelectedObject = null;
            this.Text = "esMeta Properties";
        }

        //===============================================================================
        // Properties
        //===============================================================================
        public void DisplayDatabaseProperties(IDatabase database, TreeNode tableNode)
        {
            if (this._currentHashCode == database.GetHashCode()) return;

            propGrid.SelectedObject = database;
            this._currentHashCode = database.GetHashCode();
        }

        public void DisplayColumnProperties(IColumn column, TreeNode columnNode)
        {
            if (this._currentHashCode == column.GetHashCode()) return;

            propGrid.SelectedObject = column;
            this._currentHashCode = column.GetHashCode();
        }

        public void DisplayTableProperties(ITable table, TreeNode tableNode)
        {
            if (this._currentHashCode == table.GetHashCode()) return;

            propGrid.SelectedObject = table;
            this._currentHashCode = table.GetHashCode();
        }

        public void DisplayViewProperties(IView view, TreeNode tableNode)
        {
            if (this._currentHashCode == view.GetHashCode()) return;

            propGrid.SelectedObject = view;
            this._currentHashCode = view.GetHashCode();
        }

        public void DisplayProcedureProperties(IProcedure proc, TreeNode tableNode)
        {
            if (this._currentHashCode == proc.GetHashCode()) return;

            propGrid.SelectedObject = proc;
            this._currentHashCode = proc.GetHashCode();
        }

        public void DisplayDomainProperties(IDomain domain, TreeNode tableNode)
        {
            if (this._currentHashCode == domain.GetHashCode()) return;

            propGrid.SelectedObject = domain;
            this._currentHashCode = domain.GetHashCode();
        }

        public void DisplayParameterProperties(IParameter parameter, TreeNode parameterNode)
        {
            if (this._currentHashCode == parameter.GetHashCode()) return;

            propGrid.SelectedObject = parameter;

//            switch (parameter.Direction)
//            {
//                case ParamDirection.Input:
//                    dir = "IN";
//                    break;

//                case ParamDirection.InputOutput:
//                    dir = "INOUT";
//                    break;

//                case ParamDirection.Output:
//                    dir = "OUT";
//                    break;

//                case ParamDirection.ReturnValue:
//                    dir = "RETURN";
//                    break;
//            }

            this._currentHashCode = parameter.GetHashCode();
        }

        public void DisplayResultColumnProperties(IResultColumn resultColumn, TreeNode resultColumnNode)
        {
            if (this._currentHashCode == resultColumn.GetHashCode()) return;

            propGrid.SelectedObject = resultColumn;
            this._currentHashCode = resultColumn.GetHashCode();
        }

        public void DisplayForeignKeyProperties(IForeignKey foreignKey, TreeNode foreignKeyNode)
        {
            if (this._currentHashCode == foreignKey.GetHashCode()) return;

            propGrid.SelectedObject = foreignKey;
            this._currentHashCode = foreignKey.GetHashCode();
        }

        public void DisplayIndexProperties(IIndex index, TreeNode indexNode)
        {
            if (this._currentHashCode == index.GetHashCode()) return;

            propGrid.SelectedObject = index;
            this._currentHashCode = index.GetHashCode();
        }

        public void DisplayProviderTypeProperties(IProviderType type, TreeNode indexNode)
        {
            if (this._currentHashCode == type.GetHashCode()) return;

            propGrid.SelectedObject = type;
            this._currentHashCode = type.GetHashCode();
        }
    }
}
