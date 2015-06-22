using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class Article
    {
        public Article()
        {
            this.AARelationInstances = new List<AARelationInstance>();
            this.AARelationInstances1 = new List<AARelationInstance>();
            this.SavedArticles = new List<SavedArticle>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Nullable<System.DateTime> ReleasedDate { get; set; }
        public Nullable<System.DateTime> CollectedDate { get; set; }
        public string Abstract { get; set; }
        public string Author { get; set; }
        public string Tags { get; set; }
        public string Content { get; set; }
        public bool IsIndexed { get; set; }
        public bool IsMark { get; set; }
        public Nullable<bool> IsRelevantToLocal { get; set; }
        public Nullable<int> FieldId { get; set; }
        public Nullable<int> GFieldId { get; set; }
        public Nullable<int> NewspaperId { get; set; }
        public virtual ICollection<AARelationInstance> AARelationInstances { get; set; }
        public virtual ICollection<AARelationInstance> AARelationInstances1 { get; set; }
        public virtual Field Field { get; set; }
        public virtual GField GField { get; set; }
        public virtual Newspaper Newspaper { get; set; }
        public virtual ArticleKG ArticleKG { get; set; }
        public virtual ICollection<SavedArticle> SavedArticles { get; set; }
    }
}
