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
            base.OnEnable();
            Bridge.Initialize();
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFang-Regular"));
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/PingFang-W500"));
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/Brands"));
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/MaterialIcons-Regular"));
            Reactive.CurrentPath.Publish("/");
            Application.targetFrameRate = 60;
        }

        protected override Widget createWidget()
        {
            return new WidgetsApp(
                home: new DocCNApp(),
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