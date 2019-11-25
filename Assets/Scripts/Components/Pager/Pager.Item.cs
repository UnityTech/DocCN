using Unity.DocZh.Utility;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public partial class Pager
    {
        private class PagerItem : StatefulWidget
        {
            public PagerItem(
                GestureTapCallback onTap = null,
                string label = null,
                bool active = false,
                Key key = null) : base(key)
            {
                _onTap = onTap;
                _label = label;
                _active = active;
            }

            private readonly GestureTapCallback _onTap;

            private readonly string _label;

            private readonly bool _active;

            public override State createState() => new PagerItemState();

            private class PagerItemState : State<PagerItem>
            {
                private static readonly TextStyle TextStyle = new TextStyle(
                    color: new Color(0xffffffff),
                    fontSize: 16f,
                    fontWeight: FontWeight.w500
                );
                
                private static readonly Color ActiveColor = new Color(0xff2196f3);
                private static readonly Color NormalColor = new Color(0xffd8d8d8);
                private static readonly Color HoverColor = new Color(0xff979797);

                private bool _hover;

                public override void initState()
                {
                    base.initState();
                    _hover = false;
                }

                private void OnHoverChanged(bool hover)
                {
                    if (mounted)
                    {
                        setState(() => _hover = hover);
                    }
                }

                public override Widget build(BuildContext buildContext)
                {
                    return new Clickable(
                        onTap: widget._onTap,
                        hoverChanged: OnHoverChanged,
                        child: new Container(
                            margin: EdgeInsets.only(right: 16f),
                            height: 32,
                            width: 32,
                            color: _hover ? HoverColor : widget._active ? ActiveColor : NormalColor,
                            child: new Center(
                                child: new Text(
                                    widget._label,
                                    style: TextStyle
                                )
                            )
                        )
                    );
                }
            }
        }
    }
}