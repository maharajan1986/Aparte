using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Aparete.JWT
{
    /// <summary>
    /// EncryptedPayload class
    /// Represent complete token with 5 parts
    /// 1. Header, 
    /// 2. Encrypted Master Key
    /// 3. Initial Vector
    /// 4. CipherText
    /// 5. Tag
    /// </summary>
    public class EncryptedPayload
    {
        /// <summary>
        /// Parse dot separated string value and return EncryptedPayload object
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static EncryptedPayload Parse(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 5)
                throw new SecurityException("Bad Token");

            return new EncryptedPayload()
            {
                Header = Encoding.UTF8.GetString(parts[0].FromBase64Url()),
                EncryptedMasterKey = parts[1].FromBase64Url(),
                InitializationVector = parts[2].FromBase64Url(),
                CipherText = parts[3].FromBase64Url(),
                Tag = parts[4].FromBase64Url()
            };
        }

        /// <summary>
        /// Header portion of Payload
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// EncryptedMasterKey portion of Payload
        /// </summary>
        public byte[] EncryptedMasterKey { get; set; }
        /// <summary>
        /// InitializationVector portion of Payload
        /// </summary>
        public byte[] InitializationVector { get; set; }
        /// <summary>
        /// CipherText portion of Payload
        /// </summary>
        public byte[] CipherText { get; set; }
        /// <summary>
        /// Tag portion of Payload
        /// </summary>
        public byte[] Tag { get; set; }

        /// <summary>
        /// Convert this payload to Base64Url string
        /// </summary>
        /// <returns></returns>
        public string SerializeToBase64UrlString()
        {
            return String.Format("{0}.{1}.{2}.{3}.{4}",
                Header.GetBase64UrlFromNormalString(),
                EncryptedMasterKey.ToBase64UrlString(),
                InitializationVector.ToBase64UrlString(),
                CipherText.ToBase64UrlString(),
                Tag.ToBase64UrlString()
                );
        }

        /// <summary>
        /// To create Additional Authenticated Data and return byte[]
        /// </summary>
        /// <returns></returns>
        public byte[] ToAdditionalAuthenticatedData()
        {
            string data = String.Format("{0}.{1}.{2}",
                Header.GetBase64UrlFromNormalString(),
                EncryptedMasterKey.ToBase64UrlString(),
                InitializationVector.ToBase64UrlString()
                );
            return Encoding.UTF8.GetBytes(data);
        }


    }
}
