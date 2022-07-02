using System.Runtime.InteropServices;

namespace Groophy
{
    internal class Structes
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll")]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int mode);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int handle);

        public const int STD_INPUT_HANDLE = -10;
        public const int ENABLE_QUICK_EDIT_MODE = 0x40 | 0x80;
        public const int DISABLE_QUICK_EDIT_MODE = 0x0080;
        public const int CHECK_QUICK_EDIT = 0x0040;

        public enum QuickEditMode
        {
            ENABLE_QUICK_EDIT_MODE = 0x40 | 0x80,
            DISABLE_QUICK_EDIT = 0x0080
        };
    }
}
