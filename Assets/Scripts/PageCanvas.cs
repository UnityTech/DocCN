using DocCN.Components;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace DocCN.Utility
{
    public class PageCanvas : UIWidgetsPanel
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFang-Regular"), "PingFang");
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFang-W500"), "PingFang", FontWeight.w500);
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/Brands"), "Brands");
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/MaterialIcons-Regular"), "MaterialIcons");
            Bridge.Initialize();
            Application.targetFrameRate = 60;
        }

        protected override Widget createWidget()
        {
            return new WidgetsApp(
                home: new DocApp(),
                pageRouteBuilder: (settings, builder) =>
                    new PageRouteBuilder(
                        settings: settings,
                        pageBuilder: (context, animation, secondaryAnimation) => builder(context)
                    )
            );
        }
    }
}