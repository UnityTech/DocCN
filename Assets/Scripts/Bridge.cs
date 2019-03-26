using System.Runtime.InteropServices;

namespace DocCN
{
    public static class Bridge
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        public static extern void Initialize();

        [DllImport("__Internal")]
        public static extern void ChangeCursor(string cursor);

        [DllImport("__Internal")]
        public static extern void LocationPush(string title, string url);

        [DllImport("__Internal")]
        public static extern void LocationBack();

        [DllImport("__Internal")]
        public static extern void LocationReplace(string title, string url);
#else
        public static void Initialize()
        {
        }

        public static void ChangeCursor(string cursor)
        {
        }

        public static void LocationPush(string title, string url)
        {
        }

        public static void LocationBack()
        {
        }

        public static void LocationReplace(string title, string url)
        {
        }
#endif
    }
}