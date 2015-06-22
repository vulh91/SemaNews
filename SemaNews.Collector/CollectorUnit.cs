using HtmlAgilityPack;
using SemaNews.Collector.Models;
using SemaNews.Utilities;
using SemaNewsCore;
using SemaNewsCore.Configurations;
using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SemaNews.Collector
{
    /// <summary>
    /// Represent a collector agent handling collecting news for a particular field of a newspaper
    /// </summary>
    public class CollectorUnit
    {
        public int NewspaperId { get; private set; }
        public int FieldId { get; private set; }
        public int Progress { get; private set; }
        public Newspaper Newspaper { get; set; }
        public Field Field { get; set; }
        public List<Article> Results { get; private set; }
        public CollectorStatus Status { get; private set; }
        //Events
        public DelChangedEventHandler NewArticleCollected;
        public DelChangedEventHandler NewMessageAdded;

        protected virtual void OnNewArticleCollected(object data)
        {
            if (NewArticleCollected != null)
                NewArticleCollected(this, data);
        }

        protected virtual void OnNewMessageAdded(object msg)
        {
            if (NewMessageAdded != null)
                NewMessageAdded(this, msg);
        }

        public CollectorUnit(int newspaperId, int fieldId)
        {
            this.NewspaperId = newspaperId;
            this.FieldId = fieldId;
            Results = new List<Article>();
            Status = CollectorStatus.Started;

            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                this.Newspaper = db.Newspapers.Find(newspaperId);
                this.Field = db.Fields.Find(fieldId);
            }
        }

        public void Start()
        {
            Status = CollectorStatus.Started;
            CollectNews();
        }

        public void Stop()
        {
            Status = CollectorStatus.Stopped;
        }

        public void Pause()
        {
            Status = CollectorStatus.Stopped;
        }

        public async void CollectNews()
        {
            try
            {
                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    var field = db.Fields.Find(FieldId);
                    if (field == null || field.NewspaperId.HasValue == false || field.Group.HasValue == false)
                        throw new ArgumentException();

                    var fieldStructure = FieldStructure.GetFieldStructure(field.NewspaperId.Value, field.Group.Value);
                    
                    var currentHtmlOfField = Utilities.HtmlHandler.GetRawHtmlSource(field.Url);

                    do
                    {
                        var newArticles = ExtractArticles(currentHtmlOfField, fieldStructure);
                        if (newArticles == null && newArticles.Count == 0)
                            break;

                        currentHtmlOfField = GetNextPage(currentHtmlOfField, fieldStructure.PaginationElement.Address);
                    }
                    while (true);
                }
            }
            catch (Exception e)
            {
                //Write log
                string msg = string.Format("ERROR: A Collector Unit for newspaper {0} failed while working. Info : {1}", Newspaper.Name, e.Message);
                OnNewMessageAdded(msg);

                Status = CollectorStatus.Stopped;
                return;
            }

            OnNewMessageAdded(String.Format("Collector Unit for field {0} - {1} has finished successfully", this.Field.Name, this.Newspaper.Name));
            Status = CollectorStatus.Stopped;
        }

        private string GetNextPage(string currentHtmlPage, string address)
        {
            throw new NotImplementedException();
        }

        private List<Article> ExtractArticles(string fieldHtml, FieldStructure fieldStructure)
        {
            List<Article> articles = new List<Article>();
            var articleLinks = ExtractArticleURLs(fieldHtml, fieldStructure);
            if (articleLinks == null | articleLinks.Count == 0)
                return articles;

            foreach (var link in articleLinks)
            {
                try
                {
                    var newArticle = ExtractArticleContent(link);
                    if (newArticle != null)
                    {
                        articles.Add(newArticle);
                        Results.Add(newArticle);
                        OnNewArticleCollected(newArticle);
                    }
                }
                catch(Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }

            }
            return articles;
        }

        public List<string> ExtractArticleURLs(string htmlSource, FieldStructure fieldStructure)
        {
            List<string> URLs = new List<string>();
            if (string.IsNullOrEmpty(htmlSource) || fieldStructure == null)
                return URLs;

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlSource);

            foreach (var item in fieldStructure.ArticleListElements)
            {
                var links = HtmlHandler.ExtractUrls(htmlSource, item.Address);
                URLs = URLs.Union(links).ToList();
            }

            URLs = FixShortToFullURL(URLs, this.Field.Url);
            URLs.RemoveAll(m => IsVisitedLink(m));
            AddVisitedLinks(URLs);

            URLs.RemoveAll(m => m.Contains("/trang"));
            return URLs;
        }

        public Article ExtractArticleContent(string articleURL)
        {
            try
            {
                var msg = string.Format("Trying to extract content for the article with the URL:\"{0}\"", articleURL);
                OnNewMessageAdded(msg);

                var htmlSource = SemaNews.Utilities.HtmlHandler.GetRawHtmlSource(articleURL);
                if (string.IsNullOrEmpty(htmlSource))
                    return null;

                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    var field = db.Fields.Find(FieldId);
                    if (field == null || field.NewspaperId.HasValue == false)
                        throw new ArgumentException();

                    var articleStructures = ArticleStructure.GetAllArticleStructures(db, field.NewspaperId.Value);
                    foreach (var structure in articleStructures)
                    {
                        var article = ExtractArticleContent(structure, htmlSource);

                        if (article != null && IsValidArticle(article))
                        {
                            article.NormalizeForm();
                            article.FieldId = FieldId;
                            article.NewspaperId = field.NewspaperId;
                            article.GFieldId = field.GFieldId;
                            article.CollectedDate = DateTime.Now;
                            article.Url = articleURL;

                            string domain = HtmlHandler.DetermineDomain(article.Url);
                            article.Content = HtmlHandler.ReSetSourceStyle(article.Content, domain);

                            OnNewMessageAdded(string.Format("SUCCESS: Collected \"{0}\" at \"{1}\"", article.Title, articleURL));

                            var link = db.VisitedLinks.SingleOrDefault(m => m.URL == article.Url);
                            if (link != null)
                            {
                                link.Name = article.Title;
                            }
                            else
                            {
                                AddVisitedLinks(new List<string> { article.Url });
                            }

                            db.SaveChanges();

                            return article;
                        }
                    }
                }

                OnNewMessageAdded(string.Format("ERROR: Engine has tried using all article structue group but can not collect the article at {0}", articleURL));
                return null;
            }
            catch (Exception)
            {
                OnNewMessageAdded(string.Format("ERROR: Exception raised !!! Can not collect the article at {0}", articleURL)); ;
                return null;
            }
        }

        private Article ExtractArticleContent(ArticleStructure articleStructure, string htmlSource)
        {
            if (articleStructure == null || string.IsNullOrEmpty(htmlSource))
                return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlSource);

            List<ArticleHtmlNode> articleHtml = new List<ArticleHtmlNode>();

            foreach (var ele in articleStructure.WebElements)
            {
                if (ele != null && string.IsNullOrEmpty(ele.Address) == false)
                {
                    List<HtmlNode> element = HtmlQueryHelpers.GetNodesByXpathAttr(doc, ele.Address);

                    if (element == null)
                        continue;

                    articleHtml.Add(new ArticleHtmlNode { Element = element, Notation = ele.WebElementType.WENotation, XpathInfo = new NodeXpathInfo(ele.Address) });
                }
            }

            if (articleHtml.Count == 0)
                return null;

            return GetArticleData(articleHtml);
        }

        private Article GetArticleData(List<ArticleHtmlNode> articleHtml)
        {
            Article a = new Article();
            foreach (ArticleHtmlNode item in articleHtml)
            {
                var notation = (WebElementNotation)Enum.Parse(typeof(WebElementNotation), item.Notation);
                switch (notation)
                {
                    case WebElementNotation.AUTHOR:
                        if (item.Element.Count >= 1)
                        {
                            HtmlNode mostMatch = GetMostMatchElement(item);
                            a.Author = mostMatch.InnerText.Trim();
                            a.Author = FilterContent(articleHtml, item, a.Author);
                            a.Author = HtmlEntity.DeEntitize(a.Author);
                            if (item.Element.Count > 1)
                                a.Author = a.Author + "(?)";
                        }
                        break;
                    case WebElementNotation.RELATION:
                        break;
                    case WebElementNotation.DATETIME:
                        if (item.Element.Count >= 1)
                        {
                            a.ReleasedDate = ParseTime(item.Element[0].InnerText);
                        }
                        break;
                    case WebElementNotation.CONTENT:
                        if (item.Element.Count >= 1)
                        {
                            int max = 0;
                            HtmlNode mostMatch = null;
                            foreach (var ele in item.Element)
                                if (ele.InnerText.Length > max)
                                {
                                    max = ele.InnerText.Length;
                                    mostMatch = ele;
                                }

                            a.Content = mostMatch.InnerHtml.Trim();
                            a.Content = FilterContent(articleHtml, item, a.Content);
                            a.Content = HtmlEntity.DeEntitize(a.Content);

                        }
                        break;
                    case WebElementNotation.TAGS:
                        if (item.Element.Count >= 1)
                        {
                            a.Tags = item.Element[0].InnerText.Trim();
                            a.Tags = FilterContent(articleHtml, item, a.Tags);
                            a.Tags = HtmlEntity.DeEntitize(a.Tags);
                            a.Tags = a.Tags.Replace("\r\n", string.Empty);
                            a.Tags = a.Tags.Replace("  ", "");
                            if (item.Element.Count > 1)
                                a.Tags = a.Tags + "(?)";
                        }
                        break;
                    case WebElementNotation.ABSTRACT:
                        if (item.Element.Count >= 1)
                        {
                            foreach (var s in item.Element[0].ChildNodes)
                                if (s.Name != "a")
                                    a.Abstract += s.InnerText;

                            a.Abstract = FilterContent(articleHtml, item, a.Abstract);
                            a.Abstract = HtmlEntity.DeEntitize(a.Abstract);
                            if (item.Element.Count > 1)
                                a.Abstract = a.Abstract + "(?)";
                        }
                        break;
                    case WebElementNotation.TITLE:
                        if (item.Element.Count >= 1)
                        {
                            a.Title = item.Element[0].InnerText.Trim();
                            a.Title = FilterContent(articleHtml, item, a.Title);
                            a.Title = HtmlEntity.DeEntitize(a.Title);
                            if (item.Element.Count > 1)
                                a.Title = a.Title + "(?)";
                        }
                        break;

                }
            }
            return a;
        }

        private DateTime? ParseTime(string time)
        {
            DateTime result = new DateTime();
            List<string> dateFormat = new List<string>() { "(?:(\\d{1,2})/(\\d{1,2})/(\\d{2,4}))", "(?:(\\d{1,2})-(\\d{1,2})-(\\d{2,4}))" };
            List<string> timeFormat = new List<string>() { "(\\d{1,2}:(\\d{1,2})" };
            Match dateMatch;
            Match timeMatch;
            foreach (string format in dateFormat)
            {
                dateMatch = Regex.Match(time, format);
                timeMatch = Regex.Match(time, "(\\d{1,2}):(\\d{1,2})");
                if (dateMatch.Value != string.Empty)
                {
                    CultureInfo culture = new CultureInfo("vi-VN");
                    string parseVal = dateMatch.Value + " " + timeMatch.Value;
                    if (DateTime.TryParse(parseVal, culture, DateTimeStyles.None, out result))
                    {
                        return result;
                    }
                    else if (DateTime.TryParse(dateMatch.Value, culture, DateTimeStyles.None, out result))
                    {
                        return result;
                    }
                    else
                        continue;
                }
            }
            return result;
        }

        private HtmlNode GetMostMatchElement(ArticleHtmlNode htmlNodeList)
        {

            if (htmlNodeList.Element.Count == 1)
                return htmlNodeList.Element[0];
            float compare = 100;
            HtmlNode candidate = htmlNodeList.Element[0];
            foreach (var item in htmlNodeList.Element)
            {
                int fromHead = item.ParentNode.ChildNodes.IndexOf(item);
                int fromTail = item.ParentNode.ChildNodes.Count - fromHead;
                float checkVal = 0;
                if (htmlNodeList.XpathInfo.FromTailPos != null && htmlNodeList.XpathInfo.FromHeadPos != null)
                {
                    if (int.Parse(htmlNodeList.XpathInfo.FromTailPos.Value) <= int.Parse(htmlNodeList.XpathInfo.FromHeadPos.Value))
                    {
                        checkVal = Math.Abs(fromTail - int.Parse(htmlNodeList.XpathInfo.FromTailPos.Value));
                    }
                    else
                    {
                        checkVal = Math.Abs(fromHead - int.Parse(htmlNodeList.XpathInfo.FromHeadPos.Value));
                    }
                    if (checkVal != 0 && checkVal < compare)
                    {
                        if (item.InnerText.Trim() != string.Empty)
                        {
                            compare = checkVal;
                            candidate = item;
                        }
                    }
                }
            }
            return candidate;
        }

        private string FilterContent(List<ArticleHtmlNode> articleHtml, ArticleHtmlNode contentElement, string content)
        {
            string result = content;
            foreach (ArticleHtmlNode item in articleHtml)
            {
                if (item.Element.Count >= 1 && item.Element[0] != contentElement.Element[0])
                {
                    if (item.Element[0].XPath.Contains(contentElement.Element[0].XPath))
                    {
                        string matchContent;
                        if (contentElement.Notation == "CONTENT")
                            matchContent = item.Element[0].OuterHtml.Trim();
                        else
                            matchContent = item.Element[0].InnerText.Trim();

                        result = result.Replace(matchContent, "");
                    }
                }
            }
            return result;
        }

        private bool IsValidArticle(Article article)
        {
            if (article == null)
                return false;

            if (article.ReleasedDate == null)
                return false;

            if (string.IsNullOrEmpty(article.Title))
                return false;

            if (Regex.Matches(article.Title, @"\|").Count > 2)
                return false;

            if (string.IsNullOrEmpty(article.Content))
                return false;

            if (article.Author != null && article.Author.Length > 100)
                return false;

            return true;
        }

        private bool IsVisitedLink(string url)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                if (db.VisitedLinks.SingleOrDefault(m => m.URL == url) != null)
                    return true;
            }
            return false;
        }

        private static List<string> FixShortToFullURL(List<string> linkset, string domain)
        {
            for (int j = 0; j < linkset.Count; j++)
            {
                linkset[j] = HtmlHandler.FixLink(linkset[j], domain);
            }
            return linkset;
        }

        private void AddVisitedLinks(IEnumerable<string> urls)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                foreach (var url in urls)
                {
                    var link = db.VisitedLinks.SingleOrDefault(m => m.URL == url);
                    if (link == null)
                    {
                        link = new VisitedLink()
                        {
                            URL = url,
                            VisitCount = 0,
                        };
                        db.VisitedLinks.Add(link);
                    }
                    else
                        db.Entry(link).State = System.Data.Entity.EntityState.Modified;

                    link.Time = DateTime.Now;
                    link.VisitCount++;
                    db.SaveChanges();
                }
            }
        }
    }

}
