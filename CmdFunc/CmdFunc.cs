﻿using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Groophy
{
    public class CmdFunc
    {
        private Process Shell;
        private ShellType Shell_Type;
        static bool DEBUG;
        static Dictionary<string, string> KeyFilteredVarriables = new Dictionary<string, string>();

        private void addorupdate(string key, string value)
        {
            try
            {
                KeyFilteredVarriables[key] = value;
            }
            catch
            {
                KeyFilteredVarriables.Add(key, value);
            }
        }
        public struct Grp
        {
            public int ProcId;
            public string Std_Input;
            public System.Text.StringBuilder Std_Out;
            public bool IsError;
            public Stopwatch Stopwatch;
            public bool probability_of_having_a_set;
            public string probability_of_having_a_set_VARRIABLE_KEY;

            public void Print(int std_out_limit = 30)
            {
                CmdFunc.Print(this, std_out_limit);
            }
        }

        private static string CopyChar(char chr, int len)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < len; i++) sb.Append(chr);
            return sb.ToString();
        }

        /// <summary>
        /// std_out_limit < 0 -> best width value for output
        /// </summary>
        /// <param name="grp_"></param>
        /// <param name="std_out_limit"></param>
        public static void Print(Grp grp_, int std_out_limit = -1)
        {
            if (std_out_limit < 0)
            {
                std_out_limit = Console.WindowWidth - 64;
            }

            Grp[] grp = new Grp[] { grp_ };

            Print(grp, std_out_limit);
        }

        /// <summary>
        /// std_out_limit < 0 -> best width value for output
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="std_out_limit"></param>
        public static void Print(Grp[] grp, int std_out_limit = -1)
        {
            if (std_out_limit < 0)
            {
                std_out_limit = Console.WindowWidth - 64;
            }

            Console.WriteLine("/-i---|-ID--|-Err-|-MS--|-Stdio--------------|-vp--|-vname----|-stdout---" + CopyChar('-', std_out_limit - 10) + "\\");

            for (int i = 0; i < grp.Length; i++)
            {
                Console.WriteLine("|" + subbysp(i.ToString(), 5, 5) +
                    "|" + subbysp(grp[i].ProcId.ToString(), 5, 5) +
                    "|" + subbysp(grp[i].IsError.ToString(), 5, 5) +
                    "|" + subbysp(grp[i].Stopwatch.Elapsed.TotalMilliseconds.ToString(), 5, 5) +
                    "|" + subbysp(grp[i].Std_Input.ToString(), 20, 20) +
                    "|" + subbysp(grp[i].probability_of_having_a_set.ToString(), 5, 5) +
                    "|" + subbysp(grp[i].probability_of_having_a_set_VARRIABLE_KEY.ToString(), 10, 10) +
                    "|" + subbysp(grp[i].Std_Out.ToString().Replace("\n", ",").Replace("\r", ""), std_out_limit, std_out_limit) +
                    "|");
            }
            Console.WriteLine("\\-----|-----|-----|-----|--------------------|-----|----------|" + CopyChar('-', std_out_limit) + "/");
        }

        private static string subbysp(string word, int len, int retlen)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            char[] crs = word.ToCharArray();

            for (int i = 0; i < retlen; i++)
            {
                if (i < len)
                {
                    if (crs.Length > i)
                    {
                        sb.Append(crs[i]);
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }
                else
                {
                    sb.Append(' ');
                }
            }

            return sb.ToString();
        }

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


        const int PROCESS_WM_READ = 0x0010;

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
            //Picobat_PBat
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingdir"></param>
        /// <param name="type"></param>
        public CmdFunc(string workingdir, ShellType type, bool _Debug)
        {
            DEBUG = _Debug;
            Shell_Type = type;
            if (type == ShellType.ChairmanandManagingDirector_CMD)
            {

                Shell = new Process();
                ProcessStartInfo si = new ProcessStartInfo("cmd.exe");
                si.Arguments = "/k";
                si.RedirectStandardInput = true;
                si.RedirectStandardOutput = true;
                si.RedirectStandardError = true;
                si.UseShellExecute = false;
                si.CreateNoWindow = true;
                //Control.CheckForIllegalCrossThreadCalls = false;
                si.WorkingDirectory = workingdir;
                Shell.StartInfo = si;
                Shell.OutputDataReceived += Shell_OutputDataReceived;
                Shell.ErrorDataReceived += Shell_ErrorDataReceived;
                Shell.Start();
                Shell.BeginErrorReadLine();
                Shell.BeginOutputReadLine();

                Input("echo init");
            }
            else if (type == ShellType.PowerShell_PS)
            {

                Shell = new Process();
                ProcessStartInfo si = new ProcessStartInfo("powershell.exe");
                si.RedirectStandardInput = true;
                si.RedirectStandardOutput = true;
                si.RedirectStandardError = true;
                si.UseShellExecute = false;
                si.CreateNoWindow = true;
                //Control.CheckForIllegalCrossThreadCalls = false;
                si.WorkingDirectory = workingdir;
                Shell.StartInfo = si;
                Shell.OutputDataReceived += Shell_OutputDataReceived;
                Shell.ErrorDataReceived += Shell_ErrorDataReceived;
                Shell.Start();
                Shell.BeginErrorReadLine();
                Shell.BeginOutputReadLine();

                Input("echo init");
            }
        }

        private void Shell_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (endflag)
            {
                mtn.Std_Out.Append(e.Data + "\n");
                if (!mtn.IsError) mtn.IsError = true;
            }
        }

        private void Shell_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == mtn.Std_Input || string.IsNullOrEmpty(e.Data))
            {
                return;
            }
            try
            {
                if (Shell_Type == ShellType.ChairmanandManagingDirector_CMD)
                {
                    if (e.Data.Split(new[] { "@echo " }, StringSplitOptions.None)[1] == "End-Flag-ID-Null")
                    {
                        endflag = false;
                        return;
                    }
                }
                else if (Shell_Type == ShellType.PowerShell_PS)
                {
                    if (e.Data.Split(new[] { "echo " }, StringSplitOptions.None)[1] == "End-Flag-ID-Null")
                    {
                        endflag = false;
                        return;
                    }
                }
            }
            catch { }

            try
            {
                if (Shell_Type == ShellType.ChairmanandManagingDirector_CMD)
                {
                    int spt = e.Data.IndexOf(">");
                    string nv = e.Data.Substring(spt + 1);
                    if (string.Equals(nv, mtn.Std_Input)) return;
                    if (e.Data.Substring(0, 16) == "End-Flag-ID-Null") return;
                }
                else if (Shell_Type == ShellType.PowerShell_PS)
                {
                    if (e.Data.StartsWith("PS C:") || e.Data.Substring(0, 16) == "End-Flag-ID-Null")
                    {
                        return;
                    }
                }
            }
            catch { }

            if (endflag)
            {
                mtn.Std_Out.Append(e.Data + "\n");
            }
        }

        public Dictionary<string, string> GetAllVarriable(bool justFiltered = false)
        {
            if (justFiltered)
            {
                return KeyFilteredVarriables;
            }

            Dictionary<string, string> vars = new Dictionary<string, string>();

            string[] currentData = Input("@set").Std_Out.ToString().Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < currentData.Length; i++)
            {
                if (!string.IsNullOrEmpty(currentData[i])) vars.Add(currentData[i].Split('=')[0], currentData[i].Split('=')[1]);
            }

            foreach (var x in KeyFilteredVarriables)
            {
                try
                {
                    vars.Add(x.Key, x.Value);
                }
                catch
                {
                    vars[x.Key] = x.Value;
                }
            }

            return vars;
        }

        public string GetVarriable(string varname)
        {
            var value = Input("@echo %" + varname + "%").Std_Out.ToString();
            if (value.EndsWith("\n")) return value.Substring(0, value.Length - 1);
            return value;
        }

        public Grp SetVarriable(string varname, string value)
        {
            string sla = '"'.ToString();
            return Input("set " + sla + varname + "=" + value + sla);
        }

        public void SetQuickEdit(QuickEditMode _mode)
        {
            IntPtr handle = GetStdHandle(-10);
            int mode;
            GetConsoleMode(handle, out mode);
            bool flag = _mode == QuickEditMode.ENABLE_QUICK_EDIT_MODE;
            if (flag)
            {
                SetConsoleMode(handle, mode | 192);
            }
            else
            {
                bool flag2 = _mode == QuickEditMode.DISABLE_QUICK_EDIT;
                if (flag2)
                {
                    SetConsoleMode(handle, mode &= -65);
                }
            }
        }

        public bool GetQuickEdit()
        {
            IntPtr handle = GetStdHandle(-10);
            int mode;
            GetConsoleMode(handle, out mode);
            return mode == (mode | 192);
        }

        public string GetConsoleTitle() => Shell.MainWindowTitle;

        [Obsolete]
        public int GetNonpagedSystemMemorySize() => Shell.NonpagedSystemMemorySize;

        public long GetNonpagedSystemMemorySize64() => Shell.NonpagedSystemMemorySize64;

        [Obsolete]
        public int GetPagedMemorySize() => Shell.PagedMemorySize;

        public long GetPagedMemorySize64() => Shell.PagedMemorySize64;

        [Obsolete]
        public int GetPagedSystemMemorySize() => Shell.PagedSystemMemorySize;

        public long GetPagedSystemMemorySize64() => Shell.PagedSystemMemorySize64;

        [Obsolete]
        public int GetPeakPagedMemorySize() => Shell.PeakPagedMemorySize;

        public long GetPeakPagedMemorySize64() => Shell.PeakPagedMemorySize64;

        [Obsolete]
        public int GetPeakVirtualMemorySize() => Shell.PeakVirtualMemorySize;

        public long GetPeakVirtualMemorySize64() => Shell.PeakVirtualMemorySize64;

        [Obsolete]
        public int GetPrivateMemorySize() => Shell.PrivateMemorySize;

        public long GetPrivateMemorySize64() => Shell.PrivateMemorySize64;

        [Obsolete]
        public int GetVirtualMemorySize() => Shell.VirtualMemorySize;

        public long GetVirtualMemorySize64() => Shell.VirtualMemorySize64;

        public string GetCulture() => FindCulture();

        private string FindCulture()
        {
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, Shell.Id);

            int bytesRead = 0;
            byte[] buffer = new byte[10]; //'tr-TR' takes 5*2 bytes because of Unicode 
            ReadProcessMemory((int)processHandle, 0x00A02930, buffer, buffer.Length, ref bytesRead);
            return System.Text.Encoding.Unicode.GetString(buffer);
        }

        static void log(string text)
        {
            if (DEBUG) Console.WriteLine(text);
        }

        public string filter(string script)
        {
            bool probability_of_having_a_set = script.ToLower().Contains("set");

            if (probability_of_having_a_set)
            {
                int probability_of_having_a_set_index = script.IndexOf("=");

                if (probability_of_having_a_set_index != -1) //if have = char
                {
                    probability_of_having_a_set_index--; //remove '='

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    for (int i = probability_of_having_a_set_index; ; i--)
                    {
                        if (script[i] == '"' || script[i] == ' ')
                        {
                            break;
                        }
                        else
                        {
                            sb.Insert(0, script[i]);
                        }
                        if (i < 2) break;
                    }

                    pohas = true;
                    pohas_Key = sb.ToString();
                }
            }

            bool probability_of_having_a_call_function = script.ToLower().Contains("call"); 

            if (probability_of_having_a_call_function)
            {
                string[] parts = script.Trim().Split(" ");

                // 0  -> call function
                // 1  -> path
                // +1 -> parameters

                if (parts[1].StartsWith("\"")) parts[1] = parts[1].Substring(1);
                if (parts[1].EndsWith("\"")) parts[1] = parts[1].Substring(0, parts[1].Length-1);

                if (getExtentionofFile(parts[1]).ToLower() != "bat" && File.Exists(parts[1]))
                {
                    string[] lines = File.ReadAllLines(parts[1]);

                    pohacf_Keys = GetKeys(lines);

                    if (pohacf_Keys.Count > 0) pohacf = true;
                }
            }

            return script;
        }

        public string getExtentionofFile(string path)
        {
            string[] parts = path.Trim().Split(".");
            return parts[parts.Length - 1];
        }

        public List<string> GetKeys(string[] lines)
        {
            List<string> keys = new List<string>(); 
            for (int i2 = 0;i2 < lines.Length;i2++)
            {
                string script = lines[i2];

                bool probability_of_having_a_set = script.ToLower().Contains("set");

                if (probability_of_having_a_set)
                {
                    int probability_of_having_a_set_index = script.IndexOf("=");

                    if (probability_of_having_a_set_index != -1) //if have = char
                    {
                        probability_of_having_a_set_index--; //remove '='

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        for (int i = probability_of_having_a_set_index; ; i--)
                        {
                            if (script[i] == '"' || script[i] == ' ')
                            {
                                break;
                            }
                            else
                            {
                                sb.Insert(0, script[i]);
                            }
                            if (i < 2) break;
                        }

                        keys.Add(sb.ToString());
                    }
                }
            }
            return keys;
        }

        public void checkVarriable(string script)
        {
            if (pohas)
            {
                string key = pohas_Key;
                string value = GetVarriable(pohas_Key);
                addorupdate(key, value);
            }
            if (pohacf)
            {
                List<string> keys = pohacf_Keys;
                for (int i = 0;i < keys.Count;i++)
                {
                    if (string.IsNullOrEmpty(keys[i])) continue;
                    string key = keys[i];
                    string value = GetVarriable(key);
                    addorupdate(key, value);
                }
            }
        }

        public Grp Input(string script)
        {
            var r = CInput(script);
            checkVarriable(script);
            return r;
        }

        public Grp[] Input(string[] script)
        {
            Grp[] grps = new Grp[script.Length];
            for (int i = 0;i < script.Length;i++)
            {
                var r = CInput(script[i]);
                checkVarriable(script[i]);
                grps[i] = r;
            }
            return grps;
        }

        bool endflag = false;
        bool pohas = false; //probability_of_having_a_set
        string pohas_Key = string.Empty;
        bool pohacf = false; //probability_of_having_a_call_function
        List<string> pohacf_Keys = new List<string>();
        Grp mtn = new Grp();
        private Grp CInput(string command)
        {
            log("new script => " + command);
            pohas = false;
            pohas_Key = string.Empty;
            pohacf = false;
            pohacf_Keys = new List<string>();
            command = filter(command);
            if (string.IsNullOrEmpty(command)) return new Grp() { IsError = false, ProcId = Shell.Id, Std_Input = "", Std_Out = new System.Text.StringBuilder(), Stopwatch = new Stopwatch() };

            mtn.Std_Input = command;
            mtn.Std_Out = new System.Text.StringBuilder();
            mtn.IsError = false;
            mtn.ProcId = Shell.Id;
            mtn.Stopwatch = new Stopwatch();
            mtn.probability_of_having_a_set = pohas;
            mtn.probability_of_having_a_set_VARRIABLE_KEY = pohas_Key;
            mtn.Stopwatch.Start();

            endflag = true;
            Shell.StandardInput.WriteLine(command);
            if (Shell_Type == ShellType.ChairmanandManagingDirector_CMD) Shell.StandardInput.WriteLine("@echo End-Flag-ID-Null");
            else if (Shell_Type == ShellType.PowerShell_PS) Shell.StandardInput.WriteLine("echo End-Flag-ID-Null");

            while (endflag) { }

            string[] lines = mtn.Std_Out.ToString().Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.None);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    sb.Append(lines[i] + Environment.NewLine);
                }
            }
            endflag = false;

            mtn.Stopwatch.Stop();

            try
            {
                return mtn;
            }
            finally
            {
                mtn = default;
            }
        }

        public static Grp OneTimeInput(string script, ShellType type, string workingDir, bool _DEBUG = false)
        {
            if (string.IsNullOrEmpty(workingDir)) workingDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            switch (type)
            {
                case ShellType.ChairmanandManagingDirector_CMD:

                    CmdFunc c = new CmdFunc(workingDir, ShellType.ChairmanandManagingDirector_CMD, _DEBUG);
                    return c.Input(script);
                case ShellType.PowerShell_PS:

                    CmdFunc p = new CmdFunc(workingDir, ShellType.PowerShell_PS, _DEBUG);
                    return p.Input(script);
                default:

                    throw new ArgumentNullException();
            }
        }
    }
}
