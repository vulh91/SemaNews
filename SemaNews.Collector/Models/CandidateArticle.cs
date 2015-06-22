using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNews.Collector.Models
{
    public class CandidateArticle
    {
        public string Title { get; set; }
        public string URL {get; set;}

        public CandidateArticle(string title, string url)
        {
            this.Title = title;
            this.URL = url;
            Normalize();
        }

        private void Normalize()
        {
            Title = Title.Trim();
            URL = SemaNews.Utilities.HtmlHandler.NormalizeUrl(URL).Trim();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Title, this.URL);
        }
    }
}
