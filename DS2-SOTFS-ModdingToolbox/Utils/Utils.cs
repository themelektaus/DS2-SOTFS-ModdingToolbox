using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

using System.IO;
using System.Reflection;
using System.Runtime.Loader;

using Flags = System.Reflection.BindingFlags;

namespace DS2_SOTFS_ModdingToolbox;

public static class Utils
{
    const Flags PRIVATE_FLAGS = Flags.Instance | Flags.NonPublic;
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

    public static MethodInfo GetPrivateMethod<T>(string name, params Type[] types)
    {
        return typeof(T).GetMethod(name, PRIVATE_FLAGS, types);
    }

    public static string GetSystemLanguageName()
    {
        var culture = System.Globalization.CultureInfo.InstalledUICulture;
        return culture.EnglishName?.Split(' ').FirstOrDefault()?.Trim();
    }

    public static async Task ChangeLanguage(string name)
    {
        var path = Path(Data.languageFolder, $"{(name ?? "")}.cs");

        if (!FileExists(path))
            path = Path(Data.languageFolder, "English.cs");

        if (!FileExists(path))
            return;

        var sourceCode = ReadText(path);

        await CompileAsync(name, sourceCode, (result, assembly) =>
        {
            if (!result.Success)
                return;

            var types = assembly.GetTypes();

            var langType = types.FirstOrDefault(x => x.Name == name);
            if (langType is null)
                return;

            PatchLanguage(typeof(Lang), new() { langType });
        });
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

    static async Task CompileAsync(string name, string sourceCode, Action<EmitResult, Assembly> callback)
    {
        var compilation = CSharpCompilation.Create(name)
            .WithOptions(new(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(MetadataReference.CreateFromFile(Data.coreLibrary))
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));
        using var stream = new MemoryStream();
        var result = await Task.Run(() => compilation.Emit(stream));
        stream.Seek(0, SeekOrigin.Begin);
        var context = new AssemblyLoadContext(name);
        callback(result, context.LoadFromStream(stream));
    }
}