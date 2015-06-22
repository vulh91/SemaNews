using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class SavedArticle
    {
        public int Id { get; set; }
        public int UserQueryId { get; set; }
        public Nullable<int> ArticleId { get; set; }
        public Nullable<System.DateTime> SavedTime { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Nullable<System.DateTime> ReleasedDate { get; set; }
        public Nullable<System.DateTime> CollectedDate { get; set; }
        public string Abstract { get; set; }
        public string Author { get; set; }
        public string Tags { get; set; }
        public string Content { get; set; }
        public Nullable<int> FieldId { get; set; }
        public Nullable<int> GFieldId { get; set; }
        public Nullable<int> NewspaperId { get; set; }
        public virtual Article Article { get; set; }
        public virtual Field Field { get; set; }
        public virtual GField GField { get; set; }
        public virtual Newspaper Newspaper { get; set; }
        public virtual UserQuery UserQuery { get; set; }
    }
}
