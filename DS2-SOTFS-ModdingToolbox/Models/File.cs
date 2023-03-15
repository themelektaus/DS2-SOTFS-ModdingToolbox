namespace DS2_SOTFS_ModdingToolbox;

public abstract class File : ISelectable
{
    public string name { get; init; }
    public string path { get; init; }
    public DateTimeOffset? timestamp => GetModificationDate();

    public File(string path)
    {
        this.path = path;
        name = GetFileNameWithoutExtension(path);
    }

    public virtual string GetDisplayName()
    {
        return name;
    }

    public DateTimeOffset? GetModificationDate()
    {
        var info = GetFileInfo(path);
        if (info.Exists)
            return info.LastWriteTime;
        return null;
    }

    public static IEnumerable<T> GetAll<T>(params string[] folders) where T : File
    {
        foreach (var folder in folders)
            if (FolderExists(folder))
                foreach (var file in EnumerateFiles(folder))
                    yield return Activator.CreateInstance(typeof(T), file) as T;
    }
}