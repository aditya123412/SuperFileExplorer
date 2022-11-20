using System.Runtime.InteropServices;

namespace Classes.Common
{

    internal static class NativeMethods
    {
        internal const int WM_HOTKEY = 786;
        internal const int DWM_BB_ENABLE = 1;
        internal const int HSHELL_WINDOWACTIVATED = 4;
        internal const int HSHELL_WINDOWCREATED = 1;
        internal const int HSHELL_WINDOWDESTROYED = 2;
        internal const int MAX_PATH = 0xff;
        internal const int MAX_TRANSPARANCY = 255;
        internal const int GWL_EXSTYLE = -20;
        internal const int WS_EX_LAYERED = 0x80000;
        internal const int LWA_ALPHA = 2;


        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int DeregisterShellHookWindow(IntPtr hwnd);
        [DllImport("dwmapi.dll")]
        internal static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("dwmapi.dll", PreserveSig = false)]
        internal static extern bool DwmIsCompositionEnabled();
        [DllImport("psapi")]
        internal static extern int EmptyWorkingSet(IntPtr handle);
        [DllImport("user32.dll")]
        internal static extern int EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegisterShellHookWindow(IntPtr hwnd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int RegisterWindowMessage(string name);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        internal static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool IsWindow(IntPtr hWnd);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        internal static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, MARGINS pMargins);


        internal enum DWM_BB
        {
            BlurRegion = 2,
            Enable = 1,
            TransitionMaximized = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MARGINS
        {
            public int cxLeftWidth, cxRightWidth,
                       cyTopHeight, cyBottomHeight;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DWM_BLURBEHIND
        {
            public NativeMethods.DWM_BB dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTransitionOnMaximized;
        }

        internal delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);

    }
}