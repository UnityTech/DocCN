using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public class MetaFields : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                padding: EdgeInsets.only(
                    left: 48.0f,
                    right: 48.0f
                ),
                width: 324.0f,
                child: new Column(
                    children: new List<Widget>
                    {
                        new Container(
                            padding: EdgeInsets.only(bottom: 12.0f),
                            margin: EdgeInsets.only(bottom: 24.0f),
                            decoration: new BoxDecoration(
                                border: new Border(
                                    bottom: new BorderSide(
                                        color: new Color(0xffd8d8d8),
                                        width: 1.0f
                                    )
                                )
                            ),
                            child: new LanguageSelection()
                        ),
                        new RelatedArticles()
                    }
                )
            );
        }
    }
}