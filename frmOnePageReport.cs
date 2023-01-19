using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Toratan
{
    public partial class frmOnePageReport : Form
    {
        public string packetCount, pcapName, pcapSize, capturedDate;
        public List<string> allHttpUrls = new List<string>();
        public Dictionary<string, int> allDnsUrls = new Dictionary<string, int>();
        public List<string> allProtocols = new List<string>();

        sbyte mode = 0; // 0: Not Save | 1: Save
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
                lvw_DnsUrls.Items.Add($"{dns.Key} ({dns.Value})");
            }
            foreach (var proto in allProtocols)
            {
                lvw_Protocols.Items.Add(proto.ToUpper());
            }
        }


        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Print Page
        private void btn_Print_Click(object sender, EventArgs e)
        {
            mode = 1;
            hideBtn();

            CaptureScreen();
            pnt_Print.Print();
            pnt_Print.PrinterSettings.PrintFileName = "Toratan_Report.pdf";
            pnt_Print.PrintPage += new PrintPageEventHandler(pnt_Print_PrintPage);

            mode = 0;
            hideBtn();
        }
        void PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Rectangle bounds = this.ClientRectangle;
            graphics.DrawImage(new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height), 0, 0);
        }

        Bitmap memoryImage;
        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }
        private void pnt_Print_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(memoryImage, 0, 0);
        }
        #endregion
        private void hideBtn()
        {
            if (mode == 0)
            {
                btn_Print.Visible = true;
                btn_Exit.Visible = true;
            }
            else
            {
                btn_Print.Visible = false;
                btn_Exit.Visible = false;
            }
        }
    }
}
