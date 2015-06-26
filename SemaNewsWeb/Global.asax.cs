using SemaNewsWeb.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace SemaNewsWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AutoMapperConfig.RegisterMappings();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SemaNewsSearchEngine.ArticleIndexer.LuceneDir = Settings.Default.LunceneDir;
            SearchEngineConfig.InitalizeData();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        string userName = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string userRoles = string.Empty;
                        using (SemaNewsCore.Models.SemaNewsDBContext entities= new SemaNewsCore.Models.SemaNewsDBContext())
                        {
                            var user = entities.Users.SingleOrDefault(u => u.Name == userName);
                            userRoles = user.Role!=null? user.Role.Name : "";
                        }
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                            new System.Security.Principal.GenericIdentity(userName, "Forms"),
                            userRoles.Split(';'));
                    }
                    catch (Exception ex)
                    {
                        //something went wrong
                        throw ex;
                    }
                }
            }

        }
    }
}