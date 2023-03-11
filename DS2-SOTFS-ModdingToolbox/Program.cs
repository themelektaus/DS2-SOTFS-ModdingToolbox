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

        bool error = false;
        try
        {
            var name = Utils.GetSystemLanguageName();
            Utils.ChangeLanguage(name).Wait();
        }
        catch (Exception ex)
        {
            Message.CreateError(ex.Message).Show();
            error = true;
        }

        _ = taskManager.StartAsync();

        var mainForm = new MainForm();

        if (error)
        {
            mainForm.WindowState = FormWindowState.Minimized;
            mainForm.Show();
            mainForm.WindowState = FormWindowState.Normal;
        }

        Application.Run(mainForm);

        logger.Dispose();
    }
}