using System.Windows.Forms;

namespace DS2_SOTFS_ModdingToolbox;

public static class GameFinderUtils
{
    public static GameFinder Create(Config config, bool saveConfig = true)
    {
        var gameFinder = new GameFinder();

        if (gameFinder.TryFindInstalledRootFolder(in config.customDarkSouls2Folder))
            return gameFinder;

        var dialog = new OpenFileDialog
        {
            Filter = "DarkSoulsII.exe|DarkSoulsII.exe"
        };

    Retry:
        if (dialog.ShowDialog() != DialogResult.OK)
            return null;

        if (!FileExists(dialog.FileName))
            goto Retry;

        var exeFile = GetFileInfo(GetFullPath(dialog.FileName));
        if (exeFile.Directory.Name != "Game")
        {
            Message.CreateError("DarkSoulsII.exe should be inside a folder called \"Game\".").Show();
            goto Retry;
        }

        config.customDarkSouls2Folder = exeFile.Directory.Parent.FullName;

        if (saveConfig)
            config.Save();

        gameFinder.rootFolder = config.customDarkSouls2Folder;
        return gameFinder;
    }

    public static async Task RunGameAsync(Config config)
    {
        var gameFinder = Create(config);
        await StartAsync(gameFinder.gameExe);
    }

    public static bool BuildExists(Config config)
    {
        var gameFinder = Create(config);
        return FolderExists(gameFinder.gameFolder) && FolderExists(gameFinder.gameFolder_Origin);
    }

}