using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class Button
    {
        private class ButtonState : State<Button>
        {
            private bool _hover;

            private static readonly Color MainColor = new Color(0xff3c83d4);
            private static readonly Color NormalColor = new Color(0xffffffff);

            public override void initState()
            {
                base.initState();
                _hover = false;
            }

            private void OnHoverChanged(bool hover)
            {
                if (!mounted)
                {
                    return;
                }

                setState(() => _hover = hover);
            }

            public override Widget build(BuildContext context)
            {
                var children = new List<Widget>();
                if (widget._prefix != null)
                {
                    children.Add(
                        new Container(
                            margin: EdgeInsets.only(right: 20f),
                            child: new Icon(
                                widget._prefix,
                                color: _hover ? NormalColor : MainColor
                            )
                        )
                    );
                }

                children.Add(
                    new Expanded(
                        child: new Text(
                            widget._text,
                            style: new TextStyle(
                                color: _hover ? NormalColor : MainColor,
                                fontSize: 16f
                            ),
                            maxLines: 5
                        )
                    )
                );
                if (widget._suffix != null)
                {
                    children.Add(
                        new Container(
                            margin: EdgeInsets.only(left: 20f),
                            child: new Icon(
                                widget._suffix,
                                color: _hover ? NormalColor : MainColor
                            )
                        )
                    );
                }

                return new Clickable(
                    onTap: widget._onTap,
                    hoverChanged: OnHoverChanged,
                    child: new Container(
                        padding: EdgeInsets.symmetric(
                            horizontal: 16f,
                            vertical: 12f
                        ),
                        decoration: new BoxDecoration(
                            border: Border.all(
                                color: MainColor,
                                width: 1f
                            ),
                            color: _hover ? MainColor : NormalColor,
                            borderRadius: BorderRadius.all(3f)
                        ),
                        child: new Row(
                            mainAxisSize: MainAxisSize.min,
                            crossAxisAlignment: CrossAxisAlignment.center,
                            children: children
                        )
                    )
                );
            }
        }
    }
}