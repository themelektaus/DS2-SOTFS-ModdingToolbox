namespace DS2_SOTFS_ModdingToolbox;

using MethodInfo = System.Reflection.MethodInfo;
using Flags = System.Reflection.BindingFlags;

public static class Utils
{
    const Flags INSTANCE_PRIVATE_FLAGS = Flags.Instance | Flags.NonPublic;

    public static bool TryGetSavegame(out System.IO.FileInfo savegame)
    {
        savegame = null;
        var darksoulsFolder = Path(AppDataFolder, "DarkSoulsII");
        foreach (var folder in GetFolderInfo(darksoulsFolder).EnumerateDirectories())
            if (folder.Name.Length == 16)
                foreach (var file in folder.EnumerateFiles())
                    if (file.Name == "DS2SOFS0000.sl2")
                        savegame = file;
        return savegame is not null;
    }

    public static string CalcChecksum(byte[] bytes)
    {
        var hash = System.Security.Cryptography.MD5.HashData(bytes);
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
    }

    public static async Task WaitShortAsync()
    {
        await Task.Delay(50);
    }

    public static async Task WaitLongAsync()
    {
        await Task.Delay(250);
    }

    public static MethodInfo GetPrivateMethod<T>(string name, params Type[] types)
    {
        return typeof(T).GetMethod(name, INSTANCE_PRIVATE_FLAGS, types);
    }
}