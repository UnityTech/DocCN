using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class SearchBar : StatefulWidget
    {
        private class FilterItem : StatefulWidget
        {
            public FilterItem(
                string text = null,
                GestureTapCallback onTap = null,
                FilterItemStylePack stylePack = null,
                Key key = null) : base(key)
            {
                _text = text;
                _onTap = onTap;
                _stylePack = stylePack;
            }

            private readonly string _text;

            private readonly GestureTapCallback _onTap;

            private readonly FilterItemStylePack _stylePack;
            public override State createState() => new FilterItemState();


            private class FilterItemState : State<FilterItem>
            {
                private bool _hover;

                public override void initState()
                {
                    _hover = false;
                    base.initState();
                }

                private static readonly BorderSide TransparentBorderSide = new BorderSide(
                    width: 2.0f,
                    color: new Color(0x00000000)
                );
                
                private static readonly TextStyle TransparentStyle = new TextStyle(
                    color: new Color(0x00000000),
                    fontSize: 16f
                );
                
                private static readonly Color TransparentColor = new Color(0x00000000);

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
                            width: 168.0f,
                            height: 48.0f,
                            decoration: new BoxDecoration(
                                color: _hover ?
                                    widget._stylePack.hoverColor :
                                    widget._stylePack.normalColor,
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
                                        style: widget._stylePack.normalStyle
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