using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GyftoList.Data
{
    public class DataMethods : IDisposable
    {
        #region Private Parameters

        private GyftoListEntities _gyftoListEntities;

        #endregion

        #region Constructors

        public DataMethods()
        {
            _gyftoListEntities = new GyftoListEntities();
        }

        public DataMethods(string connStringName)
        {
            _gyftoListEntities = new GyftoListEntities(connStringName);
        }

        #endregion

        #region Public Methods

        public User User_CreateUser(string fName, string lName, string password)
        {
            User rc;

            using (_gyftoListEntities)
            {
                var newEntry = new User
                {
                    FName = fName,
                    LName = lName,
                    Password = password
                };

                _gyftoListEntities.Users.AddObject(newEntry);
                _gyftoListEntities.SaveChanges();

                rc = newEntry;
            }

            return rc;
        }

        public int User_CheckEmail(string emailAddress)
        {
            var rc = 0;

            using (_gyftoListEntities)
            {
                var addresses = from x in _gyftoListEntities.EmailAddresses
                     where x.EmailAddress1 == emailAddress
                     select x.EmailAddress1;

                rc = addresses.Count();
            }

            return rc;
        }

        public bool User_CreateEmail(int userID, string emailAddress)
        {
            var rc = false;

            using (_gyftoListEntities)
            {
                var newEntry = new EmailAddress { 
                    EmailAddress1 = emailAddress,
                    UserID = userID
                };

                _gyftoListEntities.EmailAddresses.AddObject(newEntry);
                _gyftoListEntities.SaveChanges();
            }

            return rc;
        }

        public void Dispose()
        {
            _gyftoListEntities = null;
        }

        #endregion


    }
}
