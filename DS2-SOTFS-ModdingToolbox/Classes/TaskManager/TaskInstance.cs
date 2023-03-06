namespace DS2_SOTFS_ModdingToolbox;

public class TaskInstance
{
    public readonly ManagedTask managedTask;

    public DateTimeOffset? endTimestamp;
    public TimeSpan endTimestampAge => DateTimeOffset.Now - (endTimestamp ?? DateTimeOffset.Now);

    float _progress;
    public float progress
    {
        get => _progress;
        set => _progress = MathF.Min(MathF.Max(0, value), 1);
    }

    public string info;

    public TaskInstance(ManagedTask managedTask)
    {
        this.managedTask = managedTask;
    }

    public async Task StartAsync()
    {
        progress = 0;
        try
        {
            await managedTask.ProcessAsync(this);
        }
        catch (Exception ex)
        {
            info = ex.Message;
            managedTask.InvokeError(ex);
        }
        progress = 1;
        endTimestamp = DateTimeOffset.Now;
    }

    public async Task WaitAsync()
    {
        while (!endTimestamp.HasValue)
            await Utils.WaitShortAsync();
    }
}