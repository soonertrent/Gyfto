using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GyftoList.Data;
using GyftoList.API.Translations;

namespace GyftoList.API.Controllers
{
    public class ListShareController : ApiController
    {
        /// <summary>
        /// NOTE: THIS WAS ALL AUTO-GENED BY VS2012
        /// </summary>

        // GET api/ListShare
        public IQueryable<API_ListShare>GetListShares()
        {
            API_ListShare converter = new API_ListShare();
            var rcListShares = new List<API_ListShare>();

            using (var dataMethods = new Data.DataMethods())
            {
                var listshares = dataMethods.ListShare_GetAll();
                if (listshares != null)
                {
                    foreach (var s in listshares)
                    {
                        var newItem = converter.ConvertToAPI_ListShareWithAssociatedList(s);
                        rcListShares.Add(newItem);
                    }
                }
            }

            return rcListShares.AsQueryable();
        }

        // GET api/ListShare/5
        public API_ListShare GetListShare(string id)
        {
            API_ListShare rShare = new API_ListShare();

            if (!string.IsNullOrEmpty(id))
            {
                using (var dataMethods = new Data.DataMethods())
                {
                    var listShare = dataMethods.ListShare_GetByPublicKeyWithAssociatedItems(id);
                    if (listShare == null)
                    {
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                    }
                    else
                    {
                        rShare = rShare.ConvertToAPI_ListShareWithAssociatedList(listShare);

                    }
                }
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return rShare;
        }

        // PUT api/ListShare/5
        // NOTE: Changed this from an input of int(listShare.Id) to string (listShare.PublickKey) - not sure 
        // what the outcome will be.  TODO: Test This
        //public HttpResponseMessage PutListShare(string id, ListShare listshare)
        //{
        //    if (ModelState.IsValid && id == listshare.PublicKey)
        //    {
        //        db.ListShares.Attach(listshare);
        //        db.ObjectStateManager.ChangeObjectState(listshare, System.Data.EntityState.Modified);

        //        try
        //        {
        //            db.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}

        // POST api/ListShare
        public HttpResponseMessage PostListShare(API_ListShare listshare)
        {
            if ((ModelState.IsValid) && ((listshare.ListPublicKey != string.Empty) && (listshare.ConsumerPublicKey != string.Empty)))
            {
                using (var dataMethods = new Data.DataMethods())
                {
                    var newListShare = dataMethods.ListShare_Create(listshare.ListPublicKey, listshare.ConsumerPublicKey);
                    API_ListShare converter = new API_ListShare();
                    listshare = converter.ConvertToAPI_ListShareWithAssociatedList(newListShare);
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, listshare);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = listshare.ListPublicKey }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/ListShare/5
        public HttpResponseMessage DeleteListShare(string id)
        {
            HttpResponseMessage rMsg = new HttpResponseMessage();

            try
            {
                using (var dataMethods = new Data.DataMethods())
                {
                    var rc = dataMethods.ListShare_DeleteListShare(id);
                    if (!rc)
                    {
                        rMsg = Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        rMsg = Request.CreateResponse(HttpStatusCode.OK, id);
                    }
                }
            }
            catch (Exception)
            {
                rMsg = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return rMsg;
        }
    }
}