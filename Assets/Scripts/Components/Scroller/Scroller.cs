using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class Scroller : StatefulWidget
    {
        public Scroller(
            Widget child,
            Key key = null
        ) : base(key: key)
        {
            this.child = child;
        }

        private readonly Widget child;

        public override State createState()
        {
            return new ScrollerState();
        }
    }

    
}