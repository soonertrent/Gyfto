using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GyftoList.Data;

namespace GyftoList.API.Translations
{
    public class API_List
    {
        #region Constructors
        public API_List(){ }
        #endregion

        #region Private Members

        private string _listName = string.Empty;
        private string _description = string.Empty;
        private string _publicKey = string.Empty;
        private string _userPublicKey = string.Empty;
        private List<API_ListItem> _items = new List<API_ListItem>();
        private List<API_ListShare> _listShares = new List<API_ListShare>();

        #endregion

        #region Public Members

        public string ListName
        {
            get { return _listName; }
            set { _listName = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        public string UserPublicKey
        {
            get { return _userPublicKey; }
            set { _userPublicKey = value; }
        }

        public List<API_ListItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public List<API_ListShare> ListShares
        {
            get { return _listShares; }
            set { _listShares = value; }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// This method converts a List to an API_List object.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public API_List ConvertToAPI_ListWithItems(List list)
        {
            var rcList = _CreateShellList(list);
            try
            {
                if ((rcList != null) && (list.Items != null) && (list.Items.Count > 0))
                {
                    API_ListItem apiListItem = new API_ListItem();
                    foreach (var i in list.Items.OrderBy(li => li.Ordinal))
                    {
                        rcList.Items.Add(apiListItem.ConvertToAPI_ListItem(i, rcList.PublicKey));
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                
            }
            

            return rcList;
        }

        /// <summary>
        /// Creates an API_List object without associated List Items
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public API_List ConvertToAPI_ListWithoutItems(GyftoList.Data.List list)
        {
            var rcList = _CreateShellList(list);

            return rcList;
        }

        /// <summary>
        /// Creates a new shell API_List object.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private API_List _CreateShellList(GyftoList.Data.List list)
        {
            return new API_List { PublicKey = list.PublicKey, Description = list.Description, ListName = list.Title, UserPublicKey = list.User.PublicKey };

        }

        #endregion

    }
}