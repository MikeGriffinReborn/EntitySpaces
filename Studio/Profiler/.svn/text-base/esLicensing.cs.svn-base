using System;
using System.IO;
using System.Text;
using System.Web;
using System.Net;
using System.Management;

using Microsoft.Win32;

namespace EntitySpaces.ProfilerApplication
{
    internal class Licensing
    {
        private Guid magicCookie = Guid.NewGuid();

        internal Licensing()
        {

        }


        internal int ValidateLicense(string serialNumber, string machineName, string uniqueHardwareIdentifier, string version)
        {
            int result = 0; // normal failure as a default

            try
            {
                StringBuilder post = new StringBuilder();

                Random seed = new Random();
                Random r = new Random(seed.Next());
                byte playback = (byte)r.Next(250);

                post.AppendFormat("SerialNumber={0}", Uri.EscapeDataString(serialNumber.ToString()));
                post.AppendFormat("&MachineName={0}", Uri.EscapeDataString(machineName.ToString()));
                post.AppendFormat("&UniqueHardwareIdentifier={0}", Uri.EscapeDataString(uniqueHardwareIdentifier.ToString()));
                post.AppendFormat("&Version={0}", Uri.EscapeDataString(version));
                post.AppendFormat("&Playback={0}", Uri.EscapeDataString(playback.ToString()));

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"https://api.entityspaces.net/VerifyActivation.ashx");
                //              HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"http://localhost:1444/VerifyActivation.ashx");


                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 8000;
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
            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
        }

        internal int RegisterLicense(string serialNumber, string machineName, string uniqueHardwareIdentifier, string version)
        {
            int result = 0; // normal failure as a default

            try
            {
                StringBuilder post = new StringBuilder();

                Random seed = new Random();
                Random r = new Random(seed.Next());
                byte playback = (byte)r.Next(250);

                post.AppendFormat("SerialNumber={0}", Uri.EscapeDataString(serialNumber.ToString()));
                post.AppendFormat("&MachineName={0}", Uri.EscapeDataString(machineName.ToString()));
                post.AppendFormat("&UniqueHardwareIdentifier={0}", Uri.EscapeDataString(uniqueHardwareIdentifier.ToString()));
                post.AppendFormat("&Version={0}", Uri.EscapeDataString(version));
                post.AppendFormat("&Playback={0}", Uri.EscapeDataString(playback.ToString()));

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"https://api.entityspaces.net/AddActivation.ashx");
                //              HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"http://localhost:1444/AddActivation.ashx");

                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 8000;
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
            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
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
}
