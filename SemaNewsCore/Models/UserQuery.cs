using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class UserQuery
    {
        public UserQuery()
        {
            this.SavedArticles = new List<SavedArticle>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SearchQuery { get; set; }
        public Nullable<System.DateTime> SavedTime { get; set; }
        public Nullable<bool> IsSaved { get; set; }
        public virtual ICollection<SavedArticle> SavedArticles { get; set; }
        public virtual User User { get; set; }
    }
}
