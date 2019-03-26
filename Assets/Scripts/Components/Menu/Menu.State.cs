using System;
using System.Collections.Generic;
using DocCN.Style;
using Newtonsoft.Json;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public partial class Menu
    {
        private class MenuState : State<Menu>
        {
            private Models.Menu _menu;

            public override void initState()
            {
                base.initState();
                var request = UnityWebRequest.Get($"http://doc.unity.cn/Data/{widget._type}/toc.json");
                var asyncOperation = request.SendWebRequest();
                asyncOperation.completed += operation =>
                {
                    if (!mounted)
                    {
                        return;
                    }
                    var content = DownloadHandlerBuffer.GetContent(request);
                    using (WindowProvider.of(context).getScope())
                    {
                        setState(() => _menu = JsonConvert.DeserializeObject<Models.Menu>(content));
                    }
                };
            }

            public void UpdateMenu(Action action)
            {
                setState(action.Invoke);
            }

            private List<MenuItem> BuildMenuItems(IReadOnlyList<Models.Menu> toParse, int level = 0)
            {
                var items = new List<MenuItem>();
                foreach (var child in toParse)
                {
                    items.Add(
                        new MenuItem(
                            new UniqueKey(),
                            child,
                            this,
                            level: level,
                            hasChildren: child.children != null
                        )
                    );
                    if (child.children == null || !child.expanded)
                    {
                        continue;
                    }

                    var childrenOfChild = BuildMenuItems(child.children, level + 1);
                    items.AddRange(childrenOfChild);
                }

                return items;
            }

            public override Widget build(BuildContext buildContext)
            {
                var children = new Column(
                    children: new List<Widget>
                    {
                        new Container(
                            margin: EdgeInsets.only(
                                top: 32f,
                                bottom: 4f,
                                left: 48f
                            ),
                            height: 28f,
                            child: new Align(
                                alignment: Alignment.centerLeft,
                                child: new Text(
                                    "unity手册",
                                    style: new TextStyle(
                                        fontSize: 24.0f,
                                        fontFamily: "PingFang-W500"
                                    )
                                )
                            )
                        ),
                        new Container(
                            height: 1f,
                            margin: EdgeInsets.only(
                                bottom: 4f,
                                left: 48f
                            ),
                            color: new Color(0xffd8d8d8)
                        )
                    }
                );
                var items = new List<MenuItem>();
                if (_menu != null)
                {
                    items = BuildMenuItems(_menu.children);
                }

                return new Container(
                    width: 336.0f,
                    padding: EdgeInsets.only(right: 48f),
                    child: ListView.builder(
                        itemCount: items.Count + 1,
                        itemBuilder: (ctx, idx) =>
                        {
                            if (idx == 0)
                            {
                                return children;
                            }

                            return items[idx - 1];
                        }
                    )
                );
            }
        }
    }
}