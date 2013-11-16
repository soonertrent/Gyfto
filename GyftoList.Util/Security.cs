using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


namespace GyftoList.Util
{
    public class Security
    {
        #region Constructors

        public Security(){}

        #endregion

        #region Properties

        private static Int32 _iter = 1000;

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a slated version of an input Password
        /// </summary>
        /// <param name="salt">Salt of Password</param>
        /// <param name="password">Clear Password</param>
        /// <param name="iterations">Number of times to iterate Salt</param>
        /// <returns></returns>
        public string CreateSaltedPassword(string salt, string password, int iterations)
        {
            SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(salt + password);
            do
            {
                byteValue = SHA1.ComputeHash(byteValue);
            } while (--iterations > 0);
            SHA1.Clear();
            return Convert.ToBase64String(byteValue);
        }

        /// <summary>
        /// Compares and input and Salted Password
        /// </summary>
        /// <param name="salt">Salt of Password method</param>
        /// <param name="password">Input - the clear Password</param>
        /// <param name="storedPassword">Input - previously Salted Password</param>
        /// <param name="iterations">Number of times to iterate Salt</param>
        /// <returns>True|False</returns>
        public bool CompareSaltedPasswords(string salt, string password, string storedPassword, int iterations)
        {
            var passwordHash = string.Empty;

            // Create the hashed password
            passwordHash = this.CreateSaltedPassword(salt, password, iterations);

            // Compare the passwords
            return (string.Compare(storedPassword, passwordHash) == 0);
        }

        #endregion
    }


}
