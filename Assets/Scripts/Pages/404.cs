using System.Collections.Generic;
using Unity.DocZh.Components;
using Unity.DocZh.Utility;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Pages
{
    public class NotFoundPage : StatelessWidget
    {
        private static readonly TextStyle EmStyle = new TextStyle(
            color: new Color(0xff88b4e7)
        );

        public override Widget build(BuildContext context)
        {
            var pageHeight = MediaQuery.of(context).size.height;
            const float minHeight = Header.Height + SearchBar.Height + Footer.Height;
            return new Container(
                height: pageHeight,
                child: new Scroller(
                    child: new SingleChildScrollView(
                        child: new ScrollableOverlay(
                            child: new Column(
                                children: new List<Widget>
                                {
                                    new Header(),
                                    new SearchBar(),
                                    new Container(
                                        constraints: new BoxConstraints(
                                            minHeight: minHeight < pageHeight ? pageHeight - minHeight : 0
                                        ),
                                        padding: EdgeInsets.symmetric(vertical: 48f),
                                        color: new Color(0xffffffff),
                                        child: new Center(
                                            child: new Column(
                                                mainAxisAlignment: MainAxisAlignment.center,
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget>
                                                {
                                                    new Container(
                                                        width: 512,
                                                        height: 205,
                                                        margin: EdgeInsets.only(bottom: 24f),
                                                        child: new Image(
                                                            image: new AssetImage("Images/404@3x")
                                                        )
                                                    ),
                                                    new RichText(
                                                        text: new TextSpan(
                                                            children: new List<TextSpan>
                                                            {
                                                                new TextSpan("抱歉，您所访问的页面已被"),
                                                                new TextSpan("移除", style: EmStyle),
                                                                new TextSpan("或"),
                                                                new TextSpan("不存在", style: EmStyle),
                                                                new TextSpan("。")
                                                            },
                                                            style: new TextStyle(
                                                                fontSize: 24f,
                                                                color: new Color(0xffd8d8d8),
                                                                height: 1.33333333333f.LineHeight(),
                                                                fontWeight: FontWeight.w500
                                                            )
                                                        )
                                                    ),
                                                    new Container(
                                                        margin: EdgeInsets.only(top: 32f),
                                                        child: new Button(
                                                            onTap: () => LocationUtil.Go("/"),
                                                            text: "返回主页"
                                                        )
                                                    )
                                                }
                                            )
                                        
                                        )
                                    ),
                                    new Footer(),
                                }
                            )
                        )
                    )
                )
            );
        }
    }
}