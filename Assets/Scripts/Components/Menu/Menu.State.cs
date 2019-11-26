using System;
using System.Collections.Generic;
using Unity.DocZh.Utility;
using Unity.DocZh.Utility.Json;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Components
{
    public partial class Menu
    {
        private class MenuState : State<Menu>
        {
            private Models.Json.Menu _menu;
            private string _currentLastSegment;

            public override void initState()
            {
                base.initState();
                
                _currentLastSegment = LastSegment(ObservableUtil.currentPath.value);
                ObservableUtil.currentPath.OnChanged += OnPathChanged;

                var version = DocApp.of(context).version;
                var request = UnityWebRequest.Get($"{Configuration.Instance.cdnPrefix}/{version.unity_version}/{version.parse_version}/{widget._type.TocFileName()}");
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
                        setState(() => _menu = Models.Json.Menu.FromJson(JsonValue.Parse(content)));
                    }
                };
            }

            public override void dispose()
            {
                ObservableUtil.currentPath.OnChanged -= OnPathChanged;
                base.dispose();
            }

            private static string LastSegment(string path)
            {
                var name = path;
                var idx = name.LastIndexOf('/');
                if (!string.IsNullOrEmpty(name) && idx == name.Length - 1)
                {
                    name = name.Substring(0, name.Length - 1);
                    idx = name.LastIndexOf('/');
                }

                return name.Substring(idx + 1);
            }
            
            private void OnPathChanged(string path)
            {
                if (!mounted)
                {
                    return;
                }

                using (WindowProvider.of(context).getScope())
                {
                    setState(() => _currentLastSegment = LastSegment(path));
                }
            }

            public void UpdateMenu(Action action)
            {
                setState(action.Invoke);
            }

            private List<MenuItem> BuildMenuItems(IEnumerable<Models.Json.Menu> toParse, int level = 0)
            {
                var items = new List<MenuItem>();
                foreach (var child in toParse)
                {
                    items.Add(
                        new MenuItem(
                            new ObjectKey(child.link), 
                            child,
                            this,
                            _currentLastSegment == child.link,
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
                                    widget._type.ToChinese(),
                                    style: new TextStyle(
                                        fontSize: 24.0f,
                                        fontWeight: FontWeight.w500
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
                    child: new Scroller(
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
                    )
                );
            }
        }
    }
}