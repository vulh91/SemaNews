using HtmlAgilityPack;
using SemaNews.Collector.Models;
using SemaNews.Utilities;
using SemaNewsCore;
using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SemaNews.Collector
{
    public class NormalCollectorUnit : CollectorUnitBase
    {
        private float QuantityOfArticlesExpected = SemaNewsCore.Configurations.DefaultValues.QtyOfArticlesExpect;

        private List<string> _urlVisitedPage = new List<string>();

        public NormalCollectorUnit(int fieldId)
            : base(fieldId)
        {
        }

        protected override void CollectNews()
        {
            try
            {

                if (this.Field == null || this.Newspaper == null)
                    return;

                if (this.Field.Group.HasValue == false)
                {
                    OnNewMessageAdded(string.Format("{0} - {1} has no defined HTML structure", this.Field.Name, this.Newspaper.Name));
                    return;
                }

                var fieldStructure = SemaNewsCore.Models.FieldStructure.GetFieldStructure(Field.NewspaperId.Value, Field.Group.Value);
                var numberOfPageToCrawl = SemaNewsCore.Configurations.CollectorConfigManager.NumberOfPageToCrawl;
                var isMultiPageCollecting = numberOfPageToCrawl > 1;
                _urlVisitedPage.Add(Field.Url);
                var urlCurrentPage = _urlVisitedPage[0];
                var currentHtmlOfField = Utilities.HtmlHandler.GetRawHtmlSource(urlCurrentPage);
                do
                {
                    var newArticles = ExtractArticles(currentHtmlOfField, fieldStructure);
                    if (newArticles == null && newArticles.Count() == 0)
                        break;

                    if(isMultiPageCollecting)
                    {
                        urlCurrentPage = GetURLNextPage(urlCurrentPage, currentHtmlOfField, fieldStructure.PaginationElement.Address);
                        if (string.IsNullOrEmpty(urlCurrentPage))
                            break;
                    }

                    currentHtmlOfField = HtmlHandler.GetRawHtmlSource(urlCurrentPage);

                    numberOfPageToCrawl--;
                }
                while (numberOfPageToCrawl > 0);
            }
            catch (Exception e)
            {
                OnNewMessageAdded(string.Format("ERROR!!! {0} - {1} : {2}", this.Field.Name, this.Newspaper.Name, e.Message));
            }
        }

        private string GetURLNextPage(string urlCurrentPage, string currentHtmlOfField, string paginationAddress)
        {
            List<string> linkset = new List<string>();

            var html = new HtmlDocument();
            html.LoadHtml(currentHtmlOfField);

            var ele = HtmlQueryHelpers.GetNodesByXpathAttr(html, paginationAddress);
            if (ele == null)
                return string.Empty;

            foreach (var item in ele)
            {
                var temp = item.DescendantsAndSelf("a").Where(a => a.Attributes["href"] != null)
                       .Select(a => a.Attributes["href"].Value)
                       .ToList();
                foreach (var link in temp)
                {
                    if (link.ToLower().Contains("javascript"))
                        continue;
                    linkset.Add(HtmlHandler.FixLink(link, Field.Url));
                }
            }

            linkset = linkset.Distinct().ToList();
            linkset.Sort();

            foreach(var link in linkset)
            {
                if(!_urlVisitedPage.Contains(link))
                {
                    _urlVisitedPage.Add(link);
                    return link;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Extracts and returns set of new articles
        /// </summary>
        /// <param name="currentHtmlOfField"></param>
        /// <param name="fieldStructure"></param>
        /// <returns></returns>
        private IEnumerable<SemaNewsCore.Models.Article> ExtractArticles(string currentHtmlOfField, SemaNewsCore.Models.FieldStructure fieldStructure)
        {
            List<Article> articles = new List<Article>();
            if (string.IsNullOrEmpty(currentHtmlOfField) || fieldStructure == null)
                return articles;

            IEnumerable<SemaNews.Collector.Models.CandidateArticle> candidateArticles = GetCandidateArticles(currentHtmlOfField, fieldStructure);

            foreach (var candidateArticle in candidateArticles)
            {
                Article article = ExtractArticle(candidateArticle);
                if (article != null && ValidateArticle(article))
                {
                    OnNewArticleCollected(article);
                    articles.Add(article);
                }
            }
            return articles;
        }

        /// <summary>
        /// Get set of candidate articles for current html page of Field
        /// </summary>
        /// <param name="currentHtmlOfField"></param>
        /// <param name="fieldStructure"></param>
        /// <returns></returns>
        private IEnumerable<Models.CandidateArticle> GetCandidateArticles(string currentHtmlOfField, SemaNewsCore.Models.FieldStructure fieldStructure)
        {
            List<Models.CandidateArticle> candidateArticles = new List<Models.CandidateArticle>();
            if (string.IsNullOrEmpty(currentHtmlOfField) || fieldStructure == null)
                return candidateArticles;

            foreach (var articleListElm in fieldStructure.ArticleListElements)
            {
                var candidatesInList = GetCandidateArticles(currentHtmlOfField, articleListElm.Address);
                if (candidatesInList != null && candidatesInList.Count() != 0)
                    candidateArticles.AddRange(candidatesInList);
            }

            candidateArticles = FilterCandidateArticles(candidateArticles).ToList();

            foreach (var item in candidateArticles)
                OnURLVisted(item.URL, item.Title);

            return candidateArticles;
        }

        /// <summary>
        /// Get set of candidate articles for a ArticleList Element in current html page of field
        /// </summary>
        /// <param name="currentHtmlOfField"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private IEnumerable<Models.CandidateArticle> GetCandidateArticles(string htmlSource, string path)
        {
            List<Models.CandidateArticle> candidateArticles = new List<Models.CandidateArticle>();

            if (string.IsNullOrEmpty(htmlSource) || string.IsNullOrEmpty(path))
                return candidateArticles;

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlSource);

            // get Node match with Element in the htmldocument
            var elements = HtmlQueryHelpers.GetNodesByXpathAttr(htmlDoc, path);
            if (elements != null && elements.Count > 0)
            {
                var anchorLinks = elements[0].Descendants("a").Where(a => a.Attributes["href"] != null);
                foreach (var anchorLink in anchorLinks)
                {
                    candidateArticles.Add(new CandidateArticle(anchorLink.InnerText.Trim(), HtmlHandler.FixLink(anchorLink.Attributes["href"].Value, Field.Url)));
                }
            }

            return candidateArticles;
        }

        /// <summary>
        /// Remove and filter duplication, visited url
        /// </summary>
        /// <param name="candidateArticles"></param>
        /// <returns></returns>
        private IEnumerable<Models.CandidateArticle> FilterCandidateArticles(IEnumerable<Models.CandidateArticle> candidateArticles)
        {
            if (candidateArticles == null)
                throw new ArgumentNullException("Candidate Articles is NULL");

            List<CandidateArticle> goodCandidates = new List<CandidateArticle>();
            candidateArticles = candidateArticles.Where(a => a != null && !string.IsNullOrEmpty(a.Title) && !string.IsNullOrEmpty(a.URL));
            candidateArticles = candidateArticles.OrderBy(m => m.Title.Length).GroupBy(m => m.URL).Select(m => m.LastOrDefault()).ToList();

            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                for (int i = candidateArticles.Count() - 1; i >= 0; i--)
                {
                    var candidate = candidateArticles.ElementAt(i);
                    if (candidate == null)
                        continue;
                    if (!db.VisitedLinks.Any(t => (t.Name != null && t.Name == candidate.Title) || (t.URL != null && t.URL == candidate.URL)))
                        goodCandidates.Add(candidate);
                }
            }

            return goodCandidates;
        }

        private Article ExtractArticle(Models.CandidateArticle candidateArticle)
        {
            try
            {
                var msg = string.Format("EXTRACTING article \"{0}\" - {1}", candidateArticle.Title, candidateArticle.URL);
                OnNewMessageAdded(msg);

                var htmlSource = SemaNews.Utilities.HtmlHandler.GetRawHtmlSource(candidateArticle.URL);
                if (string.IsNullOrEmpty(htmlSource))
                    return null;

                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    var articleStructures = ArticleStructure.GetAllArticleStructures(db, Field.NewspaperId.Value);
                    foreach (var structure in articleStructures)
                    {
                        var article = ExtractArticleContent(structure, htmlSource);

                        if (article == null)
                            continue;

                        if (!ValidateArticle(article))
                            continue;

                        if (!candidateArticle.Title.ToLower().Contains(article.Title.Trim().ToLower()))
                        {
                            continue;
                        }

                        article.NormalizeForm();
                        article.FieldId = Field.Id;
                        article.NewspaperId = Field.NewspaperId;
                        article.GFieldId = Field.GFieldId;
                        article.CollectedDate = DateTime.Now;
                        article.Url = candidateArticle.URL;

                        string domain = HtmlHandler.DetermineDomain(article.Url);
                        article.Content = HtmlHandler.ReSetSourceStyle(article.Content, domain);

                        OnNewMessageAdded(string.Format("SUCCESS: Collected \"{0}\" at \"{1}\"", article.Title, candidateArticle.URL));
                        return article;
                    }
                }

                OnNewMessageAdded(string.Format("ERROR: Engine has tried using all article structue group but can not collect the article at {0}", candidateArticle.URL));
                return null;
            }
            catch (Exception)
            {
                OnNewMessageAdded(string.Format("ERROR: Exception raised !!! Can not collect the article at {0}", candidateArticle.URL));
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
                            {
                                if (this.Newspaper.Name.ToLower().Contains("bình dương"))
                                {
                                    //SO BAD HERE
                                    if (ele.Descendants("a").Count() > 2)
                                        continue;
                                }
                                

                                if (ele.InnerText.Length > max)
                                {
                                    max = ele.InnerText.Length;
                                    mostMatch = ele;
                                }
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

        private bool ValidateArticle(Article article)
        {
            if (article == null)
                return false;

            if (article.ReleasedDate == null)
                return false;

            if (string.IsNullOrEmpty(article.Title))
                return false;

            if (System.Text.RegularExpressions.Regex.Matches(article.Title, @"\|").Count > 2)
                return false;

            if (string.IsNullOrEmpty(article.Content))
                return false;

            if (article.Author != null && article.Author.Length > 100)
                return false;

            return true;
        }

        protected override void UpdateProgress()
        {
            //if (this.Status == SemaNewsCore.Configurations.CollectorStatus.Stopped)
            //    Progress = 100;
            //var progresss = (float)ArticleCount / ArticleCountExpectation * 100;
            //if (progresss >= 100)
            //    progresss = 99;
            //Progress = progresss;
        }
    }
}
