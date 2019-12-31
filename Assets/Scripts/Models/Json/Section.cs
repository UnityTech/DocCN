using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public abstract class Section
    {
        public static Section FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            switch (obj["type"].AsString)
            {
                case "Signature":
                    return new Signature
                    {
                        declaration = Declaration.FromJson(obj["value"]["declaration"]),
                        returnType = ReturnType.FromJson(obj["value"]["result_type"]),
                    };
                case "Summary":
                    return new Summary
                    {
                        value = obj["value"].AsJsonArray?.Select(MixedContent.FromJson).ToList(),
                    };
                case "Description":
                    return new Description
                    {
                        value = obj["value"].AsJsonArray?.Select(MixedContent.FromJson).ToList(),
                    };
                case "Example":
                    return Example.FromJson(obj["value"]);
                default: return null;
            }
        }
    }

    public class Description : Section
    {
        public List<MixedContent> value;
    }

    public class Signature : Section
    {
        public Declaration declaration;
        public ReturnType returnType;
    }

    public class Declaration
    {
        public string name;
        public string type;
        public string @namespace;
        public string modifiers;
        public string obsolete;

        public static Declaration FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            return new Declaration
            {
                name = obj["name"],
                type = obj["type"],
                @namespace = obj["namespace"],
                modifiers = obj["modifiers"],
                obsolete = obj["obsolete"],
            };
        }
    }

    public class ReturnType
    {
        public string returnType;
        public string displayName;
        public string hasLink;

        public static ReturnType FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            return new ReturnType
            {
                returnType = obj["return_type"],
                displayName = obj["display_name"],
                hasLink = obj["has_link"],
            };
        }
    }

    public class Summary : Section
    {
        public List<MixedContent> value;
    }

    public class Example : Section
    {
        public string noCheck;
        public string convertExample;
        public List<MixedContent> javascript;
        public List<MixedContent> cSharp;

        public static new Example FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            return new Example
            {
                noCheck = obj["no_check"],
                convertExample = obj["convert_example"],
                javascript = obj["javascript"].AsJsonArray?.Select(MixedContent.FromJson).ToList(),
                cSharp = obj["c_sharp"].AsJsonArray?.Select(MixedContent.FromJson).ToList(),
            };
        }
    }
}