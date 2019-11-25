using System;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class SearchBar : StatefulWidget
    {
        public SearchBar(
            SearchBarStyle style = SearchBarStyle.normal,
            DropDownOverlayType filterDropDownOverlayType = DropDownOverlayType.scrollable)
        {
            _style = style;
            _filterDropDownOverlayType = filterDropDownOverlayType;
        }

        private readonly SearchBarStyle _style;

        private readonly DropDownOverlayType _filterDropDownOverlayType;

        public const float Height = 70f;

        public override State createState() => new SearchBarState();
    }
}