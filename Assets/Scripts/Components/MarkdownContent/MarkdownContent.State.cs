using System.Collections.Generic;
using System.Linq;
using DocCN.Models.Json;
using DocCN.Style;
using DocCN.Utility;
using Newtonsoft.Json;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;

namespace DocCN.Components
{
    public partial class MarkdownContent
    {
        private partial class MarkdownContentState : State<MarkdownContent>
        {
            private Token[] _tokens;

            private Link _prevLink;

            private Link _nextLink;

            private Breadcrumb[] _breadcrumbs;

            private ScrollController _scrollController;

            public override void initState()
            {
                base.initState();
                _scrollController = new ScrollController();
                Load();
            }

            private void Load()
            {
                var url = $"http://doc.unity.cn/Data/Manual/{widget._title}.json";
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
                            var model = JsonConvert.DeserializeObject<ManualModel>(content);
                            setState(() =>
                            {
                                _tokens = model.tokens;
                                _prevLink = model.prev;
                                _nextLink = model.next;
                                _breadcrumbs = model.breadcrumbs;
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
                if (_tokens == null)
                {
                    return new Container();
                }

                var widgetCursor = new BuilderContext();

                var widgets = new List<Widget>();
                widgets.AddRange(_tokens
                    .Where(token => Mappings.ContainsKey(token.type))
                    .Select(token => Mappings[token.type].Invoke(token, widgetCursor))
                    .Where(w => !(w is null)));

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
                            child: new Breadcrumbs(_breadcrumbs)
                        )
                    )
                );

                widgets.Add(
                    new Container(
                        margin: EdgeInsets.only(top: 32, bottom: 64),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget>
                            {
                                new Button(
                                    text: _prevLink?.link ?? "",
                                    onTap: () =>
                                    {
                                        if (!string.IsNullOrEmpty(_prevLink?.link))
                                        {
                                            LocationUtil.Go($"/Manual/{_prevLink?.link}");
                                        }
                                    },
                                    prefix: Icons.MaterialArrowBack
                                ),
                                new Button(
                                    text: _nextLink?.link ?? "",
                                    onTap: () =>
                                    {
                                        if (!string.IsNullOrEmpty(_nextLink?.link))
                                        {
                                            LocationUtil.Go($"/Manual/{_nextLink?.link}");
                                        }
                                    },
                                    suffix: Icons.MaterialArrowForward
                                ),
                            }
                        )
                    )
                );

                widgets.Add(new Footer(style: Footer.Light, showSocials: false));

                return new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Expanded(
                            child: new SingleChildScrollView(
                                controller: _scrollController,
                                child: new ScrollableOverlay(
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: widgets
                                    )
                                )
                            )
                        ),
                        new MetaFields(widgetCursor.positionRecords, _scrollController)
                    }
                );
            }
        }
    }
}