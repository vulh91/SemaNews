using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class SaveSearchResultVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArticleIds { get; set; }
        public string IgnoreArticleIds { get; set; }
        public string QueryContent { get; set; }
    }
}