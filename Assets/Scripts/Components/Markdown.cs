using System.Collections.Generic;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public class Markdown : StatelessWidget
    {
        public Markdown(string title)
        {
            this.title = title;
        } 
        
        private readonly string title;
        public override Widget build(BuildContext context)
        {
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget>
                {
                    new Breadcrumb(),
                    new MarkdownContent(this.title),
                    new Container(height: 64f),
                }
            );
        }
    }
}