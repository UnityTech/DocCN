using DocCN.Components;
using UniRx;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace DocCN
{
    public class PageCanvas : UIWidgetsPanel
    {
        protected override void OnEnable()
        {
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFang-Regular"));
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFang-W500"));
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/Brands"));
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/MaterialIcons-Regular"));
            base.OnEnable();
            Bridge.Initialize();
            Reactive.CurrentPath.Publish("/");
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
            Reactive.CurrentPath.SetValueAndForceNotify(pathname);
        }
    }
}