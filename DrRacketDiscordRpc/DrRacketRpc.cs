using System.Diagnostics;

public class DrRacketRpc
{
    public static void Main()
    {
        while (true)
        {
            if (Process.GetProcessesByName("DrRacket").Length > 0)
            {
                Process[] racketProcesses = Process.GetProcessesByName("DrRacket");
            }
            Task.Delay(10000).GetAwaiter().GetResult();
        }
    }
}

