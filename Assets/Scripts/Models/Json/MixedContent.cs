namespace Unity.DocZh.Models.Json
{
    /*
    public class MixedContentConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // ONLY SUPPORT READ.
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"].ToString();
            var valueReader = jObject["value"].CreateReader();
            switch (type)
            {
                case "cdata":
                    return new DocumentCharData
                        {content = serializer.Deserialize<string>(valueReader)};
                case "link":
                    return new DocumentTagLink
                    {
                        content = serializer.Deserialize<string>(jObject["value"]["content"].CreateReader()),
                        @ref = serializer.Deserialize<string>(jObject["value"]["ref"].CreateReader())
                    };
                case "b":
                    return new DocumentTagBold
                    {
                        content = serializer.Deserialize<string>(jObject["value"]["content"].CreateReader())
                    };
                case "br":
                    return new DocumentTagBreak();
                case "image":
                    return new DocumentTagImage
                    {
                        name = serializer.Deserialize<string>(jObject["value"]["url"].CreateReader())
                    };
                case "i":
                    return new DocumentTagItalic
                    {
                        content = serializer.Deserialize<string>(jObject["value"]["content"].CreateReader())
                    };            
                default:
                    return null;
            }
        }

        public override bool CanConvert(Type objectType) => objectType.IsSubclassOf(typeof(MixedContent));
    }
    */

    // [JsonConverter(typeof(MixedContentConverter))]
    public interface MixedContent
    {
    }
    
    public class DocumentCharData : MixedContent
    {
        public string content { get; set; }

        public override string ToString() => content;
    }

    public class DocumentTagLink : MixedContent
    {
        public string @ref { get; set; }
        public string content { get; set; }
        
        public override string ToString() => content;        
    }

    public class DocumentTagA : MixedContent
    {
        public string @ref { get; set; }
        public string content { get; set; }
        public override string ToString() => content;

    }

    public class DocumentTagImage : MixedContent
    {
        public string name { get; set; }
        public override string ToString() => string.Empty;
    }

    public class DocumentTagTeletype : MixedContent
    {
        public string content { get; set; }
        public override string ToString() => content;
    }

    public class DocumentTagItalic : MixedContent
    {
        public string content { get; set; }
        public override string ToString() => content;

    }

    public class DocumentTagBold : MixedContent
    {
        public string content { get; set; }
        public override string ToString() => content;
    }

    public class DocumentTagVarName : MixedContent
    {
        public string content { get; set; }
        public override string ToString() => content;
    }

    public class DocumentTagMonoType : MixedContent
    {
        public string content { get; set; }
        public override string ToString() => content;
    }

    public class DocumentTagNote : MixedContent
    {
        public string content { get; set; }
        public override string ToString() => content;
    }

    public class DocumentTagBreak : MixedContent
    {
        public override string ToString() => "\n";
    }
}