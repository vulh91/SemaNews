using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class ArticleSetVM
    {
        public ICollection<GField> Categories { get; set; }
        public ICollection<Article> Articles { get; set; }

        public int CategoryId { get; set; }
        public int FromIndex { get; set; }
        public int SizePage { get; set; }

        public int CountTotal { get; set; }
    }
}