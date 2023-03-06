namespace DS2_SOTFS_ModdingToolbox;

public class Project
{
    public HashSet<string> activeParams = new();
    public bool useDSMapStudioParams = false;

    [JsonIgnore] public string folder;
    [JsonIgnore] public string parentFolder;
    [JsonIgnore] public string file;
    [JsonIgnore] public string paramFolder;
    [JsonIgnore] public DSMapStudioProject dsMapStudioProject;

    [JsonIgnore]
    public string name => GetFileNameWithoutExtension(folder);

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
            throw new("Project already exists.");

        var projectFolder = Path(projectsFolder, name);
        CreateFolder(projectFolder);

        var projectTemplateFile = Path(projectTemplateFolder, "project.json");

        var dsMapStudioProject = DSMapStudioProject.LoadFrom(projectTemplateFile);
        dsMapStudioProject.ProjectName = name;
        dsMapStudioProject.GameRoot = unpackedGameFolder;
        dsMapStudioProject.SaveTo(Path(projectFolder, "project.json"));

        foreach (var source in EnumerateFiles(projectTemplateFolder))
        {
            if (GetFileName(source) == "project.json")
                continue;

            var destination = Path(projectFolder, GetRelativePath(projectTemplateFolder, source));
            CopyFile(source, destination);
        }

        var project = new Project
        {
            file = Path(projectFolder, "project.extended.json")
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

        var dsMapStudioProjectFile = Path(folder, "project.json");
        if (!FileExists(dsMapStudioProjectFile))
            return null;

        var projectFile = Path(folder, "project.extended.json");
        if (!FileExists(projectFile))
            new Project { file = projectFile }.Save();

        var project = FromJson(ReadText(projectFile));
        project.folder = folder;
        project.parentFolder = projectsFolder;
        project.file = projectFile;
        project.paramFolder = Path(folder, "Param");
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
            return;

        backupFolder = Path(backupFolder, $"{name}-{DateTimeOffset.Now.Ticks}");
        CreateFolder(backupFolder);

        var folderInfo = GetFolderInfo(folder);

        foreach (var fileInfo in EnumerateFiles(folderInfo)
            .Where(x => x.Extension != ".bak" && x.Extension != ".prev")
        )
        {
            var backupFile = GetRelativePath(folderInfo, fileInfo);
            CopyFile(fileInfo.FullName, Path(backupFolder, backupFile));
        }
    }

    public IEnumerable<string> EnumerateProjectFiles(bool includeParams)
    {
        foreach (var file in EnumerateFiles(folder))
        {
            var projectFile = GetRelativePath(folder, file);

            if (includeParams)
            {
                if (projectFile.StartsWith("Param"))
                    continue;

                if (projectFile.EndsWith(".bnd.dcx"))
                    continue;
            }

            if (projectFile.EndsWith(".bak") || projectFile.EndsWith(".prev"))
                continue;

            if (projectFile.EndsWith(".json"))
                continue;

            yield return projectFile;
        }
    }

    public string[] EnumerateProjectParamNames()
    {
        return EnumerateFiles(GetFolderInfo(paramFolder))
            .Where(x => x.Extension == ".param")
            .Select(x => GetFileNameWithoutExtension(x.Name))
            .ToArray();
    }
}