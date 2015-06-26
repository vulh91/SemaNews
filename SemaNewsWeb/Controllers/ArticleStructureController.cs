using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using SemaNews.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SemaNewsWeb.Controllers
{
    public class ArticleStructureController : Controller
    {
        private SemaNewsDBContext entities = new SemaNewsDBContext();

        public ActionResult Create(int newspaperId, string articleUrl)
        {
            var newspaper = entities.Newspapers.Find(newspaperId);

            var htmlDocument = SemaNews.Utilities.HtmlHandler.GetHtmlSource(articleUrl);
            foreach (var item in htmlDocument.DocumentNode.Descendants("base"))
                item.RemoveAll();
            ViewBag.htmlDocumentStr = htmlDocument.DocumentNode.OuterHtml;
            ViewBag.newspaperId = newspaperId;

            ArticleStructure aStruct = ArticleStructure.GetArticleStructure(newspaperId, 0);

            return View(aStruct);
        }

        [HttpPost]
        public ActionResult Create(ArticleStructure aStruct)
        {
            try
            {
                var newspaper = entities.Newspapers.Find(aStruct.Newspaper.Id);
                var maxGroupElement = newspaper.ArticleWebElements.OrderBy(x => x.Group).LastOrDefault(); ;
                int group = maxGroupElement == null ? 1 : (int)maxGroupElement.Group + 1;
                aStruct.GroupIdentity = group;
                // TODO: Add insert logic here
                if (aStruct.AddToDB(entities))
                    return RedirectToAction("ManageArticleStructures", "Newspaper", new { id = aStruct.Newspaper.Id });
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int groupId, int newspaperId)
        {
            var newspaper = entities.Newspapers.Find(newspaperId);
            var delGroup = newspaper.ArticleWebElements.Where(x => x.Group == groupId);
            entities.ArticleWebElements.RemoveRange(delGroup);
            entities.SaveChanges();
            return RedirectToAction("ManageArticleStructures", "Newspaper", new { id = newspaper.Id });
        }
    }
}
