using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Resources = UnityEngine.Resources;
using Font = UnityEngine.Font;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN
{
    public class DocCNApp : StatefulWidget
    {
        public DocCNApp(Key key = null) : base(key)
        {
        }

        public override State createState() => new DocCNAppState();
    }

    internal class DocCNAppState : State<DocCNApp>
    {
        private ScrollController controller;

        public override void initState()
        {
            base.initState();
            controller = new ScrollController();
            FontManager.instance.addFont(Resources.Load<Font>("PingFang-Regular"));
        }

        public override Widget build(BuildContext context)
        {
            var column = new Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: new List<Widget>
                {
                    new Header(),
                    new Expanded(
                        child: new Container(
                            padding: EdgeInsets.only(top: 24.0f),
                            color: new Color(0xffffffff),
                            child: new Row(
                                children: new List<Widget>
                                {
                                    new Menu(),
                                    new Expanded(
                                        child: new Components.Markdown()
                                    ),
                                    new MetaFields()
                                }
                            )
                        )
                    )
                }
            );
            var defaultTextStyle = new DefaultTextStyle(
                child: column,
                style: new TextStyle(
                    fontFamily: "PingFang SC"
                )
            );

            return new Container(
                constraints: BoxConstraints.tight(MediaQuery.of(context).size),
                child: new SingleChildScrollView(
                    controller: controller,
                    child: new Container(
                        constraints: BoxConstraints.tight(MediaQuery.of(context).size), // comment this line
                        child: defaultTextStyle
                    )
                )
            );
        }
    }
}