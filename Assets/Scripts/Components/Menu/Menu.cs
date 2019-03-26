using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class Menu : StatefulWidget
    {
        public Menu() : base(new ObjectKey("menu"))
        {
        }

        public override State createState() => new MenuState();
    }
}