using SemaNewsCore.Models;
using SemaNewsSearchEngine.Config;
using SemaNewsSearchEngine.Models;
using SemaNewsWeb.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class SemanticSearchVM
    {
        public SearchQueryVM SearchQuery { get; set; }
        public List<Topic> AllTopics { get; set; }

        public List<SelectableItem<GField>> RootCategories { get; set; }
        public List<SelectableItem<Newspaper>> RootNewspapers { get; set; }

        public SemanticSearchVM()
        {
            SearchQuery = new SearchQueryVM();
            RootCategories = new List<SelectableItem<GField>>();
            RootNewspapers = new List<SelectableItem<Newspaper>>();
            AllTopics = new List<Topic>();

            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                db.GetRootGFiels().ForEach(item => RootCategories.Add(new SelectableItem<GField>(item, true)));
                db.GetRootNewspapers().ForEach(item => RootNewspapers.Add(new SelectableItem<Newspaper>(item, true)));
                AllTopics = db.Topics.ToList();
            }

            AllTopics.Add(new Topic { Id = 1, Name = "Đời sống công nhân" });
            AllTopics.Add(new Topic { Id = 1, Name = "Việc làm người lao động" });
            AllTopics.Add(new Topic { Id = 1, Name = "Nạn thất nghiệp" });
            AllTopics.Add(new Topic { Id = 1, Name = "Chính sách giải quyết việc làm" });
            AllTopics.Add(new Topic { Id = 1, Name = "Biểu tình" });
            AllTopics.Add(new Topic { Id = 1, Name = "Tai nạn lao động" });
            AllTopics.Add(new Topic { Id = 1, Name = "Bảo hiểm xã hội" });
        }
    }
}