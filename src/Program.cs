using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SAFT_Reader.UI;

using Syncfusion.Licensing;

namespace SAFT_Reader
{

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Register Syncfusion license using the found license key.
            SyncfusionLicenseProvider.RegisterLicense(FindLicenseKey());

            // Wire up the application's dependencies using CompositionRoot.
            CompositionRoot.Wire(new ApplicationModule());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize the list of attached files.
            Globals.AttachedFiles = new List<AttachedFile>();

            // Show a splash screen.
            var splash = CompositionRoot.Resolve<SplashForm>();
            splash.IsSplash = true;
            splash.ShowDialog();

            // Initialize and run the main file dialog.
            var openFileDialog = CompositionRoot.Resolve<OpenFileDialogForm>();
            Application.Run(openFileDialog);
        }

        /// <summary>
        /// Find the Syncfusion license key in the application directory.
        /// </summary>
        /// <returns>The Syncfusion license key as a string.</returns>
        public static string FindLicenseKey()
        {
            string licenseKeyFile = "SyncfusionLicense.txt";
            for (int n = 0; n < 20; n++)
            {
                if (!System.IO.File.Exists(licenseKeyFile))
                {
                    licenseKeyFile = @"..\" + licenseKeyFile;
                    continue;
                }
                return System.IO.File.ReadAllText(licenseKeyFile);
            }
            return string.Empty;
        }
    }
}