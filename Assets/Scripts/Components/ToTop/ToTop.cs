using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class ToTop : StatefulWidget
    {
        public ToTop(
            ScrollController scrollController,
            float? displayThreshold = null,
            Key key = null) : base(key)
        {
            D.assert(_scrollController != null);
            _displayThreshold = displayThreshold;
            _scrollController = scrollController;
        }

        private readonly float? _displayThreshold;
        private readonly ScrollController _scrollController;
        public override State createState() => new ToTopState();
    }
}