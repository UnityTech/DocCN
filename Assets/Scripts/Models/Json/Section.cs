using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DocCN.Utility.Models.Json
{
    [JsonConverter(typeof(SectionConverter))]
    public interface Section
    {
    }

    public class Description : Section
    {
        public MixedContent[] value { get; set; }
    }

    public class Signature : Section
    {
        [JsonProperty("declaration")] public Declaration declaration { get; set; }
        [JsonProperty("return_type")] public ReturnType returnType { get; set; }
    }

    public class Declaration
    {
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("type")] public string type { get; set; }
        [JsonProperty("namespace")] public string @namespace { get; set; }
        [JsonProperty("modifiers")] public string modifiers { get; set; }
        [JsonProperty("obsolete")] public string obsolete { get; set; }
    }

    public class ReturnType
    {
        [JsonProperty("return_type")] public string returnType { get; set; }
        [JsonProperty("display_name")] public string displayName { get; set; }
        [JsonProperty("has_link")] public string hasLink { get; set; }
    }

    public class Summary : Section
    {
        public MixedContent[] value { get; set; }
    }

    public class Example : Section
    {
        public string noCheck { get; set; }
        public string convertExample { get; set; }
        public MixedContent[] javascript { get; set; }
        public MixedContent[] cSharp { get; set; }
    }

    public class SectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // ONLY SUPPORT READ.
            throw new NotImplementedException();
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var valueReader = jObject["value"].CreateReader();
            switch (jObject["type"].ToString())
            {
                case "Signature":
                    return new Signature
                    {
                        declaration = serializer.Deserialize<Declaration>(jObject["value"]["declaration"].CreateReader()),
                        returnType = serializer.Deserialize<ReturnType>(jObject["value"]["return_type"].CreateReader())
                    };
                case "Summary":
                    return new Summary
                        {value = serializer.Deserialize<MixedContent[]>(valueReader)};
                case "Description":
                    return new Description
                        {value = serializer.Deserialize<MixedContent[]>(valueReader)};
                case "Example":
                    return new Example
                    {
                        noCheck = serializer.Deserialize<string>(jObject["value"]["no_check"].CreateReader()),
                        convertExample = serializer.Deserialize<string>(jObject["value"]["convert_example"].CreateReader()),
                        javascript = serializer.Deserialize<MixedContent[]>(jObject["value"]["javascript"].CreateReader()),
                        cSharp = serializer.Deserialize<MixedContent[]>(jObject["value"]["c_sharp"].CreateReader()),
                    };
                default: return null;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(Section));
        }
    }
}