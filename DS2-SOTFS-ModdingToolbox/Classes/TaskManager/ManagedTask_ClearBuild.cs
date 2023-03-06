namespace DS2_SOTFS_ModdingToolbox;

public class ManagedTask_ClearBuild : ManagedTask
{
    public override string name => $"Clear Build";

    readonly Config config;

    public ManagedTask_ClearBuild(Config config)
    {
        this.config = config;
    }

    public override async Task ProcessAsync(TaskInstance taskInstance)
    {
        taskInstance.info = "Clearing...";
        await GameBuilderUtils.ClearAsync(config);
        taskInstance.info = "Done";
    }
}