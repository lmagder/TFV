using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

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
            Settings = new Properties.Settings();
            Application.Run(new OpenConnection());
            Settings.Save();
        }

        public static TFV.Properties.Settings Settings { get; private set; }
    }
}
