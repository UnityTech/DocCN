using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DocCN.Models;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Debug = UnityEngine.Debug;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    internal class MarkdownContentBuilderContext
    {
        public TextStyle textStyle { get; set; }
        public Stack<TextSpan> inline { get; }
        public Container imageNode { get; set; }
        public List<List<Container>> rows { get; private set; }
        public List<Container> cells { get; private set; }

        public bool InTHead { get; set; }


        public MarkdownContentBuilderContext()
        {
            inline = new Stack<TextSpan>();
            rows = new List<List<Container>>();
            cells = new List<Container>();
        }

        public void Clear()
        {
            textStyle = null;
            inline.Clear();
            imageNode = null;
        }

        public void ClearColumn()
        {
            rows = new List<List<Container>>();
        }

        public void ClearCells()
        {
            cells = new List<Container>();
        }
    }

    internal partial class MarkdownContentState
    {
        private static readonly Dictionary<string, Func<Token, MarkdownContentBuilderContext, Widget>> Mappings;

        private static readonly EdgeInsets TABLE_CELL_PADDING =
            EdgeInsets.only(top: 4f, bottom: 12f, left: 16f, right: 16f);

        static MarkdownContentState()
        {
            Mappings = new Dictionary<string, Func<Token, MarkdownContentBuilderContext, Widget>>
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
                ["tr_open"] = ProcessTROpen,
                ["tr_close"] = ProcessTRClose,
                ["th_open"] = ProcessTHOpen,
                ["th_close"] = ProcessTHClose,
                ["tbody_open"] = ProcessTBodyOpen,
                ["tbody_close"] = ProcessTBodyClose,
                ["td_open"] = ProcessTDOpen,
                ["td_close"] = ProcessTDClose,
                ["image"] = ProcessImage
            };
        }

        private static Widget DummyProcess(Token token, MarkdownContentBuilderContext ctx) => null;

        private static Widget ProcessHeadingOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            switch (token.Tag)
            {
                case "h1":
                    ctx.textStyle = new TextStyle(
                        fontSize: 36.0f,
                        height: 1.16666666667f,
                        fontFamily: "PingFang SC"
                    );
                    break;
                case "h2":
                    ctx.textStyle = new TextStyle(
                        fontSize: 30.0f,
                        fontFamily: "PingFang SC"
                    );
                    break;
                case "h3":
                    ctx.textStyle = new TextStyle(
                        fontSize: 24.0f,
                        height: 1.26666666667f,
                        fontFamily: "PingFang SC"
                    );
                    break;
                default:
                    throw new FormatException();
            }

            return null;
        }

        private static Widget ProcessHeadingClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var richText = new RichText(text: ctx.inline.Pop());
            var container = new Container(
                margin: EdgeInsets.only(top: 40f),
                child: richText
            );
            ctx.Clear();
            return container;
        }

        private static Widget ProcessInline(Token token, MarkdownContentBuilderContext ctx)
        {
            if (ctx.textStyle == null) return null;
            ctx.inline.Push(
                new TextSpan(
                    children: new List<TextSpan>(),
                    style: ctx.textStyle
                )
            );

            foreach (var tokenChild in token.Children)
            {
                if (Mappings.ContainsKey(tokenChild.Type))
                {
                    Mappings[tokenChild.Type].Invoke(tokenChild, ctx);
                }
            }

            return null;
        }

        private static Widget ProcessText(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.inline.Peek().children.Add(new TextSpan(token.Content));
            return null;
        }

        private static Widget ProcessStrongOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.inline.Push(new TextSpan(
                    children: new List<TextSpan>(),
                    style: new TextStyle(
                        fontFamily: "PingFang-W500"
                    )
                )
            );
            return null;
        }

        private static Widget ProcessStrongClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var span = ctx.inline.Pop();
            ctx.inline.Peek().children.Add(span);
            return null;
        }

        private static Widget ProcessEmOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.inline.Push(new TextSpan(
                    children: new List<TextSpan>(),
                    style: new TextStyle(
                        fontStyle: FontStyle.italic
                    )
                )
            );
            return null;
        }

        private static Widget ProcessEmClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var span = ctx.inline.Pop();
            ctx.inline.Peek().children.Add(span);
            return null;
        }

        private static Widget ProcessLinkOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            var span = new TextSpan(
                children: new List<TextSpan>(),
                style: new TextStyle(
                    color: new Color(0xffe91e63),
                    decoration: TextDecoration.underline
                )
            );
            ctx.inline.Push(span);
            return null;
        }

        private static Widget ProcessLinkClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var span = ctx.inline.Pop();
            ctx.inline.Peek().children.Add(span);
            return null;
        }

        private static Widget ProcessTableOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.textStyle = new TextStyle(
                fontSize: 16.0f,
                height: 1.5f,
                fontFamily: "PingFang SC"
            );
            return null;
        }

        private static Widget ProcessTableClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var maxColumnCount = ctx.rows.Max(row => row.Count);
            ctx.rows.ForEach(row =>
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
            var tableRows = ctx.rows.Select(row => new TableRow(children: row.Cast<Widget>().ToList())).ToList();
            var table = new Container(
                margin: EdgeInsets.only(top: 16f, bottom: 24f),
                child: new Table(
                    border: TableBorder.all(
                        width: 1f,
                        color: new Color(0xffe0e0e0)
                    ),
                    defaultColumnWidth: new IntrinsicColumnWidth(1.0f),
                    children: tableRows
                )
            );
            ctx.ClearColumn();
            return table;
        }

        private static Widget ProcessTHeadOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.InTHead = true;
            return null;
        }

        private static Widget ProcessTHeadClose(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.InTHead = false;
            return null;
        }

        private static Widget ProcessTROpen(Token token, MarkdownContentBuilderContext ctx)
        {
            return null;
        }

        private static Widget ProcessTRClose(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.rows.Add(ctx.cells);
            ctx.ClearCells();
            return null;
        }

        private static Widget ProcessTHOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            return null;
        }

        private static Widget ProcessTHClose(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.cells.Add(
                new Container(
                    padding: TABLE_CELL_PADDING,
                    color: new Color(0xffe0e0e0),
                    child: new RichText(
                        text: ctx.inline.Pop()
                    )
                )
            );
            return null;
        }

        private static Widget ProcessTBodyOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            return null;
        }

        private static Widget ProcessTBodyClose(Token token, MarkdownContentBuilderContext ctx)
        {
            return null;
        }

        private static Widget ProcessTDOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            return null;
        }

        private static Widget ProcessTDClose(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.cells.Add(
                new Container(
                    padding: TABLE_CELL_PADDING,
                    child: new RichText(
                        text: ctx.inline.Pop()
                    )
                )
            );
            return null;
        }

        // assume image's format is fixed
        private static Widget ProcessImage(Token token, MarkdownContentBuilderContext ctx)
        {
            var urlExist = token.Attrs.Any(attr => attr[0] == "src");
            var url = urlExist
                ? $"http://doc.unity.cn{token.Attrs.Single(attr => attr[0] == "src")[1]}"
                : "http://images.performgroup.com/di/library/omnisport/59/5/james-harden-cropped_adrrcka6po2g1feaaueg721vj.jpg";

            var widgets = new List<Widget>
            {
                new Container(
                    child: Image.network(
                        url,
                        fit: BoxFit.fill)
                )
            };
            if (token.Children != null &&
                token.Children.Length == 1 &&
                token.Children[0].Type == "text")
            {
                widgets.Add(
                    new Container(
                        margin: EdgeInsets.only(top: 8.0f),
                        child: new Text(
                            token.Children[0].Content,
                            style: new TextStyle(
                                color: new Color(0xff9b9b9b)
                            )
                        )
                    )
                );
            }

            ctx.imageNode = new Container(
                margin: EdgeInsets.only(top: 16.0f),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: widgets
                )
            );

            return null;
        }

        private static Widget ProcessParagraphOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.textStyle = new TextStyle(
                fontSize: 16.0f,
                height: 1.5f,
                fontFamily: "PingFang SC"
            );
            return null;
        }

        private static Widget ProcessParagraphClose(Token token, MarkdownContentBuilderContext ctx)
        {
            if (ctx.imageNode != null)
            {
                var node = ctx.imageNode;
                ctx.Clear();
                return node;
            }

            var richText = new RichText(text: ctx.inline.Pop());
            var container = new Container(
                margin: EdgeInsets.only(top: 24f),
                child: richText
            );
            ctx.Clear();
            return container;
        }
    }
}