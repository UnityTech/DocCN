using DocCN.Style;
using DocCN.Utility;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace DocCN.Components
{
    public partial class CheckBox
    {
        private class CheckBoxState : State<CheckBox>
        {
            private static readonly Color MainColor = new Color(0xff3c83d4);
            private static readonly Color SubColor = new Color(0xffffffff);

            private bool _hover;

            private void OnPointerEnter(PointerEnterEvent @event)
            {
                if (!mounted)
                {
                    return;
                }

                setState(() => _hover = true);
                Bridge.ChangeCursor("pointer");
            }

            private void OnPointerLeave(PointerLeaveEvent @event)
            {
                if (!mounted)
                {
                    return;
                }

                setState(() => _hover = false);
                Bridge.ChangeCursor("default");
            }

            private void OnTap()
            {
                var @new = widget.value == CheckBoxValue.Checked ? CheckBoxValue.Unchecked : CheckBoxValue.Checked;
                widget.onChange?.Invoke(@new);
            }

            public override Widget build(BuildContext context)
            {
                Widget inner = null;
                if (widget.value != CheckBoxValue.Unchecked)
                {
                    inner = new Center(
                        child: new Icon(
                            Icons.MaterialDone,
                            size: 12f,
                            color: widget.value == CheckBoxValue.Checked ? SubColor : MainColor
                        )
                    );
                }

                return new Listener(
                    onPointerEnter: OnPointerEnter,
                    onPointerLeave: OnPointerLeave,
                    child: new GestureDetector(
                        onTap: OnTap,
                        child: new Container(
                            width: 16f,
                            height: 16f,
                            decoration: new BoxDecoration(
                                color: widget.value == CheckBoxValue.Checked ? MainColor : SubColor,
                                border: Border.all(
                                    color: MainColor,
                                    width: 1f
                                )
                            ),
                            child: inner
                        )
                    )
                );
            }
        }
    }
}