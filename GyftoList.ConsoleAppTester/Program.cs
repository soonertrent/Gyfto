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
            //var utilSecurity = new Util.Security();
            //var utilUser = new Util.User();

            //var kelliShare = utilUser.GenerateUserPublicKey();
            //var momShare = utilUser.GenerateUserPublicKey();
            //var sharonShare = utilUser.GenerateUserPublicKey();
            //var amandaShare = utilUser.GenerateUserPublicKey();

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
            //List newList = CreateList();
            //var newListShare = CreateListShare();
            //UpdateListItemOrdinal("d8d4c3d0", 14);

            //var list = GetListByPublicKey("49c8f932");
            //var firstItem = list.Items.OrderBy(i => i.Ordinal).First();

            SeeAllLists();

        }

        static private List GetListByPublicKey(string publicKey)
        {
            gData = new DataMethods();
            return gData.List_GetListByPublicKey(publicKey);
        }

        static private void SeeAllLists()
        {
            gData = new DataMethods();
            foreach(var l in gData.List_GetAllLists())
            {
                Console.WriteLine(string.Format("{0} ({1}) with {2} items.",l.Title,l.PublicKey, l.Items.Count.ToString()));
            }

            Console.Read();
        }

        static private ListShare CreateListShare()
        {
            gData = new DataMethods();
            return gData.ListShare_Create("c6e4d6de", "15294119");
        }

        static private void UpdateListItemOrdinal(string publicKey, int newOrdinal)
        {
            gData = new DataMethods();
            var rc = gData.ListItem_ReorderItem(publicKey, newOrdinal);
        }

        static private List CreateList()
        {
            gData = new DataMethods();
            // c6e4d6de
            return gData.List_CreateList("Espresso Machines", "Different types/brands of Espresso machines by amount", "49c8f932");
        }

        static private Item CreateListItem()
        {
            gData = new DataMethods();

            string listPublicKey = "49c8f932"; // MAIN XMAS LIST
            //string listPublicKey = "c6e4d6de"; // STOCKING STUFFER LIST
            //string listPublicKey = "9634ec5"; // KELLI'S LIST
            //string listPublicKey = "1a292692"; // ESPRESSO MACHINES
            string itemTitle = "Yeti Tundra 50 Cooler";
            string itemDescription = "Crimson/White color";
            string itemURL = "http://www.academy.com/webapp/wcs/stores/servlet/Product_10151_10051_520057_-1__?color=Crimson%2fWhite&N=363067182+4294954973";
            string itemImageURL = "http://assets.academy.com/mgen/35/10221635.jpg?is=50,50";
            decimal itemCost = 359.00m;
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
