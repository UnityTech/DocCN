using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public partial class DropDown<T>
    {
        private class DropDownOverlay : StatelessWidget
        {
            public DropDownOverlay(
                Key key = null,
                float? left = null,
                float? top = null,
                float? right = null,
                float? bottom = null,
                float? width = null,
                List<Widget> children = null,
                Color color = null,
                Border border = null) : base(key)
            {
                _left = left;
                _top = top;
                _right = right;
                _bottom = bottom;
                _width = width;
                _children = children;
                _color = color;
                _border = border;
            }

            private readonly float? _left;
            private readonly float? _top;
            private readonly float? _right;
            private readonly float? _bottom;
            private readonly float? _width;
            private readonly List<Widget> _children;
            private readonly Color _color;
            private readonly Border _border;

            public override Widget build(BuildContext context)
            {
                return new Positioned(
                    left: _left,
                    top: _top,
                    right: _right,
                    bottom: _bottom,
                    child: new Container(
                        width: _width,
                        decoration: new BoxDecoration(
                            color: _color,
                            border: _border
                        ),
                        child: new Column(
                            children: _children
                        )
                    )
                );
            }
        }
    }
}