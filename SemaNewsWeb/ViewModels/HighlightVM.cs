using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class HighlightVM
    {
        public DisplayMode Display { get; set; }
        public bool IsRelevantToLocalOnly { get; set; }
        public bool IsLocalSourceOnly { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Newspaper> Newspapers { get; set; }
        public List<GField> Categories { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public List<ArticleGroup> ViewGroups { get; set; }

        public HighlightVM()
        {
            Newspapers = new List<Newspaper>();
            Topics = new List<Topic>();
            Categories = new List<GField>();
            ViewGroups = new List<ArticleGroup>();
            IsLocalSourceOnly = false;
            IsRelevantToLocalOnly = false;
            Display = DisplayMode.Default;
        }

    }
}