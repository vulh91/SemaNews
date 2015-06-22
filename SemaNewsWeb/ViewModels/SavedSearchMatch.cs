using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class SavedSearchMatch
    {
        public SavedArticle Article { get; set; }
        public int Order { get; set; }
        public string Topics { get; set; }
    }
}