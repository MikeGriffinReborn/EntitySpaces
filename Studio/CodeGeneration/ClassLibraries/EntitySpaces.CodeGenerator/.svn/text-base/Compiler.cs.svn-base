using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace EntitySpaces.CodeGenerator
{
    /// <summary>
    /// This class Compiles a parsed template in the form of a CodeBuilder object and returns the compiler results.
    /// </summary>
    internal class Compiler
    {
        internal static string TemplateCachePath = string.Empty;
        internal static string CompilerAssemblyPath = string.Empty;

        /// <summary>
        /// Compiles a parsed template in the form of a CodeBuilder object and returns the compiler results.
        /// </summary>
        /// <param name="code">A parsed template in the form of a CodeBuilder object</param>
        /// <returns>The results of compilation.</returns>
        internal static CompilerResults Compile(CodeBuilder code)
        {
            CompilerResults results = null; 
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("C#");
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.CompilerOptions = "/target:library /optimize";

            string assembly = "";

            foreach (string reference in code.References)
            {
                assembly = CompilerAssemblyPath + reference + ".dll";

                if(File.Exists(assembly))
                    compilerParams.ReferencedAssemblies.Add(assembly);
                else
                    compilerParams.ReferencedAssemblies.Add(reference + ".dll");
            }

            if (code.CompileInMemory)
            {
                compilerParams.GenerateExecutable = false;
                compilerParams.IncludeDebugInformation = false;
                compilerParams.GenerateInMemory = true;

                results = codeProvider.CompileAssemblyFromSource(compilerParams, code.ToString());
            }
            else
            {
                string baseName = "esCompiledTemplate_" + Guid.NewGuid().ToString().Replace("-", "");
                string codeFileName = TemplateCachePath + baseName + ".cs";
                string assemblyName = TemplateCachePath + baseName + ".dll";
                File.WriteAllText(codeFileName, code.ToString());
                string[] files = new string[] { codeFileName };
                

                compilerParams.GenerateExecutable = false;
                compilerParams.IncludeDebugInformation = true;
                compilerParams.GenerateInMemory = false;
                compilerParams.OutputAssembly = assemblyName;
                

                results = codeProvider.CompileAssemblyFromFile(compilerParams, files);
            }

            return results;
        }
    }
}
