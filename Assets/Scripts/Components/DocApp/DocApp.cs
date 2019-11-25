using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public class DocApp : StatefulWidget
    {
        public DocApp(Key key = null) : base(key)
        {
        }
        
        public static DocAppState of(BuildContext context)
        {
            var result =
                (DocAppState) context.ancestorStateOfType(new TypeMatcher<DocAppState>());
            return result;
        }

        public override State createState() => new DocAppState();
    }
}