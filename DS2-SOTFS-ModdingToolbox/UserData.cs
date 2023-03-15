namespace DS2_SOTFS_ModdingToolbox;

public static class UserData
{
    public static string backupFolder
        => GetUserDataPath(Lang.System.BACKUP_FOLDER);

    public static string configFile
        => GetUserDataPath(Lang.System.CONFIG_FILE);

    public static string languageFolder
        => GetUserDataPath(Lang.System.LANGUAGE_FOLDER);

    public static string projectsFolder
        => GetUserDataPath(Lang.System.PROJECTS_FOLDER);

    public static string scriptFolder
        => GetUserDataPath(Lang.System.SCRIPT_FOLDER);

    public static string themeFolder
        => GetUserDataPath(Lang.System.THEME_FOLDER);

    static string GetUserDataPath(params string[] path)
    {
        var path1 = Lang.System.USER_DATA_FOLDER;
        var path2 = Path(path);
        return GetFullPath(Path(path1, path2));
    }
}