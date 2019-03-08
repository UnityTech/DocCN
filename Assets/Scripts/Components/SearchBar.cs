using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public class SearchBar : StatefulWidget
    {
        public SearchBar() : base(new ObjectKey("search-bar"))
        {
        }

        public override State createState() => new SearchBarState();
    }

    internal class SearchBarState : State<SearchBar>
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                padding: EdgeInsets.only(top: 8.0f, right: 48.0f, bottom: 8.0f, left: 56.0f),
                color: new Color(0xff212121),
                child: new Row(
                    children: new List<Widget>
                    {
                        new Container(
                            width: 124.0f,
                            child: new Column(
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                height: 56.0f,
                                color: new Color(0xff424242)
                            )
                        ),
                    }
                )
            );
        }
    }
}