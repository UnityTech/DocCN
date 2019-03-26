using Newtonsoft.Json;

namespace DocCN.Models.Json
{   
    public class Token
    {
        [JsonProperty("type")]
        public string type { get; set; }
        
        [JsonProperty("tag")]
        public string tag { get; set; }
        
        [JsonProperty("nesting")]
        public int nesting { get; set; }
        
        [JsonProperty("level")]
        public int level { get; set; }
        
        [JsonProperty("children")]
        public Token[] children { get; set; }
        
        [JsonProperty("content")]
        public string content { get; set; }
        
        [JsonProperty("markup")]
        public string markup { get; set; }
        
        [JsonProperty("info")]
        public string info { get; set; }
        
        [JsonProperty("block")]
        public bool block { get; set; }
        
        [JsonProperty("hidden")]
        public bool hidden { get; set; }
        
        [JsonProperty("attrs")]
        public string[][] attrs { get; set; }
    }
}