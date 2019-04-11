using System.Xml.Serialization;

namespace DocCN.Models.Xml
{
    [XmlRoot("xml")]
    public class Highlight
    {
        [XmlElement("em", typeof(HighlightEm))]
        [XmlText(typeof(string))]
        public object[] items { get; set; }
    }

    public class HighlightEm
    {
        [XmlText] public string value { get; set; }
    }
}