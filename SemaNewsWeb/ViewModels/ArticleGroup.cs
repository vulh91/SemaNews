using SemaNewsCore.Models;
using SemaNewsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemaNewsWeb.ViewModels
{
    public class ArticleGroup
    {
        public const int DEFAULT_PAGE_SIZE = 10;

        public int Id { get; set; }
        public string Name { get; set; }
        public List<SearchMatch> Articles { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public ArticleGroup()
        {
            Articles = new List<SearchMatch>();
            CurrentPage = 0;
            PageSize = 10;
        }
    }
}
