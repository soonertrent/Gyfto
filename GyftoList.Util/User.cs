using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GyftoList.Util
{
    public class User
    {
        #region Constructors

        public User() { }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public string GenerateUserPublicKey()
        {
            return Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

        #endregion
    }
}
