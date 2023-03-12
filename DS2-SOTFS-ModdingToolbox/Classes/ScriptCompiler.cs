using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System.IO;
using System.Runtime.Loader;

namespace DS2_SOTFS_ModdingToolbox;

public class ScriptCompiler
{
    public CompiledScript Compile(string path)
    {
        if (!FileExists(path))
            throw new FileNotFoundException(path);

        var compiledScript = Compile(
            GetFileNameWithoutExtension(path),
            ReadText(path)
        );

        typeof(CompiledScript)
            .GetField("_sourcePath", Utils.PRIVATE_FLAGS)
            .SetValue(compiledScript, path);

        return compiledScript;
    }

    public CompiledScript Compile(string name, string sourceCode)
    {
        var inputSourceCode = sourceCode;
        var outputSourceCode = sourceCode;

        var lines = Utils.EnumerateLines(sourceCode).ToList();
        var usings = new List<string> {
            "DS2_SOTFS_ModdingToolbox",
            "System",
            "System.Linq",
            "System.Threading",
            "System.Threading.Tasks",
            "System.Collections.Generic",
        };
        usings.Reverse();

        foreach (var @using in usings)
            if (!lines.Any(x => x.Trim() == $"using {@using};"))
                outputSourceCode = $"using {@using};{Environment.NewLine}{outputSourceCode}";

        var compilation = CSharpCompilation.Create(name)
            .WithOptions(new(OutputKind.DynamicallyLinkedLibrary))
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(outputSourceCode));

        foreach (var file in EnumerateFiles(Data.libraryFolder))
            compilation = compilation.AddReferences(MetadataReference.CreateFromFile(file));

        using var stream = new MemoryStream();

        var result = compilation.Emit(stream);
        if (!result.Success)
        {
            stream.Dispose();
            var messages = result.Diagnostics.Select(x => x.GetMessage());
            throw new Exception(string.Join(Environment.NewLine, messages));
        }

        stream.Seek(0, SeekOrigin.Begin);

        var context = new AssemblyLoadContext(name);

        try
        {
            return new(
                name,
                inputSourceCode,
                outputSourceCode,
                context.LoadFromStream(stream)
            );
        }
        catch
        {
            throw;
        }
    }
}