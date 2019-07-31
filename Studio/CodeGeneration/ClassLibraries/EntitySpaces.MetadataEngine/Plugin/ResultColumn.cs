using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Plugin
{
	public class PluginResultColumn : ResultColumn
    {
        private IPlugin plugin;

        public PluginResultColumn(IPlugin plugin)
        {
            this.plugin = plugin;
		}

        // k3b 20070709: implemented missing Properties
        #region Properties

        override public string Name
        {
            get
            {
                return this.GetString(((Plugin.PluginResultColumns) this.ResultColumns).f_Name);
            }
        }

        override public System.Int32 DataType
        {
            get
            {
                return this.GetInt32(((Plugin.PluginResultColumns)this.ResultColumns).f_DataType); 
            }
        }

        override public string DataTypeName
        {
            get
            {
                return this.GetString(((Plugin.PluginResultColumns)this.ResultColumns).f_DataTypeName);
            }
        }

        override public string DataTypeNameComplete
        {
            get
            {
                return this.GetString(((Plugin.PluginResultColumns)this.ResultColumns).f_DataTypeNameComplete);
            }
        }

        override public System.Int32 Ordinal
        {
            get
            {
                return this.GetInt32(((Plugin.PluginResultColumns)this.ResultColumns).f_Ordinal);
            }
        }

        #endregion
    }
}
