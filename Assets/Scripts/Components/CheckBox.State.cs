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
            private static readonly Color MAIN_COLOR = new Color(0xff3c83d4);
            private static readonly Color SUB_COLOR = new Color(0xffffffff);

            private bool hover;

            private void OnPointerEnter(PointerEnterEvent @event)
            {
                if (!mounted)
                {
                    return;
                }

                setState(() => hover = true);
                Bridge.ChangeCursor("pointer");
            }

            private void OnPointerLeave(PointerLeaveEvent @event)
            {
                if (!mounted)
                {
                    return;
                }

                setState(() => hover = false);
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
                            color: widget.value == CheckBoxValue.Checked ? SUB_COLOR : MAIN_COLOR
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
                                color: widget.value == CheckBoxValue.Checked ? MAIN_COLOR : SUB_COLOR,
                                border: Border.all(
                                    color: MAIN_COLOR,
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