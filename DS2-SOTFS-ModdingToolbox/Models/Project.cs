namespace DS2_SOTFS_ModdingToolbox;

using static Lang.System;

public class Project : ISelectable
{
    [JsonIgnore]
    public string name => GetFileNameWithoutExtension(folder);

    [JsonIgnore]
    public string path => folder;

    [JsonIgnore]
    public DateTimeOffset? timestamp => GetModificationDate();

    public HashSet<string> activeParams = new();
    public bool useDSMapStudioParams = false;

    [JsonIgnore] public string folder;
    [JsonIgnore] public string parentFolder;
    [JsonIgnore] public string file;
    [JsonIgnore] public string paramFolder;
    [JsonIgnore] public DSMapStudioProject dsMapStudioProject;

    public DateTimeOffset? GetCreationDate()
    {
        var info = GetFileInfo(file);
        if (info.Exists)
            return info.CreationTime;
        return null;
    }

    public DateTimeOffset? GetModificationDate()
    {
        var info = GetFileInfo(file);
        if (info.Exists)
            return info.LastWriteTime;
        return null;
    }

    public static Project Create(string projectsFolder, string projectTemplateFolder, string name, string unpackedGameFolder)
    {
        if (Exists(projectsFolder, name))
            throw new(Lang.Text.PROJECT_ALREADY_EXISTS);

        var projectFolder = Path(projectsFolder, name);
        CreateFolder(projectFolder);

        var projectTemplateFile = Path(projectTemplateFolder, DS_MAP_STUDIO_PROJECT_FILE);

        var dsMapStudioProject = DSMapStudioProject.LoadFrom(projectTemplateFile);
        dsMapStudioProject.ProjectName = name;
        dsMapStudioProject.GameRoot = unpackedGameFolder;
        dsMapStudioProject.SaveTo(Path(projectFolder, DS_MAP_STUDIO_PROJECT_FILE));

        foreach (var source in EnumerateFiles(projectTemplateFolder))
        {
            if (GetFileName(source) == DS_MAP_STUDIO_PROJECT_FILE)
                continue;

            var destination = Path(projectFolder, GetRelativePath(projectTemplateFolder, source));
            CopyFile(source, destination);
        }

        var project = new Project
        {
            file = Path(projectFolder, PROJECT_FILE)
        };
        project.Save();

        return Load(projectsFolder, name);
    }

    public static bool Exists(string projectsFolder, string name)
    {
        return FolderExists(Path(projectsFolder, name));
    }

    public static IEnumerable<Project> LoadAll(string projectsFolder)
    {
        if (!FolderExists(projectsFolder))
            yield break;

        foreach (var folder in GetFolders(projectsFolder))
            yield return Load(projectsFolder, GetFileNameWithoutExtension(folder));
    }

    public static Project Load(string projectsFolder, string name)
    {
        var folder = Path(projectsFolder, name);
        if (!FolderExists(folder))
            return null;

        var dsMapStudioProjectFile = Path(folder, DS_MAP_STUDIO_PROJECT_FILE);
        if (!FileExists(dsMapStudioProjectFile))
            return null;

        var projectFile = Path(folder, PROJECT_FILE);
        if (!FileExists(projectFile))
            new Project { file = projectFile }.Save();

        var project = FromJson(ReadText(projectFile));
        project.folder = folder;
        project.parentFolder = projectsFolder;
        project.file = projectFile;
        project.paramFolder = Path(folder, PARAM_FOLDER);
        project.dsMapStudioProject = DSMapStudioProject.LoadFrom(dsMapStudioProjectFile);

        return project;
    }

    public void Save()
    {
        WriteText(file, ToJson());
    }

    public static Project FromJson(string json)
    {
        return JsonConvert.DeserializeObject<Project>(json);
    }

    public string ToJson(bool prettyPrint = false)
    {
        return JsonConvert.SerializeObject(this, prettyPrint ? Formatting.Indented : Formatting.None);
    }

    public void Backup(string backupFolder)
    {
        if (!FolderExists(folder))
        {
            Program.logger.Log(
                Lang.Title.PROJECT,
                LogType.Error,
                Lang.Title.BACKUP,
                Lang.Text.BACKUP_FAILED.Format(GetRelativePath(folder))
            );
            return;
        }

        backupFolder = Path(backupFolder, Lang.Format.BACKUP_FOLDER.Format(name, DateTimeOffset.Now.Ticks));
        CreateFolder(backupFolder);

        var folderInfo = GetFolderInfo(folder);

        foreach (var fileInfo in EnumerateFiles(folderInfo)
            .Where(x => x.Extension != BAK_FILE_EXT && x.Extension != PREV_FILE_EXT)
        )
        {
            var backupFile = GetRelativePath(folderInfo, fileInfo);
            CopyFile(fileInfo.FullName, Path(backupFolder, backupFile));
        }

        Program.logger.Log(
            Lang.Title.PROJECT,
            LogType.Info,
            Lang.Title.BACKUP,
            Lang.Text.BACKUP_SUCCESS.Format(GetRelativePath(backupFolder))
        );
    }

    public IEnumerable<string> EnumerateProjectFiles(bool includeParams)
    {
        foreach (var file in EnumerateFiles(folder))
        {
            var projectFile = GetRelativePath(folder, file);

            if (includeParams)
            {
                if (projectFile.StartsWith(PARAM_FOLDER))
                    continue;

                if (projectFile.EndsWith(BND_DCX_FILE_EXT))
                    continue;
            }

            if (projectFile.EndsWith(BAK_FILE_EXT) || projectFile.EndsWith(PREV_FILE_EXT))
                continue;

            if (projectFile.EndsWith(JSON_FILE_EXT))
                continue;

            yield return projectFile;
        }
    }

    public string[] EnumerateProjectParamNames()
    {
        return EnumerateFiles(GetFolderInfo(paramFolder))
            .Where(x => x.Extension == PARAM_FILE_EXT)
            .Select(x => GetFileNameWithoutExtension(x.Name))
            .ToArray();
    }
}