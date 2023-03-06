using Microsoft.Win32;

namespace DS2_SOTFS_ModdingToolbox;

public class GameFinder
{
    public string rootFolder;

    public string gameFolder => Path(rootFolder, "Game");
    public string gameFolder_Origin => Path(rootFolder, "Game.origin");
    public string gameFolder_Unpacked => Path(rootFolder, "Game.unpacked");
    public string gameExe => Path(gameFolder, "DarkSoulsII.exe");

    public bool TryFindInstalledRootFolder(in string alternativeFolder)
    {
        rootFolder = Registry.GetValue(
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 335300",
            "InstallLocation",
            null
        ) as string;

        if (!IsValidPath(rootFolder))
            rootFolder = alternativeFolder;

        return IsValidPath(rootFolder);
    }

    public async Task RunGameAsync()
    {
        await StartAsync(gameExe);
    }
}