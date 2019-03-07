using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public class Markdown : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget>
                {
                    new Breadcrumb(),
                    new Container(
                        height: 1.0f,
                        decoration: new BoxDecoration(
                            color: new Color(0xffd8d8d8)
                        ),
                        margin: EdgeInsets.only(bottom: 24.0f)
                    ),
                    new MarkdownTitleRow(),
                    new ModifiedInfo(),
                    new MarkdownContent(new ValueKey<string>("data.json"))
                }
            );
        }
    }
}