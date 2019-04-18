using System;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class SearchBar : StatefulWidget
    {
        public SearchBar(
            SearchBarStyle style = SearchBarStyle.normal)
        {
            _style = style;
        }

        private readonly SearchBarStyle _style;

        public const float Height = 70f;

        public override State createState() => new SearchBarState();
    }
}