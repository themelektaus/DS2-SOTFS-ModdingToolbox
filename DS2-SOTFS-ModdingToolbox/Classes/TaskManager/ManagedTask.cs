namespace DS2_SOTFS_ModdingToolbox;

public abstract class ManagedTask
{
    public abstract string name { get; }

    public event Action<Exception> onError;

    public abstract Task ProcessAsync(TaskInstance taskInstance);

    public void InvokeError(Exception ex)
    {
        onError?.Invoke(ex);
    }
}