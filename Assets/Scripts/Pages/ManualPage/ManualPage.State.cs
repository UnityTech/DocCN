using System.Collections.Generic;
using Unity.DocZh.Components;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Pages
{
    public partial class ManualPage
    {
        private class ManualPageState : State<ManualPage>
        {
            public override Widget build(BuildContext buildContext)
            {
                var column = new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Header(),
                        new SearchBar(
                            filterDropDownOverlayType: DropDownOverlayType.builtin
                        ),
                        new Expanded(
                            child: new Container(
                                color: new Color(0xffffffff),
                                child: new Row(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget>
                                    {
                                        new Menu(MenuType.manual),
                                        new Expanded(
                                            child: new MarkdownContent(widget._title)
                                        ),
                                    }
                                )
                            )
                        ),
                    }
                );
                var container = new Container(
                    height: MediaQuery.of(context).size.height,
                    width: MediaQuery.of(context).size.width,
                    child: column
                );
                return container;
            }
        }
    }
}