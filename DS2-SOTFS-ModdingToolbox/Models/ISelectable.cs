namespace DS2_SOTFS_ModdingToolbox;

public interface ISelectable
{
    public string name { get; }
    public string path { get; }
    public DateTimeOffset? timestamp { get; }

    public string GetRelativePath()
        => path is null ? "" : FileSystemUtils.GetRelativePath(path);
    
    public string GetTimestampString()
        => timestamp?.ToString(Lang.Format.DATETIME) ?? "";
}