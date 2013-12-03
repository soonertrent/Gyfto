using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GyftoList.Data;

namespace GyftoList.API.Translations
{
    public class API_ItemShare
    {
         #region Constructors

        public API_ItemShare() { }

        #endregion

        #region  Private Members

        private string _publicKey = string.Empty;
        private string _consumerPublicKey = string.Empty;
        private string _consumerDisplayName = string.Empty;
        private string _consumerAvatarURL = string.Empty;
        private string _ownerPublicKey = string.Empty;
        private string _ownerDisplayName = string.Empty;
        private string _listSharePublicKey = string.Empty;
        private string _listPublicKey = string.Empty;
        private string _itemPublicKey = string.Empty;

        #endregion

        #region Public Members

        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        public string ConsumerPublicKey
        {
            get { return _consumerPublicKey; }
            set { _consumerPublicKey = value; }
        }

        public string ConsumerDisplayName
        {
            get { return _consumerDisplayName; }
            set { _consumerDisplayName = value; }
        }

        public string ConsumerAvatarURL
        {
            get { return _consumerAvatarURL; }
            set { _consumerAvatarURL = value; }
        }

        //public string OwnerPublicKey
        //{
        //    get { return _ownerPublicKey; }
        //    set { _ownerPublicKey = value; }
        //}

        //public string OwnerDisplayName
        //{
        //    get { return _ownerDisplayName; }
        //    set { _ownerDisplayName = value; }
        //}

        //public string ListPublicKey
        //{
        //    get { return _listPublicKey; }
        //    set { _listPublicKey = value; }

        //}

        public string ListSharePublicKey
        {
            get { return _listSharePublicKey; }
            set { _listSharePublicKey = value; }

        }

        public string ItemPublicKey
        {
            get { return _itemPublicKey; }
            set { _itemPublicKey = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This will convert the internal ItemShare to a API version
        /// </summary>
        /// <param name="itemShare"></param>
        /// <param name="listShare"></param>
        /// <param name="consumer"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public API_ItemShare ConvertToAPI_ItemShare(GyftoList.Data.ItemShare itemShare, GyftoList.Data.ListShare listShare, GyftoList.Data.User consumer, GyftoList.Data.Item item)
        {
            API_ItemShare rcItemShare;

            try 
	        {	        
		        rcItemShare = new API_ItemShare();
                rcItemShare.PublicKey = itemShare.PublicKey;
                rcItemShare.ItemPublicKey = item.PublicKey;
                rcItemShare.ConsumerAvatarURL = consumer.AvatarURL;
                rcItemShare.ConsumerPublicKey = consumer.PublicKey;
                rcItemShare.ConsumerDisplayName = string.Format("{0} {1}",consumer.FName,consumer.LName.Substring(0,1));
                rcItemShare.ListSharePublicKey = listShare.PublicKey;
	        }
	        catch (Exception ex)
	        {
			        throw ex;
	        }

            return rcItemShare;
        }

        #endregion
    }
}