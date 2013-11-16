using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Gyfto.Web.Models;
using System.Configuration.Provider;

namespace Gyfto.Web
{
    public static class AuthConfig
    {
        

        public static void RegisterAuth()
        {

            string facebookAppId = "382355695189525";
            string facebookAppSecret = "fec2c780f878a500c19a02ca3adfc4d2";

            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: facebookAppId,
                appSecret: facebookAppSecret);

            //configuration.LoadFromAppSettings();

            //OAuthWebSecurity.RegisterClient(new Gyfto.Web.App_Start.FacebookExtendedClient(
            //    facebookAppId,
            //    facebookAppSecret,
            //    "id,first_name,last_name,link,username,gender,email,age_range,picture.height(200)",
            //    new Func<string, object, string>(fieldsTransformer),
            //    "email"));

            //OAuthWebSecurity.RegisterGoogleClient();
        }

        private static string fieldsTransformer(string key, object value)
        {
            switch (key)
            {
                case "picture":
                    var data = (value as IDictionary<string, object>)["data"] as IDictionary<string, object>;
                    return data["url"].ToString();
                case "age_range":
                    var min = (value as IDictionary<string, object>)["min"];
                    return min.ToString();
                default:
                    return value.ToString();
            }
        }
    }
}
