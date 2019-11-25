using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class Menu : StatefulWidget
    {
        public Menu(MenuType type) : base(new ObjectKey("menu"))
        {
            _type = type;
        }

        private readonly MenuType _type;

        public override State createState() => new MenuState();
    }
}