namespace Unity.DocZh.Models.Json
{
    public class Token
    {
        public string type { get; set; }

        public string tag { get; set; }

        public int nesting { get; set; }

        public int level { get; set; }

        public Token[] children { get; set; }

        public string content { get; set; }

        public string markup { get; set; }

        public string info { get; set; }

        public bool block { get; set; }

        public bool hidden { get; set; }

        public string[][] attrs { get; set; }
    }
}