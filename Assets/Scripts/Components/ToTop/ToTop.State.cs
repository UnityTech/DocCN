using System;
using System.Collections.Generic;
using Unity.DocZh.Style;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace Unity.DocZh.Components
{
    public partial class ToTop
    {
        private static readonly TimeSpan SuitableAnimationTimeSpan = new TimeSpan(0, 0, 0, 0, 240);
        private class ToTopState : SingleTickerProviderStateMixin<ToTop>
        {
            
            
            private bool _display;
            private AnimationController _animationController;
            private Animation<float> _animation;

            public override void initState()
            {
                base.initState();
                if (widget._displayThreshold != null)
                {
                    widget._scrollController.addListener(OnHostScroll);
                }

                _display = widget._displayThreshold == null ||
                           widget._scrollController.offset > widget._displayThreshold;
                _animationController = new AnimationController(
                    duration: SuitableAnimationTimeSpan,
                    vsync: this
                );
                _animation = _animationController.drive(new FloatTween(0, 1).chain(new CurveTween(Curves.easeIn)));
                _animationController.addListener(UpdateFrame);
            }

            private void UpdateFrame()
            {
                if (mounted)
                {
                    setState(() => { });
                }
            }

            public override void dispose()
            {
                widget._scrollController.removeListener(OnHostScroll);
                _animationController.removeListener(UpdateFrame);
                _animationController.dispose();
                base.dispose();
            }

            public override void didUpdateWidget(StatefulWidget oldWidget)
            {
                var old = (ToTop) oldWidget;
                old._scrollController.removeListener(OnHostScroll);
                base.didUpdateWidget(oldWidget);
                widget._scrollController.addListener(OnHostScroll);
            }

            private void OnHostScroll()
            {
                if (_display &&
                    widget._scrollController.offset < widget._displayThreshold)
                {
                    _display = false;
                    _animationController.reverse();
                }
                else if (!_display &&
                         widget._scrollController.offset > widget._displayThreshold)
                {
                    _display = true;
                    _animationController.forward();
                }
            }

            public override Widget build(BuildContext context)
            {
                Widget child = new Container(
                    decoration: new BoxDecoration(
                        boxShadow: new List<BoxShadow>
                        {
                            new BoxShadow(
                                color: new Color(0x66000000),
                                offset: new Offset(0, 2f),
                                blurRadius: 4f
                            )
                        },
                        borderRadius: BorderRadius.all(32),
                        color: new Color(0xff000000)
                    ),
                    height: 64,
                    width: 64,
                    child: new Center(
                        child: Transform.rotate(
                            degree: Mathf.PI,
                            alignment: Alignment.center,
                            child: new Icon(
                                Icons.MaterialExpandMore,
                                color: new Color(0xffffffff),
                                size: 32f
                            )
                        )
                    )
                );
                if (widget._displayThreshold != null)
                {
                    if (Math.Abs(_animation.value) < 1e-2)
                    {
                        child = null;
                    }
                    else
                    {
                        child = new Opacity(
                            opacity: _animation.value,
                            child: child
                        );
                    }
                }

                return new Positioned(
                    bottom: 140,
                    right: 48,
                    child: new Clickable(
                        onTap: () => widget._scrollController.animateTo(
                            0, 
                            SuitableAnimationTimeSpan, 
                            Curves.easeIn),
                        child: child
                    )
                );
            }
        }
    }
}