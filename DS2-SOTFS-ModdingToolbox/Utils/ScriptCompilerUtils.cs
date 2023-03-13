using System.IO;

namespace DS2_SOTFS_ModdingToolbox;

public static class ScriptCompilerUtils
{
    public static CompiledScript Compile(ScriptFile scriptFile)
    {
        return CompileFile(scriptFile.name, scriptFile.path);
    }

    public static CompiledScript CompileLanguage(string name)
    {
        return CompileFile(Path(Data.languageFolder, $"{(name ?? "")}.cs"));
    }

    public static CompiledScript CompileFile(string path)
    {
        return CompileFile(GetFileNameWithoutExtension(path), path);
    }

    public static CompiledScript CompileFile(string name, string path)
    {
        if (!FileExists(path))
            throw new FileNotFoundException(path);

        var compiledScript = Compile(name, ReadText(path));
        compiledScript.SetFieldValue("_sourcePath", path);
        return compiledScript;
    }

    public static CompiledScript Compile(string name, string sourceCode)
    {
        var scriptCompiler = new ScriptCompiler();
        scriptCompiler.referenceFiles.AddRange(EnumerateFiles(Data.libraryFolder));
        return scriptCompiler.Compile(name, sourceCode);
    }
}