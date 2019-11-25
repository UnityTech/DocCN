using System;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class SocialBrand : StatefulWidget
    {
        public SocialBrand(
            IconData data,
            Color hoverColor,
            Action<BuildContext> onTap)
        {
            _data = data;
            _hoverColor = hoverColor;
            _onTap = onTap;
        }

        private readonly IconData _data;

        private readonly Color _hoverColor;

        private readonly Action<BuildContext> _onTap;
        public override State createState() => new SocialBrandState();
    }
}