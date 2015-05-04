namespace TFV
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.MenuItem menuItem1;
            System.Windows.Forms.MenuItem menuItem3;
            System.Windows.Forms.MenuItem menuItem2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuOpenConnection = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tcTrees = new System.Windows.Forms.TabControl();
            this.tbServer = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuWorkspaces = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuShowDeleted = new System.Windows.Forms.MenuItem();
            this.menuInWorkspace = new System.Windows.Forms.MenuItem();
            this.cbAddress = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusBarProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.stvServerTreeView = new TFV.ServerTreeView();
            this.ltvLocalTreeView = new TFV.LocalTreeView();
            menuItem1 = new System.Windows.Forms.MenuItem();
            menuItem3 = new System.Windows.Forms.MenuItem();
            menuItem2 = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tcTrees.SuspendLayout();
            this.tbServer.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItem1
            // 
            menuItem1.Index = 0;
            menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuOpenConnection,
            menuItem3,
            this.menuExit});
            menuItem1.Text = "&File";
            // 
            // menuOpenConnection
            // 
            this.menuOpenConnection.Index = 0;
            this.menuOpenConnection.Text = "&Open Connection...";
            this.menuOpenConnection.Click += new System.EventHandler(this.menuOpenConnection_Click);
            // 
            // menuItem3
            // 
            menuItem3.Index = 1;
            menuItem3.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Index = 2;
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuItem2
            // 
            menuItem2.Index = 2;
            menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAbout});
            menuItem2.Text = "&Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Index = 0;
            this.menuAbout.Text = "&About";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(888, 334);
            this.splitContainer1.SplitterDistance = 246;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tcTrees);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer2.Size = new System.Drawing.Size(888, 246);
            this.splitContainer2.SplitterDistance = 296;
            this.splitContainer2.TabIndex = 0;
            // 
            // tcTrees
            // 
            this.tcTrees.Controls.Add(this.tbServer);
            this.tcTrees.Controls.Add(this.tabPage2);
            this.tcTrees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTrees.Location = new System.Drawing.Point(0, 0);
            this.tcTrees.Name = "tcTrees";
            this.tcTrees.SelectedIndex = 0;
            this.tcTrees.Size = new System.Drawing.Size(296, 246);
            this.tcTrees.TabIndex = 0;
            this.tcTrees.Selected += new System.Windows.Forms.TabControlEventHandler(this.tcTrees_Selected);
            // 
            // tbServer
            // 
            this.tbServer.Controls.Add(this.stvServerTreeView);
            this.tbServer.Location = new System.Drawing.Point(4, 22);
            this.tbServer.Name = "tbServer";
            this.tbServer.Padding = new System.Windows.Forms.Padding(3);
            this.tbServer.Size = new System.Drawing.Size(288, 220);
            this.tbServer.TabIndex = 0;
            this.tbServer.Text = "Server";
            this.tbServer.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ltvLocalTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(288, 236);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Workspace";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(588, 246);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(580, 220);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(580, 236);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(888, 84);
            this.textBox1.TabIndex = 0;
            this.textBox1.WordWrap = false;
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            menuItem1,
            this.menuItem4,
            menuItem2});
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuWorkspaces,
            this.menuItem6,
            this.menuShowDeleted,
            this.menuInWorkspace});
            this.menuItem4.Text = "&View";
            // 
            // menuWorkspaces
            // 
            this.menuWorkspaces.Index = 0;
            this.menuWorkspaces.Text = "Manage &Workspaces...";
            this.menuWorkspaces.Click += new System.EventHandler(this.menuWorkspaces_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "-";
            // 
            // menuShowDeleted
            // 
            this.menuShowDeleted.Index = 2;
            this.menuShowDeleted.Text = "Show &Deleted Files";
            this.menuShowDeleted.Click += new System.EventHandler(this.menuShowDeleted_Click);
            // 
            // menuInWorkspace
            // 
            this.menuInWorkspace.Checked = true;
            this.menuInWorkspace.Index = 3;
            this.menuInWorkspace.Text = "&Limit View to Items in Workspace";
            this.menuInWorkspace.Click += new System.EventHandler(this.menuInWorkspace_Click);
            // 
            // cbAddress
            // 
            this.cbAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAddress.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbAddress.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbAddress.Location = new System.Drawing.Point(5, 3);
            this.cbAddress.Name = "cbAddress";
            this.cbAddress.Size = new System.Drawing.Size(880, 21);
            this.cbAddress.TabIndex = 0;
            this.cbAddress.SelectionChangeCommitted += new System.EventHandler(this.cbAddress_SelectionChangeCommitted);
            this.cbAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbAddress_KeyDown);
            this.cbAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbAddress_KeyUp);
            this.cbAddress.Validating += new System.ComponentModel.CancelEventHandler(this.cbAddress_Validating);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.cbAddress, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(888, 27);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // statusBar
            // 
            this.statusBar.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarProgress});
            this.statusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusBar.Location = new System.Drawing.Point(0, 361);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(888, 22);
            this.statusBar.TabIndex = 3;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusBarProgress
            // 
            this.statusBarProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusBarProgress.MarqueeAnimationSpeed = 50;
            this.statusBarProgress.Name = "statusBarProgress";
            this.statusBarProgress.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.statusBarProgress.Size = new System.Drawing.Size(100, 16);
            this.statusBarProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.statusBarProgress.Value = 50;
            this.statusBarProgress.Visible = false;
            // 
            // stvServerTreeView
            // 
            this.stvServerTreeView.DeletedState = Microsoft.TeamFoundation.VersionControl.Client.DeletedState.NonDeleted;
            this.stvServerTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stvServerTreeView.LimitToWorkspace = false;
            this.stvServerTreeView.Location = new System.Drawing.Point(3, 3);
            this.stvServerTreeView.Margin = new System.Windows.Forms.Padding(6);
            this.stvServerTreeView.Name = "stvServerTreeView";
            this.stvServerTreeView.ShowFiles = true;
            this.stvServerTreeView.Size = new System.Drawing.Size(282, 214);
            this.stvServerTreeView.SourceControl = null;
            this.stvServerTreeView.TabIndex = 0;
            this.stvServerTreeView.Workspace = null;
            // 
            // ltvLocalTreeView
            // 
            this.ltvLocalTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ltvLocalTreeView.Location = new System.Drawing.Point(3, 3);
            this.ltvLocalTreeView.Name = "ltvLocalTreeView";
            this.ltvLocalTreeView.ShowFiles = true;
            this.ltvLocalTreeView.Size = new System.Drawing.Size(282, 230);
            this.ltvLocalTreeView.SourceControl = null;
            this.ltvLocalTreeView.TabIndex = 0;
            this.ltvLocalTreeView.Workspace = null;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 383);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "TFV";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tcTrees.ResumeLayout(false);
            this.tbServer.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tcTrees;
        private System.Windows.Forms.TabPage tbServer;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuOpenConnection;
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.MenuItem menuAbout;
        private System.Windows.Forms.ComboBox cbAddress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ServerTreeView stvServerTreeView;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuShowDeleted;
        private System.Windows.Forms.MenuItem menuInWorkspace;
        private System.Windows.Forms.MenuItem menuWorkspaces;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripProgressBar statusBarProgress;
        private LocalTreeView ltvLocalTreeView;
    }
}

