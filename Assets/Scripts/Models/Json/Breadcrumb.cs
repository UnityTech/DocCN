using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public class Breadcrumb
    {
        public string content;
        public string link;

        public static Breadcrumb FromJson(JsonValue obj)
        {
            return new Breadcrumb
            {
                content = obj["content"],
                link = obj["link"],
            };
        }
    }
}