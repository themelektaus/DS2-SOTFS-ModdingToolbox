namespace DS2_SOTFS_ModdingToolbox;

public static class GameBuilderUtils
{
    static GameBuilder Create(Config config)
        => new(GameFinderUtils.Create(config));

    public static async Task BuildAsync(Config config, Project project, IProgress<(string info, float value)> progress)
    {
        var gameBuilder = Create(config);
        gameBuilder.decryptedRegulationFile = Data.decryptedRegulationFile;
        gameBuilder.uxmSelectiveUnpackExe = config.uxmSelectiveUnpackExe;

        await gameBuilder.BuildAsync(project, Data.unpackedParamFilesFolder, progress);
    }

    public static async Task ReloadModAsync(Config config, Project project)
    {
        var gameBuilder = Create(config);
        await gameBuilder.AddProjectAsync(project);
    }

    public static async Task ClearAsync(Config config)
    {
        var gameBuilder = Create(config);
        await gameBuilder.ClearAsync();
    }
}