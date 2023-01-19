using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toratan
{
    public partial class frmOnePageReport : Form
    {
        public string packetCount, pcapName, pcapSize, capturedDate;
        public List<string> allHttpUrls = new List<string>(), allDnsUrls = new List<string>();
        public List<string> allProtocols = new List<string>();
        public frmOnePageReport()
        {
            InitializeComponent();
        }

        private void frmOnePageReport_Load(object sender, EventArgs e)
        {
            ctx_TodayDate.Text    = DateTime.Now.ToString();
            ctx_PCAPFileName.Text = pcapName;
            ctx_FileSize.Text     = pcapSize + " Mb";
            ctx_PacketsCount.Text = packetCount;
            ctx_CapturedDate.Text = capturedDate;

            foreach (var http in allHttpUrls)
            {
                lvw_HttpUrls.Items.Add(http);
            }
            foreach (var dns in allDnsUrls)
            {
                lvw_DnsUrls.Items.Add(dns);
            }
            foreach (var proto in allProtocols)
            {
                lvw_Protocols.Items.Add(proto);
            }
        }
    }
}
