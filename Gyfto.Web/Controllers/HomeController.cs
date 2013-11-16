using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gyfto.Web.App_Start;
using Newtonsoft.Json.Linq;
using Microsoft.Web.WebPages.OAuth;


namespace Gyfto.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Jason T.";

            //IDictionary<string, string> userData = (OAuthWebSecurity.GetOAuthClientData("facebook").AuthenticationClient as FacebookExtendedClient).UserData;

            //if (userData != null)
            //{
            //    string email = userData["email"];

            //    // If leave null the fieldTransform of the client you can access to complex properties like this:
            //    JObject picture = JObject.Parse(userData["picture"]);
            //    string url = (picture["data"] as JObject)["url"].ToString();

            //    ViewBag.Email = userData["email"];
            //    ViewBag.PictureUrl = url;
            //}
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
