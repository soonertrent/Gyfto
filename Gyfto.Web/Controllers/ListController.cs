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
    public class ListController : Controller
    {
        //
        // GET: /List/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

    }
}
