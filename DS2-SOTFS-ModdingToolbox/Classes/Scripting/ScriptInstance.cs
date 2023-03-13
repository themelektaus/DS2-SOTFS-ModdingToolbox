using Microsoft.CodeAnalysis;

using System.Reflection;
using System.Runtime.CompilerServices;

namespace DS2_SOTFS_ModdingToolbox;

public class ScriptInstance
{
    public readonly Type runtimeType;
    public readonly dynamic runtimeObject;

    public static ScriptInstance Create(CompiledScript compiledScript)
        => new(compiledScript);

    ScriptInstance(CompiledScript compiledScript)
    {
        runtimeType = compiledScript.GetExportedType();
        runtimeObject = Activator.CreateInstance(runtimeType);
    }

    public FieldInfo[] GetFields()
    {
        return runtimeType.GetFields().Where(x => !x.IsInitOnly).ToArray();
    }

    public MethodInfo[] GetMethods()
    {
        return runtimeType.GetMethods().Where(x => x.DeclaringType != typeof(object)).ToArray();
    }

    public async Task<object> ExecuteAsync(string methodName, params object[] args)
    {
        return await Task.Run(() => Execute(methodName, args));
    }

    public object Execute(string methodName, params object[] args)
    {
        var method = runtimeType.GetMethods()
            .Where(x => x.Name == methodName)
            .Where(x => x.GetParameters().Select(x => x.ParameterType)
                .SequenceEqual(args.Select(x => x.GetType())))
            .FirstOrDefault();
        return Execute(method, args);
    }

    public async Task<object> ExecuteAsync(MethodInfo method, params object[] args)
    {
        return await Task.Run(() => Execute(method, args));
    }
    public object Execute(MethodInfo method, params object[] args)
    {
        if (method.GetCustomAttribute<AsyncStateMachineAttribute>() is null)
            return method.Invoke(runtimeObject, args);

        dynamic awaitable = method.Invoke(runtimeObject, args);
        var result = (awaitable.GetType() as Type).GetProperty("Result");
        return result.GetValue(awaitable);
    }

    public ProgressEventListener GetProgressEventListener()
    {
        var field = runtimeType
            .GetPrivateFields()
            .FirstOrDefault(x => x.FieldType == typeof(ProgressEventListener));

        return field.GetValue(runtimeObject) as ProgressEventListener;
    }
}