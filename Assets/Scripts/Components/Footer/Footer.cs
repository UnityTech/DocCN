using System;
using System.Collections.Generic;
using DocCN.Style;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public partial class Footer : StatelessWidget
    {
        public Footer(
            bool showSocials = true,
            Style style = null,
            Key key = null) : base(key)
        {
            _showSocials = showSocials;
            _style = style ?? Dark;
        }

        private readonly bool _showSocials;
        private readonly Style _style;

        public const float Height = 193f;

        public override Widget build(BuildContext context)
        {
            var rightPartChildren = new List<Widget>();
            if (_showSocials)
            {
                rightPartChildren.Add(
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
                                    style: _style.hintTextStyle
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
                    )
                );
            }

            rightPartChildren.Add(
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
                                style: _style.hintTextStyle
                            ),
                            new LanguageSelect(
                                selectTextColor: _style.langSelectTextColor,
                                underlineColor: _style.langUnderlineColor,
                                expandIconColor: _style.langExpandIconColor
                            )
                        }
                    )
                )
            );

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
                                        image: _style.unityBrand,
                                        fit: BoxFit.fill
                                    )
                                )
                            ),
                            new Row(
                                children: new List<Widget>
                                {
                                    new Text(
                                        $"Copyright © {DateTime.Now.Year} Unity Technologies",
                                        style: _style.copyRightStyle
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 40.0f),
                                        child: new HyperLink(
                                            text: "法律相关",
                                            link: "https://unity3d.com/legal",
                                            style: _style.legalLinkStyle
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 32.0f),
                                        child: new HyperLink(
                                            text: "隐私条款",
                                            link: "https://unity3d.com/legal/privacy-policy",
                                            style: _style.legalLinkStyle
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 32.0f),
                                        child: new HyperLink(
                                            text: "Cookies",
                                            link: "https://unity3d.com/legal/cookie-policy",
                                            style: _style.legalLinkStyle
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
                                children: rightPartChildren
                            ),
                        }
                    )
                }
            );
            return new Container(
                height: Height,
                color: _style.bgColor,
                padding: _style.horizontalPadding ? EdgeInsets.only(right: 48.0f, left: 48.0f) : EdgeInsets.zero,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Container(
                            height: 53f,
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
                                        style: _style.siteLinkStyle,
                                        link: "https://unity3d.com/cn/learn"
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 24),
                                        child: new HyperLink(
                                            text: "教程",
                                            style: _style.siteLinkStyle,
                                            link: "https://answers.unity.com"
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 24),
                                        child: new HyperLink(
                                            text: "知识库",
                                            style: _style.siteLinkStyle,
                                            link: "https://support.unity3d.com/hc/zh-cn"
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 24),
                                        child: new HyperLink(
                                            text: "论坛",
                                            style: _style.siteLinkStyle,
                                            link: "https://forum.unity.com"
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 24),
                                        child: new HyperLink(
                                            text: "素材商店",
                                            style: _style.siteLinkStyle,
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