namespace TFV
{
    partial class OpenConnection
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
            System.Windows.Forms.Label label5;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenConnection));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnConnections = new System.Windows.Forms.Button();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbWorkspace = new System.Windows.Forms.TextBox();
            this.btnBrowseWorkspace = new System.Windows.Forms.Button();
            this.cbNTLM = new System.Windows.Forms.CheckBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            label5 = new System.Windows.Forms.Label();
            flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(label1, 3);
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Location = new System.Drawing.Point(3, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(415, 30);
            label1.TabIndex = 1;
            label1.Text = "Select a connection or enter a new one:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.Location = new System.Drawing.Point(3, 30);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(65, 26);
            label2.TabIndex = 3;
            label2.Text = "Server:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Fill;
            label3.Location = new System.Drawing.Point(3, 79);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(65, 26);
            label3.TabIndex = 4;
            label3.Text = "User:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = System.Windows.Forms.DockStyle.Fill;
            label4.Location = new System.Drawing.Point(3, 131);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(65, 29);
            label4.TabIndex = 5;
            label4.Text = "Workspace:";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(flowLayoutPanel1, 2);
            flowLayoutPanel1.Controls.Add(this.btnOk);
            flowLayoutPanel1.Controls.Add(this.btnCancel);
            flowLayoutPanel1.Location = new System.Drawing.Point(358, 170);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(162, 29);
            flowLayoutPanel1.TabIndex = 12;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(3, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(84, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = System.Windows.Forms.DockStyle.Fill;
            label5.Location = new System.Drawing.Point(3, 105);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(65, 26);
            label5.TabIndex = 14;
            label5.Text = "Password:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.15204F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.84796F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnConnections, 3, 0);
            this.tableLayoutPanel1.Controls.Add(label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbServer, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbUser, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbWorkspace, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowseWorkspace, 3, 5);
            this.tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.cbNTLM, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbPassword, 1, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(520, 199);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnConnections
            // 
            this.btnConnections.AutoSize = true;
            this.btnConnections.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnConnections.Image = ((System.Drawing.Image)(resources.GetObject("btnConnections.Image")));
            this.btnConnections.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnConnections.Location = new System.Drawing.Point(425, 3);
            this.btnConnections.Name = "btnConnections";
            this.btnConnections.Size = new System.Drawing.Size(92, 24);
            this.btnConnections.TabIndex = 2;
            this.btnConnections.Text = "&Connections";
            this.btnConnections.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnConnections.UseVisualStyleBackColor = true;
            this.btnConnections.Click += new System.EventHandler(this.btnConnections_Click);
            // 
            // tbServer
            // 
            this.tbServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbServer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.tableLayoutPanel1.SetColumnSpan(this.tbServer, 2);
            this.tbServer.Location = new System.Drawing.Point(74, 33);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(344, 20);
            this.tbServer.TabIndex = 3;
            // 
            // tbUser
            // 
            this.tbUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.tbUser, 2);
            this.tbUser.Enabled = false;
            this.tbUser.Location = new System.Drawing.Point(74, 82);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(344, 20);
            this.tbUser.TabIndex = 5;
            // 
            // tbWorkspace
            // 
            this.tbWorkspace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.tbWorkspace, 2);
            this.tbWorkspace.Location = new System.Drawing.Point(74, 135);
            this.tbWorkspace.Name = "tbWorkspace";
            this.tbWorkspace.Size = new System.Drawing.Size(344, 20);
            this.tbWorkspace.TabIndex = 7;
            // 
            // btnBrowseWorkspace
            // 
            this.btnBrowseWorkspace.Location = new System.Drawing.Point(424, 134);
            this.btnBrowseWorkspace.Name = "btnBrowseWorkspace";
            this.btnBrowseWorkspace.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseWorkspace.TabIndex = 8;
            this.btnBrowseWorkspace.Text = "B&rowse";
            this.btnBrowseWorkspace.UseVisualStyleBackColor = true;
            this.btnBrowseWorkspace.Click += new System.EventHandler(this.btnBrowseWorkspace_Click);
            // 
            // cbNTLM
            // 
            this.cbNTLM.AutoSize = true;
            this.cbNTLM.Checked = true;
            this.cbNTLM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.cbNTLM, 2);
            this.cbNTLM.Location = new System.Drawing.Point(74, 59);
            this.cbNTLM.Name = "cbNTLM";
            this.cbNTLM.Size = new System.Drawing.Size(155, 17);
            this.cbNTLM.TabIndex = 4;
            this.cbNTLM.Text = "Use Existing Authentication";
            this.cbNTLM.UseVisualStyleBackColor = true;
            this.cbNTLM.CheckedChanged += new System.EventHandler(this.cbNTLM_CheckedChanged);
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.tbPassword, 2);
            this.tbPassword.Enabled = false;
            this.tbPassword.Location = new System.Drawing.Point(74, 108);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(344, 20);
            this.tbPassword.TabIndex = 6;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // OpenConnection
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(544, 223);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenConnection";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Open Connection";
            this.Load += new System.EventHandler(this.OpenConnection_Load);
            flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnConnections;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbWorkspace;
        private System.Windows.Forms.Button btnBrowseWorkspace;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbNTLM;
        private System.Windows.Forms.TextBox tbPassword;


    }
}