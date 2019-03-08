using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Page
{
    public class DocumentPage : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            var column = new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget>
                {
                    new Header(),
                    new Container(
                        padding: EdgeInsets.only(top: 24.0f),
                        color: new Color(0xffffffff),
                        child: new Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget>
                            {
                                new Menu(),
                                new Expanded(
                                    child: new Components.Markdown()
                                ),
                                new MetaFields()
                            }
                        )
                    )

                }
            );
            return column;
        }
    }
}