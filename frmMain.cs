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
        public Regex rx = new Regex(@"/Type=(.*?)]|SourceAddress=(.*?),|DestinationAddress=(.*?),|Protocol=(.*?),|TimeToLive=(.*?)]|SourcePort=(.*?),|DestinationPort=(.*?)]/gm", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
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

                
                Match packetInfo = rx.Match(packet.ToString());
                if (packetInfo.Success)
                {
                    this.Invoke(new Action(() => { pgb_BackProgress.Value = 0; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows.Add(); }));
                    int i = dgv_PacketsList.Rows.Count - 1;

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_No"].Value = i + 1; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_SrcIP"].Value = packetInfo.Groups[2].Value; }));
                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_DestIP"].Value = packetInfo.Groups[3].Value; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_SourcePort"].Value = packetInfo.Groups[6].Value; }));
                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_DestPort"].Value = packetInfo.Groups[7].Value; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_Protocol"].Value = packetInfo.Groups[4].Value; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_PacketType"].Value = packetInfo.Groups[1].Value; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Rows[i].Cells["col_LiveTime"].Value = packetInfo.Groups[5].Value; }));

                    Thread.Sleep(20);

                    this.Invoke(new Action(() => { ctx_NowStatus.Text = $"Capture Packet: {i}"; }));
                    this.Invoke(new Action(() => { ctx_PacketsStatus.Text = $"Is Loading"; }));
                    this.Invoke(new Action(() => { ctx_PacketsStatus.ForeColor = Color.Indigo; }));

                    this.Invoke(new Action(() => { pgb_BackProgress.Value += 50; }));

                    this.Invoke(new Action(() => { dgv_PacketsList.Controls.Remove(lblEmpty); }));
                    this.Invoke(new Action(() => { lblEmpty.Visible = false; }));
                }
                else
                {
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
