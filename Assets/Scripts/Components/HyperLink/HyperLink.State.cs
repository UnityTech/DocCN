using Unity.DocZh.Utility;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class HyperLink
    {
        private class HyperLinkState : State<HyperLink>
        {
            public override Widget build(BuildContext context)
            {
                return new Clickable(
                    onTap: () => Bridge.HrefTo(widget._link),
                    child: new Text(
                        widget._text,
                        style: widget._style
                    )
                );
            }
        }
    }
}