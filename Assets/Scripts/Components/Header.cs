using System.Collections.Generic;
using Unity.UIWidgets.rendering;
using Painting = Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public class Header : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                height: 60.0f,
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
            return new Container(
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
            );
        }
    }

    public class CategoryTabs : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Row(
                children: new List<Widget>
                {
                    new TabItem("手册", true),
                    new TabItem("脚本API", false)
                }
            );
        }
    }

    internal class TabItem : StatelessWidget
    {
        internal TabItem(string text, bool isFirst)
        {
            this.text = text;
            this.isFirst = isFirst;
        }

        private readonly string text;
        private readonly bool isFirst;

        public override Widget build(BuildContext context)
        {
            var margin = Painting.EdgeInsets.all(0.0f);
            if (!isFirst)
            {
                margin = Painting.EdgeInsets.only(left: 32.0f);
            }

            return new Container(
                margin: margin,
                child: new Text(
                    text,
                    style: new Painting.TextStyle(
                        fontSize: 16.0f,
                        fontWeight: FontWeight.w400,
                        color: new Color(0xffffffff)))
            );
        }
    }
}