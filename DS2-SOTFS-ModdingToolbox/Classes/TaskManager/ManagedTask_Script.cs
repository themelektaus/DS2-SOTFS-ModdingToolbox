namespace DS2_SOTFS_ModdingToolbox;

public class ManagedTask_Script : ManagedTask
{
    public override string name => GetName();

    readonly Script.Instance scriptInstance;
    readonly string method;
    readonly object[] args;

    public ManagedTask_Script(Script.Instance scriptInstance, string method, params object[] args)
    {
        this.scriptInstance = scriptInstance;
        this.method = method;
        this.args = args;
    }

    public override async Task ProcessAsync(TaskInstance taskInstance)
    {
        taskInstance.info = Lang.ManagedTask.Info.RUNNING;

        var result = await scriptInstance.ExecuteAsync(method, args);
        var text = GetText(result);

        Program.logger.Log(
            sender: GetSender(),
            type: LogType.Info,
            title: GetTitle(),
            text: text
        );

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
        var title = method;
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