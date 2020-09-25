using SAFT_Reader.UI;
using Syncfusion.Licensing;
using System;
using System.Windows.Forms;

namespace SAFT_Reader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SyncfusionLicenseProvider.RegisterLicense(FindLicenseKey());
            CompositionRoot.Wire(new ApplicationModule());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var openFileDialog = CompositionRoot.Resolve<OpenFileDialogForm>();
            Application.Run(openFileDialog);
        }

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
