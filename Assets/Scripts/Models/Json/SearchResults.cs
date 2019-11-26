using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public class SearchResults
    {
        public int currentPage;
        public List<SearchResultItem> items;
        public List<int> pages;
        public int total;
        public int totalPages;

        public static SearchResults FromJson(JsonValue obj)
        {
            return new SearchResults
            {
                currentPage = obj["currentPage"],
                items = obj["items"].AsJsonArray?.Select(SearchResultItem.FromJson).ToList(),
                pages = obj["pages"].AsJsonArray?.Select(value => (int) value).ToList(),
                total = obj["total"],
                totalPages = obj["totalPages"],
            };
        }
    }
    
    public class SearchResultItem
    {
        public string name;
        public string id;
        public string type;
        public string version;
        public string highlight;
        public List<Breadcrumb> breadcrumbs;

        public static SearchResultItem FromJson(JsonValue obj)
        {
            return new SearchResultItem
            {
                name = obj["name"],
                id = obj["id"],
                type = obj["type"],
                version = obj["version"],
                highlight = obj["highlight"],
                breadcrumbs = obj["breadcrumbs"].AsJsonArray?.Select(Breadcrumb.FromJson).ToList(),
            };
        }
    }
}