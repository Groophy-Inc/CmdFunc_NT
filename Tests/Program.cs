using System;
using System.Diagnostics;

namespace Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Groophy.CmdFunc c = new Groophy.CmdFunc("C:\\", Groophy.Structes.ShellType.ChairmanandManagingDirector_CMD, true);

            var x = c.Input(new string[]{
                "call \"C:\\Users\\GROOPHY\\Desktop\\test.bat\"",
        });

            Groophy.Utils.Print(x);

            var p = c.GetAllVarriable(true);

            foreach (var item in p)
            {
                Console.WriteLine(item.Key+": " + item.Value);
            }
        }
    }
}