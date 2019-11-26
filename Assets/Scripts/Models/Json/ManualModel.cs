using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public class ManualModel
    {
        public string name;
        public Link prev;
        public Link next;
        public List<Breadcrumb> bread_crumb;
        public List<Token> tokens;
        public List<ImageMeta> image_meta;
        

        public static ManualModel FromJson(JsonValue obj)
        {
            return new ManualModel
            {
                name = obj["name"],
                prev = Link.FromJson(obj["prev"]),
                next = Link.FromJson(obj["next"]),
                tokens = obj["tokens"].AsJsonArray?.Select(Token.FromJson).ToList(),
                bread_crumb = obj["bread_crumb"].AsJsonArray?.Select(Breadcrumb.FromJson).ToList(),
                image_meta = obj["image_meta"].AsJsonArray?.Select(ImageMeta.FromJson).ToList(),
            };
        }
    }

    public class Link
    {
        public string content;
        public string link;

        public static Link FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            return new Link
            {
                content = obj["content"],
                link = obj["link"],
            };
        }
    }

    public class ImageMeta
    {
        public string name;
        public float width;
        public float height;

        public static ImageMeta FromJson(JsonValue obj)
        {
            if (obj.IsNull)
            {
                return null;
            }
            return new ImageMeta
            {
                name = obj["name"],
                width = obj["width"],
                height = obj["height"],
            };
        }
    }
}