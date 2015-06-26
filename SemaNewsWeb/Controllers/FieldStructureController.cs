using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using SemaNews.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SemaNewsWeb.Extensions;
using System.Text.RegularExpressions;
namespace SemaNewsWeb.Controllers
{
    [Authorize]
    public class FieldStructureController : Controller
    {
        private SemaNewsDBContext entities = new SemaNewsDBContext();
        //
        // GET: /FieldStructure/

        public ActionResult Index(int newspaperId, int group, string url = "")
        {
            return View();
        }

        public ActionResult Edit(int newspaperId, int group)
        {
            throw new NotImplementedException();
        }


        public ActionResult Create(int newspaperId, int fieldId)
        {
            var newspaper = entities.Newspapers.Find(newspaperId);
            var field = entities.Fields.Find(fieldId);

            if (field == null || newspaper == null)
            {
                // handle wrong request
            }

            FieldStructure fieldStructure = FieldStructure.GetFieldStructure(newspaperId, 0);

            var htmlDocument = SemaNews.Utilities.HtmlHandler.GetHtmlSource(field.Url);
            foreach (var item in htmlDocument.DocumentNode.Descendants("base"))
                item.RemoveAll();

            ViewBag.htmlDocumentStr = htmlDocument.DocumentNode.OuterHtml;
            ViewBag.fieldId = field.Id;
            return View();
        }

        [HttpPost]
        public ActionResult Create(string heList, string peEle, int fieldId)
        {
            try
            {
            var weStrList = Regex.Split(heList, "###").Where(s => s != String.Empty);
            var wePaging = peEle;
            List<FieldWebElement> fieldList = new List<FieldWebElement>();

            var currentField = entities.Fields.Find(fieldId);
            var newspaper = entities.Newspapers.Find(currentField.NewspaperId);
            if (currentField == null || newspaper == null)
            {
                return Json(new { status = "error", msg = "Can't find field" });
            }

            // create new group number
            var maxGroupElement = newspaper.FieldWebElements.OrderBy(x => x.Group).LastOrDefault();
            int group = maxGroupElement == null ? 1 : (int)maxGroupElement.Group + 1;

            // save LIST and PAGINATION element to the database
            // WebElementTypeId = 1 means Field LIST
            foreach (var item in weStrList)
            {
                entities.FieldWebElements.Add(new FieldWebElement { Address = item, Group = group, NewspaperId = currentField.NewspaperId.Value, WebElementTypeId = WebElementType.Find(SemaNewsCore.WebElementNotation.LIST).Id });
            }
            // WebElementTypeId = 2 means Field PAGINATION
            entities.FieldWebElements.Add(new FieldWebElement { Address = peEle, Group = group, NewspaperId = currentField.NewspaperId.Value, WebElementTypeId = WebElementType.Find(SemaNewsCore.WebElementNotation.PAGINATION).Id });
            // set new Struct to field defined
            currentField.Group = group;
            
                // ---------------------
                entities.SaveChanges();
            }
            catch(Exception e)
            {
                var msg = e.Message;
            }
            return Json(new { status = "success", msg = "Cấu trúc được định nghĩa thành công..." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int newspaperId, int groupId)
        {
            var newspaper = entities.Newspapers.Find(newspaperId);
            var delGroup = newspaper.FieldWebElements.Where(x => x.Group == groupId);
            var fieldHaveGroup = entities.Fields.Where(x => x.NewspaperId == newspaperId && x.Group == groupId);

            foreach (var f in fieldHaveGroup)
            {
                f.Group = null;
            }

            entities.FieldWebElements.RemoveRange(delGroup);
            entities.SaveChanges();

            return RedirectToAction("ManageFieldStructures", "Newspaper", new { id = newspaper.Id });
        }

        [HttpGet]
        public ActionResult Preview(int newspaperId, int fieldId)
        {
            var newspaper = entities.Newspapers.Find(newspaperId);
            var field = entities.Fields.Find(fieldId);

            if (field == null || newspaper == null)
            {
                // handle wrong request
            }
            // retrieve group and convert it to original xpath
            List<FieldWebElement> fieldStructures = newspaper.FieldWebElements.OrderBy(x => x.Group).ToList();
            foreach (FieldWebElement we in fieldStructures)
            {
                string editAddr = Regex.Replace(we.Address, @"\{([^\}^\{])+?\}", "");
                we.Address = editAddr;
            }
            ViewBag.fieldStructures = fieldStructures;
            //-----------------------------
            var htmlDocument = SemaNews.Utilities.HtmlHandler.GetHtmlSource(field.Url);
            foreach (var item in htmlDocument.DocumentNode.Descendants("base"))
                item.RemoveAll();
            ViewBag.htmlDocumentStr = htmlDocument.DocumentNode.OuterHtml;
            ViewBag.fieldId = field.Id;
            return View();
        }

        [HttpPost]
        public ActionResult SelectGroup(int fieldId, int groupId)
        {
            var field = entities.Fields.Find(fieldId);
            if (field == null)
            {
                // handle wrong request
            }

            field.Group = groupId;

            entities.SaveChanges();

            return RedirectToAction("Edit", "Field", new { id = field.Id });
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //        entities.Dispose();
        //    base.Dispose();
        //}
    }
}
