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
            emptyTable();
        }

        private void emptyTable()
        {
            Label lblEmpty = new Label();
            lblEmpty.Text = "Select/Capture pcap file.";
            lblEmpty.Font = new Font(lblEmpty.Font, FontStyle.Bold);
            lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblEmpty.Dock = DockStyle.Fill;

            if (dgv_PacketsList.RowCount == 0)
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
    }
}
