using System;
using UnityEngine;

namespace Unity.DocZh.Models.Json
{
    [Serializable]
    public class Menu
    {
        [SerializeField] public string title;
        [SerializeField] public string link;
        [SerializeField] public Menu[] children;
        public bool expanded;
    }
}