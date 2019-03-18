using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public class Button : StatefulWidget
    {
        public Button(
            Key key = null,
            GestureTapCallback onTap = null,
            string text = null,
            IconData prefix = null,
            IconData suffix = null
        ) : base(key)
        {
            this.onTap = onTap;
            this.text = text;
            this.prefix = prefix;
            this.suffix = suffix;
        }

        internal readonly GestureTapCallback onTap;

        internal readonly string text;

        internal readonly IconData prefix;

        internal readonly IconData suffix;

        public override State createState() => new ButtonState();
    }

    internal class ButtonState : State<Button>
    {
        private bool hover;

        private static readonly Color MAIN_COLOR = new Color(0xff3c83d4);
        private static readonly Color NORMAL_COLOR = new Color(0xffffffff);

        public override void initState()
        {
            base.initState();
            hover = false;
        }

        private void OnPointerEnter(PointerEnterEvent @event)
        {
            if (!mounted)
            {
                return;
            }

            setState(() => { hover = true; });
            Bridge.ChangeCursor("pointer");
        }

        private void OnPointerLeave(PointerLeaveEvent @event)
        {
            if (!mounted)
            {
                return;
            }

            setState(() => hover = false);
            Bridge.ChangeCursor("pointer");
        }

        public override Widget build(BuildContext context)
        {
            var children = new List<Widget>();
            if (widget.prefix != null)
            {
                children.Add(
                    new Container(
                        margin: EdgeInsets.only(right: 20f),
                        child: new Icon(
                            widget.prefix,
                            color: MAIN_COLOR
                        )
                    )
                );
            }

            children.Add(
                new Text(
                    widget.text,
                    style: new TextStyle(
                        color: hover ? NORMAL_COLOR : MAIN_COLOR,
                        fontSize: 16f
                    )
                )
            );
            if (widget.suffix != null)
            {
                children.Add(
                    new Container(
                        margin: EdgeInsets.only(left: 20f),
                        child: new Icon(
                            widget.suffix,
                            color: MAIN_COLOR
                        )
                    )
                );
            }

            return new GestureDetector(
                onTap: () => widget.onTap?.Invoke(),
                child: new Listener(
                    onPointerEnter: OnPointerEnter,
                    onPointerLeave: OnPointerLeave,
                    child: new Container(
                        height: 48f,
                        padding: EdgeInsets.symmetric(horizontal: 16f),
                        decoration: new BoxDecoration(
                            border: Border.all(
                                color: MAIN_COLOR,
                                width: 1f
                            ),
                            color: hover ? MAIN_COLOR : NORMAL_COLOR,
                            borderRadius: BorderRadius.all(3f)
                        ),
                        child: new Row(
                            mainAxisSize: MainAxisSize.min,
                            children: children
                        )
                    )
                )
            );
        }
    }
}