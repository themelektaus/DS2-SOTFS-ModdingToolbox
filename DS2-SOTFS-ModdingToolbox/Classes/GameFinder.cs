using Microsoft.Win32;

namespace DS2_SOTFS_ModdingToolbox;

using static Lang.System;

public class GameFinder
{
    public string rootFolder;

    public string gameFolder => Path(rootFolder, GAME_FOLDER);
    public string gameFolder_Origin => Path(rootFolder, GAME_ORIGIN_FOLDER);
    public string gameFolder_Unpacked => Path(rootFolder, UNPACKED_GAME_FOLDER);
    public string gameExe => Path(gameFolder, DARK_SOULS_2_EXE);

    public bool TryFindInstalledRootFolder(in string alternativeFolder)
    {
        rootFolder = Registry.GetValue(
            INSTALL_LOCATION_REGISTRY_KEY_NAME,
            INSTALL_LOCATION_REGISTRY_VALUE_NAME,
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