using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SemaNewsCore.Models
{
    public partial class Newspaper
    {
        public Newspaper()
        {
            this.Articles = new List<Article>();
            this.ArticleWebElements = new List<ArticleWebElement>();
            this.Fields = new List<Field>();
            this.FieldWebElements = new List<FieldWebElement>();
            this.NNRelationInstancesOut = new List<NNRelationInstance>();
            this.NNRelationInstancesIn = new List<NNRelationInstance>();
            this.SavedArticles = new List<SavedArticle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Nullable<bool> IsLocal { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DefinedTime { get; set; }
        public Nullable<bool> IsActivated { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<ArticleWebElement> ArticleWebElements { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<FieldWebElement> FieldWebElements { get; set; }
        public virtual ICollection<NNRelationInstance> NNRelationInstancesOut { get; set; }
        public virtual ICollection<NNRelationInstance> NNRelationInstancesIn { get; set; }
        public virtual ICollection<SavedArticle> SavedArticles { get; set; }
    }
}
