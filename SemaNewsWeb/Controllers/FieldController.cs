using SemaNewsCore;
using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SemaNewsWeb.Controllers
{
    public class FieldController : Controller
    {
        private SemaNewsDBContext entities = new SemaNewsDBContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int newspaperId)
        {
            FieldVM fieldVM = new FieldVM
            {
                Field = new Field
                {
                    NewspaperId = newspaperId,
                },
                Newspaper =  entities.Newspapers.Find(newspaperId),
                GFields = entities.GFields,
                SiblingFields = entities.Fields.Where(m => m.NewspaperId == newspaperId),
                FieldStructures = FieldStructure.GetAllFieldStructures(entities, newspaperId),
            };
            ViewBag.FieldsCount = entities.Newspapers.Find(newspaperId).Fields.Count();

            return View(fieldVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FieldVM fieldVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    fieldVM.Field.DefinedTime = DateTime.Now;

                    entities.Fields.Add(fieldVM.Field);
                    entities.Entry(fieldVM.Field).State = System.Data.Entity.EntityState.Added;

                    //Update relation with GField
                    if (fieldVM.GFieldId.HasValue && fieldVM.GFieldId.Value != 0)
                    {
                        fieldVM.Field.GFieldId = fieldVM.GFieldId;
                    }
                    else
                    {
                        var gField = GField.Find(entities, fieldVM.Field.Name);
                        if (gField != null)
                            fieldVM.Field.GFieldId = gField.Id;
                        else
                        {
                            gField = new GField
                            {
                                Name = fieldVM.Field.Name,
                                Description = "Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này",
                            };
                            entities.GFields.Add(gField);
                            entities.Entry(gField).State = System.Data.Entity.EntityState.Added;
                            fieldVM.Field.GField = gField;
                        }
                    }

                    //Update relation with parent field
                    if (fieldVM.ParentFieldId.HasValue && fieldVM.ParentFieldId.Value != 0)
                    {
                        var fieldRel = new FFRelationInstance
                        {
                            FieldId1 = fieldVM.ParentFieldId.Value,
                            Field2 = fieldVM.Field,
                            NRelationId = NRelation.Find(SemaNewsCore.NRelationNotation.PARENT).Id,
                        };
                        entities.FFRelationInstances.Add(fieldRel);
                        entities.Entry(fieldRel).State = System.Data.Entity.EntityState.Added;
                    }

                    entities.SaveChanges();

                    return RedirectToAction("Edit", "Field", new { id = fieldVM.Field.Id });
                }

                fieldVM.Newspaper =  entities.Newspapers.Find(fieldVM.Field.NewspaperId);
                fieldVM.GFields = entities.GFields;
                fieldVM.SiblingFields = entities.Fields.Where(m => m.NewspaperId == fieldVM.Field.NewspaperId);
                fieldVM.FieldStructures = FieldStructure.GetAllFieldStructures(entities, fieldVM.Field.NewspaperId??0);

                ViewBag.FieldsCount = entities.Newspapers.Find(fieldVM.Field.NewspaperId).Fields.Count();
                return View(fieldVM);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public ActionResult Edit(int id)
        {
            var field = entities.Fields.Find(id);
            if (field == null)
                return null;
            FieldVM fieldVM = new FieldVM()
            {
                Field = field,
                GFields = entities.GFields,
                GFieldId = field.GFieldId,
                Newspaper = field.Newspaper,
                SiblingFields = field.Newspaper.Fields,
                FieldStructures = FieldStructure.GetAllFieldStructures(entities, field.NewspaperId??0),
            };
            var parentRel  = entities.FFRelationInstances.FirstOrDefault(m=>m.FieldId2 == field.Id && m.NRelation.Notation == NRelationNotation.PARENT.ToString());
            fieldVM.ParentFieldId = parentRel != null ? parentRel.FieldId1 : 0;

            ViewBag.FieldsCount = entities.Newspapers.Find(fieldVM.Newspaper.Id).Fields.Count();
            return View(fieldVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FieldVM fieldVM)
        {
            if (ModelState.IsValid)
            {
                var field = fieldVM.Field;
                entities.Entry(field).State = System.Data.Entity.EntityState.Modified;

                if (field.GFieldId.HasValue == false)
                {
                    var gField = GField.Find(entities, fieldVM.Field.Name);
                    if (gField == null)
                    {
                        gField = new GField
                        {
                            Name = fieldVM.Field.Name,
                            Description = "An auto-generated GField",
                        };

                        entities.GFields.Add(gField);
                        entities.Entry(gField).State = System.Data.Entity.EntityState.Added;
                    }
                    fieldVM.Field.GField = gField;
                }

                //Update parent Rels
                var parentRel = entities.FFRelationInstances.FirstOrDefault(m => m.FieldId2 == field.Id && m.NRelation.Notation == NRelationNotation.PARENT.ToString());
                if (parentRel == null)
                {
                    if (fieldVM.ParentFieldId.HasValue)
                    {
                        parentRel = new FFRelationInstance
                        {
                            FieldId1 = fieldVM.ParentFieldId.Value,
                            FieldId2 = field.Id,
                            NRelationId = entities.NRelations.FirstOrDefault(m => m.Notation.ToUpper() == NRelationNotation.PARENT.ToString().ToUpper()).Id,
                        };
                        entities.FFRelationInstances.Add(parentRel);
                        entities.Entry(parentRel).State = System.Data.Entity.EntityState.Added;
                    }
                }
                else
                {
                    entities.FFRelationInstances.Remove(parentRel);
                    entities.Entry(parentRel).State = System.Data.Entity.EntityState.Deleted;

                    if (fieldVM.ParentFieldId.HasValue)
                    {
                        parentRel = new FFRelationInstance
                        {
                            FieldId1 = fieldVM.ParentFieldId.Value,
                            FieldId2 = field.Id,
                            NRelationId = entities.NRelations.FirstOrDefault(m=>m.Notation.ToUpper() == NRelationNotation.PARENT.ToString().ToUpper()).Id,
                        };

                        entities.FFRelationInstances.Add(parentRel);
                        entities.Entry(parentRel).State = System.Data.Entity.EntityState.Added;
                    }
                }

                entities.SaveChanges();
                
                return RedirectToAction("ManageFields", "Newspaper", new { id = field.NewspaperId});
            }
            fieldVM.Newspaper = entities.Newspapers.Find(fieldVM.Field.NewspaperId);
            fieldVM.GFields = entities.GFields;
            fieldVM.SiblingFields = fieldVM.Newspaper.Fields;
            fieldVM.FieldStructures = FieldStructure.GetAllFieldStructures(entities, fieldVM.Field.NewspaperId ?? 0);

            return View(fieldVM);
        }

        public ActionResult Delete(int id)
        {
            var field = entities.Fields.Find(id);
            if (field == null)
                return null;

            var fieldVM = new FieldVM
            {
                Field = field,
                Newspaper = field.Newspaper
            };

            ViewBag.FieldsCount = entities.Newspapers.Find(fieldVM.Newspaper.Id).Fields.Count();
            return View(fieldVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(FieldVM fieldVM)
        {
            var delField = entities.Fields.Find(fieldVM.Field.Id);
            if (delField != null)
            {
                int returnNewspaperId = delField.NewspaperId.Value;
                Field.RemoveFromDb(entities, delField.Id, true);
                entities.SaveChanges();

                ViewBag.FieldsCount = entities.Newspapers.Find(fieldVM.Newspaper.Id).Fields.Count();
                return RedirectToAction("ManageFields", "Newspaper", new { id = returnNewspaperId });
            }

            fieldVM.Field = delField;
            fieldVM.Newspaper = delField.Newspaper;
            ViewBag.FieldsCount = entities.Newspapers.Find(fieldVM.Newspaper.Id).Fields.Count();

            return View(fieldVM);
        }

        [HttpPost]
        public ActionResult ActivateOrDeactivate(int id)
        {
            var field = entities.Fields.Find(id);
            if (field == null)
                return Json(new { OK = false, Message = "LỖI! Không thể tìm thấy trang lĩnh vực đang yêu cầu thực hiện..." });
            if (field.IsActivated == null || field.IsActivated == false)
            {
                field.IsActivated = true;
                entities.SaveChanges();
                return Json(new { OK = true, Message = string.Format("THÀNH CÔNG! Trang lĩnh vực {0} đã được kích hoạt để thu thập", field.Name) });
            }
            else
            {
                field.IsActivated = false;
                entities.SaveChanges();
                return Json(new { OK = true, Message = string.Format("CHÚ Ý! Trang lĩnh vực {0} đã được HỦY kích hoạt", field.Name) });
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                entities.Dispose();
            base.Dispose(disposing);
        }
    }
}