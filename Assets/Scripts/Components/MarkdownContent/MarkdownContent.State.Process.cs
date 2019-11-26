using System;
using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Style;
using Unity.DocZh.Models.Json;
using Unity.DocZh.Utility;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using FontStyle = Unity.UIWidgets.ui.FontStyle;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Components
{
    public partial class MarkdownContent
    {
        private partial class MarkdownContentState
        {
            private static readonly Dictionary<string, Func<Token, BuilderContext, Widget>> Mappings;

            private static readonly EdgeInsets TableCellPadding =
                EdgeInsets.only(top: 4f, bottom: 12f, left: 16f, right: 16f);

            private static readonly Color DividerColor = new Color(0xffe0e0e0);

            private static readonly HoverRecognizer HoverRecognizer;

            private static readonly TextStyle FenceTextStyle = new TextStyle(
                color: new Color(0xff212121),
                fontSize: 16f,
                height: 1.5f.LineHeight()
            );

            private static readonly Paint CodeInlineBackgroundPaint = new Paint
            {
                color = new Color(0xfff0f0f0)
            };

            private static readonly TextStyle CodeInlineTextStyle = new TextStyle(background: CodeInlineBackgroundPaint);

            static MarkdownContentState()
            {
                Mappings = new Dictionary<string, Func<Token, BuilderContext, Widget>>
                {
                    ["heading_open"] = ProcessHeadingOpen,
                    ["heading_close"] = ProcessHeadingClose,
                    ["inline"] = ProcessInline,
                    ["text"] = ProcessText,
                    ["paragraph_open"] = ProcessParagraphOpen,
                    ["paragraph_close"] = ProcessParagraphClose,
                    ["strong_open"] = ProcessStrongOpen,
                    ["strong_close"] = ProcessStrongClose,
                    ["em_open"] = ProcessEmOpen,
                    ["em_close"] = ProcessEmClose,
                    ["link_open"] = ProcessLinkOpen,
                    ["link_close"] = ProcessLinkClose,
                    ["table_open"] = ProcessTableOpen,
                    ["table_close"] = ProcessTableClose,
                    ["thead_open"] = DummyProcess,
                    ["thead_close"] = DummyProcess,
                    ["tr_open"] = DummyProcess,
                    ["tr_close"] = ProcessTRClose,
                    ["th_open"] = DummyProcess,
                    ["th_close"] = ProcessTHClose,
                    ["tbody_open"] = DummyProcess,
                    ["tbody_close"] = DummyProcess,
                    ["td_open"] = DummyProcess,
                    ["td_close"] = ProcessTDClose,
                    ["image"] = ProcessImage,
                    ["hr"] = ProcessHR,
                    ["fence"] = ProcessFence,
                    ["code_inline"] = ProcessCodeInline,
                    ["list_item_open"] = ProcessListItemOpen,
                    ["list_item_close"] = ProcessListItemClose,
                };

                HoverRecognizer = new HoverRecognizer
                {
                    OnPointerEnter = evt => Bridge.ChangeCursor("pointer"),
                    OnPointerLeave = () => Bridge.ChangeCursor("default")
                };
            }

            private static Widget DummyProcess(Token token, BuilderContext ctx) => null;

            private static Widget ProcessHeadingOpen(Token token, BuilderContext ctx)
            {
                switch (token.tag)
                {
                    case "h1":
                        ctx.TextStyle = new TextStyle(
                            fontSize: 36.0f,
                            height: 1.16666666667f.LineHeight(),
                            fontFamily: "PingFang"
                        );
                        break;
                    case "h2":
                        ctx.TextStyle = new TextStyle(
                            fontSize: 30.0f,
                            fontFamily: "PingFang",
                            height: 1.26666666667f.LineHeight()
                        );
                        ctx.UseNotifyContainer = true;
                        break;
                    case "h3":
                        ctx.TextStyle = new TextStyle(
                            fontSize: 24.0f,
                            height: 1.26666666667f.LineHeight(),
                            fontFamily: "PingFang"
                        );
                        break;
                    case "h4":
                        ctx.TextStyle = new TextStyle(
                            fontSize: 20.0f,
                            height: 1.26666666667f.LineHeight(),
                            fontFamily: "PingFang"
                        );
                        break;
                    case "h5":
                        ctx.TextStyle = new TextStyle(
                            fontSize: 18f,
                            height: 1.26666666667f.LineHeight(),
                            fontFamily: "PingFang"
                        );
                        break;
                    default:
                        throw new FormatException();
                }

                return null;
            }

            private static Widget ProcessHeadingClose(Token token, BuilderContext ctx)
            {
                var richText = new SelectableText(textSpan: ctx.Inline.Pop());
                Widget container = null;
                if (ctx.UseNotifyContainer)
                {
                    var positionRecord = new PositionRecord {title = ctx.Title};
                    container = new NotifyContainer(
                        margin: EdgeInsets.only(top: 40f),
                        child: richText,
                        notifyFn: fn => positionRecord.getPosition = fn
                    );
                    ctx.PositionRecords.Add(positionRecord);
                }
                else
                {
                    container = new Container(
                        margin: EdgeInsets.only(top: 40f),
                        child: richText
                    );
                }

                ctx.Clear();
                return container;
            }

            private static Widget ProcessInline(Token token, BuilderContext ctx)
            {
                if (ctx.UseNotifyContainer)
                {
                    ctx.Title = token.content;
                }

                if (ctx.TextStyle == null) return null;
                ctx.Inline.Push(
                    new TextSpan(
                        children: new List<TextSpan>(),
                        style: ctx.TextStyle
                    )
                );

                if (token.children != null)
                {
                    foreach (var tokenChild in token.children)
                    {
                        if (Mappings.ContainsKey(tokenChild.type))
                        {
                            Mappings[tokenChild.type].Invoke(tokenChild, ctx);
                        }
                    }
                }

                return null;
            }

            private static Widget ProcessText(Token token, BuilderContext ctx)
            {
                ctx.Inline.Peek().children.Add(
                    new TextSpan(
                        token.content,
                        recognizer: ctx.UseRecognizer ? ctx.SpanRecognizers.Last() : null,
                        hoverRecognizer: ctx.UseRecognizer ? HoverRecognizer : null
                    )
                );
                return null;
            }

            private static Widget ProcessStrongOpen(Token token, BuilderContext ctx)
            {
                ctx.Inline.Push(new TextSpan(
                        children: new List<TextSpan>(),
                        style: new TextStyle(
                            fontWeight: FontWeight.w500
                        )
                    )
                );
                return null;
            }

            private static Widget ProcessStrongClose(Token token, BuilderContext ctx)
            {
                var span = ctx.Inline.Pop();
                ctx.Inline.Peek().children.Add(span);
                return null;
            }

            private static Widget ProcessEmOpen(Token token, BuilderContext ctx)
            {
                ctx.Inline.Push(new TextSpan(
                        children: new List<TextSpan>(),
                        style: new TextStyle(
                            fontStyle: FontStyle.italic
                        )
                    )
                );
                return null;
            }

            private static Widget ProcessEmClose(Token token, BuilderContext ctx)
            {
                var span = ctx.Inline.Pop();
                ctx.Inline.Peek().children.Add(span);
                return null;
            }

            private static Widget ProcessLinkOpen(Token token, BuilderContext ctx)
            {
                var uriString = token.attrs[0][1];
                ctx.SpanRecognizers.Add(new TapGestureRecognizer
                {
                    onTap = () =>
                    {
                        var uri = new Uri(uriString);
                        switch (uri.Scheme.ToLower())
                        {
                            case "manual":
                                LocationUtil.Go($"/Manual/{uri.LocalPath}");
                                break;
                            case "http":
                            case "https":
                            case "mailto":
                                LocationUtil.HrefTo(uriString);
                                break;
                            case "scripting":
                                LocationUtil.Go($"/Scripting/{uri.LocalPath}");
                                break;
                            case "attachment":
                                LocationUtil.Download(
                                    $"{Configuration.Instance.apiHost}/api/documentation/resource/v/2018.1/t/manual_static/f/{uri.LocalPath}",
                                    uri.LocalPath
                                );
                                break;

                            default:
                                Debug.Log($"Unrecognized scheme of uri {uriString}");
                                break;
                        }
                    }
                });
                var span = new TextSpan(
                    children: new List<TextSpan>(),
                    style: new TextStyle(
                        color: new Color(0xffe91e63),
                        decoration: TextDecoration.underline
                    )
                );
                ctx.Inline.Push(span);
                ctx.UseRecognizer = true;
                return null;
            }

            private static Widget ProcessLinkClose(Token token, BuilderContext ctx)
            {
                var span = ctx.Inline.Pop();
                ctx.Inline.Peek().children.Add(span);
                ctx.UseRecognizer = false;
                return null;
            }

            private static Widget ProcessTableOpen(Token token, BuilderContext ctx)
            {
                ctx.TextStyle = new TextStyle(
                    fontSize: 16.0f,
                    height: 1.5f.LineHeight(),
                    fontFamily: "PingFang"
                );
                return null;
            }

            private static Widget ProcessTableClose(Token token, BuilderContext ctx)
            {
                var maxColumnCount = ctx.Rows.Max(row => row.Count);
                ctx.Rows.ForEach(row =>
                {
                    while (row.Count < maxColumnCount)
                    {
                        var cellContainer = row.Last();
                        row.Add(
                            new Container(
                                padding: cellContainer.padding,
                                decoration: cellContainer.decoration
                            )
                        );
                    }
                });
                var tableRows = ctx.Rows.Select(row => new TableRow(children: row.Cast<Widget>().ToList())).ToList();
                var table = new Container(
                    margin: EdgeInsets.only(top: 16f, bottom: 24f),
                    child: new Table(
                        border: TableBorder.all(
                            width: 1f,
                            color: new Color(0xffe0e0e0)
                        ),
                        defaultColumnWidth: new FlexColumnWidth(1.0f),
                        children: tableRows
                    )
                );
                ctx.ClearColumn();
                return table;
            }

            private static Widget ProcessTRClose(Token token, BuilderContext ctx)
            {
                ctx.Rows.Add(ctx.Cells);
                ctx.ClearCells();
                return null;
            }

            private static Widget ProcessTHClose(Token token, BuilderContext ctx)
            {
                ctx.Cells.Add(
                    new Container(
                        padding: TableCellPadding,
                        color: new Color(0xffe0e0e0),
                        child: new SelectableText(
                            textSpan: ctx.Inline.Pop()
                        )
                    )
                );
                return null;
            }

            private static Widget ProcessTDClose(Token token, BuilderContext ctx)
            {
                ctx.Cells.Add(
                    new Container(
                        padding: TableCellPadding,
                        child: new SelectableText(
                            textSpan: ctx.Inline.Pop()
                        )
                    )
                );
                return null;
            }

            // assume image's format is fixed
            private static Widget ProcessImage(Token token, BuilderContext ctx)
            {
                var imageName = token.attrs.Single(attr => attr[0] == "src")[1];
                var version = DocApp.of(ctx.Context).version;
                var url =
                    $"{Configuration.Instance.cdnPrefix}/{version.unity_version}/{version.parse_version}/manual/static/{imageName.Replace('-', '_')}";

                var widgets = new List<Widget>
                {
                    new ImageWithPlaceholder(
                        ctx.ImageMetas[imageName].width,
                        ctx.ImageMetas[imageName].height,
                        new NetworkImage(url)
                    ),
                };
                if (token.children != null &&
                    token.children.Count == 1 &&
                    token.children[0].type == "text")
                {
                    widgets.Add(
                        new Container(
                            margin: EdgeInsets.only(top: 8.0f),
                            child: new SelectableText(
                                token.children[0].content,
                                style: new TextStyle(
                                    color: new Color(0xff9b9b9b),
                                    fontFamily: "PingFang"
                                )
                            )
                        )
                    );
                }

                ctx.ImageNode = new Container(
                    margin: EdgeInsets.only(top: 16.0f),
                    child: new Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: widgets
                    )
                );

                return null;
            }

            private static Widget ProcessHR(Token token, BuilderContext ctx)
            {
                return new Container(
                    margin: EdgeInsets.symmetric(vertical: 16f),
                    height: 1f,
                    color: DividerColor
                );
            }

            private static Widget ProcessFence(Token token, BuilderContext ctx)
            {
                return new Container(
                    margin: EdgeInsets.only(top: 16),
                    decoration: new BoxDecoration(
                        border: Border.all(
                            color: new Color(0xffe0e0e0)
                        )
                    ),
                    padding: EdgeInsets.all(16f),
                    child: new Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget>
                        {
                            new Expanded(
                                child: new SelectableText(
                                    token.content.Trim(),
                                    style: FenceTextStyle
                                )
                            ),
                            new CopyIcon(token.content.Trim())
                        }
                    )
                );
            }

            private static Widget ProcessCodeInline(Token token, BuilderContext ctx)
            {
                ctx.Inline.Peek().children.Add(
                    new TextSpan(
                        token.content,
                        recognizer: ctx.UseRecognizer ? ctx.SpanRecognizers.Last() : null,
                        hoverRecognizer: ctx.UseRecognizer ? HoverRecognizer : null,
                        style: CodeInlineTextStyle
                    )
                );
                return null;
            }

            private static Widget ProcessListItemOpen(Token token, BuilderContext ctx)
            {
                ctx.ListItem = true;
                return null;
            }

            private static Widget ProcessListItemClose(Token token, BuilderContext ctx)
            {
                ctx.ListItem = false;
                return null;
            }

            private static Widget ProcessParagraphOpen(Token token, BuilderContext ctx)
            {
                ctx.TextStyle = new TextStyle(
                    fontSize: 16.0f,
                    height: 1.5f.LineHeight(),
                    fontFamily: "PingFang"
                );
                return null;
            }

            private static Widget ProcessParagraphClose(Token token, BuilderContext ctx)
            {
                if (ctx.ImageNode != null)
                {
                    var node = ctx.ImageNode;
                    ctx.Clear();
                    return node;
                }

                var richText = new SelectableText(
                    textSpan: ctx.Inline.Pop()
                );
                var container = new Container(
                    margin: EdgeInsets.only(top: 24f),
                    child: richText
                );
                ctx.Clear();
                if (ctx.ListItem)
                {
                    return new Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget>
                        {
                            new Container(
                                margin: EdgeInsets.only(top: 26f, right: 8f),
                                child: new Text("\u2022")
                            ),
                            new Expanded(
                                child: container
                            ),
                        }
                    );
                }
                return container;
            }
        }
    }
}