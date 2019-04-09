using System.Collections.Generic;
using DocCN.Utility.Components;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Utility.Pages
{
    public class NotFoundPage : StatelessWidget
    {
        private static readonly TextStyle EM_STYLE = new TextStyle(
            color: new Color(0xff88b4e7)
        );

        public override Widget build(BuildContext context)
        {
            var size = MediaQuery.of(context).size;
            return new Container(
                height: size.height,
                width: size.width,
                child: new Column(
                    children: new List<Widget>
                    {
                        new Header(),
                        
                        new Expanded(
                            child: new Container(
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
                                                        new TextSpan("移除", style: EM_STYLE),
                                                        new TextSpan("或"),
                                                        new TextSpan("不存在", style: EM_STYLE),
                                                        new TextSpan("。")
                                                    },
                                                    style: new TextStyle(
                                                        fontSize: 24f,
                                                        color: new Color(0xffd8d8d8),
                                                        height: 1.33333333333f,
                                                        fontWeight: FontWeight.w500
                                                    )
                                                )
                                            ),
                                            new Container(
                                                margin: EdgeInsets.only(top: 32f),
                                                child: new Button(
                                                    text: "返回主页"
                                                )
                                            )
                                        }
                                    )
                                )
                            )
                        ),
                        new Footer(),
                    }
                )
            );
        }
    }
}