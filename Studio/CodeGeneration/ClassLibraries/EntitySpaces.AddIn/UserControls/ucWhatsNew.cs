using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace EntitySpaces.AddIn
{
    internal partial class ucWhatsNew : esUserControl
    {
        bool showingNews = true;

        public ucWhatsNew()
        {
            InitializeComponent();
        }

        private void ucWhatsNew_Load(object sender, EventArgs e)
        {
            try
            {
                ScreenScrape();

                if (File.Exists(whatsNewFilePath))
                {
                    WhatsNewWebBrowser.Url = new Uri(whatsNewFilePath);
                }
            }
            catch { }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (WhatsNewWebBrowser.DocumentTitle.StartsWith("Navigation"))
            {
                WhatsNewWebBrowser.Visible = false;
            }
        }

        public void ScreenScrape()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri("http://www.developer.entityspaces.net/documentation/WhatsNew.aspx"), this.WhatsNewWebBrowser);
            }
        }

        private static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                if (e.Error == null)
                {
                    if (!string.IsNullOrEmpty(e.Result))
                    {
                        File.WriteAllText(whatsNewFilePath, e.Result.Trim());
                    }
                }

                if (File.Exists(whatsNewFilePath))
                {
                    WebBrowser browser = e.UserState as WebBrowser;
                    browser.Url = new Uri(whatsNewFilePath);
                }
            }
            catch { }
        }

        private static string whatsNewFilePath
        {
            get
            {
                return string.Concat(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "\\EntitySpaces\\ES2019\\WhatsNew.html");
            }
        }

        private void toolStripHome_Click(object sender, EventArgs e)
        {
            try
            {
                showingNews = false;

                if (!this.toolStripEmbed.Checked)
                    System.Diagnostics.Process.Start("http://www.entityspaces.net/");
                else
                    WhatsNewWebBrowser.Navigate("http://www.entityspaces.net/");
            }
            catch { }
        }

        private void toolStripDocumentation_Click(object sender, EventArgs e)
        {
            try
            {
                showingNews = false;

                if (!this.toolStripEmbed.Checked)
                    System.Diagnostics.Process.Start("http://developer.entityspaces.net/documentation/");
                else
                    WhatsNewWebBrowser.Navigate("http://developer.entityspaces.net/documentation/");
            }
            catch { }
        }

        private void toolStripSupport_Click(object sender, EventArgs e)
        {
            try
            {
                showingNews = false;

                if (!this.toolStripEmbed.Checked)
                    System.Diagnostics.Process.Start("http://www.entityspaces.net/portal/Forums/tabid/203/Default.aspx");
                else
                    WhatsNewWebBrowser.Navigate("http://www.entityspaces.net/portal/Forums/tabid/203/Default.aspx");
            }
            catch { }
        }

        private void toolStripBlog_Click(object sender, EventArgs e)
        {
            try
            {
                showingNews = false;

                if (!this.toolStripEmbed.Checked)
                    System.Diagnostics.Process.Start("http://www.entityspaces.net/blog/");
                else
                    WhatsNewWebBrowser.Navigate("http://www.entityspaces.net/blog/");
            }
            catch { }
        }

        private void toolStripTwitter_Click(object sender, EventArgs e)
        {
            try
            {
                showingNews = false;

                if (!this.toolStripEmbed.Checked)
                    System.Diagnostics.Process.Start("http://twitter.com/entityspaces/");
                else
                    WhatsNewWebBrowser.Navigate("http://twitter.com/entityspaces/");
            }
            catch { }
        }

        private void toolStripWhatsNew_Click(object sender, EventArgs e)
        {
            showingNews = true;

            WhatsNewWebBrowser.Navigate("http://www.developer.entityspaces.net/documentation/WhatsNew.aspx");
        }

        private void toolStripRefresh_Click(object sender, EventArgs e)
        {
            if (showingNews)
            {
                ScreenScrape();
            }
            else
            {
                WhatsNewWebBrowser.Refresh();
            }
        }
    }
}
