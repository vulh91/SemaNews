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
    public class SearchFilter
    {
        public IEnumerable<SearchMatch> FilterRelatedToLocalProvince(IEnumerable<SearchMatch> articles)
        {
            return articles.Where(m => m.Article.IsRelevantToLocal == true);
        }

        public IEnumerable<SearchMatch> FilterByNewspapers(IEnumerable<SearchMatch> articles, IEnumerable<string> newspapers)
        {
            if (newspapers != null && newspapers.Count() != 0)
            {
                var acceptedNewspaperIds = new List<int>();
                foreach (var name in newspapers)
                {
                    var newspaper = Newspaper.Search(name);
                    if (newspaper != null)
                        acceptedNewspaperIds.AddRange(Newspaper.GetDescendants(newspaper.Id).Select(m => m.Id));
                }
                articles = articles.Except(articles.Where(a => a.Article.NewspaperId.HasValue == false));
                articles = articles.Except(articles.Where(a => !acceptedNewspaperIds.Contains(a.Article.NewspaperId.Value)));
            }
            return articles;
        }

        public IEnumerable<SearchMatch> FilterByCategories(IEnumerable<SearchMatch> articles, IEnumerable<string> categories)
        {
            if (categories != null && categories.Count() != 0)
            {
                var acceptedGFieldId = new List<int>();
                foreach (var name in categories)
                {
                    var gfield = GField.Search(name);
                    if (gfield != null)
                        acceptedGFieldId.AddRange(GField.GetDescendants(gfield.Id).Select(m => m.Id));
                }
                articles = articles.Except(articles.Where(a => a.Article.GFieldId.HasValue == false));
                articles = articles.Except(articles.Where(a => !acceptedGFieldId.Contains(a.Article.GFieldId.Value)));
            }
            return articles;
        }

        public IEnumerable<SearchMatch> FilterByCollectedTime(IEnumerable<SearchMatch> articles, DateTime from, DateTime to)
        {
            return articles.Where(a => a.Article.CollectedDate.HasValue && a.Article.CollectedDate >= from && a.Article.CollectedDate <= to);
        }

        public IEnumerable<SearchMatch> FilterByPublihedTime(IEnumerable<SearchMatch> articles, DateTime from, DateTime to)
        {
            return articles.Where(a => a.Article.ReleasedDate.HasValue && a.Article.ReleasedDate >= from && a.Article.ReleasedDate <= to);
        }

        public IEnumerable<SearchMatch> ClassifyResultsByTopics(IEnumerable<SearchMatch> results, params string[] topicStrs)
        {
            var textSearchEngine = new TextSearchEngine();
            bool removeUnknownTopic = true;
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                if (topicStrs == null || topicStrs.Length == 0)
                {
                    removeUnknownTopic = false;
                    topicStrs = db.Topics.Select(m => m.Name).ToArray();
                }

                foreach (var topicStr in topicStrs)
                {
                    if (string.IsNullOrEmpty(topicStr)) continue;
                    var topic = Topic.Search(db, topicStr);
                    if (topic == null) continue;

                    var topicSearchQuery = new SearchQuery();
                    topicSearchQuery.ExactWords.AddRange(topic.Tags.Split(';'));

                    var articlesInTopic = textSearchEngine.Search(topicSearchQuery.ToLuceneQuery()).Select(m => m.Article.Id);

                    foreach (var item in results)
                    {
                        if (articlesInTopic.Contains(item.Id))
                            item.Topics.Add(topicStr);
                    }
                }
                if(removeUnknownTopic)
                    results = results.Where(m => m.Topics.Count != 0);
            }
            return results;
        }
    }
}
