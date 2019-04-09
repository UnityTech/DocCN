using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEditor;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Utility.Components
{
    public class EditButton : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            var container = new Container(
                height: 48.0f,
                padding: EdgeInsets.only(left: 24.0f, right: 24.0f),
                decoration: new BoxDecoration(
                    border: Border.all(
                        width: 1.0f,
                        color: new Color(0xff00bcd4)
                    ),
                    borderRadius: BorderRadius.circular(3.0f)
                ),
                child: new Center(
                    child: new Text(
                        "编辑",
                        style: new TextStyle(
                            color: new Color(0xff00bcd4),
                            fontSize: 18.0f
                        )
                    )
                )
            );
            return new Listener(
                onPointerEnter: evt => Bridge.ChangeCursor("pointer"),
                onPointerLeave: evt => Bridge.ChangeCursor("default"),
                child: container
            );
        }
    }
}