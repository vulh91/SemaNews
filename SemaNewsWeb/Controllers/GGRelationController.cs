using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SemaNewsWeb.Controllers
{
    public class GGRelationController :Controller
    {
        SemaNewsCore.Models.SemaNewsDBContext entities = new SemaNewsCore.Models.SemaNewsDBContext();

        public ActionResult Index()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult AjaxCreate(int gfieldId1, int gfieldId2, int relId)
        {
            var errorMsg = "Đã có lỗi xảy ra trong quá trình thực hiện yêu cầu ! Hãy thử lại sau";
            try
            {
                var gfield1 = entities.GFields.Find(gfieldId1);
                var gfield2 = entities.GFields.Find(gfieldId2);
                var rel = entities.GGRelations.Find(relId);

                if (gfield1 == null || gfield2 == null || rel == null)
                {
                    return Json(new { OK = false, Message = errorMsg });
                }

                GGRelationInstance newRel = new GGRelationInstance
                {
                    GFieldId1 = gfield1.Id,
                    GFieldId2 = gfield2.Id,
                    GGRelationId = rel.Id,
                };
                entities.GGRelationInstances.Add(newRel);
                entities.SaveChanges();

                string successMsg = string.Format("Tạo quan hệ {0} giữa {1} và {2} thành công", rel.Name, gfield1.Name, gfield2.Name);
                return Json(new { OK = true, Message = successMsg });
            }
            catch
            {
                return Json(new { OK = false, Message = errorMsg });
            }
        }

        [HttpPost]
        public ActionResult AjaxDelete(int gfieldId1, int gfieldId2, int relId)
        {
            var errorMsg = "Đã có lỗi xảy ra trong quá trình thực hiện yêu cầu ! Hãy thử lại sau";
            try
            {
                var delRel = entities.GGRelationInstances.SingleOrDefault(m => m.GFieldId1 == gfieldId1 && m.GFieldId2 == gfieldId2 && m.GGRelationId == relId);
                string successMsg = string.Format("Xóa quan hệ {0} giữa {1} và {2} thành công", delRel.GField2.Name, delRel.GField2.Name, delRel.GGRelation.Name);

                entities.GGRelationInstances.Remove(delRel);
                entities.SaveChanges();
                return Json(new { OK = true, Message = successMsg });
            }
            catch
            {
                return Json(new { OK = false, Message = errorMsg });
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