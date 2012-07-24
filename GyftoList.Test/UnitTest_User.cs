using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GyftoList.Test.GyftoList.Services;

namespace GyftoList.Test
{
    [TestClass]
    public class UnitTest_User
    {
        [TestMethod]
        public void TestMethod1()
        {
            var userProxy = new GyftoList.Services.UserClient();
            var userPublicKey = userProxy.GetUserDetails(string.Empty);


        }
    }
}
