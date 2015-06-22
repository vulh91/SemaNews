using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNews.Collector.Models
{
    public class CandidateArticleComparer : IEqualityComparer<CandidateArticle>
    {
        public bool Equals(CandidateArticle x, CandidateArticle y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.URL == y.URL || x.Title.Contains(y.Title) || y.Title.Contains(x.Title);
        }

        public int GetHashCode(CandidateArticle obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashURL = obj.URL == null ? 0 : obj.URL.GetHashCode();

            return hashURL;
        }
    }
}
