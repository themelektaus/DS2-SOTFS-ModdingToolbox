namespace DS2_SOTFS_ModdingToolbox;

public class ManagedTask_Build : ManagedTask
{
    public override string name
        => Lang.ManagedTask.Name.BUILD.Format(
            GetFileNameWithoutExtension(project.name)
        );

    readonly Config config;
    readonly Project project;

    public ManagedTask_Build(Config config, Project project)
    {
        this.config = config;
        this.project = project;
    }

    public override async Task ProcessAsync(TaskInstance taskInstance)
    {
        var progress = new Progress<(string info, float value)>();
        progress.ProgressChanged += (sender, progres) =>
        {
            taskInstance.info = progres.info;
            taskInstance.progress = progres.value;
        };
        await GameBuilderUtils.BuildAsync(config, project, progress);
    }
}