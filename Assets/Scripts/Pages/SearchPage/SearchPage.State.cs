using System;
using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Components;
using Unity.DocZh.Models.Json;
using Unity.DocZh.Utility;
using Unity.DocZh.Utility.Json;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Pages
{
    public partial class SearchPage
    {
        private class SearchPageState : State<SearchPage>
        {
            private SearchResults _results;

            private string _keyword;
            private int _page;
            private bool _searching;
            private SearchBar.FilterType _filterType;

            public override void initState()
            {
                base.initState();
                _searching = false;
                DoSearch();
            }

            public override void didUpdateWidget(StatefulWidget oldWidget)
            {
                base.didUpdateWidget(oldWidget);
                DoSearch();
            }

            private void DoSearch()
            {
                _keyword = Uri.UnescapeDataString(widget._keyword);
                _page = widget._page;
                _filterType = widget._filterType;

                if (_keyword == string.Empty)
                {
                    return;
                }

                _searching = true;
                var url =
                    $"{Configuration.Instance.apiHost}/api/documentation/search/v/2018.1/t/{_filterType}?query={_keyword}&page={_page}&pageSize=10";
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
                        var content = DownloadHandlerBuffer.GetContent(request);
                        var results = SearchResults.FromJson(JsonValue.Parse(content));
                        setState(() =>
                        {
                            _results = results;
                            _searching = false;
                        });
                    }
                };
            }

            public override Widget build(BuildContext buildContext)
            {
                var size = MediaQuery.of(context).size;
                var children = new List<Widget>();
                var emptyKeyword = string.IsNullOrEmpty(_keyword);
                var mainAxisAlignment = _searching || emptyKeyword ? MainAxisAlignment.center : MainAxisAlignment.start;
                var crossAxisAlignment = emptyKeyword ? CrossAxisAlignment.center : CrossAxisAlignment.start;

                if (emptyKeyword)
                {
                    children.Add(
                        new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget>
                            {
                                new Container(
                                    width: 253,
                                    height: 334,
                                    margin: EdgeInsets.only(bottom: 24f),
                                    decoration: new BoxDecoration(
                                        image: new DecorationImage(
                                            image: new AssetImage(
                                                "Images/search@3x"
                                            ),
                                            fit: BoxFit.cover
                                        )
                                    )
                                ),
                                new Text(
                                    "请输入关键词搜索",
                                    style: new TextStyle(
                                        fontSize: 24f,
                                        fontWeight: FontWeight.w500,
                                        color: new Color(0xff979797),
                                        height: 1.33333333f.LineHeight()
                                    )
                                )
                            }
                        )
                    );
                }
                else if (_searching)
                {
                    children.Add(
                        new Container(
                            child: new Center(
                                child: new Loading(
                                    size: 48f
                                )
                            )
                        )
                    );
                }
                else
                {
                    if (_results != null)
                    {
                        children.Add(
                            new Container(
                                height: 24f,
                                child: new Align(
                                    alignment: Alignment.centerLeft,
                                    child: new Text(
                                        _results.total == 0
                                            ? $"您搜索的 “{_keyword}” 暂无匹配的结果"
                                            : $"您搜索的“{_keyword}”共有{_results.total}个匹配的结果",
                                        style: new TextStyle(
                                            fontSize: 16f,
                                            fontWeight: FontWeight.w500
                                        )
                                    )
                                ),
                                margin: EdgeInsets.only(bottom: 16f)
                            )
                        );
                        if (_results.items != null)
                        {
                            children.AddRange(_results.items
                                .Select(searchResultItem => new SearchResultCard(searchResultItem))
                                .Cast<Widget>());
                        }

                        if (_results.pages != null)
                        {
                            children.Add(
                                new Container(
                                    margin: EdgeInsets.only(top: 16f),
                                    child: new Pager(
                                        pages: _results.pages,
                                        currentPage: _results.currentPage,
                                        totalPages: _results.totalPages,
                                        onPageChanged: page =>
                                            LocationUtil.Go(
                                                $"/Search/{_filterType}/{Uri.EscapeDataString(_keyword)}/{page}")
                                    )
                                )
                            );
                        }
                    }
                }


                var pageHeight = MediaQuery.of(context).size.height;
                const float minHeight = Header.Height + SearchBar.Height + Footer.Height;

                return new Container(
                    constraints: new BoxConstraints(
                        minHeight: size.height
                    ),
                    width: size.width,
                    child: new Scroller(
                        child: new SingleChildScrollView(
                            child: new ScrollableOverlay(
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.stretch,
                                    children: new List<Widget>
                                    {
                                        new Header(),
                                        new SearchBar(),
                                        new Container(
                                            constraints: new BoxConstraints(
                                                minHeight: minHeight < pageHeight ? pageHeight - minHeight : 0
                                            ),
                                            padding: EdgeInsets.only(top: 16f, right: 48f, bottom: 48f, left: 48f),
                                            color: new Color(0xfff2f1f2),
                                            child: new Column(
                                                crossAxisAlignment: crossAxisAlignment,
                                                mainAxisAlignment: mainAxisAlignment,
                                                children: children
                                            )
                                        ),
                                        new Footer(),
                                    }
                                )
                            )
                        )
                    )
                );
            }
        }
    }
}