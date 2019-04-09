using System.Collections.Generic;
using System.Linq;
using DocCN.Utility.Models.Json;
using Newtonsoft.Json;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;

namespace DocCN.Utility.Components
{
    public partial class MarkdownContent
    {
        private partial class MarkdownContentState : State<MarkdownContent>
        {
            private Token[] _tokens;

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
                        var content = DownloadHandlerBuffer.GetContent(request);
                        var token = JsonConvert.DeserializeObject<Token[]>(content);
                        setState(() => _tokens = token);
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

                var widgets = _tokens
                    .Where(token => Mappings.ContainsKey(token.type))
                    .Select(token => Mappings[token.type].Invoke(token, widgetCursor))
                    .Where(w => !(w is null)).ToList();

                return new Row(
                    children: new List<Widget>
                    {
                        new Expanded(
                            child: new SingleChildScrollView(
                                controller: _scrollController,
                                child: new Container(
                                    padding: EdgeInsets.only(bottom: 64f),
                                    child: new Column(
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: widgets
                                    )
                                )
                            )
                            /*child: ListView.builder(
                                controller: _scrollController,
                                padding: EdgeInsets.only(bottom: 64f),
                                itemCount: widgets.Count + 1,
                                itemBuilder: (ctx, idx) => idx == 0 ? new Breadcrumb() : widgets[idx - 1])*/
                        ),
                        new MetaFields(widgetCursor.positionRecords, _scrollController)
                    }
                );
            }
        }
    }
}