using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.UIWidgets.painting;
using UnityEngine;

namespace Unity.DocZh.Utility
{
    public static class SearchResultHighlightParser
    {
        private static readonly Regex EmRegex = new Regex("<em>(.+?)</em>");

        public static List<TextSpan> Parse(
            string highlight,
            TextStyle normalStyle,
            TextStyle highlightStyle
        )
        {
            if (string.IsNullOrEmpty(highlight))
            {
                return null;
            }

            var textSpans = new List<TextSpan>();

            if (EmRegex.IsMatch(highlight))
            {
                var normalTexts = EmRegex.Split(highlight);
                textSpans.AddRange(normalTexts.Select((t, i) => new TextSpan(text: t, style: i % 2 == 0 ? normalStyle : highlightStyle)));
            }
            else
            {
                textSpans.Add(
                    new TextSpan(
                        text: highlight,
                        style: normalStyle
                    )
                );
            }

            return textSpans;
        }
    }
}