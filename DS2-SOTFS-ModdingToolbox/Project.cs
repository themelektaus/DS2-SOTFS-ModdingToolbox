using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DS2_SOTFS_ModdingToolbox;

public class Project
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

    public static Project FromJson(string json)
    {
        return JsonConvert.DeserializeObject<Project>(json);
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}