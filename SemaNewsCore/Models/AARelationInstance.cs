using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class AARelationInstance
    {
        public int ArticleId1 { get; set; }
        public int ArticleId2 { get; set; }
        public int NRelationId { get; set; }
        public Nullable<double> RelationWeight { get; set; }
        public virtual Article Article { get; set; }
        public virtual Article Article1 { get; set; }
        public virtual NRelation NRelation { get; set; }
    }
}
