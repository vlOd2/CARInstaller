using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace CARInstaller
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServicePointManager.Expect100Continue = true;
            // 768 = TLS 1.1
            // 3072 = TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | 
                (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
