namespace DocCN.Utility
{
    public static class FloatExtension
    {
        private const float defaultFontSize = 16f;
        private const float defaultLineSpacing = 22.4f;

        public static float LineHeight(this float @this) => defaultFontSize * @this / defaultLineSpacing;
    }
}