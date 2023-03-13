using System.Reflection;

namespace DS2_SOTFS_ModdingToolbox;

public class ManagedTask_Script : ManagedTask
{
    public override string name => GetName();

    readonly ScriptInstance scriptInstance;
    readonly MethodInfo method;
    readonly object[] args;

    public ManagedTask_Script(ScriptInstance scriptInstance, MethodInfo method, params object[] args)
    {
        this.scriptInstance = scriptInstance;
        this.method = method;
        this.args = args;
    }

    public override async Task ProcessAsync(TaskInstance taskInstance)
    {
        taskInstance.info = Lang.ManagedTask.Info.RUNNING;

        var eventListener = scriptInstance.GetProgressEventListener();

        eventListener?.AddListener((x, y) =>
        {
            if (x is not null)
                taskInstance.info = x;

            if (y.HasValue)
                taskInstance.progress = y.Value;
        });

        var result = await Task.Run(() => scriptInstance.Execute(method, args));

        eventListener?.RemoveAllListeners();

        var text = GetText(result);

        Program.logger.Log(
            sender: GetSender(),
            type: LogType.Info,
            title: GetTitle(),
            text: text
        );

        if (text is not null)
            taskInstance.info = text;
    }

    string GetName()
    {
        return $"{GetSender()}.{GetTitle()}";
    }

    string GetSender()
    {
        return scriptInstance.runtimeType.Name;
    }

    string GetTitle()
    {
        var title = method.Name;
        if (args.Length > 0)
            title += $"({string.Join(", ", args)})";
        return title;
    }

    string GetText(object result)
    {
        var text = result?.ToString();
        if (text == "System.Threading.Tasks.VoidTaskResult")
            return null;
        return text;
    }
}