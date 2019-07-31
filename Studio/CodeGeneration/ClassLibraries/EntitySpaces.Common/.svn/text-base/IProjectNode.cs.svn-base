using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using EntitySpaces.MetadataEngine;
using EntitySpaces.CodeGenerator;

namespace EntitySpaces.Common
{
    public interface IProjectNode
    {
        bool IsFolder {get; set;}
        string Name { get; set; }

        Template Template { get; set; }
        Hashtable Input { get; set; }
        ISettings Settings { get; set; }

        List<IProjectNode> Children { get; set; }
    }
}
