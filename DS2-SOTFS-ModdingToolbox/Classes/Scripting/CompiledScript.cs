using Microsoft.CodeAnalysis;

using System.Reflection;

namespace DS2_SOTFS_ModdingToolbox;

public class CompiledScript
{
    public readonly string name;
    public readonly string inputSourceCode;
    public readonly string outputSourceCode;
    public readonly Assembly assembly;

    string _sourcePath;
    public string sourcePath => _sourcePath;

    public CompiledScript(
        string name,
        string inputSourceCode,
        string outputSourceCode,
        Assembly assembly
    )
    {
        this.name = name;
        this.inputSourceCode = inputSourceCode;
        this.outputSourceCode = outputSourceCode;
        this.assembly = assembly;
    }

    public Type GetExportedType()
    {
        return assembly.ExportedTypes.FirstOrDefault();
    }
}