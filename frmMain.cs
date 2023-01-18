using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpPcap;
using PacketDotNet;
using SharpPcap.LibPcap;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;

namespace Toratan
{
    public partial class frmMain : Form
    {
        Label lblEmpty = new Label();

        #region Public Data For Report

        public string pcapName = "";
        public int packetsCount = 0;
        public Dictionary<string, int> destinationIPs = new Dictionary<string, int>();
        public List<string> allHttpUrls = new List<string>(), allDnsUrls = new List<string>();
        public List<string> allProtocols = new List<string>();

        #endregion



        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            emptyTable();

            if (!File.Exists("log.txt"))
            {
                File.Create("log.txt").Dispose();
            }
        }

        #region Capture Packets
        private void btn_SelectPcap_Click(object sender, EventArgs e)
        {
            OpenFileDialog pcap = new OpenFileDialog();

            pcap.Title = "Select pcap file";
            pcap.Filter = "PCap Files | *.pcap";
            pcap.ShowDialog();

            if (pcap.FileName != "")
            {
                txt_PCapSelect.Text = pcap.FileName;
            }
        }
        sbyte cpackMode = 0; // 0: Off | 1: On
        private void btn_CapturePackets_Click(object sender, EventArgs e)
        {
            if (ctx_PCapFile.Text.Trim() == "Not Loaded")
            {
                MessageBox.Show("Please select .pcap file to capture", "File Not Exsist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cpackMode == 0)
            {
                cpackMode = 1;
                btn_CapturePackets.Text = "Cancel";
                dgv_PacketsList.Rows.Clear();

                bgw_ReadPackets = new BackgroundWorker();
                bgw_ReadPackets.WorkerSupportsCancellation = true;
                bgw_ReadPackets.DoWork += new DoWorkEventHandler(bgw_ReadPackets_DoWork);
                bgw_ReadPackets.RunWorkerAsync();
            }
            else
            {
                if (MessageBox.Show("Are you sure?", "Cancel Capture Proccess", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cpackMode = 0;
                    btn_CapturePackets.Text = "Capture Packets";
                    bgw_ReadPackets.CancelAsync();
                }
            }

        }
        private void capturePackets(string pcapFile)
        {
            ICaptureDevice device;
            try
            {
                device = new CaptureFileReaderDevice(pcapFile);
                device.Open();
                saveLog($"[{DateTime.Now}] Start capture packets from {txt_PCapSelect.Text.Trim().Split('\\').Last()}\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception when opening file" + e.ToString());
                return;
            }

            device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);
            device.Capture();

            if (cpackMode == 1)
            {
                this.Invoke(new Action(() => { ctx_PacketsStatus.Text = $"Loaded"; }));
                this.Invoke(new Action(() => { btn_CapturePackets.Text = $"Capture Packets"; }));
                this.Invoke(new Action(() => { ctx_PacketsStatus.ForeColor = Color.Green; }));
                saveLog($"[{DateTime.Now}] All packets captured from {txt_PCapSelect.Text.Trim().Split('\\').Last()}\n");
                cpackMode = 0;
                packetsCount = dgv_PacketsList.Rows.Count;
                pgb_BackProgress.Style = ProgressBarStyle.Blocks;
            }

            device.Close();

            void device_OnPacketArrival(object sender, PacketCapture e)
            {
                if (cpackMode == 0)
                {
                    device.StopCapture();
                    device.Close();
                    ctx_NowStatus.Text = $"Packet Capture Stoped (Capture: {dgv_PacketsList.Rows.Count} Packet)";
                    ctx_PacketsStatus.Text = "Stoped";
                    saveLog($"[{DateTime.Now}] {txt_PCapSelect.Text.Trim().Split('\\').Last()} Capture packets stoped \n");
                    pgb_BackProgress.Style = ProgressBarStyle.Blocks;

                    return;
                }

                
                var rawPacket = e.GetPacket();
                var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

                #region Packet Type

                var tcpPacket = packet.Extract<TcpPacket>();
                var udpPacket = packet.Extract<UdpPacket>();

                var arpPacket = packet.Extract<ArpPacket>();

                var icmp4Packet = packet.Extract<IcmpV4Packet>();
                var icmp6Packet = packet.Extract<IcmpV6Packet>();

                var igmpPacket = packet.Extract<IgmpPacket>();
                var igmp2Packet = packet.Extract<IgmpV2Packet>();

                var dhcpPacket = packet.Extract<DhcpV4Packet>();

                var ipPacket = packet.Extract<IPPacket>();

                #endregion

                #region Set Packet Values

                var packetDict = new Dictionary<string, string> {
                    { "SrcIP", "" },
                    { "DestIP", "" },

                    { "SrcPort", "" },
                    { "DestPort", "" },

                    { "PacketType", "" },
                    { "TTL", "" },
                    { "Protocol", "" },

                    { "Length", "" },
                };


                if (tcpPacket != null)
                {
                    var tcp = (IPPacket)tcpPacket.ParentPacket;
                    packetDict["SrcIP"] = tcp.SourceAddress.ToString();
                    packetDict["DestIP"] = tcp.SourceAddress.ToString();
                    packetDict["SrcPort"] = tcpPacket.SourcePort.ToString();
                    packetDict["DestPort"] = tcpPacket.DestinationPort.ToString();

                    switch (tcpPacket.DestinationPort.ToString())
                    {
                        case "80": packetDict["Protocol"] = "HTTP"; break;
                        case "443": packetDict["Protocol"] = "HTTPS"; break;
                        case "21": packetDict["Protocol"] = "FTP"; break;
                        case "22": packetDict["Protocol"] = "SFTP"; break;
                        case "23": packetDict["Protocol"] = "TelNet"; break;
                        case "25": packetDict["Protocol"] = "SMTP"; break;
                        case "110": packetDict["Protocol"] = "POP3"; break;
                        case "143": packetDict["Protocol"] = "IMAP"; break;
                        case "389": packetDict["Protocol"] = "LDAP"; break;
                        case "3389": packetDict["Protocol"] = "RDP"; break;
                        case "1723": packetDict["Protocol"] = "VPN"; break;
                        case "5060": packetDict["Protocol"] = "SIP"; break;
                        case "137": packetDict["Protocol"] = "NBNS"; break;

                        default: packetDict["Protocol"] = "TCP"; break;
                    }

                    if (!allProtocols.Contains(packetDict["Protocol"]))
                    {
                        allProtocols.Add(packetDict["Protocol"]);
                    }

                    #region Http URL
                    if (tcpPacket.PayloadData.Length > 0 && Encoding.ASCII.GetString(tcpPacket.PayloadData).Contains("GET"))
                    {
                        string request = Encoding.ASCII.GetString(tcpPacket.PayloadData);

                        if (request.Contains("Host:"))
                        {
                            Regex r = new Regex(@"Host: (.*?)\n", RegexOptions.IgnoreCase);
                            Match url = r.Match(request);
                            if (!allHttpUrls.Contains(url.Groups[1].ToString()))
                            {
                                allHttpUrls.Add(url.Groups[1].ToString());
                            }
                        }
                    }
                    #endregion


                    packetDict["PacketType"] = tcp.Version.ToString();
                    packetDict["TTL"] = tcp.TimeToLive.ToString();
                    packetDict["Length"] = tcpPacket.PayloadData.Length.ToString();
                }
                if (udpPacket != null)
                {
                    var udp = (IPPacket)udpPacket.ParentPacket;
                    packetDict["SrcIP"] = udp.SourceAddress.ToString();
                    packetDict["DestIP"] = udp.SourceAddress.ToString();
                    packetDict["SrcPort"] = udpPacket.SourcePort.ToString();
                    packetDict["DestPort"] = udpPacket.DestinationPort.ToString();

                    switch (udpPacket.DestinationPort.ToString())
                    {
                        case "53": packetDict["Protocol"] = "DNS"; break;

                        case "67":
                        case "68": packetDict["Protocol"] = "DHCP"; break;

                        case "161":
                        case "162": packetDict["Protocol"] = "SNMP"; break;

                        case "123": packetDict["Protocol"] = "NTP"; break;

                        case "16384":
                        case "32767": packetDict["Protocol"] = "RTP"; break;

                        case "137": packetDict["Protocol"] = "NBNS"; break;
                        case "5355": packetDict["Protocol"] = "LLMNR"; break;
                        case "1900": packetDict["Protocol"] = "SSDP"; break;
                        case "5353": packetDict["Protocol"] = "MDNS"; break;

                        default: packetDict["Protocol"] = "UDP"; break;
                    }

                    if (!allProtocols.Contains(packetDict["Protocol"]))
                    {
                        allProtocols.Add(packetDict["Protocol"]);
                    }

                    #region DNS URL
                    if (udpPacket.DestinationPort.ToString() == "53")
                    {
                        string dnsUrl = getDnsUrl(udpPacket.PayloadData);
                        if (!allDnsUrls.Contains(dnsUrl))
                        {
                            allDnsUrls.Add(dnsUrl);
                        }
                    }
                    #endregion

                    packetDict["PacketType"] = udp.Version.ToString();
                    packetDict["TTL"] = udp.TimeToLive.ToString();
                    string length = (udpPacket.PayloadData != null ? udpPacket.PayloadData.Length.ToString() : "");
                    packetDict["Length"] = length;
                }

                if (arpPacket != null)
                {
                    var arp = (IPPacket)arpPacket.ParentPacket;
                    packetDict["SrcIP"] = arpPacket.SenderProtocolAddress.ToString();
                    packetDict["DestIP"] = arpPacket.TargetProtocolAddress.ToString();
                    packetDict["SrcPort"] = arpPacket.SenderHardwareAddress.ToString();
                    packetDict["DestPort"] = arpPacket.TargetHardwareAddress.ToString();
                    packetDict["Protocol"] = "ARP";
                    packetDict["PacketType"] = (arp is IPPacket ? arp.Version.ToString() : "");
                    packetDict["TTL"] = (arp is IPPacket ? arp.TimeToLive.ToString() : "0");
                    packetDict["Length"] = "42";

                    if (!allProtocols.Contains("ARP"))
                    {
                        allProtocols.Add("ARP");
                    }
                }

                if (icmp4Packet != null ||
                    icmp6Packet != null ||
                    igmpPacket != null ||
                    igmp2Packet != null)
                {

                    packetDict["SrcIP"] = ipPacket.SourceAddress.ToString();
                    packetDict["DestIP"] = ipPacket.DestinationAddress.ToString();
                    packetDict["SrcPort"] = "Null";
                    packetDict["DestPort"] = "Null";
                    packetDict["Protocol"] = ipPacket.Protocol.ToString();
                    packetDict["PacketType"] = ipPacket.Version.ToString();
                    packetDict["TTL"] = ipPacket.TimeToLive.ToString();

                    if (!allProtocols.Contains(ipPacket.Protocol.ToString()))
                    {
                        allProtocols.Add(ipPacket.Protocol.ToString());
                    }

                    switch (packetDict["Protocol"])
                    {
                        case "IcmpV4": packetDict["Length"] = (icmp4Packet.PayloadData != null ? icmp4Packet.PayloadData.Length.ToString() : ""); break;
                        case "IcmpV6": packetDict["Length"] = (icmp6Packet.PayloadData != null ? icmp6Packet.PayloadData.Length.ToString() : ""); break;
                        case "Igmp": packetDict["Length"] = (igmpPacket.PayloadData != null ? igmpPacket.PayloadData.Length.ToString() : ""); break;
                        case "IgmpV2": packetDict["Length"] = (igmp2Packet.PayloadData != null ? igmp2Packet.PayloadData.Length.ToString() : ""); break;
                    }

                }

                #endregion
                
                #region Fill Packets Table
                if (tcpPacket != null ||
                    udpPacket != null ||
                    arpPacket != null ||
                    icmp4Packet != null ||
                    icmp6Packet != null ||
                    igmpPacket != null ||
                    igmp2Packet != null)
                {

                    this.Invoke(new Action(() => {
                        pgb_BackProgress.Style = ProgressBarStyle.Marquee;
                        dgv_PacketsList.Rows.Add();
                    }));
                    int i = dgv_PacketsList.Rows.Count - 1;

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_No"].Value = i + 1; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_SrcIP"].Value = packetDict["SrcIP"]; }));
                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_DestIP"].Value = packetDict["DestIP"]; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_SourcePort"].Value = packetDict["SrcPort"]; }));
                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_DestPort"].Value = packetDict["DestPort"]; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_Protocol"].Value = packetDict["Protocol"]; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_PacketType"].Value = packetDict["PacketType"]; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_LiveTime"].Value = packetDict["TTL"] + " ms"; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_Length"].Value = packetDict["Length"]; }));


                    Thread.Sleep(20);

                    this.Invoke(new Action(() =>
                    {
                        ctx_NowStatus.Text = $"Capture Packet: {i + 1}";
                        ctx_PacketsStatus.Text = $"Is Loading";
                        ctx_PacketsStatus.ForeColor = Color.Indigo;
                    }));

                    this.Invoke(new Action(() =>
                    {
                        dgv_PacketsList.Controls.Remove(lblEmpty);
                        lblEmpty.Visible = false;
                    }));
                }
                
                #endregion
            }
        }
        private void bgw_ReadPackets_DoWork(object sender, DoWorkEventArgs e)
        {
            if (txt_PCapSelect.Text != "")
            {
                capturePackets(txt_PCapSelect.Text);
            }
            else
            {
                MessageBox.Show("Select pcap file", "Select File");
            }
        }
        #endregion

        #region Decode DNS
        private string getDnsUrl(byte[] udpPacketBytes)
        {
            int i = 12;
            while (udpPacketBytes[i] != 0)
                i++;

            string name = Encoding.ASCII.GetString(udpPacketBytes, 12, i - 12);

            return ReplaceSpecialCharacters(name);
        }
        private string ReplaceSpecialCharacters(string input)
        {
            input = Regex.Replace(input, @"^[^a-zA-Z0-9\s]+", "");

            input = Regex.Replace(input, @"\r\n|\r|\n", ".");

            input = Regex.Replace(input, @"[\s\t]+|[^a-zA-Z0-9\s]+", ".");

            input = Regex.Replace(input, @"\.{2,}", ".");

            return input;
        }
        #endregion

        #region Check Empty Table
        private void emptyTable()
        {
            lblEmpty.Text = "Capture pcap file.";
            lblEmpty.Font = new Font(lblEmpty.Font, FontStyle.Bold);
            lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblEmpty.Dock = DockStyle.Fill;

            if (dgv_PacketsList.RowCount <= 0)
            {
                dgv_PacketsList.Controls.Add(lblEmpty);
                lblEmpty.Visible = true;
            }
            else
            {
                dgv_PacketsList.Controls.Remove(lblEmpty);
                lblEmpty.Visible = false;
            }
        }
        #endregion

        #region Save Log
        private void saveLog(string log)
        {
            File.AppendAllText("log.txt", log);
            txt_Log.Text += log;
        }
        #endregion

        private void txt_PCapSelect_TextChanged(object sender, EventArgs e)
        {
            if (!File.Exists(txt_PCapSelect.Text.Trim()))
            {
                ctx_PCapFile.Text = "Not Loaded";
                ctx_PCapFile.ForeColor = Color.Red;
            }
            else
            {
                ctx_PCapFile.Text = "Loaded";
                ctx_PCapFile.ForeColor = Color.Green;
            }
        }

        private void btn_ClearLog_Click(object sender, EventArgs e)
        {
            txt_Log.Clear();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_OnePageReport_Click(object sender, EventArgs e)
        {
            
        }
    }
}
