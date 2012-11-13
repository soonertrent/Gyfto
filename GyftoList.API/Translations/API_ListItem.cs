using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyftoList.API.Translations
{
    public class API_ListItem
    {
        #region Constructors

        public API_ListItem(){}

        #endregion

        #region Private Members

        private string _publicKey = string.Empty;
        private string _listPublicKey = string.Empty;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private decimal? _cost = decimal.MinValue;
        private int? _ordinal = int.MinValue;
        private bool? _active = false;
        private string _imageUrl = string.Empty;
        private string _itemUrl = string.Empty;

        #endregion

        #region Public Members

        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        public string ListPublicKey
        {
            get { return _listPublicKey; }
            set { _listPublicKey = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public decimal? Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public int? Ordinal
        {
            get { return _ordinal; }
            set { _ordinal = value; }
        }

        public bool? Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public string ImageURL
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public string ItemURL
        {
            get { return _itemUrl; }
            set { _itemUrl = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts "listItem" to an API_ListItem object
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public API_ListItem ConvertToAPI_ListItem(GyftoList.Data.Item listItem, string listPublicKey)
        {
            return new API_ListItem { Cost = listItem.Cost, Description = listItem.Description, ImageURL = listItem.ImageURL, ItemURL = listItem.ItemURL, ListPublicKey = listPublicKey, Ordinal = listItem.Ordinal, PublicKey = listItem.PublicKey, Title = listItem.Title, Active = listItem.Active };
        }

        #endregion
    }
}