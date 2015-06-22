using SemaNews.Collector;
using SemaNews.Collector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collector_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var collectorUnit = new NormalCollectorUnit(8);
            //collectorUnit.CollectorUnitStarted += collectorUnit_CollectorUnitStarted;
            //collectorUnit.CollectorUnitFinished += collectorUnit_CollectorUnitFinished;
            //collectorUnit.NewMesssageAdded += collectorUnit_NewMesssageAdded;
            //collectorUnit.URLVisited += collectorUnit_URLVisited;
            //collectorUnit.NewArticleCollected += collectorUnit_NewArticleCollected;

            //collectorUnit.Start();
            var html = SemaNews.Utilities.HtmlHandler.GetRawHtmlSource("http://baobinhduong.vn/quoc-hoi-tien-hanh-lay-phieu-tin-nhiem-doi-voi-50-chuc-danh-a104744.html");
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);
            var nodes = SemaNews.Utilities.HtmlQueryHelpers.GetNodesByXpathAttr(htmlDoc, "/html/body/div{class=wapper}/div{class=padding10}/div{class=left w506 marginright10}/div{id=listcate}/div[5]{id=newscontents}{fromHead=5}{fromTail=11}");
            foreach(var item in nodes)
            {
                var text = item.InnerText;
                Console.WriteLine(text);
            }
        }

        static void collectorUnit_URLVisited(CollectorUnitBase unit, string url, string title)
        {
            var file = new StreamWriter("LOGS\\VisitedURLs.txt", true);
            file.WriteLine(string.Format("[{0}] - {1} - {2}", DateTime.Now, url, title));
            file.Close();
        }

        static void collectorUnit_NewMesssageAdded(CollectorUnitBase unit, string message)
        {
            var file = new StreamWriter("LOGS\\CollectorMessages.txt", true);
            var msg = string.Format("[{0}] - {1}", unit.Field.Name, message);
            Console.WriteLine(msg);
            file.WriteLine(msg);
            file.Close();
        }

        static void collectorUnit_NewArticleCollected(CollectorUnitBase unit, SemaNewsCore.Models.Article article)
        {
            var file = new StreamWriter("LOGS\\CollectedArticles.txt", true);
            file.WriteLine(string.Format("[{0}] - {1}", article.ReleasedDate, article.Title));
            file.Close();
        }

        static void collectorUnit_CollectorUnitFinished(CollectorUnitBase unit)
        {
            Console.WriteLine(string.Format("{0} - Collector finished", DateTime.Now));
        }

        static void collectorUnit_CollectorUnitStarted(CollectorUnitBase unit)
        {
            Console.WriteLine(string.Format("{0} - Collector started", DateTime.Now));
        }
    }
}
