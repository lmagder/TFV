using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;
using Microsoft.TeamFoundation.VersionControl.Controls;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation;

namespace TFV
{

    public partial class MainForm : Form
    {
        Workspace m_workspace;
        TfsTeamProjectCollection m_projectCollection;
        VersionControlServer m_vcServer;


        static int s_instanceCount = 0;
        public MainForm(TfsTeamProjectCollection pc, Workspace ws)
        {
            InitializeComponent();

            m_projectCollection = pc;
            m_workspace = ws;
            m_vcServer = m_projectCollection.GetService<VersionControlServer>();
            Text = "TFV - " + m_projectCollection.ToString();

            stvServerTreeView.SourceControl = m_vcServer;
            stvServerTreeView.Workspace = m_workspace;


            ltvLocalTreeView.SourceControl = m_vcServer;
            ltvLocalTreeView.Workspace = m_workspace;

            UpdateLimitViewToWorkspace();
            UpdateShowDeleted();

            stvServerTreeView.LastSelectedServerItemChanged += stvServerTreeView_CurrentServerItemChanged;
            stvServerTreeView.BackgroundWorkStarted += delegate(object o, EventArgs e) { StartWaiting(); };
            stvServerTreeView.BackgroundWorkEnded += delegate(object o, EventArgs e) { StopWaiting(); };


            ltvLocalTreeView.LastSelectedServerItemChanged += stvServerTreeView_CurrentServerItemChanged;
            ltvLocalTreeView.BackgroundWorkStarted += delegate (object o, EventArgs e) { StartWaiting(); };
            ltvLocalTreeView.BackgroundWorkEnded += delegate (object o, EventArgs e) { StopWaiting(); };


            cbAddress.Text = stvServerTreeView.LastSelectedServerItem;

            m_vcServer.UpdatedWorkspace += m_vcServer_UpdatedWorkspace;
            m_vcServer.FolderContentChanged += m_vcServer_FolderContentChanged;
            m_vcServer.CommitCheckin += m_vcServer_CommitCheckin;
            s_instanceCount++;
        }

        void stvServerTreeView_CurrentServerItemChanged(object sender, EventArgs e)
        {
            if (tcTrees.SelectedTab == tbServer)
            {
                cbAddress.Text = stvServerTreeView.LastSelectedServerItem;
                ltvLocalTreeView.Navigate(m_workspace.TryGetLocalItemForServerItem(cbAddress.Text));
            }
            else
            {
                cbAddress.Text = ltvLocalTreeView.LastSelectedServerItem;
                if (!string.IsNullOrWhiteSpace(cbAddress.Text))
                    stvServerTreeView.Navigate(m_workspace.TryGetServerItemForLocalItem(cbAddress.Text));
            }
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
            else
            {
                if (!FileSpec.IsLegalNtfsName(cbAddress.Text.Trim(), (int)PathLength.MaxLength, false))
                    e.Cancel = true;
            }
        }

        private void menuShowDeleted_Click(object sender, EventArgs e)
        {
            Program.Settings.ShowDeletedFiles = !Program.Settings.ShowDeletedFiles;
            UpdateShowDeleted();
                
        }

        private void menuInWorkspace_Click(object sender, EventArgs e)
        {
            Program.Settings.LimitViewToWorkspace = !Program.Settings.LimitViewToWorkspace;
            UpdateLimitViewToWorkspace();
        }

        private void menuWorkspaces_Click(object sender, EventArgs e)
        {
            SelectWorkspace.ShowWorkspaceDialog(m_vcServer, this);
        }

        private void UpdateLimitViewToWorkspace()
        {
            if (Program.Settings.LimitViewToWorkspace)
            {
                stvServerTreeView.LimitToWorkspace = true;
                menuInWorkspace.Checked = true;
            }
            else
            {
                stvServerTreeView.LimitToWorkspace = false;
                menuInWorkspace.Checked = false;
            }
        }

        private void UpdateShowDeleted()
        {
            if (Program.Settings.ShowDeletedFiles)
            {
                stvServerTreeView.DeletedState = DeletedState.Any;
                menuShowDeleted.Checked = true;
            }
            else
            {
                stvServerTreeView.DeletedState = DeletedState.NonDeleted;
                menuShowDeleted.Checked = false;
            }
        }

        private int m_waitingCount = 0;

        public void StartWaiting()
        {
            m_waitingCount++;
            if (m_waitingCount > 0)
                statusBarProgress.Visible = true;
        }
 
        public void StopWaiting()
        {
            m_waitingCount--;
            if (m_waitingCount <= 0)
                statusBarProgress.Visible = false;
        }

        private void cbAddress_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (tcTrees.SelectedTab == tbServer)
            {
                if (VersionControlPath.IsValidPath(cbAddress.Text))
                    stvServerTreeView.Navigate(cbAddress.Text);
            }
            else
            {
                if (FileSpec.IsLegalNtfsName(cbAddress.Text, (int)PathLength.MaxLength, false))
                    ltvLocalTreeView.Navigate(cbAddress.Text);
            }
        }


        private void cbAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void cbAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (tcTrees.SelectedTab == tbServer)
                {
                    if (VersionControlPath.IsValidPath(cbAddress.Text))
                    {
                        stvServerTreeView.Navigate(cbAddress.Text);
                        cbAddress.Items.Add(cbAddress.Text);
                    }
                }
                else
                {
                    if (FileSpec.IsLegalNtfsName(cbAddress.Text, (int)PathLength.MaxLength, false))
                    {
                        ltvLocalTreeView.Navigate(cbAddress.Text);
                        cbAddress.Items.Add(cbAddress.Text);
                    }
                }
            }
        }

        private void tcTrees_Selected(object sender, TabControlEventArgs e)
        {
            if (tcTrees.SelectedTab == tbServer)
            {
                cbAddress.Text = stvServerTreeView.LastSelectedServerItem;
            }
            else
            {
                cbAddress.Text = ltvLocalTreeView.LastSelectedServerItem;
            }
        }

 


    }
}
