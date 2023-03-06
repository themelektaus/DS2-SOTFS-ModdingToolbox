namespace DS2_SOTFS_ModdingToolbox;

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
        progress.Report(("Try move original game", .1f));

        var originalGameFolder = gameFinder.gameFolder_Origin;
        if (!FolderExists(originalGameFolder))
        {
            if (FolderExists(gameFinder.gameFolder))
            {
                MoveFolder(gameFinder.gameFolder, gameFinder.gameFolder_Origin);
            }
            else
            {
                progress.Report(("Move original game failed", 1));
                return;
            }
        }

        var unpackedGameFolder = gameFinder.gameFolder_Unpacked;
        if (!FolderExists(unpackedGameFolder))
        {
            progress.Report(("Unpack game", .2f));

            if (!FileExists(uxmSelectiveUnpackExe))
            {
                progress.Report(("UXM Selective Unpack not found", 1));
                return;
            }

            string file;

            foreach (var target in EnumerateFiles(originalGameFolder))
            {
                file = Path(unpackedGameFolder, GetRelativePath(originalGameFolder, target));
                CreateHardLink(file, target);
            }

            progress.Report(("Unpack game", .3f));

            file = Path(unpackedGameFolder, GetFileName(decryptedRegulationFile));
            CopyFile(decryptedRegulationFile, file);

            file = Path(unpackedGameFolder, "DarkSoulsII.exe");
            CopyFile(Path(originalGameFolder, "DarkSoulsII.exe"), file);

            var exePath = GetFullPath(Path(unpackedGameFolder, "DarkSoulsII.exe"));
            await RunAsync(uxmSelectiveUnpackExe, $"\"{exePath}\"");
        }

        if (!FolderExists(unpackedGameFolder))
        {
            progress.Report(("Unpack failed", 1));
            return;
        }

        progress.Report(("Delete Backup", .4f));

        await DeleteFolderAsync(Path(unpackedGameFolder, "_backup"));

        progress.Report(("Recreate game folder", .5f));

        await RecreateFolderAsync(gameFinder.gameFolder);


        if (project is not null)
        {
            if (packRegulationFile)
            {
                progress.Report(("Pack regulation file", .6f));
                await PackRegulationFileAsync(project);
            }
        }

        progress.Report(("Add unpacked files", .7f));

        await AddUnpackedFilesAsync(includeRegulationFile: false);


        if (!useDSMapStudioParams && FolderExists(paramFilesFolder))
        {
            progress.Report(("Add unpacked param files", .8f));
            await AddUnpackedParamFilesAsync(paramFilesFolder);
        }

        if (project is not null)
        {
            progress.Report(("Add project", .9f));
            await AddProjectAsync(project);
        }

        progress.Report(("Done", 1));
    }

    async Task PackRegulationFileAsync(Project project)
    {
        var gameFolder = gameFinder.gameFolder;
        var unpackedGameFolder = gameFinder.gameFolder_Unpacked;

        var source = Path(unpackedGameFolder, "enc_regulation.bnd.dcx");
        var destination = Path(gameFolder, "enc_regulation.bnd.dcx");
        CopyFile(source, destination);

        await RunAsync(yabberDcxExe, @$"""{gameFolder}\enc_regulation.bnd.dcx""");
        await RunAsync(yabberExe, @$"""{gameFolder}\enc_regulation.bnd""");

        DeleteFile(@$"{gameFolder}\enc_regulation.bnd");
        DeleteFile(@$"{gameFolder}\enc_regulation.bnd.dcx");

        foreach (var looseParam in project.activeParams.Where(x => char.IsUpper(x[0])))
        {
            source = @$"{project.folder}\Param\{looseParam}.param";
            destination = @$"{gameFolder}\enc_regulation-bnd\{looseParam}.param";
            CopyFile(source, destination);
        }

        await RunAsync(yabberExe, @$"""{gameFolder}\enc_regulation-bnd""");
        await RunAsync(yabberDcxExe, @$"""{gameFolder}\enc_regulation.bnd""");

        await DeleteFolderAsync(@$"{gameFolder}\enc_regulation-bnd");

        DeleteFile(@$"{gameFolder}\enc_regulation.bnd");
        DeleteFile(@$"{gameFolder}\enc_regulation.bnd-yabber-dcx.xml");
    }

    async Task AddUnpackedFilesAsync(bool includeRegulationFile)
    {
        var unpackedGameFolder = gameFinder.gameFolder_Unpacked;

        foreach (var target in EnumerateFiles(unpackedGameFolder))
        {
            var file = GetRelativePath(unpackedGameFolder, target);

            if (useDSMapStudioParams)
                if (file.StartsWith("Param"))
                    continue;

            if (!includeRegulationFile)
                if (file.Equals("enc_regulation.bnd.dcx"))
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
            await AddFileAsync(project.folder, @$"Param\{param}.param", false);
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