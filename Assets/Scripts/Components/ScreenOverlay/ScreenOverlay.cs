using Unity.UIWidgets.widgets;
using UnityEngine;

namespace Unity.DocZh.Components
{
    public partial class ScreenOverlay : StatefulWidget
    {
        public ScreenOverlay(Widget child)
            : base(key: new ObjectKey("screen-overlay"))
        {
            _child = child;
        }

        private readonly Widget _child;

        public override State createState() => new ScreenOverlayState();

        public static ScreenOverlayState of(BuildContext buildContext) 
        {
            var ancestorState = buildContext?.ancestorStateOfType(new TypeMatcher<ScreenOverlayState>());
            return (ScreenOverlayState) ancestorState;
        }
    }
}