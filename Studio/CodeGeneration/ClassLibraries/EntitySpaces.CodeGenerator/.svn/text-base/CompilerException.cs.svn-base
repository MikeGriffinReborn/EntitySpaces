using System;
using System.Text;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace EntitySpaces.CodeGenerator
{
    /// <summary>
    /// An exception that contains information about a template compilation. It is thrown only when 
    /// there is an error in compilation prevents the creation of an assembly.
    /// </summary>
    public class CompilerException : Exception
    {
        private CompilerResults _compilerResults;
        private Template _template;

        /// <summary>
        /// Create an exception that contains information about a template compilation.
        /// </summary>
        /// <param name="results">The CompilerResults from a template compilation.</param>
        /// <param name="template">The template that was being compiled.</param>
        public CompilerException(CompilerResults results, Template template) : base()
        {
            _compilerResults = results;
            _template = template;
        }

        /// <summary>
        /// The CompilerResults from a template compilation.< 
        /// </summary>
        public CompilerResults Results
        {
            get
            {
                return _compilerResults;
            }
        }

        /// <summary>
        /// The template that was being compiled.
        /// </summary>
        public Template Template
        {
            get
            {
                return _template;
            }
        }
    }
}
