namespace DS2_SOTFS_ModdingToolbox;

public static class LanguageFileUtils
{
    public static List<LanguageFile> GetAll()
    {
        return LanguageFile.GetAll(Data.languageFolder);
    }
}