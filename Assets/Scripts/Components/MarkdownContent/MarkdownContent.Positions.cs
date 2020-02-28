using System;
using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Style;
using Unity.DocZh.Utility;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Components
{
    public partial class MarkdownContent
    {
        private class MetaFields : StatelessWidget
        {
            public MetaFields(
                List<PositionRecord> items,
                ScrollController controller,
                string githubLink = null)
            {
                _items = items;
                _controller = controller;
                _githubLink = githubLink;
            }

            private static readonly TextStyle ItemStyle = new TextStyle(
                height: 1.5f.LineHeight(),
                color: new Color(0xff616161),
                fontSize: 16.0f
            );

            private static readonly Color DividerColor = new Color(0xffd8d8d8);

            private readonly List<PositionRecord> _items;

            private readonly ScrollController _controller;

            private readonly string _githubLink;

            public override Widget build(BuildContext context)
            {
                var children = new List<Widget>
                {
                };
                children.AddRange(
                    _items.Select<PositionRecord, Widget>(
                        Item => new Clickable(
                            onTap: () => _controller.animateTo(
                                _controller.position.pixels - Item.getPosition.Invoke() - 96,
                                new TimeSpan(0, 0, 0, 0, 240),
                                curve: Curves.easeInOut)
                            ,
                            child: new Container(
                                margin: EdgeInsets.only(bottom: 16.0f),
                                child: new Text(
                                    Item.title,
                                    style: ItemStyle
                                )
                            )
                        )
                    )
                );
                return new Container(
                    padding: EdgeInsets.only(
                        left: 48.0f,
                        right: 48.0f
                    ),
                    width: 324.0f,
                    child: new Column(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: new List<Widget>
                        {
                            new Container(
                                height: 64f,
                                padding: EdgeInsets.only(bottom: 8.0f),
                                child: new Align(
                                    alignment: Alignment.bottomLeft,
                                    child: new Row(
                                        children: new List<Widget>
                                        {
                                            new Text(
                                                "文章导览",
                                                style: new TextStyle(
                                                    fontSize: 16,
                                                    fontWeight: FontWeight.w500,
                                                    color: new Color(0xff212121)
                                                )
                                            )
                                        }
                                    )
                                )
                            ),
                            new Container(
                                height: 1f,
                                color: DividerColor,
                                margin: EdgeInsets.only(bottom: 24f)
                            ),
                            new Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: children
                            )
                        }
                    )
                );
            }
        }
    }
}