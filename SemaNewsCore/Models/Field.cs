using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class Field
    {
        public Field()
        {
            this.Articles = new List<Article>();
            this.FFRelationInstances = new List<FFRelationInstance>();
            this.FFRelationInstances1 = new List<FFRelationInstance>();
            this.SavedArticles = new List<SavedArticle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActivated { get; set; }
        public Nullable<int> Group { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        public Nullable<System.DateTime> DefinedTime { get; set; }
        public Nullable<int> NewspaperId { get; set; }
        public Nullable<int> GFieldId { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<FFRelationInstance> FFRelationInstances { get; set; }
        public virtual ICollection<FFRelationInstance> FFRelationInstances1 { get; set; }
        public virtual GField GField { get; set; }
        public virtual Newspaper Newspaper { get; set; }
        public virtual ICollection<SavedArticle> SavedArticles { get; set; }
    }
}
