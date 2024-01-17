using CARInstaller.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CARInstaller
{
    public partial class MainForm : Form
    {
        public const string JRE_PART_DOWNLOAD_LINK = "https://github.com/vlOd2/vlOd2.github.io/raw/" +
            "main/car-java/Java.zip";
        public const string CHROMIUM_DOWNLOAD_LINK = "https://www.googleapis.com/download/storage/" +
            "v1/b/chromium-browser-snapshots/o/Win%2F330231%2Fmini_installer.exe?alt=media";
        private bool isInstalling;

        public MainForm()
        {
            InitializeComponent();
        }

        private DialogResult ShowInstallWarning() 
        {
            return MessageBox.Show($"This installer is PROVIDED \"as-is\" with no warranty!{Environment.NewLine}" +
                $"We ARE NOT responsible for ANY damages caused by using this software{Environment.NewLine}{Environment.NewLine}" +
                $"It is recommended that you do not interfere " +
                $"with the installer whilst it's working!{Environment.NewLine}" +
                $"It is also recommended to close other running apps!{Environment.NewLine}{Environment.NewLine}" +
                $"Do you wish to proceed with the installation?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;

            if (Assembly.GetEntryAssembly().Location.IndexOf(
                Path.GetTempPath(), StringComparison.OrdinalIgnoreCase) == 0) 
            {
                MessageBox.Show($"The installer is running from a temporary directory!{Environment.NewLine}" +
                    $"This means you are running it inside your archiving software!{Environment.NewLine}" +
                    $"Make sure to extract the installer properly and try again!", "FATAL ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                isInstalling = false;
                Close();
                return;
            }

            if (!File.Exists(Path.Combine("7z", "7z.exe")) || !File.Exists(Path.Combine("7z", "7z.dll")))
            {
                MessageBox.Show($"The installer is unable to find the required components!{Environment.NewLine}" +
                    $"Either you misplaced the installer, or you are running it inside your archiving software!{Environment.NewLine}" +
                    $"Make sure to extract the installer properly and try again!", "FATAL ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                isInstalling = false;
                Close();
                return;
            }

            if (ShowInstallWarning() != DialogResult.Yes) 
            {
                isInstalling = false;
                Close();
                return;
            }

            await StartInstall();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isInstalling) 
                e.Cancel = true;
        }

        public void WriteLog(string msg) 
        {
            WriteLog(msg, Color.Black);
        }

        public void WriteLog(string msg, Color color)
        {
            Invoke(new Action(() =>
            {
                rtxtlogs.SelectionStart = rtxtlogs.Text.Length;
                rtxtlogs.SelectionColor = color;
                rtxtlogs.SelectedText = msg + Environment.NewLine;
                rtxtlogs.SelectionColor = rtxtlogs.ForeColor;
            }));
        }

        private string GetLocalLow() 
        {
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(local, "..", "LocalLow");
        }

        private void ResetProgress() 
        {
            Invoke(new Action(() => 
            {
                lProgressStatus.Text = "Not doing anything!";
                pbProgress.Value = 0;
                pbProgress.Maximum = 100;
                pbProgress.Style = ProgressBarStyle.Blocks;
            }));
        }

        private async Task<bool> DownloadFile(string url, string output, string friendlyName)
        {
            WriteLog($"Downloading {friendlyName} from {url}...");
            ResetProgress();
            lProgressStatus.Text = $"Downloading {friendlyName}...";

            return await Task.Run(() =>
            {
                bool ended = false;
                bool result = false;
                WebClient client = new WebClient();

                client.DownloadProgressChanged += (sender, e) => Invoke(
                    new Action(() => pbProgress.Value = e.ProgressPercentage));
                client.DownloadFileCompleted += (sender, e) =>
                {
                    ended = true;
                    result = false;
                    ResetProgress();

                    if (e.Error != null)
                        WriteLog($"Unable to download {friendlyName}: {e.Error}", Color.Red);
                    else
                    {
                        result = true;
                        WriteLog($"Downloaded {friendlyName} sucessfully!");
                    }
                };

                client.DownloadFileAsync(new Uri(url), output);
                while (!ended) { Thread.Sleep(1); }

                return result;
            });
        }

        private async Task<bool> ExtractJavaInstaller() 
        {
            ResetProgress();
            pbProgress.Style = ProgressBarStyle.Marquee;
            lProgressStatus.Text = "Extracting Java installer...";
            WriteLog("Extracting Java installer...");

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine("7z", "7z.exe"),
                Arguments = "-aoa x _Java.zip.001",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            process.Start();

            await Task.Run(() => process.WaitForExit(10000));
            WriteLog(process.StandardOutput.ReadToEnd());

            if (!File.Exists("Java.msi"))
            {
                isInstalling = false;
                ResetProgress();
                WriteLog("Failed to extract the Java installer, aborting installation...", Color.Red);
                return false;
            }

            return true;
        }

        private bool InstallJava() 
        {
            ResetProgress();
            pbProgress.Style = ProgressBarStyle.Marquee;
            lProgressStatus.Text = "Installing Java...";
            WriteLog("Installing Java...");

            IntPtr phWnd = IntPtr.Zero;
            MSIAPI.MsiSetInternalUI(MSIAPI.UI_LEVEL_BASIC + MSIAPI.UI_LEVEL_HIDE_CANCEL, ref phWnd);
            uint msiResult = MSIAPI.MsiInstallProduct("Java.msi", "");

            if (msiResult != 0)
            {
                isInstalling = false;
                ResetProgress();
                WriteLog($"Failed to install Java (error {msiResult}), aborting installation...", Color.Red);
                return false;
            }

            return true;
        }

        private async Task InstallChromium() 
        {
            ResetProgress();
            pbProgress.Style = ProgressBarStyle.Marquee;
            lProgressStatus.Text = "Installing Chromium...";
            WriteLog("Installing Chromium...");

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = "Chromium.exe",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
            await Task.Run(() => process.WaitForExit());
        }

        private async Task StartInstall() 
        {
            try 
            {
                isInstalling = true;
                WriteLog("Starting installation...");
                bool javaInstalled = JavaUtils.IsJavaInstalled();
                bool result = await DownloadFile(CHROMIUM_DOWNLOAD_LINK, "Chromium.exe", "Chromium 44 x86");

                if (javaInstalled)
                    WriteLog("Java appears to be already installed", Color.DarkOrange);
                else
                {
                    result &= await DownloadFile(JRE_PART_DOWNLOAD_LINK + ".001", "_Java.zip.001", "Java JRE x86 (part 1)");
                    result &= await DownloadFile(JRE_PART_DOWNLOAD_LINK + ".002", "_Java.zip.002", "Java JRE x86 (part 2)");
                    result &= await DownloadFile(JRE_PART_DOWNLOAD_LINK + ".003", "_Java.zip.003", "Java JRE x86 (part 3)");
                }

                if (!result)
                {
                    WriteLog("Not everything was downloaded successfully, aborting installation...", Color.Red);
                    isInstalling = false;
                    return;
                }

                if (!javaInstalled && !await ExtractJavaInstaller()) return;
                if (!javaInstalled && !InstallJava()) return;
                await InstallChromium();

                ResetProgress();
                pbProgress.Style = ProgressBarStyle.Marquee;
                lProgressStatus.Text = "Cleaning up...";
                WriteLog("Cleaning up installers...");
                File.Delete("Chromium.exe");
                File.Delete("_Java.zip.001");
                File.Delete("_Java.zip.002");
                File.Delete("_Java.zip.003");
                File.Delete("Java.msi");

                ResetProgress();
                lProgressStatus.Text = "Configuring Java...";
                pbProgress.Maximum = 2;

                WriteLog("Configuring Java policy...");
                string javaPolicy = Path.Combine(JavaUtils.GetJavaHome(),
                    "lib", "security", "java.policy");
                File.AppendAllText(javaPolicy, Resources.JAVA_POLICY);
                pbProgress.Value++;

                WriteLog("Configuring Java applet exceptions...");
                string javaExceptions = Path.Combine(GetLocalLow(), "Sun", "Java",
                    "Deployment", "security", "exception.sites");
                File.WriteAllText(javaExceptions, Resources.EXCEPTION_SITES);
                pbProgress.Value++;

                ResetProgress();
                WriteLog("Done.");
                MessageBox.Show("Successfully installed and configured the prerequisites!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                isInstalling = false;
            }
            catch (Exception ex) 
            {
                isInstalling = false;
                for (int i = 0; i < 2; i++) 
                {
                    WriteLog($"!!! ENCOUNTERED UNKNOWN FATAL ERROR: {ex}", Color.Red);
                    WriteLog($"!!! PLEASE REPORT THIS IMMEDIATELY BY ATTACHING THIS LOG", Color.Red);
                    WriteLog($"!!! THE INSTALLER IS UNABLE TO CLEAN-UP, YOU WILL NEED TO DO IT YOURSELF", Color.Red);
                }
                MessageBox.Show($"ENCOUNTERED UNKNOWN FATAL ERROR{Environment.NewLine}" +
                    $"PLEASE REPORT THIS IMMEDIATELY BY ATTACHING THE INSTALLER LOG{Environment.NewLine}" +
                    $"THE INSTALLER IS UNABLE TO CLEAN-UP, YOU WILL NEED TO DO IT YOURSELF", "FATAL ERROR", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
