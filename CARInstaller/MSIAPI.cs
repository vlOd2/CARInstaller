using System;
using System.Runtime.InteropServices;

namespace CARInstaller
{
    public class MSIAPI
    {
        public const uint UI_LEVEL_BASIC = 3;
        public const uint UI_LEVEL_HIDE_CANCEL = 32;

        [DllImport("msi.dll", SetLastError = true)]
        public static extern uint MsiSetInternalUI(uint dwUILevel, ref IntPtr phWnd);

        [DllImport("msi.dll", EntryPoint = "MsiInstallProductA", SetLastError = true)]
        public static extern uint MsiInstallProduct(string szPackagePath, string szCommandLine);
    }
}
