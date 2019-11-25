using Newtonsoft.Json;

namespace Unity.DocZh.Models.Json
{
    public class Version
    {
        [JsonProperty("unity_version")]
        public string unityVersion { get; set; }
        
        [JsonProperty("parse_version")]
        public int parsedVersion { get; set; }
    }
}