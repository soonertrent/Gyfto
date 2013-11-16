using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Builder;

namespace GyftoList.API.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<GyftoList.API.Translations.API_ListItem>("API_ListItems");
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}