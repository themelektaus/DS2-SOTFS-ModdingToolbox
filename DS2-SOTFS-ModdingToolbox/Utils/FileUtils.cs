namespace DS2_SOTFS_ModdingToolbox;

public static class FileUtils
{
    public static IEnumerable<LanguageFile> GetLanguageFiles()
    {
        return File.GetAll<LanguageFile>(UserData.languageFolder, Data.languageFolder);
    }

    public static IEnumerable<ScriptFile> GetScriptFiles()
    {
        return File.GetAll<ScriptFile>(UserData.scriptFolder, Data.scriptFolder);
    }

    public static IEnumerable<ThemeFile> GetThemeFiles()
    {
        return File
            .GetAll<ThemeFile>(UserData.themeFolder, Data.themeFolder)
            .Where(x => x.path.EndsWith(".css"))
            .Prepend(new(Path(Data.themeFolder, ".css")));
    }
}