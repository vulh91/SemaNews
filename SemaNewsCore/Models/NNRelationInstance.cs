using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class NNRelationInstance
    {
        public int NewspaperId1 { get; set; }
        public int NewspaperId2 { get; set; }
        public int NRelationId { get; set; }
        public virtual Newspaper Newspaper1 { get; set; }
        public virtual Newspaper Newspaper2 { get; set; }
        public virtual NRelation NRelation { get; set; }
    }
}
