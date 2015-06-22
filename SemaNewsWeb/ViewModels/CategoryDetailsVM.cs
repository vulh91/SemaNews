using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class CategoryDetailsVM
    {
        public GField GField { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<GField> Ancestors { get; set; }
    }
}