using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginForeignKeys : ForeignKeys
    {
        private IPlugin plugin;

        public PluginForeignKeys(IPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetForeignKeys(this.Table.Database.Name, this.Table.Name);
            PopulateArray(metaData);
		}
	}
}
