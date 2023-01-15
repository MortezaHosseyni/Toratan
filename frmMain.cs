using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toratan
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CheckEmptyDataGridView(dgv_PacketsList);
        }

        public static void CheckEmptyDataGridView(DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count == 0)
            {
                dataGridView.Visible = false;
                var label = new Label();
                label.Text = "Select .pcap file / Capture packets";
                label.Dock = DockStyle.Fill;
                label.TextAlign = ContentAlignment.MiddleCenter;
                dataGridView.Controls.Add(label);
            }
        }
    }
}
