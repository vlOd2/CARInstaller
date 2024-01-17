using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARInstaller
{
    public static class JavaUtils
    {
        public static string GetJavaHome() 
        {
            string programFiles = Environment.Is64BitOperatingSystem ?
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) :
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Path.Combine(programFiles, "Java", "jre-1.8");
        }

        public static bool IsJavaInstalled() 
        {
            string javaExe = Path.Combine(GetJavaHome(), "bin", "java.exe");

            if (!File.Exists(javaExe)) 
                return false;

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = javaExe,
                Arguments = "-version",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            process.WaitForExit(1000);

            return process.ExitCode == 0;
        }
    }
}
