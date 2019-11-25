using Newtonsoft.Json;

namespace Unity.DocZh.Models.Json
{
    public class Breadcrumb
    {
        [JsonProperty("content")] public string content { get; set; }
        [JsonProperty("link")] public string link { get; set; }
    }
}