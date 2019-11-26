using System.Collections.Generic;
using Unity.DocZh.Models.Json;
using Unity.DocZh.Utility;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public class SearchResultCard : StatelessWidget
    {
        public SearchResultCard(SearchResultItem item, Key key = null) : base(key)
        {
            _item = item;
            _highlight = SearchResultHighlightParser.Parse(
                _item.highlight,
                HighlightTextStyle,
                HighlightEmTextStyle
            );
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
            height: 1.333333333333333f.LineHeight()
        );

        private static readonly TextStyle HighlightTextStyle = new TextStyle(
            color: new Color(0xff212121),
            fontSize: 16f,
            height: 1.5f.LineHeight(),
            fontFamily: "PingFang"
        );

        private static readonly TextStyle HighlightEmTextStyle = new TextStyle(
            color: new Color(0xff212121),
            fontSize: 16f,
            height: 1.5f.LineHeight(),
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
            var children = new List<Widget>
            {
                new Clickable(
                    onTap: () => LocationUtil.Go($"/{_item.type.FirstCharToUpper()}/{_item.name}"),
                    child: new Container(
                        margin: EdgeInsets.only(top: 24f, left: 24f, right: 24f, bottom: 8f),
                        child: new Text(
                            _item.name,
                            style: TitleTextStyle
                        )
                    )
                )
            };
            if (_highlight != null)
            {
                children.Add(
                    new Clickable(
                        onTap: () => LocationUtil.Go($"/{_item.type.FirstCharToUpper()}/{_item.name}"),
                        child: new Container(
                            child: new RichText(
                                text: new TextSpan(
                                    children: _highlight
                                )
                            ),
                            margin: EdgeInsets.only(bottom: 24f, left: 24f, right: 24f)
                        )
                    )
                );
            }

            if (_item.breadcrumbs != null && _item.breadcrumbs.Count != 0)
            {
                children.Add(Splitter);
                children.Add(
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
                );
            }
            else
            {
                children.Add(
                    new Container(
                        height: 24f
                    )
                );
            }

            return new Container(
                decoration: ContainerDecoration,
                margin: EdgeInsets.only(bottom: 12f),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: children
                )
            );
        }
    }
}