using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public partial class ScriptingContent : StatefulWidget
    {
        public ScriptingContent(string title)
        {
            _title = title;
        }
        private readonly string _title;
        public override State createState() => new ScriptingContentState();
    }
}