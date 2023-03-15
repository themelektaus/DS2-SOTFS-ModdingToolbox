namespace DS2_SOTFS_ModdingToolbox;

public class ThemeFile : File
{
    public ThemeFile(string path) : base(path)
    {

    }

    public override string GetDisplayName()
    {
        return string.IsNullOrEmpty(name) ? Lang.UI.DEFAULT : name;
    }
}