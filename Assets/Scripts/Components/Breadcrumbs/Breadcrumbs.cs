using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Models.Json;
using Unity.DocZh.Utility;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Components
{
    public partial class Breadcrumbs : StatelessWidget
    {
        public Breadcrumbs(
            List<Breadcrumb> breadcrumbs,
            TextStyle normalBreadcrumbStyle = null,
            TextStyle hoverBreadCrumbStyle = null,
            TextStyle splitterStyle = null,
            Key key = null) : base(key)
        {
            _breadcrumbs = breadcrumbs;
            _normalBreadcrumbStyle = normalBreadcrumbStyle;
            _hoverBreadCrumbStyle = hoverBreadCrumbStyle;
            _splitterStyle = splitterStyle;
        }

        private readonly List<Breadcrumb> _breadcrumbs;

        private readonly TextStyle _normalBreadcrumbStyle;

        private readonly TextStyle _hoverBreadCrumbStyle;

        private readonly TextStyle _splitterStyle;

        private const string _splitter = "/";

        public override Widget build(BuildContext context)
        {
            return new Wrap(
                runSpacing: 8f,
                children: _breadcrumbs?
                    .Skip(1) // Maybe the first one is not so reasonable to display on the page?
                    .Select<Breadcrumb, Widget>(
                        breadcrumb => new ClickableText(
                            text: breadcrumb.content,
                            normalStyle: _normalBreadcrumbStyle,
                            hoverStyle: _hoverBreadCrumbStyle,
                            onTap: () => LocationUtil.Go($"/Manual/{breadcrumb.link}")
                        )
                    ).SelectMany<Widget, Widget>((widget, i) =>
                    {
                        if (i == 0)
                        {
                            return new[] {widget};
                        }

                        return new[]
                        {
                            new Container(
                                margin: EdgeInsets.symmetric(horizontal: 8f),
                                child: new Text(
                                    _splitter,
                                    style: _splitterStyle
                                )
                            ),
                            widget,
                        };
                    }).ToList()
            );
        }
    }
}