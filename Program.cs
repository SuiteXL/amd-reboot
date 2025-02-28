using System;
using System.Threading;

namespace amd_reboot
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== AMD Process Manager ===\n");

            Function.IsRunning();
            Console.WriteLine();
            Function.KillProcess();
            Console.WriteLine();
            Function.RestartAMD();

            Console.WriteLine("\nShutting down in 3 seconds...");
            Thread.Sleep(3000);
        }
    }
}
