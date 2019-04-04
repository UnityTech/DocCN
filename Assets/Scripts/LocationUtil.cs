namespace DocCN
{
    public static class LocationUtil
    {
        public static void Go(string path)
        {
            Reactive.CurrentPath.SetValueAndForceNotify(path);
            Bridge.LocationPush(path);
        }

        public static void HrefTo(string path)
        {
            Bridge.HrefTo(path);
        }
    }
}