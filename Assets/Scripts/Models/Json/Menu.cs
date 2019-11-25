using Newtonsoft.Json;

namespace Unity.DocZh.Models.Json
{
    public class Menu
    {
        [JsonProperty("title")]
        public string title { get; set; }
        
        [JsonProperty("link")]
        public string link { get; set; }
        
        [JsonProperty("children")]
        public Menu[] children { get; set; }
        
        [JsonIgnore]
        public bool expanded { get; set; }
    }
}