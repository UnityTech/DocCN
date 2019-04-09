using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public partial class Menu : StatefulWidget
    {
        public Menu(string type) : base(new ObjectKey("menu"))
        {
            _type = type;
        }

        public const string TypeManual = "manual_json";
        public const string TypeScripting = "scripting_json";

        private readonly string _type;

        public override State createState() => new MenuState();
    }
}