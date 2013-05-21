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

        public ActionResult UserLists()
        {
            var gAPI = new GyftoList.API.Controllers.UserListController();
            List<API_List> usrLists = new List<API_List>();
            ViewBag.Title = "Your Gyfto Lists";
            ViewBag.UserName = "Jason";
            try
            {
                usrLists = gAPI.GetUserLists("49c8f932");
                if ((usrLists == null) || (usrLists.Count < 1))
                {
                    throw new Exception("List for this User not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(usrLists);
        }

        public ActionResult Details(string id)
        {
            var gAPI = new GyftoList.API.Controllers.ListController();
            API_List usrList = new API_List();
            try
            {
                usrList = gAPI.GetList(id);
            }
            catch (Exception)
            {
                
                throw;
            }


            return View(usrList);
        }
    }
}
