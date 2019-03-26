using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using DocCN.Models;
using UniRx.Async;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.ui.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public partial class ScriptingContent
    {
        private static readonly TextStyle NORMAL_TEXT_STYLE = new TextStyle(
            fontSize: 16f,
            height: 1.5f
        );

        private static readonly TextStyle HYPER_LINK_STYLE = new TextStyle(
            fontSize: 16f,
            height: 1.5f,
            color: new Color(0xffe91e63),
            decoration: TextDecoration.underline
        );

        private class ScriptingContentState : State<ScriptingContent>
        {
            private Models.Scripting scripting;

            public override void initState()
            {
                base.initState();
                var url = $"http://doc.unity.cn/Data/Scripting/EditorWindow.ShowPopup.xml";
                var request = UnityWebRequest.Get(url);
                var asyncOperation = request.SendWebRequest();
                asyncOperation.completed += operation =>
                {
                    if (!mounted)
                    {
                        return;
                    }
                    var content = DownloadHandlerBuffer.GetContent(request);
                    var xmlSerializer = new XmlSerializer(typeof(Models.Scripting));
                    var stringReader = new StringReader(content);
                    var scripting = xmlSerializer.Deserialize(stringReader);
                    using (WindowProvider.of(context).getScope())
                    {
                        setState(() => this.scripting = scripting as Models.Scripting);
                    }
                };
            }

            private static Widget BuildSegment(BuildContext context, string name, IEnumerable<Member> members)
            {
                return new Container(
                    child: new Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget>
                        {
                            new Container(
                                margin: EdgeInsets.only(
                                    top: 40f,
                                    bottom: 16f
                                ),
                                child: new Text(
                                    name,
                                    style: new TextStyle(
                                        fontSize: 30f,
                                        fontFamily: "PingFang-W500",
                                        height: 1.26666666667f
                                    )
                                )
                            ),
                            new Table(
                                defaultColumnWidth: new IntrinsicColumnWidth(1.0f),
                                children: members.Select((member, idx) =>
                                    {
                                        var even = idx % 2 == 0;
                                        return new TableRow(
                                            decoration: new BoxDecoration(
                                                color: even ? new Color(0xffeeeeee) : new Color(0x00000000)
                                            ),
                                            children: new List<Widget>
                                            {
                                                new Container(
                                                    padding: EdgeInsets.only(
                                                        top: 4f,
                                                        bottom: 12f,
                                                        left: 24f,
                                                        right: 24f
                                                    ),
                                                    child: new Text(
                                                        member.name,
                                                        style: HYPER_LINK_STYLE
                                                    )
                                                ),
                                                new Container(
                                                    padding: EdgeInsets.only(
                                                        top: 8f,
                                                        bottom: 8f,
                                                        left: 24f,
                                                        right: 24f
                                                    ),
                                                    child: BuildTextUsingMixedContent(context, member.summary)
                                                ),
                                            }
                                        );
                                    }
                                ).ToList()
                            )
                        }
                    )
                );
            }

            private static WealthyText BuildTextUsingMixedContent(BuildContext context, MixedContent summary)
            {
                return new WealthyText(
                    textSpanList: summary.items.Select(item =>
                    {
                        switch (item)
                        {
                            case string text:
                                return new TextSpan(
                                    text
                                );
                            case DocumentLink link:
                                return new TextSpan(
                                    link.value,
                                    style: HYPER_LINK_STYLE
                                );
                            case DocumentBreak br:
                                return new TextSpan(
                                    "\n"
                                );
                            case DocumentImage image:
                                var networkImage = new NetworkImage(
                                    "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/James_Harden_%2830735342912%29.jpg/220px-James_Harden_%2830735342912%29.jpg");
                                networkImage.resolve(new ImageConfiguration())
                                    .addListener((info, call) =>
                                    {
                                        Debug.Log($"{info.image.width}*{info.image.height}");
                                    });
                                return new ImageSpan(
                                    networkImage,
                                    imageWidth: 220f,
                                    imageHeight: 337f
                                );
                        }

                        return null;
                    }).Where(span => span != null).ToList(),
                    style: NORMAL_TEXT_STYLE
                );
            }

            public override Widget build(BuildContext context)
            {
                if (scripting == null)
                {
                    return new Container();
                }

                return new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>
                    {
                        new Breadcrumb(),
                        new Text(
                            "AudioMixer",
                            style: new TextStyle(
                                fontSize: 36f
                            )
                        ),
                        new Container(
                            child: new Text(
                                "NavMesh",
                                style: new TextStyle(
                                    fontSize: 36f,
                                    fontFamily: "PingFang-W500"
                                )
                            )
                        ),
                        BuildTextUsingMixedContent(context, scripting.model.section.description),
                        new Container(
                            padding: EdgeInsets.all(24f),
                            margin: EdgeInsets.only(top: 24f),
                            decoration: new BoxDecoration(
                                border: Border.all(
                                    color: new Color(0xffe0e0e0),
                                    width: 1f
                                )
                            ),
                            child: BuildTextUsingMixedContent(context, scripting.model.section.example.cSharp)
                        ),
                        //BuildSegment(context, "静态属性", scripting.model.staticVars),
                        //BuildSegment(context, "静态方法", scripting.model.staticFunctions),
                        //BuildSegment(context, "委托", scripting.model.delegates),
                        new Container(height: 64f),
                    }
                );
            }
        }
    }
}