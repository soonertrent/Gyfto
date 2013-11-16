using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GyftoList.Data;

namespace GyftoList.API.Translations
{
    public class API_ListShare
    {
        #region Constructors

        public API_ListShare(){}

        #endregion

        #region  Private Members

        private string _publicKey = string.Empty;
        private string _consumerPublicKey = string.Empty;
        private string _ownerPublicKey = string.Empty;
        private string _consumerDisplayName = string.Empty;
        private string _ownerDisplayName = string.Empty;
        private string _listPublicKey = string.Empty;
        private API_List _sharedList = new API_List();

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

        public string OwnerPublicKey
        {
            get { return _ownerPublicKey; }
            set { _ownerPublicKey = value; }
        }

        public string OwnerDisplayName
        {
            get { return _ownerDisplayName; }
            set { _ownerDisplayName = value; }
        }

        public string ListPublicKey
        {
            get { return _listPublicKey; }
            set { _listPublicKey = value; }

        }

        public API_List SharedList
        {
            get { return _sharedList; }
            set { _sharedList = value; }
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// This will convert the internal ListShare to a API version with the associated List object
        /// </summary>
        /// <param name="listShare"></param>
        /// <returns></returns>
        public API_ListShare ConvertToAPI_ListShareWithAssociatedList(GyftoList.Data.ListShare listShare)
        {
            var apiShare = GetShellListShare(listShare);
            var apiList = new API_List();
            apiShare.SharedList = apiList.ConvertToAPI_ListWithAllItems(listShare.List);

            return apiShare;
        }

        /// <summary>
        /// This will convert the internal ListShare to a API version with the associated List object
        /// </summary>
        /// <param name="listShare"></param>
        /// <returns></returns>
        public API_ListShare ConvertToAPI_ListShareWithoutAssociatedList(GyftoList.Data.ListShare listShare)
        {
            var apiShare = GetShellListShare(listShare);
            var apiList = new API_List();

            return apiShare;
        }

        public ListShare ConvertFromAPI_ListShare(API_ListShare listShare)
        {
            using(var dataMethods = new DataMethods())
            {
                return new ListShare() { 
                    List = dataMethods.List_GetListByPublicKey(listShare.SharedList.PublicKey)
                    ,UserConsumer = dataMethods.User_GetUser(listShare.ConsumerPublicKey)
                    ,UserOwner = dataMethods.User_GetUser(listShare.OwnerPublicKey)
                };
            }
        }

        /// <summary>
        /// This builds a base/shell API_ListShare object
        /// </summary>
        /// <param name="listShare"></param>
        /// <returns></returns>
        private API_ListShare GetShellListShare(ListShare listShare)
        {
            var apiShare = new API_ListShare();
            var apiUsr = new API_User();
            var apiList = new API_List();
            DataMethods _dataMethods = new DataMethods();
            var uConsumerObj = _dataMethods.User_GetUser(listShare.ConsumerID);
            var oConsumerObj = _dataMethods.User_GetUser(listShare.OwnerID);

            var uConsumer = apiUsr.ConvertToAPI_UserWithoutAssociatedLists(uConsumerObj);
            var uOwner = apiUsr.ConvertToAPI_UserWithoutAssociatedLists(oConsumerObj);

            apiShare.PublicKey = listShare.PublicKey;
            apiShare.ConsumerPublicKey = uConsumer.PublicKey;
            apiShare.OwnerPublicKey = uOwner.PublicKey;
            apiShare.ConsumerDisplayName = uConsumer.DisplayName;
            apiShare.OwnerDisplayName = uOwner.DisplayName;

            return apiShare;
        }

        #endregion

    }
}