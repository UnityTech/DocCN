using System;
using System.Collections.Generic;
using DocCN.Style;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    internal class SocialBrand : StatelessWidget
    {
        public SocialBrand(IconData data)
        {
            this.data = data;
        }

        private readonly IconData data;

        public override Widget build(BuildContext context)
        {
            return new Container(
                margin: EdgeInsets.only(left: 20.0f),
                child: new Icon(
                    data,
                    color: new Color(0xff908f90)
                )
            );
        }
    }

    public class Footer : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                height: 160.0f,
                color: new Color(0xff111111),
                padding: EdgeInsets.only(top: 32.0f, right: 48.0f, bottom: 32.0f, left: 48.0f),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget>
                            {
                                new Container(
                                    height: 48.0f,
                                    width: 132.0f,
                                    decoration: new BoxDecoration(
                                        image: new DecorationImage(
                                            image: new AssetImage(
                                                "Images/unity-master-white"
                                            ),
                                            fit: BoxFit.fill
                                        )
                                    )
                                ),
                                new Row(
                                    children: new List<Widget>
                                    {
                                        new SocialBrand(Icons.IconYoutube),
                                        new SocialBrand(Icons.IconLinkedIn),
                                        new SocialBrand(Icons.IconTwitter),
                                        new SocialBrand(Icons.IconFacebook)
                                    }
                                )
                            }
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 22.0f),
                            child: new Row(
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
                                        margin: EdgeInsets.only(left: 48.0f),
                                        child: new Text(
                                            "法律条款",
                                            style: new TextStyle(
                                                fontSize: 14.0f,
                                                color: new Color(0xff00bcd4)
                                            )
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 32.0f),
                                        child: new Text(
                                            "隐私条款",
                                            style: new TextStyle(
                                                fontSize: 14.0f,
                                                color: new Color(0xff00bcd4)
                                            )
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(left: 32.0f),
                                        child: new Text(
                                            "Cookies",
                                            style: new TextStyle(
                                                fontSize: 14.0f,
                                                color: new Color(0xff00bcd4)
                                            )
                                        )
                                    )
                                }
                            )
                        ),
                    }
                )
            );
        }
    }
}