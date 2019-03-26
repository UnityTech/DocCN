using System.Collections.Generic;
using System.Linq;
using DocCN.Models.Json;
using Newtonsoft.Json;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine.Networking;

namespace DocCN.Components
{
    public partial class MarkdownContent
    {
        private partial class MarkdownContentState : State<MarkdownContent>
        {
            private Token[] _tokens;

            public override void initState()
            {
                base.initState();
                Load();
            }

            private void Load()
            {
                var url = $"http://doc.unity.cn/Data/manual_json/{widget._title}.json";
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

                return new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Breadcrumb(),
                        new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: widgets
                        ),
                        new Container(height: 64f),
                    }
                );
            }
        }
    }
}