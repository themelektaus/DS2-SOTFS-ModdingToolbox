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

    public object Execute(string method, params object[] args)
    {
        var methodInfo = runtimeType.GetMethod(method);

        if (methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>() is null)
            return methodInfo.Invoke(runtimeObject, args);

        dynamic awaitable = methodInfo.Invoke(runtimeObject, args);
        var result = (awaitable.GetType() as Type).GetProperty("Result");
        return result.GetValue(awaitable);
    }

    public async Task<object> ExecuteAsync(string method, params object[] args)
    {
        return await Task.Run(() => Execute(method, args));
    }

    public ProgressEventListener GetProgressEventListener()
    {
        var field = runtimeType
            .GetPrivateFields()
            .FirstOrDefault(x => x.FieldType == typeof(ProgressEventListener));

        return field.GetValue(runtimeObject) as ProgressEventListener;
    }
}