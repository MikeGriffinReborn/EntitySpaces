using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntitySpaces.MetadataEngine;
using EntitySpaces.CodeGenerator;
using EntitySpaces.Common;

namespace EntitySpaces.CommandLine
{
    class Program
    {
        static int Main(string[] args)
        {
#if !DEBUG
            SecurityTest securityTest = new SecurityTest();
            if (!securityTest.IsAllSecurityOkay)
            {
                while (true) { }

                object o = null;
                o.ToString();
            }
#endif

#if !DEBUG
            bool isAllSecurityOkay = securityTest.IsAllSecurityOkay;
            if (isAllSecurityOkay)
            {
                isAllSecurityOkay = securityTest.IsPublicTokenOkay;
            }

            if (!isAllSecurityOkay) return -1;
#endif

            List<Exception> errors = new List<Exception>();

            bool silentMode = false;

            try
            {
                if (args == null || args.Length == 0 || args[0] == "/?")
                {
                    Console.WriteLine();
                    Console.WriteLine("Usage: EntitySpaces.CommandLine {project file} {project node} /S");
                    Console.WriteLine();
                    Console.ReadKey();
                    return 0;
                }

                string projectName = null;
                string projectNode = null;

                #region Parse Arguments

                // I feel lazy, so I'm doing this the poor mans way
                if (args.Length == 1) projectName = args[0];

                if (args.Length == 2)
                {
                    projectName = args[0];

                    if (args[1] == "/s" || args[1] == "/S")
                    {
                        silentMode = true;
                    }
                    else
                    {
                        projectNode = args[1];
                    }
                }

                if (args.Length == 3)
                {
                    projectName = args[0];
                    projectNode = args[1];

                    if (args[2] == "/s" || args[2] == "/S")
                    {
                        silentMode = true;
                    }
                }

                #endregion Arguments

                if (projectName != null)
                {
                    esSettings settings = esSettings.Load();

                    Template.SetTemplateCachePath(esSettings.TemplateCachePath);
                    Template.SetCompilerAssemblyPath(settings.CompilerAssemblyPath);

                    ProjectExecuter exe = new ProjectExecuter(projectName, settings);
                    List<Exception> moreErrors = exe.ExecuteFromNode(projectNode);

                    if (moreErrors.Count > 0)
                    {
                        errors.AddRange(moreErrors);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            if (!silentMode && errors.Count > 0)
            {
                Console.WriteLine();

                foreach (Exception ex in errors)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }

                Console.ReadKey();
            }

            return errors.Count == 0 ? 0 : 1;
        }
    }
}
