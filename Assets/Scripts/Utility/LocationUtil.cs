namespace DocCN.Utility
{
    public static class LocationUtil
    {
        public static void Go(string path)
        {
            var appendedPrefix = $"{Configuration.Instance.pageBase}{path}";
            ObservableUtil.currentPath.value = appendedPrefix;
            Bridge.LocationPush(appendedPrefix);
        }

        public static void HrefTo(string path)
        {
            Bridge.HrefTo(path);
        }
    }
}