using System.Collections.Generic;
using Unity.DocZh.Components;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Pages
{
    public partial class ScriptingPage
    {
        private class ScriptingPageState : State<ScriptingPage>
        {
            public override Widget build(BuildContext buildContext)
            {
                var scriptingContent = widget._title == string.Empty
                    ? new ScriptingContent.Entrance() as Widget
                    : new ScriptingContent(widget._title);
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
                                        new Menu(MenuType.scripting),
                                        new Expanded(child: scriptingContent)
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