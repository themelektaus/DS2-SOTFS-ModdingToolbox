namespace DS2_SOTFS_ModdingToolbox;

public static class UserData
{
    const string USER_DATA_FOLDER = @"UserData";

    const string BACKUP_FOLDER = @"Backup";
    const string PROJECTS_FOLDER = @"Projects";

    const string CONFIG_FILE = @"Config.json";

    public static string backupFolder => GetUserDataPath(BACKUP_FOLDER);
    public static string projectsFolder => GetUserDataPath(PROJECTS_FOLDER);

    public static string configFile => GetUserDataPath(CONFIG_FILE);

    static string GetUserDataPath(params string[] path)
    {
        var path1 = USER_DATA_FOLDER;
        var path2 = Path(path);
        return GetFullPath(Path(path1, path2));
    }
}