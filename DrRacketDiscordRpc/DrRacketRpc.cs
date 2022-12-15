using DiscordRPC;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DrRacketDiscordRPC
{
    public class DrRacketRpc
    {
        static DiscordRpcClient? client;
        static RichPresence? rp;
        static string? windowName;
        static string? details;
        static bool drRacketIsOpen;
        static DateTime drRacketOpened;
        const int DelayTimeNotOpened = 10000;
        const int DelayTimeOpened = 2000;

        public static void Main()
        {
            foreach (Process process in Process.GetProcessesByName("DrRacketDiscordRpc"))
            {
                if (process.Id != Environment.ProcessId)
                {
                    process.Kill();
                }
            }
            
            AddToWindowsStartup();
            client = new DiscordRpcClient("1047212817427726419");
            client.Initialize();
            rp = new RichPresence
            {
                Assets = new Assets()
                {
                    LargeImageKey = "racket",
                    LargeImageText = "DrRacket",
                }
            };
            while (true)
            {
                if (Process.GetProcessesByName("DrRacket").Length > 0)
                {
                    if (!drRacketIsOpen)
                    {
                        drRacketOpened = DateTime.UtcNow;
                        drRacketIsOpen = true;
                    }
                    Process racketProcess = Process.GetProcessesByName("DrRacket")[0];
                    client.SetPresence(rp);
                    windowName = racketProcess.MainWindowTitle;
                    if (windowName.StartsWith("DrRacket"))
                    {
                        details = "Starting DrRacket";
                    }
                    else if (windowName.EndsWith("- DrRacket") || windowName.EndsWith("- DrRacket*"))
                    {
                        string[] splitName = windowName.Split(" - ");
                        if (splitName[0].Equals("Untitled"))
                        {
                            details = "Editing a file";
                        }
                        else
                        {
                            details = "Editing " + splitName[0];
                        }
                    }
                    else if (windowName.Equals("Warning"))
                    {
                        details = "Closing DrRacket";
                    }
                    client.UpdateDetails(details);
                    client.UpdateStartTime(drRacketOpened);
                }
                else
                {
                    drRacketIsOpen = false;
                    client.ClearPresence();
                }
                Task.Delay(drRacketIsOpen ? DelayTimeOpened : DelayTimeNotOpened).GetAwaiter().GetResult();
            }
        }

        static void AddToWindowsStartup()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string startUpKeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
#pragma warning disable CS8604 // Possible null reference argument.
                string? executablePath = System.Reflection.Assembly.GetEntryAssembly()?.Location;
                RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey(startUpKeyPath, true);
                registryKey?.SetValue("DrRacketRPC", executablePath);
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }
    }
}
