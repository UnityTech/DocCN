using System.Collections.Generic;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public class SelectableWealthyText : StatefulWidget
    {
        public SelectableWealthyText(List<TextSpan> textSpanList,
            Key key = null,
            TextStyle style = null,
            TextAlign? textAlign = null,
            bool? softWrap = null,
            TextOverflow? overflow = null,
            float? textScaleFactor = null,
            int? maxLines = null,
            FocusNode focusNode = null,
            Color selectionColor = null) : base(key)
        {
            this.textSpanList = textSpanList;
            this.data = null;
            this.style = style;
            this.textAlign = textAlign;
            this.softWrap = softWrap;
            this.overflow = overflow;
            this.textScaleFactor = textScaleFactor;
            this.maxLines = maxLines;
            this.focusNode = focusNode ?? new FocusNode();
            this.selectionColor = selectionColor;
        }

        public readonly string data;

        public readonly FocusNode focusNode;

        public readonly List<TextSpan> textSpanList;

        public readonly TextStyle style;

        public readonly TextAlign? textAlign;

        public readonly bool? softWrap;

        public readonly TextOverflow? overflow;

        public readonly float? textScaleFactor;

        public readonly int? maxLines;

        public readonly Color selectionColor;
        public override State createState() {
            return new SelectableWealthyTextState();
        }
    }
    
    internal class SelectableWealthyTextState : State<SelectableWealthyText>, WidgetsBindingObserver {
        readonly GlobalKey _richTextKey = GlobalKey.key();

        WealthyRenderParagraph _renderParagragh {
            get { return (WealthyRenderParagraph) this._richTextKey.currentContext.findRenderObject(); }
        }

        public override void initState() {
            base.initState();
            this.widget.focusNode.addListener(this._handleFocusChanged);
        }


        public override void didUpdateWidget(StatefulWidget old) {
            SelectableText oldWidget = (SelectableText) old;
            base.didUpdateWidget(oldWidget);

            if (oldWidget.focusNode != this.widget.focusNode) {
                oldWidget.focusNode.removeListener(this._handleFocusChanged);
                this.widget.focusNode.addListener(this._handleFocusChanged);
            }
        }

        public override void dispose() {
            this.widget.focusNode.removeListener(this._handleFocusChanged);
            base.dispose();
        }

        bool _hasFocus {
            get { return this.widget.focusNode.hasFocus; }
        }

        void _handleFocusChanged() {
            if (this._hasFocus) {
                WidgetsBinding.instance.addObserver(this);
                this._renderParagragh.hasFocus = true;
            }
            else {
                WidgetsBinding.instance.removeObserver(this);
                this._renderParagragh.hasFocus = false;
            }
        }


        public void didChangeMetrics() {
        }

        public void didChangeTextScaleFactor() {
        }
        
        public void didChangePlatformBrightness() {
        }
        
        public void didChangeLocales(List<Locale> locale) {
        }

        public IPromise<bool> didPopRoute() {
            return Promise<bool>.Resolved(false);
        }

        public IPromise<bool> didPushRoute(string route) {
            return Promise<bool>.Resolved(false);
        }

        void _handleTapDown(TapDownDetails details) {
        }

        void _handleSingleTapUp(TapUpDetails details) {
        }

        void _handleSingleTapCancel() {
        }

        void _handleLongPress() {
        }

        void _handleDragSelectionStart(DragStartDetails details)
        {
            this._renderParagragh.selectPositionAt(
                from: details.globalPosition,
                cause: SelectionChangedCause.drag);
        }

        void _handleDragSelectionUpdate(DragStartDetails startDetails,
            DragUpdateDetails updateDetails) {
            this._renderParagragh.selectPositionAt(
                from: startDetails.globalPosition,
                to: updateDetails.globalPosition,
                cause: SelectionChangedCause.drag);
        }

        public override Widget build(BuildContext context) {
            FocusScope.of(context).reparentIfNeeded(this.widget.focusNode);

            DefaultTextStyle defaultTextStyle = DefaultTextStyle.of(context);
            TextStyle effectiveTextStyle = this.widget.style;
            if (this.widget.style == null || this.widget.style.inherit) {
                effectiveTextStyle = defaultTextStyle.style.merge(this.widget.style);
            }

            Widget child = new WealthyText(
                key: this._richTextKey,
                textAlign: this.widget.textAlign ?? defaultTextStyle.textAlign ?? TextAlign.left,
                softWrap: this.widget.softWrap ?? defaultTextStyle.softWrap,
                overflow: this.widget.overflow ?? defaultTextStyle.overflow,
                textScaleFactor: this.widget.textScaleFactor ?? MediaQuery.textScaleFactorOf(context),
                maxLines: this.widget.maxLines ?? defaultTextStyle.maxLines,
                textSpanList: this.widget.textSpanList,
                style: effectiveTextStyle,
                onSelectionChanged: () => {
                    if (this._hasFocus) {
                        return;
                    }

                    FocusScope.of(this.context).requestFocus(this.widget.focusNode);
                },
                selectionColor: this.widget.selectionColor ?? Colors.blue);

            return new IgnorePointer(
                ignoring: false,
                child: new RichTextSelectionGestureDetector(
                    onTapDown: this._handleTapDown,
                    onSingleTapUp: this._handleSingleTapUp,
                    onSingleTapCancel: this._handleSingleTapCancel,
                    onSingleLongTapStart: this._handleLongPress,
                    onDragSelectionStart: this._handleDragSelectionStart,
                    onDragSelectionUpdate: this._handleDragSelectionUpdate,
                    behavior: HitTestBehavior.translucent,
                    child: child
                )
            );
        }
    }
}