using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class GGRelation
    {
        public GGRelation()
        {
            this.GGRelationInstances = new List<GGRelationInstance>();
        }

        public int Id { get; set; }
        public string Notation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public virtual ICollection<GGRelationInstance> GGRelationInstances { get; set; }
    }
}
