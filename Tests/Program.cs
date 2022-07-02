using System;

namespace Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Groophy.CmdFunc c = new Groophy.CmdFunc("C:\\", Groophy.CmdFunc.ShellType.ChairmanandManagingDirector_CMD, false);

            c.Input("echo Hello World").Print();
        }
    }
}