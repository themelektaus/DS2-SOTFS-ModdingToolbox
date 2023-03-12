using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace DS2_SOTFS_ModdingToolbox;

public class Script
{
    public class Instance
    {
        public readonly Type runtimeType;
        public readonly dynamic runtimeObject;

        public Instance(Script script)
        {
            runtimeType = script.GetExportedType();
            runtimeObject = Activator.CreateInstance(runtimeType);
        }

        public FieldInfo[] GetFields()
        {
            return runtimeType.GetFields();
        }

        public MethodInfo[] GetMethods()
        {
            return runtimeType.GetMethods().Where(x => x.DeclaringType != typeof(object)).ToArray();
        }

        public async Task<object> ExecuteAsync(string method, params object[] args)
        {
            var methodInfo = GetMethodInfo(method);
            if (methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>() is null)
                return GetValue(methodInfo, args);
            return await Task.Run(() => GetValueAsync(methodInfo, args));
        }

        MethodInfo GetMethodInfo(string method)
        {
            return runtimeType.GetMethod(method);
        }

        object GetValue(MethodInfo methodInfo, object[] args)
        {
            return methodInfo.Invoke(runtimeObject, args);
        }

        object GetValueAsync(MethodInfo methodInfo, object[] args)
        {
            dynamic awaitable = methodInfo.Invoke(runtimeObject, args);
            var result = (awaitable.GetType() as Type).GetProperty("Result");
            return result.GetValue(awaitable);
        }

    }

    public string path { get; private set; }
    public string name { get; private set; }
    public string originSourceCode { get; private set; }
    public string sourceCode { get; private set; }
    public Assembly assembly { get; private set; }

    public void Compile(string path)
    {
        if (!FileExists(path))
            throw new FileNotFoundException(path);

        this.path = path;
        name = GetFileNameWithoutExtension(path);
        Compile(name, ReadText(path));
    }

    public void Compile(string name, string sourceCode)
    {
        originSourceCode = sourceCode;

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
                sourceCode = $"using {@using};{Environment.NewLine}{sourceCode}";

        this.sourceCode = sourceCode;

        var compilation = CSharpCompilation.Create(name)
            .WithOptions(new(OutputKind.DynamicallyLinkedLibrary))
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));

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

        try { assembly = context.LoadFromStream(stream); } catch { throw; }
    }

    public void CompileScript(string name)
    {
        Compile(Path(Data.scriptsFolder, $"{name}.cs"));
    }

    public Type GetExportedType()
    {
        return assembly.ExportedTypes.FirstOrDefault();
    }

    public Instance Instantiate()
    {
        return new(this);
    }
}