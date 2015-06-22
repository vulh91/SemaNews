using SemaNewsCore.Configurations;
using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace SemaNewsWeb.Controllers
{
    public class CollectorController : Controller
    {
        public ActionResult Index(DateTime? from, DateTime? to)
        {
            var collectorReports = new CollectorReportsVM() 
            { 
                FromDate = from,
                ToData = to,
            };

            collectorReports.InitChartArticlesByDays(from.HasValue?from.Value:DateTime.Today.AddDays(-10), to.HasValue?to.Value:DateTime.Today.AddDays(1).AddSeconds(-1));
            collectorReports.InitChartArticlesByNewspapers(from, to);
            collectorReports.InitChartArticlesByCategories(from, to);

            return View(collectorReports);
        }

        [HttpPost]
        public ActionResult Index(CollectorReportsVM collectorReportVM)
        {
            return RedirectToAction("Index", new { @from = collectorReportVM.FromDate, @to = collectorReportVM.ToData });
        }

        public ActionResult Config()
        {
            return View(new CollectorConfigVM());
        }

        public ActionResult AjaxChangeMode(string collectingMode)
        {
            try
            {
                var targetMode = (CollectingMode)Enum.Parse(typeof(CollectingMode), collectingMode, true);
                CollectorConfigManager.CollectingMode = targetMode;
                return Json(new { OK = true, Message = string.Format("Chế độ thu thập {0} đã được kích hoạt", collectingMode) });
            }
            catch
            {
                return Json(new { OK = false, Message = string.Format("Đã có lỗi xảy ra trong quá trình kích hoạt chế độ thu thập {0}", collectingMode) });
            }
        }

        [HttpPost]
        public ActionResult AjaxChangeApproach(string approach)
        {
            try
            {
                var targetApproach = (CollectingApproach)Enum.Parse(typeof(CollectingApproach), approach, true);
                CollectorConfigManager.CollectingApproach = targetApproach;
                return Json(new { OK = true, Message = string.Format("Bạn vừa kích hoạt chế độ thu thập {0} thành công", targetApproach.ToString()) });
            }
            catch
            {
                return Json(new { OK = false, Message = string.Format("Đã có lỗi xảy ra trong quá trình kích hoạt chế độ thu thập {0}", approach) });
            }

        }

        [HttpPost]
        public ActionResult AjaxEditCollectingInterval(string time)
        {
            try
            {
                CollectorConfigManager.CollectingInterval = TimeSpan.Parse(time);
                return Json(new { OK = true, Message = "Lưu thay đổi thành công !" });
            }
            catch
            {
                return Json(new { OK = false, Message = "Đã có lỗi xảy ra. Thay đổi tần suất thu thập thất bại !" });
            }
        }

        [HttpPost]
        public ActionResult AjaxEditCollectingDelay(string time)
        {
            try
            {
                CollectorConfigManager.CollectingDelay = TimeSpan.Parse(time);
                return Json(new { OK = true, Message = "Lưu thay đổi thành công !" });
            }
            catch
            {
                return Json(new { OK = false, Message = "Đã có lỗi xảy ra. Thay đổi thời gian nghĩ giữa hai lần thu thập thất bại !" });
            }
        }

        [HttpPost]
        public ActionResult AjaxEditCollectingTimes(string time)
        {
            try
            {
                if (time.StartsWith("[") && time.EndsWith("]"))
                {
                    time = time.Substring(1, time.Length - 2);
                }
                var timeStrs = time.Split(',', ';', '|');
                List<DateTime> newTimes = new List<DateTime>();
                foreach (var item in timeStrs)
                {
                    var timeStr = item;
                    if (timeStr.StartsWith("\"") && timeStr.EndsWith("\""))
                        timeStr = timeStr.Substring(1, timeStr.Length - 2);
                    var parts = timeStr.Split(':');
                    if (parts.Length != 3) continue;
                    var newTime = new DateTime(1, 1, 1, int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                    newTimes.Add(newTime);
                }
                CollectorConfigManager.CollectingTimes = newTimes;
                return Json(new { OK = true, Message = "Lưu thay đổi thành công !" });
            }
            catch
            {
                return Json(new { OK = false, Message = "Thay đổi đổi thời điểm thu thập thất bại !" });
            }
        }

        public ActionResult AjaxEditNumberOfPageToCrawl(int value)
        {
            try
            {
                CollectorConfigManager.NumberOfPageToCrawl = value;
                return Json(new { OK = true, Message = "Thay đổi cấu hình thành công" });
            }
            catch
            {
                return Json(new { OK = false, Message = "Thay đổi cấu hình không thành công!" });
            }
        }

        //[HttpPost]
        //public ActionResult AjaxActivateCollecter(string isActivated)
        //{
        //    try
        //    {
        //        bool value = bool.Parse(isActivated);
        //        var successMsg = value ? "Bộ thu thập tin tự động theo lịch trình đã được kích hoạt" : "Bộ thu thập tin tự đông theo lịc trình đã bị hủy";
        //        return Json(new { OK = true, Message = successMsg });
        //    }
        //    catch
        //    {
        //        return Json(new { OK = false, Message="Lỗi xảy ra trong quá trình Kích hoạt/Hủy kích hoạt bộ thu thập tự động"});
        //    }
        //}
    }
}