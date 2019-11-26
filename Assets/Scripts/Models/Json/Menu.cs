using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public class Menu
    {
        public string title;
        public string link;
        public List<Menu> children;
        public bool expanded;

        public static Menu FromJson(JsonValue obj)
        {
            return new Menu
            {
                title = obj["title"],
                link = obj["link"],
                children = obj["children"].AsJsonArray?.Select(FromJson).ToList(),
            };
        } 
    }
}