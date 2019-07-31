using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginView : View
    {
        private IPlugin plugin;

        public PluginView(IPlugin plugin)
        {
            this.plugin = plugin;
		}

		public override string ViewText
		{
			get
			{
				PluginViews views = this.Views as PluginViews;
				return this.GetString(views.f_viewText);
			}
		}

		override public IViews SubViews 
		{ 
			get
			{
				if(!_subViewInfoLoaded)
				{
					LoadSubViewInfo();
				}
				return _views;				
			}
		}

		override public ITables SubTables
		{ 
			get
			{
				if(!_subViewInfoLoaded)
				{
					LoadSubViewInfo();
				}
				return _tables;
			}
		}

		private void LoadSubViewInfo()
		{
			_views  = (Views)this.dbRoot.ClassFactory.CreateViews();
			_views.dbRoot = this._dbRoot;
            _views.Database = this.Views.Database;
            System.Collections.Generic.List<string> subViews = this.plugin.GetViewSubViews(this.Database.Name, this.Name);
            foreach (string entity in subViews)
            {
                View view = this.Database.Views[entity] as View;
                if (null != view) _views.AddView(view);
            }

			_tables = (Tables)this.dbRoot.ClassFactory.CreateTables();
			_tables.dbRoot = this._dbRoot;
            _tables.Database = this.Views.Database;
            System.Collections.Generic.List<string> subTables = this.plugin.GetViewSubTables(this.Database.Name, this.Name);
            foreach (string entity in subTables)
            {
                Table table = this.Database.Tables[entity] as Table;
                if (null != table) _tables.AddTable(table);
            }
        }

        public override object DatabaseSpecificMetaData(string key)
        {
            return this.plugin.GetDatabaseSpecificMetaData(this, key);
        }
	}
}
