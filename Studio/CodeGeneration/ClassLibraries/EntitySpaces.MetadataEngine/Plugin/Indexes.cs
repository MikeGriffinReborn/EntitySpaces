using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginIndexes : Indexes
    {
        private IPlugin plugin;

        public PluginIndexes(IPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetTableIndexes(this.Table.Database.Name, this.Table.Name);
            PopulateArray(metaData);
		}
	}
}
