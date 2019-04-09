using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Utility.Components
{
    public class Breadcrumb : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Column(
                children: new List<Widget>
                {
                    new Container(
                        margin: EdgeInsets.only(top: 16f),
                        height: 40f,
                        child: new Align(
                            alignment: Alignment.bottomLeft,
                            child: new RichText(
                                text: new TextSpan(
                                    "Unity用户手册（2018.3） / 导入"
                                )
                            )
                        )
                    ),
                    new Container(
                        color: new Color(0xffd8d8d8),
                        height: 1f,
                        margin: EdgeInsets.only(top: 8f)
                    )
                }
            );
        }
    }
}