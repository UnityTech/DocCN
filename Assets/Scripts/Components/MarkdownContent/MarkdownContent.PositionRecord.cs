using System;

namespace Unity.DocZh.Components
{
    public partial class MarkdownContent
    {
        public class PositionRecord
        {
            public string title { get; set; }
            public Func<float> getPosition { get; set; }
        }
    }
}