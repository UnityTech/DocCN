using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class Loading : StatefulWidget
    {
        public Loading(
            float? size = null,
            Key key = null) : base(key)
        {
            _size = size;
        }

        private readonly float? _size;
        public override State createState() => new LoadingState();
    }
}