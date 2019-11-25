using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class MarkdownContent : StatefulWidget
    {
        public MarkdownContent(string title) : base(new ObjectKey(title))
        {
            _title = title;
        }

        public override State createState() => new MarkdownContentState();

        private readonly string _title;
    }
}