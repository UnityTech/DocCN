using System.Collections.Generic;
using Unity.DocZh.Components;
using Unity.DocZh.Utility;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Banner = Unity.DocZh.Components.Banner;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Pages
{
    public class LandingPage : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            var height = MediaQuery.of(context).size.height;
            const float minHeight = Header.Height + Footer.Height + Banner.Height + EntryCard.Height;
            var verticalPadding = minHeight < height ? (height - minHeight) / 2 : 16f;
            return new Container(
                height: height,
                child: new Scroller(
                    child: new SingleChildScrollView(
                        child: new ScrollableOverlay(
                            child: new Column(
                                crossAxisAlignment: CrossAxisAlignment.stretch,
                                children: new List<Widget>
                                {
                                    new Header(),
                                    new Banner(),
                                    new Container(
                                        padding: EdgeInsets.only(
                                            top: verticalPadding,
                                            right: 36.0f,
                                            bottom: verticalPadding,
                                            left: 36.0f
                                        ),
                                        color: new Color(0xffffffff),
                                        child: new Row(
                                            children: new List<Widget>
                                            {
                                                new EntryCard(
                                                    link: "/Manual/UnityManual",
                                                    title: "Unity 用户手册",
                                                    description: "使用Unity Editor创建2D和3D游戏，应用程序和体验。",
                                                    imageLocation: "Images/block1@2x"),
                                                new EntryCard(
                                                    link: "/Scripting",
                                                    title: "Unity 脚本 API",
                                                    description: "本文档的这一部分包含Unity提供的脚本API的详细信息。",
                                                    imageLocation: "Images/block2@2x"),
                                                new EntryCard(
                                                    link: "/Manual/UnityServices",
                                                    title: "Unity 服务手册",
                                                    description: "通过轻松集成广告，分析，应用内购买等功能为您的应用增值。",
                                                    imageLocation: "Images/block3@2x"),
                                            }
                                        )
                                    ),
                                    new Footer()
                                }
                            )
                        )
                    )
                )
            );
        }
    }

    public class EntryCard : StatelessWidget
    {
        public EntryCard(
            string title = "",
            string latestVersion = "",
            string description = "",
            string imageLocation = "",
            string link = "")
        {
            _title = title;
            _latestVersion = latestVersion;
            _description = description;
            _imageLocation = imageLocation;
            _link = link;
        }

        public const float Height = 400f;

        private static readonly Color TitleTextColor = new Color(0xff212121);
        private readonly string _title;
        private readonly string _latestVersion;
        private readonly string _description;
        private readonly string _imageLocation;
        private readonly string _link;

        public override Widget build(BuildContext context)
        {
            return new Expanded(
                child: new Clickable(
                    onTap: () => LocationUtil.Go(_link),
                    child: new Container(
                        height: Height,
                        margin: EdgeInsets.only(left: 12.0f, right: 12.0f),
                        child: new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget>
                            {
                                new Container(
                                    height: 32.0f,
                                    margin: EdgeInsets.only(bottom: 16.0f),
                                    child: new Align(
                                        alignment: Alignment.centerLeft,
                                        child: new Text(
                                            _title,
                                            style: new TextStyle(
                                                fontSize: 24.0f,
                                                fontWeight: FontWeight.w500,
                                                color: TitleTextColor
                                            )
                                        )
                                    )
                                ),
                                new Expanded(
                                    child: new Container(
                                        padding: EdgeInsets.all(24.0f),
                                        decoration: new BoxDecoration(
                                            image: new DecorationImage(
                                                image: new AssetImage(_imageLocation),
                                                fit: BoxFit.cover
                                            )
                                        ),
                                        child: new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget>
                                            {
                                                new Container(
                                                    margin: EdgeInsets.only(top: 16.0f),
                                                    child: new Text(
                                                        _latestVersion,
                                                        style: new TextStyle(
                                                            fontSize: 14.0f,
                                                            fontWeight: FontWeight.w500,
                                                            height: 1.42857142857f.LineHeight(),
                                                            color: new Color(0xffffffff)
                                                        )
                                                    )
                                                ),
                                                new Container(
                                                    margin: EdgeInsets.only(top: 4.0f),
                                                    child: new Text(
                                                        _description,
                                                        style: new TextStyle(
                                                            fontSize: 24.0f,
                                                            height: 1.33333333333f.LineHeight(),
                                                            fontWeight: FontWeight.w500,
                                                            color: new Color(0xffffffff)
                                                        )
                                                    )
                                                ),
                                                new Container(
                                                    margin: EdgeInsets.only(top: 56.0f),
                                                    child: new Text(
                                                        _description,
                                                        style: new TextStyle(
                                                            fontSize: 16.0f,
                                                            height: 1.5f.LineHeight(),
                                                            fontWeight: FontWeight.w400,
                                                            color: new Color(0xffffffff)
                                                        )
                                                    )
                                                )
                                            }
                                        )
                                    )
                                ),
                            }
                        )
                    )
                )
            );
        }
    }
}