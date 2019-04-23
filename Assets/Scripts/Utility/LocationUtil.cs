using System.Web;
using UnityEngine;

namespace DocCN.Utility
{
    public static class LocationUtil
    {
        public static void Go(string path)
        {
#if UNITY_EDITOR
            Debug.Log($"Location to {path}");
#endif
            var appendedPrefix = $"{Configuration.Instance.pageBase}{path}";
            ObservableUtil.currentPath.value = appendedPrefix;
            Bridge.LocationPush(appendedPrefix);
        }

        public static void HrefTo(string path)
        {
            Bridge.HrefTo(path);
        }

        public static void Download(string path, string filename)
        {
            Bridge.Download(path, filename);
        }
    }
}