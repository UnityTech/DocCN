using System;
using System.Collections.Generic;
using System.Linq;
using DocCN.Markdown;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using FontStyle = Unity.UIWidgets.ui.FontStyle;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public class MarkdownContent : StatefulWidget
    {
        public MarkdownContent(Key key) : base(key)
        {
        }

        public override State createState() => new MarkdownContentState();
    }

    internal class MarkdownContentBuilderContext
    {
        public TextStyle TextStyle { get; set; }
        public Stack<TextSpan> Inline { get; }
        public EdgeInsets Margin { get; set; }
        public Container ImageNode { get; set; }

        public MarkdownContentBuilderContext()
        {
            this.Inline = new Stack<TextSpan>();
        }

        public void Clear()
        {
            TextStyle = null;
            Inline.Clear();
            Margin = null;
            ImageNode = null;
        }
    }

    internal class MarkdownContentState : State<MarkdownContent>
    {
        private Token[] tokens;
        private static readonly Dictionary<string, Func<Token, MarkdownContentBuilderContext, Widget>> mappings;

        static MarkdownContentState()
        {
            mappings = new Dictionary<string, Func<Token, MarkdownContentBuilderContext, Widget>>
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
                ["image"] = ProcessImage
            };
        }

        private static Widget ProcessHeadingOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            switch (token.Tag)
            {
                case "h1":
                    ctx.TextStyle = new TextStyle(
                        fontSize: 36.0f,
                        height: 1.16666666667f,
                        fontFamily: "PingFang SC"
                    );
                    break;
                case "h3":
                    ctx.TextStyle = new TextStyle(
                        fontSize: 24.0f,
                        height: 1.26666666667f,
                        fontFamily: "PingFang SC"
                    );
                    ctx.Margin = EdgeInsets.only(top: 32.0f);
                    break;
                default:
                    throw new FormatException();
            }

            return null;
        }

        private static Widget ProcessHeadingClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var richText = new RichText(text: ctx.Inline.Pop());
            var container = new Container(
                margin: ctx.Margin,
                child: richText
            );
            ctx.Clear();
            return container;
        }

        private static Widget ProcessInline(Token token, MarkdownContentBuilderContext ctx)
        {
            if (ctx.TextStyle == null) return null;
            ctx.Inline.Push(new TextSpan(
                    children: new List<TextSpan>(),
                    style: ctx.TextStyle
                )
            );

            foreach (var tokenChild in token.Children)
            {
                if (mappings.ContainsKey(tokenChild.Type))
                {
                    mappings[tokenChild.Type].Invoke(tokenChild, ctx);
                }
            }

            return null;
        }

        private static Widget ProcessText(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.Inline.Peek().children.Add(new TextSpan(token.Content));
            return null;
        }

        private static Widget ProcessStrongOpen(Token token, MarkdownContentBuilderContext ctx)
        {
            ctx.Inline.Push(new TextSpan(
                    children: new List<TextSpan>(),
                    style: new TextStyle(
                        fontWeight: FontWeight.w700
                    )
                )
            );
            return null;
        }

        private static Widget ProcessStrongClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var span = ctx.Inline.Pop();
            ctx.Inline.Peek().children.Add(span);
            return null;
        }

        private static Widget ProcessEmOpen(Token token, MarkdownContentBuilderContext ctx)
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

        private static Widget ProcessEmClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var span = ctx.Inline.Pop();
            ctx.Inline.Peek().children.Add(span);
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
            ctx.Inline.Push(span);
            return null;
        }

        private static Widget ProcessLinkClose(Token token, MarkdownContentBuilderContext ctx)
        {
            var span = ctx.Inline.Pop();
            ctx.Inline.Peek().children.Add(span);
            return null;
        }

        // assume image's format is fixed
        private static Widget ProcessImage(Token token, MarkdownContentBuilderContext ctx)
        {
            var widgets = new List<Widget>
            {
                new Container(
                    height: 274.0f,
                    child: Image.network(
                        "http://images.performgroup.com/di/library/omnisport/59/5/james-harden-cropped_adrrcka6po2g1feaaueg721vj.jpg",
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

            ctx.ImageNode = new Container(
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
            ctx.TextStyle = new TextStyle(
                fontSize: 16.0f,
                height: 1.5f,
                fontFamily: "PingFang SC"
            );
            ctx.Margin = EdgeInsets.only(top: 24.0f);
            return null;
        }

        private static Widget ProcessParagraphClose(Token token, MarkdownContentBuilderContext ctx)
        {
            if (ctx.ImageNode != null)
            {
                var node = ctx.ImageNode;
                ctx.Clear();
                return node;
            }

            var richText = new RichText(text: ctx.Inline.Pop());
            var container = new Container(
                margin: ctx.Margin,
                child: richText
            );
            ctx.Clear();
            return container;
        }

        public override void initState()
        {
            base.initState();
            Token.Fetch("output.json", token => this.setState(() => this.tokens = token));
        }

        public override Widget build(BuildContext context)
        {
            if (tokens == null)
            {
                return new Container();
            }

            var widgetCursor = new MarkdownContentBuilderContext();

            var widgets = this.tokens
                .Where(token => mappings.ContainsKey(token.Type))
                .Select(token => mappings[token.Type].Invoke(token, widgetCursor))
                .Where(widget => !(widget is null)).ToList();

            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: widgets
            );
        }
    }
}