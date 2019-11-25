using System.Linq;
using Unity.DocZh.Models.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Components
{
    public partial class Breadcrumbs
    {
        private class ClickableText : StatefulWidget
        {
            public ClickableText(
                GestureTapCallback onTap = null,
                TextStyle normalStyle = null,
                TextStyle hoverStyle = null,
                string text = null,
                Key key = null) : base(key)
            {
                _onTap = onTap;
                _normalStyle = normalStyle;
                _hoverStyle = hoverStyle;
                _text = text;
            }

            private readonly GestureTapCallback _onTap;

            private readonly TextStyle _normalStyle;

            private readonly TextStyle _hoverStyle;

            private readonly string _text;

            public override State createState() => new ClickableTextState();

            private class ClickableTextState : State<ClickableText>
            {
                private bool _hover;

                public override Widget build(BuildContext context)
                {
                    return new Clickable(
                        onTap: widget._onTap,
                        hoverChanged: hover =>
                        {
                            if (mounted)
                            {
                                setState(() => _hover = hover);
                            }
                        },
                        child: new Text(
                            widget._text,
                            style: _hover ? widget._hoverStyle : widget._normalStyle
                        )
                    );
                }
            }
        }
    }
}