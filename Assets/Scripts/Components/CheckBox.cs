using System.Runtime.InteropServices;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public enum CheckBoxValue
    {
        Unchecked,
        Indeterminate,
        Checked
    }

    public partial class CheckBox : StatefulWidget
    {
        public CheckBox(
            Key key = null,
            CheckBoxValue value = CheckBoxValue.Unchecked,
            OnChange onChange = null
        ) : base(key)
        {
            this.value = value;
            this.onChange = onChange;
        }
        
        public delegate void OnChange(CheckBoxValue @new);

        private readonly CheckBoxValue value;
        
        private readonly OnChange onChange;

        public override State createState() => new CheckBoxState();
    }
}