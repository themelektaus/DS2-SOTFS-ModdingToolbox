namespace DS2_SOTFS_ModdingToolbox;

using static Lang.GameBuilder;
using static Lang.System;

public class GameBuilder
{
    public GameFinder gameFinder;

    public bool packRegulationFile;
    public bool useDSMapStudioParams;

    public string decryptedRegulationFile;
    public string uxmSelectiveUnpackExe;
    public string yabberExe;
    public string yabberDcxExe;

    public GameBuilder(GameFinder gameFinder)
    {
        this.gameFinder = gameFinder;
    }

    public async Task BuildAsync(Project project, string paramFilesFolder, IProgress<(string info, float value)> progress)
    {
        progress.Report((MOVE_ORIGINAL_GAME, .1f));

        var originalGameFolder = gameFinder.gameFolder_Origin;
        if (!FolderExists(originalGameFolder))
        {
            if (FolderExists(gameFinder.gameFolder))
            {
                MoveFolder(gameFinder.gameFolder, gameFinder.gameFolder_Origin);
            }
            else
            {
                progress.Report((MOVE_ORIGINAL_GAME_FAILED, 1));
                return;
            }
        }

        var unpackedGameFolder = gameFinder.gameFolder_Unpacked;
        if (!FolderExists(unpackedGameFolder))
        {
            progress.Report((UNPACK_GAME, .2f));

            if (!FileExists(uxmSelectiveUnpackExe))
            {
                progress.Report((UXM_NOT_FOUND, 1));
                return;
            }

            string file;

            foreach (var target in EnumerateFiles(originalGameFolder))
            {
                file = Path(unpackedGameFolder, GetRelativePath(originalGameFolder, target));
                CreateHardLink(file, target);
            }

            file = Path(unpackedGameFolder, GetFileName(decryptedRegulationFile));
            CopyFile(decryptedRegulationFile, file);

            file = Path(unpackedGameFolder, DARK_SOULS_2_EXE);
            CopyFile(Path(originalGameFolder, DARK_SOULS_2_EXE), file);

            var exePath = GetFullPath(Path(unpackedGameFolder, DARK_SOULS_2_EXE));
            await RunAsync(uxmSelectiveUnpackExe, $"\"{exePath}\"");
        }

        if (!FolderExists(unpackedGameFolder))
        {
            progress.Report((UNPACK_FAILED, 1));
            return;
        }

        progress.Report((DELETE_BACKUP, .4f));

        await DeleteFolderAsync(Path(unpackedGameFolder, UNPACKED_GAME_BACKUP_FOLDER));

        progress.Report((RECREATE_GAME_FOLDER, .5f));

        await RecreateFolderAsync(gameFinder.gameFolder);


        if (project is not null)
        {
            if (packRegulationFile)
            {
                progress.Report((PACK_REGULATION_FILE, .6f));
                await PackRegulationFileAsync(project);
            }
        }

        progress.Report((ADD_UNPACKED_FILES, .7f));

        await AddUnpackedFilesAsync(includeRegulationFile: false);


        if (!useDSMapStudioParams && FolderExists(paramFilesFolder))
        {
            progress.Report((ADD_UNPACKED_PARAM_FILES, .8f));
            await AddUnpackedParamFilesAsync(paramFilesFolder);
        }

        if (project is not null)
        {
            progress.Report((ADD_PROJECT, .9f));
            await AddProjectAsync(project);
        }

        progress.Report((DONE, 1));
    }

    async Task PackRegulationFileAsync(Project project)
    {
        var gameFolder = gameFinder.gameFolder;
        var unpackedGameFolder = gameFinder.gameFolder_Unpacked;

        var source = Path(unpackedGameFolder, COMPRESSED_REGULATION_FILE);
        var destination = Path(gameFolder, COMPRESSED_REGULATION_FILE);
        CopyFile(source, destination);

        await RunAsync(yabberDcxExe, @$"""{gameFolder}\{COMPRESSED_REGULATION_FILE}""");
        await RunAsync(yabberExe, @$"""{gameFolder}\{REGULATION_FILE}""");

        DeleteFile(@$"{gameFolder}\{REGULATION_FILE}");
        DeleteFile(@$"{gameFolder}\{COMPRESSED_REGULATION_FILE}");

        foreach (var looseParam in project.activeParams.Where(x => char.IsUpper(x[0])))
        {
            source = @$"{project.folder}\{PARAM_FOLDER}\{looseParam}{PARAM_FILE_EXT}";
            destination = @$"{gameFolder}\{REGULATION_FOLDER}\{looseParam}{PARAM_FILE_EXT}";
            CopyFile(source, destination);
        }

        await RunAsync(yabberExe, @$"""{gameFolder}\{REGULATION_FOLDER}""");
        await RunAsync(yabberDcxExe, @$"""{gameFolder}\{REGULATION_FILE}""");

        await DeleteFolderAsync(@$"{gameFolder}\{REGULATION_FOLDER}");

        DeleteFile(@$"{gameFolder}\{REGULATION_FILE}");
        DeleteFile(@$"{gameFolder}\{REGULATION_FILE_YABBER_XML}");
    }

    async Task AddUnpackedFilesAsync(bool includeRegulationFile)
    {
        var unpackedGameFolder = gameFinder.gameFolder_Unpacked;

        foreach (var target in EnumerateFiles(unpackedGameFolder))
        {
            var file = GetRelativePath(unpackedGameFolder, target);

            if (useDSMapStudioParams)
                if (file.StartsWith(PARAM_FOLDER))
                    continue;

            if (!includeRegulationFile)
                if (file.Equals(COMPRESSED_REGULATION_FILE))
                    continue;

            await AddFileAsync(unpackedGameFolder, file, true);
        }
    }

    async Task AddUnpackedParamFilesAsync(string paramFilesFolder)
    {
        foreach (var target in EnumerateFiles(paramFilesFolder))
        {
            var file = GetRelativePath(paramFilesFolder, target);
            await AddFileAsync(paramFilesFolder, file, false);
        }
    }

    public async Task AddProjectAsync(Project project)
    {
        if (!FolderExists(project.folder))
            return;

        foreach (var file in project.EnumerateProjectFiles(includeParams: !useDSMapStudioParams))
            await AddFileAsync(project.folder, file, false);

        if (useDSMapStudioParams)
            return;

        foreach (var param in project.activeParams)
        {
            await AddFileAsync(
                project.folder,
                @$"{PARAM_FOLDER}\{param}{PARAM_FILE_EXT}",
                false
            );
        }
    }

    async Task AddFileAsync(string folder, string file, bool hardlink)
    {
        var gameFolder = gameFinder.gameFolder;

        CreateFolder(gameFolder);

        var gameFile = Path(gameFolder, file);

        var target = Path(folder, file);

        await Task.Run(() =>
        {
            if (hardlink)
            {
                CreateHardLink(gameFile, target);
                return;
            }

            DeleteFile(gameFile);
            CopyFile(target, gameFile);
        });
    }

    public async Task ClearAsync()
    {
        if (!FolderExists(gameFinder.gameFolder_Origin))
            return;

        await DeleteFolderAsync(gameFinder.gameFolder);
        MoveFolder(gameFinder.gameFolder_Origin, gameFinder.gameFolder);
    }
}