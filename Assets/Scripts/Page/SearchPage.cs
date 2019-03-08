using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.widgets;

namespace DocCN.Page
{
    public class SearchPage : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Column(
                children: new List<Widget>
                {
                    new Header(),
                    new SearchBar(),
                    new Footer(),
                }
            );
        }
    }
}