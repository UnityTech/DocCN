using System.Runtime.InteropServices;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class CheckBox : StatefulWidget
    {
        public CheckBox(Key key = null) : base(key)
        {
        }

        public override State createState() => new CheckBoxState();
    }
}