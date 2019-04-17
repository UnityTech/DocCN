using System;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class SearchBar : StatefulWidget
    {
        public SearchBar()
        {
        }

        public const float Height = 70f;

        public override State createState() => new SearchBarState();
    }
}