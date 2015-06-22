using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class FFRelationInstance
    {
        public int FieldId1 { get; set; }
        public int FieldId2 { get; set; }
        public int NRelationId { get; set; }
        public virtual Field Field1 { get; set; }
        public virtual Field Field2 { get; set; }
        public virtual NRelation NRelation { get; set; }
    }
}
