using Newtonsoft.Json;

namespace Unity.DocZh.Models.Json
{
    public class ManualModel
    {
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("prev")] public Link prev { get; set; }
        [JsonProperty("next")] public Link next { get; set; }
        [JsonProperty("bread_crumb")] public Breadcrumb[] breadcrumbs { get; set; }
        [JsonProperty("tokens")] public Token[] tokens { get; set; }
        [JsonProperty("image_meta")] public ImageMeta[] imageMetas { get; set; }
    }

    public class Link
    {
        [JsonProperty("content")] public string content { get; set; }
        [JsonProperty("link")] public string link { get; set; }
    }

    public class ImageMeta
    {
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("width")] public float width { get; set; }
        [JsonProperty("height")] public float height { get; set; }
    }
}