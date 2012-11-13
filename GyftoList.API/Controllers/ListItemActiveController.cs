using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GyftoList.Data;

namespace GyftoList.API.Controllers
{
    public class ListItemActiveController : ApiController
    {
        private DataMethods _dataMethods = new DataMethods();

        // PUT api/ListItemActive/5
        //TODO: Not sure how this will work
        public HttpResponseMessage PutItem(string id, bool active)
        {
            HttpResponseMessage rMsg = new HttpResponseMessage();
            try
            {
                var rc = _dataMethods.ListItem_UpdateActive(id, active);
                if (!rc)
                {
                    rMsg = Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    rMsg = Request.CreateResponse(HttpStatusCode.OK, id);
                }
            }
            catch (Exception)
            {
                rMsg = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return rMsg;
        }
    }
}
