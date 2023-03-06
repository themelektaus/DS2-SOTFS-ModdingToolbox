namespace DS2_SOTFS_ModdingToolbox;

public class DSMapStudioProject
{
    public string ProjectName { get; set; } = "";
    public string GameRoot { get; set; } = "";
    public int GameType { get; set; } = 4;
    public JArray PinnedParams { get; set; } = new();
    public JObject PinnedRows { get; set; } = new();
    public JObject PinnedFields { get; set; } = new();
    public bool UseLooseParams { get; set; } = true;
    public bool PartialParams { get; set; } = false;
    public string LastFmgLanguageUsed { get; set; }

    public static DSMapStudioProject LoadFrom(string file)
    {
        return FromJson(ReadText(file));
    }

    public void SaveTo(string file)
    {
        WriteText(file, ToJson());
    }

    public static DSMapStudioProject FromJson(string json)
    {
        return JsonConvert.DeserializeObject<DSMapStudioProject>(json);
    }

    public string ToJson(bool prettyPrint = false)
    {
        return JsonConvert.SerializeObject(this, prettyPrint ? Formatting.Indented : Formatting.None);
    }
}