using Programatica.Saft.Models;
using System;
using System.Reflection;

namespace SAFT_Reader
{
    public static class Globals
    {
        public static AuditFile AuditFile { get; set; }
        public static string Filepath { get; set; }

        public static string VersionLabel
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return string.Format("Versão: {0}.{1}.{2}.{3} (NetworkDeployed)", 
                        ver.Major, ver.Minor, ver.Build, ver.Revision, 
                        Assembly.GetEntryAssembly().GetName().Name);
                }
                else
                {
                    var ver = Assembly.GetExecutingAssembly().GetName().Version;
                    return string.Format("Versão: {0}.{1}.{2}.{3} (Debug)", 
                        ver.Major, ver.Minor, ver.Build, ver.Revision, 
                        Assembly.GetEntryAssembly().GetName().Name);
                }
            }
        }
    }
}
