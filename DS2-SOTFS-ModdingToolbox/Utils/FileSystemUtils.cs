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

public static class FileSystemUtils
{
    public static string AppDataFolder => E.GetFolderPath(E.SpecialFolder.ApplicationData);

    public static string Path(params string[] paths)
    {
        return P.Combine(paths);
    }

    public static FI GetFileInfo(string path) => new(path);

    public static DI GetFolderInfo(string path) => new(path);

    public static string GetFileName(string path)
    {
        return P.GetFileName(path);
    }

    public static string GetFileNameWithoutExtension(string path)
    {
        return P.GetFileNameWithoutExtension(path);
    }

    public static string GetFullPath(string path)
    {
        return P.GetFullPath(path);
    }

    public static bool IsValidPath(params string[] path)
    {
        if (path.All(string.IsNullOrWhiteSpace))
            return false;

        if (Path(path).IndexOfAny(P.GetInvalidPathChars()) < 0)
            return true;

        return false;
    }

    public static bool IsValidFileName(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return false;

        if (filename.IndexOfAny(P.GetInvalidFileNameChars()) < 0)
            return true;

        return false;
    }

    public static bool FileExists(string path)
    {
        if (!IsValidPath(path))
            return false;

        return F.Exists(Path(path));
    }

    public static void CopyFile(string source, string destination)
    {
        if (PrepareCopyFile(source, destination))
            F.Copy(source, destination);
    }

    public static void DeleteFile(string path)
    {
        if (FileExists(path))
            F.Delete(path);
    }

    public static byte[] ReadBytes(string path)
    {
        return F.ReadAllBytes(path);
    }

    public static void WriteBytes(string path, byte[] bytes)
    {
        F.WriteAllBytes(path, bytes);
    }

    public static string ReadText(string path)
    {
        return F.ReadAllText(path);
    }

    public static void WriteText(string path, string contents)
    {
        F.WriteAllText(path, contents);
    }

    static bool PrepareCopyFile(string source, string destination)
    {
        if (!FileExists(source))
            return false;

        if (FileExists(destination))
        {
            DeleteFile(destination);
            return true;
        }

        var info = GetFileInfo(destination);
        var folder = info.Directory.FullName;
        CreateFolder(folder);

        return true;
    }

    public static bool FolderExists(string path)
    {
        if (!IsValidPath(path))
            return false;

        return D.Exists(path);
    }

    public static string[] GetFolders(string path)
    {
        return D.GetDirectories(path);
    }

    public static void CreateFolder(string path)
    {
        D.CreateDirectory(path);
    }

    public static async Task RecreateFolderAsync(string path)
    {
        await DeleteFolderAsync(path);
        CreateFolder(path);
    }

    public static void MoveFolder(string source, string destination)
    {
        D.Move(source, destination);
    }

    public static void DeleteFolder(string path)
    {
        if (FolderExists(path))
            D.Delete(path, true);
    }

    public static async Task DeleteFolderAsync(string path)
    {
        if (FolderExists(path))
            await Task.Run(() => DeleteFolder(path));
    }

    public static IEnumerable<FI> EnumerateFiles(DI info)
    {
        return info.EnumerateFiles("*.*", SO.AllDirectories);
    }

    public static IEnumerable<string> EnumerateFiles(string path)
    {
        return D.EnumerateFiles(path, "*.*", SO.AllDirectories);
    }

    public static string GetRelativePath(FSI root, FSI path)
    {
        return GetRelativePath(root.FullName, path.FullName);
    }

    public static string GetRelativePath(string root, string path)
    {
        return path[(root.Length + 1)..];
    }

    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
    static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

    public static bool CreateHardLink(string file, string target)
    {
        if (!PrepareCopyFile(target, file))
            return false;

        return CreateHardLink(GetFullPath(file), GetFullPath(target), IntPtr.Zero);
    }

    public static string CalcFileChecksum(string file)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        using var stream = F.OpenRead(file);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
    }
}