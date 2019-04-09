using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public class MarkdownTitleRow : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                margin: EdgeInsets.only(bottom: 8.0f),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Expanded(
                            child: new Container(
                                child: new Text(
                                    "用于特定于特殊平台的覆盖样式的纹理的压缩格式",
                                    style: new TextStyle(
                                        fontSize: 36.0f,
                                        height: 1.16666666667f
                                    )
                                ),
                                margin: EdgeInsets.only(right: 48.0f)
                            )
                        ),
                        new EditButton()
                    }
                )
            );
        }
    }
}