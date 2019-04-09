using System;
using System.Collections.Generic;
using System.Linq;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public partial class MarkdownContent
    {
        private class MetaFields : StatelessWidget
        {
            public MetaFields(
                List<PositionRecord> items,
                ScrollController controller)
            {
                _items = items;
                _controller = controller;
            }

            private static readonly TextStyle ItemStyle = new TextStyle(
                height: 1.5f,
                color: new Color(0xff616161),
                fontSize: 16.0f
            );

            private static readonly Color DividerColor = new Color(0xffd8d8d8);

            private readonly List<PositionRecord> _items;

            private readonly ScrollController _controller;

            public override Widget build(BuildContext context)
            {
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
                                    child: new Text(
                                        "文章导览",
                                        style: new TextStyle(
                                            fontSize: 16f,
                                            color: new Color(0xff212121)
                                        )
                                    )
                                )
                            ),
                            new Container(
                                height: 1f,
                                color: DividerColor
                            ),
                            new Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: _items.Select<PositionRecord, Widget>(
                                    Item => new Clickable(
                                        onTap: () => _controller.animateTo(
                                            _controller.position.pixels - Item.getPosition.Invoke() - 24,
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
                                ).ToList()
                            )
                        }
                    )
                );
            }
        }
    }
}