using System.Windows.Forms;

using D = System.IO.Directory;

namespace DS2_SOTFS_ModdingToolbox;

static class Program
{
    public static readonly TaskManager taskManager = new();

    static int business;
    public static bool isBusy => business > 0;
    public static void IncreaseBusiness() => business++;
    public static void DecreaseBusiness() => business--;

    public static bool dsMapStudioIsRunning => IsProcessRunning("DSMapStudio");
    public static bool gameIsRunning => IsProcessRunning("DarkSoulsII");
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

        D.SetCurrentDirectory(path);

#endif
        var task = taskManager.StartAsync();

        ApplicationConfiguration.Initialize();

        Application.Run(new MainForm());
    }
}