namespace DS2_SOTFS_ModdingToolbox;

public static class ScriptFileUtils
{
    public static List<ScriptFile> GetAll()
    {
        return ScriptFile.GetAll(UserData.scriptsFolder);
    }
}