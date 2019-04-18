using System;

namespace DocCN.Components
{
    public enum MenuType
    {
        scripting,
        manual,
    }

    internal static class MenuTypeExtension
    {
        public static string RequestUrlPrefix(this MenuType type)
        {
            switch (type)
            {
                case MenuType.scripting:
                    return "scripting_json";
                case MenuType.manual:
                    return "manual_json";
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
    }
}