using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Models.Json;
using Unity.DocZh.Style;
using Unity.DocZh.Utility;
using Unity.DocZh.Utility.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public partial class MarkdownContent
    {
        private partial class MarkdownContentState : State<MarkdownContent>
        {
            private List<Token> _tokens;

            private Link _prevLink;

            private Link _nextLink;

            private List<Breadcrumb> _breadcrumbs;

            private ScrollController _scrollController;

            private List<ImageMeta> _imageMetas;

            private bool _loading;

            private List<TapGestureRecognizer> _spanRecognizers;

            public override void initState()
            {
                base.initState();
                _scrollController = new ScrollController();
                Load();
            }

            private void Load()
            {
                _loading = true;
                var version = DocApp.of(context).version;
                var url =
                    $"{Configuration.Instance.cdnPrefix}/{version.unity_version}/{version.parse_version}/manual/json/{widget._title.Replace('-', '_')}.json";
                var request = UnityWebRequest.Get(url);
                var asyncOperation = request.SendWebRequest();
                asyncOperation.completed += operation =>
                {
                    if (!mounted)
                    {
                        return;
                    }

                    using (WindowProvider.of(context).getScope())
                    {
                        if (request.responseCode == 404)
                        {
                            DocApp.of(context).ForceUnknown();
                        }
                        else
                        {
                            var content = DownloadHandlerBuffer.GetContent(request);
                            var model = ManualModel.FromJson(JsonValue.Parse(content));
                            setState(() =>
                            {
                                _tokens = model.tokens;
                                _prevLink = model.prev;
                                _nextLink = model.next;
                                _breadcrumbs = model.bread_crumb;
                                _imageMetas = model.image_meta;
                                _loading = false;
                            });
                        }
                    }
                };
            }

            public override void didUpdateWidget(StatefulWidget oldWidget)
            {
                base.didUpdateWidget(oldWidget);
                if (!(oldWidget is MarkdownContent old)) return;
                if (old._title != widget._title)
                {
                    Load();
                }
            }

            public override Widget build(BuildContext buildContext)
            {
                if (_loading)
                {
                    return new Container(
                        child: new Center(
                            child: new Loading(
                                size: 48f
                            )
                        )
                    );
                }

                var markdownBuildCtx = new BuilderContext(_imageMetas, buildContext);
                _spanRecognizers?.ForEach(recognizer => recognizer.dispose());

                var widgets = new List<Widget>();
                widgets.AddRange(_tokens
                    .Where(token => Mappings.ContainsKey(token.type))
                    .Select(token => Mappings[token.type].Invoke(token, markdownBuildCtx))
                    .Where(w => !(w is null))
                    .Select(w => new RepaintBoundary(child: w)));
                _spanRecognizers = markdownBuildCtx.SpanRecognizers;

                widgets.Insert(0,
                    new Container(
                        height: 64f,
                        padding: EdgeInsets.only(bottom: 8f),
                        decoration: new BoxDecoration(
                            border: new Border(
                                bottom: new BorderSide(
                                    color: new Color(0xffd8d8d8),
                                    width: 1f
                                )
                            )
                        ),
                        child: new Align(
                            alignment: Alignment.bottomLeft,
                            child: new Breadcrumbs(
                                _breadcrumbs,
                                normalBreadcrumbStyle: new TextStyle(
                                    fontSize: 16f,
                                    color: new Color(0xff979797)
                                ),
                                hoverBreadCrumbStyle: new TextStyle(
                                    fontSize: 16f,
                                    color: new Color(0xff979797),
                                    decoration: TextDecoration.underline
                                ),
                                splitterStyle: new TextStyle(
                                    fontSize: 16f,
                                    color: new Color(0xff979797)
                                )
                            )
                        )
                    )
                );

                var linkButtons = new List<Widget>();
                if (_prevLink != null)
                {
                    linkButtons.Add(
                        new Expanded(
                            child: new Container(
                                margin: EdgeInsets.only(right: 20),
                                child: new Button(
                                    text: _prevLink.content ?? "",
                                    onTap: () =>
                                    {
                                        if (!string.IsNullOrEmpty(_prevLink?.link))
                                        {
                                            LocationUtil.Go(
                                                $"/Manual/{_prevLink?.link}");
                                        }
                                    },
                                    prefix: Icons.MaterialArrowBack
                                )
                            )
                        )
                    );
                }
                else
                {
                    linkButtons.Add(
                        new Expanded(
                            flex: 1,
                            child: new Container()
                        )
                    ); // placeholder to use spaceBetween
                }

                if (_nextLink != null)
                {
                    linkButtons.Add(
                        new Expanded(
                            child: new Container(
                                margin: EdgeInsets.only(left: 20),
                                child: new Button(
                                    text: _nextLink.content ?? "",
                                    onTap: () =>
                                    {
                                        if (!string.IsNullOrEmpty(_nextLink.link))
                                        {
                                            LocationUtil.Go(
                                                $"/Manual/{_nextLink.link}");
                                        }
                                    },
                                    suffix: Icons.MaterialArrowForward
                                )
                            )
                        )
                    );
                }
                else
                {
                    linkButtons.Add(
                        new Expanded(
                            flex: 1,
                            child: new Container()
                        )
                    ); // placeholder to use spaceBetween
                }


                widgets.Add(
                    new Container(
                        margin: EdgeInsets.only(top: 32, bottom: 64),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: linkButtons
                        )
                    )
                );

                var version = DocApp.of(buildContext).version;

                return new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Expanded(
                            child: new Stack(
                                children: new List<Widget>
                                {
                                    new Scroller(
                                        child: new SingleChildScrollView(
                                            controller: _scrollController,
                                            child: new ScrollableOverlay(
                                                child: new Column(
                                                    children: new List<Widget>
                                                    {
                                                        new Container(
                                                            constraints: new BoxConstraints(
                                                                minHeight: MediaQuery.of(buildContext).size.height -
                                                                           Header.Height -
                                                                           SearchBar.Height -
                                                                           Footer.Height
                                                            ),
                                                            child: new Column(
                                                                mainAxisAlignment: MainAxisAlignment.start,
                                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                                children: widgets)
                                                        ),
                                                        new Footer(style: Footer.Light, showSocials: false)
                                                    }
                                                )
                                            )
                                        )
                                    ),
                                    new ToTop(_scrollController, displayThreshold: 128f)
                                }
                            )
                        ),
                        new MetaFields(
                            markdownBuildCtx.PositionRecords,
                            _scrollController,
                            $"https://github.com/UnityTech/documentation-cn/blob/{version.unity_version}/Manual/md/{widget._title}.md"
                        )
                    }
                );
            }
        }
    }
}