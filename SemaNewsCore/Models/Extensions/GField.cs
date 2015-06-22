using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class GField
    {
        public static GField Find(SemaNewsDBContext db, string name)
        {
            return db.GFields.FirstOrDefault(m => m.Name.ToUpper() == name.ToUpper());
        }

        public static List<GField> Search(IEnumerable<int> IDs)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                return db.GFields.Where(m => IDs.Contains(m.Id)).ToList();
            }
        }

        public static GField Search(string name)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                return db.GFields.SingleOrDefault(m => m.Name.ToLower() == name.ToLower().Trim());
            }
        }

        public static List<GField> GetDescendants(int id)
        {
            List<GField> results = new List<GField>();
            using(SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var gfield = db.GFields.Find(id);
                if (gfield == null) return results;

                results = GetDescendants(db, gfield);
            }

            results.Reverse();
            return results;
        }

        public static List<GField> GetAncestors(int id)
        {
            List<GField> results = new List<GField>();
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var gfield = db.GFields.Find(id);
                if (gfield == null) return results;

                results = GetAncestors(db, gfield);
            }

            results.Reverse();
            return results;
        }

        private static List<GField> GetAncestors(SemaNewsDBContext db, GField gfield)
        {
            List<GField> results = new List<GField>();

            if (gfield == null)
                return results;

            results.Add(gfield);

            var parents = gfield.GGRelationInstancesIn.Where(r => r.GFieldId2 == gfield.Id
                && r.GGRelation.Notation == NRelationNotation.PARENT.ToString())
                .Select(r => r.GField1);

            foreach (var parent in parents)
            {
                results.AddRange(GetAncestors(db, parent));
            }

            return results;
        }

        private static List<GField> GetDescendants(SemaNewsDBContext dbContext, GField gfield)
        {
            List<GField> results = new List<GField>();

            if (gfield == null)
                return results;

            results.Add(gfield);

            var childs = gfield.GGRelationInstancesOut.
                Where(r => r.GFieldId1 == gfield.Id 
                    && r.GGRelation.Notation == NRelationNotation.PARENT.ToString())
                .Select(r => r.GField2);

            foreach (var child in childs)
            {
                results.AddRange(GetDescendants(dbContext, child)); ;
            }

            return results;
        }

        public static GField Search(SemaNewsDBContext entities, string item)
        {
            return entities.GFields.SingleOrDefault(m => m.Name.ToLower() == item.ToLower().Trim());
        }

        public bool HasRelationTo(string name, NRelationNotation relation)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var targetGField = db.GFields.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
                if (targetGField == null) return false;
                if (db.GGRelationInstances.Any(m =>
                    ((m.GFieldId1 == this.Id && m.GFieldId2 == targetGField.Id) || (m.GFieldId1 == targetGField.Id && m.GFieldId2 == targetGField.Id))
                    && (m.GGRelation.Notation == relation.ToString())))
                    return true;
                return false;
            }
        }

        public List<Article> GetArticleByRelation(SemaNewsDBContext dbContext, NRelationNotation relation)
        {
            List<Article> articles = new List<Article>();
            switch (relation)
            {
                case NRelationNotation.SYNONYM:
                    break;
                case NRelationNotation.PARENT:
                    break;
                case NRelationNotation.RELATED:
                        var relatedGField = new List<int>(){this.Id};
                        relatedGField = relatedGField.Union(this.GGRelationInstancesOut.Where(m => m.GGRelation.Notation == relation.ToString()).Select(m => m.GFieldId2)).ToList();
                        relatedGField = relatedGField.Union(this.GGRelationInstancesIn.Where(m => m.GGRelation.Notation == relation.ToString()).Select(m => m.GFieldId1)).ToList();
                        articles = dbContext.Articles.Where(m =>
                             m.GFieldId.HasValue &&
                             relatedGField.Contains(m.GFieldId.Value))
                            .ToList(); 
                    break;
            }
            return articles;
        }

        public List<GField> GetGFieldsByRelation(SemaNewsDBContext dbContext, NRelationNotation relation)
        {
            var results = new List<GField>();
            switch (relation)
            {
                case NRelationNotation.SYNONYM:
                    results = results.Union(this.GGRelationInstancesOut.Where(m => m.GGRelation.Notation == relation.ToString()).Select(m => m.GField2)).ToList();
                    results = results.Union(this.GGRelationInstancesIn.Where(m => m.GGRelation.Notation == relation.ToString()).Select(m => m.GField1)).ToList();
                    break;
                case NRelationNotation.PARENT:
                    break;
                case NRelationNotation.RELATED:
                    results = results.Union(this.GGRelationInstancesOut.Where(m => m.GGRelation.Notation == relation.ToString()).Select(m => m.GField2)).ToList();
                    results = results.Union(this.GGRelationInstancesIn.Where(m => m.GGRelation.Notation == relation.ToString()).Select(m => m.GField1)).ToList();
                    break;
            }
            return results;
        }

        public List<Article> GetArticles(SemaNewsDBContext context, bool includeChilds, bool includeSibling)
        {
            var articles = this.Articles.ToList();

            if(includeChilds)
            {
                var childs = this.GGRelationInstancesOut.Where(m => m.GGRelation.Notation == SemaNewsCore.NRelationNotation.PARENT.ToString()).Select(r => r.GField2).ToList();
                foreach (var item in childs)
                    articles = articles.Union(item.GetArticles(context, includeChilds, includeSibling)).ToList();
            }

            if(includeSibling)
            {
                var siblings = this.GetGFieldsByRelation(context, NRelationNotation.SYNONYM);
                foreach (var item in siblings)
                    articles = articles.Union(item.GetArticles(context, includeChilds, includeSibling)).ToList();
            }

            return articles.OrderByDescending(m=>m.ReleasedDate).ToList();
        }
    }
}
