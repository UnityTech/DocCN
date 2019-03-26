using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DocCN.Models;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;
using Icons = DocCN.Style.Icons;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public class Menu : StatefulWidget
    {
        public Menu() : base(new ObjectKey("menu"))
        {
        }

        public override State createState() => new MenuState();
    }

    internal class MenuState : State<Menu>
    {
        private Models.Menu menu;

        public override void initState()
        {
            base.initState();
            var request = UnityWebRequest.Get("http://doc.unity.cn/Menu/toc");
            var asyncOperation = request.SendWebRequest();
            asyncOperation.completed += operation =>
            {
                var content = DownloadHandlerBuffer.GetContent(request);
                using (WindowProvider.of(context).getScope())
                {
                    setState(() => menu = JsonConvert.DeserializeObject<Models.Menu>(content));
                }
            };
        }

        public void UpdateMenu(Action action)
        {
            this.setState(action.Invoke);
        }

        private List<MenuItem> BuildMenuItems(IReadOnlyList<Models.Menu> toParse, int level = 0)
        {
            var items = new List<MenuItem>();
            for (var i = 0; i < toParse.Count; i++)
            {
                var child = toParse[i];
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

        public override Widget build(BuildContext context)
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
            if (this.menu != null)
            {
                items = BuildMenuItems(menu.children);
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

    internal class MenuItem : StatefulWidget
    {
        public MenuItem(
            Key key,
            Models.Menu bind,
            MenuState parent,
            bool hasChildren,
            int level = 0
        ) : base(key)
        {
            this.bind = bind;
            this.parent = parent;
            this.level = level;
            this.hasChildren = hasChildren;
        }

        internal readonly Models.Menu bind;

        internal readonly int level;

        internal readonly string text;

        internal readonly string link;

        internal readonly MenuState parent;

        internal readonly bool hasChildren;

        public override State createState() => new MenuItemState();
    }

    internal class MenuItemState : State<MenuItem>
    {
        private static readonly Color NORMAL_COLOR = new Color(0x00000000);
        private static readonly Color HOVER_COLOR = new Color(0xfff5f5f5);
        private static readonly Color ICON_COLOR = new Color(0xffd8d8d8);

        private bool hover;

        public override void initState()
        {
            base.initState();
            hover = false;
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            if (mounted)
            {
                setState(() =>
                {
                    hover = true;
                    Bridge.ChangeCursor("pointer");
                });
            }
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            if (mounted)
            {
                setState(() =>
                {
                    hover = false;
                    Bridge.ChangeCursor("default");
                });
            }
        }

        private void OnTap()
        {
            Debug.Log(widget.bind.link);
            var link = $"/Manual/{widget.bind.link}";
            Bridge.LocationPush(widget.text, link);
            Reactive.CurrentPath.SetValueAndForceNotify(link);
        }

        private void OnExpandTap()
        {
            widget.parent.UpdateMenu(() => widget.bind.expanded = !widget.bind.expanded);
        }

        public override Widget build(BuildContext context)
        {
            var children = new List<Widget>
            {
                new Expanded(
                    child: new Container(
                        decoration: new BoxDecoration(
                            border: new Border(
                                left: new BorderSide(
                                    width: 2f,
                                    color: widget.level > 1 ? ICON_COLOR : NORMAL_COLOR
                                )
                            )
                        ),
                        margin: EdgeInsets.only(
                            left: 32f + 16f * widget.level
                        ),
                        padding: EdgeInsets.only(
                            top: 4f,
                            bottom: 12f
                        ),
                        child: new Container(
                            margin: EdgeInsets.only(left: 14f),
                            child: new Text(
                                widget.bind.title,
                                style: new TextStyle(
                                    fontSize: 16f,
                                    height: 1.5f,
                                    color: new Color(0xff616161)
                                )
                            )
                        )
                    )
                ),
            };
            if (widget.hasChildren)
            {
                children.Add(new GestureDetector(
                        onTap: OnExpandTap,
                        child: new Container(
                            padding: EdgeInsets.only(top: 4f),
                            child: new Icon(
                                Icons.MaterialExpandMore,
                                color: ICON_COLOR,
                                size: 24f
                            )
                        )
                    )
                );
            }

            var self = new Listener(
                onPointerEnter: OnPointerEnter,
                onPointerLeave: OnPointerLeave,
                child: new GestureDetector(
                    onTap: OnTap,
                    child: new Container(
                        color: hover ? HOVER_COLOR : NORMAL_COLOR,
                        padding: EdgeInsets.only(right: 16f),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            crossAxisAlignment: CrossAxisAlignment.center,
                            children: children
                        )
                    )
                )
            );

            return self;
        }
    }
}