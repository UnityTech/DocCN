using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class SearchBar : StatefulWidget
    {
        private class FilterItem : StatefulWidget
        {
            public FilterItem(
                string text = null,
                GestureTapCallback onTap = null,
                Key key = null) : base(key)
            {
                _text = text;
                _onTap = onTap;
            }

            private readonly string _text;

            private readonly GestureTapCallback _onTap;
            public override State createState() => new FilterItemState();

            private static readonly Color NormalColor = new Color(0xff212121);

            private static readonly Color HoverColor = new Color(0xff204f74);
            
            private static readonly Color TransparentColor = new Color(0x00000000);

            private static readonly TextStyle TransparentStyle = new TextStyle(
                color: TransparentColor,
                fontSize: 16f
            );

            private static readonly TextStyle NormalStyle = new TextStyle(
                color: new Color(0xffffffff),
                fontSize: 16f
            );
            
            private static readonly BorderSide TransparentBorderSide =
                new BorderSide(width: 2.0f, color: TransparentColor);

            private class FilterItemState : State<FilterItem>
            {
                private bool _hover;

                public override void initState()
                {
                    _hover = false;
                    base.initState();
                }

                public override Widget build(BuildContext buildContext)
                {
                    return new Clickable(
                        onTap: widget._onTap,
                        hoverChanged: (hover) =>
                        {
                            if (mounted)
                            {
                                setState(() => _hover = hover);
                            }
                        },
                        child: new Container(
                            width: 170.0f,
                            height: 48.0f,
                            decoration: new BoxDecoration(
                                color: _hover ? HoverColor : NormalColor,
                                border: new Border(
                                    top: TransparentBorderSide,
                                    left: TransparentBorderSide,
                                    bottom: TransparentBorderSide
                                )
                            ),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: new List<Widget>
                                {
                                    new Text(
                                        "筛选：",
                                        style: TransparentStyle
                                    ),
                                    new Text(
                                        widget._text,
                                        style: NormalStyle
                                    ),
                                    new Icon(
                                        Style.Icons.MaterialArrowDropDown,
                                        color: TransparentColor,
                                        size: 24f
                                    )
                                }
                            )
                        )
                    );
                }
            }
        }
    }
}