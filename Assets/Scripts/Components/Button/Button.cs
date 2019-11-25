using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class Button : StatefulWidget
    {
        public Button(
            Key key = null,
            GestureTapCallback onTap = null,
            string text = null,
            IconData prefix = null,
            IconData suffix = null
        ) : base(key)
        {
            _onTap = onTap;
            _text = text;
            _prefix = prefix;
            _suffix = suffix;
        }

        private readonly GestureTapCallback _onTap;

        private readonly string _text;

        private readonly IconData _prefix;

        private readonly IconData _suffix;

        public override State createState() => new ButtonState();
    }
}