using SemaNewsCore;
using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SemaNewsWeb.Controllers
{
    public class NewspaperController : Controller
    {
        private SemaNewsDBContext entities = new SemaNewsDBContext();

        //
        // GET: /Newspaper/
        public ActionResult Index(string filter = "")
        {
            if (string.IsNullOrEmpty(filter) == false)
            {
                ViewBag.FilterSting = filter;
                return View(entities.Newspapers.Where(m => m.Name.ToLower().Contains(filter.ToLower())).ToList());
            }
            return View(entities.Newspapers.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.ParentNewspaperId = new SelectList(entities.Newspapers.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewspaperVM newspaperVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newspaper = newspaperVM.Newspaper;
                    NNRelationInstance newspaperRel = null;

                    newspaperVM.Newspaper.DefinedTime = DateTime.Now;
                    if (newspaperVM.ParentNewspaperId.HasValue)
                    {
                        newspaperRel = new NNRelationInstance
                        {
                            NewspaperId1 = newspaperVM.ParentNewspaperId.Value,
                            Newspaper2 = newspaper,
                            NRelationId = NRelation.Find(SemaNewsCore.NRelationNotation.PARENT).Id,
                        };
                    }

                    entities.Newspapers.Add(newspaper);
                    entities.Entry(newspaper).State = System.Data.Entity.EntityState.Added;

                    if (newspaperRel != null)
                    {
                        entities.NNRelationInstances.Add(newspaperRel);
                        entities.Entry(newspaperRel).State = System.Data.Entity.EntityState.Added;
                    }
                    entities.SaveChanges();
                    return RedirectToAction("Edit", new { id = newspaper.Id });
                }

                ViewBag.ParentNewspaperId = new SelectList(entities.Newspapers.ToList(), "Id", "Name");
                return View(newspaperVM);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw (e);
            }
        }

        public ActionResult Edit(int id)
        {
            var newspaper = entities.Newspapers.Find(id);
            var newspaperRel = entities.NNRelationInstances.FirstOrDefault(m => m.NewspaperId2 == newspaper.Id && m.NRelation.Notation == NRelationNotation.PARENT.ToString());

            NewspaperVM newspaperVM = new NewspaperVM();
            newspaperVM.Newspaper = newspaper;
            newspaperVM.Fields = newspaper.Fields;
            if (newspaperRel != null)
                newspaperVM.ParentNewspaperId = newspaperRel.NewspaperId1;

            ViewBag.ParentNewspaperId = new SelectList(entities.Newspapers.ToList(), "Id", "Name", newspaperVM.ParentNewspaperId);

            return View(newspaperVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewspaperVM newspaperVM)
        {
            if (ModelState.IsValid)
            {
                entities.Entry(newspaperVM.Newspaper).State = System.Data.Entity.EntityState.Modified;

                var newspaperRel = entities.NNRelationInstances.FirstOrDefault(m => m.NewspaperId2 == newspaperVM.Newspaper.Id && m.NRelation.Notation == NRelationNotation.PARENT.ToString());
                if (newspaperVM.ParentNewspaperId.HasValue)
                {
                    if (newspaperRel != null)
                    {
                        //Update NNRelationInstance
                        entities.NNRelationInstances.Remove(newspaperRel);
                        entities.Entry(newspaperRel).State = System.Data.Entity.EntityState.Deleted;

                        var newNewspaperRel = new NNRelationInstance
                        {
                            NewspaperId1 = newspaperVM.ParentNewspaperId.Value,
                            NewspaperId2 = newspaperRel.NewspaperId2,
                            NRelationId = newspaperRel.NRelationId,
                        };
                        entities.NNRelationInstances.Add(newNewspaperRel);
                        entities.Entry(newNewspaperRel).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        //Create new NNRelationInstance
                        newspaperRel = new NNRelationInstance
                        {
                            NewspaperId1 = newspaperVM.ParentNewspaperId.Value,
                            NewspaperId2 = newspaperVM.Newspaper.Id,
                            NRelationId = NRelation.Find(SemaNewsCore.NRelationNotation.PARENT).Id,
                        };
                        entities.NNRelationInstances.Add(newspaperRel);
                        entities.Entry(newspaperRel).State = System.Data.Entity.EntityState.Added;
                    }
                }
                else
                {
                    //Remove NNRelation Instance if available
                    if (newspaperRel != null)
                    {
                        entities.NNRelationInstances.Remove(newspaperRel);
                        entities.Entry(newspaperRel).State = System.Data.Entity.EntityState.Deleted;
                    }
                }
                entities.SaveChanges();
                return RedirectToAction("Index");
            }
            newspaperVM.Newspaper.Fields.ToList();
            ViewBag.ParentNewspaperId = new SelectList(entities.Newspapers.ToList(), "Id", "Name", newspaperVM.ParentNewspaperId);
            return View(newspaperVM);
        }

        public ActionResult Delete(int id)
        {
            return View(entities.Newspapers.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Newspaper newspaper)
        {
            var delNewspaper = entities.Newspapers.Find(newspaper.Id);
            if (delNewspaper!=null)
            {
                if (Newspaper.RemoveFromDb(entities, newspaper.Id, true) == true)
                    return RedirectToAction("Index");
            }
            return View(newspaper);
        }

        public ActionResult ManageFields(int id)
        {
            var newspaper = entities.Newspapers.Find(id);

            if (newspaper == null)
            {
                return null;
            }

            var newspaperVM = new NewspaperVM
            {
                Newspaper = newspaper,
                Fields = newspaper.Fields,
            };

            return View(newspaperVM);
        }

        public ActionResult ManageFieldStructures(int id)
        {
            var newspaper = entities.Newspapers.Find(id);
            if (newspaper == null)
                return null;

            var fieldStructures = FieldStructure.GetAllFieldStructures(entities, id);

            var newspaperVM = new NewspaperVM
            {
                Newspaper = newspaper,
                FieldStructures = fieldStructures,
                Fields = newspaper.Fields,
            };

            return View(newspaperVM);
        }

        public ActionResult ManageArticleStructures(int id)
        {
            var newspaper = entities.Newspapers.Find(id);
            if (newspaper == null)
                return null;

            var articleStructures = ArticleStructure.GetAllArticleStructures(entities, id);

            var newspaperVM = new NewspaperVM
            {
                Newspaper = newspaper,
                Fields = newspaper.Fields,
                ArticleStructures = articleStructures,
            };
            return View(newspaperVM);
        }

        [HttpPost]
        public ActionResult ActivateOrDeactivate(int id)
        {
            var newspaper = entities.Newspapers.Find(id);
            if (newspaper == null)
                return Json(new { OK = false, Message= "LỖI! Không thể tìm thấy trang báo điện tử đang yêu cầu thực hiện..."});
            if (newspaper.IsActivated == null || newspaper.IsActivated == false)
            {
                newspaper.IsActivated = true;
                entities.SaveChanges();
                return Json(new { OK = true, Message = string.Format("THÀNH CÔNG! Trang báo điện tử {0} đã được kích hoạt để thu thập", newspaper.Name) });
            }
            else
            {
                newspaper.IsActivated = false;
                entities.SaveChanges();
                return Json(new { OK = true, Message = string.Format("CHÚ Ý! Trang báo điện tử {0} đã được HỦY kích hoạt", newspaper.Name )});
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
