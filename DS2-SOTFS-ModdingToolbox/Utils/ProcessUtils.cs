using System.Diagnostics;

namespace DS2_SOTFS_ModdingToolbox;

public static class ProcessUtils
{
    public static async Task StartAsync(string name)
    {
        var process = Process.Start(name);
        while (!process.HasExited)
            await Utils.WaitLongAsync();
    }

    public static async Task StartAsync(string name, string args)
    {
        var process = Process.Start(name, args);
        while (!process.HasExited)
            await Utils.WaitLongAsync();
    }

    public static async Task RunAsync(string name, string args)
    {
        await SimpleExec.Command.RunAsync(name, args, createNoWindow: true);
        await Utils.WaitLongAsync();
    }

    public static bool IsProcessRunning(string processName)
    {
        return Process.GetProcessesByName(processName).Length > 0;
    }
}