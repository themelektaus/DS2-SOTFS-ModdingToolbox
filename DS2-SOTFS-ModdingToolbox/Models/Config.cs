namespace DS2_SOTFS_ModdingToolbox;

public class Config
{
    public string customDarkSouls2Folder = "";

#if DEBUG
    public string dsMapStudioExe = @"..\DSMapStudio\DSMapStudio\bin\Debug\net7.0-windows\DSMapStudio.exe";
    public string uxmSelectiveUnpackExe = @"..\UXM Selective Unpack\UXM\bin\Debug\UXM Selective Unpack.exe";
#else
    public string dsMapStudioExe = @"DSMapStudio\DSMapStudio.exe";
    public string uxmSelectiveUnpackExe = @"UXM Selective Unpack\UXM Selective Unpack.exe";
#endif

    public string yabberExe = @"";
    public string yabberDcxExe = @"";

    public bool freezeSavegame = true;

    Config() { }

    public static Config Load()
    {
        if (FileExists(UserData.configFile))
        {
            var json = ReadText(UserData.configFile);
            return JsonConvert.DeserializeObject<Config>(json);
        }
        return new();
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(this);
        WriteText(UserData.configFile, json);
    }
}