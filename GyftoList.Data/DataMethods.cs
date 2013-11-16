using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Text;
using GyftoList.Util;
using System.Configuration;

namespace GyftoList.Data
{
    public class DataMethods : IDisposable
    {
        #region Private Members

        private GyftoListEntities _gyftoListEntities = new GyftoListEntities();

        #endregion

        #region Constructors

        public DataMethods()
        {
            if (_gyftoListEntities == null)
            {
                _gyftoListEntities = new GyftoListEntities();
            }
        }

        public DataMethods(string connStringName)
        {
            _gyftoListEntities = new GyftoListEntities(connStringName);
        }

        #endregion

        #region Public Methods

        #region Utility Methods
        /// <summary>
        /// This method will return a unique key to be utilized within Gyfto
        /// </summary>
        /// <returns></returns>
        public string GeneratePublicKey()
        {
            return new Util.User().GenerateUserPublicKey();
        }

        /// <summary>
        /// Returns a salted password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string CreateSaltedPassword(string password)
        {
            var utilSecurity = new Util.Security();
            var salt = Convert.ToString(Convert.ToInt32(ConfigurationSettings.AppSettings["Salt"].ToString()));
            var userToken = "CAAFbwB9jvhUBAFGwwcvEzYRGtSIVis7RO9dScMbZBbyHZCKjyAY1Q5Fr3i90TbNTlyQyf2W4MWsfsZCuppQ1ZBJFprKBkWB5hLJNk2njlecCA5J4xhiTaoSaPAdxiRnsFujPmHsqrPAH169JDEAd3Q8gxFen1FDp0jCIk6J25ukJzIPMkE5znTBcavYMYYbEvo27F3Ad2QZDZD";
            return  utilSecurity.CreateSaltedPassword(salt, userToken, Convert.ToInt32(ConfigurationSettings.AppSettings["SaltIteration"].ToString()));
        }

        #endregion

        #region ItemExclusion

        /// <summary>
        /// Gets all ItemExclusions for a given ListShare
        /// </summary>
        /// <param name="listShareID"></param>
        /// <returns></returns>
        public List<ItemExclusion> ItemExclusion_GetAllByListShareID(int listShareID)
        {
            return _gyftoListEntities.ItemExclusions.Where(ie => ie.ListShareID == listShareID).ToList();
        }

        /// <summary>
        /// Return a collection of List Item Exclusions by List Share Public Key
        /// </summary>
        /// <param name="publicKey">List Share Public Key</param>
        /// <returns></returns>
        public List<ItemExclusion> ItemExclusion_GetAllByListShare(string publicKey)
        {
            //TODO: Test by attempting to get all the Item Exclusions for a know List Share
            var rcItemExclusions = new List<ItemExclusion>();
            
            //Get the List Share, then iterate across all the Items and then all the associated Item Exclusions
            var listShare = ListShare_GetByPublicKeyWithAssociatedItems(publicKey);
            return _gyftoListEntities.ItemExclusions.Where(i => i.ListShareID == listShare.ListShareID).ToList();
        }

        /// <summary>
        /// Creates a List Item Exclusion which will hide this List Item
        /// from Consumers on a Shared List
        /// </summary>
        /// <param name="listSharePublicKey"></param>
        /// <param name="itemPublicKey"></param>
        /// <returns></returns>
        public ItemExclusion ItemExclusion_Create(int listShareID, int itemID)
        {
            ItemExclusion rcItemExclusion = new ItemExclusion();
            // First, make sure  the ListShare and Item exist
            if ((listShareID != null) && (itemID != null ))
            {
                // Make sure we don't already have an ItemExclusion for this
                if (ItemExclusion_Count(listShareID, itemID) == 0)
                {
                    try
                    {
                        rcItemExclusion.ItemID = itemID;
                        rcItemExclusion.CreateDate = DateTime.Now;
                        rcItemExclusion.ListShareID = listShareID;
                        rcItemExclusion.PublicKey = GeneratePublicKey();

                        _gyftoListEntities.ItemExclusions.AddObject(rcItemExclusion);
                        _gyftoListEntities.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Unable to create List Item Exclusion '{0}'", e.ToString()));
                    }
                }
            }
            else
            {
                throw new Exception("Unable to create Item Exclusion - either List Share Public Key or Item Public Key not provided");
            }

            return rcItemExclusion;
        }

        /// <summary>
        /// Returns a count of the Item Exclusions for a given list
        /// </summary>
        /// <param name="listShareID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int ItemExclusion_Count(int listShareID, int itemID)
        {
            return _gyftoListEntities.ItemExclusions.Where(ie => ie.ListShareID == listShareID && ie.ItemID == itemID).Count();
        }

        /// <summary>
        /// Deletes a List Item Exclusion
        /// </summary>
        /// <param name="itemExclusionPublicKey"></param>
        /// <returns></returns>
        public bool ItemExclusion_Delete(string itemExclusionPublicKey)
        {
            var rc = false;
            var itemExclusion = ItemExclusion_GetByPublicKey(itemExclusionPublicKey);

            if (itemExclusion != null)
            {
                //Delete the Item Exclusion
                _gyftoListEntities.ItemExclusions.DeleteObject(itemExclusion);

                try
                {
                    _gyftoListEntities.SaveChanges();
                    rc = true;
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Unable to delete Item Exclusion '{0}' - '{1}'", itemExclusionPublicKey, e.InnerException.ToString()));
                }
            }
            else
            {
                throw new Exception("Unabel to delete Item Exclusion - Missing Item Exclusion Public Key");
            }

            return rc;
        }

        /// <summary>
        /// Deletes all the Item Exclusions for a given List Share 
        /// </summary>
        /// <param name="listSharePublicKey"></param>
        /// <returns></returns>
        public bool ItemExclusion_DeleteByListShare(string listSharePublicKey)
        {
            var rc = false;

            try
            {
                foreach (var ie in ItemExclusion_GetAllByListShare(listSharePublicKey))
                {
                    _gyftoListEntities.ItemExclusions.DeleteObject(ie);
                }

                _gyftoListEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to delte Item Exclusions for List Share with Public Key '{0}'. Exception: '{1}'",listSharePublicKey,ex.ToString()));
            }

            


            return rc;
        }

        /// <summary>
        /// Delete's an Item Exclusion by it's associated Item
        /// </summary>
        /// <param name="itemPublicKey"></param>
        /// <returns></returns>
        public bool ItemExclusion_DeleteByItem(string itemPublicKey)
        {
            var rc = false;

            try
            {
                var ie = ItemExclusion_GetByItemPublicKey(itemPublicKey);
                if (ie != null)
                {
                    _gyftoListEntities.ItemExclusions.DeleteObject(ie);
                    _gyftoListEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to delte Item Exclusions for Item with Public Key '{0}'. Exception: '{1}'", itemPublicKey, ex.ToString()));
            }

            return rc;
        }

        /// <summary>
        /// Returns an Item Exclusion by Item Exclusion Publick Key
        /// </summary>
        /// <param name="itemExclusionPublicKey"></param>
        /// <returns></returns>
        public ItemExclusion ItemExclusion_GetByPublicKey(string itemExclusionPublicKey)
        { 
            return _gyftoListEntities.ItemExclusions.Where(i => i.PublicKey == itemExclusionPublicKey).SingleOrDefault();
        }

        /// <summary>
        /// Gets an Item Exclusion by it's associated Item 
        /// </summary>
        /// <param name="itemPublicKey"></param>
        /// <returns></returns>
        public ItemExclusion ItemExclusion_GetByItemPublicKey(string itemPublicKey)
        {
            var rc = new ItemExclusion();
            var item = ListItem_GetByPublicKey(itemPublicKey);
            if (item != null)
            {
                rc = _gyftoListEntities.ItemExclusions.Where(i => i.ItemID == item.ItemID).SingleOrDefault();
            }
            else
            {
                throw new Exception(string.Format("Unable to get Item with Public Key '{0}'",itemPublicKey));
            }
            
            return rc;
        }

        #endregion

        #region List

        /// <summary>
        /// Creates a new Gyfto List
        /// </summary>
        /// <param name="listTitle"></param>
        /// <param name="listDescription"></param>
        /// <param name="userPublicKey"></param>
        /// <returns></returns>
        public List List_CreateList(string listTitle, string listDescription, string userPublicKey)
        {
            List newList = new List();

            try
            {
                newList.Active = true;
                newList.CreateDate = DateTime.Now;
                newList.Description = listDescription;
                newList.Title = listTitle;
                newList.User = User_GetUser(userPublicKey);
                newList.PublicKey = GeneratePublicKey();

                _gyftoListEntities.Lists.AddObject(newList);
                _gyftoListEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newList;
        }

        /// <summary>
        /// Delete's a List
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <returns></returns>
        public bool List_DeleteList(string listPublicKey)
        {
            var list = List_GetListByPublicKey(listPublicKey);

            //First, delete the items
            foreach (var i in list.Items)
            {
                ListItem_Delete(i.PublicKey);
            }

            //TODO: Next, delete the list

            return true;

        }

        /// <summary>
        /// Gets all Lists by a given UserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<List> List_GetListByUserID(int id)
        {
            List<List> usrList;
            using (_gyftoListEntities = new GyftoListEntities())
            {
                usrList = _gyftoListEntities.Lists.Include("Items").Include("User").Include("ListShares").Where(l => l.UserID == id).ToList();
            }
            return usrList;
        }

        /// <summary>
        /// Updates the Title/Description of a List
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <param name="newTitle"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        public List List_UpdateList(string listPublicKey, string newTitle, string newDescription)
        {
            List usrList = null;
            using (_gyftoListEntities = new GyftoListEntities())
            {
                try
                {
                    usrList = _gyftoListEntities.Lists.Where(l => l.PublicKey == listPublicKey).FirstOrDefault();

                    if (usrList != null)
                    {
                        usrList.Title = newTitle;
                        usrList.Description = newDescription;

                        _gyftoListEntities.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            

            return usrList;
        }

        /// <summary>
        /// Gets a List by it's PublicKey
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public List List_GetListByPublicKey(string publicKey)
        {
            // NOT SURE WHY THE .Items.Any LAMBDA WAS APPLIED - CAN'T SEE A DIFFERENCE IN RESULT
            //return _gyftoListEntities.Lists.Include("Items").Include("User").Where(i => i.Items.Any(x => x.Active == true)).SingleOrDefault(l => l.PublicKey == publicKey);
            return _gyftoListEntities.Lists.Include("Items").Include("User").SingleOrDefault(l => l.PublicKey == publicKey);
            
        }

        /// <summary>
        /// Gets all current Gyfto Lists for all Users
        /// </summary>
        /// <returns></returns>
        public List<List> List_GetAllLists()
        {
            return _gyftoListEntities.Lists.Include("User").Include("Items").ToList();
        }

        #endregion

        #region ListItem

        

        /// <summary>
        /// This wil delete the  associated ListItem
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public bool ListItem_Delete(string publicKey)
        {
            var rc = false;

            //First, get the item in question
            Item item = _gyftoListEntities.Items.Single(i => i.PublicKey == publicKey);
            if (item == null)
            {
                throw new Exception("Unable to retrieve ListItem (" + publicKey + ").");
            }

            //Next, we've got clean up Item Exclusions associated to the List Item
            ItemExclusion_DeleteByItem(publicKey);

            //Implement deleting of Item Tags once that has been turned on

            //Finnaly, delete the List Item
            _gyftoListEntities.Items.DeleteObject(item);

            try
            {
                _gyftoListEntities.SaveChanges();
                rc = true;
            }
            catch (Exception)
            {
                throw;
            }
            //TODO: Sort this out
            //catch (DbUpdateConcurrencyException)
            //{
            //    return Request.CreateResponse(HttpStatusCode.NotFound);
            //}

            return rc;
        }

        /// <summary>
        /// Creates a ListItem
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <param name="itemTitle"></param>
        /// <param name="itemDescription"></param>
        /// <param name="itemCost"></param>
        /// <param name="itemOrdinal"></param>
        /// <param name="itemImageURL"></param>
        /// <param name="itemURL"></param>
        /// <returns></returns>
        public Item ListItem_Create (string listPublicKey, string itemTitle, string itemDescription, decimal? itemCost, decimal? itemCostRangeStart, decimal? itemCostRangeEnd, string itemSize, string itemColor, short? itemQty, int? itemOrdinal, string itemImageURL, string itemURL)
        {
            Item newItem = new Item();

            try
            {
                newItem.Active = true;
                newItem.Cost = itemCost;
                newItem.CreateDate = DateTime.Now;
                newItem.Description = itemDescription;
                newItem.Title = itemTitle;
                newItem.Ordinal = itemOrdinal;
                newItem.ImageURL = itemImageURL;
                newItem.ItemURL = itemURL;
                newItem.PublicKey = GeneratePublicKey();
                newItem.List = List_GetListByPublicKey(listPublicKey);
                if (itemSize != null)
                {
                    newItem.Size = itemSize;
                }

                if (itemColor != null)
                {
                    newItem.Color = itemColor;
                }

                if (itemQty.HasValue)
                {
                    newItem.Qty = itemQty.Value;
                }

                if (itemOrdinal.HasValue)
                {
                    newItem.Ordinal = itemOrdinal;
                }
                else
                {
                    newItem.Ordinal = ListItem_GetHighestOrdinal(listPublicKey) + 1;
                }

                if (itemCostRangeStart.HasValue)
                {
                    newItem.CostRangeStart = itemCostRangeStart;
                }

                if (itemCostRangeEnd.HasValue)
                {
                    newItem.CostRangeEnd = itemCostRangeEnd;
                }

                _gyftoListEntities.Items.AddObject(newItem);
                _gyftoListEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to save Item '{0}' - Error: '{1}'",itemTitle,ex.InnerException.ToString())); ;
            }

            return newItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public Item ListItem_Create(string listPublicKey, Item newItem)
        {
            try
            {
                newItem.Active = true;
                newItem.PublicKey = GeneratePublicKey();
                newItem.List = List_GetListByPublicKey(listPublicKey);
                newItem.CreateDate = DateTime.Now;
                if (!newItem.Ordinal.HasValue)
                {
                    newItem.Ordinal = ListItem_GetHighestOrdinal(listPublicKey) + 1;
                }

                _gyftoListEntities.Items.AddObject(newItem);
                _gyftoListEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to save Item '{0}' - Error: '{1}'", newItem.Title, ex.InnerException.ToString())); ;
            }

            return newItem;
        }

        /// <summary>
        /// Sets the Item to  either Unobtained/Active or Obtained/Inactive
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public Item ListItem_UpdateActive(Item listItem, bool active)
        {
            var rc = false;

            try
            {
                if (listItem != null)
                {
                    listItem.Active = active;
                    _gyftoListEntities.SaveChanges();
                }
                rc = true;
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Unable to set Active Flag on List Item '{1}'",listItem.PublicKey)) ;
            }

            return listItem;
        }

        /// <summary>
        /// Update's the Active flag of a List Item
        /// </summary>
        /// <param name="itemPublicKey"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public Item ListItem_UpdateActiveByItemPublicKey(string itemPublicKey, bool active)
        {
            Item updatedItem = null;

            try
            {
                using (_gyftoListEntities = new GyftoListEntities())
                {
                    updatedItem = _gyftoListEntities.Items.Where(i => i.PublicKey == itemPublicKey).FirstOrDefault();
                    if (updatedItem != null)
                    {
                        updatedItem.Active = active;
                        _gyftoListEntities.SaveChanges();
                    }
                    else
                    {
                        throw new Exception(string.Format("Unable to locate List Item '{0}'", itemPublicKey));
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Unable to update List Item '{0}' - Error: '{1}'", itemPublicKey, ex.InnerException.ToString()));
            }

            return updatedItem;
        }

        /// <summary>
        /// Updates a List Item
        /// </summary>
        /// <param name="updatedItem"></param>
        /// <returns></returns>
        public Item ListItem_UpdateItem(Item updatedItem, string itemPublicKey)
        {
            try
            {
                if (updatedItem != null)
                {
                    using (_gyftoListEntities = new GyftoListEntities())
                    {
                        var itemToDetach = _gyftoListEntities.Items.Where(i => i.PublicKey == itemPublicKey).FirstOrDefault();
                        _gyftoListEntities.Detach(itemToDetach);
                        _gyftoListEntities.AttachTo("Items", updatedItem);
                        _gyftoListEntities.ObjectStateManager.ChangeObjectState(updatedItem, EntityState.Modified);
                        _gyftoListEntities.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception(string.Format("Unable to Locate List Item '{0}'", updatedItem.PublicKey));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to Update List Item '{0}' - Error: '{1}'", updatedItem.PublicKey, ex.InnerException.ToString()));
            }
            return updatedItem;
        }

        /// <summary>
        /// Reorders a List Item in a List
        /// </summary>
        /// <param name="listItemPublicKey"></param>
        /// <param name="newOrdinal"></param>
        /// <returns></returns>
        public bool ListItem_ReorderItem(string listItemPublicKey, int newOrdinal)
        {
            var rc = false;

            try
            {
                // First, get the item that is being updated
                var item = ListItem_GetByPublicKey(listItemPublicKey);

                // Next, get all the items after this Ordinal to update
                var itemsToBeUpdated = ListItem_GetActiveByListPublicKey(item.List.PublicKey).Where(i => i.Ordinal >= newOrdinal && i.Active == true).OrderBy(i => i.Ordinal);

                
                if ((itemsToBeUpdated != null) && (itemsToBeUpdated.Count() > 0))
                {
                    foreach (var i in itemsToBeUpdated)
                    {
                        i.Ordinal = i.Ordinal + 1;
                    }
                }

                // Last, update the item with the new  Ordinal
                item.Ordinal = newOrdinal;
                _gyftoListEntities.SaveChanges();

                rc = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rc;
        }

        /// <summary>
        /// Reorders a List Item in List by Display Ordinals
        /// </summary>
        /// <param name="currentItemDisplayOrdinal"></param>
        /// <param name="replaceItemDisplayOrdinal"></param>
        /// <param name="listPublicKey"></param>
        /// <returns></returns>
        public bool ListItem_ReorderItem(int currentItemDisplayOrdinal, int replaceItemDisplayOrdinal, string listPublicKey)
        {
            var rc = false;

            try
            {
                var currentItem = ListItem_GetByActiveAndDisplayOrdinal(listPublicKey, currentItemDisplayOrdinal);

                if (currentItem != null)
                {
                    rc = ListItem_ReorderItem(currentItem.PublicKey, replaceItemDisplayOrdinal);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return rc;
        }

        /// <summary>
        /// This returns all the Items for a given List
        /// </summary>
        /// <param name="listID">Internal List ID</param>
        /// <param name="onlyActive">True = only get Active items</param>
        /// <returns></returns>
        public List<Item> ListItem_GetAllByListID(int listID, bool onlyActive)
        {
            var rcItems = new List<Item>();
            if (onlyActive)
            { 
                rcItems = _gyftoListEntities.Items.Where(i => i.ListID == listID && i.Active == true).ToList();
            }
            else
            {
                rcItems = _gyftoListEntities.Items.Where(i => i.ListID == listID).ToList();
            }

            return rcItems;
        }

        /// <summary>
        /// This returns all Items within Gyfto
        /// </summary>
        /// <returns></returns>
        public List<Item> ListItem_GetAll()
        {
            List<Item> rcListItems = new List<Item>();
            try
            {
                rcListItems = _gyftoListEntities.Items.Where(i => i.Active == true).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return rcListItems;
        }

        /// <summary>
        /// This returns all the Items for a given List
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <returns>ALL items</returns>
        public List<Item> ListItem_GetAllByListPublicKey(string listPublicKey)
        {
            return _gyftoListEntities.Items.Where(i => i.List.PublicKey  == listPublicKey).ToList();
        }

        /// <summary>
        /// This returns all the ACTIVE Items for a given List
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <returns>ACTIVE items</returns>
        public List<Item> ListItem_GetActiveByListPublicKey(string listPublicKey)
        {
            return _gyftoListEntities.Items.Where(i => i.List.PublicKey == listPublicKey && i.Active == true).ToList();
        }

        /// <summary>
        /// Returns a single Item
        /// </summary>
        /// <param name="itemPublicKey"></param>
        /// <returns></returns>
        public Item ListItem_GetByPublicKey(string itemPublicKey)
        {
            //Item returnItem = null;
            //using (_gyftoListEntities = new GyftoListEntities())
            //{
            //    try
            //    {
            //        returnItem = _gyftoListEntities.Items.Where(i => i.PublicKey == itemPublicKey).FirstOrDefault();
            //    }
            //    catch (Exception)
            //    {
                    
            //        throw;
            //    }
            //}
            
            //return returnItem;
            return _gyftoListEntities.Items.Where(i => i.PublicKey == itemPublicKey).FirstOrDefault();
        }

        /// <summary>
        /// Returns a single Item based upon how it is shown within the list
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <param name="listItemDisplayOrdinal"></param>
        /// <returns></returns>
        public Item ListItem_GetByActiveAndDisplayOrdinal(string listPublicKey, int listItemDisplayOrdinal)
        {
            var rcItem = new Item();

            var targetList = ListItem_GetActiveByListPublicKey(listPublicKey).OrderBy(i => i.Ordinal).ToList();
            if((targetList != null) && (targetList.Count >= listItemDisplayOrdinal))
            {
                rcItem = targetList[listItemDisplayOrdinal - 1];
            }

            return rcItem;
        }

        /// <summary>
        /// Returns  the highest ordinal List Item in a given List
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <returns></returns>
        public int ListItem_GetHighestOrdinal(string listPublicKey)
        {
            var returnOrdinal = (from i in _gyftoListEntities.Items
                                 where i.List.PublicKey == listPublicKey
                                 orderby i.Ordinal descending
                                 select i).FirstOrDefault();

            return Convert.ToInt32(returnOrdinal.Ordinal);
        }

        /// <summary>
        /// NOT TO BE EXPOSED TO THE API
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <returns></returns>
        public List<Item> ListItem_ReorderByOrdinalForList(string listPublicKey)
        {
            // This is the list in it's current order (by ordinal)
            var currentOrder = _gyftoListEntities.Items.Where(i => i.List.PublicKey == listPublicKey && i.Active == true).OrderBy(i => i.Ordinal).ToList();
            var cnt = 1;

            if (currentOrder != null)
            {
                foreach (var i in currentOrder)
                {
                    i.Ordinal = cnt;
                    cnt++;
                }

                _gyftoListEntities.SaveChanges();
            }

            return ListItem_GetActiveByListPublicKey(listPublicKey).OrderBy(i => i.Ordinal).ToList();
        }

        #endregion

        #region ListShare

        /// <summary>
        /// Gets a List Share with Items included
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public ListShare ListShare_GetByPublicKeyWithAssociatedItems(string publicKey)
        {
            var rcLS = new ListShare();
            rcLS = _gyftoListEntities.ListShares.Include("List").Include("UserOwner").Include("UserConsumer").SingleOrDefault(ls => ls.PublicKey == publicKey);
            
            // Need to filter out any items not available for the current ListShare
            if (rcLS != null)
            {
                rcLS.List.Items.Clear();
                var filteredItems = ListItem_GetAllByListID(rcLS.ListID, true).Where(i => !ItemExclusion_GetAllByListShareID(rcLS.ListShareID).Any(ie => ie.ItemID == i.ItemID));
                var count = filteredItems.Count();

                foreach (var li in filteredItems)
                {
                    rcLS.List.Items.Add(li);
                }
            }
            
            return rcLS;
        }

        /// <summary>
        /// Gets a List Share by Public Key (note: does not include items)
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public ListShare ListShare_GetByPublicKey(string publicKey)
        {
            return _gyftoListEntities.ListShares.Where(i => i.PublicKey == publicKey).SingleOrDefault();
        }

        /// <summary>
        /// Creates a new ListShare for the provided List and Consumer
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <param name="consumerPublicKey"></param>
        /// <returns></returns>
        public ListShare ListShare_Create(string listPublicKey, string consumerPublicKey)
        {
            ListShare newListShare = new ListShare();

            try
            {
                var list = List_GetListByPublicKey(listPublicKey);
                var userConsumer = User_GetUser(consumerPublicKey);
                var userOwner = User_GetUser(Convert.ToInt32(list.UserID));

                if ((list == null) || (userConsumer == null) || (userOwner == null))
                {
                    throw new Exception("Unable to resolve source list, user consumer or user owner");
                }

                newListShare.CreateDate = DateTime.Now;
                newListShare.UserConsumer = userConsumer;
                newListShare.UserOwner = userOwner;
                newListShare.List = list;
                newListShare.PublicKey = GeneratePublicKey();

                _gyftoListEntities.ListShares.AddObject(newListShare);
                _gyftoListEntities.SaveChanges();
            }
            catch (Exception)
            {
                
                throw;
            }

            return newListShare;
        }


        /// <summary>
        /// This will delete the assoicated ListShare
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public bool ListShare_DeleteListShare(string publicKey)
        {
            var rc = false;

            ListShare listshare = _gyftoListEntities.ListShares.Single(l => l.PublicKey == publicKey);
            if (listshare != null)
            {
                _gyftoListEntities.ListShares.DeleteObject(listshare);

                try
                {
                    _gyftoListEntities.SaveChanges();
                }
                //TODO: Figure  out where this assembly is
                //catch (DbUpdateConcurrencyException)
                //{
                //    return Request.CreateResponse(HttpStatusCode.NotFound);
                //}
                catch (Exception e)
                {
                    throw;
                }
                rc = true;
            }

            return rc;
        }

        /// <summary>
        /// Gets all the ListShares for a particular User/Consumer by their PublicKey
        /// </summary>
        /// <param name="consumerPublicKey"></param>
        /// <returns></returns>
        public List<ListShare> ListShare_GetAllListSharesForConsumer(string consumerPublicKey)
        {
            List<ListShare> listShares = new List<ListShare>();

            User usrConsumer = _gyftoListEntities.Users.Where(c => c.PublicKey == consumerPublicKey).First();
            listShares = _gyftoListEntities.ListShares.Where(ls => ls.ConsumerID == usrConsumer.UserID).ToList();

            return listShares;
        }

        /// <summary>
        /// Gets all the ListShares for a particular User/Owner by their PublicKey
        /// </summary>
        /// <param name="ownerPublicKey"></param>
        /// <returns></returns>
        public List<ListShare> ListShare_GetAllListSharesForOwner(string ownerPublicKey)
        {
            List<ListShare> listShares = new List<ListShare>();

            User usrOwner = _gyftoListEntities.Users.Where(c => c.PublicKey == ownerPublicKey).First();
            listShares = _gyftoListEntities.ListShares.Where(ls => ls.OwnerID == usrOwner.UserID).ToList();

            return listShares;
        }

        /// <summary>
        /// Gets all the ListShares for a specific List
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <returns></returns>
        public List<ListShare> ListShare_GetAllByListPublicKey(string listPublicKey)
        {
            var rcListShare = new List<ListShare>();

            //using (_gyftoListEntities = new GyftoListEntities())
            //{

                List usrList = _gyftoListEntities.Lists.SingleOrDefault(l => l.PublicKey == listPublicKey);
                rcListShare = _gyftoListEntities.ListShares.Where(ls => ls.ListID == usrList.ListID).ToList();

            //}
            return rcListShare;
        }

        /// <summary>
        /// Gets all the current List Shares
        /// </summary>
        /// <returns></returns>
        public List<ListShare> ListShare_GetAll()
        {
            List<ListShare> returnShares = null;

            try
            {
                //using (_gyftoListEntities = new GyftoListEntities())
                //{
                //    returnShares = _gyftoListEntities.ListShares.Include("List").ToList();
                //}
                returnShares = _gyftoListEntities.ListShares.Include("List").ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to retrieve List Shares - Error '{0}'", ex.InnerException.ToString()));
            }

            return returnShares;
        }

        #endregion

        #region User

        /// <summary>
        /// Creates a Gyfto User account
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="password">Assumed salted/hashed</param>
        /// <returns></returns>
        public User User_CreateUser(string fName, string lName, string password, string emailAddress, string userProviderType, int? birthYear, string avatarUrl)
        {
            //First, create a new User
            User rcNewUser;


            if (User_EmailCount(emailAddress) < 1)
            {
                try
                {
                    // Sort our the UserProvider type
                    // gyfto == 1
                    // facebook == 2

                    var providerID = Int32.MinValue;

                    switch (userProviderType.ToString().ToUpper())
                    {
                        case "GYFTO":
                            providerID = Convert.ToInt32(ConfigurationSettings.AppSettings["ProviderType_Gyfto"].ToString());
                            break;
                        case "FB":
                            providerID = providerID = Convert.ToInt32(ConfigurationSettings.AppSettings["ProviderType_Facebook"].ToString());
                            break;
                        case "FACEBOOK":
                            providerID = providerID = Convert.ToInt32(ConfigurationSettings.AppSettings["ProviderType_Facebook"].ToString());
                            break;
                        default:
                            providerID = providerID = Convert.ToInt32(ConfigurationSettings.AppSettings["ProviderType_Other"].ToString());
                            break;
                    }


                    rcNewUser = new User
                        {
                            FName = fName,
                            LName = lName,
                            Password = CreateSaltedPassword(password),
                            PublicKey = GeneratePublicKey(),
                            CreatedDate = DateTime.Now,
                            AvatarURL = avatarUrl,
                            ProviderID = providerID
                        };

                    _gyftoListEntities.Users.AddObject(rcNewUser);
                    _gyftoListEntities.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw new Exception(string.Format("Unable to create new User '{0} {1}' - '{2}'", fName, lName, ex.InnerException.ToString()));
                }
            }

            //We can't create this User b/c the email address already exists
            else
            {
                throw new Exception(string.Format("Unable to create new User '{0} {1}' with email address '{2}' as this account already exists", fName, lName, emailAddress));
            }

            try
            {
                //Next, create the Email Address associated with the User Account
                User_CreateEmail(rcNewUser.PublicKey, emailAddress);

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ex.InnerException.ToString()));
            }

            return rcNewUser;
        }

        /// <summary>
        /// Returns the count of email address matching input
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public int User_EmailCount(string emailAddress)
        {
            var rc = _gyftoListEntities.EmailAddresses.Where(e => e.EmailAddress1 == emailAddress).Count(); ;

            return rc;
        }

        /// <summary>
        /// Creates a Gyfto Email Entry
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public EmailAddress User_CreateEmail(string userPublicKey, string emailAddress)
        {
            EmailAddress rcNewEmailAddress;

            try
            {
                rcNewEmailAddress = new EmailAddress
                    {
                        EmailAddress1 = emailAddress,
                        UserID = User_GetUser(userPublicKey).UserID
                    };

                _gyftoListEntities.EmailAddresses.AddObject(rcNewEmailAddress);
                _gyftoListEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to create Email Address '{0}' for User '{1}' - {3}", emailAddress,userPublicKey, ex.InnerException.ToString()));
            }

            return rcNewEmailAddress;
        }

        /// <summary>
        /// Get a Gyfto User by Public Key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public User User_GetUser(string publicKey)
        {
            return _gyftoListEntities.Users.Include("EmailAddresses").Include("Lists").SingleOrDefault(u => u.PublicKey == publicKey);;
            //User returnUser = null;

            //try
            //{
            //    using (_gyftoListEntities = new GyftoListEntities())
            //    {
            //        returnUser = _gyftoListEntities.Users.Include("EmailAddresses").Include("Lists").SingleOrDefault(u => u.PublicKey == publicKey);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(string.Format("Unable to access User '{0}' - Error: '{1}'", publicKey, ex.InnerException.ToString()));
            //}

            //return returnUser;
        }

        /// <summary>
        /// Get a User object for a given Emmail Address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public User User_GetUserByEmail(string emailAddress)
        {
            return _gyftoListEntities.EmailAddresses.Where(e => e.EmailAddress1 == emailAddress).Select(e => e.User).FirstOrDefault();
        }

        /// <summary>
        /// Get a Gyfto User by Public Key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public User User_GetUser(int userID)
        {
            return _gyftoListEntities.Users.Include("EmailAddresses").SingleOrDefault(u => u.UserID == userID);;
        }

        /// <summary>
        /// Returns a list of Gyfto Users
        /// </summary>
        /// <returns></returns>
        public List<User> User_GetAllUsers()
        {
            //return _gyftoListEntities.Users;
            List<User> returnUsers = new List<User>();

            try
            {
                //using (_gyftoListEntities = new GyftoListEntities())
                //{
                    foreach (var usr in _gyftoListEntities.Users)
                    {
                        returnUsers.Add(usr);
                    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to return list of All Users - Error: '{0}'", ex.InnerException.ToString()));
            }

            return returnUsers;
        }

        //public bool User_DeleteUser(string publicKey)
        //{
        //    var rc = false;

        //    try
        //    {
        //        var userToDelete = User_GetUser(publicKey);
        //        _gyftoListEntities.Users.DeleteObject(userToDelete);
        //        _gyftoListEntities.SaveChanges();

        //        rc = true;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    return rc;
        //}

        /// <summary>
        /// Performs the Login of a User/Email Address (for Gyfto Provider)
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns>User Object</returns>
        public User User_Login(string emailAddress, string password)
        {
            User returnUsr = null;
            try
            {
                returnUsr = User_GetUserByEmail(emailAddress);
                var utilSecurity = new Util.Security();
                var salt = ConfigurationSettings.AppSettings["Salt"].ToString();
                var iteration = Convert.ToInt32(ConfigurationSettings.AppSettings["Salt"].ToString());
                
                // Check and see if the password salts match
                if (!utilSecurity.CompareSaltedPasswords(salt, password, returnUsr.Password, iteration))
                {
                    throw new Exception(string.Format("User '{0}' Password does not match.", emailAddress));
                }

            }
            catch (Exception)
            {
                throw new Exception(string.Format("Unable to Login User '{0}'.", emailAddress));
            }

            return returnUsr;
        }

        /// <summary>
        /// Performs the Login of a User/Email Address (for external Providers)
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public User User_LoginExternal(string userToken, string providerType)
        {
            User returnUsr = null;

            try
            {
                if ((providerType == "FB") || (providerType == "FACEBOOK"))
                {
                    // DO FACEBOOK LOGIN
                }
                else
                {
                    throw new Exception(string.Format("Unable to Login User '{0}' with Provider '{1}'.", userToken, providerType));
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return returnUsr;
        }

        #endregion

        public void Dispose()
        { 
            if (_gyftoListEntities != null)
            {
                _gyftoListEntities.Dispose();
            }
        }

        protected void Dispose(bool disposing)
        {
            _gyftoListEntities.Dispose();
        }

        #endregion


    }
}
