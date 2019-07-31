using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom;
using System.CodeDom.Compiler;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.IO;
using EntitySpaces.SandboxLoader;

namespace EntitySpaces.QuerySandbox
{
    public class CSharpExecuter
    {
        ReferenceList referenceList;

        public CSharpExecuter(ReferenceList referenceList)
        {
            this.referenceList = referenceList;
        }

        public object[] Execute(string text, string connectionString, string provider, string providerMetaData, string namespaces, bool parseOnly)
        {
            string assemblyName = "esQuerySandbox_Dynamic.dll";
            AppDomain loAppDomain = null;
            IRemoteInterface loRemote = null;
            object loResult = null;

            try
            {

                // ** Create an AppDomain
                AppDomainSetup loSetup = new AppDomainSetup();
                loSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                loSetup.ConfigurationFile = "esQuerySandbox.redirect.config";
                loAppDomain = AppDomain.CreateDomain("MyAppDomain", null, loSetup);

                string code = GetSnippet(text, connectionString, provider, providerMetaData, namespaces, parseOnly);

                CodeDomProvider compiler = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters();

                // *** Start by adding any referenced assemblies
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.dll");
                parameters.ReferencedAssemblies.Add("System.Data.dll");
                parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                parameters.ReferencedAssemblies.Add("System.Configuration.dll");
                parameters.ReferencedAssemblies.Add("EntitySpaces.SandboxLoader.dll");

                foreach (string assemblyFileName in referenceList.AssemblyPaths)
                {
                    parameters.ReferencedAssemblies.Add(assemblyFileName);
                }

                // *** Load the resulting assembly into memory
                parameters.GenerateInMemory = false;
                parameters.OutputAssembly = assemblyName;

                // *** Now compile the whole thing
                CompilerResults loCompiled = compiler.CompileAssemblyFromSource(parameters, code);

                if (loCompiled.Errors.HasErrors)
                {
                    string lcErrorMsg = "";

                    // *** Create Error String
                    lcErrorMsg = loCompiled.Errors.Count.ToString() + " Errors:";
                    for (int x = 0; x < loCompiled.Errors.Count; x++)
                        lcErrorMsg = lcErrorMsg + "\r\nLine: " + (loCompiled.Errors[x].Line - 21).ToString() + " - " +
                            loCompiled.Errors[x].ErrorText;

                    MessageBox.Show(lcErrorMsg, "Compiler Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                string compiledCode = code;

                // create the factory class in the secondary app-domain
                RemoteLoaderFactory factory =
                    (RemoteLoaderFactory)loAppDomain.CreateInstance("EntitySpaces.SandboxLoader",
                    "EntitySpaces.SandboxLoader.RemoteLoaderFactory").Unwrap();

                // with the help of this factory, we can now create a real 'LiveClass' instance
                object loObject = factory.Create(assemblyName, "MyNamespace.MyClass", null);

                // *** Cast the object to the remote interface to avoid loading type info
                loRemote = (IRemoteInterface)loObject;

                if (loObject == null)
                {
                    MessageBox.Show("Couldn't load class.");
                    return null;
                }

                try
                {
                    object[] codeParms = new object[1];
                    codeParms[0] = "EntitySpaces, LLC";

                    // *** Indirectly call the remote interface
                    loResult = loRemote.Invoke("DynamicCode", codeParms);
                }
                catch (Exception loError)
                {
                    MessageBox.Show(loError.Message, "DynamicCode Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                loRemote = null;
                if (loAppDomain != null)
                {
                    AppDomain.Unload(loAppDomain);
                    loAppDomain = null;
                }
                File.Delete(assemblyName);
            }

            return (object[])loResult;
        }

        private string GetSnippet(string text, string connectionString, string provider, string providerMetaData, string namespaces, bool parseOnly)
        {
            string usings = "";

            if (!string.IsNullOrEmpty(namespaces))
            {
                string[] spcs = namespaces.Replace("\r\n", "|").Split('|');

                foreach (string namespc in spcs)
                {
                    usings += "using " + namespc + ";\r\n";
                }
            }

            // *** Must create a fully functional assembly
            string code = @"using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using EntitySpaces.SandboxLoader;

using EntitySpaces.Core;
using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;
using EntitySpaces.Loader;"
+ usings + @"

namespace MyNamespace 
{
    public class MyClass : MarshalByRefObject,IRemoteInterface  
    {
        private object ExecuteTheUserCode()
        {
" + text + @"
        
        }

        public object DynamicCode(params object[] parameters) 
        {
            try
            {
                SetupConnection(parameters);

                object[] returnResult = new object[2];
                returnResult[0] = null;
                returnResult[1] = null;

                object data = ExecuteTheUserCode();

                if(data is esEntityCollectionBase)
                {
                    esEntityCollectionBase coll = data as esEntityCollectionBase;

                    esEntity entity = null;
                    foreach(esEntity obj in coll)
                    {
                        entity = obj;
                        break;
                    }

                    returnResult[0] = CreateDataTable(coll, entity);
                    returnResult[1] = coll.es.Query.es.LastQuery;
                }
                else if(data is esDynamicQuery)
                {
                    esDynamicQuery q = data as esDynamicQuery;
                    returnResult[1] = q.Parse();
                }

                return returnResult;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ""DynamicCode - "" + ex.Message);
            }

            return null;
        }

        public object Invoke(string lcMethod, object[] Parameters) 
        {
            try
            {
	            return this.GetType().InvokeMember(lcMethod, BindingFlags.InvokeMethod, null, this, Parameters);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ""DynamicCode"");
            }

            return null;
        }

        private void SetupConnection(params object[] Parameters)
        {
            esProviderFactory.Factory = new esDataProviderFactory();

            esConnectionElement conn = new esConnectionElement();
            conn.Name = ""SQL""; 
            conn.ConnectionString = """ + connectionString + "\";" + @"
            conn.Provider = ""EntitySpaces." + provider + "\";" + @" 
            conn.ProviderClass = ""DataProvider""; 
            conn.SqlAccessType = esSqlAccessType.DynamicSQL; 
            conn.ProviderMetadataKey = """ + providerMetaData + "\";" + @"
            conn.DatabaseVersion = ""2005"";

            esConfigSettings.ConnectionInfo.Connections.Add(conn); 
            esConfigSettings.ConnectionInfo.Default = ""SQL"";
        }

        public DataTable CreateDataTable(esEntityCollectionBase coll, esEntity entity)
        {
            DataTable table = new DataTable();
            DataColumnCollection dataColumns = table.Columns;

            List<string> trueColumns = new List<string>();

            // Stnadard Columns
            foreach (esColumnMetadata esCol in coll.es.Meta.Columns)
            {
                // If entity is null then the collection is empty we need to create
                // columns however, othewise we create only the columns that were returned
                // by the query or procedure
                if (entity == null || entity.ContainsColumn(esCol.Name))
                {
                    dataColumns.Add(new DataColumn(esCol.Name, esCol.Type));

                    // We're going to use this later
                    trueColumns.Add(esCol.Name);
                }
            }

            // Extra Columns
            if(entity != null)
            {
                Dictionary<string, object> extra = entity.GetExtraColumns();

                foreach(string columnName in extra.Keys)
                {
                    dataColumns.Add(new DataColumn(columnName, extra[columnName].GetType()));
                }
            }

            foreach (esEntity ent in coll)
            {
                DataRow row = table.NewRow();
                table.Rows.Add(row);

                foreach (string columnName in trueColumns)
                {
                    if (ent.ContainsColumn(columnName))
                    {
                        object value = ent.GetColumn(columnName);
                        row[columnName] = value == null ? DBNull.Value : value;
                    }
                }

                Dictionary<string, object> extra = ent.GetExtraColumns();

                foreach (string columnName in extra.Keys)
                {
                    object value = extra[columnName];
                    row[columnName] = value == null ? DBNull.Value : value;
                }
            }

            return table;
        }
    }    
}";
            return code;
        }
    }
}
