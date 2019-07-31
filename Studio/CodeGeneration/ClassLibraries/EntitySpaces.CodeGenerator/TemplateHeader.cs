using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitySpaces.CodeGenerator
{
    public class TemplateHeader
    {
        public TemplateHeader()
        {

        }

        public string ToComment()
        {
            StringBuilder sb = new StringBuilder(300);

            sb.AppendLine(@"// Title:        " + this.Title);
            sb.AppendLine(@"// Description:  " + this.Description);
            sb.AppendLine(@"// Author:       " + this.Author);
            sb.AppendLine(@"// Version:      " + this.Version);
            sb.AppendLine(@"// Namespace:    " + this.Namespace);
            sb.AppendLine(@"// FullFileName: " + this.FullFileName);
            sb.AppendLine(@"// Compile Date: " + DateTime.Now.ToString());
            sb.AppendLine("");

            return sb.ToString();
        }

        /// <summary>
        /// The UniqueID of the current template.
        /// </summary>
        public Guid UniqueID = Guid.Empty;

        /// <summary>
        /// The Guid to match to the UI AddIn
        /// </summary>
        public Guid UserInterfaceID = Guid.Empty;

        /// <summary>
        /// The UniqueID of the current template.
        /// </summary>
        public string Namespace = String.Empty;

        /// <summary>
        /// The Author of the current template.
        /// </summary>
        public string Author = String.Empty;

        /// <summary>
        /// The Title of the current template.
        /// </summary>
        public string Title = String.Empty;

        /// <summary>
        /// The Description of the current template.
        /// </summary>
        public string Description = String.Empty;

        /// <summary>
        /// The file path of the current template.
        /// </summary>
        public string FilePath = String.Empty;

        /// <summary>
        /// The file name of the current template
        /// </summary>
        public string FileName = String.Empty;

        /// <summary>
        /// The file name and path of the current template
        /// </summary>
        public string FullFileName = String.Empty;

        /// <summary>
        /// The Version of the current template
        /// </summary>
        public string Version = String.Empty;

        /// <summary>
        /// Whether or not the template requires a UI to be excuted
        /// </summary>
        public bool RequiresUI = false;

        /// <summary>
        /// Whether or not the template is a sub template
        /// </summary>
        public bool IsSubTemplate = false;
    }
}
