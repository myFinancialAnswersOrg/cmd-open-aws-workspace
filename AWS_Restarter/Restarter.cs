using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWS_Restarter.Utilities;

namespace AWS_Restarter
{
    class Restarter
    {
        public string registrationCode { get; set; }
        private string checksumFilename = "checksum.cfg";
        private string registrationCodeFilename = "code.cfg";
        private string configFilename = "config.cfg";
        private string credentialsFilename = "cred_storage_settings.ini";
        private string cachedUrlFilename = "wdurl.cfg";
        private string amazonRelativePath = @"Amazon Web Services\Amazon WorkSpaces";

        public Restarter(string registrationCode)
        {
            this.registrationCode = registrationCode;
        }

        public void Restart(string organizationName, string username, string password)
        {
            DeleteCachedFiles();
            WriteRegistrationCode();
            WriteGeneralSettings();
            WriteOrganizationName(organizationName);
            RunLoginCommand(username, password);
        }

        private string AmazonLocalAppDataPath()
        {
            string localAppData = Environment.GetEnvironmentVariable("LocalAppData");
            return String.Format("{0}\\{1}", localAppData, amazonRelativePath);
        }

        private void DeleteCachedFiles()
        {
            string localPath = AmazonLocalAppDataPath();
            string cachedUrlFilepath = String.Format("{0}\\{1}", localPath, this.cachedUrlFilename);
            string checksumFilepath = String.Format("{0}\\{1}", localPath, this.checksumFilename);
            string credentialsFilepath = String.Format("{0}\\{1}", localPath, this.credentialsFilename);
            if (File.Exists(cachedUrlFilepath)) File.Delete(cachedUrlFilepath);
            if (File.Exists(checksumFilepath)) File.Delete(checksumFilepath);
            if (File.Exists(credentialsFilepath)) File.Decrypt(credentialsFilepath);
        }

        private void WriteGeneralSettings()
        {
            string credentialsPath = String.Format("{0}\\{1}", AmazonLocalAppDataPath(), this.credentialsFilename);
            string[] content = new string[] {
                "[General]", 
                "FirstLaunchSaveCredentialPreference=false", 
                "SaveCredentialPreference=false"
            };
            File.WriteAllLines(credentialsPath, content);
        }

        private void WriteOrganizationName(string organizationName)
        {
            string configPath = String.Format("{0}\\{1}", AmazonLocalAppDataPath(), this.configFilename);
            string content = String.Format("{{ \"organizationName\":\"{0}\", \"endpointcode\":\"{1}\" }}", organizationName, Settings.Get("ENDPOINT"));
            File.WriteAllText(configPath, content);
        }

        private void WriteRegistrationCode()
        {
            string filepath = String.Format("{0}\\{1}", AmazonLocalAppDataPath(), this.registrationCodeFilename);
            System.IO.File.WriteAllText(filepath, this.registrationCode);
        }

        private void RunLoginCommand(string username, string password)
        {
            string programFilesDir = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            string exePath = @"Amazon Web Services, Inc\Amazon WorkSpaces\workspaces.exe";
            string options = String.Format("-u {0} -p {1} --login-mode 1", username, password);
            string command = String.Format("{0}\\{1} {2}", programFilesDir, exePath, options);

            Process proc = Process.Start(String.Format("{0}\\{1}", programFilesDir, exePath), options);
            proc.WaitForExit(60000);
            proc.Kill();
        }
    }
}
