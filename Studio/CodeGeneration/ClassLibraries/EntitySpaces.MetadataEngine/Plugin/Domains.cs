using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginDomains : Domains
	{
        private IPlugin plugin;

        public PluginDomains(IPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetDomains(this.Database.Name);
            PopulateArray(metaData);
		}
	}
}
