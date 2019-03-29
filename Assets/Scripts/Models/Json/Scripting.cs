using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DocCN.Models.Json
{
    public class Scripting
    {
        [JsonProperty("model")] public Model model { get; set; }
    }

    public class Model
    {
        [JsonProperty("namespace")] public string @namespace { get; set; }
        [JsonProperty("assembly")] public string assembly { get; set; }

        [JsonProperty("static_vars")] public Member[] staticVars { get; set; }
        [JsonProperty("vars")] public Member[] vars { get; set; }
        [JsonProperty("constructors")] public Member[] constructors { get; set; }
        [JsonProperty("member_functions")] public Member[] memberFunctions { get; set; }
        [JsonProperty("protected_functions")] public Member[] protectedFunctions { get; set; }
        [JsonProperty("static_functions")] public Member[] staticFunctions { get; set; }
        [JsonProperty("operators")] public Member[] operators { get; set; }
        [JsonProperty("messages")] public Member[] messages { get; set; }
        [JsonProperty("events")] public Member[] events { get; set; }
        [JsonProperty("delegates")] public Member[] delegates { get; set; }

        [JsonProperty("section")] public Section[][] section { get; set; }

        [JsonProperty("inherits_from")] public string inheritsFrom { get; set; }
    }

    public class Member
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("summary")] public MixedContent[] summary { get; set; }
    }

    
}