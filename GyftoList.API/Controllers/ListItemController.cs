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
    public class ListItemController : ApiController
    {
        private GyftoListEntities db = new GyftoListEntities();
        private DataMethods _dataMethods = new DataMethods();

        // GET api/ListItem
        public IEnumerable<Item> GetItems()
        {
            var items = db.Items.Include("List");
            return items.AsEnumerable();
        }

        // GET api/ListItem/5
        public API_ListItem GetItem(string id)
        {
            API_ListItem returnItem = new API_ListItem();
            Item item = db.Items.Single(i => i.PublicKey == id);
            if (item == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                returnItem = returnItem.ConvertToAPI_ListItem(item, item.List.PublicKey);
            }

            return returnItem;
        }

        // PUT api/ListItem/5
        public HttpResponseMessage PutItem(string id, Item item)
        {
            if (ModelState.IsValid && id == item.PublicKey)
            {
                db.Items.Attach(item);
                db.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);

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

        // POST api/ListItem
        public HttpResponseMessage PostItem(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.AddObject(item);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, item);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = item.ItemID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/ListItem/5
        public HttpResponseMessage DeleteItem(string id)
        {
            HttpResponseMessage rMsg = new HttpResponseMessage();

            try
            {
                var rc = _dataMethods.ListItem_Delete(id);
                if (!rc)
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