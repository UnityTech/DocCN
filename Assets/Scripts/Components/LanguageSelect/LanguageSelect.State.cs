using System.Collections.Generic;
using DocCN.Utility.Style;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public partial class LanguageSelect
    {
        private class LanguageSelectState : State<LanguageSelect>
        {
            public override Widget build(BuildContext context)
            {
                return new Container(
                    height: 32f,
                    decoration: new BoxDecoration(
                        border: new Border(
                            bottom: new BorderSide(
                                width: 1f,
                                color: new Color(0xffffffff)
                            )
                        )
                    ),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: new List<Widget>
                        {
                            new Text(
                                "简体中文",
                                style: new TextStyle(
                                    fontSize: 16f,
                                    color: new Color(0xffffffff)
                                )
                            ),
                            new Icon(
                                Icons.MaterialExpandMore,
                                color: new Color(0xffffffff)
                            )
                        }
                    )
                );
            }
        }
    }
}