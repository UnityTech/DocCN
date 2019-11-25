using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class ImageWithPlaceholder : StatefulWidget
    {
        public ImageWithPlaceholder(
            float width,
            float height,
            ImageProvider imageProvider,
            Key key = null) : base(key)
        {
            _width = width;
            _height = height;
            _imageProvider = imageProvider;
        }

        private readonly float _width;
        private readonly float _height;
        private readonly ImageProvider _imageProvider;
        public override State createState() => new ImageWithPlaceholderState();
    }
}