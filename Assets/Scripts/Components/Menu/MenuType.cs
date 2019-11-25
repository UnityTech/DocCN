using System;

namespace Unity.DocZh.Components
{
    public enum MenuType
    {
        scripting,
        manual,
    }

    internal static class MenuTypeExtension
    {
        public static string TocFileName(this MenuType type)
        {
            switch (type)
            {
                case MenuType.scripting:
                    return "scripting/json/ScriptingAPI_toc_toc.json";
                case MenuType.manual:
                    return "manual/json/Manual_toc_toc.json";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string PageUrlPrefix(this MenuType type)
        {
            switch (type)
            {
                case MenuType.scripting:
                    return "/Scripting";
                case MenuType.manual:
                    return "/Manual";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string ToChinese(this MenuType @this)
        {
            switch (@this)
            {
                case MenuType.scripting:
                    return "脚本API";
                case MenuType.manual:
                    return "手册";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@this), @this, null);
            }
        }
    }
}