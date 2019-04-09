using Newtonsoft.Json;

namespace DocCN.Models.Json
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