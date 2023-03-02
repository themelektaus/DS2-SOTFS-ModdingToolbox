using Microsoft.Win32;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;

namespace DS2_SOTFS_ModdingToolbox;

public class Config
{
    public const string DS2_EXE = @"Dark Souls II Scholar of the First Sin\Game\DarkSoulsII.exe";

    const string DATA_FOLDER = @"Data";

    const string BACKUP_FOLDER = @"Backup";
    const string PROJECTS_FOLDER = @"Projects";
    const string UNPACKED_PARAM_FILES_FOLDER = @"Unpacked Param Files";

    const string CONFIG_FILE = @"Config.json";
    const string DECRYPTED_REGULATION_FILE = @"Decrypted Regulation File\enc_regulation.bnd.dcx";

    static string _ds2Folder2;
    static string ds2Folder2
        => _ds2Folder2 ??= Registry.GetValue(
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 335300",
            "InstallLocation",
            ""
        ) as string;

    public static string ds2GameFolder => Utils.IsEmpty(ds2Folder2) ? null : Path.Combine(ds2Folder2, "Game");
    public static string ds2GameFolder_Origin => Utils.IsEmpty(ds2Folder2) ? null : Path.Combine(ds2Folder2, "Game.origin");
    public static string ds2GameFolder_Unpacked => Utils.IsEmpty(ds2Folder2) ? null : Path.Combine(ds2Folder2, "Game.unpacked");
    public static string ds2GameExe => Utils.IsEmpty(ds2Folder2) ? null : Path.Combine(ds2GameFolder, "DarkSoulsII.exe");

    public static string backupFolder => GetDataPath(BACKUP_FOLDER);
    public static string projectsFolder => GetDataPath(PROJECTS_FOLDER);
    public static string unpackedParamFilesFolder => GetDataPath(UNPACKED_PARAM_FILES_FOLDER);

    static string configFile => GetDataPath(CONFIG_FILE);
    public static string decryptedRegulationFile => GetDataPath(DECRYPTED_REGULATION_FILE);

    public string projectName = "";
    public string projectFolder => Utils.IsEmpty(projectName) ? null : Path.Combine(projectsFolder, projectName);
    public string projectFile => Utils.IsEmpty(projectName) ? null : Path.Combine(projectFolder, "project.json");
    public string projectParamFolder => Utils.IsEmpty(projectName) ? null : Path.Combine(projectFolder, "Param");

    public string mapStudioExe = @"..\DSMapStudio\DSMapStudio\bin\Debug\net7.0-windows\DSMapStudio.exe";
    public string uxmSelectiveUnpackExe = @"..\UXM Selective Unpack\UXM\bin\Debug\UXM Selective Unpack.exe";
    public string yabberExe = @"";
    public string yabberDcxExe = @"";

    public readonly HashSet<string> activeParams = new();
    public bool useMapStudioParams = false;
    public bool freezeSavegame = true;

    static Config _instance;
    public static Config instance => _instance ??= Load();

    static string GetDataPath(params string[] path)
    {
        var path1 = DATA_FOLDER;
        var path2 = Path.Combine(path);
        return Path.GetFullPath(Path.Combine(path1, path2));
    }

    static Config Load()
    {
        if (Utils.FileExists(configFile))
        {
            var json = File.ReadAllText(configFile);
            return JsonConvert.DeserializeObject<Config>(json);
        }
        return new();
    }

    public static void Reload()
    {
        _instance = Load();
    }

    Config() { }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(this);
        File.WriteAllText(configFile, json);
    }
}