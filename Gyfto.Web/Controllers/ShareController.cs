using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GyftoList.API.Controllers;
using GyftoList.API.Translations;
using GyftoList.Util;

namespace Gyfto.Web.Controllers
{
    public class ShareController : Controller
    {
        //
        // GET: /Share/

        public ActionResult List(string id)
        {
            var sharedList = new API_ListShare();
            if (id != string.Empty)
            {
                var util = new GyftoList.Util.Greeting();
                ViewBag.Greeting = util.GetRandomGreeting();

                var gAPI = new ListShareController();
                try
                {
                    sharedList = gAPI.GetListShare(id);
                }
                catch (Exception)
                {
                    throw new Exception("Shared List not found");
                }
            }
            else { throw new Exception("Shared  List not provided"); }
            
            return View(sharedList);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult All()
        {
            var gAPI = new ListShareController();
            return View(gAPI.GetListShares());

            //List<API_ListShare> mockListShares = new List<API_ListShare>();
            //mockListShares.Add(new API_ListShare { PublicKey = "1c89e969", SharedList = new API_List { ListName = "Christmas 2012", PublicKey = "2b73a917" }, ConsumerDisplayName = "Amanda W.", OwnerDisplayName = "Jason T." });

            //return View(mockListShares);
        }

    }
}
