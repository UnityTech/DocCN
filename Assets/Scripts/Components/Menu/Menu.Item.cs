using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using Json = Unity.DocZh.Models.Json;

namespace Unity.DocZh.Components
{
    public partial class Menu
    {
        private partial class MenuItem : StatefulWidget
        {
            public MenuItem(
                Key key,
                Models.Json.Menu bind,
                MenuState parent,
                bool active,
                bool hasChildren,
                int level = 0
            ) : base(key)
            {
                _bind = bind;
                _parent = parent;
                _level = level;
                _hasChildren = hasChildren;
                _active = active;
            }

            private readonly Models.Json.Menu _bind;

            private readonly int _level;

            private readonly string _text;

            private readonly string _link;

            private readonly MenuState _parent;

            private readonly bool _hasChildren;

            private readonly bool _active;

            public override State createState() => new MenuItemState();
        }
    }
}