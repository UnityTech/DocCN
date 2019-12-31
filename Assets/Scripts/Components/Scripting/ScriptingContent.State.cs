using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Models.Json;
using Unity.DocZh.Utility;
using Unity.DocZh.Utility.Json;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;
using FontStyle = Unity.UIWidgets.ui.FontStyle;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Components
{
    public partial class ScriptingContent
    {
        private static readonly TextStyle NormalTextStyle = new TextStyle(
            fontSize: 16f,
            height: 1.5f.LineHeight()
        );

        private static readonly TextStyle HyperLinkStyle = new TextStyle(
            fontSize: 16f,
            height: 1.5f.LineHeight(),
            color: new Color(0xffe91e63),
            decoration: TextDecoration.underline
        );

        private static readonly TextStyle TitleStyle = new TextStyle(
            fontSize: 30f,
            height: 1.26666666667f.LineHeight(),
            color: new Color(0xff212121)
        );

        private static readonly TextStyle SecondaryTitleStyle = new TextStyle(
            fontSize: 24f,
            height: 1.5f.LineHeight(),
            color: new Color(0xff616161)
        );

        private static readonly TextStyle NamespaceStyle = new TextStyle(
            fontSize: 16f,
            height: 1.5f.LineHeight(),
            color: new Color(0xff979797)
        );

        private static readonly EdgeInsets TitleMargin = EdgeInsets.only(top: 40f, bottom: 16f);
        private static readonly EdgeInsets SecondaryTitleMargin = EdgeInsets.only(top: 24f, bottom: 16f);

        private class ScriptingContentState : State<ScriptingContent>
        {
            private Models.Json.Scripting _scripting;

            private bool _loading = false;

            private Dictionary<string, ImageMeta> _imageMetas;

            private ScrollController _scrollController;

            public override void initState()
            {
                base.initState();
                _scrollController = new ScrollController();
                Load();
            }

            public override void dispose()
            {
                _scrollController.dispose();
                base.dispose();
            }

            private void Load()
            {
                _loading = true;
                _imageMetas = new Dictionary<string, ImageMeta>();
                var version = DocApp.of(context).version;
                var url =
                    $"{Configuration.Instance.cdnPrefix}/{version.unity_version}/{version.parse_version}/scripting/json/{widget._title.Replace('-', '_')}.json";
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
                        var scripting = Unity.DocZh.Models.Json.Scripting.FromJson(JsonValue.Parse(content));
                        setState(() =>
                        {
                            _scripting = scripting;
                            _loading = false;
                            _imageMetas = scripting.imageMetas?.ToDictionary(
                                meta => meta.name,
                                meta => meta);
                        });
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

            private void BuildSegment(
                BuildContext context,
                ICollection<Widget> columnItems,
                string name,
                IReadOnlyCollection<Member> members,
                bool primary = true)
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
                                margin: primary ? TitleMargin : SecondaryTitleMargin,
                                child: new SelectableText(
                                    name,
                                    style: primary ? TitleStyle : SecondaryTitleStyle
                                )
                            ),
                            new Table(
                                defaultColumnWidth: new FlexColumnWidth(1.0f),
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
                                                        top: 8f,
                                                        bottom: 8f,
                                                        left: 24f,
                                                        right: 24f
                                                    ),
                                                    child: new SelectableText(
                                                        textSpan: new TextSpan(
                                                            text: member.name,
                                                            style: HyperLinkStyle,
                                                            recognizer: new TapGestureRecognizer
                                                            {
                                                                onTap = () => LocationUtil.Go($"/Scripting/{member.id}")
                                                            }
                                                        )
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
                columnItems.Add(container);
            }

            private Widget BuildTextUsingMixedContent(
                BuildContext context,
                IEnumerable<MixedContent> summary)
            {
                if (summary is null)
                {
                    return new Container();
                }

                var version = DocApp.of(context).version;

                return new SelectableWealthyText(
                    textSpanList: summary.Select(item =>
                    {
                        switch (item)
                        {
                            case DocumentCharData charData:
                                return new TextSpan(charData.content);
                            case DocumentTagLink link:
                                return new TextSpan(
                                    link.content,
                                    style: HyperLinkStyle,
                                    recognizer: new TapGestureRecognizer
                                    {
                                        onTap = () => LocationUtil.Go(link.@ref),
                                    }
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
                                var networkImage =
                                    new NetworkImage(
                                        $"{Configuration.Instance.cdnPrefix}/{version.unity_version}/{version.parse_version}/scripting/static/{image.name.Replace('-', '_')}");
                                networkImage.resolve(new ImageConfiguration());
                                return new ImageSpan(
                                    networkImage,
                                    margin: EdgeInsets.symmetric(vertical: 16f),
                                    imageWidth: _imageMetas[image.name].width,
                                    imageHeight: _imageMetas[image.name].height
                                );
                            case DocumentTagItalic italic:
                                return new TextSpan(
                                    italic.content,
                                    style: new TextStyle(
                                        fontStyle: FontStyle.italic
                                    )
                                );
                            case DocumentTagMonoType monoType:
                                return new TextSpan(
                                    monoType.content
                                );
                        }

                        return null;
                    }).Where(span => span != null).ToList(),
                    style: NormalTextStyle
                );
            }

            public override Widget build(BuildContext context)
            {
                if (_loading)
                {
                    return new Container(
                        child: new Center(
                            child: new Loading(
                                size: 48f
                            )
                        )
                    );
                }

                var children = new List<Widget>
                {
                    new Container(
                        margin: EdgeInsets.only(top: 24, bottom: 8f),
                        child: new Row(
                            children: new List<Widget>
                            {
                                new Expanded(
                                    child: new SelectableText(
                                        widget._title.Split('.').Last().Replace("-", "."),
                                        style: new TextStyle(
                                            fontSize: 36f,
                                            fontWeight: FontWeight.w500
                                        )
                                    )
                                ),
                                new HyperLink(
                                    text: "在Github上编辑本文",
                                    style: new TextStyle(
                                        fontSize: 16,
                                        decoration: TextDecoration.underline,
                                        color: new Color(0xff2196f3)
                                    ),
                                    link:
                                    $"https://github.com/UnityTech/documentation-cn/blob/2018.1/ScriptingAPI/xml/{widget._title}.xml"
                                )
                            }
                        )
                    ),
                };

                if (!string.IsNullOrEmpty(_scripting.model.@namespace))
                {
                    children.Add(new Container(
                            child: new SelectableText(
                                $"class in {_scripting.model.@namespace}",
                                style: NamespaceStyle
                            ),
                            margin: EdgeInsets.only(bottom: 24)
                        )
                    );
                }

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
                                if (example.cSharp != null)
                                {
                                    children.Add(
                                        new Container(
                                            decoration: new BoxDecoration(
                                                border: Border.all(
                                                    width: 1f,
                                                    color: new Color(0xffe0e0e0)
                                                )
                                            ),
                                            padding: EdgeInsets.all(24f),
                                            margin: EdgeInsets.only(top: 16, bottom: 24),
                                            child: new Row(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget>
                                                {
                                                    new Expanded(
                                                        child: BuildTextUsingMixedContent(context, example.cSharp)
                                                    ),
                                                    new CopyIcon(string.Join(string.Empty,
                                                        example.cSharp.Select(c => c.ToString())))
                                                }
                                            )
                                        )
                                    );
                                }

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
                if (_scripting.model.baseType != null)
                {
                    children.Add(
                        new Container(
                            margin: TitleMargin,
                            child: new Text(
                                "Inherited Members",
                                style: TitleStyle
                            )
                        )
                    );
                    BuildSegment(context, children, "Static Properties", _scripting.model.baseType.staticVars, false);
                    BuildSegment(context, children, "Properties", _scripting.model.baseType.vars, false);
                    BuildSegment(context, children, "Constructors", _scripting.model.baseType.constructors, false);
                    BuildSegment(context, children, "Public Methods", _scripting.model.baseType.memberFunctions, false);
                    BuildSegment(context, children, "Protected Methods", _scripting.model.baseType.protectedFunctions,
                        false);
                    BuildSegment(context, children, "Static Methods", _scripting.model.baseType.staticFunctions, false);
                    BuildSegment(context, children, "Operators", _scripting.model.baseType.operators, false);
                    BuildSegment(context, children, "Messages", _scripting.model.baseType.messages, false);
                    BuildSegment(context, children, "Events", _scripting.model.baseType.events, false);
                    BuildSegment(context, children, "Delegates", _scripting.model.baseType.delegates, false);
                }

                var padding = new Container(height: 48f);
                children.Add(padding);
                return new Stack(
                    children: new List<Widget>
                    {
                        new Scroller(
                            child: new SingleChildScrollView(
                                controller: _scrollController,
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
                                                                   SearchBar.Height - Footer.Height
                                                    ),
                                                    child: new Column(
                                                        mainAxisAlignment: MainAxisAlignment.start,
                                                        crossAxisAlignment: CrossAxisAlignment.start,
                                                        children: children)
                                                    ),
                                                new Footer(style: Footer.Light, showSocials: false)
                                            }
                                        )
                                    )
                                )
                            )
                        ),
                        new ToTop(_scrollController, 128f)
                    }
                );
            }
        }
    }
}