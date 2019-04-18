using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace DocCN.Pages
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
                        new SearchBar(),
                        new Expanded(
                            child: new Container(
                                color: new Color(0xffffffff),
                                child: new Row(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget>
                                    {
                                        new Menu(MenuType.scripting),
                                        new Expanded(
                                            child: new SingleChildScrollView(
                                                child: new ScrollableOverlay(
                                                    child: new Container(
                                                        padding: EdgeInsets.only(right: 48f),
                                                        child: new Column(
                                                            children: new List<Widget>
                                                            {
                                                                new Container(
                                                                    constraints: new BoxConstraints(
                                                                        minHeight: MediaQuery.of(context).size.height -
                                                                                   Header.Height -
                                                                                   SearchBar.Height - Footer.Height
                                                                    ),
                                                                    child: scriptingContent
                                                                ),
                                                                new Footer(style: Footer.Light)
                                                            }
                                                        )
                                                    )
                                                )
                                            )
                                        )
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