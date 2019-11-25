using Unity.DocZh.Components;
using Unity.DocZh.Style;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Unity.DocZh.Utility;
using UnityEngine;

namespace Unity.DocZh
{
    public class PageCanvas : UIWidgetsPanel
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/Brands"), "Brands");
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/MaterialIcons-Regular"), "MaterialIcons");
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/NotoSans"), "NotoSans");
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFangHeiTC-W4"), "PingFang");
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFangHeiTC-W6"), "PingFang", FontWeight.w500);
            Icons.LoadIconFont();
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
        
        public void LocationChange(string pathname)
        {
            ObservableUtil.currentPath.value = pathname;
        }
    }
}