using Unity.UIWidgets.widgets;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using System;

namespace Unity.DocZh.Components
{
    public partial class Scroller
    {
        public class ScrollerState : TickerProviderStateMixin<Scroller>
        {
            private static readonly Color scrollbarColor = new Color(0x66000000);
            private static readonly float scrollbarMinLength = 36;
            private static readonly float scrollbarMinOverscrollLength = 8;
            private static readonly Radius scrollbarRadius = Radius.circular(4f);
            private static readonly TimeSpan scrollbarTimeToFade = TimeSpan.FromMilliseconds(50);
            private static readonly TimeSpan scrollbarFadeDuration = TimeSpan.FromMilliseconds(250);

            static readonly float scrollbarThickness = 8f;
            static readonly float scrollbarMainAxisMargin = 3;
            static readonly float scrollbarCrossAxisMargin = 3;

            private ScrollbarPainter _scrollbarPainter;
            private AnimationController _fadeoutAnimationController;
            private Animation<float> _fadeoutOpacityAnimation;
            private Timer _fadeoutTimer;

            public override void initState()
            {
                base.initState();
                _fadeoutAnimationController = new AnimationController(
                    vsync: this,
                    duration: scrollbarFadeDuration
                );
                _fadeoutOpacityAnimation = new CurvedAnimation(
                    parent: this._fadeoutAnimationController,
                    curve: Curves.fastOutSlowIn
                );
            }

            public override void didChangeDependencies()
            {
                base.didChangeDependencies();
                _scrollbarPainter = new ScrollbarPainter(
                    scrollbarColor,
                    textDirection: TextDirection.ltr,
                    scrollbarThickness,
                    fadeoutOpacityAnimation: this._fadeoutOpacityAnimation,
                    scrollbarMainAxisMargin,
                    scrollbarCrossAxisMargin,
                    scrollbarRadius,
                    scrollbarMinLength,
                    scrollbarMinOverscrollLength
                );
            }

            public override void dispose()
            {
                _fadeoutAnimationController.dispose();
                _fadeoutTimer?.cancel();
                _scrollbarPainter?.dispose();
                base.dispose();
            }

            bool _handleScrollNotification(ScrollNotification notification)
            {
                if (notification is ScrollUpdateNotification || notification is OverscrollNotification)
                {
                    if (this._fadeoutAnimationController.status != AnimationStatus.forward)
                    {
                        this._fadeoutAnimationController.forward();
                    }

                    this._fadeoutTimer?.cancel();
                    this._scrollbarPainter.update(
                        metrics: notification.metrics,
                        axisDirection: notification.metrics.axisDirection
                    );
                }
                else if (notification is ScrollEndNotification)
                {
                    this._fadeoutTimer?.cancel();
                    this._fadeoutTimer = Window.instance.run(scrollbarTimeToFade, () => {
                        this._fadeoutAnimationController.reverse();
                        this._fadeoutTimer = null;
                    });
                }

                return false;
            }

            public override Widget build(BuildContext context)
            {
                return new NotificationListener<ScrollNotification>(
                    onNotification: this._handleScrollNotification,
                    child: new RepaintBoundary(
                        child: new CustomPaint(
                            foregroundPainter: this._scrollbarPainter,
                            child: new RepaintBoundary(
                                child: this.widget.child
                            )
                        )
                    )
                );
            }
        }
    }
}
