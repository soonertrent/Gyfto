using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GyftoList.Util;
using GyftoList.Data;

namespace GyftoList.ConsoleAppTester
{
    class Program
    {
        private static int _iter = 1000;
        private static DataMethods gData;// = new DataMethods();//("GyftoListEntities");


        static void Main(string[] args)
        {
            var utilSecurity = new Util.Security();
            var utilUser = new Util.User();

            var kelliShare = utilUser.GenerateUserPublicKey();
            var momShare = utilUser.GenerateUserPublicKey();
            var sharonShare = utilUser.GenerateUserPublicKey();
            var amandaShare = utilUser.GenerateUserPublicKey();

            //var salt = Convert.ToString(54);
            //var password = "stussy7";
            //var hash = utilSecurity.CreateSaltedPassword(salt, password, _iter);

            //var result = utilSecurity.CompareSaltedPasswords(salt, password, hash, _iter);

            //var len = hash.Length;

            //var utilUser = new Util.User();

            //var uuid = utilUser.GenerateUserPublicKey();

            //var rc = CreateEmailAddress();
            //var count = QueryEmailAddresses();
            //var user = CreateUserAccount();

            //Item newItem = CreateListItem();
            UpdateListItemOrdinal("d8d4c3d0", 14);

        }

        static private void UpdateListItemOrdinal(string publicKey, int newOrdinal)
        {
            gData = new DataMethods();
            var rc = gData.ListItem_ReorderItem(publicKey, newOrdinal);
        }

        static private Item CreateListItem()
        {
            gData = new DataMethods();

            string listPublicKey = "49c8f932";
            string itemTitle = "Kimi Raikkonen T-Shirt";
            string itemDescription = "Size - L, color - Blue";
            string itemURL = "http://www.shotdeadinthehead.com/kimi-raikkonen-2-t-shirt-mens.html";
            string itemImageURL = "http://static2.shotdeadinthehead.co.uk/media/catalog/product/cache/7/thumbnail/80x/0dc2d03fe217f8c83829496872af24a0/i/m/image_16353_1_195315_1_22450.jpg";
            decimal itemCost = 27.99m;
            int itemOrdinal = GetItemsForList(listPublicKey) + 1;
            
            return gData.ListItem_Create(listPublicKey, itemTitle, itemDescription, itemCost, itemOrdinal, itemImageURL, itemURL);

        }

        static private void CreateTestUserAccounts()
        {
            var utilSecurity = new Util.Security();
            string[,] usrList = new string[4, 4] {
                {"Kelli","Trent","password", "kellitrent@gmail.com"}
                ,{"Rhonda","Trent","password","trent_rhonda@yahoo.com"}
                ,{"Sharon","Peddy","password","sharonannette52@yahoo.com"}
                ,{"Amanda","Weaver","password","mander.weaver@gmail.com"}
            };

            for (int i = 0; i != 4; i++)
            {
                var salt = Convert.ToString(54);
                var password = usrList[i, 2].ToString();
                var hash = utilSecurity.CreateSaltedPassword(salt, password, _iter);

                var newUsr = CreateUserAccount(usrList[i, 0].ToString(), usrList[i, 1].ToString(), hash);
                if (newUsr != null)
                {
                    CreateEmailAddress(newUsr.UserID, usrList[i, 3].ToString());
                }
            }
        }

        static private void GetUsers()
        {
        }

        static private int GetItemsForList(string listPublicKey)
        {
            gData = new DataMethods();
            return gData.ListItem_GetAllByListPublicKey(listPublicKey).Count();
        }

        static private int QueryEmailAddresses()
        {
            var rc = 0;

            using (gData = new DataMethods())
            {
                gData.User_EmailCount("soonertrent@gmail.com");
            }

            return rc;
        }

        static private GyftoList.Data.User CreateUserAccount(string fName, string lName, string password)
        {
            GyftoList.Data.User rc;

            using (gData = new DataMethods())
            {
                rc = gData.User_CreateUser(fName, lName, password);
            }

            return rc;
        }

        static private bool CreateEmailAddress(int userId, string emailAddress)
        {
            var rc = false;
            
            using(gData = new DataMethods())
            {
                gData.User_CreateEmail(userId, emailAddress);
            }

            return rc;
        }
    }
}
