using System.Collections.Generic;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public class Menu : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                width: 336.0f,
                child: new Column(
                    children: new List<Widget>
                    {
                        new Text(
                            "unity手册",
                            style: new TextStyle(
                                fontSize: 24.0f,
                                fontWeight: FontWeight.w700
                            )
                        ),
                    }
                )
            );
        }
    }
}