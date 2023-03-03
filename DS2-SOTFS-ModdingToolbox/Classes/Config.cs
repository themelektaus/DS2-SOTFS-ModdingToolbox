using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DS2_SOTFS_ModdingToolbox;

public class Config
{
    public const string DS2_EXE = @"Dark Souls II Scholar of the First Sin\Game\DarkSoulsII.exe";

    const string DATA_FOLDER = @"Data";

    const string BACKUP_FOLDER = @"Backup";
    const string PROJECT_TEMPLATE_FOLDER = @"Project Template";
    const string PROJECTS_FOLDER = @"Projects";
    const string UNPACKED_PARAM_FILES_FOLDER = @"Unpacked Param Files";

    const string CONFIG_FILE = @"Config.json";
    const string DECRYPTED_REGULATION_FILE = @"Decrypted Regulation File\enc_regulation.bnd.dcx";

    static string _ds2Folder;
    public static string ds2Folder
    {
        get => _ds2Folder ??= Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 335300",
                "InstallLocation",
                instance.customDs2Folder
            ) as string;
        set { _ds2Folder = value; }
    }

    public static string ds2GameFolder => Path.Combine(ds2Folder, "Game");
    public static string ds2GameFolder_Origin => Path.Combine(ds2Folder, "Game.origin");
    public static string ds2GameFolder_Unpacked => Path.Combine(ds2Folder, "Game.unpacked");
    public static string ds2GameExe => Path.Combine(ds2GameFolder, "DarkSoulsII.exe");

    public static string backupFolder => GetDataPath(BACKUP_FOLDER);
    public static string projectTemplateFolder => GetDataPath(PROJECT_TEMPLATE_FOLDER);
    public static string projectTemplateFile => Path.Combine(projectTemplateFolder, "project.json");
    public static string projectsFolder => GetDataPath(PROJECTS_FOLDER);

    public static string[] projectNames
    {
        get
        {
            if (!Directory.Exists(projectsFolder))
                return Array.Empty<string>();

            return Directory.GetDirectories(projectsFolder)
                .Select(Path.GetFileNameWithoutExtension)
                .ToArray();
        }
    }

    public static string unpackedParamFilesFolder => GetDataPath(UNPACKED_PARAM_FILES_FOLDER);

    static string configFile => GetDataPath(CONFIG_FILE);
    public static string decryptedRegulationFile => GetDataPath(DECRYPTED_REGULATION_FILE);

    public string customDs2Folder = "";

    public string projectName = "";
    public string projectFolder => Utils.IsValidFileName(projectName) ? Path.Combine(projectsFolder, projectName) : null;
    public string projectFile => Utils.IsValidFileName(projectName) ? Path.Combine(projectFolder, "project.json") : null;
    public string projectParamFolder => Utils.IsValidFileName(projectName) ? Path.Combine(projectFolder, "Param") : null;

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