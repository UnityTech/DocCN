using System.Collections.Generic;
using System.Linq;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public partial class ScrollableOverlay
    {
        public class ScrollableOverlayState : State<ScrollableOverlay>
        {
            private List<WidgetBuilder> _entryBuilders;

            public override void initState()
            {
                _entryBuilders = new List<WidgetBuilder>();
                base.initState();
            }

            public void Add(WidgetBuilder builder) => setState(() => _entryBuilders.Add(builder));

            public void Remove(WidgetBuilder builder) => setState(() => _entryBuilders.Remove(builder));

            public override Widget build(BuildContext buildContext)
            {
                var children = new List<Widget> {widget._child};
                children.AddRange(_entryBuilders.Select(builder => builder.Invoke(buildContext)));
                return new Stack(
                    fit: StackFit.loose,
                    children: children
                );
            }
        }
    }
}