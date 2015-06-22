using HtmlAgilityPack;
using SemaNews.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNews.Collector.Models
{
    public class ArticleHtmlNode
    {
        public List<HtmlNode> Element { set; get; }
        public string Notation { set; get; }
        public NodeXpathInfo XpathInfo { set; get; }
        public ArticleHtmlNode()
        {
            Element = new List<HtmlNode>();
        }
    }
}
