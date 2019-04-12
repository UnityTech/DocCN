using System;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class SearchBar : StatefulWidget
    {
        public SearchBar(
            Action<string> onSearch = null)
        {
            _onSearch = onSearch;
        }

        public const float Height = 70f; 

        private readonly Action<string> _onSearch;
        public override State createState() => new SearchBarState();
    }
}