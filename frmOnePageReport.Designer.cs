namespace Toratan
{
    partial class frmOnePageReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOnePageReport));
            this.img_Logo = new System.Windows.Forms.PictureBox();
            this.lbl_Title = new System.Windows.Forms.Label();
            this.lbl_Author = new System.Windows.Forms.Label();
            this.pnl_Panel2 = new System.Windows.Forms.Panel();
            this.lbl_PCAPFileTitle = new System.Windows.Forms.Label();
            this.lbl_PCAPFileName = new System.Windows.Forms.Label();
            this.lbl_Packets = new System.Windows.Forms.Label();
            this.lbl_CapturedTime = new System.Windows.Forms.Label();
            this.lbl_FileSize = new System.Windows.Forms.Label();
            this.lvw_DnsUrls = new System.Windows.Forms.ListView();
            this.lbl_DNSUrlsTitle = new System.Windows.Forms.Label();
            this.pnl_Panel4 = new System.Windows.Forms.Panel();
            this.pnl_Panel1 = new System.Windows.Forms.Panel();
            this.lbl_HTTPUrlsTitle = new System.Windows.Forms.Label();
            this.pnl_Panel3 = new System.Windows.Forms.Panel();
            this.lvw_HttpUrls = new System.Windows.Forms.ListView();
            this.pnl_Panel5 = new System.Windows.Forms.Panel();
            this.lbl_ProtocolsTitle = new System.Windows.Forms.Label();
            this.lvw_Protocols = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.img_Logo)).BeginInit();
            this.pnl_Panel2.SuspendLayout();
            this.pnl_Panel4.SuspendLayout();
            this.pnl_Panel1.SuspendLayout();
            this.pnl_Panel3.SuspendLayout();
            this.pnl_Panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // img_Logo
            // 
            this.img_Logo.Image = global::Toratan.Properties.Resources.toratan;
            this.img_Logo.Location = new System.Drawing.Point(4, 4);
            this.img_Logo.Margin = new System.Windows.Forms.Padding(4);
            this.img_Logo.Name = "img_Logo";
            this.img_Logo.Size = new System.Drawing.Size(123, 106);
            this.img_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.img_Logo.TabIndex = 0;
            this.img_Logo.TabStop = false;
            // 
            // lbl_Title
            // 
            this.lbl_Title.AutoSize = true;
            this.lbl_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.lbl_Title.Location = new System.Drawing.Point(134, 4);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new System.Drawing.Size(108, 31);
            this.lbl_Title.TabIndex = 1;
            this.lbl_Title.Text = "Toratan";
            // 
            // lbl_Author
            // 
            this.lbl_Author.AutoSize = true;
            this.lbl_Author.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Author.Location = new System.Drawing.Point(136, 35);
            this.lbl_Author.Name = "lbl_Author";
            this.lbl_Author.Size = new System.Drawing.Size(132, 20);
            this.lbl_Author.TabIndex = 1;
            this.lbl_Author.Text = "Morteza Hosseini";
            // 
            // pnl_Panel2
            // 
            this.pnl_Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Panel2.Controls.Add(this.lbl_FileSize);
            this.pnl_Panel2.Controls.Add(this.lbl_CapturedTime);
            this.pnl_Panel2.Controls.Add(this.lbl_Packets);
            this.pnl_Panel2.Controls.Add(this.lbl_PCAPFileName);
            this.pnl_Panel2.Location = new System.Drawing.Point(469, 12);
            this.pnl_Panel2.Name = "pnl_Panel2";
            this.pnl_Panel2.Size = new System.Drawing.Size(264, 116);
            this.pnl_Panel2.TabIndex = 2;
            // 
            // lbl_PCAPFileTitle
            // 
            this.lbl_PCAPFileTitle.AutoSize = true;
            this.lbl_PCAPFileTitle.Location = new System.Drawing.Point(473, 5);
            this.lbl_PCAPFileTitle.Name = "lbl_PCAPFileTitle";
            this.lbl_PCAPFileTitle.Size = new System.Drawing.Size(70, 17);
            this.lbl_PCAPFileTitle.TabIndex = 3;
            this.lbl_PCAPFileTitle.Text = "PCAP File";
            // 
            // lbl_PCAPFileName
            // 
            this.lbl_PCAPFileName.AutoSize = true;
            this.lbl_PCAPFileName.Location = new System.Drawing.Point(49, 10);
            this.lbl_PCAPFileName.Name = "lbl_PCAPFileName";
            this.lbl_PCAPFileName.Size = new System.Drawing.Size(49, 17);
            this.lbl_PCAPFileName.TabIndex = 4;
            this.lbl_PCAPFileName.Text = "Name:";
            // 
            // lbl_Packets
            // 
            this.lbl_Packets.AutoSize = true;
            this.lbl_Packets.Location = new System.Drawing.Point(36, 36);
            this.lbl_Packets.Name = "lbl_Packets";
            this.lbl_Packets.Size = new System.Drawing.Size(62, 17);
            this.lbl_Packets.TabIndex = 4;
            this.lbl_Packets.Text = "Packets:";
            // 
            // lbl_CapturedTime
            // 
            this.lbl_CapturedTime.AutoSize = true;
            this.lbl_CapturedTime.Location = new System.Drawing.Point(11, 62);
            this.lbl_CapturedTime.Name = "lbl_CapturedTime";
            this.lbl_CapturedTime.Size = new System.Drawing.Size(87, 17);
            this.lbl_CapturedTime.TabIndex = 4;
            this.lbl_CapturedTime.Text = "Captured At:";
            // 
            // lbl_FileSize
            // 
            this.lbl_FileSize.AutoSize = true;
            this.lbl_FileSize.Location = new System.Drawing.Point(33, 88);
            this.lbl_FileSize.Name = "lbl_FileSize";
            this.lbl_FileSize.Size = new System.Drawing.Size(65, 17);
            this.lbl_FileSize.TabIndex = 4;
            this.lbl_FileSize.Text = "File Size:";
            // 
            // lvw_DnsUrls
            // 
            this.lvw_DnsUrls.BackColor = System.Drawing.Color.White;
            this.lvw_DnsUrls.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvw_DnsUrls.HideSelection = false;
            this.lvw_DnsUrls.Location = new System.Drawing.Point(6, 10);
            this.lvw_DnsUrls.Name = "lvw_DnsUrls";
            this.lvw_DnsUrls.Size = new System.Drawing.Size(344, 395);
            this.lvw_DnsUrls.TabIndex = 4;
            this.lvw_DnsUrls.UseCompatibleStateImageBehavior = false;
            this.lvw_DnsUrls.View = System.Windows.Forms.View.List;
            // 
            // lbl_DNSUrlsTitle
            // 
            this.lbl_DNSUrlsTitle.AutoSize = true;
            this.lbl_DNSUrlsTitle.Location = new System.Drawing.Point(382, 137);
            this.lbl_DNSUrlsTitle.Name = "lbl_DNSUrlsTitle";
            this.lbl_DNSUrlsTitle.Size = new System.Drawing.Size(66, 17);
            this.lbl_DNSUrlsTitle.TabIndex = 3;
            this.lbl_DNSUrlsTitle.Text = "DNS Urls";
            // 
            // pnl_Panel4
            // 
            this.pnl_Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Panel4.Controls.Add(this.lvw_DnsUrls);
            this.pnl_Panel4.Location = new System.Drawing.Point(378, 146);
            this.pnl_Panel4.Name = "pnl_Panel4";
            this.pnl_Panel4.Size = new System.Drawing.Size(355, 410);
            this.pnl_Panel4.TabIndex = 5;
            // 
            // pnl_Panel1
            // 
            this.pnl_Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Panel1.Controls.Add(this.img_Logo);
            this.pnl_Panel1.Controls.Add(this.lbl_Title);
            this.pnl_Panel1.Controls.Add(this.lbl_Author);
            this.pnl_Panel1.Location = new System.Drawing.Point(12, 12);
            this.pnl_Panel1.Name = "pnl_Panel1";
            this.pnl_Panel1.Size = new System.Drawing.Size(451, 116);
            this.pnl_Panel1.TabIndex = 6;
            // 
            // lbl_HTTPUrlsTitle
            // 
            this.lbl_HTTPUrlsTitle.AutoSize = true;
            this.lbl_HTTPUrlsTitle.Location = new System.Drawing.Point(16, 137);
            this.lbl_HTTPUrlsTitle.Name = "lbl_HTTPUrlsTitle";
            this.lbl_HTTPUrlsTitle.Size = new System.Drawing.Size(74, 17);
            this.lbl_HTTPUrlsTitle.TabIndex = 6;
            this.lbl_HTTPUrlsTitle.Text = "HTTP Urls";
            // 
            // pnl_Panel3
            // 
            this.pnl_Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Panel3.Controls.Add(this.lvw_HttpUrls);
            this.pnl_Panel3.Location = new System.Drawing.Point(12, 146);
            this.pnl_Panel3.Name = "pnl_Panel3";
            this.pnl_Panel3.Size = new System.Drawing.Size(355, 410);
            this.pnl_Panel3.TabIndex = 7;
            // 
            // lvw_HttpUrls
            // 
            this.lvw_HttpUrls.BackColor = System.Drawing.Color.White;
            this.lvw_HttpUrls.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvw_HttpUrls.HideSelection = false;
            this.lvw_HttpUrls.Location = new System.Drawing.Point(6, 10);
            this.lvw_HttpUrls.Name = "lvw_HttpUrls";
            this.lvw_HttpUrls.Size = new System.Drawing.Size(344, 395);
            this.lvw_HttpUrls.TabIndex = 4;
            this.lvw_HttpUrls.UseCompatibleStateImageBehavior = false;
            this.lvw_HttpUrls.View = System.Windows.Forms.View.List;
            // 
            // pnl_Panel5
            // 
            this.pnl_Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Panel5.Controls.Add(this.lvw_Protocols);
            this.pnl_Panel5.Location = new System.Drawing.Point(12, 570);
            this.pnl_Panel5.Name = "pnl_Panel5";
            this.pnl_Panel5.Size = new System.Drawing.Size(355, 199);
            this.pnl_Panel5.TabIndex = 9;
            // 
            // lbl_ProtocolsTitle
            // 
            this.lbl_ProtocolsTitle.AutoSize = true;
            this.lbl_ProtocolsTitle.Location = new System.Drawing.Point(16, 559);
            this.lbl_ProtocolsTitle.Name = "lbl_ProtocolsTitle";
            this.lbl_ProtocolsTitle.Size = new System.Drawing.Size(67, 17);
            this.lbl_ProtocolsTitle.TabIndex = 8;
            this.lbl_ProtocolsTitle.Text = "Protocols";
            // 
            // lvw_Protocols
            // 
            this.lvw_Protocols.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvw_Protocols.HideSelection = false;
            this.lvw_Protocols.Location = new System.Drawing.Point(6, 8);
            this.lvw_Protocols.Name = "lvw_Protocols";
            this.lvw_Protocols.Size = new System.Drawing.Size(344, 186);
            this.lvw_Protocols.TabIndex = 10;
            this.lvw_Protocols.UseCompatibleStateImageBehavior = false;
            this.lvw_Protocols.View = System.Windows.Forms.View.Details;
            // 
            // frmOnePageReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(745, 788);
            this.Controls.Add(this.lbl_ProtocolsTitle);
            this.Controls.Add(this.pnl_Panel5);
            this.Controls.Add(this.lbl_HTTPUrlsTitle);
            this.Controls.Add(this.pnl_Panel1);
            this.Controls.Add(this.pnl_Panel3);
            this.Controls.Add(this.lbl_DNSUrlsTitle);
            this.Controls.Add(this.pnl_Panel4);
            this.Controls.Add(this.lbl_PCAPFileTitle);
            this.Controls.Add(this.pnl_Panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmOnePageReport";
            this.Text = "Report Pcap Files";
            ((System.ComponentModel.ISupportInitialize)(this.img_Logo)).EndInit();
            this.pnl_Panel2.ResumeLayout(false);
            this.pnl_Panel2.PerformLayout();
            this.pnl_Panel4.ResumeLayout(false);
            this.pnl_Panel1.ResumeLayout(false);
            this.pnl_Panel1.PerformLayout();
            this.pnl_Panel3.ResumeLayout(false);
            this.pnl_Panel5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox img_Logo;
        private System.Windows.Forms.Label lbl_Title;
        private System.Windows.Forms.Label lbl_Author;
        private System.Windows.Forms.Panel pnl_Panel2;
        private System.Windows.Forms.Label lbl_PCAPFileTitle;
        private System.Windows.Forms.Label lbl_Packets;
        private System.Windows.Forms.Label lbl_PCAPFileName;
        private System.Windows.Forms.Label lbl_CapturedTime;
        private System.Windows.Forms.Label lbl_FileSize;
        private System.Windows.Forms.ListView lvw_DnsUrls;
        private System.Windows.Forms.Label lbl_DNSUrlsTitle;
        private System.Windows.Forms.Panel pnl_Panel4;
        private System.Windows.Forms.Panel pnl_Panel1;
        private System.Windows.Forms.Label lbl_HTTPUrlsTitle;
        private System.Windows.Forms.Panel pnl_Panel3;
        private System.Windows.Forms.ListView lvw_HttpUrls;
        private System.Windows.Forms.Panel pnl_Panel5;
        private System.Windows.Forms.Label lbl_ProtocolsTitle;
        private System.Windows.Forms.ListView lvw_Protocols;
    }
}