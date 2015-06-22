using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public class ArticleStructure
    {
        public int GroupIdentity { get; set; }
        public Newspaper Newspaper { get; set; }

        public ArticleWebElement TitleElement { get; set; }
        public ArticleWebElement DatePostedElement { get; set; }
        public ArticleWebElement TagElement { get; set; }
        public ArticleWebElement AuthorElement { get; set; }
        public ArticleWebElement PaginationElement { get; set; }
        public ArticleWebElement AbstractElement { get; set; }
        public ArticleWebElement ContentElement { get; set; }

        public List<ArticleWebElement> WebElements
        {
            get
            {
                List<ArticleWebElement> results = new List<ArticleWebElement>();
                results.Add(TitleElement);
                results.Add(DatePostedElement);
                results.Add(TagElement);
                results.Add(PaginationElement);
                results.Add(AbstractElement);
                results.Add(ContentElement);
                return results;
            }
        }

        public ArticleStructure()
        {
        }

        public static List<ArticleStructure> GetAllArticleStructures(SemaNewsDBContext db, int newspaperId)
        {
            List<ArticleStructure> structures = new List<ArticleStructure>();
            var newspaper = db.Newspapers.Find(newspaperId);
            if (newspaper == null)
                return structures;

            var groups = newspaper.ArticleWebElements.GroupBy(m => m.Group);
            foreach (var group in groups)
            {
                var structure = ArticleStructure.BuildStructure(group);
                structure.GroupIdentity = group.Key.HasValue ? group.Key.Value : 0;
                structure.Newspaper = newspaper;
                structures.Add(structure);
            }
            return structures;
        }

        public static ArticleStructure GetArticleStructure(int newspaperId, int group)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var newspaper = db.Newspapers.Find(newspaperId);
                if (newspaper == null)
                    return null;

                var elements = newspaper.ArticleWebElements.Where(e => e.Group.HasValue && e.Group.Value == group);
                var articleStructure =  BuildStructure(elements);
                articleStructure.Newspaper = newspaper;
                articleStructure.GroupIdentity = group;

                return articleStructure;
            }
        }

        private static ArticleStructure BuildStructure(IEnumerable<ArticleWebElement> webElements)
        {
            var articleStr = new ArticleStructure();
            if (webElements == null)
                return articleStr;

            articleStr.TitleElement = webElements.FirstOrDefault(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.TITLE.ToString().ToUpper());
            articleStr.AbstractElement = webElements.FirstOrDefault(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.ABSTRACT.ToString().ToUpper());
            articleStr.DatePostedElement = webElements.FirstOrDefault(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.DATETIME.ToString().ToUpper());
            articleStr.AuthorElement = webElements.FirstOrDefault(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.AUTHOR.ToString().ToUpper());
            articleStr.TagElement = webElements.FirstOrDefault(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.TAGS.ToString().ToUpper());
            articleStr.PaginationElement = webElements.FirstOrDefault(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.PAGINATION.ToString().ToUpper());
            articleStr.ContentElement = webElements.FirstOrDefault(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.CONTENT.ToString().ToUpper());

            return articleStr;
        }

        public bool AddToDB(SemaNewsDBContext db)
        {
            TitleElement.WebElementTypeId = WebElementType.Find(WebElementNotation.TITLE).Id;
            DatePostedElement.WebElementTypeId = WebElementType.Find(WebElementNotation.DATETIME).Id;
            TagElement.WebElementTypeId = WebElementType.Find(WebElementNotation.TAGS).Id;
            AuthorElement.WebElementTypeId = WebElementType.Find(WebElementNotation.AUTHOR).Id;
            PaginationElement.WebElementTypeId = WebElementType.Find(WebElementNotation.PAGINATION).Id;
            AbstractElement.WebElementTypeId = WebElementType.Find(WebElementNotation.ABSTRACT).Id;
            ContentElement.WebElementTypeId = WebElementType.Find(WebElementNotation.CONTENT).Id;

            try
            {
                foreach (var webElement in this.WebElements)
                {
                    if (webElement != null && webElement.IsValid())
                    {
                        webElement.NewspaperId = this.Newspaper.Id;
                        webElement.Group = this.GroupIdentity;
                        db.ArticleWebElements.Add(webElement);
                    }
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
