using Newtonsoft.Json;
using System;

using WI = System.Security.Principal.WindowsIdentity;

namespace DS2_SOTFS_ModdingToolbox;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var config = Config.Load();

        var gameFinder = GameFinderUtils.Create(config, saveConfig: false);
        if (gameFinder is null)
            return;

        var taskManager = new TaskManager();
        var taskManagerTask = taskManager.StartAsync();

        var task = new ManagedTask_CalcFolderChecksum(gameFinder.gameFolder);

        task.onDone.AddListener(fileChecksums =>
        {
            var username = WI.GetCurrent()?.Name?.Replace('\\', '-') ?? "_";
            var json = JsonConvert.SerializeObject(fileChecksums);
            FileSystemUtils.WriteText($"ChecksumInfo.{username}.json", json);
        });

        var taskInstance = taskManager.Enqueue(task);

        Message.CreateInfo("Das Programm läuft nun im Hintergrund.\r\nDu wirst benachrichtigt, sobald der Vorgang abgeschlossen ist.").Show();

        taskInstance.WaitAsync().Wait();

        taskManager.Stop();
        taskManagerTask.Wait();

        Message.CreateInfo("Der Vorgang wurde erfolgreich beendet... hoffentlich.\r\nVielen Dank.").Show();
    }
}