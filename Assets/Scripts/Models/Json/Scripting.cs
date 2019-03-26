using DocCN.Models.Xml;
using Newtonsoft.Json;

namespace DocCN.Models.Json
{
    public class Scripting
    {
    }

    public class Model
    {
        [JsonProperty("namespace")] public string @namespace { get; set; }
        [JsonProperty("assembly")] public string assembly { get; set; }
        [JsonProperty("static_vars")] public Member[] staticVars { get; set; }
        [JsonProperty("vars")] public Member[] vars { get; set; }
        [JsonProperty("section")] public Section section { get; set; }
    }

    public class Member
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("summary")] public MixedContent summary { get; set; }
    }

    public class MixedContent
    {
        
    }

    public class Section
    {
        
    }
}