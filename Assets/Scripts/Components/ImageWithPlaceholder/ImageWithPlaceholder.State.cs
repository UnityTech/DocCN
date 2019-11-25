using Unity.DocZh.Style;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public partial class ImageWithPlaceholder
    {
        private class ImageWithPlaceholderState : State<ImageWithPlaceholder>
        {
            public override void initState()
            {
                base.initState();
                _image = null;
                widget._imageProvider
                    .resolve(new ImageConfiguration())
                    .addListener(ImageLoaded);
            }

            private void ImageLoaded(ImageInfo image, bool synchronousCall)
            {
                if (mounted)
                {
                    setState(() => { _image = image.image; });
                }
            }

            private Unity.UIWidgets.ui.Image _image;

            private static readonly Color PlaceholderBackgroundColor = new Color(0xffefefef);
            private static readonly Color PlaceholderIconColor = new Color(0xffffffff);

            public override Widget build(BuildContext buildContext)
            {
                Widget child = null;
                if (_image == null)
                {
                    child = new Center(
                        child: new Icon(
                            Icons.MaterialImage,
                            color: PlaceholderIconColor,
                            size: Mathf.Min(96f, Mathf.Min(widget._width, widget._height) / 3)
                        )
                    );
                }
                else
                {
                    child = new RawImage(image: _image);
                }

                return new Container(
                    width: widget._width,
                    height: widget._height,
                    color: PlaceholderBackgroundColor,
                    child: child
                );
            }
        }
    }
}