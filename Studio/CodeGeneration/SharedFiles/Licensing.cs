using System;
using System.IO;
using System.Text;
using System.Web;
using System.Net;
using System.Management;

using Microsoft.Win32;
using System.Xml.Serialization;
using System.Xml;

namespace EntitySpaces
{
    internal class Licensing
    {
        private int daysTheyCanRunOffline = 3; // default
        private Guid magicCookie = Guid.NewGuid();

        internal Licensing()
        {

        }

        internal int DaysTheyCanRunOffline
        {
            get
            {
                return daysTheyCanRunOffline;
            }
        }

        internal void CreateSerialNumber2Key(RegistryKey key, string keyName, string id, bool offline)
        {
            Crypto crypto = new Crypto();

            string offlineDate = offline ? DateTime.Now.ToString() : DateTime.MinValue.ToString();

            key.SetValue
                (keyName,
                    crypto.EncryptStringAES
                    (
                        Guid.NewGuid().ToString() + "|" +
                        DateTime.Now.ToString() + "|" +
                        Guid.NewGuid().ToString() + "|" +
                        id + "|" +
                        magicCookie.ToString() + "|" +
                        daysTheyCanRunOffline.ToString() + "|" +
                        offline.ToString() + "|" +
                        offlineDate,
                        id
                    )
                );
        }

        internal bool ReadSerialNumber2Key(RegistryKey key, string keyName, string id, out bool isOffline, out DateTime offLineDate)
        {
            isOffline = false;
            offLineDate = DateTime.MinValue;

            bool canRunOffline = false;

            string serialNumber2 = (string)key.GetValue(keyName);

            if (serialNumber2 != null)
            {
                Crypto crypto = new Crypto();
                string data = crypto.DecryptStringAES(serialNumber2, id);
                string[] datas = data.Split('|');

                if (datas[3] != id) throw new Exception("");

                magicCookie = new Guid(datas[4]);

                int daysOff = 3;
                if (Int32.TryParse(datas[5], out daysOff))
                {
                    daysTheyCanRunOffline = daysOff;
                }

                isOffline = bool.Parse(datas[6]);

                DateTime.TryParse(datas[7], out offLineDate);

                canRunOffline = true;
            }

            return canRunOffline;
        }

        internal DateTime CreateOfflineFile(string offlinePath)
        {
            DateTime dt = DateTime.MinValue;

            using (FileStream stream = File.Create(offlinePath))
            {
                dt = File.GetCreationTime(offlinePath);
            }

            using (FileStream stream = File.OpenWrite(offlinePath))
            {
                Crypto crypto = new Crypto();

                string data = crypto.EncryptStringAES
                    (
                      Guid.NewGuid().ToString() + "|" +
                      Guid.NewGuid().ToString() + "|" +
                      dt.ToString() + "|" +
                      Guid.NewGuid().ToString() + "|" +
                      Guid.NewGuid().ToString() + "|" +
                      magicCookie.ToString(),
                      getUniqueID("C")
                    );

                Byte[] info = new UTF8Encoding(true).GetBytes(data);

                // Add some information to the file.
                stream.Write(info, 0, info.Length);
            }

            return dt;
        }

        internal DateTime OpenOfflineFile(string offlinePath)
        {
            DateTime dt = DateTime.MaxValue;

            if (File.Exists(offlinePath))
            {
                byte[] b = File.ReadAllBytes(offlinePath);

                Crypto crypto = new Crypto();

                string data = new UTF8Encoding(true).GetString(b);

                data = crypto.DecryptStringAES(data, getUniqueID("C"));
                string[] datas = data.Split('|');

                DateTime.TryParse(datas[2], out dt);
                DateTime fileDate = File.GetCreationTime(offlinePath);

                if (dt.ToString() != fileDate.ToString())
                {
                    dt = DateTime.MaxValue;
                }

                if (dt > DateTime.Now)
                {
                    dt = DateTime.MaxValue;
                }

                try
                {
                    Guid fileMagicKey = new Guid(datas[5]);
                    if (fileMagicKey != magicCookie)
                    {
                        dt = DateTime.MaxValue;
                    }
                }
                catch { }
            }

            return dt;
        }

        internal int ValidateLicense(string product, string serialNumber, string machineName, string uniqueHardwareIdentifier, string version, ProxySettings settings)
        {
            int result = 0; // normal failure as a default

            try
            {
                StringBuilder post = new StringBuilder();

                Random seed = new Random();
                Random r = new Random(seed.Next());
                byte playback = (byte)r.Next(250);

                post.AppendFormat("Product={0}", Uri.EscapeDataString(product.ToString()));
                post.AppendFormat("&SerialNumber={0}", Uri.EscapeDataString(serialNumber.ToString()));
                post.AppendFormat("&MachineName={0}", Uri.EscapeDataString(machineName.ToString()));
                post.AppendFormat("&UniqueHardwareIdentifier={0}", Uri.EscapeDataString(uniqueHardwareIdentifier.ToString()));
                post.AppendFormat("&Version={0}", Uri.EscapeDataString(version));
                post.AppendFormat("&Playback={0}", Uri.EscapeDataString(playback.ToString()));

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"https://api.entityspaces.net/VerifyActivation2.ashx");
             // HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"http://localhost/Licensing/VerifyActivation2.ashx");

                if (settings.UseProxy)
                {
                    if (!string.IsNullOrEmpty(settings.UserName))
                    {
                        NetworkCredential creds = new NetworkCredential(settings.UserName, settings.Password);
                        if (!string.IsNullOrEmpty(settings.DomainName))
                        {
                            creds.Domain = settings.DomainName;
                        }
                        req.Proxy = new WebProxy(settings.Url, true, null, creds);
                    }
                    else
                    {
                        req.Proxy = new WebProxy(settings.Url, true, null);
                    }
                }

                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 10000;
                byte[] data = System.Text.Encoding.UTF8.GetBytes(post.ToString());
                req.ContentLength = data.Length;

                string responseFromServer = "";

                using (Stream dataStream = req.GetRequestStream())
                {
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();

                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                responseFromServer = reader.ReadToEnd();
                            }
                        }
                    }
                }

                byte[] responseData = Convert.FromBase64String(responseFromServer);
                byte offset = responseData[600];
                byte valid = responseData[offset];

                if (valid % 2 == 0)
                {
                    result = 1;
                }

                if (playback != responseData[offset + 2]) result = 0;

                if (result == 1)
                {
                    daysTheyCanRunOffline = responseData[offset + 1];
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }

        internal int RegisterLicense(string product, string serialNumber, string machineName, string uniqueHardwareIdentifier, string version, ProxySettings settings)
        {
            int result = 0; // normal failure as a default

            try
            {
                StringBuilder post = new StringBuilder();

                Random seed = new Random();
                Random r = new Random(seed.Next());
                byte playback = (byte)r.Next(250);

                post.AppendFormat("Product={0}", Uri.EscapeDataString(product.ToString()));
                post.AppendFormat("&SerialNumber={0}", Uri.EscapeDataString(serialNumber.ToString()));
                post.AppendFormat("&MachineName={0}", Uri.EscapeDataString(machineName.ToString()));
                post.AppendFormat("&UniqueHardwareIdentifier={0}", Uri.EscapeDataString(uniqueHardwareIdentifier.ToString()));
                post.AppendFormat("&Version={0}", Uri.EscapeDataString(version));
                post.AppendFormat("&Playback={0}", Uri.EscapeDataString(playback.ToString()));

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"https://api.entityspaces.net/AddActivation2.ashx");
             // HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"http://localhost/Licensing/AddActivation2.ashx");

                if (settings.UseProxy)
                {
                    if (!string.IsNullOrEmpty(settings.UserName))
                    {
                        NetworkCredential creds = new NetworkCredential(settings.UserName, settings.Password);
                        if (!string.IsNullOrEmpty(settings.DomainName))
                        {
                            creds.Domain = settings.DomainName;
                        }
                        req.Proxy = new WebProxy(settings.Url, true, null, creds);
                    }
                    else
                    {
                        req.Proxy = new WebProxy(settings.Url, true, null);
                    }
                }

                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 10000;
                byte[] data = System.Text.Encoding.UTF8.GetBytes(post.ToString());
                req.ContentLength = data.Length;

                string responseFromServer = "";

                using (Stream dataStream = req.GetRequestStream())
                {
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();

                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                responseFromServer = reader.ReadToEnd();
                            }
                        }
                    }
                }

                byte[] responseData = Convert.FromBase64String(responseFromServer);
                byte offset = responseData[600];
                byte valid = responseData[offset];

                if (valid % 2 == 0)
                {
                    result = 1;
                }

                if (playback != responseData[offset + 2]) result = 0;

                if (result == 1)
                {
                    daysTheyCanRunOffline = responseData[offset + 1];
                }
            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
        }

        internal void ReplaceMeLater(string product, string esVersion, string s1, string s2, string offlineFile, ProxySettings settings)
        {
            bool passed = false;

            try
            {
                Crypto crypto = new Crypto();

                string id = this.getUniqueID("C");
                string serialNumber = "";
                bool canRunOffline = false;
                bool isAllSecurityOkay = true;


                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2019", true);
                if (key != null)
                {
                    try
                    {
                        serialNumber = (string)key.GetValue(s1);
                    }
                    catch { }
                }

                string offlinePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                offlinePath += @"\EntitySpaces\ES2019\" + offlineFile; //Interop.ADODBX.dll";

                // See if we have registered our license
                int result = this.ValidateLicense(product, serialNumber, System.Environment.MachineName, id, esVersion, settings);

                switch (result)
                {
                    case 0:

                        // Try Registering it ...
                        int newResult = this.RegisterLicense(product, serialNumber, System.Environment.MachineName, id, esVersion, settings);

                        if (newResult == 1)
                        {
                            this.CreateSerialNumber2Key(key, s2, id, false);
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }
                        break;

                    case 1:

                        this.CreateSerialNumber2Key(key, s2, id, false);
                        try
                        {
                            File.Delete(offlinePath);
                        }
                        catch { }

                        break;

                    case -1:

                        bool isOffLine = false;
                        DateTime offLineDate = DateTime.MinValue;
                        if (this.ReadSerialNumber2Key(key, s2, id, out isOffLine, out offLineDate))
                        {
                            if (isOffLine)
                            {
                                if (File.Exists(offlinePath))
                                {
                                    DateTime fileDate = this.OpenOfflineFile(offlinePath);

                                    if (DateTime.Now > offLineDate)
                                    {
                                        TimeSpan ts = DateTime.Now.Subtract(offLineDate);
                                        if (ts.Days < this.DaysTheyCanRunOffline)
                                        {
                                            if (fileDate < DateTime.Now)
                                            {
                                                canRunOffline = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                this.CreateSerialNumber2Key(key, s2, id, true);
                                this.CreateOfflineFile(offlinePath);
                                canRunOffline = true;
                            }
                        }
                        break;
                }

                if (isAllSecurityOkay && result == 1 || (canRunOffline && result == -1))
                {
                    passed = true;
                }
            }
            catch 
            {
                throw new Exception("License Invalid or You Must Connect");
            }

            if (!passed)
            {
                throw new Exception("License Invalid or You Must Connect");
            }
        }

        internal string getUniqueID(string drive)
        {
            if (drive == string.Empty)
            {
                //Find first drive
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }

            if (drive.EndsWith(":\\"))
            {
                drive = drive.Substring(0, drive.Length - 2);
            }

            string volumeSerial = getVolumeSerial(drive);
            string cpuID = getCPUID();

            //Mix them up and remove some useless 0's
            return cpuID.Substring(13) + cpuID.Substring(1, 4) + volumeSerial + cpuID.Substring(4, 4);
        }

        private string getVolumeSerial(string drive)
        {
            ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }

        private string getCPUID()
        {
            string cpuInfo = "";
            ManagementClass managClass = new ManagementClass("win32_processor");
            ManagementObjectCollection managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = managObj.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            return cpuInfo;
        }
    }

    [Serializable]
    public class ProxySettings
    {
        public bool UseProxy;
        public string Url;
        public string UserName;
        public string Password;
        public string DomainName;

        public void Save()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path += @"\EntitySpaces\ES2019\esProfileSettings.xml";

            string xml = String.Empty;

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlSerializer serializer = new XmlSerializer(typeof(ProxySettings));
                serializer.Serialize(stream, this, ns);

                xml = ASCIIEncoding.UTF8.GetString(stream.ToArray());
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlAttribute attr = doc.CreateAttribute("Version");
            attr.Value = "2019.1.0725.0";

            doc.DocumentElement.Attributes.Append(attr);
            doc.Save(path);
        }

        public void Load()
        {
            ProxySettings settings = null;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path += @"\EntitySpaces\ES2019\esProfileSettings.xml";

            try
            {
                using (TextReader rdr = new StreamReader(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ProxySettings));
                    settings = (ProxySettings)serializer.Deserialize(rdr);
                    rdr.Close();
                }
            }
            catch { }

            if (settings != null)
            {
                this.UseProxy = settings.UseProxy;
                this.Url = settings.Url;
                this.UserName = settings.UserName;
                this.Password = settings.Password;
                this.DomainName = settings.DomainName;
            }
        }
    }
}