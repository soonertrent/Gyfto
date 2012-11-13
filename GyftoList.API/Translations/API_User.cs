using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GyftoList.Data;

namespace GyftoList.API.Translations
{
    public class API_User
    {
        #region Constructors
        public API_User (){}
        #endregion

        #region Private Members

        private string _displayName = string.Empty;
        private string _fName = string.Empty;
        private string _lName = string.Empty;
        private string _publickKey = string.Empty;
        private string _avatarURL = string.Empty;
        private List<API_List> _lists = new List<API_List>();
        private List<API_ListShare> _ownedListShares = new List<API_ListShare>();
        private List<API_ListShare> _consumedListShares = new List<API_ListShare>();

        #endregion


        #region Public Members

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public string FName
        {
            get { return _fName; }
            set { _fName = value; }
        }

        public string LName
        {
            get { return _lName; }
            set { _lName = value; }
        }

        public string PublicKey
        {
            get { return _publickKey; }
            set { _publickKey = value; }
        }

        public string AvatarURL
        {
            get { return _avatarURL; }
            set { _avatarURL = value; }
        }

        public List<API_List> Lists
        {
            get { return _lists; }
            set { _lists = value; }
        }

        public List<API_ListShare> OwnedListShares
        {
            get { return _ownedListShares; }
            set { _ownedListShares = value; }
        }

        public List<API_ListShare> ConsumedListShares
        {
            get { return _consumedListShares; }
            set { _consumedListShares = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts User object to API_User objeect as well as any associated Lists they own
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public API_User ConvertToAPI_UserWithAssociatedLists(User usr)
        {
            var apiUsr = GetShellUserObject(usr);

            // If Lists are there - convert them
            // TODO: This seems like a hack, not sure if there is a better way to handle this though
            try
            {
                if ((usr.Lists != null) && (usr.Lists.Count > 0))
                {
                    var apiList = new API_List();
                    foreach (var l in usr.Lists)
                    {
                        apiUsr.Lists.Add(apiList.ConvertToAPI_ListWithoutItems(l));
                    }
                }
            }
            catch (ObjectDisposedException) { }
            
            return apiUsr;
        }


        /// <summary>
        /// Converts User object to API_User objeect ignoring any associated Lists they own
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public API_User ConvertToAPI_UserWithoutAssociatedLists(User usr)
        {
            return GetShellUserObject(usr); ;
        }

        /// <summary>
        /// Creates the shell User object
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        private API_User GetShellUserObject(User usr)
        {
            var apiUsr =  new API_User { FName = usr.FName, LName = usr.LName, PublicKey = usr.PublicKey, AvatarURL = usr.AvatarURL };
            if ((usr.FName == string.Empty) && (usr.LName == string.Empty))
            {
                if (usr.EmailAddresses.Where(e => e.IsDefault == true).Count() != 1)
                {
                    apiUsr.DisplayName = "Unknown User";
                }
                else
                {
                    apiUsr.DisplayName = usr.EmailAddresses.SingleOrDefault(e => e.IsDefault == true).EmailAddress1;
                }
            }
            else
            {
                apiUsr.DisplayName = string.Format("{0} {1}", usr.FName, usr.LName.Substring(0, 1));
            }

            return apiUsr;
        }

        #endregion
    }
}