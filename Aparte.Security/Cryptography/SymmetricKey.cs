using System;
using System.Security.Cryptography;

namespace Aparte.Security.Cryptography
{
    /// <summary>
    /// SymmetricKey Class
    /// Generate Random Symmetric Key
    /// </summary>
    public class SymmetricKey
    {
        /// <summary>
        /// Static Method to generate random bytes and convert it to string for a key
        /// </summary>
        /// <returns></returns>
        public static string GenerateSymmetricKey()
        {
            //256-bit key
            return GenerateRandomBytes(32);
        }

        /// <summary>
        /// Static Method to generate random bytes and convert it to string
        /// with fixed digits of bytes
        /// </summary>
        /// <param name="noOfBytes"></param>
        /// <returns></returns>
        public static string GenerateRandomBytes(Int32 noOfBytes)
        {
            //256-bit key
            using (var provider = new RNGCryptoServiceProvider())
            {
                byte[] secreteKeyBytes = new Byte[noOfBytes];
                provider.GetBytes(secreteKeyBytes);

                return Convert.ToBase64String(secreteKeyBytes);
            }
        }
    }
}
