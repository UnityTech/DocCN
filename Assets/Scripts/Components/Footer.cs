using System;
using System.Collections.Generic;
using DocCN.Utility.Style;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Utility.Components
{
    public class Footer : StatelessWidget
    {
        private static readonly TextStyle SiteLinkStyle = new TextStyle(
            fontSize: 14f,
            color: new Color(0xffd8d8d8),
            decoration: TextDecoration.underline
        );

        private static readonly TextStyle LegalLinkStyle = new TextStyle(
            fontSize: 14.0f,
            color: new Color(0xff2196f3),
            decoration: TextDecoration.underline
        );

        private static readonly TextStyle HintTextStyle = new TextStyle(
            fontSize: 14.0f,
            color: new Color(0xffd8d8d8)
        );

        public override Widget build(BuildContext context)
        {
            var secondRow = new Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: new List<Widget>
                {
                    new Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget>
                        {
                            new Container(
                                height: 48.0f,
                                width: 132.0f,
                                decoration: new BoxDecoration(
                                    image: new DecorationImage(
                                        image: new AssetImage("Images/unity-master-white"),
                                        fit: BoxFit.fill
                                    )
                                )
                            ),
                            new Row(
                                children: new List<Widget>
                                {
                                    new Text(
                                        $"Copyright © {DateTime.Now.Year} Unity Technologies",
                                        style: new TextStyle(
                                            color: new Color(0xffd8d8d8),
                                            fontSize: 14.0f
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 40.0f),
                                        child: new HyperLink(
                                            text: "法律相关",
                                            link: "https://unity3d.com/legal",
                                            style: LegalLinkStyle
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 32.0f),
                                        child: new HyperLink(
                                            text: "隐私条款",
                                            link: "https://unity3d.com/legal/privacy-policy",
                                            style: LegalLinkStyle
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 32.0f),
                                        child: new HyperLink(
                                            text: "Cookies",
                                            link: "https://unity3d.com/legal/cookie-policy",
                                            style: LegalLinkStyle
                                        )
                                    )
                                }
                            ),
                        }
                    ),
                    new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget>
                        {
                            new Row(
                                children: new List<Widget>
                                {
                                    new Container(
                                        width: 180f,
                                        height: 72f,
                                        child: new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget>
                                            {
                                                new Text(
                                                    "社交媒体",
                                                    style: HintTextStyle
                                                ),
                                                new Row(
                                                    children: new List<Widget>
                                                    {
                                                        new SocialBrand(Icons.IconSinaWeiBo, new Color(0xffe6162d)),
                                                        new SocialBrand(Icons.IconWeChat, new Color(0xff7bb32e))
                                                    }
                                                )
                                            }
                                        )
                                    ),
                                    new Container(
                                        width: 180f,
                                        height: 72f,
                                        child: new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget>
                                            {
                                                new Text(
                                                    "语言",
                                                    style: HintTextStyle
                                                ),
                                                new LanguageSelect()
                                            }
                                        )
                                    )
                                }
                            ),
                        }
                    )
                }
            );
            return new Container(
                height: 200.0f,
                color: new Color(0xff111111),
                padding: EdgeInsets.only(right: 48.0f, left: 48.0f),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Container(
                            height: 56f,
                            decoration: new BoxDecoration(
                                border: new Border(
                                    bottom: new BorderSide(
                                        width: 1f,
                                        color: new Color(0xff616161)
                                    )
                                )
                            ),
                            child: new Row(
                                children: new List<Widget>
                                {
                                    new HyperLink(
                                        text: "社区问答",
                                        style: SiteLinkStyle,
                                        link: "https://unity3d.com/cn/learn"
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 24),
                                        child: new HyperLink(
                                            text: "教程",
                                            style: SiteLinkStyle,
                                            link: "https://answers.unity.com"
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 24),
                                        child: new HyperLink(
                                            text: "知识库",
                                            style: SiteLinkStyle,
                                            link: "https://support.unity3d.com/hc/zh-cn"
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 24),
                                        child: new HyperLink(
                                            text: "论坛",
                                            style: SiteLinkStyle,
                                            link: "https://forum.unity.com"
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 24),
                                        child: new HyperLink(
                                            text: "素材商店",
                                            style: SiteLinkStyle,
                                            link: "https://assetstore.unity.com"
                                        )
                                    ),
                                }
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.symmetric(vertical: 24f),
                                child: secondRow
                            )
                        )
                    }
                )
            );
        }
    }
}