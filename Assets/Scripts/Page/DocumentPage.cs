using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace DocCN.Page
{
    public class DocumentPage : StatelessWidget
    {
        public DocumentPage(string title) : base(new ObjectKey(title))
        {
            this.title = title;
        }

        private readonly string title;

        public override Widget build(BuildContext context)
        {
            var column = new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget>
                {
                    new Header(),
                    new Expanded(
                        child: new Container(
                            color: new Color(0xffffffff),
                            child: new Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget>
                                {
                                    new Menu(),
                                    new Expanded(
                                        child: new SingleChildScrollView(
                                            child: new Components.Markdown(this.title)
                                        )
                                    ),
                                    new MetaFields()
                                }
                            )
                        )
                    ),
                }
            );
            var container = new Container(
                height: MediaQuery.of(context).size.height,
                width: MediaQuery.of(context).size.width,
                child: column
            );
            return container;
        }
    }
}