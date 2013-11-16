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

        // POST api/itemexclusion/PostItemExclusion
        public HttpResponseMessage PostItemExclusion(API_ItemExclusion itemExclusion)
        {
            var rcMsg = Request.CreateResponse(HttpStatusCode.BadRequest);
            if ((ModelState.IsValid) && ((!string.IsNullOrEmpty(itemExclusion.ListSharePublicKey)) && (!string.IsNullOrEmpty(itemExclusion.ItemPublicKey))))
            {
                try
                {
                    using (var dataMethods = new DataMethods())
                    {
                        var listShareID = dataMethods.ListShare_GetByPublicKey(itemExclusion.ListSharePublicKey).ListShareID;
                        var itemID = dataMethods.ListItem_GetByPublicKey(itemExclusion.ItemPublicKey).ItemID;
                        var rcItemExclusion = dataMethods.ItemExclusion_Create(listShareID, itemID);
                        if (rcItemExclusion != null)
                        {
                            rcMsg = Request.CreateResponse(HttpStatusCode.Created, rcItemExclusion);
                        }
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

        public HttpResponseMessage DeleteItemExclusion(string id)
        {
            var rcMsg = Request.CreateResponse(HttpStatusCode.OK);
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (var dataMethods = new DataMethods())
                    {
                        var rcItemExclusion = dataMethods.ItemExclusion_Delete(id);
                        if (!rcItemExclusion)
                        {
                            Request.CreateResponse(HttpStatusCode.BadRequest);
                        }
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
