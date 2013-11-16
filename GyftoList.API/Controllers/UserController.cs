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
    public class UserController : ApiController
    {

        #region Private Parameters

        private DataMethods _dataAccess;

        #endregion

        public UserController()
        {
            _dataAccess = new DataMethods();
        }
        
        
        public List<API_User> GetAllUsers()
                {
                    var rcUsrs = new List<API_User>();
                    var rc = _dataAccess.User_GetAllUsers();
                    if (rc == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        var apiUsr = new API_User();
                        foreach (var usr in rc)
                        { 
                            rcUsrs.Add(apiUsr.ConvertToAPI_UserWithoutAssociatedLists(usr));
                        }
                    }
                    return rcUsrs;
                }
        
        /// <summary>
        /// Gets a User by PublicKey
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public API_User GetSpecificUser(string id)
        {
            var usr = _dataAccess.User_GetUser(id);
            if (usr == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // API_User object
            var apiUser = new API_User(); 
            apiUser = apiUser.ConvertToAPI_UserWithAssociatedLists(usr);

            var apiListShare = new API_ListShare();
            // Get ListShares they own
            foreach (var ols in _dataAccess.ListShare_GetAllListSharesForOwner(apiUser.PublicKey))
            {
                apiUser.OwnedListShares.Add(apiListShare.ConvertToAPI_ListShareWithoutAssociatedList(ols));
            }

            // Get ListShares they consume
            foreach (var cls in _dataAccess.ListShare_GetAllListSharesForConsumer(apiUser.PublicKey))
            {
                apiUser.ConsumedListShares.Add(apiListShare.ConvertToAPI_ListShareWithoutAssociatedList(cls));
            }

            return apiUser;
        }
    }
}
