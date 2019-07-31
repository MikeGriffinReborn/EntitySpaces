using System;
using System.IO;
using System.Text;
using System.Threading;

using EntitySpaces.Common;
using EntitySpaces.MetadataEngine;

namespace EntitySpaces.AddIn
{
    internal class esMetaCreator
    {
        static internal Root Create(esSettings settings)
        {
            Root esMeta = new Root(settings);
            if (!esMeta.Connect(settings.Driver, settings.ConnectionString))
            {
                throw new Exception("Unable to Connect to Database");
            }
            esMeta.Language = "C#";

            return esMeta;
        }
    }
}
