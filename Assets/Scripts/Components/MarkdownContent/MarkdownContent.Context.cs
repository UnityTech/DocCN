using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Models.Json;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class MarkdownContent
    {
        private class BuilderContext
        {
            public BuildContext Context { get; }
            public TextStyle TextStyle { get; set; }
            public Stack<TextSpan> Inline { get; }
            public Container ImageNode { get; set; }
            public List<List<Container>> Rows { get; private set; }
            public List<Container> Cells { get; private set; }
            public string Title { get; set; }
            public List<PositionRecord> PositionRecords { get; }
            public bool UseNotifyContainer { get; set; }
            public Dictionary<string, ImageMeta> ImageMetas { get; }
            public List<TapGestureRecognizer> SpanRecognizers { get; }
            public bool UseRecognizer { get; set; }
            public bool ListItem { get; set; }

            public BuilderContext(
                IEnumerable<ImageMeta> imageMetas,
                BuildContext context)
            {
                Inline = new Stack<TextSpan>();
                Rows = new List<List<Container>>();
                Cells = new List<Container>();
                PositionRecords = new List<PositionRecord>();
                UseNotifyContainer = false;
                this.ImageMetas = imageMetas?
                    .GroupBy(item => item.name)
                    .Select(group => group.ToArray().First())
                    .ToDictionary(
                    meta => meta.name,
                    meta => meta);
                SpanRecognizers = new List<TapGestureRecognizer>();
                UseRecognizer = false;
                ListItem = false;
                this.Context = context;
            }

            public void Clear()
            {
                TextStyle = null;
                Inline.Clear();
                ImageNode = null;
                UseNotifyContainer = false;
            }

            public void ClearColumn()
            {
                Rows = new List<List<Container>>();
            }

            public void ClearCells()
            {
                Cells = new List<Container>();
            }
        }
    }
}