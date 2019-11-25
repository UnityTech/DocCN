using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class ScrollableOverlay : StatefulWidget
    {
        public ScrollableOverlay (Widget child)
        {
            _child = child;
        }
        
        private readonly Widget _child;
        
        public static ScrollableOverlayState of(BuildContext context)
        {
            var result =
                (ScrollableOverlayState) context.ancestorStateOfType(new TypeMatcher<ScrollableOverlayState>());
            return result;
        }

        public override State createState() => new ScrollableOverlayState();
    }
}