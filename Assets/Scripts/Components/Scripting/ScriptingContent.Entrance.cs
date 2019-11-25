using System.Collections.Generic;
using Unity.DocZh.Utility;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class ScriptingContent
    {
        public class Entrance : StatefulWidget
        {
            public override State createState() => new EntranceState();

            private static readonly TextStyle EntranceNormalTextStyle = new TextStyle(
                fontSize: 16,
                color: new Color(0xff212121),
                fontFamily: "PingFang",
                height: 1.5f.LineHeight()
            );

            private static readonly TextStyle EntranceBoldTextStyle = new TextStyle(
                fontWeight: FontWeight.w500
            );

            private static readonly TextStyle EntranceLinkTextStyle = new TextStyle(
                color: new Color(0xfff32194),
                decoration: TextDecoration.underline
            );

            private class EntranceState : State<Entrance>
            {
                private TapGestureRecognizer _scriptingSectionRecognizer;

                public override void initState()
                {
                    base.initState();
                    _scriptingSectionRecognizer = new TapGestureRecognizer
                    {
                        onTap = () => { LocationUtil.Go("/Manual/ScriptingSection"); }
                    };
                }

                public override void dispose()
                {
                    _scriptingSectionRecognizer?.dispose();
                    base.dispose();
                }

                public override Widget build(BuildContext buildContext)
                {
                    var container = new Container(
                        padding: EdgeInsets.only(right: 64f, top: 32, bottom: 32f),
                        child: new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget>
                            {
                                new Text(
                                    "欢迎使用 Unity Scripting Reference（脚本手册）！",
                                    style: new TextStyle(
                                        fontSize: 36,
                                        height: 1.16666666667f.LineHeight(),
                                        color: new Color(0xff212121)
                                    )
                                ),
                                new Container(
                                    margin: EdgeInsets.only(top: 36),
                                    child: new RichText(
                                        text: new TextSpan(
                                            children: new List<TextSpan>
                                            {
                                                new TextSpan(
                                                    "文档的这部分包含 Unity 提供的脚本 API 的详细信息。要使用这些信息，您应该了解 Unity 中脚本编写的基本理论和实践。我们使用手册的"),
                                                new TextSpan(
                                                    " Scripting（脚本）部分",
                                                    style: EntranceLinkTextStyle,
                                                    recognizer: _scriptingSectionRecognizer
                                                ),
                                                new TextSpan("对此进行了解释。")
                                            },
                                            style: EntranceNormalTextStyle
                                        )
                                    )
                                ),
                                new Container(
                                    margin: EdgeInsets.only(top: 24),
                                    child: new Text(
                                        "脚本引用文档是根据脚本可用的类建立的，同时会描述这些类的方法、属性以及任何与它们的用法相关的信息。",
                                        style: EntranceNormalTextStyle
                                    )
                                ),
                                new Container(
                                    margin: EdgeInsets.only(top: 24),
                                    child: new RichText(
                                        text: new TextSpan(
                                            children: new List<TextSpan>
                                            {
                                                new TextSpan(
                                                    "这些页面配有大量的示例代码，您可以将这些示例代码用于任何目的，而无需说明源自 Unity。此外，您可以使用每个页面顶部的菜单以 "),
                                                new TextSpan(
                                                    "C#",
                                                    style: EntranceBoldTextStyle
                                                ),
                                                new TextSpan(" 或 "),
                                                new TextSpan(
                                                    "JavaScript",
                                                    style: EntranceBoldTextStyle
                                                ),
                                                new TextSpan(" 查看示例代码。请注意，无论使用哪种编程语言，API 都是相同的，所以编程语言的选择完全取决于您的喜好。")
                                            },
                                            style: EntranceNormalTextStyle
                                        )
                                    )
                                ),

                                new Container(
                                    margin: EdgeInsets.symmetric(vertical: 24),
                                    child: new RichText(
                                        text: new TextSpan(
                                            children: new List<TextSpan>
                                            {
                                                new TextSpan("API 按其所属的 namespace 进行分组，可以从左侧的侧栏中选择。对于大多数用户，"),
                                                new TextSpan("UnityEngine"),
                                                new TextSpan("部分将是主要的调用端口。")
                                            },
                                            style: EntranceNormalTextStyle
                                        )
                                    )
                                ),
                            }
                        )
                    );
                    return new Scroller(
                        child: new SingleChildScrollView(
                            child: new ScrollableOverlay(
                                child: new Container(
                                    padding: EdgeInsets.only(right: 48f),
                                    child: new Column(
                                        crossAxisAlignment: CrossAxisAlignment.stretch,
                                        children: new List<Widget>
                                        {
                                            new Container(
                                                constraints: new BoxConstraints(
                                                    minHeight: MediaQuery.of(context).size.height - Header.Height -
                                                               SearchBar.Height - Footer.Height),
                                                child: container
                                            ),
                                            new Footer(style: Footer.Light, showSocials: false)
                                        }
                                    )
                                )
                            )
                        )
                    );
                }
            }
        }
    }
}