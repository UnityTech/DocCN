using Unity.DocZh.Utility.Json;

namespace Unity.DocZh.Models.Json
{
    public class Version
    {
        public string unity_version;
        public int parse_version;

        public static Version FromJson(JsonValue obj)
        {
            return new Version
            {
                unity_version = obj["unity_version"],
                parse_version = obj["parse_version"],
            };
        }
    }
}