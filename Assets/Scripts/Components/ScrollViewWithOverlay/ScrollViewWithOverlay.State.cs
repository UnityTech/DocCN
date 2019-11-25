using System;
using System.Collections.Generic;
using System.Linq;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class ScrollableOverlay
    {
        public class ScrollableOverlayState : State<ScrollableOverlay>
        {
            private Dictionary<Guid, WidgetBuilder> _entryBuilders;

            public override void initState()
            {
                _entryBuilders = new Dictionary<Guid, WidgetBuilder>();
                base.initState();
            }

            public Guid Add(WidgetBuilder builder)
            {
                var guid = Guid.NewGuid();
                setState(() => _entryBuilders[guid] = builder);
                return guid;
            }

            public void Remove(Guid guid) => setState(() => _entryBuilders.Remove(guid));

            public override Widget build(BuildContext buildContext)
            {
                var children = new List<Widget> {widget._child};
                children.AddRange(_entryBuilders.Values.Select(builder => builder.Invoke(buildContext)));
                return new Stack(
                    fit: StackFit.loose,
                    children: children
                );
            }
        }
    }
}