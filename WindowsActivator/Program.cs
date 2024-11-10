using System;
using System.Diagnostics;
using System.Security.Principal;

namespace WindowsActivator
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            if (!IsAdministrator())
            {
                // Restart as admin if not already running as administrator
                RestartAsAdmin();
                return;
            }

            Menu.DrawMainScreen(); // Start main menu if running as admin
        }

        private static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static void RestartAsAdmin()
        {
            var exePath = System.Environment.GetCommandLineArgs()[0]; // Get the path to the executable
            var processInfo = new ProcessStartInfo(exePath)
            {
                UseShellExecute = true,
                Verb = "runas" // Request administrative privileges
            };

            try
            {
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to restart with administrative privileges. " + ex.Message);
            }
        }
    }
}