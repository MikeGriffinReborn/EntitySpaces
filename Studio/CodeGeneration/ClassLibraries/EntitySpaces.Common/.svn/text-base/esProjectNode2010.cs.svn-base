using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using EntitySpaces.MetadataEngine;
using EntitySpaces.CodeGenerator;

namespace EntitySpaces.Common
{
    public class esProjectNode2010 : IProjectNode
    {
        private bool isFolder = true;

        public bool IsFolder
        {
            get { return isFolder; }
            set { isFolder = value; }
        }
        public string Name { get; set; }
        public Template Template { get; set; }
        public Hashtable Input { get; set; }
        public ISettings Settings { get; set; }

        private List<IProjectNode> children;
        public List<IProjectNode> Children
        {
            get
            {
                if (children == null)
                {
                    children = new List<IProjectNode>();
                }

                return children;
            }

            set
            {
                children = value;
            }
        }
    }
}
