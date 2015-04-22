using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Xml.Serialization;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFV
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConfigurationManager.AppSettings.Set("EnableWindowsFormsHighDpiAutoResizing", "true");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Settings = Properties.Settings.Default;
			Settings.Reload();
            if (Settings.SavedConnnections == null)
                Settings.SavedConnnections = new SavedConnectionList();

            OpenConnection(null);
            Application.Run();
            return;
        }

        public static void OpenConnection(IWin32Window owner)
        {
            TfsTeamProjectCollection projectCollection = null;
            Workspace ws = null;
            using (OpenConnection ocDialog = new OpenConnection())
            {
                ocDialog.ShowDialog(owner);
                if (ocDialog.DialogResult == DialogResult.OK)
                {
                    projectCollection = ocDialog.OpenedCollection;
                    ws = ocDialog.SelectedWorkspace;
                }
            }

            if (projectCollection == null || ws == null)
                return;

            Settings.Save();
            MainForm mf = new MainForm(projectCollection, ws);
            mf.Show();
        }

        public static TFV.Properties.Settings Settings { get; private set; }
    }
}
