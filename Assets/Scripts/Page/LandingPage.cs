using System;
using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Page
{
    public class LandingPage : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: new List<Widget>
                {
                    new Header(),
                    new Banner(),
                    new Container(
                        padding: EdgeInsets.only(top: 64.0f, right: 36.0f, bottom: 48.0f, left: 36.0f),
                        color: new Color(0xffffffff),
                        child: new Row(
                            children: new List<Widget>
                            {
                                new EntryCard("Unity 用户手册", latestVersion: "最新版本：不可用", description: "使用Unity Editor创建2D和3D游戏，应用程序和体验。"),
                                new EntryCard("Unity 脚本 API", latestVersion: "最新版本：不可用", description: "本文档的这一部分包含Unity提供的脚本API的详细信息。"),
                                new EntryCard("Unity 服务手册", description: "通过轻松集成广告，分析，应用内购买等功能为您的应用增值。"),
                            }
                        )
                    ),
                    new Footer()
                }
            );
        }
    }

    public class EntryCard : StatelessWidget
    {
        public EntryCard(string title = "", string latestVersion = "", string description = "")
        {
            this.title = title;
            this.latestVersion = latestVersion;
            this.description = description;
        }

        private static readonly Color TITLE_TEXT_COLOR = new Color(0xff212121);
        private readonly string title;
        private readonly string latestVersion;
        private readonly string description;

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
                                        title,
                                        style: new TextStyle(
                                            fontSize: 24.0f,
                                            fontWeight: FontWeight.w700, // w600
                                            color: TITLE_TEXT_COLOR
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
                                                            latestVersion,
                                                            style: new TextStyle(
                                                                fontSize: 14.0f,
                                                                fontWeight: FontWeight.w700,
                                                                height: 1.42857142857f
                                                            )
                                                        )
                                                    ),
                                                    new Container(
                                                        margin: EdgeInsets.only(top: 4.0f),
                                                        child: new Text(
                                                            description,
                                                            style: new TextStyle(
                                                                fontSize: 24.0f,
                                                                height: 1.33333333333f,
                                                                fontWeight: FontWeight.w700
                                                            )
                                                        )
                                                    ),
                                                    new Container(
                                                        margin: EdgeInsets.only(top: 56.0f),
                                                        child: new Text(
                                                            description,
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