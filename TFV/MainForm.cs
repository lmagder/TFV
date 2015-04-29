using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;
using Microsoft.TeamFoundation.VersionControl.Controls;
using Microsoft.TeamFoundation.VersionControl.Controls.Common;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation;

namespace TFV
{
    public partial class MainForm : Form
    {
        Workspace m_workspace;
        TfsTeamProjectCollection m_projectCollection;
        VersionControlServer m_vcServer;
        ServerTreeView m_serverTreeView;

        static int s_instanceCount = 0;
        public MainForm(TfsTeamProjectCollection pc, Workspace ws)
        {
            InitializeComponent();

            m_projectCollection = pc;
            m_workspace = ws;
            m_vcServer = m_projectCollection.GetService<VersionControlServer>();
            Text = "TFV - " + m_projectCollection.ToString();

            m_serverTreeView = new ServerTreeView();
            tbServer.Controls.Add(m_serverTreeView);
            m_serverTreeView.Dock = DockStyle.Fill;
            m_serverTreeView.SourceControl = m_vcServer;

            m_serverTreeView.CurrentServerItemChanged += m_serverTreeView_CurrentServerItemChanged;

            m_vcServer.UpdatedWorkspace += m_vcServer_UpdatedWorkspace;
            m_vcServer.FolderContentChanged += m_vcServer_FolderContentChanged;
            m_vcServer.CommitCheckin += m_vcServer_CommitCheckin;
            s_instanceCount++;
        }

        void m_vcServer_CommitCheckin(object sender, CommitCheckinEventArgs e)
        {
           
        }

        void m_vcServer_FolderContentChanged(object sender, FolderContentChangedEventArgs e)
        {
   
        }

        void m_vcServer_UpdatedWorkspace(object sender, WorkspaceEventArgs e)
        {
            
        }

        void m_serverTreeView_CurrentServerItemChanged(object sender, EventArgs e)
        {
            if (tcTrees.SelectedTab == tbServer)
            {
                cbAddress.Text = m_serverTreeView.CurrentServerItem;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            s_instanceCount--;
            if (s_instanceCount == 0)
            {
                Application.Exit();
            }
        }

        private void menuOpenConnection_Click(object sender, EventArgs e)
        {
            Program.OpenConnection(this);
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            (new AboutBox()).ShowDialog(this);
        }

        private void cbAddress_Validating(object sender, CancelEventArgs e)
        {
            if (tcTrees.SelectedTab == tbServer)
            {
                if (!VersionControlPath.IsValidPath(cbAddress.Text.Trim()))
                    e.Cancel = true;
            }
        }

    }
}
