using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GyftoList.Data;

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
        private decimal? _costRangeStart = decimal.MinValue;
        private decimal? _costRangeEnd = decimal.MinValue;
        private string _size = string.Empty;
        private Int16? _qty = 0;
        private string _color = string.Empty;

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

        public decimal? CostRangeStart
        {
            get { return _costRangeStart; }
            set { _costRangeStart = value; }
        }

        public decimal? CostRangeEnd
        {
            get { return _costRangeEnd; }
            set { _costRangeEnd = value; }
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

        public string Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Int16? Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts "listItem" to an API_ListItem object
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public API_ListItem ConvertToAPI_ListItem(Item listItem, string listPublicKey)
        {
            return new API_ListItem { Cost = listItem.Cost, CostRangeStart = listItem.CostRangeStart, CostRangeEnd = listItem.CostRangeEnd,  Description = listItem.Description, ImageURL = listItem.ImageURL, ItemURL = listItem.ItemURL, ListPublicKey = listPublicKey, Ordinal = listItem.Ordinal, PublicKey = listItem.PublicKey, Title = listItem.Title, Active = listItem.Active, Size = listItem.Size, Qty = listItem.Qty, Color = listItem.Color };
        }

        public Item ConvertFromAPI_ListItem(API_ListItem apiListItem)
        { 
            // Create Date, Created By, 
            return new Item
            {
                PublicKey = apiListItem.PublicKey,
                Active = apiListItem.Active,
                Color = apiListItem.Color,
                Cost = apiListItem.Cost,
                CostRangeEnd = apiListItem.CostRangeEnd,
                CostRangeStart = apiListItem.CostRangeStart,
                Description = apiListItem.Description,
                ImageURL = apiListItem.ImageURL,
                ItemURL = apiListItem.ItemURL,
                Ordinal = apiListItem.Ordinal,
                Qty = apiListItem.Qty,
                Size = apiListItem.Size,
                Title = apiListItem.Title
            };
        }

        #endregion
    }
}