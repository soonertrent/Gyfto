using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using GyftoList.Util;

namespace GyftoList.Services.Rest
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class User
    {
        [WebGet(UriTemplate = "CurrentTime")]
        public List<string> CurrentTime()
        {
            List<string> returnItem = new List<string>();

            returnItem.Add(DateTime.Now.ToString());
            returnItem.Add(DateTime.Now.ToString());
            returnItem.Add(DateTime.Now.ToString());

            return returnItem;
        }

        public string PublicKey = string.Empty;

        public User GetUserDetails(string publicKey)
        {
            GyftoList.Util.User util = new Util.User();
            var user = new User();
            user.PublicKey = util.GenerateUserPublicKey();


            return user;
        }
    }
}