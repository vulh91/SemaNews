using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsSearchEngine.Models
{
    public class SearchMatch : IEqualityComparer<SearchMatch>
    {
        private List<string> topics;

        public int Id { get; set; }
        public Article Article { get; set; }
        public List<string> Topics
        {
            get
            {
                if (topics == null)
                    topics = new List<string>();
                return topics;
            }
            set
            {
                if (topics == null)
                    topics = new List<string>();
                topics = value;
            }
        }
        public bool IsLocalNews { get; set; }
        public float MatchingWeight { get; set; }
        public string HighLights { get; set; }

        public bool Equals(SearchMatch x, SearchMatch y)
        {
            if (x == null 
                || y == null
                || x.Article == null 
                || y.Article == null)
                return false;
            return x.Article.Id == y.Article.Id;
        }

        public int GetHashCode(SearchMatch obj)
        {
            return obj.GetHashCode();
        }
    }
}
