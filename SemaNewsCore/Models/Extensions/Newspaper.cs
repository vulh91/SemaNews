using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class Newspaper
    {
        public static bool RemoveFromDb(SemaNewsDBContext db, int id, bool removeArticles = false)
        {
            bool result = false;
                var newspaper = db.Newspapers.Find(id);
                if (newspaper == null)
                    return result;
                db.Newspapers.Remove(newspaper);
                db.Entry(newspaper).State = System.Data.Entity.EntityState.Deleted;
                RemoveRelations(db, newspaper);

                foreach (var field in newspaper.Fields)
                {
                    Field.RemoveFromDb(db, field.Id, removeArticles);
                }

            result = true;
            return result;
        }

        public static List<Newspaper> GetDescendants(int id)
        {
            List<Newspaper> results = new List<Newspaper>();
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var newspaper = db.Newspapers.Find(id);
                if (newspaper == null) return results;

                results = GetDescendants(db, newspaper);
            }
            return results;
        }

        public static Newspaper Search(string name)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                return db.Newspapers.SingleOrDefault(m => m.Name.ToLower() == name.ToLower().Trim());
            }
        }

        public static Newspaper Search(SemaNewsDBContext context, string name)
        {
            return context.Newspapers.SingleOrDefault(m => m.Name.ToLower() == name.ToLower().Trim());
        }

        private static void RemoveRelations(SemaNewsDBContext db, Newspaper newspaper)
        {
            var rels = db.NNRelationInstances.Where(m => m.NewspaperId1 == newspaper.Id || m.NewspaperId2 == newspaper.Id);
            db.NNRelationInstances.RemoveRange(rels);
        }

        private static List<Newspaper> GetDescendants(SemaNewsDBContext dbContext, Newspaper newspaper)
        {
            List<Newspaper> results = new List<Newspaper>();
            if (newspaper == null)
                return results;

            results.Add(newspaper);

            var childs = newspaper.NNRelationInstancesOut.
                Where(r =>r.NewspaperId1 == newspaper.Id
                    && r.NRelation.Notation == NRelationNotation.PARENT.ToString())
                .Select(m => m.Newspaper2);

            foreach (var child in childs)
            {
                results.AddRange(GetDescendants(dbContext, child));
            }
            return results;
        }
    }
}
