using System.Runtime.InteropServices;
using System;

namespace Groophy
{
    public class Structes
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll")]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int mode);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int handle);

        //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        public static int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
          int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        const uint WM_KEYDOWN = 0x0100;

        public const int STD_INPUT_HANDLE = -10;
        public const int ENABLE_QUICK_EDIT_MODE = 0x40 | 0x80;
        public const int DISABLE_QUICK_EDIT_MODE = 0x0080;
        public const int CHECK_QUICK_EDIT = 0x0040;

        public enum QuickEditMode
        {
            ENABLE_QUICK_EDIT_MODE = 0x40 | 0x80,
            DISABLE_QUICK_EDIT = 0x0080
        };

        public enum ShellType
        {
            ChairmanandManagingDirector_CMD,
            PowerShell_PS
            
            //may soon Picobat_PBat
        };
    }
}
