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

        // POST api/ListItemActive/5
        public Item GetItem(string id)
        {
            Item listItem = null;
            try
            {
                listItem = _dataMethods.ListItem_GetByPublicKey(id);

                if (listItem == null)
                {
                    throw new Exception(string.Format("List Item with Public Key '{0}' not found.",id));
                }
                else
                {
                    bool? activeFl = false;
                    if (Convert.ToBoolean(listItem.Active))
                    {
                        activeFl = false;
                    }
                    else
                    {
                        activeFl = true;
                    }

                    listItem = _dataMethods.ListItem_UpdateActive(listItem, Convert.ToBoolean(activeFl));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.ToString());
            }

            return listItem;
        }

    }
}
