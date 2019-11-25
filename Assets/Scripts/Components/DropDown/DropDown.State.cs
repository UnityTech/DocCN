using System;
using System.Linq;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace Unity.DocZh.Components
{
    public partial class DropDown<T>
    {
        public class DropDownState : State<DropDown<T>>
        {
            private PointerRoute _pointerRoute;

            private WidgetBuilder _dropDownOverlayBuilder;

            private OverlayEntry _overlayEntry;

            private Guid? _guid;

            private bool _expanded;

            private ScreenOverlay.ScreenOverlayState _screenOverlayState;

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
                                children: widget._items.Select(item => widget._itemBuilder.Invoke(this, item)).ToList()
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
                                children: widget._items.Select(item => widget._itemBuilder.Invoke(this, item)).ToList()
                            );
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                };
                _overlayEntry = new OverlayEntry(_dropDownOverlayBuilder);
                _expanded = false;
            }

            public override void dispose()
            {
                if (_expanded)
                {
                    Dismiss();
                }

                base.dispose();
            }

            public void Dismiss()
            {
                _screenOverlayState?.RemoveOnTapListener(OnScreenTap);
                switch (widget._overlayType)
                {
                    case DropDownOverlayType.scrollable:
                        if (_guid != null)
                        {
                            ScrollableOverlay.of(context).Remove(_guid.Value);
                        }

                        break;
                    case DropDownOverlayType.builtin:
                        _overlayEntry.remove();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _expanded = false;
            }

            private void OnScreenTap()
            {
                if (!_expanded)
                {
                    return;
                }
                Dismiss();
            }

            public override Widget build(BuildContext buildContext)
            {
                return new Clickable(
                    onTap: () =>
                    {
                        if (_expanded)
                        {
                            return;
                        }
                        _expanded = true;
                        switch (widget._overlayType)
                        {
                            case DropDownOverlayType.scrollable:
                                _guid = ScrollableOverlay.of(buildContext).Add(_dropDownOverlayBuilder);
                                break;
                            case DropDownOverlayType.builtin:
                                Overlay.of(buildContext).insert(_overlayEntry);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if (_screenOverlayState == null)
                        {
                            _screenOverlayState = ScreenOverlay.of(buildContext);
                        }
                        _screenOverlayState.AddOnTapListener(OnScreenTap);
                    },
                    child: widget._selectBuilder.Invoke()
                );
            }
        }
    }
}