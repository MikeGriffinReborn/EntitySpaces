using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginTables : Tables
    {
        private IPlugin plugin;

        public PluginTables(IPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetTables(this.Database.Name);
            PopulateArray(metaData);
		}
	}
}
