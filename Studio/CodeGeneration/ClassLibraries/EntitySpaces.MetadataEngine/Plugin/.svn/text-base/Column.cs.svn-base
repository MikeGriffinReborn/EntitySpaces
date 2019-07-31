using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginColumn : Column
	{
        private IPlugin plugin;

        public PluginColumn(IPlugin plugin)
        {
            this.plugin = plugin;
        }

		public override string DataTypeName
		{
			get
			{
				PluginColumns cols = Columns as PluginColumns;
				return this.GetString(cols.f_extTypeName);
			}
		}

		public override string DataTypeNameComplete
		{
			get
			{
				PluginColumns cols = Columns as PluginColumns;
				return this.GetString(cols.f_extTypeNameComplete);
			}
        }

        public override object DatabaseSpecificMetaData(string key)
        {
            return this.plugin.GetDatabaseSpecificMetaData(this, key);
        }
	}
}
