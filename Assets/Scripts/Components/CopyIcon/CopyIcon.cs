using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class CopyIcon : StatefulWidget
    {
        public CopyIcon(
            string text,
            Key key = null) : base(key)
        {
            _text = text;
        }

        private readonly string _text;
        public override State createState() => new CopyIconState();
    }
}