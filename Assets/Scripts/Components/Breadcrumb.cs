using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public class Breadcrumb : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                margin: EdgeInsets.only(bottom: 12.0f),
                child: new RichText(
                    text: new TextSpan(
                        "Unity用户手册（2018.3） / 导入",
                        style: new TextStyle(
                            fontFamily: "PingFang SC"
                        )
                    )
                )
            );
        }
    }
}