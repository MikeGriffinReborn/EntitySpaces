using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;

namespace EntitySpaces.CodeGenerator
{
    internal class TemplateCache
    {
        internal static CachedTemplate GetCachedTemplate(string templateName, CompileAction action)
        {
            if (_cache.Count == 0)
            {
                CleanupOldTemplates();
            }

            CachedTemplate cachedTemplate = null;

            if (_cache.ContainsKey(templateName))
            {
                cachedTemplate = _cache[templateName];

                DateTime lastUpdate = File.GetLastWriteTime(templateName);

                // Has the file changed since we last cached it?
                if (lastUpdate > cachedTemplate.CompileTime)
                {
                    // Update it
                    cachedTemplate.CodeBuilder = Parser.ParseMarkup(templateName);

                    cachedTemplate.CompiledAssembly = null;
                    cachedTemplate.CompilerResults = null;
                }

                if (action == CompileAction.Compile && cachedTemplate.CompiledAssembly == null)
                {
                    cachedTemplate.CompilerResults = Compiler.Compile(cachedTemplate.CodeBuilder);

                    if (!cachedTemplate.CompilerResults.Errors.HasErrors)
                    {
                        cachedTemplate.CompiledAssembly = cachedTemplate.CompilerResults.CompiledAssembly;
                    }
                }

                cachedTemplate.CompileTime = DateTime.Now;
            }
            else
            {
                cachedTemplate = new CachedTemplate();
                cachedTemplate.Name = templateName;
                cachedTemplate.CodeBuilder = Parser.ParseMarkup(templateName);

                if (action == CompileAction.Compile)
                {
                    CompilerResults results = Compiler.Compile(cachedTemplate.CodeBuilder);
                    cachedTemplate.CompiledAssembly = results.CompiledAssembly;
                    cachedTemplate.CompilerResults = results;
                }

                cachedTemplate.CompileTime = DateTime.Now;
                _cache[templateName] = cachedTemplate;
            }

            return cachedTemplate;
        }

        internal static void CleanupOldTemplates()
        {
            FileInfo entryPath = new FileInfo(Compiler.TemplateCachePath);
            DirectoryInfo di = entryPath.Directory;
            FileInfo[] files = di.GetFiles("esCompiledTemplate_*.*");

            foreach (FileInfo f in files)
            {
                try
                {
                    f.Delete();
                }
                catch { }
            }
        }

        private static Dictionary<string, CachedTemplate> _cache = new Dictionary<string, CachedTemplate>();
    }

    internal class CachedTemplate
    {
        public string Name;
        public Assembly CompiledAssembly;
        public CodeBuilder CodeBuilder;
        public DateTime CompileTime;
        public CompilerResults CompilerResults;
    }

    internal enum CompileAction
    {
        ParseOnly = 0,
        Compile
    }
}

