using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public class Content<T> : StatelessWidget where T : Widget
    {
        public Content(T child)
        {
            this.child = child;
        }
        
        private readonly T child;

        public override Widget build(BuildContext context)
        {
            return new Container(
                padding: EdgeInsets.only(top: 16f, right: 48f, bottom: 48f, left: 48f),
                color: new Color(0xfff2f1f2),
                child: child
            );
        }
    }
}