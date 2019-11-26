using System;
using UnityEngine;

namespace Unity.DocZh.Models.Json
{
    [Serializable]
    public class SearchResults
    {
        [SerializeField] public int currentPage;
        [SerializeField] public SearchResultItem[] items;
        [SerializeField] public int[] pages;
        [SerializeField] public int total;
        [SerializeField] public int totalPages;
    }

    [Serializable]
    public class SearchResultItem
    {
        [SerializeField] public string name;
        [SerializeField] public string id;
        [SerializeField] public string type;
        [SerializeField] public string version;
        [SerializeField] public string highlight;
        [SerializeField] public Breadcrumb[] breadcrumbs;
    }
}