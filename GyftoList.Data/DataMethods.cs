using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Text;
using GyftoList.Util;

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

        public bool ItemExclusion_Create(string listSharePublicKey, string itemPublicKey)
        {
            var rc = false;

            // First, make sure  the ListShare and Item exist
            if((_gyftoListEntities.ListShares.Where(ls => ls.PublicKey == listSharePublicKey).Count() < 1)
                || (_gyftoListEntities.Items.Where(i => i.PublicKey == itemPublicKey).Count() < 1))
            {
                //throw new Exception("Could not correlate business objects.");
            }
            else
            {

                var listShare = ListShare_GetByPublicKeyWithAssociatedItems(listSharePublicKey);
                var item = ListItem_GetByPublicKey(itemPublicKey);

                // Make sure we don't already have an ItemExclusion for  this
                if (_gyftoListEntities.ItemExclusions.Where(ie => ie.ListShareID == listShare.ListShareID && ie.ItemID == item.ItemID).Count() == 0)
                {
                    try
                    {
                        var newIE = new ItemExclusion
                        {
                            Item = item
                            ,CreateDate = DateTime.Now
                            ,ListShareID = listShare.ListShareID
                        };

                        _gyftoListEntities.ItemExclusions.AddObject(newIE);
                        _gyftoListEntities.SaveChanges();

                        rc = true;
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
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
            Util.User util = new Util.User();

            newList.Active = true;
            newList.CreateDate = DateTime.Now;
            newList.Description = listDescription;
            newList.Title = listTitle;
            newList.User = User_GetUser(userPublicKey);
            newList.PublicKey = util.GenerateUserPublicKey();

            _gyftoListEntities.Lists.AddObject(newList);
            _gyftoListEntities.SaveChanges();

            return newList;
        }

        public bool List_DeleteList(string listPublicKey)
        {
            var list = List_GetListByPublicKey(listPublicKey);

            //First, delete the items
            foreach (var i in list.Items)
            {
                ListItem_Delete(i.PublicKey);
            }

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
        /// Gets a List by it's PublicKey
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public List List_GetListByPublicKey(string publicKey)
        {
            return _gyftoListEntities.Lists.Include("Items").Include("User").Where(i => i.Items.Any(x => x.Active == false)).SingleOrDefault(l => l.PublicKey == publicKey);
            
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

            Item item = _gyftoListEntities.Items.Single(i => i.PublicKey == publicKey);
            if (item == null)
            {
                throw new Exception();
            }

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
        public Item ListItem_Create (string listPublicKey, string itemTitle, string itemDescription, decimal itemCost, int itemOrdinal, string itemImageURL, string itemURL)
        {
            Util.User util = new Util.User();
            Item newItem = new Item();

            newItem.Active = true;
            newItem.Cost = itemCost;
            newItem.CreateDate = DateTime.Now;
            newItem.Description = itemDescription;
            newItem.Title = itemTitle;
            newItem.Ordinal = itemOrdinal;
            newItem.ImageURL = itemImageURL;
            newItem.ItemURL = itemURL;
            newItem.PublicKey = util.GenerateUserPublicKey();
            newItem.List = List_GetListByPublicKey(listPublicKey);

            _gyftoListEntities.Items.AddObject(newItem);
            _gyftoListEntities.SaveChanges();

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
                throw;
            }

            return listItem;
        }

        public bool ListItem_ReorderItem(string publicKey, int newOrdinal)
        {
            var rc = false;

            try
            {
                // First, get the item
                var item = ListItem_GetByPublicKey(publicKey);

                // Next, get all the items after this Ordinal to update
                var _newConn = new GyftoListEntities();
                var itemsToBeUpdated = _newConn.Items.Where(i => i.Ordinal > newOrdinal);
                if ((itemsToBeUpdated != null) && (itemsToBeUpdated.Count() > 0))
                {
                    //var i = 1;
                    foreach (var i in itemsToBeUpdated)
                    {
                        i.Ordinal = i.Ordinal + 1;
                        _newConn.SaveChanges();
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
        /// This returns all the Items for a given List
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <returns></returns>
        public List<Item> ListItem_GetAllByListPublicKey(string listPublicKey)
        {
            return _gyftoListEntities.Items.Where(i => i.List.PublicKey == listPublicKey).ToList();
        }

        /// <summary>
        /// Returns a single Item
        /// </summary>
        /// <param name="itemPublicKey"></param>
        /// <returns></returns>
        public Item ListItem_GetByPublicKey(string itemPublicKey)
        {
            return _gyftoListEntities.Items.SingleOrDefault(i => i.PublicKey == itemPublicKey);
        }

        //public

        #endregion

        #region ListShare

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
        /// Creates a new ListShare for the provided List and Consumer
        /// </summary>
        /// <param name="listPublicKey"></param>
        /// <param name="consumerPublicKey"></param>
        /// <returns></returns>
        public ListShare ListShare_Create(string listPublicKey, string consumerPublicKey)
        {
            Util.User util = new Util.User();
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
                newListShare.PublicKey = util.GenerateUserPublicKey();

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

            using (_gyftoListEntities = new GyftoListEntities())
            {

                List usrList = _gyftoListEntities.Lists.SingleOrDefault(l => l.PublicKey == listPublicKey);
                rcListShare = _gyftoListEntities.ListShares.Where(ls => ls.ListID == usrList.ListID).ToList();

            }
            return rcListShare;
        }

        /// <summary>
        /// Gets all the current List Shares
        /// </summary>
        /// <returns></returns>
        public List<ListShare> ListShare_GetAll()
        {
            return _gyftoListEntities.ListShares.Include("List").ToList();
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
        public User User_CreateUser(string fName, string lName, string password)
        {
            User rc;
            Util.User usrUtil = new Util.User();

            var newEntry = new User
            {
                FName = fName,
                LName = lName,
                Password = password,
                PublicKey = usrUtil.GenerateUserPublicKey()
            };

            _gyftoListEntities.Users.AddObject(newEntry);
            _gyftoListEntities.SaveChanges();

            rc = newEntry;

            return rc;
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
        public bool User_CreateEmail(int userID, string emailAddress)
        {
            var rc = false;

            var newEntry = new EmailAddress
            {
                EmailAddress1 = emailAddress,
                UserID = userID
            };

            _gyftoListEntities.EmailAddresses.AddObject(newEntry);
            _gyftoListEntities.SaveChanges();

            return rc;
        }

        /// <summary>
        /// Get a Gyfto User by Public Key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public User User_GetUser(string publicKey)
        {
            return _gyftoListEntities.Users.Include("EmailAddresses").Include("Lists").SingleOrDefault(u => u.PublicKey == publicKey);;
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
        public IQueryable<User> User_GetAllUsers()
        {
            return _gyftoListEntities.Users;
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
