using SemaNewsCore.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Prog4ddictAdmin.Controllers
{
    public class AccountController : Controller
    {
        private SemaNewsDBContext entities = new SemaNewsDBContext();
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string returnUrl="")
        {
            if (!string.IsNullOrEmpty(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LogInVM login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string userName = login.UserName;
                string password = login.Password;

                bool userValid = entities.Users.Any(m => m.Name == userName && m.Password == password);

                if (userValid)
                {
                    FormsAuthentication.SetAuthCookie(userName, login.Remember);
                    if(CheckValidLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index","Home");
                }
                else
                    ModelState.AddModelError("LoginFailed", "The Username or Password is incorrect");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        private bool CheckValidLocalUrl(string url)
        {
            return Url.IsLocalUrl(url) && url.Length > 1 && url.StartsWith("./")
                        && !url.StartsWith("//") && !url.StartsWith("/\\");
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
