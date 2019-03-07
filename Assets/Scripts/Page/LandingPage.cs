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
                                new EntryCard("Unity 用户手册"),
                                new EntryCard("Unity 脚本 API"),
                                new EntryCard("Unity 服务手册"),
                            }
                        )
                    )
                }
            );
        }
    }

    public class EntryCard : StatelessWidget
    {
        public EntryCard(string title = "")
        {
            this.title = title;
        }
        
        private static readonly Color TITLE_TEXT_COLOR = new Color(0xff212121);
        private readonly string title;

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