using Newtonsoft.Json;

namespace DocCN.Models.Json
{
    public class SearchResults
    {
        [JsonProperty("currentPage")] public int currentPage { get; set; }
        [JsonProperty("items")] public SearchResultItem[] items { get; set; }
        [JsonProperty("pages")] public int[] pages { get; set; }
        [JsonProperty("total")] public int total { get; set; }
        [JsonProperty("totalPages")] public int totalPages { get; set; }
    }

    public class SearchResultItem
    {
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("type")] public string type { get; set; }
        [JsonProperty("version")] public string version { get; set; }
        [JsonProperty("highlight")] public string highlight { get; set; }
        [JsonProperty("bread_crumb")] public Breadcrumb[] breadcrumbs { get; set; }
    }
}