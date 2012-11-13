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
        private GyftoListEntities db = new GyftoListEntities();
        private DataMethods _dataMethods = new DataMethods();

        // GET api/ListShare
        public List<API_ListShare>GetListShares()
        {
            API_ListShare converter = new API_ListShare();
            //var listshares = db.ListShares.Include("List");//.Include("User");
            var listshares = _dataMethods.ListShare_GetAll();
            var rcListShares = new List<API_ListShare>();

            if (listshares != null)
            {
                foreach (var s in listshares)
                { 
                    var newItem = converter.ConvertToAPI_ListShareWithAssociatedList(s);
                    rcListShares.Add(newItem);
                }
            }

            return rcListShares;
        }

        // GET api/ListShare/5
        public API_ListShare GetListShare(string id)
        {
            API_ListShare rShare = new API_ListShare();
            var listShare = _dataMethods.ListShare_GetByPublicKeyWithAssociatedItems(id);
            
            if (listShare == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else 
            {
                rShare = rShare.ConvertToAPI_ListShareWithAssociatedList(listShare);

            }

            return rShare;
        }

        // PUT api/ListShare/5
        // NOTE: Changed this from an input of int(listShare.Id) to string (listShare.PublickKey) - not sure 
        // what the outcome will be.  TODO: Test This
        public HttpResponseMessage PutListShare(string id, ListShare listshare)
        {
            if (ModelState.IsValid && id == listshare.PublicKey)
            {
                db.ListShares.Attach(listshare);
                db.ObjectStateManager.ChangeObjectState(listshare, EntityState.Modified);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/ListShare
        public HttpResponseMessage PostListShare(ListShare listshare)
        {
            if (ModelState.IsValid)
            {
                db.ListShares.AddObject(listshare);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, listshare);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = listshare.ListShareID }));
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
                var rc = _dataMethods.ListShare_DeleteListShare(id);
                if(!rc)
                {
                    rMsg = Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    rMsg = Request.CreateResponse(HttpStatusCode.OK, id);
                }
            }
            catch (Exception)
            {
                rMsg = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return rMsg;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}