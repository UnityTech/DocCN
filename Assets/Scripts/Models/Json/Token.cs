using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public class Token
    {
        public string type;
        public string tag;
        public int nesting;
        public int level;
        public List<Token> children;
        public string content;
        public string markup;
        public string info;
        public bool block;
        public bool hidden;
        public List<List<string>> attrs;

        public static Token FromJson(JsonValue obj)
        {
            var children = obj["children"].AsJsonArray
                ?.Select(FromJson)
                .ToList();

            var attrs = obj["attrs"].AsJsonArray
                ?.Select(jsonValue => jsonValue.AsJsonArray.Select(value => (string) value).ToList())
                .ToList();

            return new Token
            {
                type = obj["type"],
                tag = obj["tag"],
                nesting = obj["nesting"],
                level = obj["level"],
                children = children,
                content = obj["content"],
                markup = obj["markup"],
                info = obj["info"],
                block = obj["block"],
                hidden = obj["hidden"],
                attrs = attrs,
            };
        }
    }
}