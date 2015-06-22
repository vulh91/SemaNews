using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class NRelation
    {
        public static NRelation Find(NRelationNotation notation)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                return db.NRelations.SingleOrDefault(m => m.Notation.ToUpper() == notation.ToString().ToUpper());
            }
        }
    }
}
