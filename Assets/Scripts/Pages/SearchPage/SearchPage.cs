using Unity.DocZh.Components;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Pages
{
    public partial class SearchPage : StatefulWidget
    {
        public SearchPage(
            string keyword = "",
            int page = 0,
            SearchBar.FilterType filterType = SearchBar.FilterType.manual,
            Key key = null) : base(key)
        {
            _keyword = keyword;
            _page = page;
            _filterType = filterType;
        }

        private readonly string _keyword;
        private readonly int _page;
        private readonly SearchBar.FilterType _filterType;
        public override State createState() => new SearchPageState();
    }
}