using Newtonsoft.Json;
using PagedList;
using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SemaNewsWeb.Controllers
{
    public class CategoryController : Controller
    {
        private SemaNewsDBContext entities = new SemaNewsDBContext();
        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Config(string searchString = "")
        {
            var tree = SemaNewsWeb.Helpers.CategoryTreeBuilder.BuildTreeViewForCategory();

            ViewBag.CategoryTreeView = JsonConvert.SerializeObject(tree.nodes, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            var categories = entities.GFields.ToList();
            if(string.IsNullOrEmpty(searchString) == false)
                categories.RemoveAll(m=>m.Name.ToLower().Contains(searchString.ToLower()) == false);

            ViewBag.searchString = searchString;

            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GField gField)
        {
            if (ModelState.IsValid)
            {
                entities.GFields.Add(gField);
                entities.Entry(gField).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
                return RedirectToAction("Config");
            }
            return View(gField);
        }

        public ActionResult Edit(int id)
        {
            CategoryVM category = new CategoryVM(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryVM category)
        {
            if (ModelState.IsValid)
            {
                entities.Entry(category.GField).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
                ViewBag.Messages = new List<MsgNotification>{new MsgNotification("Lưu thay đổi thành công !",MsgType.Success)};
                return View(category);
            }
            ViewBag.Messages = new List<MsgNotification> { new MsgNotification("Lưu thay đổi thất bại. Hãy thử lại sau !", MsgType.Error) };
            return View(category);
        }

        public ActionResult Delete(int id)
        {
            var gfield = entities.GFields.Find(id);
            ViewBag.TotalFields = gfield.Fields.Count;
            ViewBag.TotalArticles = gfield.Articles.Count;

            return View(gfield);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(GField gfield)
        {
            var delGfield = entities.GFields.Find(gfield.Id);
            if (delGfield != null)
            {
                entities.GFields.Remove(delGfield);
                entities.Entry(delGfield).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gfield);
        }

        public ActionResult Details(int id, int? page, int? size)
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
                var allArticles = entities.Articles;

                articles = allArticles.Where(m => SearchLibrary.RelatedBinhDuong
                   .GetRank(SemaNews.Utilities.HtmlHandler.StripHtmlTags(m.Title),
                   SemaNews.Utilities.HtmlHandler.StripHtmlTags(m.Abstract),
                   SemaNews.Utilities.HtmlHandler.StripHtmlTags(m.Content),
                   SemaNews.Utilities.HtmlHandler.StripHtmlTags(m.Tags)) > 0.5f).ToList();
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

        [HttpPost]
        public ActionResult AjaxDelete(int id)
        {
            var delGField = entities.GFields.Find(id);
            if (delGField.Fields.Count() != 0)
            {
                return Json(new { OK = false, Message = string.Format("Lĩnh vực phân loại {0} hiện có {1} trang lĩnh vực đang reference tới. Bạn không thể xóa lĩnh vực phân loại này !", delGField.Name, delGField.Fields.Count) });
            }
            try
            {
                entities.GFields.Remove(delGField);
                entities.Entry(delGField).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
                return Json(new { OK = true, Message = string.Format("Lĩnh vực phân loại {0} đã được xóa", delGField.Name) });
            }
            catch
            {
                return Json(new { OK = false, Message = string.Format("Đã có lỗi xảy ra trong quá trình xóa lĩnh vực phân loại {0}", delGField.Name) });
            }
        }

        public ActionResult ManageFields(int id)
        {
            return View(new CategoryVM(id));
        }

        public ActionResult ManageRelations(int id)
        {
            return View(new CategoryVM(id));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                entities.Dispose();
            base.Dispose(disposing);
        }
    }
}
