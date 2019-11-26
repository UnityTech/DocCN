namespace Unity.DocZh.Models.Json
{
    //[JsonConverter(typeof(SectionConverter))]
    public interface Section
    {
    }

    public class Description : Section
    {
        public MixedContent[] value { get; set; }
    }

    public class Signature : Section
    {
        public Declaration declaration { get; set; }
        public ReturnType returnType { get; set; }
    }

    public class Declaration
    {
        public string name { get; set; }
        public string type { get; set; }
        public string @namespace { get; set; }
        public string modifiers { get; set; }
        public string obsolete { get; set; }
    }

    public class ReturnType
    {
        public string returnType { get; set; }
        public string displayName { get; set; }
        public string hasLink { get; set; }
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

    /*
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
                        returnType = jObject["value"]["result_type"] == null ? null : serializer.Deserialize<ReturnType>(jObject["value"]["return_type"].CreateReader())
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
    */
}