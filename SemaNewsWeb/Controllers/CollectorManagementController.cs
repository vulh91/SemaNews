using SemaNews.Collector;
using SemaNewsCore.Configurations;
using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SemaNewsWeb.Controllers
{
    [Authorize]
    public class CollectorManagementController : ApiController
    {
        public bool StartEngine()
        {
            return true;
        }

        public bool StopEngine()
        {
            return true;
        }

        public bool ResetEngine()
        {
            return true;
        }

        [HttpPost]
        [ActionName("CollectNews")]
        public MsgNotification CollectNews()
        {
            if (CollectorConfigManager.CollectorInfo.IsCollectorBusy == true)
            {
                return new MsgNotification("Bộ thu thập đang bận. Hãy thử lại sau", MsgType.Error.ToString());
            }
            else
            {
                CollectorConfigManager.RequestToCollect = true;
                return new MsgNotification("Tiến trình thu thập đã được kích hoạt", MsgType.Success.ToString());

            }
        }
        [HttpPost]
        [ActionName("StopCollectNews")]
        public MsgNotification StopCollectNews()
        {
            if (CollectorConfigManager.CollectorInfo.IsCollectorBusy == true)
            {
                CollectorConfigManager.RequestStopCollect = true;
                return new MsgNotification("Tiến trình thu thập đang được dừng", MsgType.Success.ToString());
            }
            else
            {
                return new MsgNotification("Hiện không có tiến trình thu thập nào hiện đang chạy", MsgType.Error.ToString());
            }
        }

        [HttpGet]
        [ActionName("UpdateInfo")]
        public CollectorInfo UpdateInfo()
        {
            return CollectorConfigManager.CollectorInfo;
        }
    }
}