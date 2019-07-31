using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginProcedures : Procedures
    {
        private IPlugin plugin;

		#region DataColumn Binding Stuff

		// Added for 3rd party providers
		internal DataColumn f_procText = null;	

		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("PROCEDURE_TEXT"))
					f_procText = metaData.Columns["PROCEDURE_TEXT"];
			}																		
		}
		#endregion

        public PluginProcedures(IPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetProcedures(this.Database.Name);
			BindToColumns(metaData);
            PopulateArray(metaData);
		}
	}
}
