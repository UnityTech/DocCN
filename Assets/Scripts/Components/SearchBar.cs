using System.Collections.Generic;
using DocCN.Style;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Icons = Unity.UIWidgets.material.Icons;

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
        private static readonly BorderSide FILTER_BORDER_SIDE =
            new BorderSide(width: 2.0f, color: new Color(0xff424242));

        public override Widget build(BuildContext context)
        {
            return new Container(
                padding: EdgeInsets.only(top: 8.0f, right: 48.0f, bottom: 8.0f, left: 48.0f),
                color: new Color(0xff212121),
                child: new Row(
                    children: new List<Widget>
                    {
                        new Container(
                            width: 132.0f,
                            height: 56.0f,
                            decoration: new BoxDecoration(
                                border: new Border(
                                    top: FILTER_BORDER_SIDE,
                                    left: FILTER_BORDER_SIDE,
                                    bottom: FILTER_BORDER_SIDE
                                )
                            ),
                            child: new Column(
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                height: 56.0f,
                                color: new Color(0xff424242)
                            )
                        ),
                        new Container(
                            width: 56.0f,
                            height: 56.0f,
                            color: new Color(0xff565656),
                            child: null
                        )
                    }
                )
            );
        }
    }
}