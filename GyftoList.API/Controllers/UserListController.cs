using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GyftoList.Data;
using GyftoList.Util;
using GyftoList.API.Translations;

namespace GyftoList.API.Controllers
{
    public class UserListController : ApiController
    {
        #region Private Parameters

        private DataMethods _dataAccess;

        #endregion

        public UserListController()
        {
            _dataAccess = new DataMethods();
        }

        public List<API_List> GetUserLists(string userPublicKey)
        {
            var rcList = new List<API_List>();
            try
            {
                var usr = _dataAccess.User_GetUser(userPublicKey);
                if (usr != null)
                {
                    var converter = new API_List();
                    var usrLists = _dataAccess.List_GetListByUserID(usr.UserID);
                    foreach (var l in usrLists)
                    {
                        rcList.Add(converter.ConvertToAPI_ListWithItems(l));
                    }
                }
                else
                {
                    throw new Exception("Unable to source User: " + userPublicKey);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rcList;
        }
    }
}
