using System;
using System.Collections.Generic;
using Unity.DocZh.Style;
using Unity.DocZh.Utility;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
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
                                        new SocialBrand(
                                            Icons.BrandsSinaWeiBo,
                                            new Color(0xffe6162d),
                                            socialBrandCtx => LocationUtil.HrefTo("https://www.weibo.com/unitychina")
                                        ),
                                        new SocialBrand(
                                            Icons.BrandsWeChat,
                                            new Color(0xff7bb32e),
                                            socialBrandCtx =>
                                            {
                                                Guid? guid = null;
                                                guid = ScrollableOverlay.of(context).Add(ctx =>
                                                {
                                                    var overlayRenderBox = ctx.findRenderObject() as RenderBox;
                                                    var renderBox = socialBrandCtx.findRenderObject() as RenderBox;
                                                    var offset = renderBox.localToGlobal(Offset.zero, overlayRenderBox);
                                                    return new WechatOfficialAccount(
                                                        () => ScrollableOverlay.of(context).Remove(guid.Value),
                                                        left: offset.dx - 128,
                                                        bottom: overlayRenderBox.size.height - offset.dy + 16
                                                    );
                                                });
                                            }
                                        )
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
                            new Row(
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
                                    new Clickable(
                                        onTap: () => LocationUtil.HrefTo("https://connect.unity.com"),
                                        child: new Container(
                                            margin: EdgeInsets.only(left: 40),
                                            color: new Color(0x00000000),
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget>
                                                {
                                                    new Text(
                                                        "Hosts By",
                                                        style: _style.copyrightLabelStyle
                                                    ),
                                                    new Container(
                                                        child: new Text(
                                                            "Unity Connect",
                                                            style: _style.copyrightTextStyle
                                                        )
                                                    )
                                                }
                                            )
                                        )
                                    ),
                                    new Clickable(
                                        onTap: () => LocationUtil.HrefTo("https://github.com/UnityTech/UIWidgets"),
                                        child: new Container(
                                            margin: EdgeInsets.only(left: 40),
                                            color: new Color(0x00000000),
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget>
                                                {
                                                    new Text(
                                                        "Powered By",
                                                        style: _style.copyrightLabelStyle
                                                    ),
                                                    new Container(
                                                        child: new Text(
                                                            "UIWidgets",
                                                            style: _style.copyrightTextStyle
                                                        )
                                                    )
                                                }
                                            )
                                        )
                                    )
                                }
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