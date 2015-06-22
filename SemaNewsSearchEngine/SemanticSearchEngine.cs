using SemaNewsCore;
using SemaNewsCore.Models;
using SemaNewsSearchEngine.Config;
using SemaNewsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsSearchEngine
{
    public class SemanticSearchEngine : ISearchEngine
    {
        public static void InitializeData()
        {
            CreateKeyphraseGraphLibrary.KeyphraseGraph.InitialData();
            CreateKeyphraseGraphLibrary.KeyphraseGraph_DTCDTNN.InitialData();
            SearchLibrary.SemanticSearch.InitialData();
            SearchLibrary.SemanticSearch_DTCDTNN.InitialData();
        }

        public SemanticSearchEngine(){ }

        public void IndexNewArticle(SemaNewsCore.Models.Article article)
        {
            throw new NotImplementedException();
        }

        public void DeleteArticleIndex(SemaNewsCore.Models.Article article)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var articleKG = db.ArticleKGs.Find(article.Id);
                if (articleKG != null)
                {
                    db.ArticleKGs.Remove(articleKG);
                    db.Entry(articleKG).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateArticleIndex(SemaNewsCore.Models.Article article)
        {
            IndexNewArticle(article);
        }

        public IEnumerable <Models.SearchMatch> Search(Models.SearchQuery query, float threshold = 0.5f)
        {
            List<SearchMatch> results = new List<SearchMatch>();
            SearchFilter filter = new SearchFilter();

            var semanticQuery = query.ToSimpleQueryString();
            if (query.Newspapers != null)
                query.Newspapers.RemoveAll(m => String.IsNullOrEmpty(m));
            if (query.Categories != null)
                query.Categories.RemoveAll(m => String.IsNullOrEmpty(m));
            if (query.Topics != null)
                query.Topics.RemoveAll(m => String.IsNullOrEmpty(m));

            //Initalize default semantic domains to search (LDVL + DTC_DTNN)
            if (query.Categories == null || query.Categories.Count == 0)
                query.Categories = new List<string>() 
                { 
                    SemanticDomains.LDVL, 
                    SemanticDomains.DTC_DTNN,
                };

            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                List<Article> articleList = new List<Article>();

                //Filter candidate articles by selected semantic domains
                if (query.Categories != null && query.Categories.Count != 0)
                {
                    foreach (var item in query.Categories)
                    {
                        var category = GField.Search(db, item);
                        if (category != null)
                            articleList = articleList.Union(category.GetArticleByRelation(db, SemaNewsCore.NRelationNotation.RELATED)).ToList();
                    }
                }

                //Filter candidate articles by required newspapers
                if (query.Newspapers != null && query.Newspapers.Count != 0)
                {
                    var acceptedNewspaperIds = new List<int>();
                    foreach (var item in query.Newspapers)
                    {
                        var newspaper = Newspaper.Search(db, item);
                        if (newspaper != null)
                            acceptedNewspaperIds.Add(newspaper.Id);
                    }
                    articleList.RemoveAll(a => !a.NewspaperId.HasValue || acceptedNewspaperIds.Contains(a.NewspaperId.Value) == false);
                }

                //Filter articles by time
                if (query.TimeCollectedFrom.HasValue || query.TimeCollectedTo.HasValue)
                {
                    var fromTime = query.TimeCollectedFrom.HasValue ? query.TimeCollectedFrom.Value : new DateTime();
                    var toTime = query.TimeCollectedTo.HasValue ? query.TimeCollectedTo.Value : DateTime.Now;
                    articleList.RemoveAll(a => !a.CollectedDate.HasValue || a.CollectedDate < fromTime || a.CollectedDate > toTime);
                }

                if (query.TimePublishedFrom.HasValue || query.TimePublishedTo.HasValue)
                {
                    var fromTime = query.TimePublishedFrom.HasValue ? query.TimePublishedFrom.Value : new DateTime();
                    var toTime = query.TimePublishedTo.HasValue ? query.TimePublishedTo.Value : DateTime.Now;
                    articleList.RemoveAll(a => !a.ReleasedDate.HasValue == false || a.ReleasedDate < fromTime || a.ReleasedDate > toTime);
                }

                //Search matching articles
                foreach (var category in query.Categories)
                {
                    if (category == SemanticDomains.LDVL)
                        results = results.Union(SearchIn_LDVL(query, articleList, threshold)).ToList();
                    else if (category == SemanticDomains.DTC_DTNN)
                        results = results.Union(SearchIn_DTCDTNN(query, articleList, threshold)).ToList();
                    else
                        System.Diagnostics.Debug.WriteLine("Invalid semantic domains required: {0}", category);
                }
            }
            if (query.RelatedToLocalOnly == true)
                results = filter.FilterRelatedToLocalProvince(results).ToList();
            results = filter.ClassifyResultsByTopics(results, query.Topics.ToArray()).ToList();
            results = results.GroupBy(m => m.Id).Select(m => m.First()).ToList();

            return results.OrderByDescending(m=>m.MatchingWeight);
        }

        private IEnumerable<SearchMatch> SearchIn_LDVL(SearchQuery query, IEnumerable<Article> articles, float threshold = 0.5f)
        {
            SearchLibrary.BuildKeyphraseGraph process = new SearchLibrary.BuildKeyphraseGraph();
            SearchLibrary.EntityModel.GraphEntity queryKG = process.BuildKeyphraseGraphForQuery(query.ToSimpleQueryString());
            List<SearchMatch> results = new List<SearchMatch>();

            if (queryKG != null)
            {
                foreach (var article in articles)
                {
                    if (article.ArticleKG == null || article.ArticleKG.LDVL_Graph == null) continue;
                    SearchLibrary.EntityModel.GraphEntity graphG = process.BuildAnKeyphraseGraph(article.ArticleKG.LDVL_Graph);
                    var rank = SearchLibrary.SemanticSearch.GetRank(queryKG, graphG);
                    if (rank >= threshold)
                    {
                        var newMatch = new SearchMatch()
                        {
                            Id = article.Id,
                            Article = article,
                            MatchingWeight = (float)rank,
                        };
                        results.Add(newMatch);
                    }
                }
                results = results.Union(TextSearch(query, queryKG.keyphraseList.Select(m => m.keyphrase).ToArray())).ToList();
            }

            return results;
        }

        private IEnumerable<SearchMatch> SearchIn_DTCDTNN(SearchQuery query, IEnumerable<Article> articles, float threshold = 0.5f)
        {
            SearchLibrary.BuildKeyphraseGraph process = new SearchLibrary.BuildKeyphraseGraph();
            SearchLibrary.EntityModel.GraphEntity queryKG = process.BuildKeyphraseGraphForQuery_DTCDTNN(query.ToSimpleQueryString());
            List<SearchMatch> results = new List<SearchMatch>();
            if (queryKG != null)
            {
                foreach (var article in articles)
                {
                    if (article.ArticleKG == null || article.ArticleKG.DT_Graph == null) continue;
                    SearchLibrary.EntityModel.GraphEntity graphG = process.BuildAnKeyphraseGraph(article.ArticleKG.DT_Graph);
                    var rank = SearchLibrary.SemanticSearch.GetRank(queryKG, graphG);
                    if (rank >= threshold)
                    {
                        var newMatch = new SearchMatch()
                        {
                            Id = article.Id,
                            Article = article,
                            MatchingWeight = (float)rank,
                        };
                        results.Add(newMatch);
                    }
                }
                results = results.Union(TextSearch(query, queryKG.keyphraseList.Select(m => m.keyphrase).ToArray())).ToList();
            }
            
            return results;
        }

        private IEnumerable<Models.SearchMatch> TextSearch(SearchQuery query, string[] keyphrases, float threshold = 0.7f)
        {
            var queryCopy = query.DeepCopy();
            var results = new List<SearchMatch>();

            TextSearchEngine textSearchEngine = new TextSearchEngine();
            queryCopy.SearchFields = new List<string> { IndexField.Title, IndexField.Abstract, IndexField.Tags };
            queryCopy.SearchString = string.Empty;
            queryCopy.AllWord = string.Join(" ", keyphrases.ToList());
            queryCopy.ExactWords = new List<string>();
            queryCopy.NoneOfWords = new List<string>();
            queryCopy.Categories = new List<string>();

            results = textSearchEngine.Search(queryCopy).ToList();

            return results;
        }
    }
}
