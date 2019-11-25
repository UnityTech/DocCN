using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Pages
{
    public partial class ScriptingPage : StatefulWidget
    {
        public ScriptingPage(string title)
        {
            _title = title;
        }

        private readonly string _title;

        public override State createState() => new ScriptingPageState();
    }
}