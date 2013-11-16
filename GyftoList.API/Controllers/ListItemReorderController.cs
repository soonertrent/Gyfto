using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GyftoList.Data;

namespace GyftoList.API.Controllers
{
    public class ListItemReorderController : ApiController
    {

        #region Private Parameters

        private DataMethods _dataAccess;

        #endregion

        // GET api/listitemreorder/getreorderlistitem/aac6d432?newOrdinal=1
        public HttpResponseMessage GetReorderListItem(string id, int newOrdinal)
        {
            try
            {
                using (_dataAccess = new DataMethods())
                {
                    if (!_dataAccess.ListItem_ReorderItem(id, newOrdinal))
                    {
                        throw new HttpResponseException(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                        return response;
                    }
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
           
        }
    }
}
