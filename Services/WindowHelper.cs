using System.Runtime.InteropServices;

namespace QuickLogin.Services
{
    public static class WindowHelper
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private const int SW_RESTORE = 9;

        /// <summary>
        /// Switch to a window by its exact title
        /// </summary>
        public static bool FocusWindowByTitle(string windowTitle)
        {
            var hWnd = FindWindow(null, windowTitle);
            if (hWnd == IntPtr.Zero) return false;

            ShowWindow(hWnd, SW_RESTORE);   // Restore if minimized
            return SetForegroundWindow(hWnd); // Bring to front
        }
    }
}
