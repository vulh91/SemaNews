using SemaNewsCore.Models;
using SemaNewsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsSearchEngine
{
    public interface ISearchEngine
    {
        void IndexNewArticle(Article article);
        void DeleteArticleIndex(Article article);
        void UpdateArticleIndex(Article article);
        IEnumerable<SearchMatch> Search(SearchQuery query, float threshold = 0.5f);
    }
}
