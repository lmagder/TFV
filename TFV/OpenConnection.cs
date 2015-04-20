using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation;

namespace TFV
{
    public partial class OpenConnection : Form
    {
        public OpenConnection()
        {
            InitializeComponent();
        }

        private SimpleWebTokenCredential localCreds = null;

        private TfsClientCredentials GetCredentials(Uri serverUri)
        {
            if (cbNTLM.Checked)
            {
                localCreds = null;
                return TfsClientCredentials.LoadCachedCredentials(serverUri, true, true);
            }
            else
            {
                localCreds = new SimpleWebTokenCredential(tbUser.Text, tbPassword.Text);
                return new TfsClientCredentials(localCreds);
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
            localCreds = connection.Credentials;
            cbNTLM.Checked = localCreds == null;
            tbUser.Text = localCreds != null ? localCreds.UserName : "";
            tbPassword.Text = "";
            tbServer.Text = connection.ProjectURL.ToString();
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
            Uri serverUri = null;
            try
            {
                serverUri = new Uri(tbServer.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(this, "Invalid server URL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TfsClientCredentials cred = GetCredentials(serverUri);
            TfsConfigurationServer cfgServer = new TfsConfigurationServer(serverUri, cred);
            try
            {
                cfgServer.Connect(ConnectOptions.IncludeServices);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Cannot login: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
