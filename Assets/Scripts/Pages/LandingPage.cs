using System;
using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Banner = DocCN.Components.Banner;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Pages
{
    public class LandingPage : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            var height = MediaQuery.of(context).size.height;
            const float minHeight = 80f + 277f + 200f + 352f;
            var verticalPadding = minHeight < height ? (height - minHeight) / 2 : 16f;
            return new Container(
                height: height,
                child: new SingleChildScrollView(
                    child:
                    new Column(
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
                                        new EntryCard("Unity 用户手册", latestVersion: "最新版本：不可用",
                                            description: "使用Unity Editor创建2D和3D游戏，应用程序和体验。"),
                                        new EntryCard("Unity 脚本 API", latestVersion: "最新版本：不可用",
                                            description: "本文档的这一部分包含Unity提供的脚本API的详细信息。"),
                                        new EntryCard("Unity 服务手册",
                                            description: "通过轻松集成广告，分析，应用内购买等功能为您的应用增值。"),
                                    }
                                )
                            ),
                            new Footer()
                        }
                    )
                )
            );
        }
    }

    public class EntryCard : StatelessWidget
    {
        public EntryCard(string title = "", string latestVersion = "", string description = "")
        {
            _title = title;
            _latestVersion = latestVersion;
            _description = description;
        }

        private static readonly Color TitleTextColor = new Color(0xff212121);
        private readonly string _title;
        private readonly string _latestVersion;
        private readonly string _description;

        public override Widget build(BuildContext context)
        {
            return new Expanded(
                child: new Container(
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
                            new Container(
                                height: 320.0f,
                                child: new Stack(
                                    children: new List<Widget>
                                    {
                                        new Opacity(
                                            opacity: 0.3f,
                                            child: new Container(
                                                color: new Color(0xff000000)
                                            )
                                        ),
                                        new Container(
                                            padding: EdgeInsets.all(24.0f),
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
                                                                height: 1.42857142857f
                                                            )
                                                        )
                                                    ),
                                                    new Container(
                                                        margin: EdgeInsets.only(top: 4.0f),
                                                        child: new Text(
                                                            _description,
                                                            style: new TextStyle(
                                                                fontSize: 24.0f,
                                                                height: 1.33333333333f,
                                                                fontWeight: FontWeight.w500
                                                            )
                                                        )
                                                    ),
                                                    new Container(
                                                        margin: EdgeInsets.only(top: 56.0f),
                                                        child: new Text(
                                                            _description,
                                                            style: new TextStyle(
                                                                fontSize: 16.0f,
                                                                height: 1.5f,
                                                                fontWeight: FontWeight.w400
                                                            )
                                                        )
                                                    )
                                                }
                                            )
                                        )
                                    }
                                )
                            ),
                        }
                    )
                )
            );
        }
    }
}