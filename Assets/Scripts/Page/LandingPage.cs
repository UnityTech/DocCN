using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.widgets;

namespace DocCN.Page
{
    public class LandingPage : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Column(
                children: new List<Widget>
                {
                    new Header(),
                    
                }
            );
        }
    }
}