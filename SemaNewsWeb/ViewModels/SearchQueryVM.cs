using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class SearchQueryVM
    {
        public string SearchString { get; set; }
        public string AllWord { get; set; }
        public string ExactPhrases { get; set; }
        public string NoneOfWords { get; set; }

        public string SelectedSearchFields { get; set; }
        public string SelectedCategories { get; set; }
        public string SelectedNewspapers { get; set; }
        public string SelectedTopics { get; set; }

        public string PostedTimeFrom { get; set; }
        public string PostedTimeTo { get; set; }
        public string CollectedTimeFrom { get; set; }
        public string CollectedTimeTo { get; set; }

        public bool IsSemantic { get; set; }
        public bool IsRelevantToLocalOnly { get; set; }
        public string SemanticDomains { get; set; }
    }
}