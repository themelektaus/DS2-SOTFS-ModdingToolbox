using System.Windows.Forms;

using D = System.IO.Directory;

namespace DS2_SOTFS_ModdingToolbox;

static class Program
{
    public static readonly TaskManager taskManager = new();

    public static readonly Logger logger = new(
        minLowLevel:
#if DEBUG
        LogLevel.Debug
#else
        LogLevel.Release
#endif
        , minHighLevel: LogLevel.Release
    );

    public static bool dsMapStudioIsRunning => IsProcessRunning(Lang.System.DS_MAP_STUDIO_PROCESS_NAME);
    public static bool gameIsRunning => IsProcessRunning(Lang.System.DARK_SOULS_2_PROCESS_NAME);
    public static bool externalApplicationIsRunning => dsMapStudioIsRunning || gameIsRunning;

    [STAThread]
    static void Main()
    {
        var currentThread = System.Threading.Thread.CurrentThread;
        var invariantCulture = System.Globalization.CultureInfo.InvariantCulture;
        currentThread.CurrentCulture = invariantCulture;
        currentThread.CurrentUICulture = invariantCulture;

#if DEBUG
        var path = D.GetCurrentDirectory();

        while (path.Length > 3)
        {
            var files = D.GetFiles(path);
            if (files.Any(x => x == Path(path, "DS2-SOTFS-ModdingToolbox.sln")))
                break;

            path = GetFullPath(Path(path, ".."));
        }

        if (path.Length <= 3)
            return;

        D.SetCurrentDirectory(Path(path, "DS2-SOTFS-ModdingToolbox"));

#endif
        ApplicationConfiguration.Initialize();

        var config = Config.Load();

        try
        {
            var languageName = config.languageName;
            if (languageName is null)
                Utils.ChangeToSystemLanguage(config);
            else
                Utils.ChangeLanguage(config, languageName);
            if (config.languageName != languageName)
                config.Save();
        }
        catch (Exception ex)
        {
            Message.CreateError(ex.Message).Show();
        }

        _ = taskManager.StartAsync();

        var mainForm = new MainForm(config);

        if (Message.wasShown)
        {
            mainForm.WindowState = FormWindowState.Minimized;
            mainForm.Show();
            mainForm.WindowState = FormWindowState.Normal;
        }

        Application.Run(mainForm);

        logger.Dispose();
    }
}