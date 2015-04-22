using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Controls;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.VisualStudio.Services.Common;

namespace TFV
{
    public partial class OpenConnection : Form
    {
        public OpenConnection()
        {
            InitializeComponent();
        }

        private TfsClientCredentials GetCredentials(Uri serverUri)
        {

            if (cbNTLM.Checked)
            {
                   return TfsClientCredentials.LoadCachedCredentials(serverUri, true, true);
            }
            else
            {
                string[] domainUser = tbUser.Text.Split('\\');
                NetworkCredential netCred = null;
                if (domainUser.Length > 1)
                    netCred = new NetworkCredential(domainUser[1], tbPassword.Text, domainUser[0]);
                else
                    netCred = new NetworkCredential(tbUser.Text, tbPassword.Text);
                BasicAuthCredential cred = new BasicAuthCredential(netCred);
                return new TfsClientCredentials(cred);
            }
        }

        private void btnConnections_Click(object sender, EventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            foreach (var setting in TFV.Program.Settings.SavedConnnections.Connections)
            {
                menu.MenuItems.Add(setting.ToString(), (o, innere) => { ApplyConnection(setting); });
            }
            menu.Show(btnConnections, new Point(0, btnConnections.Height));
        }

        public void ApplyConnection(SavedConnection connection)
        {
            cbNTLM.Checked = connection.UserName == null;
            tbUser.Text = connection.UserName != null ? connection.UserName : "";
            tbPassword.Text = "";
            tbServer.Text = connection.ProjectURL;
			tbWorkspace.Text = connection.Workspace;
        }

        private void OpenConnection_Load(object sender, EventArgs e)
        {
            var list = TFV.Program.Settings.SavedConnnections.Connections;
            if (list.Count > 0)
            {
                ApplyConnection(list[0]);
            }
            else
            {
                tbUser.Text = Environment.UserName;
            }
        }

        private void cbNTLM_CheckedChanged(object sender, EventArgs e)
        {
            tbUser.Enabled = !cbNTLM.Checked;
            tbPassword.Enabled = !cbNTLM.Checked;
        }

        private void btnBrowseWorkspace_Click(object sender, EventArgs e)
        {
            TfsTeamProjectCollection prj = ConnectAndCacheCredentials();
            if (prj != null)
            {
                SelectWorkspace ws = new SelectWorkspace(prj.GetService<VersionControlServer>(), tbWorkspace.Text);
                if (ws.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && ws.WSName != null)
                {
                    tbWorkspace.Text = ws.WSName.Name;
                }
            }
        }

        private TfsTeamProjectCollection ConnectAndCacheCredentials()
        {
            Uri serverUri = null;
            try
            {
                serverUri = new Uri(tbServer.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(this, "Invalid server URL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            TfsClientCredentials cred = GetCredentials(serverUri);
            TfsTeamProjectCollection cfgServer = new TfsTeamProjectCollection(serverUri, cred);
            try
            {
                cfgServer.Connect(ConnectOptions.IncludeServices);
                return cfgServer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Cannot login: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            TfsTeamProjectCollection prj = ConnectAndCacheCredentials();
            if (prj != null)
            {
                var vcs = prj.GetService<VersionControlServer>();
                try
                {
                    Workspace ws = vcs.GetWorkspace(tbWorkspace.Text, vcs.AuthorizedUser);
                    var list = TFV.Program.Settings.SavedConnnections.Connections;
                    SavedConnection sc = null;
                    foreach(var l in list)
                    {
                        if (new Uri(l.ProjectURL) == prj.Uri)
                        {
                            sc = l;
                            break;
                        }
                    }

                    if (sc == null)
                    {
                        sc = new SavedConnection();
                        list.Add(sc);
                    }

                    sc.ProjectURL = prj.Uri.ToString();
                    sc.Workspace = ws.Name;
                    sc.UserName = cbNTLM.Checked ? null : tbUser.Text;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    OpenedCollection = prj;
                    SelectedWorkspace = ws;
                    Close();

                }
                catch(ItemNotMappedException ex)
                {
                    MessageBox.Show(this, "Invalid workspace: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public TfsTeamProjectCollection OpenedCollection { get; private set; }
        public Workspace SelectedWorkspace { get; private set; }
    }


}
