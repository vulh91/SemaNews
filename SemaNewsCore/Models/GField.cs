using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class GField
    {
        public GField()
        {
            this.Articles = new List<Article>();
            this.Fields = new List<Field>();
            this.GGRelationInstancesOut = new List<GGRelationInstance>();
            this.GGRelationInstancesIn = new List<GGRelationInstance>();
            this.SavedArticles = new List<SavedArticle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<GGRelationInstance> GGRelationInstancesOut { get; set; }
        public virtual ICollection<GGRelationInstance> GGRelationInstancesIn { get; set; }
        public virtual ICollection<SavedArticle> SavedArticles { get; set; }
    }
}
