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
using System.Web.Http.OData.Builder;
using GyftoList.Data;
using GyftoList.API.Translations;



namespace GyftoList.API.Controllers
{
    public class ListItemController : ApiController
    {
        private API_ListItem converter = new API_ListItem();
        private DataMethods _dataMethods;

        // GET api/ListItem/GetItems
        public List<API_ListItem> GetItems()
        {
            List<API_ListItem> returnItems = new List<API_ListItem>();

            try
            {
                using (_dataMethods = new DataMethods())
                {
                    var listItems = _dataMethods.ListItem_GetAll();
                    foreach (var i in listItems)
                    {
                        returnItems.Add(converter.ConvertToAPI_ListItem(i,i.List.PublicKey));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return returnItems;
        }

        // GET api/ListItem/GetItem/5
        public API_ListItem GetItem(string id)
        {
            API_ListItem returnItem = new API_ListItem();

            using (_dataMethods = new DataMethods())
            {
                var item = _dataMethods.ListItem_GetByPublicKey(id);
                if (item == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }
                else
                {
                    returnItem = returnItem.ConvertToAPI_ListItem(item, item.List.PublicKey);
                }
            }

            return returnItem;
        }

        // GET api/ListItem/GetItemsForList/5
        [Queryable]
        public IQueryable<API_ListItem> GetItemsForList(string id)
        {
            var rcListItem = new List<API_ListItem>();
            if (id != string.Empty)
            {
                using (_dataMethods = new DataMethods())
                {
                    var list = _dataMethods.List_GetListByPublicKey(id);
                    if (list == null)
                    {
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                    }
                    else
                    {
                        // Convert each ListItem to an API_ListItem                        
                        foreach (var i in list.Items)
                        {
                            rcListItem.Add(converter.ConvertToAPI_ListItem(i, id));
                        }
                    }
                }
            }
            else 
            {
                throw new HttpException("List Public Key not Provided!");
            }

            return rcListItem.AsQueryable();
        }

        #region Deprecated
        //// PUT api/ListItem/5
        //public HttpResponseMessage PutItem(string id, Item item)
        //{
        //    if (ModelState.IsValid && id == item.PublicKey)
        //    {
        //        db.Items.Attach(item);
        //        db.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);

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
        #endregion

        // POST api/ListItem/PostItem/
        public HttpResponseMessage PostItem(API_ListItem item)
        {
            if (ModelState.IsValid)
            {
                var createdItem = new Item();

                try
                {
                    using (_dataMethods = new DataMethods())
                    { 
                        // Convert from the API to the Item
                        var listItem = converter.ConvertFromAPI_ListItem(item);
                        
                        // Create the item 
                        createdItem = _dataMethods.ListItem_Create(item.ListPublicKey,
                            listItem.Title,
                            listItem.Description,
                            listItem.Cost,
                            listItem.CostRangeStart,
                            listItem.CostRangeEnd,
                            listItem.Size,
                            listItem.Color,
                            listItem.Qty,
                            listItem.Ordinal,
                            listItem.ImageURL,
                            listItem.ItemURL);

                        // Next, create an ItemShare for the item for all the associated ListShares
                        foreach (var ls in _dataMethods.ListShare_GetAllByListPublicKey(item.ListPublicKey))
                        {
                            _dataMethods.ItemShare_Create(ls.ListShareID, item.ItemID);
                        }
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, item);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = createdItem.PublicKey }));
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

    }
}