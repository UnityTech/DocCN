using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public abstract class MixedContent
    {
        public static MixedContent FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }

            var type = obj["type"].AsString;
            switch (type)
            {
                case "cdata":
                    return new DocumentCharData
                    {
                        content = obj["value"],
                    };
                case "link":
                    return new DocumentTagLink
                    {
                        content = obj["value"]["content"],
                        @ref = obj["value"]["ref"],
                    };
                case "b":
                    return new DocumentTagBold
                    {
                        content = obj["value"]["content"],
                    };
                case "br":
                    return new DocumentTagBreak();
                case "image":
                    return new DocumentTagImage
                    {
                        name = obj["value"]["url"],
                    };
                case "i":
                    return new DocumentTagItalic
                    {
                        content = obj["value"]["content"]
                    };
                case "monotype":
                    return new DocumentTagMonoType
                    {
                        content = obj["value"]["content"]
                    };
                default:
                    return null;
            }
        }
    }
    
    public class DocumentCharData : MixedContent
    {
        public string content;

        public override string ToString() => content;
    }

    public class DocumentTagLink : MixedContent
    {
        public string @ref;
        public string content;
        
        public override string ToString() => content;        
    }

    public class DocumentTagA : MixedContent
    {
        public string @ref;
        public string content;
        public override string ToString() => content;
    }

    public class DocumentTagImage : MixedContent
    {
        public string name;
        public override string ToString() => string.Empty;
    }

    public class DocumentTagTeletype : MixedContent
    {
        public string content;
        public override string ToString() => content;
    }

    public class DocumentTagItalic : MixedContent
    {
        public string content;
        public override string ToString() => content;

    }

    public class DocumentTagBold : MixedContent
    {
        public string content;
        public override string ToString() => content;
    }

    public class DocumentTagVarName : MixedContent
    {
        public string content;
        public override string ToString() => content;
    }

    public class DocumentTagMonoType : MixedContent
    {
        public string content;
        public override string ToString() => content;
    }

    public class DocumentTagNote : MixedContent
    {
        public string content;
        public override string ToString() => content;
    }

    public class DocumentTagBreak : MixedContent
    {
        public override string ToString() => "\n";
    }
}