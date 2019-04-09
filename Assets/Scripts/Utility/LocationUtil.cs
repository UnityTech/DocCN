namespace DocCN.Utility
{
    public static class LocationUtil
    {
        public static void Go(string path)
        {
            ObservableUtil.currentPath.value = path;
            Bridge.LocationPush(path);
        }

        public static void HrefTo(string path)
        {
            Bridge.HrefTo(path);
        }
    }
}