namespace DS2_SOTFS_ModdingToolbox;

public class Config
{
    public float uiScale = 1.5f;
    public string languageName = null;

    public string customDarkSouls2Folder = "";

#if DEBUG
    public string dsMapStudioExe = @$"..\DSMapStudio\DSMapStudio\bin\Debug\net7.0-windows\{Lang.System.DS_MAP_STUDIO_EXE}";
    public string uxmSelectiveUnpackExe = @$"..\UXM Selective Unpack\UXM\bin\Debug\{Lang.System.UXM_SELECTIVE_UNPACK_EXE}";
#else
    public string dsMapStudioExe = @$"DSMapStudio\{Lang.System.DS_MAP_STUDIO_EXE}";
    public string uxmSelectiveUnpackExe = @$"UXM Selective Unpack\{Lang.System.UXM_SELECTIVE_UNPACK_EXE}";
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