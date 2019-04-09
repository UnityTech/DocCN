using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public class ModifiedInfo : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                margin: EdgeInsets.only(bottom: 24.0f),
                child: new Text(
                    "最后编辑于2019.02.20（有限编辑审查）"
                )
            );
        }
    }
}