using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using DocCN.Models.Json;
using DocCN.Models.Xml;
using DocCN.Utility;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace DocCN.Components
{
    public class SearchResultCard : StatelessWidget
    {
        public SearchResultCard(SearchResultItem item, Key key = null) : base(key)
        {
            _item = item;
            var serializer = new XmlSerializer(typeof(Highlight));
            var stringReader = new StringReader($"<xml>{_item.highlight}</xml>");
            var highlight = serializer.Deserialize(stringReader) as Highlight;
            _highlight = highlight.items.Select<object, TextSpan>(highlightItem =>
            {
                switch (highlightItem)
                {
                    case string szItem:
                        return new TextSpan(
                            szItem,
                            style: HighlightTextStyle
                        );

                    case HighlightEm em:
                        return new TextSpan(
                            em.value,
                            style: HighlightEmTextStyle
                        );
                }

                return null;
            }).ToList();
        }

        private readonly SearchResultItem _item;

        private readonly List<TextSpan> _highlight;

        private static readonly BoxDecoration ContainerDecoration = new BoxDecoration(
            color: new Color(0xffffffff),
            borderRadius: BorderRadius.all(4f)
        );

        private static readonly TextStyle TitleTextStyle = new TextStyle(
            color: new Color(0xff2196f3),
            fontSize: 24f,
            height: 1.333333333333333f
        );

        private static readonly TextStyle HighlightTextStyle = new TextStyle(
            color: new Color(0xff212121),
            fontSize: 16f,
            height: 1.5f,
            fontFamily: "PingFang"
        );

        private static readonly TextStyle HighlightEmTextStyle = new TextStyle(
            color: new Color(0xff212121),
            fontSize: 16f,
            height: 1.5f,
            fontWeight: FontWeight.w500,
            fontFamily: "PingFang"
        );

        private static readonly Container Splitter = new Container(
            height: 1f,
            color: new Color(0xffefefef)
        );

        private static readonly TextStyle BreadcrumbSplitterStyle = new TextStyle(
            color: new Color(0xff979797),
            fontSize: 16f
        );
        
        private static readonly TextStyle BreadcrumbStyle = new TextStyle(
            color: new Color(0xff979797),
            fontSize: 16f
        );
        
        private static readonly TextStyle BreadcrumbHoverStyle = new TextStyle(
            color: new Color(0xff979797),
            fontSize: 16f,
            decoration: TextDecoration.underline
        ); 

        public override Widget build(BuildContext context)
        {
            return new Container(
                decoration: ContainerDecoration,
                margin: EdgeInsets.only(bottom: 12f),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: new List<Widget>
                    {
                        new Clickable(
                            onTap: () => LocationUtil.Go($"/Manual/{_item.name}"),
                            child: new Container(
                                margin: EdgeInsets.only(top: 24f, left: 24f, right: 24f, bottom: 8f),
                                child: new Text(
                                    _item.breadcrumbs.last().content,
                                    style: TitleTextStyle
                                )
                            )
                        ),
                        new Clickable(
                            onTap: () => LocationUtil.Go($"/Manual/{_item.name}"),
                            child: new Container(
                                child: new RichText(
                                    text: new TextSpan(
                                        children: _highlight
                                    )
                                ),
                                margin: EdgeInsets.only(bottom: 24f, left: 24f, right: 24f)
                            )
                        ),
                        Splitter,
                        new Container(
                            height: 44f,
                            padding: EdgeInsets.symmetric(horizontal: 24f),
                            child: new Align(
                                alignment: Alignment.centerLeft,
                                child: new Breadcrumbs(
                                    _item.breadcrumbs,
                                    splitterStyle: BreadcrumbSplitterStyle,
                                    normalBreadcrumbStyle: BreadcrumbStyle,
                                    hoverBreadCrumbStyle: BreadcrumbHoverStyle
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}