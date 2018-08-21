
using Newtonsoft.Json;
using Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Aparete.JWT
{
    /// <summary>
    /// JWE: Json Web Encrypted Token
    /// signed and encrypted, using Asymmetric key and AES-symmetric key
    /// </summary>
    public class JWEAsymmetric : JWT
    {
        /// <summary>
        /// Parse plain string token and get JWE Asymmetric token object
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static JWEAsymmetric Parse(string token, string privateKey)
        {
            byte[] claimSet = null;
            EncryptedPayload payload = null;

            try
            {
                payload = EncryptedPayload.Parse(token);
                byte[] masterKey = null;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);
                    masterKey = rsa.Decrypt(payload.EncryptedMasterKey, true);
                }

                byte[] additionalAuthenticateData = payload.ToAdditionalAuthenticatedData();
                using (var aes = new AuthenticatedAesCng())
                {
                    aes.CngMode = CngChainingMode.Gcm;
                    aes.Key = masterKey;
                    aes.IV = payload.InitializationVector;
                    aes.AuthenticatedData = additionalAuthenticateData;
                    aes.Tag = payload.Tag;

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            byte[] cipherText = payload.CipherText;
                            cs.Write(cipherText, 0, cipherText.Length);
                            cs.FlushFinalBlock();

                            claimSet = ms.ToArray();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new SecurityException("Invalid Token", ex);
            }

            var jwt = JsonConvert.DeserializeObject<JWEAsymmetric>(payload.Header);
            jwt.AsymmetricKey = privateKey;
            jwt.claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.UTF8.GetString(claimSet));

            return jwt;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JWEAsymmetric()
        {
            this.Type = JWTConstant.JWEAsymmetric;
        }

        /// <summary>
        /// To encrypt this token, we need receiver's public key
        /// </summary>
        /// <returns></returns>
        public override string SerializeToBase64UrlString()
        {
            string header = JsonConvert.SerializeObject(this);
            string claims = JsonConvert.SerializeObject(this.claims);
            byte[] masterKey = new byte[32];
            byte[] initVector = new byte[12];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(masterKey);
                provider.GetBytes(initVector);
            }

            byte[] encryptedMasterKey = null;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(AsymmetricKey);
                encryptedMasterKey = rsa.Encrypt(masterKey, true);
            }

            var authoData = new EncryptedPayload()
            {
                Header = header,
                EncryptedMasterKey = encryptedMasterKey,
                InitializationVector = initVector
            };

            byte[] additionalAuthenticatedData = authoData.ToAdditionalAuthenticatedData();

            byte[] tag = null;
            byte[] cipherText = null;

            using (var aes = new AuthenticatedAesCng())
            {
                //AES+GCM is used for encryption AND authentication(digital signature)
                aes.CngMode = CngChainingMode.Gcm;
                aes.Key = masterKey;
                aes.IV = initVector;
                aes.AuthenticatedData = additionalAuthenticatedData;

                using (var ms = new MemoryStream())
                {
                    using (var encryptor = aes.CreateAuthenticatedEncryptor())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            byte[] claimSet = Encoding.UTF8.GetBytes(claims);
                            cs.Write(claimSet, 0, claimSet.Length);
                            cs.FlushFinalBlock();
                            tag = encryptor.GetTag();
                            cipherText = ms.ToArray();
                        }
                    }
                }
            }
            var payload = new EncryptedPayload()
            {
                Header = header,
                EncryptedMasterKey = encryptedMasterKey,
                InitializationVector = initVector,
                CipherText = cipherText,
                Tag = tag
            };
            string token = payload.SerializeToBase64UrlString();

            return token;
        }

        /// <summary>
        /// AsymmetricKey used to encrypt the content
        /// If encryption then use private key, if signature then public key
        /// </summary>
        [JsonIgnore]
        public string AsymmetricKey { get; set; }

    }
}
