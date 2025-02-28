using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace amd_reboot
{
    internal class Function
    {
        private static readonly List<string> ProcessList = new List<string>
        {
            "RadeonSoftware", "AMDRSServ", "amdow", "AMDRSSrcExt", "atiesrxx", "cncmd"
        };

        public static List<string> GetProcessList()
        {
            return ProcessList;
        }

        public static void IsRunning()
        {
            var runningProcesses = Process.GetProcesses()
                .Where(p => ProcessList.Contains(p.ProcessName, StringComparer.OrdinalIgnoreCase))
                .Select(p => p.ProcessName)
                .ToList();

            if (runningProcesses.Any())
            {
                Console.WriteLine("INFO: Running processes:");
                foreach (var process in runningProcesses)
                {
                    Console.WriteLine($"- {process}");
                }
            }
            else
            {
                Console.WriteLine("INFO: No monitored processes are running.");
            }
        }

        public static void KillProcess()
        {
            var runningProcesses = Process.GetProcesses()
                .Where(p => ProcessList.Contains(p.ProcessName, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (!runningProcesses.Any())
            {
                Console.WriteLine("INFO: No matching processes are running.");
                return;
            }

            foreach (var process in runningProcesses)
            {
                try
                {
                    process.Kill();
                    Console.WriteLine($"KILL: {process.ProcessName} has been terminated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: Failed to terminate {process.ProcessName}: {ex.Message}");
                }
            }

            // Ensure all processes have fully exited before proceeding
            bool allClosed = WaitForProcessesToClose(ProcessList, 5000);
            if (!allClosed)
            {
                Console.WriteLine("WARNING: Some AMD processes did not close properly.");
            }
        }

        public static void RestartAMD()
        {
            const string pathToEXE = @"C:\Program Files\AMD\CNext\CNext\RadeonSoftware.exe";
            const string processName = "RadeonSoftware";

            // Ensure all AMD processes are terminated before restarting
            bool allClosed = WaitForProcessesToClose(ProcessList, 5000);
            if (!allClosed)
            {
                Console.WriteLine("ERROR: Some AMD processes are still running! Restart aborted.");
                return;
            }

            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = pathToEXE,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Minimized
                };

                process.Start();
                Console.WriteLine("RESTART: AMD software is starting...");

                // Wait until the process fully starts
                bool started = WaitForProcessToStart(processName, 10000);
                if (started)
                {
                    Console.WriteLine("SUCCESS: AMD software is now running.");
                }
                else
                {
                    Console.WriteLine("ERROR: AMD software failed to start within timeout.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to start AMD software: {ex.Message}");
            }
        }

        private static bool WaitForProcessesToClose(List<string> processNames, int timeoutMs)
        {
            int elapsedTime = 0;
            while (elapsedTime < timeoutMs)
            {
                bool anyRunning = Process.GetProcesses()
                    .Any(p => processNames.Contains(p.ProcessName, StringComparer.OrdinalIgnoreCase));

                if (!anyRunning) return true; // All processes are closed

                Thread.Sleep(500);
                elapsedTime += 500;
            }
            return false; // Some processes are still running after timeout
        }

        private static bool WaitForProcessToStart(string processName, int timeoutMs)
        {
            int elapsedTime = 0;
            while (elapsedTime < timeoutMs)
            {
                if (Process.GetProcessesByName(processName).Any())
                    return true; // Process successfully started

                Thread.Sleep(500);
                elapsedTime += 500;
            }
            return false; // Process didn't start within timeout
        }
    }
}
