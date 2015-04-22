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
            s_instanceCount++;
        }

        private void openConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.OpenConnection(this);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            s_instanceCount--;
            if (s_instanceCount == 0)
            {
                Application.Exit();
            }
        }
    }
}
