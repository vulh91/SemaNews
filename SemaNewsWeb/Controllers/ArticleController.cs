using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;

namespace SemaNewsWeb.Controllers
{
    public class ArticleController : Controller
    {
        private SemaNewsDBContext entities = new SemaNewsDBContext();

        public ActionResult Index(int id)
        {
            var article = entities.Articles.Find(id);

            List<MsgNotification> messages = new List<MsgNotification>();
            if (article == null)
            {
                messages.Add(new MsgNotification("LỖI! Tin bài không tồn tại", MsgType.Error));
                ViewBag.Messages = messages;
                ViewBag.Title = "LỖI";
                return View();
            }

            ViewBag.Title = article.Title;
            ViewData["ArticleKGs"] = article.ArticleKG;

            return View(article);
        }

        public ActionResult Set(int FromIndex = 0, int SizePage = 100, int CategoryId = 0)
        {
            ArticleSetVM setVM = new ArticleSetVM()
            {
                CategoryId = CategoryId,
                Categories = entities.GFields.ToList(),
                FromIndex = FromIndex,
                SizePage = SizePage,
                CountTotal = 0,
            };
            if (CategoryId == 0)
            {
                setVM.CountTotal = entities.Articles.Count();
                setVM.Articles = entities.Articles.OrderByDescending(m=>m.CollectedDate).Skip(FromIndex).Take(SizePage).ToList();
            }
            else
            {
                var gf = entities.GFields.Find(CategoryId);
                if (gf != null)
                {
                    var articles = gf.GetArticles(entities, true, true);
                    setVM.CountTotal = articles.Count;
                    setVM.Articles = articles.OrderByDescending(m => m.CollectedDate).Skip(FromIndex).Take(SizePage).ToList();
                }
            }

            return View(setVM);
        }

        public ActionResult Add()
        {
            ViewData["Newspapers"] = entities.Newspapers.ToList();
            ViewData["Fields"] = entities.Fields.ToList();
            ViewData["GFields"] = entities.GFields.ToList();

            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add(Article article)
        {
            if(ModelState.IsValid)
            {
                entities.Articles.Add(article);
                entities.SaveChanges();
                return RedirectToAction("Index", new { id = article.Id });
            }
            return View(article);
        }

        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Edit(Article article)
        {
            throw new NotImplementedException();
            if (ModelState.IsValid)
            {
                entities.Entry(article).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();

                SemaNewsSearchEngine.ArticleIndexer.AddUpdateLuceneIndex(article);
                if(true)
                {
                    SemaNewsSearchEngine.ArticleIndexer.GenerateGraphs(new Article[]{ article}, "");
                }

            }
            return RedirectToAction("Index", new { id = article.Id });
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var article = entities.Articles.Find(id);
            if(article!=null)
            {
                foreach (var item in article.SavedArticles) item.ArticleId = null;
                SemaNewsSearchEngine.ArticleIndexer.ClearLuceneIndexRecord(article.Id);
                entities.Articles.Remove(article);
                entities.SaveChanges();
                return Json(new { OK = true, Message = string.Format("SUCCESS! Tin bài \"{0}\" đã được xóa", article.Title) });
            }
            return Json(new { OK = false, Message = string.Format("ERROR! Đã có lỗi xảy ra trong quá trình thực hiện yêu cầu") });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entities.Dispose();
                base.Dispose(disposing);
            }
        }
    }
}