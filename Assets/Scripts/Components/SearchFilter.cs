using System;
using System.Collections.Generic;
using DocCN.Style;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace DocCN.Components
{
    public class SearchFilter : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                width: 380.0f,
                child: new Column(
                    children: new List<Widget>
                    {
                        new Container(
                            height: 56.0f,
                            padding: EdgeInsets.only(left: 48f, right: 12f),
                            margin: EdgeInsets.only(bottom: 4f),
                            decoration: new BoxDecoration(
                                color: new Color(0xffffffff),
                                borderRadius: BorderRadius.circular(4.0f)
                            ),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget>
                                {
                                    new Text(
                                        "搜索结果筛选",
                                        style: new TextStyle(
                                            fontSize: 16f,
                                            color: new Color(0xff212121),
                                            fontWeight: FontWeight.w500
                                        )
                                    ),
                                    new Text(
                                        "清除所选",
                                        style: new TextStyle(
                                            fontSize: 14f,
                                            color: new Color(0xff3c83d4)
                                        )
                                    )
                                }
                            )
                        ),
                        new FilterItem()
                    }
                )
            );
        }
    }

    internal class FilterItem : StatefulWidget
    {
        public override State createState() => new FilterItemState();

        private class FilterItemState : SingleTickerProviderStateMixin<FilterItem>
        {
            private AnimationController controller;
            private Animation<float> animation;

            public override void initState()
            {
                base.initState();
                controller = new AnimationController(
                    duration: new TimeSpan(0, 0, 0, 0, 360),
                    vsync: this
                );
                animation = controller.drive(new FloatTween(0, Mathf.PI).chain(new CurveTween(Curves.easeInOut)));
                controller.addListener(() => setState(() => { }));
            }

            public override Widget build(BuildContext context)
            {
                var container = new Container(
                    margin: EdgeInsets.only(top: 8f),
                    decoration: new BoxDecoration(
                        new Color(0xffffffff),
                        borderRadius: BorderRadius.circular(4f)
                    ),
                    height: 48f,
                    padding: EdgeInsets.only(left: 16f, right: 32f),
                    child: new Row(
                        children: new List<Widget>
                        {
                            Transform.rotate(
                                degree: animation.value,
                                alignment: Alignment.center,
                                child: new Container(
                                    child: new Icon(
                                        Icons.MaterialExpandMore,
                                        color: new Color(0xffd8d8d8)
                                    )
                                )
                            ),
                            new Container(
                                height: 16f,
                                width: 16f,
                                margin: EdgeInsets.only(right: 16f)
                            ),
                            new Expanded(
                                child: new Text(
                                    "离线部署Unity",
                                    style: new TextStyle(
                                        color: new Color(0xff212121),
                                        fontSize: 16f
                                    )
                                )
                            ),
                            new CheckBox(value: CheckBoxValue.Unchecked),
                        }
                    )
                );
                return new GestureDetector(
                    onTap: () =>
                    {
                        if (controller.value > 0.5)
                        {
                            this.controller.reverse();                            
                        }
                        else
                        {
                            this.controller.forward();
                        }
                         
                    },
                    child: container
                );
            }
        }
    }
}