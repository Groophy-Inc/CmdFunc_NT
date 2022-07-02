using System;

namespace Groophy
{
    public class Utils
    {
        /// <summary>
        /// std_out_limit < 0 -> best width value for output
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="std_out_limit"></param>
        public static void Print(CmdFunc.Grp[] grp, int std_out_limit = -1)
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

        private static string CopyChar(char chr, int len)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < len; i++) sb.Append(chr);
            return sb.ToString();
        }
    }
}
