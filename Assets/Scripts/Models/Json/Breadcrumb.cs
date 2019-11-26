using System;
using UnityEngine;

namespace Unity.DocZh.Models.Json
{
    [Serializable]
    public class Breadcrumb
    {
        [SerializeField] public string content;
        [SerializeField] public string link;
    }
}