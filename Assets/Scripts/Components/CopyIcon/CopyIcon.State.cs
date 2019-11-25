using System.Collections.Generic;
using Unity.DocZh.Style;
using Unity.DocZh.Utility;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class CopyIcon
    {
        private static readonly Color NormalColor = new Color(0xffd8d8d8);

        private static readonly Color HoverColor = new Color(0xff2196f3);

        private static readonly TextStyle TextStyle = new TextStyle(
            color: new Color(0xff979797),
            fontSize: 14f
        );

        private static readonly BoxDecoration TextBackground = new BoxDecoration(
            color: new Color(0xffe6e6e6),
            borderRadius: BorderRadius.all(2)
        );

        private class CopyIconState : State<CopyIcon>
        {
            private bool _hover;

            public override void initState()
            {
                base.initState();
                _hover = false;
            }

            public override Widget build(BuildContext context)
            {
                var children = new List<Widget>
                {
                    new Icon(
                        Icons.IconFontCopy,
                        color: _hover ? HoverColor : NormalColor,
                        size: 24
                    )
                };
                if (_hover)
                {
                    children.Add(
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            height: 20,
                            width: 36,
                            decoration: TextBackground,
                            child: new Center(
                                child: new Text(
                                    "复制",
                                    style: TextStyle
                                )
                            )
                        )
                    );
                }

                return new Clickable(
                    hoverChanged: hover => setState(() => _hover = hover),
                    onTap: () => Bridge.CopyText(widget._text),
                    child: new Container(
                        width: 36,
                        child: new Column(
                            children: children
                        )
                    )
                );
            }
        }
    }
}