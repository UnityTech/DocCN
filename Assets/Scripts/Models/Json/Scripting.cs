namespace Unity.DocZh.Models.Json
{
    public class Scripting
    {
        public Model model { get; set; }
        public ImageMeta[] imageMetas { get; set; }
    }

    public class Model
    {
        public string @namespace { get; set; }
        public string assembly { get; set; }

        public Member[] staticVars { get; set; }
        public Member[] vars { get; set; }
        public Member[] constructors { get; set; }
        public Member[] memberFunctions { get; set; }
        public Member[] protectedFunctions { get; set; }
        public Member[] staticFunctions { get; set; }
        public Member[] operators { get; set; }
        public Member[] messages { get; set; }
        public Member[] events { get; set; }
        public Member[] delegates { get; set; }

        public Section[][] section { get; set; }

        public Model baseType { get; set; }
    }

    public class Member
    {
        public string id { get; set; }
        public string name { get; set; }
        public MixedContent[] summary { get; set; }
    }

    
}