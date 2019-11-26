using System;
using UnityEngine;

namespace Unity.DocZh.Models.Json
{
    [Serializable]
    public class Version
    {
        [SerializeField] public string unityVersion;
        [SerializeField] public int parsedVersion;
    }
}