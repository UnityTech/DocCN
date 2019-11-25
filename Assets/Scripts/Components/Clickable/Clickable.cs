using System;
using Unity.DocZh.Utility;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public class Clickable : StatelessWidget
    {
        public Clickable(
            Widget child = null,
            GestureTapCallback onTap = null,
            Action<bool> hoverChanged = null)
        {
            _child = child;
            _onTap = onTap;
            _hoverChanged = hoverChanged;
        }

        private readonly Widget _child;
        private readonly GestureTapCallback _onTap;
        private readonly Action<bool> _hoverChanged;

        private static void OnPointerEnter(PointerEnterEvent @event, Clickable clickable)
        {
            Bridge.ChangeCursor("pointer");
            clickable._hoverChanged?.Invoke(true);
        }

        private static void OnPointerExit(PointerExitEvent @event, Clickable clickable)
        {
            Bridge.ChangeCursor("default");
            clickable._hoverChanged?.Invoke(false);
        }
        
        public override Widget build(BuildContext buildContext)
        {
            return new Listener(
                onPointerEnter: evt => OnPointerEnter(evt, this),
                onPointerExit: evt => OnPointerExit(evt, this),
                child: new GestureDetector(
                    onTap: _onTap,
                    child: _child
                )
                );
        }
    }
}