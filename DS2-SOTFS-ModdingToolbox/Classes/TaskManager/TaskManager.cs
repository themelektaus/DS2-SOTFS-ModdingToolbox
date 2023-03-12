namespace DS2_SOTFS_ModdingToolbox;

public class TaskManager
{
    public readonly List<TaskInstance> taskInstances = new();

    readonly Queue<TaskInstance> pendingTaskInstances = new();

    bool running;

    public bool hasPendingTaskInstances => pendingTaskInstances.Count > 0;

    public bool hasStopped { get; private set; }

    public async Task RunAsync(ManagedTask task)
    {
        await Enqueue(task).WaitAsync();
    }

    public TaskInstance Enqueue(ManagedTask task)
    {
        var taskInstance = new TaskInstance(task);
        taskInstances.Add(taskInstance);
        pendingTaskInstances.Enqueue(taskInstance);
        return taskInstance;
    }

    public async Task StartAsync()
    {
        running = true;

        while (running)
        {
            taskInstances.RemoveAll(x => x.endTimestamp?.GetAgeInSeconds() > 4);

            while (hasPendingTaskInstances)
            {
                var taskInstance = pendingTaskInstances.Peek();
                await taskInstance.StartAsync();
                pendingTaskInstances.Dequeue();
            }
            await Utils.WaitLongAsync();
        }

        hasStopped = true;
    }

    public void Stop()
    {
        running = false;
    }
}