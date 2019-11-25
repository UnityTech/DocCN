using System.Collections.Generic;
using Unity.DocZh.Utility;
using Unity.UIWidgets.rendering;
using Painting = Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public class Header : StatelessWidget
    {
        public const float Height = 60f;

        public override Widget build(BuildContext context)
        {
            return new Container(
                height: Height,
                color: new Color(0xff000000),
                child: new Row(
                    children: new List<Widget>
                    {
                        new UnityLogo(),
                        new CategoryTabs(),
                    }
                )
            );
        }
    }

    public class UnityLogo : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Clickable(
                onTap: () => LocationUtil.Go("/"),
                child: new Container(
                    width: 336.0f,
                    child: new Row(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: new List<Widget>
                        {
                            new Container(
                                margin: Painting.EdgeInsets.only(left: 48.0f),
                                height: 34.0f,
                                width: 93.5f,
                                decoration: new Painting.BoxDecoration(
                                    image: new Painting.DecorationImage(
                                        image: new Painting.AssetImage(
                                            "Images/unity-master-white"
                                        ),
                                        fit: Painting.BoxFit.fill
                                    )
                                )
                            ),
                            new Container(
                                height: 34.0f,
                                width: 2.0f,
                                margin: Painting.EdgeInsets.only(right: 8.0f, left: 8.0f),
                                color: new Color(0xffffffff)
                            ),
                            new Text(
                                "DOCUMENTATION",
                                style: new Painting.TextStyle(
                                    color: new Color(0xffffffff),
                                    letterSpacing: -0.8f,
                                    fontWeight: FontWeight.w500,
                                    fontSize: 16.0f
                                )
                            )
                        }
                    )
                )
            );
        }
    }

    public class CategoryTabs : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Row(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: new List<Widget>
                {
                    new TabItem("手册", true, "/Manual/UnityManual"),
                    new TabItem("脚本API", false, "/Scripting"),
                    //new TabItem("服务手册", false, "/Manual/UnityManual")
                }
            );
        }
    }

    internal class TabItem : StatefulWidget
    {
        internal TabItem(string text, bool isFirst, string link)
        {
            _text = text;
            _isFirst = isFirst;
            _link = link;
        }

        private readonly string _text;
        private readonly bool _isFirst;
        private readonly string _link;

        public override State createState() => new TabItemState();

        private class TabItemState : State<TabItem>
        {
            private static readonly Painting.TextStyle NormalStyle = new Painting.TextStyle(
                fontSize: 16.0f,
                fontWeight: FontWeight.w400,
                color: new Color(0xffffffff)
            );

            private static readonly Painting.TextStyle HoverStyle = new Painting.TextStyle(
                fontSize: 16.0f,
                fontWeight: FontWeight.w500,
                color: new Color(0xff2196f3)
            );

            private bool _hover;

            public override void initState()
            {
                base.initState();
                _hover = false;
            }

            public override Widget build(BuildContext buildContext)
            {
                var margin = Painting.EdgeInsets.all(0.0f);
                if (!widget._isFirst)
                {
                    margin = Painting.EdgeInsets.only(left: 16.0f);
                }

                return new Clickable(
                    onTap: () => LocationUtil.Go(widget._link),
                    hoverChanged: hover =>
                    {
                        if (mounted)
                        {
                            setState(() => _hover = hover);
                        }
                    },
                    child: new Container(
                        color: new Color(0x00000000),
                        margin: margin,
                        padding: Painting.EdgeInsets.symmetric(horizontal: 8f),
                        child: new Center(
                            child: new Text(
                                widget._text,
                                style: _hover ? HoverStyle : NormalStyle
                            )
                        )
                    )
                );
            }
        }
    }
}