using System;
using System.Linq;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace DocCN.Components
{
    public partial class DropDown<T>
    {
        private class DropDownState : State<DropDown<T>>
        {
            private PointerRoute _pointerRoute;

            private WidgetBuilder _dropDownOverlayBuilder;

            public override void initState()
            {
                base.initState();
                _dropDownOverlayBuilder = ctx =>
                {
                    var overlayRenderBox = ctx.findRenderObject() as RenderBox;
                    var renderBox = context.findRenderObject() as RenderBox;
                    switch (widget._direction)
                    {
                        case DropDownDirection.bottom:
                        {
                            var offset = renderBox.localToGlobal(
                                renderBox.size.bottomLeft(Offset.zero),
                                overlayRenderBox);
                            return new DropDownOverlay(
                                top: offset.dy,
                                left: offset.dx,
                                color: widget._overlayColor,
                                border: widget._overlayBorder,
                                children: widget._items.Select(item => widget._itemBuilder.Invoke(item)).ToList()
                            );
                        }
                        case DropDownDirection.top:
                        {
                            var offset = renderBox.localToGlobal(Offset.zero, overlayRenderBox);
                            return new DropDownOverlay(
                                left: offset.dx,
                                bottom: overlayRenderBox.size.height - offset.dy,
                                color: widget._overlayColor,
                                border: widget._overlayBorder,
                                children: widget._items.Select(item => widget._itemBuilder.Invoke(item)).ToList()
                            );
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                };
                _pointerRoute = evt =>
                {
                    if (evt is PointerDownEvent)
                    {
                        var renderBox = context.findRenderObject() as RenderBox;
                        if (!renderBox.paintBounds.contains(evt.position))
                        {
                            GestureBinding.instance.pointerRouter.removeGlobalRoute(_pointerRoute);
                            ScrollableOverlay.of(context).Remove(_dropDownOverlayBuilder);
                        }
                    }
                };
            }

            public override Widget build(BuildContext buildContext)
            {
                return new Clickable(
                    onTap: () =>
                    {
                        GestureBinding.instance.pointerRouter.addGlobalRoute(_pointerRoute);
                        ScrollableOverlay.of(buildContext).Add(_dropDownOverlayBuilder);
                    },
                    child: widget._selectBuilder.Invoke()
                );
            }
        }
    }
}