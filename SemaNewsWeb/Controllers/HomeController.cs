using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SemaNewsWeb.Controllers
{
    public class HomeController : Controller
    {
        SemaNewsDBContext entities = new SemaNewsDBContext();
        //
        // GET: /Home/
        public ActionResult Index()
        {
                return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                entities.Dispose();
            base.Dispose(disposing);
        }
    }
}
