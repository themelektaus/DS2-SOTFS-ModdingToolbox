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

        if (!Utils.IsValidPath(Config.ds2Folder))
        {
            var dialog = new OpenFileDialog
            {
                Filter = "DarkSoulsII.exe|DarkSoulsII.exe"
            };
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            if (!Utils.FileExists(dialog.FileName))
            {
                Utils.ShowErrorMessage("Nothing selected.");
                return;
            }

            var exeFile = new FileInfo(Path.GetFullPath(dialog.FileName));
            if (exeFile.Directory.Name != "Game")
            {
                Utils.ShowErrorMessage("DarkSoulsII.exe should be inside a folder called \"Game\".");
                return;
            }

            var ds2Folder = exeFile.Directory.Parent.FullName;
            Config.ds2Folder = ds2Folder;
        }

        if (Utils.FolderExists(Config.ds2GameFolder))
        {
            if (!Utils.FolderExists(Config.ds2GameFolder_Origin))
                Utils.MoveFolder(Config.ds2GameFolder, Config.ds2GameFolder_Origin);
        }
        else if (!Utils.FolderExists(Config.ds2GameFolder_Origin))
        {
            return;
        }

        Config.instance.customDs2Folder = Config.ds2Folder;
        Config.instance.Save();

        Application.Run(new MainForm());

        if (!Utils.FolderExists(Config.ds2GameFolder))
            Utils.MoveFolder(Config.ds2GameFolder_Origin, Config.ds2GameFolder);
    }
}