using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class Article
    {
        public string NewspaperName
        {
            get
            {
                if (this.NewspaperId.HasValue == false)
                    return string.Empty;

                if (this.Newspaper != null)
                    return this.Newspaper.Name;

                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    return db.Newspapers.Find(this.NewspaperId.Value).Name;
                }
            }
        }

        public string CategoryName
        {
            get
            {
                if (this.GFieldId.HasValue == false)
                    return string.Empty;

                if (this.GField != null)
                    return this.GField.Name;

                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    return db.GFields.Find(this.GFieldId).Name;
                }

            }
        }

        public string GetImageLink()
        {
            var link = "";
            string pattern = "(img .*?)(src=\")(.*?)(\")";
            var matches = Regex.Matches(this.Content, pattern, RegexOptions.IgnoreCase);
            if (matches.Count == 0)
                return link;
            link = matches[0].Groups[3].Value;
            return link;

        }

        public void NormalizeForm()
        {
            if (this.Title == null) this.Title = string.Empty;
            if (this.Abstract == null) this.Abstract = string.Empty;
            if (this.Tags == null) this.Tags = string.Empty;
            if(this.Content == null) this.Content = string.Empty;
            if (this.Author == null) this.Author = string.Empty;
            if (this.Url == null) this.Url = string.Empty;
        }

        public static bool RemoveFromDb(SemaNewsDBContext db, int id)
        {
            bool result = false;
            var article = db.Articles.Find(id);
            if (article == null)
                return result;

            db.Articles.Remove(article);
            db.Entry(article).State = System.Data.Entity.EntityState.Deleted;

            //Remove all relations
            RemoveRelations(db, article);

            //Remove its keyphrase-graph

            //Remove article from index-file

            result = true;
            return result;
        }

        private static void RemoveRelations(SemaNewsDBContext db, Article article)
        {
            var rels = db.AARelationInstances.Where(m => m.ArticleId1 == article.Id || m.ArticleId2 == article.Id);
            db.AARelationInstances.RemoveRange(rels);
        }

        public static IEnumerable<Article> GetAllArticles(SemaNewsDBContext db, params int[] ids)
        {
            return db.Articles.Where(m => ids.Contains(m.Id)).ToList();
        }
    }
}
