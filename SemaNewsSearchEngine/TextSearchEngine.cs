using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SemaNewsCore.Models;
using SemaNewsSearchEngine.Config;
using SemaNewsSearchEngine.Models;
using SemaNews.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsSearchEngine
{
    public class TextSearchEngine : ISearchEngine
    {
        private static IEnumerable<SearchMatch> MapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            List<SearchMatch> matches = new List<SearchMatch>();

            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                foreach (var item in hits)
                {
                    var newSearchMatch = new SearchMatch();
                    newSearchMatch.Id = int.Parse(searcher.Doc(item.Doc).Get(IndexField.Id));
                    newSearchMatch.MatchingWeight = item.Score;
                    newSearchMatch.Article = db.Articles.Include("Newspaper").Include("GField").SingleOrDefault(m => m.Id == newSearchMatch.Id);
                    if (newSearchMatch.Article != null)
                    {
                        newSearchMatch.HighLights = newSearchMatch.Article.Abstract;
                        matches.Add(newSearchMatch);
                    }
                }
            }
            return matches;
        }

        public void IndexNewArticle(SemaNewsCore.Models.Article article)
        {
            ArticleIndexer.AddUpdateLuceneIndex(article);
        }

        public void DeleteArticleIndex(SemaNewsCore.Models.Article article)
        {
            ArticleIndexer.ClearLuceneIndexRecord(article.Id);
        }

        public void UpdateArticleIndex(SemaNewsCore.Models.Article article)
        {
            ArticleIndexer.AddUpdateLuceneIndex(article);
        }

        public IEnumerable<SearchMatch> Search(SearchQuery query, float threshold = 0.05f)
        {
            var queryCopy = EnhanceQuery(query);

            var results = Search(queryCopy.ToLuceneQuery(), threshold);

            results = FilterMatchingArticles(results, queryCopy);

            return results.OrderByDescending(m => m.MatchingWeight);
        }

        public IEnumerable<SearchMatch> GetNewsHighLight(SearchQuery query, DateTime fromTime, DateTime toTime)
        {
            IEnumerable<SearchMatch> articles = new List<SearchMatch>();
            using(SemaNewsDBContext db = new SemaNewsDBContext())
            {
                articles = db.Articles.Where(a => a.ReleasedDate.HasValue && a.ReleasedDate >= fromTime && a.ReleasedDate <= toTime)
                    .Select(a => new SearchMatch() { Article = a, HighLights = a.Abstract, Id = a.Id, MatchingWeight = 1, IsLocalNews = a.Newspaper.IsLocal.Value }).ToList();
            }
            articles = articles.OrderByDescending(a => a.Article.ReleasedDate);
            articles = FilterMatchingArticles(articles, query);
            return articles;
        }

        private IEnumerable<SearchMatch> FilterMatchingArticles(IEnumerable<SearchMatch> articles, SearchQuery query)
        {
             SearchFilter filter = new SearchFilter();

            if (query.RelatedToLocalOnly)
                articles = filter.FilterRelatedToLocalProvince(articles);

            articles = filter.FilterByCategories(articles, query.Categories);

            articles = filter.FilterByNewspapers(articles, query.Newspapers);

            if (query.TimeCollectedFrom.HasValue || query.TimeCollectedTo.HasValue)
            {
                DateTime fromTime = query.TimeCollectedFrom.HasValue ? query.TimeCollectedFrom.Value : new DateTime();
                DateTime toTime = query.TimeCollectedFrom.HasValue ? query.TimeCollectedTo.Value : DateTime.Now;
                articles = filter.FilterByCollectedTime(articles, fromTime, toTime);
            }

            if(query.TimePublishedFrom.HasValue || query.TimePublishedTo.HasValue)
            {
                DateTime fromTime = query.TimePublishedFrom.HasValue ? query.TimePublishedFrom.Value : new DateTime();
                DateTime toTime = query.TimePublishedTo.HasValue ? query.TimePublishedTo.Value : DateTime.Now;
                articles = filter.FilterByPublihedTime(articles, fromTime, toTime);
            }

            articles = filter.ClassifyResultsByTopics(articles, query.Topics.ToArray());

            return articles;
        }

        private SearchQuery EnhanceQuery(SearchQuery query)
        {
            var queryCopy = query.DeepCopy();

            if (queryCopy.Topics != null)
                queryCopy.Topics.RemoveAll(t => string.IsNullOrEmpty(t));
            if (queryCopy.Newspapers != null)
                queryCopy.Newspapers.RemoveAll(t => string.IsNullOrEmpty(t));
            if(queryCopy.Categories!=null)
                queryCopy.Categories.RemoveAll(t => string.IsNullOrEmpty(t));

            //Detect LDVL and DT keyphrases to search more efficiently
            if (!string.IsNullOrEmpty(queryCopy.AllWord))
            {
                SearchLibrary.BuildKeyphraseGraph process = new SearchLibrary.BuildKeyphraseGraph();
                var queryLDVLGraph = process.BuildKeyphraseGraphForQuery(query.AllWord);
                var queryDTGraph = process.BuildKeyphraseGraphForQuery_DTCDTNN(query.AllWord);

                if(queryLDVLGraph != null)
                    foreach(var keyphrase in queryLDVLGraph.keyphraseList)
                    {
                        queryCopy.AllWord.Replace(keyphrase.keyphrase, "");
                        queryCopy.ExactWords.Add(keyphrase.keyphrase);
                    }
                if(queryDTGraph!= null)
                    foreach (var keyphrase in queryDTGraph.keyphraseList)
                    {
                        queryCopy.AllWord.Replace(keyphrase.keyphrase, "");
                        queryCopy.ExactWords.Add(keyphrase.keyphrase);
                    }
            }

            return queryCopy;
        }

        public IEnumerable<SearchMatch> Search(string queryStr, float threshold = 0)
        {
            using (var searcher = new IndexSearcher(ArticleIndexer.Directory, true))
            {
                int hitsLimit = 1000;
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, queryStr, analyzer);
                var query = ParseQuery(queryStr, parser);

                var collector = TopScoreDocCollector.Create(hitsLimit, true);
                searcher.Search(query, collector);
                //null, hitsLimit, Sort.RELEVANCE)..ScoreDocs;
                var hits = collector.TopDocs().ScoreDocs.Where(m => m.Score > threshold);
                var results = MapLuceneToDataList(hits, searcher);
                analyzer.Close();
                searcher.Dispose();

                return results;
            }
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            try
            {
                return parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                return parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
        }
    }
}
