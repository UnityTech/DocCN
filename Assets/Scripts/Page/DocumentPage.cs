using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace DocCN.Page
{
    public class DocumentPage : StatefulWidget
    {
        public DocumentPage(string title) : base(new ObjectKey(title))
        {
            _title = title;
        }

        private readonly string _title;

        public override State createState()
        {
            return new DocumentPageState();
        }

        private class DocumentPageState : State<DocumentPage>
        {
            public override Widget build(BuildContext buildContext)
            {
                var column = new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Header(),
                        new Expanded(
                            child: new Container(
                                color: new Color(0xffffffff),
                                child: new Row(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget>
                                    {
                                        new Menu(Menu.TypeManual),
                                        new Expanded(
                                            child: new SingleChildScrollView(
                                                child: new MarkdownContent(widget._title)
                                            )
                                        ),
                                        new MetaFields()
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