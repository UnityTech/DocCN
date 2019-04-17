using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace DocCN.Pages
{
    public partial class SearchPage : StatefulWidget
    {
        public SearchPage(
            string keyword = "",
            int page = 0,
            Key key = null) : base(key)
        {
            _keyword = keyword;
            _page = page;
        }

        private readonly string _keyword;
        private readonly int _page;
        public override State createState() => new SearchPageState();
    }
}