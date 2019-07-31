using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace EntitySpaces.QuerySandbox
{
    public partial class ReferencesForm : DevExpress.XtraEditors.XtraForm
    {
        private ReferenceList referenceList;

        public ReferencesForm(ReferenceList refList)
        {
            InitializeComponent();

            this.referenceList = refList;
            UpdateReferencesList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Open .NET Assembly";
            openFileDialog.Filter = ".NET assembly (*.dll;*.exe)|*.dll;*.exe";

            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;

            try
            {
                string dstPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                dstPath = dstPath.Replace("esQuerySandbox.exe", "");

                List<string> newAssemblies = new List<string>();

                foreach (string sPath in openFileDialog.FileNames)
                {
                    string dPath = Path.Combine(dstPath, Path.GetFileName(sPath));

                    File.Delete(dPath);
                    File.Copy(sPath, dPath);

                    referenceList.AssemblyPaths.Add(dPath);
                    newAssemblies.Add(dPath);
                }

                foreach (string assembly in newAssemblies)
                {
                    referenceList.ProjectResolver.AddExternalReference(assembly);
                }

                this.UpdateReferencesList();
                listBox.SelectedIndex = listBox.ItemCount - 1;
            }
            catch (Exception ex)
            {
                string innerExceptionMessage = String.Empty;
                if (ex.InnerException != null)
                    innerExceptionMessage = "\r\n\r\nInner exception: " + ex.InnerException.Message;
                MessageBox.Show(this, "An exception occurred while loading the assembly: " + ex.Message 
                    + "\r\n\r\nPlease make sure that any referenced assemblies are in the same folder." + innerExceptionMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void UpdateReferencesList()
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            foreach (string assemblyName in referenceList.ProjectResolver.ExternalReferences)
                listBox.Items.Add(assemblyName);
            listBox.EndUpdate();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                int index = listBox.SelectedIndex;

                string assemblyName = listBox.SelectedItem.ToString();

                string[] parts = assemblyName.Split(',');
                string assemblyDll = parts[0] + ".dll";
                assemblyDll = assemblyDll.ToLower();

                bool found = false;
                string assemblyToDelete = string.Empty;

                foreach (string name in referenceList.AssemblyPaths)
                {
                    string filename = Path.GetFileName(name);

                    if (filename.ToLower() == assemblyDll)
                    {
                        assemblyToDelete = name;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    referenceList.AssemblyPaths.Remove(assemblyToDelete);
                }

                referenceList.ProjectResolver.RemoveExternalReference(listBox.SelectedItem.ToString());
                this.UpdateReferencesList();

                listBox.SelectedIndex = Math.Min(index, listBox.ItemCount - 1);
            }
        }
    }
}