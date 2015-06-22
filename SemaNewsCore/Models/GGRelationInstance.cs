using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class GGRelationInstance
    {
        public int GFieldId1 { get; set; }
        public int GFieldId2 { get; set; }
        public int GGRelationId { get; set; }
        public virtual GField GField1 { get; set; }
        public virtual GField GField2 { get; set; }
        public virtual GGRelation GGRelation { get; set; }
    }
}
