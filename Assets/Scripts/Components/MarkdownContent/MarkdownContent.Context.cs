using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class MarkdownContent
    {
        private class BuilderContext
        {
            public TextStyle textStyle { get; set; }
            public Stack<TextSpan> inline { get; }
            public Container imageNode { get; set; }
            public List<List<Container>> rows { get; private set; }
            public List<Container> cells { get; private set; }
            public string title { get; set; }
            public List<PositionRecord> positionRecords { get; }
            public bool useNotifyContainer { get; set; }

            public BuilderContext()
            {
                inline = new Stack<TextSpan>();
                rows = new List<List<Container>>();
                cells = new List<Container>();
                positionRecords = new List<PositionRecord>();
                useNotifyContainer = false;
            }

            public void Clear()
            {
                textStyle = null;
                inline.Clear();
                imageNode = null;
                useNotifyContainer = false;
            }

            public void ClearColumn()
            {
                rows = new List<List<Container>>();
            }

            public void ClearCells()
            {
                cells = new List<Container>();
            }
        }
    }
}