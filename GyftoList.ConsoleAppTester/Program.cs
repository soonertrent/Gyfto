using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GyftoList.Util;
using GyftoList.Data;

using GyftoList.Services;

namespace GyftoList.ConsoleAppTester
{
    class Program
    {
        private static int _iter = 1000;
        private static DataMethods gData;// = new DataMethods();//("GyftoListEntities");


        static void Main(string[] args)
        {
            var utilSecurity = new Util.Security();

            //var salt = Convert.ToString(54);
            //var password = "stussy7";
            //var hash = utilSecurity.CreateSaltedPassword(salt, password, _iter);

            //var result = utilSecurity.CompareSaltedPasswords(salt, password, hash, _iter);

            //var len = hash.Length;

            var utilUser = new Util.User();

            var uuid = utilUser.GenerateUserPublicKey();

            var rc = CreateEmailAddress();
            var count = QueryEmailAddresses();
            var user = CreateUserAccount();
            

        }

        static private void GetUsers()
        {
        }

        static private int QueryEmailAddresses()
        {
            var rc = 0;

            using (gData = new DataMethods())
            {
                gData.User_CheckEmail("soonertrent@gmail.com");
            }

            return rc;
        }

        static private GyftoList.Data.User CreateUserAccount()
        {
            GyftoList.Data.User rc;

            using (gData = new DataMethods())
            {
                rc = gData.User_CreateUser("Jason", "Trent", "password");
            }

            return rc;
        }

        static private bool CreateEmailAddress()
        {
            var rc = false;
            
            using(gData = new DataMethods())
            {
                gData.User_CreateEmail(1, "soonertrent@hotmail.com");
            }

            return rc;
        }
    }
}
