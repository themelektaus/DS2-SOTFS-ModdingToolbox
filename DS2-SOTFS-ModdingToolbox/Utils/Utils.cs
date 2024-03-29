﻿using Microsoft.CodeAnalysis;

using System.IO;

using Flags = System.Reflection.BindingFlags;

namespace DS2_SOTFS_ModdingToolbox;

public static class Utils
{
    const Flags PRIVATE_STATIC_FLAGS = Flags.Public | Flags.NonPublic | Flags.Static;

    public static bool TryGetSavegame(out FileInfo savegame)
    {
        savegame = null;
        var darksoulsFolder = Path(GetAppDataFolder(), Lang.System.DARK_SOULS_2_PROCESS_NAME);
        foreach (var folder in GetFolderInfo(darksoulsFolder).EnumerateDirectories())
            if (folder.Name.Length == 16)
                foreach (var file in folder.EnumerateFiles())
                    if (file.Name == Lang.System.DARK_SOULS_2_SAVEGAME_FILE)
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

    public static void ChangeToSystemLanguage(Config config)
    {
        var culture = System.Globalization.CultureInfo.InstalledUICulture;
        var name = culture.EnglishName?.Split(' ').FirstOrDefault()?.Trim();

        var path = Path(Data.languageFolder, $"{(name ?? "")}.cs");
        if (!FileExists(path))
        {
            ChangeLanguage(config, Lang.ENGLISH);
            return;
        }

        ChangeLanguage(config, name);
    }

    public static void ChangeLanguage(Config config, string name)
    {
        var compiledScript = ScriptCompilerUtils.CompileLanguage(name);
        PatchLanguage(typeof(Lang), new() { compiledScript.GetExportedType() });
        config.languageName = name;
    }

    static void PatchLanguage(Type originLangType, List<Type> tree)
    {
        var last = tree.Last();
        var fields = last.GetFields(PRIVATE_STATIC_FLAGS);

        foreach (var field in fields.Where(x => x.FieldType == typeof(string)))
        {
            var targetType = originLangType;

            foreach (var sourceType in tree.Skip(1))
            {
                targetType = targetType
                    .GetNestedTypes(PRIVATE_STATIC_FLAGS)
                    .FirstOrDefault(x => x.Name == sourceType.Name);

                if (targetType is null)
                    break;
            }

            targetType?
                .GetField(field.Name, PRIVATE_STATIC_FLAGS)?
                .SetValue(null, field.GetValue(null));
        }

        foreach (var type in last.GetNestedTypes(PRIVATE_STATIC_FLAGS))
        {
            tree.Add(type);
            PatchLanguage(originLangType, tree);
            tree.Remove(type);
        }
    }

    public static IEnumerable<string> EnumerateLines(string s)
    {
        using var reader = new StringReader(s);
        string line;
        while ((line = reader.ReadLine()) is not null)
            yield return line;
    }

    public static bool IsEmpty(string s) => string.IsNullOrEmpty(s);
}