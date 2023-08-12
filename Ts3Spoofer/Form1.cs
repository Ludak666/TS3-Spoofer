using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Ts3Spoofer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            // Specify the registry key path
            string keyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";

            try
            {
                // Generate a random ID in the format "*****-*****-*****-****"
                string randomID = GenerateRandomID();

                // Open the registry key with write access
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true))
                {
                    if (key != null)
                    {
                        // Set the new value for the "Product" key
                        key.SetValue("ProductId", randomID);

                        // Display a Windows notification with the new ID
                        ShowNotification("Your new Product ID is: " + randomID);
                    }
                    else
                    {
                        ShowNotification("Registry key not found");
                    }
                }

                // Delete directories using environment variables
                string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                string appDataRoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appDataLocalPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string userName = Environment.UserName;

                string[] directoriesToDelete = new string[]
                {
                    Path.Combine(programFilesPath, "TeamSpeak 3 Client"),
                    Path.Combine(appDataRoamingPath, "TS3Client"),
                    Path.Combine(appDataLocalPath, "TeamSpeak 3")
                };

                foreach (string directoryPath in directoriesToDelete)
                {
                    if (Directory.Exists(directoryPath))
                    {
                        Directory.Delete(directoryPath, true); // The second parameter 'true' enables recursive deletion
                        ShowNotification("Directory deleted successfully: " + directoryPath);
                    }
                    else
                    {
                        ShowNotification("Directory not found: " + directoryPath);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowNotification("An error occurred: " + ex.Message);
            }
        }

        private void ShowNotification(string message)
        {
            // Display a Windows notification with the specified message
            MessageBox.Show(message, "Product ID Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GenerateRandomID()
        {
            // Generate a random ID in the format "*****-*****-*****-****" using only 0 and 1
            Random random = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    char randomChar = (char)random.Next('0', '2'); // Only 0 and 1
                    sb.Append(randomChar);
                }

                if (i < 3)
                {
                    sb.Append('-');
                }
            }

            return sb.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                string exeUrl = "https://files.teamspeak-services.com/releases/client/3.6.1/TeamSpeak3-Client-win64-3.6.1.exe"; 
                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string downloadPath = Path.Combine(downloadsPath, "TeamSpeak3-Client-win64-3.6.1.exe");
                // Download the exe file
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(exeUrl, downloadPath);
                }

                // Run the downloaded exe file
                Process.Start(downloadPath);

                // Display a Windows notification indicating successful download and execution
                ShowNotification("Executable file downloaded and executed successfully.");
            }
            catch (Exception ex)
            {
                // Display an error message if download or execution fails
                ShowNotification("An error occurred: " + ex.Message);
            }
        }
    }
}
