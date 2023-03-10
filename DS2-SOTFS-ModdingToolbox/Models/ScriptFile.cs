namespace DS2_SOTFS_ModdingToolbox;

public class ScriptFile : ISelectable
{
    public string name { get; init; }
    public string path { get; init; }
    public DateTimeOffset? timestamp => GetModificationDate();

    public ScriptFile(string path)
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

    public static List<ScriptFile> GetAll(string scriptsFolder)
    {
        if (!FolderExists(scriptsFolder))
            return new();

        return EnumerateFiles(scriptsFolder).Select(x => new ScriptFile(x)).ToList();
    }
}