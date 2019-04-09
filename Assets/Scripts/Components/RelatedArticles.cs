using System.Linq;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public class RelatedArticles : StatelessWidget
    {
        private static readonly string[] RELATED_ARTICLES = {"用于特定于特殊平台的覆盖样式的纹理的压缩格式", "审阅资源", "相关信息"};

        private static readonly Color TEXT_COLOR = new Color(0xff616161);

        private static Widget map(string text)
        {
            return new Container(
                margin: EdgeInsets.only(bottom: 16.0f),
                child: new Text(
                    text,
                    style: new TextStyle(
                        height: 1.5f,
                        color: TEXT_COLOR,
                        fontSize: 16.0f
                    )
                )
            );
        }

        public override Widget build(BuildContext context)
        {
            var children = RELATED_ARTICLES
                .Select(map)
                .ToList();
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: children
            );
        }
    }
}