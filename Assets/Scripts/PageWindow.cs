using Unity.DocZh.Components;
using Unity.DocZh.Style;
using Unity.UIWidgets.editor;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Unity.DocZh.Utility;
using UnityEditor;
using UnityEngine;

namespace Unity.DocZh
{
    public class PageWindow : UIWidgetsEditorWindow
    {
        [MenuItem("Window/Documentation")]
        public static void Doc()
        {
            EditorWindow.GetWindow<PageWindow>(typeof(SceneView));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            var icon = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin
                ? "Images/icon_white"
                : "Images/icon_black");
            this.titleContent = new GUIContent("Documentation", icon);
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
    }
}