using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class SemaNewsDBContext
    {
        public List<GField> GetRootGFiels()
        {
            return this.GFields.Where(m => !this.GGRelationInstances.Any(r => 
                r.GGRelation.Notation == NRelationNotation.PARENT.ToString() 
                && r.GField2.Id == m.Id))
                .ToList();
        }

        public List<Newspaper> GetRootNewspapers()
        {
            return this.Newspapers.Where(m => !this.NNRelationInstances.Any(r => 
                r.NRelation.Notation == NRelationNotation.PARENT.ToString()
                && r.NewspaperId2 == m.Id))
                .ToList();
        }
    }
}
