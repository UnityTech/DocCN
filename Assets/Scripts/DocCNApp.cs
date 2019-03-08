using System.Collections.Generic;
using DocCN.Components;
using DocCN.Page;
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
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFang-Regular"));
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/Brands"));
        }

        public override Widget build(BuildContext context)
        {
            var defaultTextStyle = new DefaultTextStyle(
                child: new SearchPage(),
                style: new TextStyle(
                    fontFamily: "PingFang SC"
                )
            );

            return new SingleChildScrollView(
                    controller: controller,
                    child: new Container(
                        child: defaultTextStyle
                    )
                
            );
        }
    }
}