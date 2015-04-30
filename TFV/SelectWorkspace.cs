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
using Microsoft.TeamFoundation.VersionControl.Controls;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation;
using System.Reflection;

namespace TFV
{
    public partial class SelectWorkspace : Form
    {
        VersionControlServer m_server;
        public Workspace WSName { get; private set; }
        public SelectWorkspace(VersionControlServer s, string curName)
        {
            m_server = s;
            InitializeComponent();
            Populate(curName);
        }
        private void Populate(string curName)
        {
            listView1.Items.Clear();
            var wsList = m_server.QueryWorkspaces(null, m_server.AuthorizedUser, cbThisMachine.Checked ? Environment.MachineName : null);
            foreach (var w in wsList)
            {
                var temp = new ListViewItem(new string[] { w.Name, w.Computer, w.Comment });
                temp.Tag = w;
                listView1.Items.Add(temp);
                if (w.Name == curName)
                    temp.Selected = true;
            }
            if (listView1.Items.Count > 0 && listView1.SelectedItems.Count == 0)
                listView1.Items[0].Selected = true;

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            if (listView1.SelectedItems.Count > 0)
                WSName = listView1.SelectedItems[0].Tag as Workspace;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public static void ShowWorkspaceDialog(VersionControlServer server, IWin32Window owner)
        {
            var wsListType = typeof(ControlWorkspaceSettings).Assembly.GetType("Microsoft.TeamFoundation.VersionControl.Controls.WorkspaceList", false);
            if (wsListType == null)
                return;

            var instanceProp = wsListType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            if (instanceProp == null)
                return;

            var setServerMethod = wsListType.GetMethod("SetServer");
            if (setServerMethod == null)
                return;

            var wsListInstance = instanceProp.GetValue(null);
            setServerMethod.Invoke(wsListInstance, new object[] { server });

            var helperType = typeof(ControlWorkspaceSettings).Assembly.GetType("Microsoft.TeamFoundation.VersionControl.Controls.ClientHelper", false);
            if (helperType == null)
                return;

            var method = helperType.GetMethod("ManageWorkspaces", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
                return;

            var constructor = helperType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
                return;

            var instance = constructor.Invoke(new object[] { });
            method.Invoke(instance, new object[] { server, owner });

            setServerMethod.Invoke(wsListInstance, new object[] { null });
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            ShowWorkspaceDialog(m_server, this);
            Populate(null);
        }

        private void cbThisMachine_CheckedChanged(object sender, EventArgs e)
        {
            Populate(null);
        }
    }
}
