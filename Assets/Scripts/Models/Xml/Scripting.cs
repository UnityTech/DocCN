using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Unity.DocZh.Models.Xml
{
    [Obsolete("Use json model instead.")]
    [XmlRoot("Root")]
    public class Scripting
    {
        [XmlElement("Model")] public Model model { get; set; }
    }

    public class Model
    {
        [XmlAttribute("id")] public string id { get; set; }
        [XmlAttribute("Namespace")] public string @namespace { get; set; }
        [XmlAttribute("Assembly")] public string assembly { get; set; }
        [XmlElement("BaseType")] public Model baseType { get; set; }

        [XmlArray("Constructors")]
        [XmlArrayItem("member")]
        public Member[] constructors { get; set; }

        [XmlArray("StaticFunctions")]
        [XmlArrayItem("member")]
        public Member[] staticFunctions { get; set; }

        [XmlArray("MemberFunctions")]
        [XmlArrayItem("member")]
        public Member[] memberFunctions { get; set; }

        [XmlArray("ProtectedFunctions")]
        [XmlArrayItem("member")]
        public Member[] protectedFunctions { get; set; }

        [XmlArray("Messages")]
        [XmlArrayItem("member")]
        public Member[] messages { get; set; }

        [XmlArray("Events")]
        [XmlArrayItem("member")]
        public Member[] events { get; set; }

        [XmlArray("Delegates")]
        [XmlArrayItem("member")]
        public Member[] delegates { get; set; }

        [XmlArray("Vars")]
        [XmlArrayItem("member")]
        public Member[] vars { get; set; }

        [XmlArray("StaticVars")]
        [XmlArrayItem("member")]
        public Member[] staticVars { get; set; }

        [XmlArray("Operators")]
        [XmlArrayItem("member")]
        public Member[] operators { get; set; }

        [XmlElement("Section")] public Section section { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Namespace = {@namespace}");
            builder.AppendLine($"Assembly = {assembly}");
            builder.AppendLine();
            WriteArrayField(builder, nameof(staticFunctions), staticFunctions);
            WriteArrayField(builder, nameof(memberFunctions), memberFunctions);
            WriteArrayField(builder, nameof(vars), vars);
            return builder.ToString();
        }

        private static void WriteArrayField(StringBuilder builder, string name, IEnumerable<Member> members)
        {
            builder.AppendLine($"{name} = ...");
            foreach (var member in members)
            {
                builder.AppendLine(member.ToString());
            }

            builder.AppendLine();
        }
    }

    public class Section
    {
        [XmlElement("Summary")] public string summary { get; set; }
        [XmlElement("Description")] public MixedContent description { get; set; }
        [XmlElement("Signature")] public Signature signature { get; set; }
        [XmlElement("Example")] public Example example { get; set; }
    }

    public class Example
    {
        [XmlAttribute("nocheck")] public bool noCheck { get; set; }
        [XmlAttribute("convertexample")] public bool convertExample { get; set; }
        [XmlElement("JavaScript")] public MixedContent javaScript { get; set; }
        [XmlElement("CSharp")] public MixedContent cSharp { get; set; }
    }

    public class Signature
    {
        [XmlElement("Declaration")] public Declaration declaration { get; set; }
        [XmlElement("ReturnType")] public string returnType { get; set; }
    }

    public class Declaration
    {
        [XmlAttribute("name")] public string name { get; set; }
        [XmlAttribute("type")] public string type { get; set; }
        [XmlAttribute("namespace")] public string @namespace { get; set; }
        [XmlAttribute("modifiers")] public string modifiers { get; set; }
    }

    public class ParamElement
    {
        [XmlAttribute("name")] public string name { get; set; }
        [XmlAttribute("type")] public string type { get; set; }
    }

    public class Member
    {
        [XmlAttribute("id")] public string id { get; set; }
        [XmlElement("name")] public string name { get; set; }

        [XmlElement("summary")] public MixedContent summary { get; set; }

        public override string ToString()
        {
            return $"[Id = {id}, Name = {name}, Summary = {summary}]";
        }
    }

    public class MixedContent
    {
        [XmlElement("br", typeof(DocumentBreak))]
        [XmlElement("link", typeof(DocumentLink))]
        [XmlElement("image", typeof(DocumentImage))]
        [XmlText(typeof(string))]
        public object[] items { get; set; }
    }

    public class DocumentLink
    {
        [XmlAttribute("ref")] public string @ref { get; set; }

        [XmlText] public string value { get; set; }
    }

    public class DocumentBreak
    {
    }

    public class DocumentImage
    {
        [XmlText] public string url { get; set; }
    }
}