using System.Collections.Generic;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using DocCN.Components;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;

namespace DocCN
{
    public class ExampleCanvas : WidgetCanvas
    {
        protected override Widget getWidget()
        {
            return new ExampleApp();
        }

        class ExampleApp : StatefulWidget
        {
            public ExampleApp(Key key = null) : base(key)
            {
            }

            public override State createState()
            {
                return new ExampleState();
            }
        }

        class ExampleState : State<ExampleApp>
        {
            int counter = 0;

            public override Widget build(BuildContext context)
            {
                return new Column(
                    children: new List<Widget>
                    {
                        new Header(),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.only(top: 24.0f),
                                color: new Color(0xffffffff),
                                child: new Row(
                                    children: new List<Widget>
                                    {
                                        new Menu(),
                                        new Expanded(
                                            child: new Markdown()
                                        ),
                                        new MetaFields()
                                    }
                                )
                            )
                        )
                    }
                );
            }
        }
    }
}