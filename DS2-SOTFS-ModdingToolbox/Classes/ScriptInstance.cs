using Microsoft.CodeAnalysis;

using System.Reflection;
using System.Runtime.CompilerServices;

namespace DS2_SOTFS_ModdingToolbox;

public class ScriptInstance
{
    public readonly Type runtimeType;
    public readonly dynamic runtimeObject;

    public static ScriptInstance Create(Script script) => new(script);

    ScriptInstance(Script script)
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
}