namespace DS2_SOTFS_ModdingToolbox;

public static class ProjectUtils
{
    public static bool CanCreate(string name)
    {
        if (name is null)
            return false;

        if (!IsValidFileName(name))
            return false;

        if (FolderExists(name))
            return false;

        return !Project.Exists(
            UserData.projectsFolder,
            name
        );
    }

    public static Project Create(Config config, string name)
    {
        var gameFinder = GameFinderUtils.Create(config);

        return Project.Create(
            UserData.projectsFolder,
            Data.projectTemplateFolder,
            name,
            gameFinder.gameFolder_Unpacked
        );
    }

    public static Project Load(string name)
    {
        return Project.Load(UserData.projectsFolder, name);
    }


    public static IEnumerable<Project> LoadAll()
    {
        return Project.LoadAll(UserData.projectsFolder);
    }

}