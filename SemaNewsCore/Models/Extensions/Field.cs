using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class Field
    {
        public static bool RemoveFromDb(SemaNewsDBContext db, int id, bool removeArticles = false)
        {
            bool result = false;
                var field = db.Fields.Find(id);
                if (field == null)
                    return result;

                db.Fields.Remove(field);
                db.Entry(field).State = System.Data.Entity.EntityState.Deleted;

                //Remove all relations
                RemoveRelations(db, field);

                //Remove all articles if requested
                if (removeArticles)
                {
                    foreach (var article in field.Articles)
                        Article.RemoveFromDb(db, article.Id);
                }

            result = true;
            return result;
        }

        private static void RemoveRelations(SemaNewsDBContext db, Field field)
        {
            var rels = db.FFRelationInstances.Where(m => m.FieldId1 == field.Id || m.FieldId2 == field.Id);
            db.FFRelationInstances.RemoveRange(rels);
        }

    }
}
