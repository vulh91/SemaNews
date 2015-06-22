using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class GGRelation
    {
        public static bool AddRelation(SemaNewsDBContext context, GGRelation newRel)
        {
            if (context == null || newRel == null)
                throw new ArgumentNullException();
            if (context.GGRelations.Any(rel => rel.Notation == newRel.Notation))
                return false;
            context.GGRelations.Add(newRel);
            context.SaveChanges();
            return true;
        }

        public static bool DeleteRelation(SemaNewsDBContext context, int id)
        {
            if (context == null)
                throw new ArgumentNullException();

            var rel = context.GGRelations.Find(id);
            if (rel == null)
                return false;
            foreach(var relInstance in rel.GGRelationInstances)
            {
                context.GGRelationInstances.Remove(relInstance);
                context.Entry(relInstance).State = System.Data.Entity.EntityState.Deleted;
            }
            context.GGRelations.Remove(rel);

            context.SaveChanges();
            return true;
        }
    }
}
