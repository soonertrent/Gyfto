using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GyftoList.Data;
using GyftoList.API.Translations;

namespace GyftoList.API.Controllers
{
    public class ItemExclusionController : ApiController
    {
        #region Private Members

        private DataMethods _dataMethods = new DataMethods();

        #endregion

        #region Public Methods

        // GET api/ListShare/5
        public API_ItemExclusion GetSomething(string id)
        {
            return new API_ItemExclusion();
        }

        // POST api/itemexclusion/
        public HttpResponseMessage PostListShare(API_ItemExclusion itemExclusion)
        {
            var rcMsg = Request.CreateResponse(HttpStatusCode.BadRequest);
            if ((ModelState.IsValid) && (itemExclusion != null))
            {
                try
                {
                    var b = _dataMethods.ItemExclusion_Create(itemExclusion.ListSharePublicKey, itemExclusion.ItemPublicKey);
                    if (b)
                    {
                        rcMsg = Request.CreateResponse(HttpStatusCode.Created, itemExclusion);
                    }
                }
                catch (Exception ex)
                {
                    rcMsg = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                rcMsg = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return rcMsg;
        }

        #endregion
    }
}
