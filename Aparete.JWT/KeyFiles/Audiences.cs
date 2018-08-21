using Aparte.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aparete.JWT
{
    /// <summary>
    /// Audience class
    /// contains information of audience name, public key, secret symmetric key, client secret
    /// </summary>
    public class Audience
    {
        /// <summary>
        /// Name of the audience
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Public Key of the audience
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// Secret symmetric key of the audience
        /// </summary>
        public string SecretSymmetricKey { get; set; }

        /// <summary>
        /// Client Secret of the audience
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Redirect Url
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Verify the audience data with audience's signature, using SHA256
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool VerifySignature(byte[] signature, byte[] content)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(this.PublicKey);
                return rsa.VerifyData(content, "SHA256", signature);
            }
        }
    }
    /// <summary>
    /// Audiences class, containing the list of audiences in Dictionary
    /// </summary>
    public static class Audiences
    {
        private static Dictionary<string, Audience> keys = new Dictionary<string, Audience>();
        /// <summary>
        /// Check to see the audience with key name already exists
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool Exists(string keyName)
        {
            return keys.ContainsKey(keyName);
        }

        /// <summary>
        /// Get the audience with key name
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static Audience Item(string keyName)
        {
            return keys.ContainsKey(keyName) ? keys[keyName] : null;
        }

        /// <summary>
        /// Add an audience
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="publicKey"></param>
        /// <param name="secreteKey"></param>
        /// <returns></returns>
        public static Audience Add(string keyName, string publicKey, string secreteKey)
        {
            return Audiences.Add(keyName, publicKey, secreteKey, null);
        }
        /// <summary>
        /// Add public key with key name, if not exists.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="publicKey"></param>
        /// <param name="secreteKey"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public static Audience Add(string keyName, string publicKey, string secreteKey, string clientSecret)
        {
            if (!keys.ContainsKey(keyName))
            {
                var secrete = RNGCryptoServiceProvider.Create();
                keys.Add(keyName, new Audience { Name = keyName, PublicKey = publicKey, SecretSymmetricKey = secreteKey, ClientSecret = clientSecret });
            }
            return keys[keyName];
        }

        /// <summary>
        /// Encrypt the content to be sent to the audience, using audience's public key
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Encrypt(string keyName, string content)
        {
            var audience = keys[keyName];
            return AsymmetricEncryption.Encrypt(content, audience.PublicKey);
        }

        /// <summary>
        /// Delete an audience from the list
        /// </summary>
        /// <param name="keyName"></param>
        public static void Delete(string keyName)
        {
            if (keys.ContainsKey(keyName))
                keys.Remove(keyName);
        }

        /// <summary>
        /// Verify the content is authentic, whether truly sent from the audience
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="signature"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool VerifySignature(string keyName, byte[] signature, byte[] content)
        {
            if (keys.ContainsKey(keyName))
            {
                var party = keys[keyName];
                return party.VerifySignature(signature, content);
            }
            return false;
            //var sha = new SHA256Managed();
            //byte[] contentByte = Encoding.UTF8.GetBytes(content);
            //byte[] hashData = sha.ComputeHash(contentByte);

            //using (var rsa = new RSACryptoServiceProvider())
            //{
            //    var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //    rsaDeformatter.SetHashAlgorithm("SHA512");
            //    return rsaDeformatter.VerifySignature(hashData, Convert.FromBase64String(signature));
            //}
        }
    }
}
