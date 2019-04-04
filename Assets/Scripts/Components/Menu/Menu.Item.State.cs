using System.Collections.Generic;
using DocCN.Style;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public partial class Menu
    {
        private partial class MenuItem
        {
            private class MenuItemState : State<MenuItem>
            {
                private static readonly Color NormalColor = new Color(0x00000000);
                private static readonly Color HoverColor = new Color(0xfff5f5f5);
                private static readonly Color IconColor = new Color(0xffd8d8d8);

                private bool _hover;

                public override void initState()
                {
                    base.initState();
                    _hover = false;
                }

                private void OnPointerEnter(PointerEnterEvent evt)
                {
                    if (mounted)
                    {
                        setState(() =>
                        {
                            _hover = true;
                            Bridge.ChangeCursor("pointer");
                        });
                    }
                }

                private void OnPointerLeave(PointerLeaveEvent evt)
                {
                    if (mounted)
                    {
                        setState(() =>
                        {
                            _hover = false;
                            Bridge.ChangeCursor("default");
                        });
                    }
                }

                private void OnTap()
                {
                    var link = $"/Manual/{widget._bind.link}";
                    LocationUtil.Go(link);
                }

                private void OnExpandTap()
                {
                    widget._parent.UpdateMenu(() => widget._bind.expanded = !widget._bind.expanded);
                }

                public override Widget build(BuildContext buildContext)
                {
                    var children = new List<Widget>
                    {
                        new Expanded(
                            child: new Container(
                                decoration: new BoxDecoration(
                                    border: new Border(
                                        left: new BorderSide(
                                            width: 2f,
                                            color: widget._level > 1 ? IconColor : NormalColor
                                        )
                                    )
                                ),
                                margin: EdgeInsets.only(
                                    left: 32f + 16f * widget._level
                                ),
                                padding: EdgeInsets.only(
                                    top: 4f,
                                    bottom: 12f
                                ),
                                child: new Container(
                                    margin: EdgeInsets.only(left: 14f),
                                    child: new Text(
                                        widget._bind.title,
                                        style: new TextStyle(
                                            fontSize: 16f,
                                            height: 1.5f,
                                            color: new Color(0xff616161)
                                        )
                                    )
                                )
                            )
                        ),
                    };
                    if (widget._hasChildren)
                    {
                        children.Add(new GestureDetector(
                                onTap: OnExpandTap,
                                child: new Container(
                                    padding: EdgeInsets.only(top: 4f),
                                    child: new Icon(
                                        Icons.MaterialExpandMore,
                                        color: IconColor,
                                        size: 24f
                                    )
                                )
                            )
                        );
                    }

                    var self = new Listener(
                        onPointerEnter: OnPointerEnter,
                        onPointerLeave: OnPointerLeave,
                        child: new GestureDetector(
                            onTap: OnTap,
                            child: new Container(
                                color: _hover ? HoverColor : NormalColor,
                                padding: EdgeInsets.only(right: 16f),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    crossAxisAlignment: CrossAxisAlignment.center,
                                    children: children
                                )
                            )
                        )
                    );

                    return self;
                }
            }
        }
    }
}