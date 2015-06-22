using SemaNewsCore.Models;
using SemaNewsSearchEngine.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SemaNewsWeb.Controllers
{
    public class UserQueryController : Controller
    {
        public SemaNewsDBContext entities = new SemaNewsDBContext();

        public ActionResult Index(int page = 0, bool isSavedOnly = true)
        {
            var user = entities.Users.FirstOrDefault(m => m.Name.ToLower() == User.Identity.Name.ToLower());
            if (user == null)
                return RedirectToAction("Login", "Account");

            List<UserQuery> results;
            if (isSavedOnly == true)
                results = entities.UserQueries.Where(m => m.IsSaved.HasValue && m.IsSaved.Value).ToList();
            else
                results = entities.UserQueries.ToList();
            return View(results.OrderByDescending(m => m.SavedTime).ToList());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var userQuery = entities.UserQueries.Find(id);
            if (userQuery != null)
            {
                try
                {
                    entities.UserQueries.Remove(userQuery);
                    entities.SaveChanges();
                    string msg = string.Format("SUCCESS! Câu truy vấn \"{0}\" đã được xóa", userQuery.Name);
                    return Json(new { OK = true, Message = msg });
                }
                catch
                {
                    return null;
                }
                
            }
            return Json(new { OK = false, Message = "ERROR! Đã có lỗi xảy ra trong quá trình thực hiện yêu cầu"});
        }

        [HttpPost]
        public ActionResult DeleteArticle(int id)
        {
            var article = entities.SavedArticles.Find(id);
            if (article == null)
            {
                return Json(new { OK = true, Message = string.Format("ERROR! Tin bài yêu cầu không tồn tại") });
            }
            entities.SavedArticles.Remove(article);
            entities.SaveChanges();
            return Json(new { OK = true, Message= string.Format("SUCCESS! Xóa tin bài \"{0}\" thành công", article.Title)});
        }


        public ActionResult Query(int id)
        {
            var query = entities.UserQueries.Find(id);
            if (query == null)
                return View();
            return View(query.SavedArticles.ToList());
        }

        public ActionResult ViewArticle(int id)
        {
            var article = entities.SavedArticles.Find(id);
            if (article == null)
                return View();
            return View(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                entities.Dispose();
            base.Dispose(disposing);
        }
    }
}