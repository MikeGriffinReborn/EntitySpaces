using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace EntitySpaces.CodeGenerator
{
    /// <summary>
    /// The Template parser for the EntitySpaces CodeGenerator. It populates a CodeBuilder class from a template file.
    /// </summary>
    internal class Parser
    {
        private const string MarkupOpenTag = "<%";
        private const string TagStartHeaders = "<%@";
        private const string TagStartShortcut = "<%=";
        private const string TagStartComment = "<%--";
        private const string TagEndComment = "--%>";
        private const string MarkupCloseTag = "%>";
        private const string RunAtTemplateOpenTag = "<script runat=\"template\">";
        private const string RunAtTemplateCloseTag = "</script>";
        private static string[,] Chars2Replace = new string[5, 2] { { "\\", "\\\\" }, { "\"", "\\\"" }, { "\r", "\\r" }, { "\n", "\\n" }, { "\t", "\\t" } };

        private const string ClassDeclarationText = @"
public class ";
        private const string IntrinsicObjectDeclarationText = @"
{
    private Root esMeta;
    private StringBuilder output;
    private Template Template;";

        private const string ConstructorText = @"(Root esMeta, StringBuilder output) 
    {
        this.esMeta = esMeta;
        this.output = output;
    }

    protected string ExecuteTemplate(Root esMeta, string templateLocation)
    {
        Template template = new Template();
        template.Execute(esMeta, templateLocation); 
        return template.Output;
    }

    protected void ExecuteTemplateAndAppend(Root esMeta, string templateLocation)
    {
        Template template = new Template();
        template.Execute(esMeta, templateLocation); 
        output.Append(template.Output);
    }

    protected void ClearOutput()
    {
        Template.ClearOutput();
    }

	// Enter your template redering logic in the method below
	public void Render(Template template)
	{
        this.Template = template;
        
";

        /// <summary>
        /// Parse a template.
        /// </summary>
        /// <param name="filename">The file path of the template.</param>
        /// <returns>A CodeBuilder representing the parsed template code.</returns>
        internal static CodeBuilder ParseMarkup(string filename)
        {
            using (StreamReader reader = new StreamReader(filename, true))
            {
                CodeBuilder builder = new CodeBuilder();
                builder.FilePath = filename;

                builder.Imports.Add("System");
                builder.Imports.Add("System.Text");
                builder.Imports.Add("System.IO");
                builder.Imports.Add("System.Collections");
                builder.Imports.Add("System.CodeDom.Compiler");
                builder.Imports.Add("EntitySpaces.CodeGenerator");
                builder.Imports.Add("EntitySpaces.MetadataEngine");
 
                builder.References.Add("System");
                builder.References.Add("EntitySpaces.CodeGenerator");
                builder.References.Add("EntitySpaces.MetadataEngine");

                StringBuilder codeBodyBuilder = new StringBuilder();
                StringBuilder altBuilder = new StringBuilder();
                StringBuilder usingBuilder = new StringBuilder();

                string line,
                    nextline,
                    nextTagToFind = string.Empty;

                int index,
                    i,
                    lineIndex = 0;

                bool inBlock = false,
                    wasCustomLine = false,
                    isShortcut = false,
                    isCustom = false,
                    isComment = false,
                    isRunAtTemplate = false;

                line = reader.ReadLine();
                i = reader.Peek();
                while (line != null)
                {
                    wasCustomLine = false;
                    builder.SourceLineIndex = lineIndex;
                    nextline = reader.ReadLine();

                    if (isRunAtTemplate)
                    {
                        if (line.StartsWith(RunAtTemplateCloseTag, StringComparison.CurrentCultureIgnoreCase))
                        {
                            isRunAtTemplate = false;
                        }
                        else
                        {
                            builder.Append(line + Environment.NewLine);
                        }
                    }
                    else
                    {
                        if (!inBlock && line.StartsWith(RunAtTemplateOpenTag, StringComparison.CurrentCultureIgnoreCase))
                        {
                            builder.Append("    }" + Environment.NewLine);
                            isRunAtTemplate = true;
                        }
                        else
                        {
                            //TagEndComment
                            // Get the index of the next start or end tag (like <% or %>)
                            if (inBlock && isComment)
                                index = line.IndexOf(TagEndComment);
                            else
                                index = line.IndexOf(inBlock ? MarkupCloseTag : MarkupOpenTag);

                            // If a tag was found, resolve any tag issues in the line
                            while (index >= 0)
                            {
                                if (inBlock)
                                {
                                    inBlock = false;

                                    if (isComment)
                                    {
                                        isComment = false;
                                        line = line.Substring(index + TagEndComment.Length);
                                    }
                                    else
                                    {
                                        if (isShortcut)
                                        {
                                            altBuilder.Append(line.Substring(0, index));
                                            builder.Append(BuildWriteCommand(altBuilder.ToString()));

                                            //Added an extra newline after command!
                                            altBuilder.Remove(0, altBuilder.Length);

                                            isShortcut = false;
                                        }
                                        else if (isCustom)
                                        {
                                            altBuilder.Append(line.Substring(0, index));
                                            BuildSpecialCommand(builder, altBuilder.ToString());

                                            //Added an extra newline after command?
                                            altBuilder.Remove(0, altBuilder.Length);

                                            isCustom = false;
                                        }
                                        else
                                        {
                                            builder.Append(line.Substring(0, index) + Environment.NewLine);
                                        }

                                        line = line.Substring(index + MarkupCloseTag.Length);
                                    }
                                }
                                else
                                {
                                    inBlock = true;
                                    if (index > 0)
                                    {
                                        //emptyCount
                                        bool isEmpty;
                                        string outText = EscapeLiteral(line.Substring(0, index), out isEmpty);
 
                                        if (!isEmpty && !wasCustomLine)
                                        {
                                            builder.Append(BuildWriteCommand(outText));
                                        }
                                    }

                                    if (index == line.IndexOf(TagStartComment))
                                    {
                                        isComment = true;
                                        line = line.Substring(index + TagStartComment.Length);
                                    }
                                    else if (index == line.IndexOf(TagStartShortcut))
                                    {
                                        isShortcut = true;
                                        line = line.Substring(index + TagStartShortcut.Length);
                                    }
                                    else if (index == line.IndexOf(TagStartHeaders))
                                    {
                                        wasCustomLine = true;
                                        isCustom = true;
                                        line = line.Substring(index + TagStartHeaders.Length);
                                    }
                                    else
                                    {
                                        line = line.Substring(index + MarkupOpenTag.Length);
                                    }
                                }

                                if (inBlock && isComment)
                                    index = line.IndexOf(TagEndComment);
                                else
                                    index = line.IndexOf(inBlock ? MarkupCloseTag : MarkupOpenTag);
                            }

                            if (inBlock)
                            {
                                if (isShortcut || isCustom)
                                {
                                    altBuilder.Append(line + Environment.NewLine);
                                }
                                else if (!isComment)
                                {
                                    builder.Append(line + Environment.NewLine);
                                }
                            }
                            else
                            {
                                if (!((nextline == null) && (line == string.Empty)) && !wasCustomLine)
                                {
                                    bool isEmpty;
                                    string outText = EscapeLiteral(line, out isEmpty);
                                    if (isEmpty) outText = string.Empty;
                                    builder.Append(BuildWriteLineCommand(outText));
                                }
                            }
                        }
                    }

                    line = nextline;
                    lineIndex++;
                }

                // Finish off the Render method
                builder.Append("}" + Environment.NewLine);

                foreach (string import in builder.Imports)
                {
                    usingBuilder.Append("using ").Append(import).Append(";").AppendLine();
                }

                usingBuilder.Append(ClassDeclarationText);
                usingBuilder.Append(builder.ClassName);
                usingBuilder.Append(IntrinsicObjectDeclarationText);

                // End class
                usingBuilder.Append(Environment.NewLine);
                // End Namespace
                usingBuilder.Append(Environment.NewLine);

                foreach (Dictionary<string, string> property in builder.Properties)
                {
                    string prop = "\tprivate ";

                    try
                    {
                        prop += property["Type"] + " " + property["Name"];

                        if (property.ContainsKey("Default"))
                        {
                            switch (property["Type"])
                            {
                                case "System.String":

                                    prop += " = \"" + property["Default"] + "\"";
                                    break;

                                case "System.Char":

                                    prop += " = '" + property["Default"] + "'";
                                    break;

                                default:

                                    prop += " = " + property["Default"];
                                    break;
                            }
                        }

                        prop += ";" + Environment.NewLine;

                        usingBuilder.Append(prop);
                    }
                    catch { }
                }

                usingBuilder.Append(Environment.NewLine + "\tpublic " + builder.ClassName);
                usingBuilder.Append(ConstructorText);

                builder.Insert(0, usingBuilder);
                builder.Insert(0, builder.Header.ToComment());

                return builder;
            }
        }

        /// <summary>
        /// Handling Special template tags like TemplateInfo, CodeTemplate, Assembly, Import and Debug.
        /// </summary>
        /// <param name="builder">The current CodeBuilder object</param>
        /// <param name="text">The text in the special command tag.</param>
        public static void BuildSpecialCommand(CodeBuilder builder, string text)
        {
            string element = string.Empty;
            Dictionary<string, string> parsedAttributes = new Dictionary<string, string>();

            string s = text.Trim();
            int idx = s.IndexOf(" ");
            if (idx > 0)
            {
                element = s.Substring(0, idx);
                string attributes = s.Substring(idx).Trim();

                int mode = 0;
                char c;
                string key = string.Empty, val = string.Empty;
                for (int i = 0; i < attributes.Length; i++)
                {
                    c = attributes[i];
                    switch (mode)
                    {
                        case 0:
                            if (c == '=') mode = 1;
                            else key += attributes[i];
                            break;
                        case 1:
                            if (c == '"') mode = 2;
                            break;
                        case 2:
                            if (c == '"') mode = 3;
                            else val += attributes[i];
                            break;
                        case 3:
                            if (c == '"')
                            {
                                val += c;
                                mode = 2;
                            }
                            else
                            {
                                parsedAttributes[key.Trim()] = val.Trim();
                                key = string.Empty;
                                val = string.Empty;
                                mode = 0;
                            }
                            break;
                    }

                    if (mode == 3)
                    {
                        parsedAttributes[key.Trim()] = val.Trim();
                    }
                }
            }
            else if (s.Equals("debug", StringComparison.CurrentCultureIgnoreCase))
            {
                parsedAttributes[string.Empty] = string.Empty;
            }

            if (parsedAttributes.Count > 0)
            {
                AddDirective(builder, element, parsedAttributes);
            }
        }

        private static void AddDirective(CodeBuilder builder, string element, Dictionary<string, string> parsedAttributes)
        {

            #region TemplateInfo
            if (element.Equals("templateinfo", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (string attribute in parsedAttributes.Keys)
                {
                    if (attribute.Equals("uniqueid", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.UniqueID = new Guid(parsedAttributes[attribute]);
                    }
                    else if (attribute.Equals("userinterfaceid", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.UserInterfaceID = new Guid(parsedAttributes[attribute]);
                    }
                    else if (attribute.Equals("namespace", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.Namespace = parsedAttributes[attribute];
                    }
                    else if (attribute.Equals("author", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.Author = parsedAttributes[attribute];
                    }
                    else if (attribute.Equals("description", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.Description = parsedAttributes[attribute];
                    }
                    else if (attribute.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.Title = parsedAttributes[attribute];
                    }
                    else if (attribute.Equals("version", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.Version = parsedAttributes[attribute];
                    }
                    else if (attribute.Equals("requiresui", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.RequiresUI = Convert.ToBoolean(parsedAttributes[attribute]);
                    }
                    else if (attribute.Equals("issubtemplate", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.Header.IsSubTemplate = Convert.ToBoolean(parsedAttributes[attribute]);
                    }
                }
            }
            #endregion

            #region Property
            else if (element.Equals("property", StringComparison.CurrentCultureIgnoreCase))
            {
                builder.Properties.Add(parsedAttributes);
            }
            #endregion

            #region CodeTemplate
            else if (element.Equals("codetemplate", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (string attribute in parsedAttributes.Keys)
                {
                    if (attribute.Equals("classname", StringComparison.CurrentCultureIgnoreCase))
                    {
                        builder.ClassName = parsedAttributes[attribute];
                    }
                }
            }
            #endregion

            #region Assembly
            else if (element.Equals("assembly", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (string attribute in parsedAttributes.Keys)
                {
                    if (attribute.Equals("name", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string value = parsedAttributes[attribute];

                        if (!builder.References.Contains(value)) builder.References.Add(value);
                    }
                }
            }
            #endregion

            #region Import
            else if (element.Equals("import", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (string attribute in parsedAttributes.Keys)
                {
                    if (attribute.Equals("namespace", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string value = parsedAttributes[attribute];

                        if (!builder.Imports.Contains(value)) builder.Imports.Add(value);
                    }
                }
            }
            #endregion

            #region CompileInMemory
            else if (element.Equals("CompileInMemory", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (string attribute in parsedAttributes.Keys)
                {
                    string value = parsedAttributes[attribute];

                    if (!String.IsNullOrEmpty(value))
                    {
                        builder.CompileInMemory = Convert.ToBoolean(value);
                    }
                }
            }
            #endregion
            
        }

        /// <summary>
        /// This escapes a text literal and wraps it in double quotes.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        /// <param name="isEmpty">Is this string empty?</param>
        /// <returns>The escaped text wrapped in double quotes.</returns>
        public static string EscapeLiteral(string text, out bool isEmpty)
        {
            string escapedString = text;

            for (int i = 0; i < (Chars2Replace.Length / 2); i++) 
			{
				escapedString = escapedString.Replace(Chars2Replace[i, 0], Chars2Replace[i, 1]);
			}
            isEmpty = (escapedString.Length == 0);

			return "\"" + escapedString + "\"";
        }

        /// <summary>
        /// Builds a write command that writes to the ouput stream.
        /// </summary>
        /// <param name="text">The text to write to the output stream.</param>
        /// <returns>The write command text.</returns>
        public static string BuildWriteCommand(string text)
        {
            return "output.Append(" + text + ");" + Environment.NewLine;
		}

        /// <summary>
        /// Builds a writeline command that writes to the ouput stream and appends with a linefeed.
        /// </summary>
        /// <param name="text">The text to write to the output stream.</param>
        /// <returns>The writeline command text.</returns>
        public static string BuildWriteLineCommand(string text)
        {
            return "output.AppendLine(" + text + ");" + Environment.NewLine;
        }
    }
}
