using System;
using System.Collections.Generic;
using Unity.DocZh.Style;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace Unity.DocZh.Components
{
    public class WechatOfficialAccount : StatelessWidget
    {
        public WechatOfficialAccount(
            Action closeFn,
            float? left = null,
            float? top = null,
            float? right = null,
            float? bottom = null,
            Key key = null) : base(key)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
            _closeFn = closeFn;
        }

        private readonly float? _left;
        private readonly float? _top;
        private readonly float? _right;
        private readonly float? _bottom;
        private readonly Action _closeFn;

        public override Widget build(BuildContext context)
        {
            return new Positioned(
                left: _left,
                top: _top,
                right: _right,
                bottom: _bottom,
                child: new Stack(
                    children: new List<Widget>
                    {
                        new Container(
                            width: 256,
                            decoration: new BoxDecoration(
                                color: new Color(0x7f7f7f7f),
                                borderRadius: BorderRadius.all(4)
                            ),
                            padding: EdgeInsets.all(24f),
                            child: new Image(
                                image: new AssetImage("Images/official-account-qr-code")
                            )
                        ),
                        new Positioned(
                            right: 4,
                            top: 4,
                            child: new Clickable(
                                onTap: () => _closeFn.Invoke(),
                                child: new Icon(
                                    Icons.MaterialClose,
                                    size: 16
                                )
                            )
                        ),
                    }
                )
            );
        }
    }
}