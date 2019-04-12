using Newtonsoft.Json;

namespace DocCN.Models.Json
{
    public class ManualModel
    {
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("prev")] public Link prev { get; set; }
        [JsonProperty("next")] public Link next { get; set; }
        [JsonProperty("bread_crumb")] public Breadcrumb[] breadcrumbs { get; set; }
        [JsonProperty("tokens")] public Token[] tokens { get; set; }
    }

    public class Link
    {
        [JsonProperty("content")] public string content { get; set; }
        [JsonProperty("link")] public string link { get; set; }
    }
}