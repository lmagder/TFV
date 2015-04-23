using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Xml.Serialization;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Runtime.InteropServices;
using Microsoft.TeamFoundation.Client;

namespace TFV
{
    static class Natives
    {
        public enum PROCESS_DPI_AWARENESS
        {
            Process_DPI_Unaware = 0,
            Process_System_DPI_Aware = 1,
            Process_Per_Monitor_DPI_Aware = 2
        }
        [DllImport("user32.dll", PreserveSig = false, ExactSpelling = true, EntryPoint="SetProcessDpiAwarenessInternal")] //This is a #define in Windows.h
        public static extern void SetProcessDPIAwareness(PROCESS_DPI_AWARENESS awareness);

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool SetProcessDPIAware();
        
        public static System.Version Win81 = new Version(6,3);
        public static System.Version WinVista = new Version(6,0);
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version >= Natives.Win81)
            {
                Natives.SetProcessDPIAwareness(Natives.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            }
            else if (Environment.OSVersion.Version >= Natives.WinVista)
            {
                Natives.SetProcessDPIAware();
            }

            ConfigurationManager.AppSettings.Set("EnableWindowsFormsHighDpiAutoResizing", "true");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Settings = Properties.Settings.Default;
			Settings.Reload();
            if (Settings.SavedConnnections == null)
                Settings.SavedConnnections = new SavedConnectionList();

            NotificationManager.Initialize();

            if (!OpenConnection(null))
                return;

            Application.Run();
            NotificationManager.Shutdown();
            Settings.Save();
            return;
        }

        static void NotificationHandler(Notification notification, IntPtr param1, IntPtr param2);

        public static bool OpenConnection(IWin32Window owner)
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
                return false;

            Settings.Save();
            MainForm mf = new MainForm(projectCollection, ws);
            mf.Show();
            return true;
        }

        public static TFV.Properties.Settings Settings { get; private set; }
    }
}
