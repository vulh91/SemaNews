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
    public class TextSearchVM
    {
        public SearchQueryVM SearchQuery { get; set; }
        public List<Topic> AllTopics { get; set; }

        public List<SelectableItem<GField>> RootCategories { get; set; }
        public List<SelectableItem<Newspaper>> RootNewspapers { get; set; }
        public List<SelectableItem<string>> SearchFields { get; set; }

        public TextSearchVM()
        {
            SearchQuery = new SearchQueryVM();
            RootCategories = new List<SelectableItem<GField>>();
            RootNewspapers = new List<SelectableItem<Newspaper>>();
            SearchFields = new List<SelectableItem<string>>();
            AllTopics = new List<Topic>();

            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                db.GetRootGFiels().ForEach(item => RootCategories.Add(new SelectableItem<GField>(item, true)));
                db.GetRootNewspapers().ForEach(item => RootNewspapers.Add(new SelectableItem<Newspaper>(item, true)));
                AllTopics = db.Topics.ToList();
            }

            SearchFields.Add(new SelectableItem<string>(IndexField.Text, "Nội dung", true));
            SearchFields.Add(new SelectableItem<string>(IndexField.Title, "Tiêu đề", true));
            SearchFields.Add(new SelectableItem<string>(IndexField.Abstract, "Tóm tắt", true));
            SearchFields.Add(new SelectableItem<string>(IndexField.Tags, "Các từ khóa", true));
        }
    }
}