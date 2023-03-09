﻿namespace DS2_SOTFS_ModdingToolbox;

public class ManagedTask_CalcFolderChecksum : ManagedTask
{
    public override string name => $"Calc Folder Checksum ({GetFileName(gameFolder)})";

    public readonly EventListener<HashSet<FileChecksum>, string> onDone = new();

    readonly string gameFolder;

    public ManagedTask_CalcFolderChecksum(string gameFolder)
    {
        this.gameFolder = gameFolder;
    }

    public override async Task ProcessAsync(TaskInstance taskInstance)
    {
        var fileChecksums = new HashSet<FileChecksum>();

        var files = EnumerateFiles(gameFolder)
            .Where(x => !x.EndsWith("userconfig.properties"))
            .ToList();

        foreach (var file in files)
        {
            var relativeFile = GetRelativePath(gameFolder, file);
            taskInstance.info = relativeFile;

            string checksum;
            if (file.EndsWith(".bdt"))
                checksum = Utils.CalcChecksum(BitConverter.GetBytes(GetFileInfo(file).Length));
            else
                checksum = await Task.Run(() => CalcFileChecksum(file));

            fileChecksums.Add(new(
                relativeFile,
                checksum
            ));
            taskInstance.progress = (float) fileChecksums.Count / files.Count;
        }

        var info = onDone.Invoke(fileChecksums);
        if (info is not null)
            taskInstance.info = info;
    }
}