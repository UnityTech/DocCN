using System;
using UnityEngine;

namespace Unity.DocZh.Models.Json
{
    [Serializable]
    public class ManualModel
    {
        [SerializeField] public string name;
        [SerializeField] public Link prev;
        [SerializeField] public Link next;
        [SerializeField] public Breadcrumb[] breadcrumbs;
        [SerializeField] public Token[] tokens;
        [SerializeField] public ImageMeta[] imageMetas;
    }

    [Serializable]
    public class Link
    {
        [SerializeField] public string content;
        [SerializeField] public string link;
    }

    [Serializable]
    public class ImageMeta
    {
        [SerializeField] public string name;
        [SerializeField] public float width;
        [SerializeField] public float height;
    }
}