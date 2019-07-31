using System;
using System.Collections.Generic;
using System.Text;

using System.EnterpriseServices.Internal;

namespace esGacInstall
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if(args.Length == 0) return;

                Publish p = new Publish();

                if(args[0] == "/install")
                {
                    p.GacInstall("EntitySpaces.Common.dll");
                    p.GacInstall("EntitySpaces.MetadataEngine.dll");
                    p.GacInstall("EntitySpaces2019.AddIn.dll");
                    p.GacInstall("EntitySpaces.AddIn.TemplateUI.dll");
                    p.GacInstall("EntitySpaces.CodeGenerator.dll");
                }
                else if(args[0] == "/remove")
                {
                    p.GacRemove("EntitySpaces.MetadataEngine.dll");
                    p.GacRemove("EntitySpaces2019.AddIn.dll");
                    p.GacRemove("EntitySpaces.AddIn.TemplateUI.dll");
                    p.GacRemove("EntitySpaces.CodeGenerator.dll");
                    p.GacRemove("EntitySpaces.TemplateUI.dll");
                    p.GacRemove("EntitySpaces.MSDASC.dll");
                    p.GacRemove("EntitySpaces.Common.dll");
                }
            }
            catch { }
        }
    }
}
