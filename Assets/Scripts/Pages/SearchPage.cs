using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace DocCN.Pages
{
    public class SearchPage : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                constraints: new BoxConstraints(
                    minHeight: MediaQuery.of(context).size.height
                ),
                child: new Column(
                    children: new List<Widget>
                    {
                        new Header(),
                        new SearchBar(),
                        new Expanded(
                            child: new Content<Row>(
                                child: new Row(
                                    children: new List<Widget>
                                    {
                                        new SearchFilter(),
                                    }
                                )
                            ) 
                        ),
                        new Footer(),
                    }
                )
            );
        }
    }
}