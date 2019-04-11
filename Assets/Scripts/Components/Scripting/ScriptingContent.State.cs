using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using DocCN.Models.Json;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;
using FontStyle = Unity.UIWidgets.ui.FontStyle;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public partial class ScriptingContent
    {
        private static readonly TextStyle NormalTextStyle = new TextStyle(
            fontSize: 16f,
            height: 1.5f
        );

        private static readonly TextStyle HyperLinkStyle = new TextStyle(
            fontSize: 16f,
            height: 1.5f,
            color: new Color(0xffe91e63),
            decoration: TextDecoration.underline
        );

        private static readonly TextStyle TitleStyle = new TextStyle(
            fontSize: 30f,
            height: 38 / 30,
            color: new Color(0xff212121)
        );

        private class ScriptingContentState : State<ScriptingContent>
        {
            private Models.Json.Scripting _scripting;

            public override void initState()
            {
                base.initState();
                Load();
            }

            private void Load()
            {
                var url = $"http://doc.unity.cn/Data/scripting_json/{widget._title}.json";
                var request = UnityWebRequest.Get(url);
                var asyncOperation = request.SendWebRequest();
                asyncOperation.completed += operation =>
                {
                    if (!mounted)
                    {
                        return;
                    }

                    using (WindowProvider.of(context).getScope())
                    {
                        var content = DownloadHandlerBuffer.GetContent(request);
                        var scripting = JsonConvert.DeserializeObject<Models.Json.Scripting>(content);
                        setState(() => _scripting = scripting);
                    }
                };
            }

            public override void didUpdateWidget(StatefulWidget oldWidget)
            {
                base.didUpdateWidget(oldWidget);
                if (!(oldWidget is ScriptingContent old)) return;
                if (old._title != widget._title)
                {
                    Load();
                }
            }

            private static void BuildSegment(BuildContext context, ICollection<Widget> columnItems, string name,
                IReadOnlyCollection<Member> members)
            {
                if (members == null || members.Count == 0)
                {
                    return;
                }

                var container = new Container(
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
                                    style: TitleStyle
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
                                                        style: HyperLinkStyle
                                                    )
                                                ),
                                                new Container(
                                                    padding: EdgeInsets.only(
                                                        top: 4f,
                                                        bottom: 12f,
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
                columnItems.Add(container);
            }

            private static Widget BuildTextUsingMixedContent(BuildContext context,
                IEnumerable<MixedContent> summary)
            {
                if (summary is null)
                {
                    return new Container();
                }

                return new WealthyText(
                    textSpanList: summary.Select(item =>
                    {
                        switch (item)
                        {
                            case DocumentCharData charData:
                                return new TextSpan(charData.content);
                            case DocumentTagLink link:
                                return new TextSpan(
                                    link.content,
                                    style: HyperLinkStyle
                                );
                            case DocumentTagBreak br:
                                return new TextSpan("\n");
                            case DocumentTagBold bold:
                                return new TextSpan(
                                    bold.content,
                                    style: new TextStyle(
                                        fontWeight: FontWeight.w500
                                    )
                                );
                            case DocumentTagImage image:
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
                            case DocumentTagItalic italic:
                                return new TextSpan(
                                    italic.content,
                                    style: new TextStyle(
                                        fontStyle: FontStyle.italic
                                    )
                                );
                        }

                        return null;
                    }).Where(span => span != null).ToList(),
                    style: NormalTextStyle
                );
            }

            public override Widget build(BuildContext context)
            {
                if (_scripting == null)
                {
                    return new Container();
                }

                var children = new List<Widget>
                {
                    new Breadcrumbs(null),
                    new Container(
                        child: new Text(
                            widget._title.Split('.').Last().Replace("-", "."),
                            style: new TextStyle(
                                fontSize: 36f,
                                fontWeight: FontWeight.w500
                            )
                        )
                    )
                };

                foreach (var sections in _scripting.model.section)
                {
                    foreach (var section in sections)
                    {
                        switch (section)
                        {
                            case Summary summary:
                                children.Add(BuildTextUsingMixedContent(context, summary.value));
                                break;
                            case Description description:
                                children.Add(BuildTextUsingMixedContent(context, description.value));
                                break;
                            case Example example:
                                children.Add(
                                    new Container(
                                        decoration: new BoxDecoration(
                                            border: Border.all(
                                                width: 1f,
                                                color: new Color(0xffe0e0e0)
                                            )
                                        ),
                                        padding: EdgeInsets.all(24f),
                                        child: BuildTextUsingMixedContent(context, example.cSharp)
                                    )
                                );
                                break;
                        }
                    }
                }

                BuildSegment(context, children, "Static Properties", _scripting.model.staticVars);
                BuildSegment(context, children, "Properties", _scripting.model.vars);
                BuildSegment(context, children, "Constructors", _scripting.model.constructors);
                BuildSegment(context, children, "Public Methods", _scripting.model.memberFunctions);
                BuildSegment(context, children, "Protected Methods", _scripting.model.protectedFunctions);
                BuildSegment(context, children, "Static Methods", _scripting.model.staticFunctions);
                BuildSegment(context, children, "Operators", _scripting.model.operators);
                BuildSegment(context, children, "Messages", _scripting.model.messages);
                BuildSegment(context, children, "Events", _scripting.model.events);
                BuildSegment(context, children, "Delegates", _scripting.model.delegates);
                return new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: children
                );
            }
        }
    }
}