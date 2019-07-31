using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using EntitySpaces.AddIn.TemplateUI;


namespace EntitySpaces.AddIn
{
    internal class TemplateUICollection
    {
        private List<esTemplateInfo> templateUserInterfaces = new List<esTemplateInfo>();
        private bool isLoaded = false;

        public TemplateUICollection()
        {

        }

        public void Clear()
        {
            this.templateUserInterfaces.Clear();
            isLoaded = false;
        }

        public bool IsLoaded
        {
            get
            {
                return this.isLoaded;
            }
        }

        public SortedList<int, esTemplateInfo> GetTemplateUI(Guid userInterfaceID)
        {
            SortedList<int, esTemplateInfo> list = new SortedList<int, esTemplateInfo>();

            foreach (esTemplateInfo info in templateUserInterfaces)
            {
                if (info.UserInterfaceId == userInterfaceID)
                {
                    list.Add(info.TabOrder, info);
                }
            }

            return list;
        }

        public void RegisterAssemblies(string path)
        {
            isLoaded = true;

            string[] assemblies = Directory.GetFiles(path, "*.dll");

            foreach (string assemblyName in assemblies)
            {
                Assembly assembly = Assembly.LoadFile(assemblyName);

                Type[] types = assembly.GetExportedTypes();

                foreach (Type type in types)
                {
                    if (type.IsSubclassOf(typeof(UserControl)))
                    {
                        UserControl userControl = Activator.CreateInstance(type) as UserControl;

                        ITemplateUI templateUI = userControl as ITemplateUI;

                        if (templateUI != null)
                        {
                            esTemplateInfo templateInfo = templateUI.Init();

                            if (templateInfo != null)
                            {
                                templateUserInterfaces.Add(templateInfo);
                            }
                        }
                    }
                }
            }
        }
    }
}
