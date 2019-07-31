using System;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace AddInInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", false);
                if (key != null)
                {
                    string basePath = (string)key.GetValue("Install_Dir");

                    if (!basePath.EndsWith(@"\"))
                    {
                        basePath += @"\";
                    }

                    string source = basePath + @"CodeGeneration\Bin\EntitySpaces2019.AddIn";

                    string dest = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                    string text = "";

                    using (TextReader reader = new StreamReader(source))
                    {
                        text = reader.ReadToEnd();
                    }

                    text = text.Replace("[PATH]", basePath);

                    string dir = dest + @"\Microsoft\MSEnvShared\AddIns\";

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    dest += @"\Microsoft\MSEnvShared\AddIns\EntitySpaces2019.AddIn";
                    using (StreamWriter writer = new StreamWriter(dest, false, Encoding.BigEndianUnicode))
                    {
                        writer.Write(text);
                    }

                    using (StreamWriter writer = new StreamWriter(source, false, Encoding.BigEndianUnicode))
                    {
                        writer.Write(text);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
    }
}
