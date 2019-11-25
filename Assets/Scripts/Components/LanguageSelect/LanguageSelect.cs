using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class LanguageSelect : StatefulWidget
    {
        public LanguageSelect(
            Color selectTextColor = null,
            Color underlineColor = null,
            Color expandIconColor = null,
            Key key = null) : base(key)
        {
            _selectTextColor = selectTextColor;
            _underlineColor = underlineColor;
            _expandIconColor = expandIconColor;
        }

        private readonly Color _selectTextColor;
        private readonly Color _underlineColor;
        private readonly Color _expandIconColor;
        public override State createState() => new LanguageSelectState();
    }
}