using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using EntitySpaces.MetadataEngine;

namespace EntitySpaces.CodeGenerator
{
    /// <summary>
    /// The template class represents an EntitySpaces template. It is also used as a Controller for executing EntitySpaces templates.
    /// </summary>
    public class Template
    {
        private CodeBuilder _codeBuilder;
        private Assembly _assembly;
        private CompilerResults _results;
        private TemplateHeader _templateHeader;
        private StringBuilder _buffer;

        /// <summary>
        /// Create the EntitySpaces template controller. In order to execute a template, you need to call 
        /// the Execute method and pass in the esMeta and a template location.
        /// </summary>
        public Template() 
        {

        }

        static public void SetTemplateCachePath(string path)
        {
            if (!path.EndsWith(@"\"))
            {
                path += @"\";
            }
            Compiler.TemplateCachePath = path;
        }

        static public void SetCompilerAssemblyPath(string path)
        {
            if (!path.EndsWith(@"\"))
            {
                path += @"\";
            }
            Compiler.CompilerAssemblyPath = path;
        }

        /// <summary>
        /// Contains the Template header information that would come from this tag:
        /// 
        /// <%@ TemplateInfo 
        ///     UniqueID="9D9C3562-88A0-4e9b-B706-6AA92BCC8C53" 
        ///     Title="Generated - esCollection (C#)"   
        ///     Description="The Abstract Collection Class" 
        ///     Namespace="EntitySpaces 2009.C#.Generated" 
        ///     Author="EntitySpaces, LLC" %>
        ///     
        /// </summary>
        public TemplateHeader Header
        {
            get
            {
                if (this._templateHeader == null)
                {
                    if (this._codeBuilder != null)
                    {
                        this._templateHeader = _codeBuilder.Header;
                    }
                    else
                    {
                        this._templateHeader = new TemplateHeader();
                    }
                }

                return this._templateHeader;
            }
        }


        /// <summary>
        /// The output of all template executions since the output was last cleared.
        /// </summary>
        public string Output
        {
            get { return _buffer.ToString(); }
        }

        /// <summary>
        /// The unparsed template line index from a compiled template line index.
        /// </summary>
        /// <param name="lineNumber">A compiled template line index.</param>
        /// <returns>A unparsed template line index.</returns>
        public int TemplateLineFromErrorLine(int lineNumber)
        {
            int index = -1;
            if (_codeBuilder != null)
            {
                List<int> indeces = _codeBuilder.GetRawIndex(lineNumber-1);
                if (indeces.Count > 0) index = indeces[0];
            }
            return index + 1;
        }

        /// <summary>
        /// This method only parses the template so we can get to the header info.
        /// </summary>
        /// <param name="templateName">The template file path.</param>
        public void Parse(string templateName)
        {
            this._templateHeader = null;

            CachedTemplate cachedTemplate = TemplateCache.GetCachedTemplate(templateName, CompileAction.ParseOnly);

            _codeBuilder = cachedTemplate.CodeBuilder;
            _assembly = cachedTemplate.CompiledAssembly;
            _results = cachedTemplate.CompilerResults;
        }

        /// <summary>
        /// Compiles a template, and set the current template information accordingly. Note that this does not execute the template.
        /// </summary>
        /// <param name="templateName">The template file path.</param>
        public void Compile(string templateName)
        {
            this._templateHeader = null;

            CachedTemplate cachedTemplate = TemplateCache.GetCachedTemplate(templateName, CompileAction.Compile);

            _codeBuilder = cachedTemplate.CodeBuilder;
            _assembly = cachedTemplate.CompiledAssembly;
            _results = cachedTemplate.CompilerResults;

            if (_results != null && _results.Errors.HasErrors)
            {
                throw new CompilerException(_results, this);
            }
        }

        /// <summary>
        /// Compiles and executes a template, and also sets the current template information accordingly.
        /// </summary>
        /// <param name="esMeta">The esMeta object which holds the context needed for an EntitySpaces template to generate meaningful code.</param>
        /// <param name="templateLocation">The template file path.</param>
        public void Execute(Root esMeta, string templateLocation)
        {
            try
            {
                _buffer = new StringBuilder();

                this._templateHeader = null;

                Compile(templateLocation);

                if (_assembly != null)
                {
                    Type type = _assembly.GetTypes()[0];
                    ConstructorInfo info = type.GetConstructor(new Type[] { typeof(Root), typeof(StringBuilder) });
                    object o = info.Invoke(new object[] { esMeta, _buffer });
                    MethodInfo m = type.GetMethod("Render");
                    m.Invoke(o, new object[] { this });
                }
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        /// <summary>
        /// Clear the generated code buffer.
        /// </summary>
        public void ClearOutput()
        {
            _buffer.Remove(0, _buffer.Length);
        }
    }
}
