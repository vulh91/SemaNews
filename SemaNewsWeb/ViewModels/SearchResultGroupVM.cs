using PagedList;
using SemaNewsCore.Models;
using SemaNewsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class SearchResultGroupVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SearchQuery Query { get; set; }
        public List<SearchMatch> Articles { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int ArticlesCount { get; set; }
    }
}