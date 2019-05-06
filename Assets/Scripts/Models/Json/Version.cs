using Newtonsoft.Json;

namespace DocCN.Models.Json
{
    public class Version
    {
        [JsonProperty("unity_version")]
        public string unityVersion { get; set; }
        
        [JsonProperty("parse_version")]
        public int parsedVersion { get; set; }
    }
}