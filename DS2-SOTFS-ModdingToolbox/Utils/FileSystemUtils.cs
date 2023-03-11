using System.Runtime.InteropServices;

using D = System.IO.Directory;
using DI = System.IO.DirectoryInfo;
using E = System.Environment;
using F = System.IO.File;
using FI = System.IO.FileInfo;
using FSI = System.IO.FileSystemInfo;
using P = System.IO.Path;
using SO = System.IO.SearchOption;

namespace DS2_SOTFS_ModdingToolbox;

using static Lang.FileSystemUtils;

public static class FileSystemUtils
{
    static void Log(string title, Exception ex)
        => Program.logger?.Log(Lang.Title.FILE_SYSTEM, title, ex);

    static void Log(LogLevel level, LogType type, string title, string text)
        => Program.logger?.Log(Lang.Title.FILE_SYSTEM, level, type, title, text);

    public static string GetAppDataFolder()
    {
        return E.GetFolderPath(E.SpecialFolder.ApplicationData);
    }

    public static string Path(params string[] paths)
    {
        return P.Combine(paths);
    }

    public static FI GetFileInfo(string path)
    {
        Log(LogLevel.Verbose, LogType.Info, nameof(GetFileInfo), path);
        return new(path);
    }

    public static DI GetFolderInfo(string path)
    {
        Log(LogLevel.Verbose, LogType.Info, nameof(GetFolderInfo), path);
        return new(path);
    }

    public static string GetFileName(string path)
    {
        Log(LogLevel.Verbose, LogType.Info, nameof(GetFileName), path);
        return P.GetFileName(path);
    }

    public static string GetFileNameWithoutExtension(string path)
    {
        Log(LogLevel.Verbose, LogType.Info, nameof(GetFileNameWithoutExtension), path);
        return P.GetFileNameWithoutExtension(path);
    }

    public static string GetFullPath(string path)
    {
        Log(LogLevel.Verbose, LogType.Info, nameof(GetFullPath), path);
        return P.GetFullPath(path);
    }

    public static bool IsValidPath(params string[] path)
    {
        if (path.All(string.IsNullOrWhiteSpace))
        {
            Log(LogLevel.Verbose, LogType.Warning, nameof(IsValidPath), PATH_IS_EMPTY);
            return false;
        }

        var _path = Path(path);
        if (_path.IndexOfAny(P.GetInvalidPathChars()) < 0)
        {
            Log(LogLevel.Verbose, LogType.Info, nameof(IsValidPath), PATH_IS_VALID.Format(_path));
            return true;
        }

        Log(LogLevel.Verbose, LogType.Warning, nameof(IsValidPath), PATH_IS_INVALID.Format(_path));
        return false;
    }

    public static bool IsValidFileName(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            Log(LogLevel.Debug, LogType.Warning, nameof(IsValidFileName), PATH_IS_EMPTY);
            return false;
        }

        if (filename.IndexOfAny(P.GetInvalidFileNameChars()) < 0)
        {
            Log(LogLevel.Debug, LogType.Info, nameof(IsValidFileName), FILENAME_IS_VALID.Format(filename));
            return true;
        }

        Log(LogLevel.Debug, LogType.Warning, nameof(IsValidFileName), FILENAME_IS_INVALID.Format(filename));
        return false;
    }

    public static bool FileExists(string path)
    {
        if (!IsValidPath(path))
        {
            Log(LogLevel.Debug, LogType.Warning, nameof(FileExists), PATH_IS_INVALID.Format(path));
            return false;
        }

        var _path = Path(path);

        var exists = F.Exists(_path);
        if (exists)
        {
            Log(LogLevel.Verbose, LogType.Info, nameof(FileExists), PATH_EXISTS.Format(_path));
            return true;
        }

        Log(LogLevel.Verbose, LogType.Warning, nameof(FileExists), PATH_NOT_EXISTS.Format(_path));
        return exists;
    }

    public static void CopyFile(string source, string destination)
    {
        if (!PrepareCopyFile(source, destination))
            return;

        try
        {
            F.Copy(source, destination);
            Log(LogLevel.Debug, LogType.Info, nameof(CopyFile), SOURCE_TO_DESTINATION.Format(source, destination));
        }
        catch (Exception ex)
        {
            Log(nameof(CopyFile), ex);
        }
    }

    public static void DeleteFile(string path)
    {
        if (!F.Exists(path))
            return;

        try
        {
            F.Delete(path);
            Log(LogLevel.Debug, LogType.Info, nameof(DeleteFile), path);
        }
        catch (Exception ex)
        {
            Log(nameof(DeleteFile), ex);
        }
    }

    public static byte[] ReadBytes(string path)
    {
        try
        {
            var bytes = F.ReadAllBytes(path);
            Log(LogLevel.Debug, LogType.Info, nameof(ReadBytes), path);
            return bytes;
        }
        catch (Exception ex)
        {
            Log(nameof(ReadBytes), ex);
            return null;
        }
    }

    public static void WriteBytes(string path, byte[] bytes)
    {
        try
        {
            F.WriteAllBytes(path, bytes);
            Log(LogLevel.Debug, LogType.Info, nameof(WriteBytes), path);
        }
        catch (Exception ex)
        {
            Log(nameof(WriteBytes), ex);
        }
    }

    public static string ReadText(string path)
    {
        try
        {
            var text = F.ReadAllText(path);
            Log(LogLevel.Debug, LogType.Info, nameof(ReadText), path);
            return text;
        }
        catch (Exception ex)
        {
            Log(nameof(ReadText), ex);
            return null;
        }
    }

    public static void WriteText(string path, string contents)
    {
        try
        {
            F.WriteAllText(path, contents);
            Log(LogLevel.Debug, LogType.Info, nameof(WriteText), path);
        }
        catch (Exception ex)
        {
            Log(nameof(WriteText), ex);
        }
    }

    static bool PrepareCopyFile(string source, string destination)
    {
        if (!FileExists(source))
        {
            Log(LogLevel.Verbose, LogType.Error, nameof(PrepareCopyFile), SOURCE_NOT_FOUND.Format(source));
            return false;
        }

        if (FileExists(destination))
        {
            DeleteFile(destination);

            Log(LogLevel.Verbose, LogType.Error, nameof(PrepareCopyFile), DESTINATION_ALREADY_EXISTS.Format(destination));
            return true;
        }

        var info = GetFileInfo(destination);
        var folder = info.Directory.FullName;
        CreateFolder(folder);

        Log(LogLevel.Verbose, LogType.Info, nameof(PrepareCopyFile), SOURCE_TO_DESTINATION.Format(source, destination));
        return true;
    }

    public static bool FolderExists(string path)
    {
        if (!IsValidPath(path))
        {
            Log(LogLevel.Debug, LogType.Warning, nameof(FolderExists), PATH_IS_INVALID.Format(path));
            return false;
        }

        var _path = Path(path);

        var exists = D.Exists(_path);
        if (exists)
        {
            Log(LogLevel.Verbose, LogType.Info, nameof(FolderExists), PATH_EXISTS.Format(_path));
            return true;
        }

        Log(LogLevel.Verbose, LogType.Warning, nameof(FolderExists), PATH_NOT_EXISTS.Format(_path));
        return exists;
    }

    public static string[] GetFolders(string path)
    {
        try
        {
            var folders = D.GetDirectories(path);
            Log(LogLevel.Debug, LogType.Info, nameof(GetFolders), path);
            return folders;
        }
        catch (Exception ex)
        {
            Log(nameof(GetFolders), ex);
            return null;
        }
    }

    public static void CreateFolder(string path)
    {
        if (D.Exists(path))
            return;

        try
        {
            D.CreateDirectory(path);
            Log(LogLevel.Debug, LogType.Info, nameof(CreateFolder), path);
        }
        catch (Exception ex)
        {
            Log(nameof(CreateFolder), ex);
        }
    }

    public static async Task RecreateFolderAsync(string path)
    {
        await DeleteFolderAsync(path);
        CreateFolder(path);
    }

    public static void MoveFolder(string source, string destination)
    {
        try
        {
            D.Move(source, destination);
            Log(LogLevel.Debug, LogType.Info, nameof(MoveFolder), SOURCE_TO_DESTINATION.Format(source, destination));
        }
        catch (Exception ex)
        {
            Log(nameof(MoveFolder), ex);
        }
    }

    public static void DeleteFolder(string path)
    {
        if (!D.Exists(path))
            return;

        try
        {
            D.Delete(path, true);
            Log(LogLevel.Debug, LogType.Info, nameof(DeleteFolder), path);
        }
        catch (Exception ex)
        {
            Log(nameof(DeleteFolder), ex);
        }
    }

    public static async Task DeleteFolderAsync(string path)
    {
        if (!D.Exists(path))
            return;

        await Task.Run(() => DeleteFolder(path));
    }

    public static IEnumerable<FI> EnumerateFiles(DI info)
    {
        try
        {
            var files = info.EnumerateFiles("*.*", SO.AllDirectories);
            Log(LogLevel.Debug, LogType.Info, nameof(EnumerateFiles), info.FullName);
            return files;
        }
        catch (Exception ex)
        {
            Log(nameof(EnumerateFiles), ex);
            return Enumerable.Empty<FI>();
        }
    }

    public static IEnumerable<string> EnumerateFiles(string path)
    {
        try
        {
            var files = D.EnumerateFiles(path, "*.*", SO.AllDirectories);
            Log(LogLevel.Debug, LogType.Info, nameof(EnumerateFiles), path);
            return files;
        }
        catch (Exception ex)
        {
            Log(nameof(EnumerateFiles), ex);
            return Enumerable.Empty<string>();
        }
    }

    public static string GetRelativePath(FSI root, FSI path)
    {
        return GetRelativePath(root.FullName, path.FullName);
    }

    public static string GetRelativePath(string root, string path)
    {
        try
        {
            var relativePath = path[(root.Length + 1)..];
            Log(LogLevel.Verbose, LogType.Info, nameof(GetRelativePath), relativePath);
            return relativePath;
        }
        catch (Exception ex)
        {
            Log(nameof(GetRelativePath), ex);
            return null;
        }
    }

    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
    static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

    public static bool CreateHardLink(string file, string target)
    {
        if (!PrepareCopyFile(target, file))
            return false;

        try
        {
            var _file = GetFullPath(file);
            var _target = GetFullPath(target);

            if (CreateHardLink(_file, _target, IntPtr.Zero))
            {
                Log(LogLevel.Debug, LogType.Info, nameof(CreateHardLink), SOURCE_TO_DESTINATION.Format(_target, file));
                return true;
            }

            Log(LogLevel.Release, LogType.Error, nameof(CreateHardLink), SOURCE_TO_DESTINATION.Format(_target, file));
            return true;
        }
        catch (Exception ex)
        {
            Log(nameof(CreateHardLink), ex);
            return false;
        }
    }

    public static string CalcFileChecksum(string file)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        using var stream = F.OpenRead(file);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
    }
}