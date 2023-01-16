using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpPcap;
using PacketDotNet;
using SharpPcap.LibPcap;
using PacketDotNet.Ieee80211;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

namespace Toratan
{
    public partial class frmMain : Form
    {
        Label lblEmpty = new Label();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            emptyTable();
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
                ctx_PCapFile.Text = "Loaded";
                ctx_PCapFile.ForeColor = Color.Green;
            }
        }
        private void btn_CapturePackets_Click(object sender, EventArgs e)
        {
            bgw_ReadPackets = new BackgroundWorker();
            bgw_ReadPackets.WorkerReportsProgress = true;
            bgw_ReadPackets.DoWork += new DoWorkEventHandler(bgw_ReadPackets_DoWork);
            bgw_ReadPackets.RunWorkerAsync();
        }
        private void capturePackets(string pcapFile)
        {
            ICaptureDevice device;
            try
            {
                device = new CaptureFileReaderDevice(pcapFile);
                device.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception when opening file" + e.ToString());
                return;
            }

            device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);
            device.Capture();
            device.Close();

            void device_OnPacketArrival(object sender, PacketCapture e)
            {
                var rawPacket = e.GetPacket();
                var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

                #region Packet Type

                var tcpPacket = packet.Extract<TcpPacket>();
                var udpPacket = packet.Extract<UdpPacket>();

                var arpPacket = packet.Extract<ArpPacket>();

                var icmp4Packet = packet.Extract<IcmpV4Packet>();
                var icmp5Packet = packet.Extract<IcmpV6Packet>();

                var igmpPacket = packet.Extract<IgmpPacket>();
                var igmp2Packet = packet.Extract<IgmpV2Packet>();

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
                };


                if (tcpPacket != null)
                {
                    var tcp = (IPPacket)tcpPacket.ParentPacket;
                    packetDict["SrcIP"]      = tcp.SourceAddress.ToString();
                    packetDict["DestIP"]     = tcp.SourceAddress.ToString();
                    packetDict["SrcPort"]    = tcpPacket.SourcePort.ToString();
                    packetDict["DestPort"]   = tcpPacket.DestinationPort.ToString();
                    packetDict["Protocol"]   = tcp.Protocol.ToString();
                    packetDict["PacketType"] = tcp.Version.ToString();
                    packetDict["TTL"]        = tcp.TimeToLive.ToString();
                }
                if (udpPacket != null)
                {
                    var udp = (IPPacket)udpPacket.ParentPacket;
                    packetDict["SrcIP"]      = udp.SourceAddress.ToString();
                    packetDict["DestIP"]     = udp.SourceAddress.ToString();
                    packetDict["SrcPort"]    = udpPacket.SourcePort.ToString();
                    packetDict["DestPort"]   = udpPacket.DestinationPort.ToString();
                    packetDict["Protocol"]   = udp.Protocol.ToString();
                    packetDict["PacketType"] = udp.Version.ToString();
                    packetDict["TTL"]        = udp.TimeToLive.ToString();
                }

                if (arpPacket != null)
                {
                    var arp = (IPPacket)arpPacket.ParentPacket;
                    packetDict["SrcIP"]      = arpPacket.SenderProtocolAddress.ToString();
                    packetDict["DestIP"]     = arpPacket.TargetProtocolAddress.ToString();
                    packetDict["SrcPort"]    = arpPacket.SenderHardwareAddress.ToString();
                    packetDict["DestPort"]   = arpPacket.TargetHardwareAddress.ToString();
                    packetDict["Protocol"]   = "ARP";
                    packetDict["PacketType"] = (arp is IPPacket ? arp.Version.ToString() : "");
                    packetDict["TTL"]        = (arp is IPPacket ? arp.TimeToLive.ToString() : "0");
                }

                if (icmp4Packet != null ||
                    icmp5Packet != null ||
                    igmpPacket  != null ||
                    igmp2Packet != null)
                {
                    var ipPacket = packet.Extract<IPPacket>();
                    packetDict["SrcIP"]      = ipPacket.SourceAddress.ToString();
                    packetDict["DestIP"]     = ipPacket.DestinationAddress.ToString();
                    packetDict["SrcPort"]    = "Null";
                    packetDict["DestPort"]   = "Null";
                    packetDict["Protocol"]   = ipPacket.Protocol.ToString();
                    packetDict["PacketType"] = ipPacket.Version.ToString();
                    packetDict["TTL"]        = ipPacket.TimeToLive.ToString();
                }

                #endregion







                if (tcpPacket   != null ||
                    udpPacket   != null ||
                    arpPacket   != null ||
                    icmp4Packet != null ||
                    icmp5Packet != null ||
                    igmpPacket  != null ||
                    igmp2Packet != null)
                {
                    this.Invoke(new Action(() => { pgb_BackProgress.Value = 0; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows.Add(); }));
                    int i = dgv_PacketsList.Rows.Count - 1;

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_No"].Value = i + 1; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_SrcIP"].Value = packetDict["SrcIP"]; }));
                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_DestIP"].Value = packetDict["DestIP"]; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_SourcePort"].Value = packetDict["SrcPort"]; }));
                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_DestPort"].Value = packetDict["DestPort"]; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_Protocol"].Value = packetDict["Protocol"]; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_PacketType"].Value = packetDict["PacketType"]; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_LiveTime"].Value = packetDict["TTL"] + " ms"; }));


                    Thread.Sleep(20);

                    this.Invoke(new Action(() =>
                    {
                        ctx_NowStatus.Text = $"Capture Packet: {i}";
                        ctx_PacketsStatus.Text = $"Is Loading";
                        ctx_PacketsStatus.ForeColor = Color.Indigo;
                    }));

                    this.Invoke(new Action(() => { pgb_BackProgress.Value += 50; }));

                    this.Invoke(new Action(() =>
                    {
                        dgv_PacketsList.Controls.Remove(lblEmpty);
                        lblEmpty.Visible = false;
                        // dgv_PacketsList.FirstDisplayedScrollingRowIndex = dgv_PacketsList.RowCount - 1;
                    }));
                }
                else
                {
                    MessageBox.Show("Packet:\n\n" + packet);
                    this.Invoke(new Action(() => { ctx_PacketsStatus.Text = $"Loaded"; }));
                    this.Invoke(new Action(() => { ctx_PacketsStatus.ForeColor = Color.Green; }));
                }
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

    }
}
