using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DS2_SOTFS_ModdingToolbox;

static class Program
{
    [STAThread]
    static void Main()
    {
        if (Config.ds2GameExe is null)
            return;

        if (Utils.FolderExists(Config.ds2GameFolder))
        {
            if (!Utils.FolderExists(Config.ds2GameFolder_Origin))
                Utils.MoveFolder(Config.ds2GameFolder, Config.ds2GameFolder_Origin);
        }
        else if (!Utils.FolderExists(Config.ds2GameFolder_Origin))
        {
            return;
        }

#if DEBUG
        var path = Directory.GetCurrentDirectory();

        while (path.Length > 3)
        {
            var files = Directory.GetFiles(path);
            if (files.Any(x => x == Path.Combine(path, "DS2-SOTFS-ModdingToolbox.sln")))
                break;

            path = Path.GetFullPath(Path.Combine(path, ".."));
        }

        if (path.Length <= 3)
            return;

        Directory.SetCurrentDirectory(path);
#endif
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());

        if (!Utils.FolderExists(Config.ds2GameFolder))
            Utils.MoveFolder(Config.ds2GameFolder_Origin, Config.ds2GameFolder);
    }
}