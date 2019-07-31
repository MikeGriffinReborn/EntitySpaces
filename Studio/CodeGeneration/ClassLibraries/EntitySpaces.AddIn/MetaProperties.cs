using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using WeifenLuo.WinFormsUI.Docking;

using MyMeta;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for MetaProperties.
	/// </summary>
    public class MetaProperties : DockContent, IMyGenContent
    {
        private IMyGenerationMDI mdi;
		private System.Windows.Forms.DataGrid Grid;
		private System.Windows.Forms.DataGridTableStyle MyStyle;

		private DataTable emptyTable;
		private System.Windows.Forms.DataGridTextBoxColumn col_Property;
		private System.Windows.Forms.DataGridTextBoxColumn col_Value;

		private Type stringType = Type.GetType("System.String");
		private System.Windows.Forms.LinkLabel lnkHELP;

		private string helpInterface = "";

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public MetaProperties(IMyGenerationMDI mdi)
		{
			emptyTable = new DataTable("MyData");

			emptyTable.Columns.Add("Property", stringType);
			emptyTable.Columns.Add("Value", stringType);

            InitializeComponent();
            this.mdi = mdi;
			this.ShowHint = DockState.DockRight;
        }

        protected override string GetPersistString()
        {
            return this.GetType().FullName;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetaProperties));
            this.Grid = new System.Windows.Forms.DataGrid();
            this.MyStyle = new System.Windows.Forms.DataGridTableStyle();
            this.col_Property = new System.Windows.Forms.DataGridTextBoxColumn();
            this.col_Value = new System.Windows.Forms.DataGridTextBoxColumn();
            this.lnkHELP = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Grid
            // 
            this.Grid.AlternatingBackColor = System.Drawing.Color.Wheat;
            this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid.BackColor = System.Drawing.SystemColors.Control;
            this.Grid.BackgroundColor = System.Drawing.Color.Wheat;
            this.Grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Grid.CaptionBackColor = System.Drawing.SystemColors.Control;
            this.Grid.CaptionVisible = false;
            this.Grid.DataMember = "";
            this.Grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Grid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.Grid.Location = new System.Drawing.Point(0, 0);
            this.Grid.Name = "Grid";
            this.Grid.PreferredColumnWidth = 200;
            this.Grid.ReadOnly = true;
            this.Grid.RowHeadersVisible = false;
            this.Grid.RowHeaderWidth = 0;
            this.Grid.Size = new System.Drawing.Size(431, 518);
            this.Grid.TabIndex = 9;
            this.Grid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.MyStyle});
            // 
            // MyStyle
            // 
            this.MyStyle.AlternatingBackColor = System.Drawing.Color.PaleGoldenrod;
            this.MyStyle.BackColor = System.Drawing.Color.Wheat;
            this.MyStyle.DataGrid = this.Grid;
            this.MyStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.col_Property,
            this.col_Value});
            this.MyStyle.GridLineColor = System.Drawing.Color.Goldenrod;
            this.MyStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.MyStyle.MappingName = "MyData";
            this.MyStyle.ReadOnly = true;
            this.MyStyle.RowHeadersVisible = false;
            this.MyStyle.RowHeaderWidth = 0;
            // 
            // col_Property
            // 
            this.col_Property.Format = "";
            this.col_Property.FormatInfo = null;
            this.col_Property.HeaderText = "Property";
            this.col_Property.MappingName = "Property";
            this.col_Property.NullText = "";
            this.col_Property.ReadOnly = true;
            this.col_Property.Width = 75;
            // 
            // col_Value
            // 
            this.col_Value.Format = "";
            this.col_Value.FormatInfo = null;
            this.col_Value.HeaderText = "Value";
            this.col_Value.MappingName = "Value";
            this.col_Value.NullText = "";
            this.col_Value.ReadOnly = true;
            this.col_Value.Width = 75;
            // 
            // lnkHELP
            // 
            this.lnkHELP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkHELP.Location = new System.Drawing.Point(8, 522);
            this.lnkHELP.Name = "lnkHELP";
            this.lnkHELP.Size = new System.Drawing.Size(416, 21);
            this.lnkHELP.TabIndex = 10;
            this.lnkHELP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHELP_LinkClicked);
            // 
            // MetaProperties
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Wheat;
            this.ClientSize = new System.Drawing.Size(431, 550);
            this.Controls.Add(this.lnkHELP);
            this.Controls.Add(this.Grid);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MetaProperties";
            this.TabText = "MyMeta Properties";
            this.Text = "MyMeta Properties";
            this.ToolTipText = "MyMeta Properties";
            this.Load += new System.EventHandler(this.MetaProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void MetaProperties_Load(object sender, System.EventArgs e)
		{
			this.col_Property.TextBox.BorderStyle = BorderStyle.None;
			this.col_Value.TextBox.BorderStyle    = BorderStyle.None;
 
			this.col_Property.TextBox.Move += new System.EventHandler(this.ColorTextBox);
			this.col_Value.TextBox.Move    += new System.EventHandler(this.ColorTextBox);

			this.Clear();
		}

		/*public void DefaultSettingsChanged(DefaultSettings settings)
		{
			this.Clear();
		}*/

		public void MetaBrowserRefresh()
		{
			this.Clear();
		}

		public void Clear()
		{
			this.Grid.DataSource = emptyTable;
			this.InitializeGrid();
			this.Text = "MyMeta Properties";
			this.lnkHELP.Text = "";
			this.helpInterface = "";
		}

		//===============================================================================
		// Properties
		//===============================================================================
		public void DisplayDatabaseProperties(IDatabase database, TreeNode tableNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Name", database.Name});
			dt.Rows.Add(new object[] {"Alias", database.Alias});
			dt.Rows.Add(new object[] {"Description", database.Description});
			dt.Rows.Add(new object[] {"SchemaName", database.SchemaName});
			dt.Rows.Add(new object[] {"SchemaOwner", database.SchemaOwner});
			dt.Rows.Add(new object[] {"DefaultCharSetCatalog", database.DefaultCharSetCatalog});
			dt.Rows.Add(new object[] {"DefaultCharSetSchema", database.DefaultCharSetSchema});
			dt.Rows.Add(new object[] {"DefaultCharSetName", database.DefaultCharSetName});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IDatabase Properties";
			this.lnkHELP.Text = "IDatabase Help ...";
			this.helpInterface = "IDatabase";
		}

		public void DisplayColumnProperties(IColumn column, TreeNode columnNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Name", column.Name});
			dt.Rows.Add(new object[] {"Alias", column.Alias});
			dt.Rows.Add(new object[] {"Description", column.Description});
			dt.Rows.Add(new object[] {"Ordinal", column.Ordinal.ToString()});
			dt.Rows.Add(new object[] {"DataTypeName", column.DataTypeName});
			dt.Rows.Add(new object[] {"DataTypeNameComplete", column.DataTypeNameComplete});
			dt.Rows.Add(new object[] {"NumericPrecision", column.NumericPrecision.ToString()});
			dt.Rows.Add(new object[] {"NumericScale", column.NumericScale.ToString()});
			dt.Rows.Add(new object[] {"DateTimePrecision", column.DateTimePrecision.ToString()});
			dt.Rows.Add(new object[] {"CharacterMaxLength", column.CharacterMaxLength.ToString()});
			dt.Rows.Add(new object[] {"CharacterOctetLength", column.CharacterOctetLength.ToString()});
			dt.Rows.Add(new object[] {"LanguageType", column.LanguageType});
			dt.Rows.Add(new object[] {"DbTargetType", column.DbTargetType});
			dt.Rows.Add(new object[] {"IsNullable", column.IsNullable ? "True" : "False"});
			dt.Rows.Add(new object[] {"IsComputed", column.IsComputed ? "True" : "False"});
			dt.Rows.Add(new object[] {"IsInPrimaryKey", column.IsInPrimaryKey ? "True" : "False"});
			dt.Rows.Add(new object[] {"IsInForeignKey", column.IsInForeignKey ? "True" : "False"});
			dt.Rows.Add(new object[] {"IsAutoKey", column.IsAutoKey ? "True" : "False"});
			dt.Rows.Add(new object[] {"AutoKeySeed", column.AutoKeySeed});
			dt.Rows.Add(new object[] {"AutoKeyIncrement", column.AutoKeyIncrement});
			dt.Rows.Add(new object[] {"HasDefault", column.HasDefault ? "True" : "False"});
			dt.Rows.Add(new object[] {"Default", column.Default});
			dt.Rows.Add(new object[] {"Flags", column.Flags.ToString()});
			dt.Rows.Add(new object[] {"PropID", column.PropID.ToString()});
			dt.Rows.Add(new object[] {"Guid", column.Guid.ToString()});
			dt.Rows.Add(new object[] {"TypeGuid", column.TypeGuid.ToString()});
			dt.Rows.Add(new object[] {"LCID", column.LCID.ToString()});
			dt.Rows.Add(new object[] {"SortID", column.SortID.ToString()});
			dt.Rows.Add(new object[] {"CompFlags", column.CompFlags.ToString()});
			dt.Rows.Add(new object[] {"DomainName", column.DomainName});
			dt.Rows.Add(new object[] {"HasDomain", column.HasDomain ? "True" : "False"});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IColumn Properties";
			this.lnkHELP.Text = "IColumn Help ...";
			this.helpInterface = "IColumn";
		}

		public void DisplayTableProperties(ITable table, TreeNode tableNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Name", table.Name});
			dt.Rows.Add(new object[] {"Alias", table.Alias});
			dt.Rows.Add(new object[] {"Schema", table.Schema});			
			dt.Rows.Add(new object[] {"Description", table.Description});
			dt.Rows.Add(new object[] {"Type", table.Type});
			dt.Rows.Add(new object[] {"Guid", table.Guid.ToString()});
			dt.Rows.Add(new object[] {"PropID", table.PropID.ToString()});
			dt.Rows.Add(new object[] {"DateCreated", table.DateCreated.ToShortDateString()});
			dt.Rows.Add(new object[] {"DateModified", table.DateModified.ToShortDateString()});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "ITable Properties";
			this.lnkHELP.Text = "ITable Help ...";
			this.helpInterface = "ITable";
		}

		public void DisplayViewProperties(IView view, TreeNode tableNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Name", view.Name});
			dt.Rows.Add(new object[] {"Alias", view.Alias});
			dt.Rows.Add(new object[] {"Schema", view.Schema});	
			dt.Rows.Add(new object[] {"Description", view.Description});
			dt.Rows.Add(new object[] {"Type", view.Type});
			dt.Rows.Add(new object[] {"Guid", view.Guid.ToString()});
			dt.Rows.Add(new object[] {"PropID", view.PropID.ToString()});
			dt.Rows.Add(new object[] {"DateCreated", view.DateCreated.ToShortDateString()});
			dt.Rows.Add(new object[] {"DateModified", view.DateModified.ToShortDateString()});
			dt.Rows.Add(new object[] {"CheckOption", view.CheckOption ? "True" : "False"});
			dt.Rows.Add(new object[] {"IsUpdateable", view.IsUpdateable? "True" : "False"});
			dt.Rows.Add(new object[] {"ViewText", view.ViewText});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IView Properties";
			this.lnkHELP.Text = "IView Help ...";
			this.helpInterface = "IView";
		}

		public void DisplayProcedureProperties(IProcedure proc, TreeNode tableNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Name", proc.Name});
			dt.Rows.Add(new object[] {"Alias", proc.Alias});
			dt.Rows.Add(new object[] {"Schema", proc.Schema});
			dt.Rows.Add(new object[] {"Description", proc.Description});
			dt.Rows.Add(new object[] {"Type", proc.Type.ToString()});
			dt.Rows.Add(new object[] {"DateCreated", proc.DateCreated.ToShortDateString()});
			dt.Rows.Add(new object[] {"DateModified", proc.DateModified.ToShortDateString()});
			dt.Rows.Add(new object[] {"ProcedureText", proc.ProcedureText});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IProcedure Properties";
			this.lnkHELP.Text = "IProcedure Help ...";
			this.helpInterface = "IProcedure";
		}

		public void DisplayDomainProperties(IDomain domain, TreeNode tableNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);
/*
			if(metaData.Columns.Contains("DOMAIN_CATALOG"))						f_DomainCatalog		= metaData.Columns["DOMAIN_CATALOG"];
			if(metaData.Columns.Contains("DOMAIN_SCHEMA"))						f_DomainSchema		= metaData.Columns["DOMAIN_SCHEMA"];
			if(metaData.Columns.Contains("DOMAIN_NAME"))						f_DomainName		= metaData.Columns["DOMAIN_NAME"];
			if(metaData.Columns.Contains("DATA_TYPE"))							f_DataType			= metaData.Columns["DATA_TYPE"];
			if(metaData.Columns.Contains("CHARACTER_MAXIMUM_LENGTH"))			f_MaxLength			= metaData.Columns["CHARACTER_MAXIMUM_LENGTH"];
			if(metaData.Columns.Contains("CHARACTER_OCTET_LENGTH"))				f_OctetLength		= metaData.Columns["CHARACTER_OCTET_LENGTH"];
			if(metaData.Columns.Contains("COLLATION_CATALOG"))					f_CollationCatalog	= metaData.Columns["COLLATION_CATALOG"];
			if(metaData.Columns.Contains("COLLATION_SCHEMA"))					f_CollationSchema	= metaData.Columns["COLLATION_SCHEMA"];
			if(metaData.Columns.Contains("COLLATION_NAME"))						f_CollationName		= metaData.Columns["COLLATION_NAME"];
			if(metaData.Columns.Contains("CHARACTER_SET_CATALOG"))				f_CharSetCatalog    = metaData.Columns["CHARACTER_SET_CATALOG"];
			if(metaData.Columns.Contains("CHARACTER_SET_SCHEMA"))				f_CharSetSchema     = metaData.Columns["CHARACTER_SET_SCHEMA"];
			if(metaData.Columns.Contains("CHARACTER_SET_NAME"))					f_CharSetName       = metaData.Columns["CHARACTER_SET_NAME"];
			if(metaData.Columns.Contains("NUMERIC_PRECISION"))					f_NumericPrecision	= metaData.Columns["NUMERIC_PRECISION"];
			if(metaData.Columns.Contains("NUMERIC_SCALE"))						f_NumericScale		= metaData.Columns["NUMERIC_SCALE"];
			if(metaData.Columns.Contains("DATETIME_PRECISION"))					f_DatetimePrecision = metaData.Columns["DATETIME_PRECISION"];
			if(metaData.Columns.Contains("DOMAIN_DEFAULT"))						f_Default			= metaData.Columns["COLUMN_DEFAULT"];
			if(metaData.Columns.Contains("IS_NULLABLE"))	
			*/

			dt.Rows.Add(new object[] {"Name", domain.Name});
			dt.Rows.Add(new object[] {"Alias", domain.Alias});

			dt.Rows.Add(new object[] {"DataTypeName", domain.DataTypeName});
			dt.Rows.Add(new object[] {"DataTypeNameComplete", domain.DataTypeNameComplete});
			dt.Rows.Add(new object[] {"NumericPrecision", domain.NumericPrecision.ToString()});
			dt.Rows.Add(new object[] {"NumericScale", domain.NumericScale.ToString()});
			dt.Rows.Add(new object[] {"DateTimePrecision", domain.DateTimePrecision.ToString()});
			dt.Rows.Add(new object[] {"CharacterMaxLength", domain.CharacterMaxLength.ToString()});
			dt.Rows.Add(new object[] {"CharacterOctetLength", domain.CharacterOctetLength.ToString()});
			dt.Rows.Add(new object[] {"LanguageType", domain.LanguageType});
			dt.Rows.Add(new object[] {"DbTargetType", domain.DbTargetType});
			dt.Rows.Add(new object[] {"IsNullable", domain.IsNullable ? "True" : "False"});
			dt.Rows.Add(new object[] {"HasDefault", domain.HasDefault ? "True" : "False"});
			dt.Rows.Add(new object[] {"Default", domain.Default});


			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IDomain Properties";
			this.lnkHELP.Text = "IDomain Help ...";
			this.helpInterface = "IDomain";
		}

		public void DisplayParameterProperties(IParameter parameter, TreeNode parameterNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);
			
			string dir = "";

			switch(parameter.Direction)
			{
				case ParamDirection.Input:
					dir = "IN";
					break;

				case ParamDirection.InputOutput:
					dir = "INOUT";
					break;

				case ParamDirection.Output:
					dir = "OUT";
					break;

				case ParamDirection.ReturnValue:
					dir = "RETURN";
					break;
			}

			dt.Rows.Add(new object[] {"Name", parameter.Name});
			dt.Rows.Add(new object[] {"Alias", parameter.Alias});
			dt.Rows.Add(new object[] {"Description", parameter.Description});
			dt.Rows.Add(new object[] {"Direction", dir});
			dt.Rows.Add(new object[] {"Ordinal", parameter.Ordinal.ToString()});
			dt.Rows.Add(new object[] {"TypeName", parameter.TypeName});
			dt.Rows.Add(new object[] {"DataTypeNameComplete", parameter.DataTypeNameComplete});
			dt.Rows.Add(new object[] {"NumericPrecision", parameter.NumericPrecision.ToString()});
			dt.Rows.Add(new object[] {"NumericScale", parameter.NumericScale.ToString()});
			dt.Rows.Add(new object[] {"CharacterMaxLength", parameter.CharacterMaxLength.ToString()});
			dt.Rows.Add(new object[] {"CharacterOctetLength", parameter.CharacterOctetLength.ToString()});
			dt.Rows.Add(new object[] {"LanguageType", parameter.LanguageType});
			dt.Rows.Add(new object[] {"DbTargetType", parameter.DbTargetType});
			dt.Rows.Add(new object[] {"HasDefault", parameter.HasDefault ? "True" : "False"});
			dt.Rows.Add(new object[] {"Default", parameter.Default});
			dt.Rows.Add(new object[] {"IsNullable", parameter.IsNullable ? "True" : "False"});
			dt.Rows.Add(new object[] {"LocalTypeName", parameter.LocalTypeName});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IParameter Properties";
			this.lnkHELP.Text = "IParameter Help ...";
			this.helpInterface = "IParameter";
		}

		public void DisplayResultColumnProperties(IResultColumn resultColumn, TreeNode resultColumnNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Name", resultColumn.Name});
			dt.Rows.Add(new object[] {"Alias", resultColumn.Alias});
			dt.Rows.Add(new object[] {"Ordinal", resultColumn.Ordinal.ToString()});
			dt.Rows.Add(new object[] {"DataTypeName", resultColumn.DataTypeName});
			dt.Rows.Add(new object[] {"DataTypeNameComplete", resultColumn.DataTypeNameComplete});
			dt.Rows.Add(new object[] {"LanguageType", resultColumn.LanguageType});
			dt.Rows.Add(new object[] {"DbTargetType", resultColumn.DbTargetType});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IResultColumn Properties";
			this.lnkHELP.Text = "IResultColumn Help ...";
			this.helpInterface = "IResultColumn";
		}

		public void DisplayForeignKeyProperties(IForeignKey foreignKey, TreeNode foreignKeyNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Name", foreignKey.Name});
			dt.Rows.Add(new object[] {"Alias", foreignKey.Alias});
			// NOTE, THIS MUST USE INDIRECTION THROUGH THE TABLE
			dt.Rows.Add(new object[] {"PrimaryTable", foreignKey.PrimaryTable.Name});
			dt.Rows.Add(new object[] {"ForeignTable", foreignKey.ForeignTable.Name});
			dt.Rows.Add(new object[] {"UpdateRule", foreignKey.UpdateRule});
			dt.Rows.Add(new object[] {"DeleteRule", foreignKey.DeleteRule});
			dt.Rows.Add(new object[] {"PrimaryKeyName", foreignKey.PrimaryKeyName});
			dt.Rows.Add(new object[] {"Deferrability", foreignKey.Deferrability.ToString()});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IForeignKey Properties";
			this.lnkHELP.Text = "IForeignKey Help ...";
			this.helpInterface = "IForeignKey";
		}

		public void DisplayIndexProperties(IIndex index, TreeNode indexNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Name", index.Name});
			dt.Rows.Add(new object[] {"Alias", index.Alias});
			dt.Rows.Add(new object[] {"Schema", index.Schema});
			dt.Rows.Add(new object[] {"Unique", index.Unique ? "True" : "False"});
			dt.Rows.Add(new object[] {"Clustered", index.Clustered ? "True" : "False"});
			dt.Rows.Add(new object[] {"Type", index.Type.ToString()});
			dt.Rows.Add(new object[] {"FillFactor", index.FillFactor.ToString()});
			dt.Rows.Add(new object[] {"InitialSize", index.InitialSize.ToString()});
			dt.Rows.Add(new object[] {"SortBookmarks", index.SortBookmarks ? "True" : "False"});
			dt.Rows.Add(new object[] {"AutoUpdate", index.AutoUpdate ? "True" : "False"});
			dt.Rows.Add(new object[] {"NullCollation", index.NullCollation.ToString()});
			dt.Rows.Add(new object[] {"Collation", index.Collation.ToString()});
			dt.Rows.Add(new object[] {"Cardinality", index.Cardinality.ToString()});
			dt.Rows.Add(new object[] {"Pages", index.Pages.ToString()});
			dt.Rows.Add(new object[] {"FilterCondition", index.FilterCondition});
			dt.Rows.Add(new object[] {"Integrated", index.Integrated ? "True" : "False"});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IIndex Properties";
			this.lnkHELP.Text = "IIndex Help ...";
			this.helpInterface = "IIndex";
		}

		public void DisplayProviderTypeProperties(IProviderType type, TreeNode indexNode)
		{
			DataTable dt = new DataTable("MyData");

			dt.Columns.Add("Property", stringType);
			dt.Columns.Add("Value", stringType);

			dt.Rows.Add(new object[] {"Type", type.Type});
			dt.Rows.Add(new object[] {"DataType", type.DataType.ToString()});
			dt.Rows.Add(new object[] {"ColumnSize", type.ColumnSize.ToString()});
			dt.Rows.Add(new object[] {"LiteralPrefix", type.LiteralPrefix});
			dt.Rows.Add(new object[] {"LiteralSuffix", type.LiteralSuffix});
			dt.Rows.Add(new object[] {"CreateParams", type.CreateParams});
			dt.Rows.Add(new object[] {"IsNullable", type.IsNullable ? "True" : "False"});
			dt.Rows.Add(new object[] {"IsCaseSensitive", type.IsCaseSensitive ? "True" : "False"});
			//			dt.Rows.Add(new object[] {"Searchable", type.Searchable});
			dt.Rows.Add(new object[] {"IsUnsigned", type.IsUnsigned ? "True" : "False"});
			dt.Rows.Add(new object[] {"HasFixedPrecScale", type.HasFixedPrecScale ? "True" : "False"});
			dt.Rows.Add(new object[] {"CanBeAutoIncrement", type.CanBeAutoIncrement ? "True" : "False"});
			dt.Rows.Add(new object[] {"LocalType", type.LocalType});
			dt.Rows.Add(new object[] {"MinimumScale", type.MinimumScale.ToString()});
			dt.Rows.Add(new object[] {"MaximumScale", type.MaximumScale.ToString()});
			dt.Rows.Add(new object[] {"TypeGuid", type.TypeGuid.ToString()});
			dt.Rows.Add(new object[] {"TypeLib", type.TypeLib});
			dt.Rows.Add(new object[] {"Version", type.Version});
			dt.Rows.Add(new object[] {"IsLong", type.IsLong ? "True" : "False"});
			dt.Rows.Add(new object[] {"BestMatch", type.BestMatch ? "True" : "False"});
			dt.Rows.Add(new object[] {"IsFixedLength", type.IsFixedLength ? "True" : "False"});

			this.Grid.DataSource = dt;

			this.InitializeGrid();

			this.Text = "IProviderType Properties";
			this.lnkHELP.Text = "IProviderType Help ...";
			this.helpInterface = "IProviderType";
		}

		private void ColorTextBox(object sender, System.EventArgs e)
		{
			TextBox txtBox = (TextBox)sender;

			// Bail if we're in la la land
			Size size = txtBox.Size;
			if(size.Width == 0 && size.Height == 0)
			{
				return;
			}

			int row = this.Grid.CurrentCell.RowNumber;

			if(isEven(row))
				txtBox.BackColor = Color.Wheat;
			else
				txtBox.BackColor = Color.PaleGoldenrod;
		}

		private bool isEven(int x)
		{
			return (x & 1) == 0;
		}


		private void InitializeGrid()
		{
			if(!gridInitialized)
			{
				if(MyStyle.GridColumnStyles.Count > 0)
				{
					gridInitialized = true;
					gridHelper = new GridLayoutHelper(this.Grid, this.MyStyle,
						new decimal[] { 0.50M, 0.50M },	new int[] { 130, 130 });
				}
			}
		}



		private GridLayoutHelper gridHelper;
		private bool gridInitialized = false;

		private void lnkHELP_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				if(this.helpInterface != "")
				{
					Process myProcess = new Process();

					myProcess.StartInfo.FileName = "mk:@MSITStore:" + Application.StartupPath +
						@"\MyMeta.chm::/html/T_MyMeta_" + this.helpInterface + ".htm";

					myProcess.StartInfo.CreateNoWindow = true;
					myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
					myProcess.Start();
				}
			}
			catch {}
		}

        #region IMyGenContent Members
        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command == "UpdateDefaultSettings")
            {
                this.Clear();
            }
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public void ResetMenu()
        {
            //
        }

        public DockContent DockContent
        {
            get { return this; }
        }
        #endregion
    }
}
