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
    public class ListController : ApiController
    {

        #region Private Parameters

        private DataMethods _dataAccess;

        #endregion

        public ListController()
        {
            _dataAccess = new DataMethods();
        }

        public API_List GetList(string id)
        {
            var usrList = _dataAccess.List_GetListByPublicKey(id);
            API_List rcUsrList = new API_List(); ;
            API_ListShare listShare = new API_ListShare();
            if (usrList == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                rcUsrList = rcUsrList.ConvertToAPI_ListWithActiveItems(usrList);
            }

            // Add the associated ListShares
            foreach (var ls in _dataAccess.ListShare_GetAllByListPublicKey(id))
            {
                rcUsrList.ListShares.Add(listShare.ConvertToAPI_ListShareWithoutAssociatedList(ls));
            }

            return rcUsrList;
        }
    }
}
