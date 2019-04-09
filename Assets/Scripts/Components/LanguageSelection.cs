using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Utility.Components
{
    public class LanguageSelection : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Row(
                children: new List<Widget>
                {
                    new RichText(
                        text: new TextSpan(
                            children: new List<TextSpan>
                            {
                                new TextSpan(
                                    "语言：",
                                    style: new TextStyle(
                                        color: new Color(0xff616161)
                                    )
                                ),
                                new TextSpan(
                                    "简体中文",
                                    style: new TextStyle(
                                        color: new Color(0xff212121)
                                    )
                                )
                            }
                        )
                    )
                }
            );
        }
    }
}