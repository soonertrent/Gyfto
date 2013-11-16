using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GyftoList.Util;
using GyftoList.Data;
using GyftoList.API.Translations;
using Facebook;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace GyftoList.ConsoleAppTester
{
    class Program
    {
        private static int _iter = 1000;
        private static DataMethods gData;// = new DataMethods();//("GyftoListEntities");
        
        private static HttpClient client = new HttpClient();
           

        static void Main(string[] args)
        {
            var utilSecurity = new Util.Security();
            client.BaseAddress = new Uri("http://localhost:9000/");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //var utilUser = new Util.User();

            //var kelliShare = utilUser.GenerateUserPublicKey();
            //var momShare = utilUser.GenerateUserPublicKey();
            //var sharonShare = utilUser.GenerateUserPublicKey();
            //var amandaShare = utilUser.GenerateUserPublicKey();

            //var salt = Convert.ToString(54);
            //var password = "stussy7";
            //var userToken = "CAAFbwB9jvhUBAFGwwcvEzYRGtSIVis7RO9dScMbZBbyHZCKjyAY1Q5Fr3i90TbNTlyQyf2W4MWsfsZCuppQ1ZBJFprKBkWB5hLJNk2njlecCA5J4xhiTaoSaPAdxiRnsFujPmHsqrPAH169JDEAd3Q8gxFen1FDp0jCIk6J25ukJzIPMkE5znTBcavYMYYbEvo27F3Ad2QZDZD";
            //var hash = utilSecurity.CreateSaltedPassword(salt, userToken, _iter);

            //var result = utilSecurity.CompareSaltedPasswords(salt, password, hash, _iter);

            //var len = hash.Length;

            //var utilUser = new Util.User();

            //var uuid = utilUser.GenerateUserPublicKey();

            //var rc = CreateEmailAddress();
            //var count = QueryEmailAddresses();
            //var user = CreateUserAccount();

            var listItemPublicKey = "87a020ce";
            Console.WriteLine("Deleting List Item " + listItemPublicKey);
            Console.WriteLine(DeleteListItem(listItemPublicKey));
            Console.ReadLine();

            var listPublicKey = "49c8f932";

            //var newListItem = GenerateListItem();
            //var newItem = CreateListItem(listPublicKey, newListItem);
            
            //Console.WriteLine("Item " + newItem.Title + " added!");
            //Console.ReadLine();

            //foreach (var u in GetUsers())
            //{
            //    Console.WriteLine(string.Format("{0} {1} - {2}",u.FName, u.LName, u.PublicKey));
            //}
            //Console.ReadLine();

            //var usr = GetUserByPublicKey("49c8f932");
            //Console.ReadLine();

            //var listItems = GetListItemsForList(listPublicKey).Where(i => i.Active == true).OrderBy(i => i.Ordinal);
            //Console.WriteLine("Getting all items for " + GetListByPublicKey(listPublicKey).Title);
            //var iCnt = 1;
            //foreach (var i in listItems)
            //{
            //    Console.WriteLine(iCnt.ToString() + " - (" + i.PublicKey + ") " + i.Title + " - Ordinal: " + i.Ordinal);
            //    iCnt++;
            //}
            //Console.ReadLine();

            // Manually Update the Ordinals
            //var updatedListOrdinal = ReorderListItemsByListPublicKey(listPublicKey);

                        
            //List newList = CreateList();
            //var newListShare = CreateListShare();

            //var newUser = CreateUserAccount("Joe", "Bocardo", "mustache", "joebocardo@outlook.com", "gyfto", 1955, "http://m.c.lnkd.licdn.com/mpr/mpr/shrink_200_200/p/1/000/002/0ca/1ddccb0.jpg");
            //var usr = GetUserByEmailAddress("joebocardo@outlook.com");
            //if (usr != null)
            //{
            //    CreateEmailAddress(usr.PublicKey, "joeb@k2.com");
            //}

            //var loggedInUser = LoginUser("joebocardo@outlook.com", "mustahce");

            // GoPro - "acb62b1b" - to 2nd

            //var itemPublicKey = "22dd09fa";
            //GetListItemByPublicKey(itemPublicKey);
            //var rcMsg = UpdateListItemOrdinal(itemPublicKey, 2);

            // DELETING CURRENT LIST SHARES
            //foreach (var ls in GetListShares())
            //{
            //    Console.WriteLine(string.Format("Deleting List Share for List '{0}' - Public Key '{1}' - Consumer '{2}'", ls.List.Title, ls.PublicKey, ls.UserConsumer.FName + " " + ls.UserConsumer.LName.Substring(0, 1) + "."));
            //    var rc = DeleteListShare(ls.PublicKey);
            //    if (!rc.IsSuccessStatusCode)
            //    {
            //        throw new Exception("Issue deleting List Share!!!");
            //    }
            //}
            //Console.WriteLine("Deleted");

            // DELETING ITEM EXCLUSION
            //var foo = DeleteItemExclusion("e315da9");
            //Console.ReadLine();

            // CREATING LIST SHARES
            //var userEmailAddress = "mander.weaver@gmail.com";
            //var consumer = GetUserByEmailAddress(userEmailAddress);
            //var newListShare = new API_ListShare()
            //{
            //    ListPublicKey = listPublicKey
            //    ,
            //    ConsumerPublicKey = consumer.PublicKey
            //};
            //var createdListShare = CreateListShare(newListShare);

            // HIDE LIST SHARE ITEMS FROM 
            //var listShareForUser = GetListShares().Where(i => i.UserConsumer.PublicKey == consumer.PublicKey).FirstOrDefault();
            //if (listShareForUser != null)
            //{
            //    var itemSet = GetListItemsForList(listShareForUser.List.PublicKey).Where(i => i.Cost > 50.00m && Convert.ToBoolean(i.Active) == true);
            //    foreach (var i in itemSet)
            //    {
            //        var newItemExclusion = new API_ItemExclusion() { ListSharePublicKey = listShareForUser.PublicKey, ItemPublicKey = i.PublicKey };
            //        //var rc = CreateItemExclusion_nonAPI(newItemExclusion);
            //        var rc = CreateItemExclusion(newItemExclusion);
            //        if (rc.IsSuccessStatusCode)
            //        {
            //            Console.WriteLine("Created Item Exclusion for '{0}' in the share for '{1}'", i.Title, consumer.FName);
            //        }

            //    }
            //    Console.ReadLine();

            //}
           


            //Console.ReadLine();
            //UpdateListItemOrdinal(12, 2, listPublicKey);

            //var one = GetListItemByDisplayOrdinal(listPublicKey, 1);
            //Console.WriteLine(string.Format("List Item '{0}' - '{1}'", one.Title, 1));
            //var two = GetListItemByDisplayOrdinal(listPublicKey, 20);
            //Console.WriteLine(string.Format("List Item '{0}' - '{1}'", two.Title, 20));

            //Console.WriteLine("Item " + itemPublicKey + " updated!");
            //Console.WriteLine("Updated!");
            //Console.ReadLine();

            //var list = GetListByPublicKey("49c8f932").Items.Where(i => i.Active == true);

            //var fItem = list.First();
            //var firstItem = list.Items.OrderBy(i => i.Ordinal).First();

            //SeeAllLists();

            //var newPublickKey = new DataMethods().GeneratePublicKey();

            //var friends = GetFacebookUsers(userToken);

        }

        static private bool GetFacebookUsers(string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                var client = new FacebookClient(accessToken);
                dynamic jason = client.Get("me");
            }


            return true;
        }

        static private List GetListByPublicKey(string listPublicKey)
        {
            var gyftoApi = new API_List();
            List translatedList = null;

            var requestURI = string.Format("api/List/GetList/{0}", listPublicKey);
            HttpResponseMessage response = client.GetAsync(requestURI.ToString()).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
            else
            {
                translatedList = gyftoApi.ConvertFromAPI_List(response.Content.ReadAsAsync<API_List>().Result);
            }

            return translatedList;
        }

        static private List UpdateListByPublicKey(string publicKey, string newTitle, string newDescription)
        {
            gData = new DataMethods();
            return gData.List_UpdateList(publicKey, newTitle, newDescription);
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

        static private HttpResponseMessage CreateListShare(API_ListShare listShare)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/ListShare/PostListShare", listShare).Result; 
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            return response;
        }

        static private HttpResponseMessage UpdateListItemOrdinal(string listItemPublicKey, int newOrdinal)
        {
            //api/listitemreorder/getreorderlistitem/aac6d432?newOrdinal=1
            HttpResponseMessage response = client.GetAsync(string.Format("api/listitemreorder/getreorderlistitem/{0}?newOrdinal={1}",listItemPublicKey,newOrdinal).ToString()).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            return response;
        }

        static private void UpdateListItemOrdinal(int currentItemDisplayOrdinal, int replaceItemDisplayOrdinal, string listPublicKey)
        {
            gData = new DataMethods();
            var rc = gData.ListItem_ReorderItem(currentItemDisplayOrdinal, replaceItemDisplayOrdinal, listPublicKey);
        }

        static private List<Item> ReorderListItemsByListPublicKey(string listPublicKey)
        {
            gData = new DataMethods();
            return gData.ListItem_ReorderByOrdinalForList(listPublicKey);
        }

        static private List CreateList()
        {
            gData = new DataMethods();
            // c6e4d6de
            return gData.List_CreateList("Espresso Machines", "Different types/brands of Espresso machines by amount", "49c8f932");
        }

        static private bool DeleteListItem(string publicKey)
        {
            gData = new DataMethods();

            return gData.ListItem_Delete(publicKey);
        }

        static private Item GenerateListItem()
        { 
            return new Item(){
                Title = "Eastern Collective iPhone cable"
                , Description = "I seem to keep loosing these."
                ,
                ItemURL = "http://www.easterncollective.com/product/lightning-ronald/"
                ,
                ImageURL = "http://www.easterncollective.com/wp-content/uploads/2013/05/Lightning-Ronald-114x145.jpg"
                , Size = string.Empty
                , Color = "Red/Yellow"
                , Qty = 1
                , Cost = 23.95m
                , CostRangeStart = 0.00m
                , CostRangeEnd = 0.00m};
        }
    
        static private Item Deprecated_CreateListItem(string listPublicKey)
        {
            gData = new DataMethods();

            //string listPublicKey = "49c8f932"; // MAIN XMAS LIST
            //string listPublicKey = "c6e4d6de"; // STOCKING STUFFER LIST
            //string listPublicKey = "9634ec5"; // KELLI'S LIST
            //string listPublicKey = "1a292692"; // ESPRESSO MACHINES
            string itemTitle = "PlaySeat Challenge";
            string itemDescription = "Seat for racing/gaming console";
            string itemURL = "http://www.playseat.com/shop/us/us/playseat-challenge-seats/playseat-challenge-racing-seat.html";
            string itemImageURL = "http://www.playseat.com/page_content_files/shop_content_images/challenge-seat2.jpg";
            string itemSize = string.Empty;
            string itemColor = string.Empty;
            short itemQty = 1;
            decimal itemCost = 249.00m;
            decimal itemCostRangeStart = 0.00m;
            decimal itemCostRangeEnd = 0.00m;

            return gData.ListItem_Create(listPublicKey, itemTitle, itemDescription, itemCost, itemCostRangeStart, itemCostRangeEnd, itemSize, itemColor, itemQty, null, itemImageURL, itemURL);

        }

        static private Item CreateListItem(string listPublicKey, Item newItem)
        {
            GyftoList.API.Translations.API_ListItem newListItem = new API.Translations.API_ListItem();
            var gyftoApi = new API_ListItem();
            var translatedListItem = gyftoApi.ConvertToAPI_ListItem(newItem, listPublicKey);

            Uri apiURI = null;

            HttpResponseMessage response = client.PostAsJsonAsync("api/listitem/postitem", translatedListItem).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
            else
            {
                apiURI = response.Headers.Location;
            }

            return newItem;
        }

        static private Item GetListItemByDisplayOrdinal(string listPublicKey, int listItemDisplayOrdinal)
        {
            gData = new DataMethods();
            return gData.ListItem_GetByActiveAndDisplayOrdinal(listPublicKey, listItemDisplayOrdinal);
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

                var newUsr = CreateUserAccount(usrList[i, 0].ToString(), usrList[i, 1].ToString(), hash, usrList[i, 3].ToString(), "gyfto", null, string.Empty);
                if (newUsr != null)
                {
                    CreateEmailAddress(newUsr.PublicKey, usrList[i, 3].ToString());
                }
            }
        }

        static private List<GyftoList.Data.User> GetUsers()
        {
            gData = new DataMethods();
            return gData.User_GetAllUsers();
        }

        static private List<Item> GetListItemsForList(string listPublicKey)
        {
            gData = new DataMethods();
            return gData.ListItem_GetAllByListPublicKey(listPublicKey);
        }

        static private Item GetListItemByPublicKey(string listItemPublicKey)
        {
            GyftoList.API.Translations.API_ListItem newListItem = new API.Translations.API_ListItem();
            var gyftoApi = new API_ListItem();
            Item translatedListItem = null;


            var requestURI = string.Format("api/ListItem/GetItem/{0}",listItemPublicKey);
            HttpResponseMessage response = client.GetAsync(requestURI.ToString()).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
            else
            {
                translatedListItem = gyftoApi.ConvertFromAPI_ListItem(response.Content.ReadAsAsync<API_ListItem>().Result);
            }

            return translatedListItem;

        }

        static private int GetItemsCountForList(string listPublicKey)
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

        static private GyftoList.Data.User CreateUserAccount(string fName, string lName, string password, string emailAddress, string providerType, int? birthYear, string avatarUrl)
        {
            GyftoList.Data.User rc;

            using (gData = new DataMethods())
            {
                rc = gData.User_CreateUser(fName, lName, password, emailAddress, providerType, birthYear, avatarUrl);
            }

            return rc;
        }

        static private bool CreateEmailAddress(string userPublicKey, string emailAddress)
        {
            var rc = false;
            
            using(gData = new DataMethods())
            {
                gData.User_CreateEmail(userPublicKey, emailAddress);
            }

            return rc;
        }

        static private GyftoList.Data.User GetUserByEmailAddress(string emailAddress)
        {
            gData = new DataMethods();
            return gData.User_GetUserByEmail(emailAddress);
        }

        static private GyftoList.Data.User GetUserByPublicKey(string userPublicKey)
        {
            gData = new DataMethods();
            return gData.User_GetUser(userPublicKey);
        }

        static private GyftoList.Data.User LoginUser(string emailAddress, string password)
        {
            GyftoList.Data.User returnUser = null;
            using (gData = new DataMethods())
            {
                // TODO - MAKE THIS LESS AMBIGUOUS
                //returnUser = gData.User_Login(emailAddress, password);
            }

            return returnUser;
        }

        static private Item UpdateListItem(Item updatedItem, string itemPublicKey)
        {
            gData = new DataMethods();
            return gData.ListItem_UpdateItem(updatedItem, itemPublicKey);
        }

        static private HttpResponseMessage DeleteListShare(string listSharePublicKey)
        {
            //api/ListShare/DeleteListShare/5
            HttpResponseMessage response = client.DeleteAsync(string.Format("api/ListShare/DeleteListShare/{0}", listSharePublicKey).ToString()).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            return response;
        }

        static private bool CreateItemExclusion_nonAPI(API_ItemExclusion itemExclusion)
        {
            var rc = false;
            using (DataMethods dataMethods = new DataMethods())
            {
                var listShareID = dataMethods.ListShare_GetByPublicKey(itemExclusion.ListSharePublicKey).ListShareID;
                var itemID = dataMethods.ListItem_GetByPublicKey(itemExclusion.ItemPublicKey).ItemID;

                var newItemExclusion = dataMethods.ItemExclusion_Create(listShareID, itemID);
                if (newItemExclusion != null)
                {
                    rc = true;
                }
            }

            return rc;
        }

        static private HttpResponseMessage CreateItemExclusion(API_ItemExclusion itemExclusion)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/ItemExclusion/PostItemExclusion", itemExclusion).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            return response;
        }

        static private HttpResponseMessage DeleteItemExclusion(string itemExclusionPublicKey)
        {
            HttpResponseMessage response = client.DeleteAsync(string.Format("api/ItemExclusion/DeleteItemExclusion/{0}", itemExclusionPublicKey)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            return response;
        }

        static private List<ListShare> GetListShares()
        {
            gData = new DataMethods();
            return gData.ListShare_GetAll();
        }
    }
}
