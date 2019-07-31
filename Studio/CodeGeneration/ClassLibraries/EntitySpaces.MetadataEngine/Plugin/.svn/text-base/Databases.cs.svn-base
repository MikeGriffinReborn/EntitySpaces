using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginDatabases : Databases
	{
        private IPlugin plugin;

        public PluginDatabases(IPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
		{
			DataTable metaData  = this.plugin.Databases;
			PopulateArray(metaData);
		}
	}
}
