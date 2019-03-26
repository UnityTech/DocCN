using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class DocApp : StatefulWidget
    {
        public DocApp(Key key = null) : base(key)
        {
        }

        public override State createState() => new DocAppState();
    }
}