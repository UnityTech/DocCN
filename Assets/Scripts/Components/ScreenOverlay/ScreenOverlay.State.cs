using Unity.UIWidgets.gestures;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class ScreenOverlay
    {
        private static readonly Color ScreenOverlayBackgroundColor = new Color(0xff000000);

        public class ScreenOverlayState : State<ScreenOverlay>
        {
            private GestureTapCallback _onTap;
            private void OnTap()
            {
                _onTap?.Invoke();
            }

            public override Widget build(BuildContext context)
            {
                return new Container(
                    color: ScreenOverlayBackgroundColor,
                    child: new GestureDetector(
                        onTap: OnTap,
                        child: widget._child
                    )
                );
            }
            
            public void ClearAll()
            {
                _onTap = null;
            }

            public void AddOnTapListener(GestureTapCallback onTap) => _onTap += onTap;

            public void RemoveOnTapListener(GestureTapCallback onTap) => _onTap -= onTap;
        }
    }
}