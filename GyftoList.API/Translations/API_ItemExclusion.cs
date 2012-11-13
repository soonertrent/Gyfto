using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyftoList.API.Translations
{
    public class API_ItemExclusion
    {
        #region Constructors

        public API_ItemExclusion() { }

        #endregion

        #region Private Members

        private string _itemPublicKey = string.Empty;
        private string _listSharePublicKey = string.Empty;

        #endregion

        #region Public Members

        public string ItemPublicKey
        {
            get { return _itemPublicKey; }
            set { _itemPublicKey = value; }
        }

        public string ListSharePublicKey
        {
            get { return _listSharePublicKey; }
            set { _listSharePublicKey = value; }
        }

        #endregion
    }
}