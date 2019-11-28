using System;
using System.Collections.Generic;
using Unity.DocZh.Utility;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using Icons = Unity.DocZh.Style.Icons;
using ImageUtils = Unity.UIWidgets.painting.ImageUtils;
using Rect = Unity.UIWidgets.ui.Rect;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.DocZh.Components
{
    // RichText is not so rich
    public class WealthyText : Text
    {
        public WealthyText(
            List<TextSpan> textSpanList,
            Key key = null,
            TextStyle style = null,
            TextAlign? textAlign = TextAlign.left,
            bool? softWrap = true,
            TextOverflow? overflow = TextOverflow.clip,
            float? textScaleFactor = 1.0f,
            int? maxLines = null,
            Action onSelectionChanged = null,
            Color selectionColor = null) :
            base(string.Empty,
                key,
                style,
                textAlign: textAlign,
                softWrap: softWrap,
                overflow: overflow,
                textScaleFactor: textScaleFactor,
                maxLines: maxLines)
        {
            _textSpanList = textSpanList;
            _onSelectionChanged = onSelectionChanged;
            _selectionColor = selectionColor;
        }

        private readonly List<TextSpan> _textSpanList;
        private Action _onSelectionChanged;
        private Color _selectionColor;

        public override Widget build(BuildContext context)
        {
            var defaultTextStyle = DefaultTextStyle.of(context);
            var effectiveTextStyle = style;
            if (style == null || style.inherit)
            {
                effectiveTextStyle = defaultTextStyle.style.merge(style);
            }

            if (MediaQuery.boldTextOverride(context))
            {
                effectiveTextStyle = effectiveTextStyle.merge(new TextStyle(fontWeight: FontWeight.w700));
            }

            var textSpan = new TextSpan(
                "",
                style: effectiveTextStyle,
                children: _textSpanList
            );

            textSpan.children.ForEach(child =>
            {
                if (child is ImageSpan imageSpan)
                {
                    imageSpan.UpdateImageConfiguration(context);
                }
            });

            var result = new WealthyTextWrapper(
                textAlign: textAlign ?? defaultTextStyle.textAlign ?? TextAlign.left,
                softWrap: softWrap ?? defaultTextStyle.softWrap,
                overflow: overflow ?? defaultTextStyle.overflow,
                textScaleFactor: textScaleFactor ?? MediaQuery.textScaleFactorOf(context),
                maxLines: maxLines ?? defaultTextStyle.maxLines,
                text: textSpan,
                onSelectionChanged: _onSelectionChanged,
                selectionColor: _selectionColor
            );

            return result;
        }
    }

    public class ImageSpan : TextSpan
    {
        private readonly float _imageWidth;
        private readonly float _imageHeight;
        private readonly EdgeInsets _margin;
        private readonly ImageProvider _imageProvider;

        public ImageSpan(
            ImageProvider imageProvider,
            float imageWidth = 14f,
            float imageHeight = 14f,
            EdgeInsets margin = null,
            GestureRecognizer recognizer = null
        ) : base(
            style: new TextStyle(
                color: Colors.transparent,
                // letterSpacing: imageWidth + (margin == null ? 0 : margin.horizontal),
                height: 1f.LineHeight(),
                fontSize: imageHeight + (margin == null ? 0 : margin.vertical)
            ),
            text: "X",
            // text: "\u200b",
            children: null
            //recognizer: recognizer
        )
        {
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
            _margin = margin;
            _imageProvider = imageProvider;
            imageResolver = new ImageResolver(imageProvider);
        }

        public void UpdateImageConfiguration(BuildContext context)
        {
            imageResolver.UpdateImageConfiguration(context, _imageWidth, _imageHeight);
        }

        public float width => _imageWidth + (_margin == null ? 0 : _margin.horizontal);

        public float height => _imageHeight + (_margin == null ? 0 : _margin.vertical);

        public float innerHeight => _imageHeight;

        public float innerWidth => _imageWidth;

        public EdgeInsets margin => _margin;

        internal ImageResolver imageResolver { get; }
    }

    internal class ImageResolver
    {
        private readonly ImageProvider _imageProvider;
        private ImageStream _imageStream;
        private Action<ImageInfo, bool> _listener;
        private ImageConfiguration _imageConfiguration;
        public Unity.UIWidgets.ui.Image image { get; private set; }

        public ImageResolver(ImageProvider imageProvider)
        {
            _imageProvider = imageProvider;
        }

        public void UpdateImageConfiguration(BuildContext context, float width, float height)
        {
            _imageConfiguration = Unity.UIWidgets.widgets.ImageUtils.createLocalImageConfiguration(
                context,
                size: new Size(width, height)
            );
        }

        internal void Resolve(Action<ImageInfo, bool> listener)
        {
            var oldImageStream = _imageStream;
            _imageStream = _imageProvider.resolve(_imageConfiguration);
            _listener = listener;
            if (_imageStream.key == oldImageStream?.key)
            {
                return;
            }

            oldImageStream?.removeListener(HandleImageChanged);
            _imageStream.addListener(HandleImageChanged);
        }

        private void HandleImageChanged(ImageInfo imageInfo, bool synchronousCall)
        {
            image = imageInfo.image;
            _listener?.Invoke(imageInfo, synchronousCall);
        }

        public void StopListening()
        {
            _imageStream?.removeListener(HandleImageChanged);
        }
    }

    internal class WealthyTextWrapper : RichText
    {
        public WealthyTextWrapper(
            Key key = null,
            TextSpan text = null,
            TextAlign textAlign = TextAlign.left,
            bool softWrap = true,
            TextOverflow overflow = TextOverflow.clip,
            float textScaleFactor = 1F,
            int? maxLines = null,
            Action onSelectionChanged = null,
            Color selectionColor = null
        ) : base(
            key: key,
            text: text,
            textAlign: textAlign,
            softWrap: softWrap,
            overflow: overflow,
            textScaleFactor: textScaleFactor,
            maxLines: maxLines,
            onSelectionChanged: onSelectionChanged,
            selectionColor: selectionColor
        )
        {
        }

        public override RenderObject createRenderObject(BuildContext context)
        {
            return new WealthyRenderParagraph(
                text,
                textAlign: textAlign,
                softWrap: softWrap,
                overflow: overflow,
                textScaleFactor: textScaleFactor,
                maxLines: maxLines,
                onSelectionChanged: onSelectionChanged,
                selectionColor: selectionColor
            );
        }
    }

    internal class WealthyRenderParagraph : RenderParagraph
    {
        public WealthyRenderParagraph(
            TextSpan text,
            TextAlign textAlign = TextAlign.left,
            TextDirection textDirection = TextDirection.ltr,
            bool softWrap = true,
            TextOverflow overflow = TextOverflow.clip,
            float textScaleFactor = 1,
            int? maxLines = null,
            Action onSelectionChanged = null,
            Color selectionColor = null) : base(
            text: text,
            textAlign: textAlign,
            textDirection: textDirection,
            softWrap: softWrap,
            overflow: overflow,
            textScaleFactor: textScaleFactor,
            maxLines: maxLines,
            onSelectionChanged: onSelectionChanged,
            selectionColor: selectionColor)
        {
        }

        private static readonly Paint ImagePlaceholderPaint;
        private static readonly Paint ImagePlaceholderBackgroundPaint;

        static WealthyRenderParagraph()
        {
            ImagePlaceholderPaint = new Paint {color = new Color(0xffffffff)};
            ImagePlaceholderBackgroundPaint = new Paint {color = new Color(0xffefefef)};
        }

        public override void paint(PaintingContext context, Offset offset)
        {
            base.paint(context, offset);
            paintImageSpan(context, offset);
        }

        public override void detach()
        {
            base.detach();
            text.children.ForEach(span =>
            {
                if (span is ImageSpan imageSpan)
                {
                    imageSpan.imageResolver.StopListening();
                }
            });
        }

        private void paintImageSpan(PaintingContext context, Offset offset)
        {
            var canvas = context.canvas;
            var bounds = offset & size;
            canvas.save();
            var textOffset = 0;
            foreach (var span in text.children)
            {
                if (span is ImageSpan imageSpan)
                {
                    var offsetForCaret = getOffsetForCaret(
                        new TextPosition(offset: textOffset),
                        bounds
                    );
                    if (textOffset != 0 &&
                        offsetForCaret.dx == 0F &&
                        offsetForCaret.dy == 0F)
                    {
                        canvas.restore();
                        return;
                    }

                    var topLeftOffset = new Offset(
                        offset.dx + offsetForCaret.dx - (textOffset == 0 ? 0 : 0),
                        offset.dy + offsetForCaret.dy
                    );
                    if (imageSpan.imageResolver.image == null)
                    {
                        canvas.drawRect(
                            rect: Rect.fromLTWH(
                                topLeftOffset.dx + imageSpan.margin.left,
                                topLeftOffset.dy + imageSpan.margin.top,
                                imageSpan.innerWidth,
                                imageSpan.innerHeight
                            ),
                            paint: ImagePlaceholderBackgroundPaint);
                        var textBlobBuilder = new TextBlobBuilder();
                        var text = $"{(char) Style.Icons.MaterialImage.codePoint}";
                        var iconSize = imageSpan.innerWidth > imageSpan.innerHeight * 3
                            ? imageSpan.innerHeight * 3 / 4
                            : imageSpan.innerWidth / 4;
                        textBlobBuilder.allocRunPos(
                            style: new TextStyle(
                                fontFamily: Style.Icons.MaterialImage.fontFamily,
                                fontSize: iconSize
                            ),
                            text: text,
                            offset: 0,
                            size: text.Length);
                        var rect = topLeftOffset & new Size(imageSpan.innerWidth, imageSpan.innerHeight);
                        textBlobBuilder.setBounds(rect.toRect());
                        canvas.drawTextBlob(
                            textBlobBuilder.make(),
                            new Offset(
                                rect.left + (rect.width - iconSize) / 2,
                                rect.top + (rect.height + iconSize) / 2),
                            ImagePlaceholderPaint);

                        imageSpan.imageResolver.Resolve((imageInfo, synchronousCall) =>
                        {
                            if (synchronousCall)
                            {
                                ImageUtils.paintImage(
                                    canvas: canvas,
                                    rect: topLeftOffset & new Size(imageSpan.width, imageSpan.height),
                                    image: imageSpan.imageResolver.image,
                                    fit: BoxFit.scaleDown,
                                    alignment: Alignment.center
                                );
                            }
                            else
                            {
                                if (owner == null || !owner.debugDoingPaint)
                                {
                                    markNeedsPaint();
                                }
                            }
                        });
                        textOffset += imageSpan.toPlainText().Length;

                        continue;
                    }

                    ImageUtils.paintImage(
                        canvas: canvas,
                        rect: Rect.fromLTWH(
                            topLeftOffset.dx + imageSpan.margin.left,
                            topLeftOffset.dy + imageSpan.margin.top,
                            imageSpan.innerWidth,
                            imageSpan.innerHeight
                        ),
                        image: imageSpan.imageResolver.image,
                        fit: BoxFit.scaleDown,
                        alignment: Alignment.center
                    );
                }

                textOffset += span.toPlainText().Length;
            }

            canvas.restore();
        }
    }
}