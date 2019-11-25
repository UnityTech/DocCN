using System;
using Unity.DocZh.Style;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Transform = Unity.UIWidgets.widgets.Transform;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public partial class Loading
    {
        private class LoadingState : SingleTickerProviderStateMixin<Loading>
        {
            private const float AnimationStart = Mathf.PI / 4;
            private const float AnimationEnd = Mathf.PI * 2 + AnimationStart;
            private static readonly Color IconColor = new Color(0xffd8d8d8);

            private AnimationController _controller;
            private Animation<float> _animation;

            public override void initState()
            {
                base.initState();
                _controller = new AnimationController(
                    duration: new TimeSpan(0, 0, 0, 0, 720),
                    vsync: this
                );
                _animation =
                    _controller.drive(
                        new FloatTween(AnimationStart, AnimationEnd).chain(new CurveTween(Curves.easeInOut)));
                _controller.addListener(() => setState(() => { }));
                _controller.repeat();
            }

            public override void dispose()
            {
                _controller.dispose();
                base.dispose();
            }

            public override Widget build(BuildContext context)
            {
                return Transform.rotate(
                    degree: _animation.value,
                    alignment: Alignment.center,
                    child: new Icon(
                        Icons.IconFontLoading,
                        size: widget._size,
                        color: IconColor
                    )
                );
            }
        }
    }
}