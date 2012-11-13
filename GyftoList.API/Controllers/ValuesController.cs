using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GyftoList.API.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post(string value)
        {
            return value;
        }

        // PUT api/values/5
        public string Put(int id, string value)
        {
            return id.ToString();
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}