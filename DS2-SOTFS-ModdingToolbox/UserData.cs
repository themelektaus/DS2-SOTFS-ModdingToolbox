namespace DS2_SOTFS_ModdingToolbox;

public static class UserData
{
    public static string backupFolder
        => GetUserDataPath(Lang.System.BACKUP_FOLDER);

    public static string configFile
        => GetUserDataPath(Lang.System.CONFIG_FILE);

    public static string projectsFolder
        => GetUserDataPath(Lang.System.PROJECTS_FOLDER);

    public static string scriptsFolder
        => GetUserDataPath(Lang.System.SCRIPTS_FOLDER);

    static string GetUserDataPath(params string[] path)
    {
        var path1 = Lang.System.USER_DATA_FOLDER;
        var path2 = Path(path);
        return GetFullPath(Path(path1, path2));
    }
}