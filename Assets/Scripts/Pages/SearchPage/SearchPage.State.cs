using System.Collections.Generic;
using DocCN.Components;
using DocCN.Models.Json;
using Newtonsoft.Json;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine.Networking;

namespace DocCN.Pages
{
    public partial class SearchPage
    {
        private class SearchPageState : State<SearchPage>
        {
            private SearchResults _results;

            private string _keyword;

            public override void initState()
            {
                base.initState();
            }

            private void onSearch(
                string keyword,
                int page)
            {
                if (keyword == null)
                {
                    keyword = _keyword;
                }
                var url =
                    $"http://doc.unity.cn/api/documentation/search/v/2018.1/t/manual?query={keyword}&page={page}&pageSize=10";
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
                        var results = JsonConvert.DeserializeObject<SearchResults>(content);
                        setState(() =>
                        {
                            _keyword = keyword;
                            _results = results;
                        });
                    }
                };
            }

            public override Widget build(BuildContext buildContext)
            {
                var size = MediaQuery.of(context).size;
                var children = new List<Widget>();
                if (_results?.items != null)
                {
                    children.Add(
                        new Container(
                            height: 24f,
                            child: new Align(
                                alignment: Alignment.centerLeft,
                                child: new Text(
                                    $"您搜索的“{_keyword}”共有{_results.total}个匹配的结果",
                                    style: new TextStyle(
                                        fontSize: 16f,
                                        fontWeight: FontWeight.w500
                                    )
                                )
                            ),
                            margin: EdgeInsets.only(bottom: 16f)
                        )
                    );
                    foreach (var searchResultItem in _results.items)
                    {
                        children.Add(new SearchResultCard(searchResultItem));
                    }

                    children.Add(
                        new Container(
                            margin: EdgeInsets.only(top: 16f),
                            child: new Pager(
                                pages: _results.pages,
                                currentPage: _results.currentPage,
                                totalPages: _results.totalPages,
                                onPageChanged: page => onSearch(null, page)
                            )
                        )
                    );
                }

                var pageHeight = MediaQuery.of(context).size.height;
                const float minHeight = Header.Height + SearchBar.Height + Footer.Height;

                return new Container(
                    constraints: new BoxConstraints(
                        minHeight: size.height
                    ),
                    width: size.width,
                    child: new SingleChildScrollView(
                        child: new ScrollableOverlay(
                            child: new Column(
                                crossAxisAlignment: CrossAxisAlignment.stretch,
                                children: new List<Widget>
                                {
                                    new Header(),
                                    new SearchBar(
                                        onSearch: keyword => onSearch(keyword, 0)
                                    ),
                                    new Container(
                                        constraints: new BoxConstraints(
                                            minHeight: minHeight < pageHeight ? pageHeight - minHeight : 0
                                        ),
                                        padding: EdgeInsets.only(top: 16f, right: 48f, bottom: 48f, left: 48f),
                                        color: new Color(0xfff2f1f2),
                                        child: new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: children
                                        )
                                    ),
                                    new Footer(),
                                }
                            )
                        )
                    )
                );
            }
        }
    }
}