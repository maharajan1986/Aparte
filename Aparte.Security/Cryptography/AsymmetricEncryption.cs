using System;
using System.Security.Cryptography;
using System.Text;

namespace Aparte.Security.Cryptography
{
    /// <summary>
    /// Static Library to encrypt with a public key and decrypt with a private key
    /// </summary>
    public class AsymmetricEncryption
    {
        /// <summary>
        /// Encrypt Clear Text with a given public key
        /// </summary>
        /// <param name="clearText"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText, string publicKey)
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(clearText);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                var encryptedContentBytes = rsa.Encrypt(contentBytes, true);
                return Convert.ToBase64String(encryptedContentBytes);
            }
        }

        /// <summary>
        /// Decrypt contents which have been encrypted with public key into clear texts with private key
        /// </summary>
        /// <param name="encryptedBase64Content"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedBase64Content, string privateKey)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64Content);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                var decryptedBytes = rsa.Decrypt(encryptedBytes, true);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
