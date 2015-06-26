using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SemaNewsWeb.Controllers
{
    [Authorize]
    public class TopicController : Controller
    {
        //
        // GET: /Topic/
        SemaNewsDBContext entities = new SemaNewsDBContext();
        public ActionResult Index(string filter = "")
        {
            if (string.IsNullOrEmpty(filter) == false)
            {
                ViewBag.FilterSting = filter;
                return View(entities.Topics.Where(m => m.Name.ToLower().Contains(filter.ToLower())).ToList());
            }

            return View(entities.Topics);
        }

        public ActionResult Create()
        {
            var LDVL_Keyphrases = new string[] { };
            var DT_Keyphrases = new string[] { };
            try
            {
                LDVL_Keyphrases = SearchLibrary.Helper.KeyphraseUtility.GetKeyphraseFromDatabase().Select(m => m.Name).ToArray();
                DT_Keyphrases = SearchLibrary.Helper.KeyphraseUtility.GetKeyphraseFromDatabase_DTCDTNN().Select(m => m.Name).ToArray();
            }
            catch
            {
            }
            ViewData["LDVL_Keyphrases"] = LDVL_Keyphrases;
            ViewData["DT_Keyphrases"] = DT_Keyphrases;

            return View(new TopicVM()); 
        }

        [HttpPost]
        public ActionResult AjaxCreate(TopicVM topic)
        {
            var newTopic = topic.ParseToTopic();
            var isValid = true;
            if (isValid)
            {
                entities.Topics.Add(newTopic);
                entities.SaveChanges();
                return Json(new { OK = true, Message = string.Format("Đã thêm chủ đề {0} thành công", newTopic.Name) });
            }
            return Json(new { OK = false, Message = "Đã có lỗi xảy ra trong quá trình thực hiện yêu cầu" });
        }

        public ActionResult Edit(int id)
        {
            var LDVL_Keyphrases = new string[] { };
            var DT_Keyphrases = new string[] { };
            try
            {
                LDVL_Keyphrases = SearchLibrary.Helper.KeyphraseUtility.GetKeyphraseFromDatabase().Select(m => m.Name).ToArray();
                DT_Keyphrases = SearchLibrary.Helper.KeyphraseUtility.GetKeyphraseFromDatabase_DTCDTNN().Select(m => m.Name).ToArray();
            }
            catch
            {
            }
            ViewData["LDVL_Keyphrases"] = LDVL_Keyphrases;
            ViewData["DT_Keyphrases"] = DT_Keyphrases;

            var topic = entities.Topics.Find(id);
            var topicVM = new TopicVM(topic);

            return View(topicVM);
        }

        [HttpPost]
        public ActionResult AjaxEdit(TopicVM topic)
        {
            var errorResult = Json(new { OK = false, Message = string.Format("Đã có lỗi xảy ra trong quá trình thực hiện yêu cầu")});
            try
            {
                if (topic.Topic != null && topic.Topic.Id != 0)
                {
                    var targetTopic = entities.Topics.Find(topic.Topic.Id);
                    if (targetTopic == null)
                        return errorResult;

                    var temp = topic.ParseToTopic();
                    targetTopic.Name = temp.Name;
                    targetTopic.Description = temp.Description;
                    targetTopic.Tags = temp.Tags;
                    targetTopic.Keyphrases = temp.Keyphrases;
                    //Initialize graphs

                    //

                    entities.Entry(targetTopic).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();

                    return Json(new { OK = true, Message = string.Format("Thay đổi thông tin chủ đề \"{0}\" thành công", targetTopic.Name), Data = topic });
                }
                 
                return errorResult;      
            }
            catch
            {
                return errorResult;
            }
        }

        [HttpPost]
        public ActionResult AjaxDelete(int id)
        {
            var errorResult = Json(new { OK = false, Message = string.Format("Đã có lỗi xảy ra trong quá trình thực hiện yêu cầu") });
            var delTopic = entities.Topics.Find(id);
            if (delTopic == null)
            {
                return errorResult;
            }

            try
            {
                if (ModelState.IsValid)
                {
                    entities.Topics.Remove(delTopic);
                    entities.Entry(delTopic).State = System.Data.Entity.EntityState.Deleted;

                    entities.SaveChanges();
                    return Json(new { OK = true, Message = string.Format("Chủ đề \"{0}\" đã được xóa thành công", delTopic.Name), Data = delTopic });
                }
                return errorResult;
            }
            catch
            {
                return errorResult;
            }
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
