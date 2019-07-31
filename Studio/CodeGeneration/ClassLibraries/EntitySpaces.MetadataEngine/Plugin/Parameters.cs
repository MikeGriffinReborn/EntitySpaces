using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginParameters : Parameters
    {
        private IPlugin plugin;

        public PluginParameters(IPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetProcedureParameters(this.Procedure.Database.Name, this.Procedure.Name);
            PopulateArray(metaData);
		}
	}
}
