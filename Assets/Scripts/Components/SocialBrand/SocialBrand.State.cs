using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public partial class SocialBrand
    {
        private class SocialBrandState : State<SocialBrand>
        {
            private static readonly Color NormalColor = new Color(0xff908f90);

            private bool _hover;

            public override void initState()
            {
                base.initState();
                _hover = false;
            }

            public override Widget build(BuildContext context)
            {
                return new Clickable(
                    hoverChanged: hover => setState(() => _hover = hover),
                    child: new Container(
                        margin: EdgeInsets.only(right: 20.0f),
                        child: new Icon(
                            widget._data,
                            color: _hover ? widget._hoverColor : NormalColor
                        )
                    )
                );
            }
        }
    }
}