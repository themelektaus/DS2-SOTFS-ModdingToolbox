using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Directory = System.IO.Directory;
using File = System.IO.File;
using Path = System.IO.Path;

namespace DS2_SOTFS_ModdingToolbox;

public static class Utils
{
    public static bool processing;

    static Config config => Config.instance;

    public static void Backup()
    {
        var backupFolder = Path.Combine(Config.backupFolder, $"{config.projectName}-{DateTime.Now.Ticks}");
        CreateFolder(backupFolder);

        var folderInfo = new DirectoryInfo(config.projectFolder);
        foreach (var fileInfo in EnumerateFiles(folderInfo))
        {
            if (fileInfo.Extension == ".bak" || fileInfo.Extension == ".prev")
                continue;

            var backupFile = fileInfo.FullName[(folderInfo.FullName.Length + 1)..];
            CreateFolderForFile(backupFolder, backupFile);
            CopyFile(fileInfo.FullName, Path.Combine(backupFolder, backupFile));
        }
    }

    public static async Task OpenMapStudioAsync()
    {
        await StartAsync(config.mapStudioExe, $"\"{config.projectFile}\"");
    }

    public static async Task OpenUXMSelectiveUnpackAsync()
    {
        await StartAsync(config.uxmSelectiveUnpackExe);
    }

    public static async Task StartDarkSoulsIIAsync()
    {
        await StartAsync(Config.ds2GameExe);
    }

    public static async Task BuildGameAsync(bool packRegulationFile)
    {
        var originalGame = Config.ds2GameFolder_Origin;
        var unpackedGame = Config.ds2GameFolder_Unpacked;

        if (!FolderExists(unpackedGame))
        {
            string file;

            foreach (var target in EnumerateFiles(originalGame))
            {
                file = Path.Combine(unpackedGame, target[(originalGame.Length + 1)..]);
                CreateFolderForFile(file);
                CreateHardLink(file, target);
            }

            var decryptedRegulationFile = Config.decryptedRegulationFile;
            file = Path.Combine(unpackedGame, Path.GetFileName(decryptedRegulationFile));
            CopyFile(decryptedRegulationFile, file);

            var exePath = Path.GetFullPath(Path.Combine(unpackedGame, "DarkSoulsII.exe"));
            await RunAsync(config.uxmSelectiveUnpackExe, $"\"{exePath}\"");
        }

        if (!FolderExists(unpackedGame))
            return;

        await DeleteFolderAsync(unpackedGame, "_backup");

        await RecreateFolderAsync(Config.ds2GameFolder);

        if (packRegulationFile)
            await PackRegulationFileForGameBuildAsync();

        await AddUnpackedGameFilesToGameBuildAsync(includeRegulationFile: false);

        if (!config.useMapStudioParams)
            await AddUnpackedParamFilesToGameBuildAsync();

        await AddProjectToGameBuildAsync();
    }

    static async Task PackRegulationFileForGameBuildAsync()
    {
        var ds2GameFolder = Config.ds2GameFolder;

        var source = Path.Combine(Config.ds2GameFolder_Unpacked, "enc_regulation.bnd.dcx");
        var destination = Path.Combine(ds2GameFolder, "enc_regulation.bnd.dcx");
        CopyFile(source, destination);

        await RunAsync(config.yabberDcxExe, @$"""{ds2GameFolder}\enc_regulation.bnd.dcx""");
        await RunAsync(config.yabberExe, @$"""{ds2GameFolder}\enc_regulation.bnd""");

        DeleteFile(@$"{ds2GameFolder}\enc_regulation.bnd");
        DeleteFile(@$"{ds2GameFolder}\enc_regulation.bnd.dcx");

        foreach (var looseParam in config.activeParams.Where(x => char.IsUpper(x[0])))
        {
            source = @$"{config.projectFolder}\Param\{looseParam}.param";
            destination = @$"{ds2GameFolder}\enc_regulation-bnd\{looseParam}.param";
            CopyFile(source, destination);
        }

        await RunAsync(config.yabberExe, @$"""{ds2GameFolder}\enc_regulation-bnd""");
        await RunAsync(config.yabberDcxExe, @$"""{ds2GameFolder}\enc_regulation.bnd""");

        await DeleteFolderAsync(@$"{ds2GameFolder}\enc_regulation-bnd");

        DeleteFile(@$"{ds2GameFolder}\enc_regulation.bnd");
        DeleteFile(@$"{ds2GameFolder}\enc_regulation.bnd-yabber-dcx.xml");
    }

    static async Task AddUnpackedGameFilesToGameBuildAsync(bool includeRegulationFile)
    {
        var unpackedGameFolder = Config.ds2GameFolder_Unpacked;

        foreach (var target in EnumerateFiles(unpackedGameFolder))
        {
            var file = target[(unpackedGameFolder.Length + 1)..];

            if (config.useMapStudioParams)
                if (file.StartsWith("Param"))
                    continue;

            if (!includeRegulationFile)
                if (file.Equals("enc_regulation.bnd.dcx"))
                    continue;

            await AddFileToGameBuildAsync(unpackedGameFolder, file, true);
        }
    }

    static async Task AddUnpackedParamFilesToGameBuildAsync()
    {
        var paramFilesFolder = Config.unpackedParamFilesFolder;

        foreach (var target in EnumerateFiles(paramFilesFolder))
        {
            var file = target[(paramFilesFolder.Length + 1)..];
            await AddFileToGameBuildAsync(paramFilesFolder, file, false);
        }
    }

    public static async Task AddProjectToGameBuildAsync()
    {
        var projectFolder = config.projectFolder;

        foreach (var file in EnumerateProjectFiles(includeParams: !config.useMapStudioParams))
            await AddFileToGameBuildAsync(projectFolder, file, false);

        if (config.useMapStudioParams)
            return;

        foreach (var param in config.activeParams)
            await AddFileToGameBuildAsync(projectFolder, @$"Param\{param}.param", false);
    }

    static async Task AddFileToGameBuildAsync(string folder, string file, bool hardlink)
    {
        CreateFolder(Config.ds2GameFolder);

        var gameFile = Path.Combine(Config.ds2GameFolder, file);

        var target = Path.Combine(folder, file);

        await Task.Run(() =>
        {
            if (hardlink)
            {
                CreateHardLink(gameFile, target);
                return;
            }

            DeleteFile(gameFile);
            CopyFile(target, gameFile);
        });
    }

    public static async Task ClearGameBuildAsync()
    {
        await DeleteFolderAsync(Config.ds2GameFolder);
    }



    static async Task StartAsync(string name)
    {
        var process = Process.Start(name);
        while (!process.HasExited)
            await Task.Delay(250);
    }

    static async Task StartAsync(string name, string args)
    {
        var process = Process.Start(name, args);
        while (!process.HasExited)
            await Task.Delay(250);
    }

    static async Task RunAsync(string name, string args)
    {
        await SimpleExec.Command.RunAsync(name, args, createNoWindow: true);
        await Task.Delay(250);
    }

    public static bool IsProcessRunning(string processName)
    {
        return Process.GetProcessesByName(processName).Length > 0;
    }



    public static bool IsEmpty(string value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static bool FileExists(params string[] path)
    {
        return File.Exists(Path.Combine(path));
    }

    static void CopyFile(string source, string destination)
    {
        File.Copy(source, destination, true);
    }

    static void DeleteFile(params string[] path)
    {
        if (FileExists(path))
            File.Delete(Path.Combine(path));
    }



    public static bool FolderExists(params string[] path)
    {
        return Directory.Exists(Path.Combine(path));
    }

    static void CreateFolder(params string[] path)
    {
        Directory.CreateDirectory(Path.Combine(path));
    }

    static void CreateFolderForFile(params string[] path)
    {
        var folder = new FileInfo(Path.Combine(path)).Directory.FullName;
        if (!FolderExists(folder))
            CreateFolder(folder);
    }

    static async Task RecreateFolderAsync(params string[] path)
    {
        await DeleteFolderAsync(path);
        CreateFolder(path);
    }

    public static void MoveFolder(string source, string destination)
    {
        Directory.Move(source, destination);
    }

    static void DeleteFolder(params string[] path)
    {
        if (FolderExists(path))
            Directory.Delete(Path.Combine(path), true);
    }

    static async Task DeleteFolderAsync(params string[] path)
    {
        if (FolderExists(path))
            await Task.Run(() => DeleteFolder(path));
    }



    static IEnumerable<FileInfo> EnumerateFiles(DirectoryInfo info)
    {
        return info.EnumerateFiles("*.*", SearchOption.AllDirectories);
    }

    static IEnumerable<string> EnumerateFiles(params string[] path)
    {
        return Directory.EnumerateFiles(Path.Combine(path), "*.*", SearchOption.AllDirectories);
    }

    static IEnumerable<string> EnumerateProjectFiles(bool includeParams)
    {
        var projectFolder = config.projectFolder;

        foreach (var file in EnumerateFiles(projectFolder))
        {
            var projectFile = file[(projectFolder.Length + 1)..];

            if (includeParams)
            {
                if (projectFile.StartsWith("Param"))
                    continue;

                if (projectFile.EndsWith(".bnd.dcx"))
                    continue;
            }

            if (projectFile.EndsWith(".bak") || projectFile.EndsWith(".prev"))
                continue;

            if (projectFile.EndsWith(".json"))
                continue;

            yield return projectFile;
        }
    }

    public static string[] GetProjectParams()
    {
        var paramFolderInfo = new DirectoryInfo(config.projectParamFolder);
        return EnumerateFiles(paramFolderInfo)
            .Where(x => x.Extension == ".param")
            .Select(x => Path.GetFileNameWithoutExtension(x.Name))
            .ToArray();
    }

    public static bool TryGetSavegame(out FileInfo savegame)
    {
        savegame = null;
        var appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var darksoulsFolder = Path.Combine(appdataFolder, "DarkSoulsII");
        var darksoulsFolderInfo = new DirectoryInfo(darksoulsFolder);
        foreach (var folder in darksoulsFolderInfo.EnumerateDirectories())
            if (folder.Name.Length == 16)
                foreach (var file in folder.EnumerateFiles())
                    if (file.Name == "DS2SOFS0000.sl2")
                        savegame = file;
        return savegame is not null;
    }



    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
    static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

    public static bool CreateHardLink(string file, string target)
    {
        if (!FileExists(target))
            return false;

        CreateFolderForFile(file);

        if (FileExists(file))
            DeleteFile(file);

        return CreateHardLink(Path.GetFullPath(file), Path.GetFullPath(target), IntPtr.Zero);
    }
}