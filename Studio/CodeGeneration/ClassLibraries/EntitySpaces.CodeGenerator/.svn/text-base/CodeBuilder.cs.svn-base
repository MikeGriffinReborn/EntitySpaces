using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntitySpaces.CodeGenerator
{
    /// <summary>
    /// The CodeBuilder object manages the relationship between the original template code and
    /// the parsed template code that is actually compiled. It allows the programmer to match the
    /// line number of an error with the original line of code it occurred on in the template file.
    /// </summary>
    internal class CodeBuilder
    {
        #region Fields

        private Dictionary<int, List<int>> _raw2parsed = null;
        private Dictionary<int, List<int>> _parsed2raw = null;

        private List<string> _references = new List<string>();
        private List<string> _imports = new List<string>();
        private List<Dictionary<string, string>> _properties = new List<Dictionary<string,string>>();
        private List<CodeLine> _lines = new List<CodeLine>();
        private int _index = 0;
        private int _newlineLen = Environment.NewLine.Length;
        private bool _compileInMemory = false;
        private string _className = "esTemplate";
        private string _filePath = string.Empty;

        internal TemplateHeader Header = new TemplateHeader();

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Creates a CodeBuilder object.
        /// </summary>
        public CodeBuilder() { }

        #endregion

        #region Properties

        /// <summary>
        /// The lines of code
        /// </summary>
        public ReadOnlyCollection<CodeLine> Lines
        {
            get
            {
                return _lines.AsReadOnly();
            }
        }

        /// <summary>
        /// The templates file path
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;

                FileInfo fileInfo = new FileInfo(_filePath);

                Header.FullFileName = fileInfo.FullName;
                Header.FilePath = fileInfo.DirectoryName;
                Header.FileName = fileInfo.Name;
            }
        }

        /// <summary>
        /// The generated template class name.
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        /// <summary>
        /// The current source line index
        /// </summary>
        public int SourceLineIndex
        {
            get { return _index; }
            set { _index = value; }
        }

        /// <summary>
        /// the number of lines of code
        /// </summary>
        public int LineCount
        {
            get { return _lines.Count; }
        }

        /// <summary>
        /// Enable or disable template debug mode.
        /// </summary>
        public bool CompileInMemory
        {
            get { return _compileInMemory; }
            set { _compileInMemory = value; }
        }

        /// <summary>
        /// Namespace imports
        /// </summary>
        public List<string> Imports
        {
            get
            {
                return _imports;
            }
        }

        /// <summary>
        /// Assembly references.
        /// </summary>
        public List<string> References
        {
            get
            {
                return _references;
            }
        }

        /// <summary>
        /// Properties
        /// </summary>
        public List<Dictionary<string, string>> Properties
        {
            get
            {
                return _properties;
            }
        }

        private Dictionary<int, List<int>> Raw2Parsed
        {
            get
            {
                if (_raw2parsed == null)
                {
                    BuildIndex();
                }
                return _raw2parsed;
            }
        }

        private Dictionary<int, List<int>> Parsed2Raw
        {
            get
            {
                if (_parsed2raw == null)
                {
                    BuildIndex();
                }
                return _parsed2raw;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Append a string to the code builder.
        /// </summary>
        /// <param name="s">The string to append</param>
        public void Append(string s)
        {
            int x = 0,
                y = 0;

            x = s.IndexOf(Environment.NewLine, y);
            while (x >= 0)
            {
                x += _newlineLen;
                _lines.Add(new CodeLine(s.Substring(y, (x - y)), _index, 1));

                y = x;
                if (x < s.Length)
                    x = s.IndexOf(Environment.NewLine, y);
                else
                    x = -1;
            }
            if (y < s.Length)
            {
                _lines.Add(new CodeLine(s.Substring(y), _index, 0));
            }
        }

        /// <summary>
        /// A generic way to insert any object at any location in the code. This could be for appending 
        /// a header to the generated template code or something like that.
        /// </summary>
        /// <param name="index">The index at which the text is inserted.</param>
        /// <param name="s">The string to insert</param>
        public void Insert(int insertIndex, string s)
        {
            if (insertIndex < this.LineCount)
            {
                int newIndex = 0;
                CodeLine prevLine = null;
                if (insertIndex != 0)
                {
                    prevLine = _lines[insertIndex - 1];
                    newIndex = prevLine.Index;
                }

                int x = 0,
                    y = 0;

                x = s.IndexOf(Environment.NewLine, y);
                while (x >= 0)
                {
                    x += _newlineLen;
                    _lines.Insert(insertIndex++, new CodeLine(s.Substring(y, (x - y)), newIndex, 1));

                    y = x;
                    x = s.IndexOf(Environment.NewLine, y);
                }
                if (y < s.Length)
                {
                    _lines.Insert(insertIndex++, new CodeLine(s.Substring(y), newIndex, 0));
                }
            }
        }

        /// <summary>
        /// a generic way to append any object.
        /// </summary>
        /// <param name="o">A object that can be transformed into a string.</param>
        public void Append(object o)
        {
            if (o != null) Append(o.ToString());
        }

        /// <summary>
        /// A generic way to insert any object at any location in the code. This could be for appending 
        /// a header to the generated template code or something like that.
        /// </summary>
        /// <param name="index">The index at which the text is inserted.</param>
        /// <param name="o">A object that can be transformed into a string.</param>
        public void Insert(int index, object o)
        {
            if (o != null) Insert(index, o.ToString());
        }

        /// <summary>
        /// Clear all lines from the CodeBuilder.
        /// </summary>
        public void Clear()
        {
            _lines.Clear();
        }

        /// <summary>
        /// Writes all of the code to a single string.
        /// </summary>
        /// <returns>All of the code in a single string.</returns>
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            foreach (CodeLine line in _lines)
            {
                b.Append(line);
            }
            return b.ToString();
        }

        /// <summary>
        /// Gets the index in the original template from a parsed generated code line index. 
        /// If you have a compile/run time error with your template, use this to get the
        /// matching template line number.
        /// </summary>
        /// <param name="index">A line index from the parsed, generated code.</param>
        /// <returns></returns>
        public List<int> GetRawIndex(int index)
        {
            if (!Parsed2Raw.ContainsKey(index)) return new List<int>();
            return Parsed2Raw[index];
        }

        /// <summary>
        /// The opposite of GetRawIndex. This will give you the parsed generated code index from 
        /// a template line index.
        /// </summary>
        /// <param name="index">An line index in the original template</param>
        /// <returns>A line index from the parsed, generated code.</returns>
        public List<int> GetParsedIndex(int index)
        {
            if (!Raw2Parsed.ContainsKey(index)) return new List<int>();
            return Raw2Parsed[index];
        }

        /// <summary>
        /// Writes all of the code to a single string with line numbers.
        /// </summary>
        /// <returns>All of the code in a single string with line numbers.</returns>
        public string ToString(bool showLineNumbers)
        {
            if (showLineNumbers)
            {
                int i = 0;
                StringBuilder b = new StringBuilder();
                foreach (CodeLine line in _lines)
                {
                    b.Append(line.Index.ToString().PadLeft(3));
                    b.Append(i.ToString().PadLeft(3));
                    b.Append(line);
                    i += line.TargetIndexIncrement;
                }
                return b.ToString();
            }
            else
            {
                return this.ToString();
            }
        }


        private void BuildIndex()
        {
            _raw2parsed = new Dictionary<int, List<int>>();
            _parsed2raw = new Dictionary<int, List<int>>();
            List<int> tmp;

            int raw = 0, parsed = 0;
            foreach (CodeLine line in _lines)
            {
                raw = line.Index;

                // Raw To Parsed
                if (_raw2parsed.ContainsKey(raw))
                {
                    tmp = _raw2parsed[raw];
                }
                else
                {
                    tmp = new List<int>();
                }
                if (!tmp.Contains(parsed)) tmp.Add(parsed);
                _raw2parsed[raw] = tmp;

                // Parsed To Raw
                if (_parsed2raw.ContainsKey(parsed))
                {
                    tmp = _parsed2raw[parsed];
                }
                else
                {
                    tmp = new List<int>();
                }
                if (!tmp.Contains(raw)) tmp.Add(raw);
                _parsed2raw[parsed] = tmp;

                parsed += line.TargetIndexIncrement;
            }
        }

        #endregion

        #region Nested CodeLine Class

        /// <summary>
        /// A class representing a line of code
        /// </summary>
        internal class CodeLine : IComparable
        {
            private string _line = string.Empty;
            private int _sourceIndex = 0;
            private int _targetIndexIncrement = 0;

            /// <summary>
            /// Create a new CodeLine object representing a line of parsed code for a template.
            /// </summary>
            /// <param name="line">The line text</param>
            /// <param name="sourceIndex">The line index of the original template code.</param>
            /// <param name="targetIndexIncrement">The parsed/generated code index.</param>
            public CodeLine(string line, int sourceIndex, int targetIndexIncrement)
            {
                this._line = line;
                this._sourceIndex = sourceIndex;
                this._targetIndexIncrement = targetIndexIncrement;
            }

            /// <summary>
            /// The line index of the original template code.
            /// </summary>
            public int Index { get { return _sourceIndex; } }
            
            /// <summary>
            /// The parsed/generated code index.
            /// </summary>
            public int TargetIndexIncrement { get { return _targetIndexIncrement; } }

            /// <summary>
            /// Returns the line as a string.
            /// </summary>
            /// <returns>The line as a string</returns>
            public override string ToString()
            {
                return _line;
            }


            /// <summary>
            /// Compares to another CodeLine object.
            /// </summary>
            /// <param name="obj">the CodeLine object to compare to</param>
            /// <returns>0 if equal, - if less, + if greater.</returns>
            public int CompareTo(object obj)
            {
                int compare = 0;

                if (obj is CodeLine)
                {
                    CodeLine line = obj as CodeLine;
                    compare = _sourceIndex.CompareTo(line._sourceIndex);
                }

                return compare;
            }
        }

        #endregion
    }
}
