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
        private const int PBKDF2IterCount = 1000; // default for Rfc2898DeriveBytes
        private const int PBKDF2SubkeyLength = 256 / 8; // 256 bits
        private const int SaltSize = 128 / 8; // 128 bits

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

                var user =  entities.Users.FirstOrDefault(m => m.Name == userName);

                if (user == null || !VerifyHashedPassword(user.Password, login.Password))
                {
                    ModelState.AddModelError("", "Username/Password is incorrect");
                    return View(login);
                }

                FormsAuthentication.SetAuthCookie(userName, login.Remember);
                if (CheckValidLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Register(RegisterVM registerVM)
        {
            if(ModelState.IsValid)
            {
                if(entities.Users.Any(t=>t.Name.ToLower() == registerVM.UserName.ToLower()))
                {
                    ModelState.AddModelError("UserName", "This username is already existed");
                    return View(registerVM);
                }

                var newUser = new User() { Name = registerVM.UserName.ToLower(), Password = HashPassword(registerVM.Password) };
                var userProfile = new UserProfile() { DateCreated = DateTime.Now, User = newUser, DisplayName = "", Avatar = "", Signature = "" };
                newUser.UserProfile = userProfile;

                entities.Users.Add(newUser);
                entities.UserProfiles.Add(userProfile);
                entities.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(registerVM);
        }

        private bool CheckValidLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            return Url.IsLocalUrl(url);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entities.Dispose();
            }
            base.Dispose(disposing);
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(password, SaltSize, PBKDF2IterCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            byte[] outputBytes = new byte[1 + SaltSize + PBKDF2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, PBKDF2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            // Wrong length or version header.
            if (hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
                return false;

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            byte[] storedSubkey = new byte[PBKDF2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, PBKDF2IterCount))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
            return storedSubkey.SequenceEqual(generatedSubkey);
        }
    }
}
