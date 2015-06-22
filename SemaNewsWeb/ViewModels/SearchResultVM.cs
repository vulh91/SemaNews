using PagedList;
using SemaNewsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class SearchResultVM
    {
        public SearchResultVM()
        {
            this.RestultGroups = new List<SearchResultGroupVM>();
            this.AllowCategories = new List<string>();
            this.AllowNewspapers = new List<string>();
            this.AllowTopics = new List<string>();
        }

        public SearchQuery Query { get; set; }
        public List<string> AllowNewspapers { get; set; }
        public List<string> AllowTopics { get; set; }
        public List<string> AllowCategories { get; set; }

        public DisplayMode Display { get; set; }
        public List<SearchResultGroupVM> RestultGroups { get; set; }
        public int ArticlesCount { get; set; }
    }

    public enum DisplayMode
    {
        Default,
        ByNewspapers,
        ByCategories,
        ByGeography,
        ByTopics,
    }
}