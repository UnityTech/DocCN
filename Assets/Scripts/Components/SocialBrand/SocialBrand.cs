using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public partial class SocialBrand : StatefulWidget
    {
        public SocialBrand(IconData data, Color hoverColor)
        {
            _data = data;
            _hoverColor = hoverColor;
        }

        private readonly IconData _data;

        private readonly Color _hoverColor;
        public override State createState() => new SocialBrandState();
    }
}