using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


using EntitySpaces.MetadataEngine;

namespace EntitySpaces.AddIn.TemplateUI
{
    public interface ITemplateUI
    {
        esTemplateInfo Init();
        UserControl CreateInstance(Root esMeta, bool cachedSettings, object applicationObject);

        bool OnExecute();
        void OnCancel();
    }
}
