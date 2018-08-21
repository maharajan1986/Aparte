using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aparete.JWT
{
    /// <summary>
    /// Http Header Scheme type
    /// Basic Token
    /// Diget Token
    /// Bearer Token
    /// </summary>
    public enum HttpHeaderScheme
    {
        /// <summary>
        /// Basic scheme: Encoded but not encrypted
        /// GET / HTTP/1.1         
        /// Host: example.org 
        /// Authorization: Basic Zm9vOmJhcg==
        /// </summary>
        Basic,
        /// <summary>
        /// Diget scheme
        /// Credentials are hashed and assigned to response (Hash uses MD5)
        /// WWW-Authenticate: Digest realm="Videos", qop="auth,auth-int", nonce="jd839ud9832duj329u9u8ru32rr8u293ur9u329ru"
        /// </summary>
        Digest,
        /// <summary>
        /// Bearer scheme
        /// </summary>
        Bearer
    }

    /// <summary>
    /// KeyFile class
    /// Holding JEdix Authentication Target and secret constant values
    /// </summary>
    public class KeyFile
    {
        /// <summary>
        /// Value: JEdixAuthServer
        /// </summary>
        public const string AUTHENTICATION_SERVER_NAME = "JEdixAuthServer";
        /// <summary>
        /// Value: JEdixWpfClient
        /// </summary>
        public const string JEDIX_WIN_CLIENT_NAME = "JEdixWpfClient";
        /// <summary>
        /// Value: hard coded secret key (not exposed here)
        /// </summary>
        public const string JEDIX_WIN_CLIENT_SECRET = "y/uC1EPWms/qa8lmWsbm5y+8vXNoif/IyLuj6+dFrpY=";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="containerName"></param>
        public KeyFile(string containerName)
        {
            CspParameters = new CspParameters { KeyContainerName = containerName };
        }

        /// <summary>
        /// Name: CspParameters.KeyContainerName
        /// </summary>
        public string Name
        {
            get { return CspParameters.KeyContainerName; }
        }
        /// <summary>
        /// CspParameters
        /// </summary>
        public CspParameters CspParameters { get; set; }

        /// <summary>
        /// Get new public key
        /// </summary>
        public string PublicKey
        {
            get
            {
                using (var rsa = new RSACryptoServiceProvider(CspParameters))
                {
                    return rsa.ToXmlString(false);
                }
            }
        }

        /// <summary>
        /// Encrypt using Asymmetric key (public key of the receiver)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="receivingParty"></param>
        /// <returns></returns>
        public string Encrypt(string content, Audience receivingParty)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(receivingParty.PublicKey);
                byte[] contenteBytes = System.Text.Encoding.UTF8.GetBytes(content);
                byte[] encryptedBytes = rsa.Encrypt(contenteBytes, false);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// Decrypt using RSA Asymmetric key (private key)
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public string Decrypt(string encryptedString)
        {
            byte[] decryptedBytes = Decrypt(Convert.FromBase64String(encryptedString));
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        /// <summary>
        /// Convert dcrypted byte[] to plain byte[]
        /// </summary>
        /// <param name="encryptedBytes"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] encryptedBytes)
        {
            using (var rsa = new RSACryptoServiceProvider(CspParameters))
            {
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);
                return decryptedBytes;
            }
        }
        /// <summary>
        /// Delete Key Container
        /// </summary>
        public void DeleteKeyContainer()
        {
            using (var rsa = new RSACryptoServiceProvider(CspParameters))
            {
                rsa.PersistKeyInCsp = false;
                rsa.Clear();
            }
        }
        /// <summary>
        /// Get the signature of string, using overloaded CreateSignature(byte[] content)
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string CreateSignature(string content)
        {
            return Convert.ToBase64String(CreateSignature(Encoding.UTF8.GetBytes(content)));
        }
        /// <summary>
        /// Create Digital Signature, using this Key file's private key
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public byte[] CreateSignature(byte[] content)
        {
            using (var rsa = new RSACryptoServiceProvider(CspParameters))
            {
                return rsa.SignData(content, "SHA256");
            }
            //byte[] contentByte = Encoding.UTF8.GetBytes(content);
            //SHA512Managed sha = new SHA512Managed();
            //byte[] hashData = sha.ComputeHash(contentByte);
            //using (var rsa = new RSACryptoServiceProvider(CspParameters))
            //{
            //    //we also can use rsa.SignData()
            //    var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //    rsaFormatter.SetHashAlgorithm("SHA512");
            //    byte[] signedValue = rsaFormatter.CreateSignature(hashData);
            //    return Convert.ToBase64String(signedValue);
            //}
        }


        /// <summary>
        /// Sign and then encrypt content string before sending
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sendingParty"></param>
        /// <returns></returns>
        public string SignAndEncrypt(string content, Audience sendingParty)
        {
            var signedContent = CreateSignature(content);
            var encryptedContent = this.Encrypt(signedContent, sendingParty);
            return signedContent;
        }

        ///// <summary>
        ///// Decrypt and verify the content after receiving
        ///// Decrypt using this KeyFile private key
        ///// </summary>
        ///// <param name="encryptedContent"></param>
        ///// <returns></returns>
        //public string DecryptAndVerify(byte[] encryptedContent, Party sendingParty)
        //{
        //    //1) Decrypt using this KeyFile private key
        //    var contentByte = this.Decrypt(encryptedContent);
        //    //2) Verify using sending party's public key
        //    if (sendingParty.VerifySignature(contentByte))
        //    {
        //        return Convert.ToBase64String(signedValue);
        //    }
        //}
    }
}
