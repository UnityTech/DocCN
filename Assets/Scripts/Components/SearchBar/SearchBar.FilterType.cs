using System;

namespace Unity.DocZh.Components
{
    public partial class SearchBar
    {
        public enum FilterType
        {
            all,
            manual,
            scripting,
        }
    }

    internal static class FilterTypeExtension
    {
        public static string Text(this SearchBar.FilterType filterType)
        {
            switch (filterType)
            {
                case SearchBar.FilterType.all:
                    return "所有";
                case SearchBar.FilterType.manual:
                    return "手册内容";
                case SearchBar.FilterType.scripting:
                    return "脚本API";
                default:
                    return string.Empty;
            }
        }
    }
}