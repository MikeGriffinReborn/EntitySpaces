using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;

using TheCodeKing.Net.Messaging;

using gudusoft.gsqlparser;
using gudusoft.gsqlparser.Units;

using EntitySpaces;
using EntitySpaces.ProfilerApplication;
using Microsoft.Win32;
using System.IO;


namespace EntitySpaces.ProfilerApplication
{
    public partial class ProfilerWindow : RibbonForm
    {
        private TGSqlParser parser = new TGSqlParser(TDbVendor.DbVMssql);
        private Licensing licensing = new Licensing();
        private ProxySettings proxySettings = new ProxySettings() { UseProxy = false };
        private string version = "2012.1.0319.0";

#if PROFILER_TRIAL
        long mod = 0;
#endif

        public ProfilerWindow()
        {
            InitializeComponent();

            InitSkinGallery();

            bool canRunOffline = false;
            int result;

            if (!DesignMode)
            {
                proxySettings.Load();

#if !PROFILER_TRIAL

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EntitySpaces 2012", true);

                Crypto crypto = new Crypto();

                string id = licensing.getUniqueID("C");
                string serialNumber = ""; // b69e3783-9f56-47a7-82e0-6eee6d0779bf

                if (key != null)
                {
                    try
                    {
                        serialNumber = (string)key.GetValue("Profiler_Number");
                    }
                    catch { }
                }


                string offlinePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                offlinePath += @"\EntitySpaces\ES2012\Interop.ADODB64X.dll";

                // See if we have registered our license
                result = licensing.ValidateLicense("profiler", serialNumber, System.Environment.MachineName, id, version, proxySettings);

                switch (result)
                {
                    case 0:

                        // Try Registering it ...
                        int newResult = licensing.RegisterLicense("profiler", serialNumber, System.Environment.MachineName, id, version, proxySettings);

                        if (newResult == 1)
                        {
                            licensing.CreateSerialNumber2Key(key, "Profiler_Number2", id, false);
                            result = 1;
                        }
                        else 
                        {
                            result = 0;
                        }
                        break;

                    case 1:

                        licensing.CreateSerialNumber2Key(key, "Profiler_Number2", id, false);
                        try
                        {
                            File.Delete(offlinePath);
                        }
                        catch { }

                        break;

                    case -1:

                        bool isOffLine = false;
                        DateTime offLineDate = DateTime.MinValue;
                        if (licensing.ReadSerialNumber2Key(key, "Profiler_Number2", id, out isOffLine, out offLineDate))
                        {
                            if (isOffLine)
                            {
                                if (File.Exists(offlinePath))
                                {
                                    DateTime fileDate = licensing.OpenOfflineFile(offlinePath);

                                    if (DateTime.Now > offLineDate)
                                    {
                                        TimeSpan ts = DateTime.Now.Subtract(offLineDate);
                                        if (ts.Days < licensing.DaysTheyCanRunOffline)
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
                                licensing.CreateSerialNumber2Key(key, "Profiler_Number2", id, true);
                                licensing.CreateOfflineFile(offlinePath);
                                canRunOffline = true;
                            }
                        }
                        break;
                }
#else
                Licensing license = new Licensing();
                string id = license.getUniqueID("C");

                result = licensing.ValidateLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, version, proxySettings);

                if (1 != result)
                {
                    result = licensing.RegisterLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, version, proxySettings);
                }
#endif

                if (result == 1 || (canRunOffline && result == -1))
                {
                    // creates an instance of the IXDListener object using the given implementation  
                    listener = XDListener.CreateListener(XDTransportMode.IOStream);
                    listener.MessageReceived += new XDListener.XDMessageHandler(OnMessageReceived);

                    listener.RegisterChannel(Channels.Channel_1);
                    listener.RegisterChannel(Channels.Channel_2);
                    listener.RegisterChannel(Channels.Channel_3);
                    listener.RegisterChannel(Channels.Channel_4);
                    listener.RegisterChannel(Channels.Channel_5);
                    listener.RegisterChannel(Channels.Channel_6);
                    listener.RegisterChannel(Channels.Channel_7);
                    listener.RegisterChannel(Channels.Channel_8);
                    listener.RegisterChannel(Channels.Channel_9);
                    listener.RegisterChannel(Channels.Channel_10);

                    navChannel1.Tag = channel_1;
                    navChannel2.Tag = channel_2;
                    navChannel3.Tag = channel_3;
                    navChannel4.Tag = channel_4;
                    navChannel5.Tag = channel_5;
                    navChannel6.Tag = channel_6;
                    navChannel7.Tag = channel_7;
                    navChannel8.Tag = channel_8;
                    navChannel9.Tag = channel_9;
                    navChannel10.Tag = channel_10;

                    channel_1.NavBarItem = navChannel1;
                    channel_2.NavBarItem = navChannel2;
                    channel_3.NavBarItem = navChannel3;
                    channel_4.NavBarItem = navChannel4;
                    channel_5.NavBarItem = navChannel5;
                    channel_6.NavBarItem = navChannel6;
                    channel_7.NavBarItem = navChannel7;
                    channel_8.NavBarItem = navChannel8;
                    channel_9.NavBarItem = navChannel9;
                    channel_10.NavBarItem = navChannel10;

                    currentChannel = channel_1;
                    this.Text = "EntitySpaces Profiler - " + currentChannel.Name;
                    esTracePacketBindingSource.DataSource = currentChannel.ChannelData;

                    clock = new System.Threading.Timer(TimerCallback, null, 2000, 1500);

                    //define color of user customized token
                    lzbasetype.gFmtOpt.HighlightingElements[(int)TLzHighlightingElement.sfkUserCustomized].SetForegroundInRGB("#FF00FF");

                    foreach (var obj in lzbasetype.gFmtOpt.HighlightingElements)
                    {
                        if (obj.Foreground.ToString().ToLower() == "12632256")
                        {
                            obj.SetForegroundInRGB("#000000");
                        }
                    }
                    lzbasetype.gFmtOpt.AlignAliasInSelectList = false;
                }


                //this.gridView1.BestFitColumns();
            }
        }

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        /// <summary>
        /// The instance used to listen to broadcast messages.
        /// </summary>
        private IXDListener listener;
        private System.Threading.Timer clock;
 
        static private string lf = System.Environment.NewLine;
        static private List<Type> knownTypes = new List<Type>() { typeof(esParameters), typeof(esParameter) };

        private class Channels
        {
            public const string Channel_1 = "Channel_1";
            public const string Channel_2 = "Channel_2";
            public const string Channel_3 = "Channel_3";
            public const string Channel_4 = "Channel_4";
            public const string Channel_5 = "Channel_5";
            public const string Channel_6 = "Channel_6";
            public const string Channel_7 = "Channel_7";
            public const string Channel_8 = "Channel_8";
            public const string Channel_9 = "Channel_9";
            public const string Channel_10 = "Channel_10";
        }

        private class Channel
        {
            public string Name { get; set; }
            public DevExpress.XtraNavBar.NavBarItem NavBarItem { get; set; }
            public object TheLock = new object();
            public List<esTracePacket> IncomingPackets = new List<esTracePacket>();
            public SortableBindingList<esTracePacket> ChannelData = new SortableBindingList<esTracePacket>();
        }

        private Channel channel_1 = new Channel() { Name = Channels.Channel_1 };
        private Channel channel_2 = new Channel() { Name = Channels.Channel_2 };
        private Channel channel_3 = new Channel() { Name = Channels.Channel_3 };
        private Channel channel_4 = new Channel() { Name = Channels.Channel_4 };
        private Channel channel_5 = new Channel() { Name = Channels.Channel_5 };
        private Channel channel_6 = new Channel() { Name = Channels.Channel_6 };
        private Channel channel_7 = new Channel() { Name = Channels.Channel_7 };
        private Channel channel_8 = new Channel() { Name = Channels.Channel_8 };
        private Channel channel_9 = new Channel() { Name = Channels.Channel_9 };
        private Channel channel_10 = new Channel() { Name = Channels.Channel_10 };

        private Channel currentChannel;


        private void OnMessageReceived(object sender, XDMessageEventArgs e)
        {
#if PROFILER_TRIAL

            if (++mod % 1000 == 0)
            {
                Licensing license = new Licensing();
                string id = license.getUniqueID("C");

                int result = licensing.ValidateLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, version, proxySettings);

                if (1 != result)
                {
                    result = licensing.RegisterLicense("trial", "b69e3783-9f56-47a7-82e0-6eee6d0779bf", System.Environment.MachineName, id, version, proxySettings);
                }

                if (result != 1)
                {
                    listener.UnRegisterChannel(Channels.Channel_1);
                    listener.UnRegisterChannel(Channels.Channel_2);
                    listener.UnRegisterChannel(Channels.Channel_3);
                    listener.UnRegisterChannel(Channels.Channel_4);
                    listener.UnRegisterChannel(Channels.Channel_5);
                    listener.UnRegisterChannel(Channels.Channel_6);
                    listener.UnRegisterChannel(Channels.Channel_7);
                    listener.UnRegisterChannel(Channels.Channel_8);
                    listener.UnRegisterChannel(Channels.Channel_9);
                    listener.UnRegisterChannel(Channels.Channel_10);
                    return;
                } 
            }

#endif

            try
            {
                esTracePacket entry = new esTracePacket();

                string[] fields = e.DataGram.Message.Split('±');

                if (fields[0].Length > 0)
                {
                    entry.TransactionId = Convert.ToInt32(fields[0]);
                }

                if (fields[1].Length > 0)
                {
                    entry.ObjectType = fields[1];
                }

                entry.CallStack = fields[2];
                entry.ApplicationName = fields[3];
                entry.TraceChannel = fields[4];
                entry.ThreadId = Convert.ToInt32(fields[5]);
                entry.Sql = fields[6];
                entry.Duration = Convert.ToInt64(fields[7]);
                entry.Ticks = Convert.ToInt64(fields[8]);
                entry.PacketOrder = Convert.ToInt64(fields[9]);
                entry.Action = fields[10];
                entry.Syntax = fields[11];
                entry.Exception = fields[12];

                if (fields[13].Length > 0)
                {
                    // Parse Parameters
                    string[] parameters = fields[13].Split('«');

                    esParameter param = null;
                    for (int i = 0; i < parameters.Length; i += 5)
                    {
                        if (param == null) param = new esParameter();

                        param.Name = parameters[i];
                        param.Direction = parameters[i + 1];
                        param.ParamType = parameters[i + 2];

                        if (parameters[i + 3] != "null")
                        {
                            param.BeforeValue = parameters[i + 3];
                        }

                        if (parameters[i + 4] != "null")
                        {
                            param.AfterValue = parameters[i + 4];
                        }

                        entry.SqlParameters.Add(param);
                        param = null;
                    }
                }

                switch (entry.TraceChannel)
                {
                    case Channels.Channel_1: AddEntryToList(channel_1, entry); break;
                    case Channels.Channel_2: AddEntryToList(channel_2, entry); break;
                    case Channels.Channel_3: AddEntryToList(channel_3, entry); break;
                    case Channels.Channel_4: AddEntryToList(channel_4, entry); break;
                    case Channels.Channel_5: AddEntryToList(channel_5, entry); break;
                    case Channels.Channel_6: AddEntryToList(channel_6, entry); break;
                    case Channels.Channel_7: AddEntryToList(channel_7, entry); break;
                    case Channels.Channel_8: AddEntryToList(channel_8, entry); break;
                    case Channels.Channel_9: AddEntryToList(channel_9, entry); break;
                    case Channels.Channel_10: AddEntryToList(channel_10, entry); break;
                }
            }
            catch { }
        }

        private void SetParserSyntax(string syntax)
        {
            try
            {
                switch (syntax)
                {
                    case "MSSQL":
                    case "MSSQLCE":
                    case "VISTADB":

                        if (parser.DbVendor != TDbVendor.DbVMssql)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVMssql);
                        }
                        break;

                    case "POSTGRESQL":
                    case "ORACLE":

                        if (parser.DbVendor != TDbVendor.DbVOracle)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVOracle);
                        }
                        break;

                    case "MYSQL":

                        if (parser.DbVendor != TDbVendor.DbVMysql)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVMysql);
                        }
                        break;

                    case "SYBASE":

                        if (parser.DbVendor != TDbVendor.DbVSybase)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVSybase);
                        }
                        break;

                    case "ACCESS":

                        if (parser.DbVendor != TDbVendor.DbVAccess)
                        {
                            parser = new TGSqlParser(TDbVendor.DbVAccess);
                        }
                        break;

                    default:
                        parser = new TGSqlParser(TDbVendor.DbVGeneric);
                        break;
                }
            }
            catch { }
        }

        private void AddEntryToList(Channel channel, esTracePacket entry)
        {
            if (!IsDisposed)
            {
                lock (channel.TheLock)
                {
                    channel.IncomingPackets.Add(entry);

                    if (channel.IncomingPackets.Count == 1)
                    {
                        channel.NavBarItem.SmallImageIndex = 7;
                    }

                    if (channel.IncomingPackets.Count == 10)
                    {
                        // If called from a seperate thread, rejoin so that be can update form elements.
                        if (InvokeRequired && !IsDisposed)
                        {
                            try
                            {
                                // onClosing messages may fail if the form is being disposed.
                                Invoke((MethodInvoker)delegate() { AddEntriesToList(channel); });
                            }
                            catch (ObjectDisposedException) { }
                        }
                        else
                        {
                            AddEntriesToList(channel);
                        }
                    }
                }
            }
        }

        private void AddEntriesToList(Channel channel)
        {
            if (!IsDisposed)
            {
                //lock (channel.TheLock)
                {
                    foreach (esTracePacket packet in channel.IncomingPackets)
                    {
                        channel.ChannelData.Add(packet);
                    }
                    channel.IncomingPackets = new List<esTracePacket>();

                    siStatus.Caption = gridViewPackets.RowCount.ToString();
                }
            }
        }

        public void TimerCallback(Object state)
        {
            FlushIncomingPackets(currentChannel);
        }

        private void FlushIncomingPackets(Channel channel)
        {
            if (!IsDisposed)
            {
                lock (channel.TheLock)
                {
                    if (channel.IncomingPackets.Count > 0)
                    {
                        // If called from a seperate thread, rejoin so that be can update form elements.
                        if (InvokeRequired && !IsDisposed)
                        {
                            try
                            {
                                // onClosing messages may fail if the form is being disposed.
                                Invoke((MethodInvoker)delegate() { AddEntriesToList(channel); });
                            }
                            catch (ObjectDisposedException) { }
                        }
                    }
                }
            }
        }

        private void navChannel_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Channel channel = e.Link.Item.Tag as Channel;

            if (channel != null)
            {
                lock (channel.TheLock)
                {
                    currentChannel = channel;
                    esTracePacketBindingSource.DataSource = currentChannel.ChannelData;

                    if (currentChannel.ChannelData.Count == 0)
                    {
                        txtCallStack.Text = string.Empty;
                        esParametersBindingSource.DataSource = null;
                        txtSQL.Text = string.Empty;
                        txtException.Text = string.Empty;
                    }

                    this.Text = "EntitySpaces Profiler - " + channel.Name;
                    siStatus.Caption = gridViewPackets.RowCount.ToString();
                }
            }

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridViewPackets.IsGroupRow(e.FocusedRowHandle))
                {
                    txtCallStack.Text = string.Empty;
                    esParametersBindingSource.DataSource = null;
                    txtSQL.Text = string.Empty;
                    txtException.Text = string.Empty;
                    return;
                }

                int index = gridViewPackets.ViewRowHandleToDataSourceIndex(e.FocusedRowHandle);
                esTracePacket packet = currentChannel.ChannelData[index];

                txtSQL.Text = packet.Sql;

                SetParserSyntax(packet.Syntax);
                parser.SqlText.Text = packet.Sql;
                txtSQL.Rtf = parser.ToRTF(TOutputFmt.ofrtf);

                txtException.Text = packet.Exception;
                txtCallStack.Text = packet.CallStack;

                if (packet.SqlParameters != null)
                {
                    esParametersBindingSource.DataSource = packet.SqlParameters;
                }
            }
            catch { }
        }


        private void iFind_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.gridViewPackets.ShowFindPanel();
        }

        private void iClear_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.currentChannel.ChannelData.Clear();
            this.currentChannel.NavBarItem.SmallImageIndex = 8;
            siStatus.Caption = gridViewPackets.RowCount.ToString();

            txtCallStack.Text = string.Empty;
            esParametersBindingSource.DataSource = null;
            txtSQL.Text = string.Empty;
            txtException.Text = string.Empty;
        }

        private void iAbout_ItemClick(object sender, ItemClickEventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void iTack_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.TopMost = !this.TopMost;
        }

        private void iExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void iLicense_ItemClick(object sender, ItemClickEventArgs e)
        {
            LicensingForm form = new LicensingForm();
            form.proxySettings = this.proxySettings;
            if (DialogResult.OK == form.ShowDialog())
            {

            }
        }
    }
}