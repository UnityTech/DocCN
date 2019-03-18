using System;
using System.Collections.Generic;
using System.Linq;
using DocCN.Models;
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
        public MarkdownContent(string title) : base(new ObjectKey("markdown-content"))
        {
            this.title = title;
        }

        public override State createState() => new MarkdownContentState();

        internal readonly string title;
    }

    internal partial class MarkdownContentState : State<MarkdownContent>
    {
        private Token[] tokens;
        

        public override void initState()
        {
            base.initState();
            Token.Fetch($"/Manual/{widget.title}", token =>
            {
                using (WindowProvider.of(context).getScope())
                {
                    this.setState(() => this.tokens = token);
                }
            });
        }

        public override void didUpdateWidget(StatefulWidget oldWidget)
        {
            base.didUpdateWidget(oldWidget);
            if (!(oldWidget is MarkdownContent old)) return;
            if (old.title != widget.title)
            {
                Token.Fetch($"/Manual/{widget.title}", token => setState(() => tokens = token));
            }
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