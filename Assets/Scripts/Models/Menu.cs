using Newtonsoft.Json;

namespace DocCN.Models
{
    public class Menu
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("link")]
        public string Link { get; set; }
        
        [JsonProperty("children")]
        public Menu[] Children { get; set; }
        
        [JsonIgnore]
        public bool Expanded { get; set; }
    }
}