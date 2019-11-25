using System;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public partial class MarkdownContent : StatefulWidget
    {
        private class NotifyContainer : StatefulWidget
        {
            public NotifyContainer(
                Key key = null,
                Alignment alignment = null,
                EdgeInsets padding = null,
                Color color = null,
                Decoration decoration = null,
                Decoration foregroundDecoration = null,
                float? width = null,
                float? height = null,
                BoxConstraints constraints = null,
                EdgeInsets margin = null,
                Matrix3 transform = null,
                Widget child = null,
                Action<Func<float>> notifyFn = null
            ) : base(key: key)
            {
                _alignment = alignment;
                _padding = padding;
                _color = color;
                _decoration = decoration;
                _foregroundDecoration = foregroundDecoration;
                _width = width;
                _height = height;
                _constraints = constraints;
                _margin = margin;
                _transform = transform;
                _child = child;
                _notifyFn = notifyFn;
            }

            private readonly Alignment _alignment;
            private readonly EdgeInsets _padding;
            private readonly Color _color;
            private readonly Decoration _decoration;
            private readonly Decoration _foregroundDecoration;
            private readonly float? _width;
            private readonly float? _height;
            private readonly BoxConstraints _constraints;
            private readonly EdgeInsets _margin;
            private readonly Matrix3 _transform;
            private readonly Widget _child;
            private readonly Action<Func<float>> _notifyFn;
            
            public override State createState() => new NotifyContainerState();
            
            private class NotifyContainerState : State<NotifyContainer>
            {
                public override void initState()
                {
                    base.initState();
                    updateNotifyFunction();
                }

                public override void didUpdateWidget(StatefulWidget oldWidget)
                {
                    base.didUpdateWidget(oldWidget);
                    updateNotifyFunction();
                }

                private void updateNotifyFunction()
                {
                    widget._notifyFn?.Invoke(() =>
                    {
                        var renderBox = context.findRenderObject() as RenderBox;
                        return renderBox.globalToLocal(Offset.zero).dy;
                    });
                }

                public override Widget build(BuildContext buildContext)
                {
                    return new Container(
                        alignment: widget._alignment,
                        padding: widget._padding,
                        color: widget._color,
                        decoration: widget._decoration,
                        forgroundDecoration: widget._foregroundDecoration,
                        width: widget._width,
                        height: widget._height,
                        constraints: widget._constraints,
                        margin: widget._margin,
                        transfrom: widget._transform,
                        child: widget._child
                    );
                }
            }
        }
    }
}