using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public partial class MarkdownContent : StatefulWidget
    {
        public MarkdownContent(string title) : base(new ObjectKey("markdown-content"))
        {
            _title = title;
        }

        public override State createState() => new MarkdownContentState();

        private readonly string _title;
    }
}