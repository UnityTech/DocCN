namespace Unity.DocZh.Components
{
    internal partial class StringExtension
    {
        public static string FirstCharToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);  
        }  
    }
}