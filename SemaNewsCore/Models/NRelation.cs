using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class NRelation
    {
        public NRelation()
        {
            this.AARelationInstances = new List<AARelationInstance>();
            this.FFRelationInstances = new List<FFRelationInstance>();
            this.NNRelationInstances = new List<NNRelationInstance>();
        }

        public int Id { get; set; }
        public string Notation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<AARelationInstance> AARelationInstances { get; set; }
        public virtual ICollection<FFRelationInstance> FFRelationInstances { get; set; }
        public virtual ICollection<NNRelationInstance> NNRelationInstances { get; set; }
    }
}
