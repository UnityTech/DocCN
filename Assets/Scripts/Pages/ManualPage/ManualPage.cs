using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Pages
{
    public partial class ManualPage : StatefulWidget
    {
        public ManualPage(string title)
        {
            _title = title;
        }

        private readonly string _title;

        public override State createState()
        {
            return new ManualPageState();
        }
    }
}