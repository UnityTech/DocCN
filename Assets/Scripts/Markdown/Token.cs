using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace DocCN.Markdown
{
    public class Token
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("tag")]
        public string Tag { get; set; }
        
        [JsonProperty("nesting")]
        public int Nesting { get; set; }
        
        [JsonProperty("level")]
        public int Level { get; set; }
        
        [JsonProperty("children")]
        public Token[] Children { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }
        
        [JsonProperty("markup")]
        public string Markup { get; set; }
        
        [JsonProperty("info")]
        public string Info { get; set; }
        
        [JsonProperty("block")]
        public bool Block { get; set; }
        
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        public static void Fetch(string id, Action<Token[]> callback)
        {
            var request = UnityWebRequest.Get($"http://localhost:8000/{id}");
            var asyncOperation = request.SendWebRequest();
            asyncOperation.completed += operation =>
            {
                var content = DownloadHandlerBuffer.GetContent(request);
                var token = JsonConvert.DeserializeObject<Token[]>(content);
                callback.Invoke(token);
            };
            
        }
    }
}