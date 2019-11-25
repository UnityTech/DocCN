using System;

namespace Unity.DocZh.Components
{
    internal partial class StringExtension
    {
        public static SearchBar.FilterType ToFilterType(this string s)
        {
            switch (s)
            {
                case "manual":
                    return SearchBar.FilterType.manual;
                case "scripting":
                    return SearchBar.FilterType.scripting;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }
}