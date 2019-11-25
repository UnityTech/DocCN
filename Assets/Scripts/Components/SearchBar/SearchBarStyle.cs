using System;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;

namespace Unity.DocZh.Components
{
    public enum SearchBarStyle
    {
        normal,
        embed,
    }

    internal class FilterItemStylePack
    {
        internal Color normalColor { get; private set; }
        internal Color hoverColor { get; private set; }
        internal TextStyle normalStyle { get; private set; }

        internal static readonly FilterItemStylePack Normal = new FilterItemStylePack
        {
            normalColor = new Color(0xff212121),
            hoverColor = new Color(0xff204f74),
            normalStyle = new TextStyle(
                color: new Color(0xffffffff),
                fontSize: 16f,
                fontFamily: "PingFang"
            ),
        };

        internal static readonly FilterItemStylePack Embed = new FilterItemStylePack
        {
            normalColor = new Color(0xffffffff),
            hoverColor = new Color(0xffd8d8d8),
            normalStyle = new TextStyle(
                color: new Color(0xff212121),
                fontSize: 16f,
                fontFamily: "PingFang"
            )
        };
    }

    internal class SearchBarStylePack
    {
        internal BoxDecoration filterDecoration { get; private set; }
        internal Color searchInputBackgroundColor { get; private set; }
        internal Color searchInputColor { get; private set; }
        
        internal Color searchInputSelectionColor { get; private set; }
        internal Color searchIconBackgroundColor { get; private set; }
        internal Color filterStrongColor { get; private set; }
        internal Color filterTextColor { get; private set; }
        internal Border filterItemsBorder { get; private set; }

        private static readonly BorderSide FilterBorderSide =
            new BorderSide(width: 2.0f, color: new Color(0xff424242));

        internal static readonly SearchBarStylePack Normal = new SearchBarStylePack
        {
            filterDecoration = new BoxDecoration(
                border: new Border(
                    top: FilterBorderSide,
                    left: FilterBorderSide,
                    bottom: FilterBorderSide
                )
            ),
            searchInputBackgroundColor = new Color(0xff424242),
            searchInputColor = new Color(0xffffffff),
            searchInputSelectionColor = new Color(0xffaad1ff),
            searchIconBackgroundColor = new Color(0xff565656),
            filterStrongColor = new Color(0xffd8d8d8),
            filterTextColor = new Color(0xffffffff),
            filterItemsBorder = null,
        };

        internal static readonly SearchBarStylePack Embed = new SearchBarStylePack
        {
            filterDecoration = new BoxDecoration(
                color: new Color(0xffffffff),
                border: new Border(
                    right: new BorderSide(
                        color: new Color(0xff979797),
                        width: 1f
                    )
                )
            ),
            searchInputBackgroundColor = new Color(0xffffffff),
            searchInputColor = new Color(0xff212121),
            searchInputSelectionColor = new Color(0xffaad1ff),
            searchIconBackgroundColor = new Color(0xffefefef),
            filterStrongColor = new Color(0xff212121),
            filterTextColor = new Color(0xff212121),
            filterItemsBorder = Border.all(
                color: new Color(0xffd8d8d8),
                width: 1f
            )
        };
    }

    internal static class SearchBarStyleExtension
    {
        internal static SearchBarStylePack StylePack(this SearchBarStyle style)
        {
            switch (style)
            {
                case SearchBarStyle.normal:
                    return SearchBarStylePack.Normal;
                case SearchBarStyle.embed:
                    return SearchBarStylePack.Embed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }

        internal static FilterItemStylePack FilterItemStylePack(this SearchBarStyle style)
        {
            switch (style)
            {
                case SearchBarStyle.normal:
                    return Components.FilterItemStylePack.Normal;
                case SearchBarStyle.embed:
                    return Components.FilterItemStylePack.Embed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }
    }
}