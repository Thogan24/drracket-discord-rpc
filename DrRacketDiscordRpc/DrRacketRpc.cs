using DiscordRPC;
using System.Diagnostics;

public class DrRacketRpc
{
    static DiscordRpcClient? client;
    static RichPresence? rp;
    static string windowName;
    static string details;
    public static void Main()

    {
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
        rp.Details = "Editing File";
        while (true)
        {
            if (Process.GetProcessesByName("DrRacket").Length > 0)
            {
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
            }
            else
            {
                client.ClearPresence();
            }

            Task.Delay(10000).GetAwaiter().GetResult();
        }
    }
}