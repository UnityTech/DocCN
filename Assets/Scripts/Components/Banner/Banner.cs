using System.Collections.Generic;
using Unity.DocZh.Utility;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Components
{
    public class Banner : StatelessWidget
    {
        public const float Height = 277f;

        public override Widget build(BuildContext context)
        {
            return new Container(
                padding: EdgeInsets.only(
                    top: 16.0f,
                    right: 48.0f,
                    bottom: 16.0f,
                    left: 48.0f),
                height: Height,
                decoration: new BoxDecoration(
                    color: new Color(0xff000000),
                    image: new DecorationImage(
                        fit: BoxFit.cover,
                        image: new AssetImage(
                            "Images/hero@2x"
                        )
                    )
                ),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Container(
                            margin: EdgeInsets.only(top: 40.0f),
                            height: 72.0f,
                            child: new Align(
                                alignment: Alignment.centerLeft,
                                child: new Text(
                                    "Unity 技术手册",
                                    style: new TextStyle(
                                        color: new Color(0xffffffff),
                                        fontSize: 64.0f,
                                        fontWeight: FontWeight.w500 // actually w600
                                    )
                                )
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 32.0f, bottom: 12f),
                            height: 32.0f,
                            child: new Align(
                                alignment: Alignment.centerLeft,
                                child: new Text(
                                    "技术手册，脚本API和服务手册文档的一站式文档手册。",
                                    style: new TextStyle(
                                        color: new Color(0xffffffff),
                                        fontSize: 18.0f,
                                        fontWeight: FontWeight.w400,
                                        height: 1.77777777778f.LineHeight()
                                    )
                                )
                            )
                        ),
                        new SearchBar(SearchBarStyle.embed)
                    }
                )
            );
        }
    }
}