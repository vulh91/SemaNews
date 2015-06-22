using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SemaNewsWeb.Controllers
{
    public class NewsController : Controller
    {
        SemaNewsDBContext entities = new SemaNewsDBContext();

        public ActionResult Index()
        {
            var rootGFs = entities.GetRootGFiels().OrderBy(m => m.Name).ToList();
            return View(rootGFs);
        }

        public ActionResult Category(int id, int? page, int? size)
        {
            var gfield = entities.GFields.Find(id);
            List<MsgNotification> Messages = new List<MsgNotification>();

            var pageNumber = page ?? 1;
            var pageSize = size ?? 10;

            if (gfield == null)
            {
                Messages.Add(new MsgNotification("LỖI! Lĩnh vực yêu cầu không tồn tại", MsgType.Error));
                ViewBag.Messages = Messages;
                ViewBag.Title = "Lỗi!";
                return View();
            
            }

            var articles = gfield.GetArticles(entities, true, true);
            if (gfield.Name == "Tỉnh Bình Dương")
            {
                var allArticles = entities.Articles.ToList();

                articles = allArticles.Where(m => m.IsRelevantToLocal == true).ToList();
            }

            var results = articles.OrderByDescending(m=>m.CollectedDate).ToPagedList(pageNumber, pageSize);

            var catergoryDetailsVM = new CategoryDetailsVM()
            {
                GField = gfield,
                Articles = results,
                Ancestors = GField.GetAncestors(id),
            };

            ViewBag.Title = gfield.Name;
            ViewBag.PageNumber = results.PageNumber;
            ViewBag.PageSize = results.PageSize;
            ViewBag.PageCount = results.PageCount;

            return View(catergoryDetailsVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entities.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
