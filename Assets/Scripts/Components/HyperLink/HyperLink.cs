using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class HyperLink : StatefulWidget
    {
        public HyperLink(
            string text = null,
            string link = null,
            TextStyle style = null)
        {
            _text = text;
            _link = link;
            _style = style;
        }
        
        private readonly string _text;

        private readonly string _link;

        private readonly TextStyle _style;
        public override State createState() => new HyperLinkState();
    }
}