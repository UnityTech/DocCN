using Unity.UIWidgets.engine;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace DocCN
{
    public class ExampleCanvas : UIWidgetsPanel
    {
        protected override void OnEnable()
        {
            base.OnEnable();
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
    }
}