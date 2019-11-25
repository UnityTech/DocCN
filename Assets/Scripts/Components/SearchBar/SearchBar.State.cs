using System;
using System.Collections.Generic;
using Unity.DocZh.Utility;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace Unity.DocZh.Components
{
    public partial class SearchBar : StatefulWidget
    {
        private class SearchBarState : State<SearchBar>
        {
            private FilterType _filterType;
            private TextEditingController _textEditingController;
            private FocusNode _focusNode;
            private ScreenOverlay.ScreenOverlayState _screenOverlayState;

            public override void initState()
            {
                base.initState();
                _filterType = FilterType.manual;
                _textEditingController = new TextEditingController();
                _focusNode = new FocusNode();
                _screenOverlayState = ScreenOverlay.of(context);
                _screenOverlayState.AddOnTapListener(LoseFocus);
            }

            public override void dispose()
            {
                _focusNode.dispose();
                _textEditingController.dispose();
                _screenOverlayState.RemoveOnTapListener(LoseFocus);
                base.dispose();
            }

            private void GoToSearch()
            {
                _focusNode.unfocus();
                var encodedKeyword = Uri.EscapeDataString(_textEditingController.value.text);
                LocationUtil.Go($"/Search/{_filterType}/{encodedKeyword}");
            }

            private void LoseFocus()
            {
                _focusNode.unfocus();
            }

            public override Widget build(BuildContext buildContext)
            {
                var stylePack = widget._style.StylePack();
                var row = new Container(
                    height: 56,
                    child: new Row(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: new List<Widget>
                        {
                            new DropDown<FilterType>(
                                overlayType: widget._filterDropDownOverlayType,
                                items: new[] {FilterType.manual, FilterType.scripting},
                                itemBuilder: (state, item) => new FilterItem(
                                    onTap: () =>
                                    {
                                        state.Dismiss();
                                        if (mounted)
                                        {
                                            setState(() => _filterType = item);
                                        }
                                    },
                                    text: item.Text(),
                                    stylePack: widget._style.FilterItemStylePack()
                                ),
                                selectBuilder: () => new Container(
                                    width: 170.0f,
                                    decoration: stylePack.filterDecoration,
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        children: new List<Widget>
                                        {
                                            new Text(
                                                "筛选：",
                                                style: new TextStyle(
                                                    color: stylePack.filterStrongColor,
                                                    fontSize: 16f
                                                )
                                            ),
                                            new Text(
                                                _filterType.Text(),
                                                style: new TextStyle(
                                                    color: stylePack.filterTextColor,
                                                    fontSize: 16f
                                                )
                                            ),
                                            new Icon(
                                                Style.Icons.MaterialArrowDropDown,
                                                color: stylePack.filterTextColor,
                                                size: 24f
                                            )
                                        }
                                    )
                                ),
                                overlayBorder: stylePack.filterItemsBorder
                            ),
                            new Expanded(
                                child: new GestureDetector(
                                    onTap: () => FocusScope.of(buildContext).requestFocus(_focusNode),
                                    child: new Listener(
                                        onPointerEnter: evt => Bridge.ChangeCursor("text"),
                                        onPointerExit: evt => Bridge.ChangeCursor("default"),
                                        child: new Container(
                                            color: stylePack.searchInputBackgroundColor,
                                            padding: EdgeInsets.symmetric(horizontal: 24f),
                                            child: new Center(
                                                child: new EditableText(
                                                    controller: _textEditingController,
                                                    focusNode: _focusNode,
                                                    selectionColor: stylePack.searchInputSelectionColor,
                                                    style: new TextStyle(
                                                        color: stylePack.searchInputColor,
                                                        fontSize: 16f,
                                                        fontFamily: "PingFang"
                                                    ),
                                                    cursorColor: stylePack.searchInputColor,
                                                    onEditingComplete: GoToSearch
                                                )
                                            )
                                        )
                                    )
                                )
                            ),
                            new Clickable(
                                onTap: GoToSearch,
                                child: new Container(
                                    width: 56.0f,
                                    color: stylePack.searchIconBackgroundColor,
                                    child: new Center(
                                        child: new Icon(
                                            Style.Icons.MaterialSearch,
                                            color: new Color(0xffd8d8d8)
                                        )
                                    )
                                )
                            )
                        }
                    )
                );
                if (widget._style == SearchBarStyle.embed)
                {
                    return row;
                }

                return new Container(
                    height: Height,
                    padding: EdgeInsets.only(top: 8.0f, right: 48.0f, bottom: 8.0f, left: 48.0f),
                    color: new Color(0xff212121),
                    child: row
                );
            }
        }
    }
}