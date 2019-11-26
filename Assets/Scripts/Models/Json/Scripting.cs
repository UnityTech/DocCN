using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public class Scripting
    {
        public Model model;
        public List<ImageMeta> imageMetas;

        public static Scripting FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            return new Scripting
            {
                model = Model.FromJson(obj["model"]),
                imageMetas = obj["image_meta"].AsJsonArray?.Select(ImageMeta.FromJson).ToList(),
            };
        }
    }

    public class Model
    {
        public string @namespace;
        public string assembly;
        public List<Member> staticVars;
        public List<Member> vars;
        public List<Member> constructors;
        public List<Member> memberFunctions;
        public List<Member> protectedFunctions;
        public List<Member> staticFunctions;
        public List<Member> operators;
        public List<Member> messages;
        public List<Member> events;
        public List<Member> delegates;
        public List<List<Section>> section;
        public Model baseType;

        public static Model FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            return new Model
            {
                @namespace = obj["namespace"],
                assembly = obj["assembly"],
                staticVars = obj["static_vars"].AsJsonArray?.Select(Member.FromJson).ToList(),
                vars = obj["vars"].AsJsonArray?.Select(Member.FromJson).ToList(),
                constructors = obj["constructors"].AsJsonArray?.Select(Member.FromJson).ToList(),
                memberFunctions = obj["member_functions"].AsJsonArray?.Select(Member.FromJson).ToList(),
                protectedFunctions = obj["protected_functions"].AsJsonArray?.Select(Member.FromJson).ToList(),
                staticFunctions = obj["static_functions"].AsJsonArray?.Select(Member.FromJson).ToList(),
                operators = obj["operators"].AsJsonArray?.Select(Member.FromJson).ToList(),
                messages = obj["messages"].AsJsonArray?.Select(Member.FromJson).ToList(),
                events = obj["events"].AsJsonArray?.Select(Member.FromJson).ToList(),
                delegates = obj["delegates"].AsJsonArray?.Select(Member.FromJson).ToList(),
                section = obj["section"].AsJsonArray?
                    .Select(value => value.AsJsonArray?.Select(Section.FromJson).ToList())
                    .ToList(),
                baseType = FromJson(obj["base_type"]),
            };
        }
    }

    public class Member
    {
        public string id;
        public string name;
        public List<MixedContent> summary;

        public static Member FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            return new Member
            {
                id = obj["id"],
                name = obj["name"],
                summary = obj["summary"].AsJsonArray?.Select(MixedContent.FromJson).ToList(),
            };
        }
    }

    
}