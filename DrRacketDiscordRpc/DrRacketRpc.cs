﻿using DiscordRPC;
using System.Diagnostics;

public class DrRacketRpc
{
    static DiscordRpcClient? client;
    static RichPresence? rp;
    public static void Main()

    {
        client = new DiscordRpcClient("");
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
        client.SetPresence(rp);
        while (true)
        {
            if (Process.GetProcessesByName("DrRacket").Length > 0)
            {
                Process racketProcess = Process.GetProcessesByName("DrRacket")[0];
                client.SetPresence(rp);
                Console.WriteLine(racketProcess.MainWindowTitle);
            }
            else
            {

            }

            Task.Delay(10000).GetAwaiter().GetResult();
        }
    }
}

