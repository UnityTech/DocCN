using System.Collections.Generic;
using DocCN.Style;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public class SearchFilter : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                width: 380.0f,
                child: new Column(
                    children: new List<Widget>
                    {
                        new Container(
                            height: 56.0f,
                            padding: EdgeInsets.only(left: 48f, right: 12f),
                            margin: EdgeInsets.only(bottom: 4f),
                            decoration: new BoxDecoration(
                                color: new Color(0xffffffff),
                                borderRadius: BorderRadius.circular(4.0f)
                            ),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget>
                                {
                                    new Text(
                                        "搜索结果筛选",
                                        style: new TextStyle(
                                            fontSize: 16f,
                                            color: new Color(0xff212121),
                                            fontFamily: "PingFang W500"
                                        )
                                    ),
                                    new Text(
                                        "清除所选",
                                        style: new TextStyle(
                                            fontSize: 14f,
                                            color: new Color(0xff3c83d4)
                                        )
                                    )
                                }
                            )
                        ),
                        new FilterItem()
                    }
                )
            );
        }
    }

    internal class FilterItem : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                margin: EdgeInsets.only(top: 8f),
                decoration: new BoxDecoration(
                    new Color(0xffffffff),
                    borderRadius: BorderRadius.circular(4f)
                ),
                height: 48f,
                padding: EdgeInsets.only(left: 16f, right: 32f),
                child: new Row(
                    children: new List<Widget>
                    {
                        new Container(
                            child: new Icon(
                                Icons.MaterialExpandMore,
                                color: new Color(0xffd8d8d8)
                            )
                        ),
                        new Container(
                            height: 16f,
                            width: 16f,
                            margin: EdgeInsets.only(right: 16f)
                        ),
                        new Expanded(
                            child: new Text(
                                "离线部署Unity",
                                style: new TextStyle(
                                    color: new Color(0xff212121),
                                    fontSize: 16f
                                )
                            )
                        ),
                        new CheckBox()
                    }
                )
            );
        }
    }
}