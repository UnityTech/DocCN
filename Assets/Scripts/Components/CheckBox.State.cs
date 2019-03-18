using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class CheckBox
    {
        private class CheckBoxState : State<CheckBox>
        {
            private static readonly Color MAIN_COLOR = new Color(0xff3c83d4);
            public override Widget build(BuildContext context)
            {
                return new Container(
                    width: 16f,
                    height: 16f,
                    color: MAIN_COLOR
                );
            }
        }
    }
}