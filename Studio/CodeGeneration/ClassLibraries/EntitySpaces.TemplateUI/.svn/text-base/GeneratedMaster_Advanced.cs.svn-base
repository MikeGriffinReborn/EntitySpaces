using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EntitySpaces.AddIn.TemplateUI;
using EntitySpaces.MetadataEngine;

namespace EntitySpaces.TemplateUI
{
    public partial class GeneratedMaster_Advanced : UserControl, ITemplateUI
    {
        private Root esMeta;
        private bool UseCachedSettings;
        private object applicationObject;

        public GeneratedMaster_Advanced()
        {
            InitializeComponent();
        }

        #region ITemplateUI Members

        esTemplateInfo ITemplateUI.Init()
        {
            esTemplateInfo info = new esTemplateInfo();
            info.UserInterface = this;
            info.UserInterfaceId = new Guid("2216AB4F-BDB4-47de-8412-8560C1F2F420");
            info.TabTitle = "Advanced";
            info.TabOrder = 1;
            return info;
        }

        UserControl ITemplateUI.CreateInstance(Root esMeta, bool cachedSettings, object applicationObject)
        {
            GeneratedMaster_Advanced window = new GeneratedMaster_Advanced();
            window.esMeta = esMeta;
            window.applicationObject = applicationObject;
            window.UseCachedSettings = cachedSettings;

            return window;
        }

        bool ITemplateUI.OnExecute()
        {
            try
            {
                esMeta.Input["GenerateSingleFile"] = chkSingleFile.Checked;
                esMeta.Input["UseCustomBaseClass"] = chkUseCustomBase.Checked;
                esMeta.Input["TargetMultipleDatabases"] = chkProvIndependent.Checked;
                esMeta.Input["MetadataClassShouldIgnoreSchema"] = chkIgnoreSchema.Checked;
                esMeta.Input["MetadataClassShouldIgnoreCatalog"] = chkIgnoreCatalog.Checked;
                esMeta.Input["GenerateHierarchicalModel"] = chkHierarchical.Checked;
                esMeta.Input["GenerateHierarchicalModelSelectedTablesOnly"] = chkHierarchicalSelectedOnly.Checked;
                esMeta.Input["GenerateHierarchicalRiaServicesSupport"] = chkRiaServicesSupport.Checked;
                esMeta.Input["GenerateHierarchicalDataContracts"] = chkHierarchicalDataContractSupport.Checked;
                esMeta.Input["TargetTheCompactFramework"] = chkCompactFramework.Checked;
                esMeta.Input["SupportINotifyChanged"] = chkINotifyPropertyChanged.Checked;
                esMeta.Input["GenerateStrProperties"] = chkGenerateStrProperties.Checked;
                esMeta.Input["UseDnnObjectQualifier"] = chkUseDNNObjectQualifier.Checked;
                esMeta.Input["LINQtoSQL"] = chkLINQtoSQL.Checked;
                esMeta.Input["SerializableQueries"] = chkSerializableQueries.Checked;
                esMeta.Input["DebuggerDisplay"] = chkDebuggerDisplay.Checked;
                esMeta.Input["DebugVisualizer"] = chkDebugVisualizer.Checked;
            }
            catch { }

            return true;
        }

        void ITemplateUI.OnCancel()
        {

        }

        #endregion

        private void GeneratedMaster_Advanced_Load(object sender, EventArgs e)
        {
            try
            {
                if (!UseCachedSettings) return;

                if (esMeta.Input.ContainsKey("GenerateSingleFile"))
                {
                    chkSingleFile.Checked = (bool)esMeta.Input["GenerateSingleFile"];
                }

                if (esMeta.Input.ContainsKey("UseCustomBaseClass"))
                {
                    chkUseCustomBase.Checked = (bool)esMeta.Input["UseCustomBaseClass"];
                }

                if (esMeta.Input.ContainsKey("TargetMultipleDatabases"))
                {
                    chkProvIndependent.Checked = (bool)esMeta.Input["TargetMultipleDatabases"];
                }

                if (esMeta.Input.ContainsKey("MetadataClassShouldIgnoreSchema"))
                {
                    chkIgnoreSchema.Checked = (bool)esMeta.Input["MetadataClassShouldIgnoreSchema"];
                }

                if (esMeta.Input.ContainsKey("MetadataClassShouldIgnoreCatalog"))
                {
                    chkIgnoreCatalog.Checked = (bool)esMeta.Input["MetadataClassShouldIgnoreCatalog"];
                }

                if (esMeta.Input.ContainsKey("GenerateHierarchicalModel"))
                {
                    chkHierarchical.Checked = (bool)esMeta.Input["GenerateHierarchicalModel"];
                }

                if (esMeta.Input.ContainsKey("GenerateHierarchicalModelSelectedTablesOnly"))
                {
                    chkHierarchicalSelectedOnly.Checked = (bool)esMeta.Input["GenerateHierarchicalModelSelectedTablesOnly"];
                }

                if (esMeta.Input.ContainsKey("GenerateHierarchicalRiaServicesSupport"))
                {
                    chkRiaServicesSupport.Checked = (bool)esMeta.Input["GenerateHierarchicalRiaServicesSupport"];
                }

                if (esMeta.Input.ContainsKey("GenerateHierarchicalDataContracts"))
                {
                    chkHierarchicalDataContractSupport.Checked = (bool)esMeta.Input["GenerateHierarchicalDataContracts"];
                }

                if (esMeta.Input.ContainsKey("TargetTheCompactFramework"))
                {
                    chkCompactFramework.Checked = (bool)esMeta.Input["TargetTheCompactFramework"];
                }

                if (esMeta.Input.ContainsKey("SupportINotifyChanged"))
                {
                    chkINotifyPropertyChanged.Checked = (bool)esMeta.Input["SupportINotifyChanged"];
                }

                if (esMeta.Input.ContainsKey("GenerateStrProperties"))
                {
                    chkGenerateStrProperties.Checked = (bool)esMeta.Input["GenerateStrProperties"];
                }

                if (esMeta.Input.ContainsKey("UseDnnObjectQualifier"))
                {
                    chkUseDNNObjectQualifier.Checked = (bool)esMeta.Input["UseDnnObjectQualifier"];
                }

                if (esMeta.Input.ContainsKey("LINQtoSQL"))
                {
                    chkLINQtoSQL.Checked = (bool)esMeta.Input["LINQtoSQL"];
                }

                if (esMeta.Input.ContainsKey("LINQtoSQL"))
                {
                    chkSerializableQueries.Checked = (bool)esMeta.Input["SerializableQueries"];
                }

                if (esMeta.Input.ContainsKey("DebuggerDisplay"))
                {
                    chkDebuggerDisplay.Checked = (bool)esMeta.Input["DebuggerDisplay"];
                }

                if (esMeta.Input.ContainsKey("DebugVisualizer"))
                {
                    chkDebugVisualizer.Checked = (bool)esMeta.Input["DebugVisualizer"];
                }
            }
            catch { }
        }

        private void chkCompactFramework_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCompactFramework.Checked)
            {
                chkDebuggerDisplay.Checked = false;
                chkDebugVisualizer.Checked = false;

                chkDebuggerDisplay.Enabled = false;
                chkDebugVisualizer.Enabled = false;
            }
            else
            {
                chkDebuggerDisplay.Enabled = true;
                chkDebugVisualizer.Enabled = true;
            }
        }

        private void chkHierarchical_CheckedChanged(object sender, EventArgs e)
        {
            chkHierarchicalSelectedOnly.Enabled = chkHierarchical.Checked;
            chkRiaServicesSupport.Enabled = chkHierarchical.Checked;
            chkHierarchicalDataContractSupport.Enabled = chkHierarchical.Checked;
        }
    }
}