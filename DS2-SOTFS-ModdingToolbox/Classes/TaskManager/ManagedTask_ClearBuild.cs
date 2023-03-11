namespace DS2_SOTFS_ModdingToolbox;

public class ManagedTask_ClearBuild : ManagedTask
{
    public override string name
        => Lang.ManagedTask.Name.CLEAR_BUILD;

    readonly Config config;

    public ManagedTask_ClearBuild(Config config)
    {
        this.config = config;
    }

    public override async Task ProcessAsync(TaskInstance taskInstance)
    {
        taskInstance.info = Lang.ManagedTask.Info.CLEARING;
        await GameBuilderUtils.ClearAsync(config);
        taskInstance.info = Lang.ManagedTask.Info.DONE;
    }
}