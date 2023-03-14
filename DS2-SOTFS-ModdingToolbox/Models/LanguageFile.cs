namespace DS2_SOTFS_ModdingToolbox;

public class LanguageFile : ISelectable
{
    public string name { get; init; }
    public string path { get; init; }
    public DateTimeOffset? timestamp => GetModificationDate();

    public LanguageFile(string path)
    {
        this.path = path;
        name = GetFileNameWithoutExtension(path);
    }

    public DateTimeOffset? GetModificationDate()
    {
        var info = GetFileInfo(path);
        if (info.Exists)
            return info.LastWriteTime;
        return null;
    }

    public static List<LanguageFile> GetAll(string languageFolder)
    {
        if (!FolderExists(languageFolder))
            return new();

        return EnumerateFiles(languageFolder)
            .Select(x => new LanguageFile(x))
            .ToList();
    }
}