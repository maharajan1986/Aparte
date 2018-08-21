using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace Aparete.JWT
{
    /// <summary>
    /// Plain Text Json Web Token
    /// JWT specification specifies base64 URL encoding to be used
    /// </summary>
    public class JWT
    {
        /// <summary>
        /// Year:1970, Month:Jan, Date: 1st
        /// JWT cannot express dates, number of seconds are used instead
        /// </summary>
        public static readonly DateTime EPOCH_START = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Indicate if the token is expired
        /// </summary>
        /// <param name="expiryPeriod"></param>
        /// <returns></returns>
        public static bool IsTokenExpired(ulong expiryPeriod)
        {
            var ts = DateTime.UtcNow - EPOCH_START; // duration since 1970/1/1            
            return expiryPeriod < Convert.ToUInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// Number of seconds until Expiry date
        /// </summary>
        /// <param name="endLifeinUtc"></param>
        /// <returns></returns>
        public static ulong GetExpiryDuration(DateTime endLifeinUtc)
        {
            TimeSpan ts = endLifeinUtc.Subtract(EPOCH_START);
            return Convert.ToUInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// Expecting the string token format to be period separated, header and claims
        /// Both header and claims should be plain texts
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JWT ParseFromPlainText(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 2)
                throw new SecurityException("Bad Token");
            var jwt = JsonConvert.DeserializeObject<JWT>(parts[0]);
            jwt.claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(parts[1]);
            return jwt;
        }

        /// <summary>
        /// Expecting the string token format to be period separated, header and claims
        /// Both header and claims should be Base64Url encoded
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JWT ParseFromBase64Url(string token)
        {
            token = token.Replace("\"", "");
            var parts = token.Split('.');
            if (parts.Length != 2)
                throw new SecurityException("Bad Token");

            string basicInfo = Encoding.UTF8.GetString(parts[0].FromBase64Url());
            string claims = Encoding.UTF8.GetString(parts[1].FromBase64Url());

            var jwt = JsonConvert.DeserializeObject<JWT>(basicInfo);
            jwt.claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(claims);
            return jwt;
        }

        /// <summary>
        /// Claim property
        /// </summary>
        protected Dictionary<string, string> claims = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        public JWT()
        {
            this.Type = JWTConstant.JSON_WEB_TOKEN;
        }

        /// <summary>
        /// Check if the token is expired by calling static method JWT.IsTokenExpired(this.ExpiresOn)
        /// </summary>
        /// <returns></returns>
        public bool IsTokenExpired()
        {
            return JWT.IsTokenExpired(this.ExpiresOn);
        }

        /// <summary>
        /// Set the expriy date and time by calling static method JWT.GetExpiryDuration(endLife)
        /// </summary>
        /// <param name="endLife"></param>
        public void SetExpiry(DateTime endLife)
        {
            this.ExpiresOn = JWT.GetExpiryDuration(endLife);
        }

        /// <summary>
        /// Get the exprity date in Utc
        /// </summary>
        /// <returns></returns>
        public DateTime ExpiryDateTimeInUtc()
        {
            var totalSeconds = Convert.ToDouble(this.ExpiresOn);
            return EPOCH_START.AddSeconds(totalSeconds);
        }

        /// <summary>
        /// Json standard property: Type of JWT, should be match with JWTConstant string value
        /// </summary>
        [JsonProperty(PropertyName = JWTConstant.TYPE_HEADER)]
        public string Type { get; protected set; }

        /// <summary>
        /// Encryption Algorithm, always returns JWTConstant.RSA_OAEP
        /// </summary>
        [JsonProperty(PropertyName = JWTConstant.ENCRYPTION_ALGORITHM_HEADER)]
        public string EncryptionAlgorithm
        {
            get { return JWTConstant.RSA_OAEP; }
        }

        /// <summary>
        /// Encryption Method, always returns JWTConstant.AES_256_GCM
        /// </summary>
        [JsonProperty(PropertyName = JWTConstant.ENCRYPTION_METHOD_HEADER)]
        public string EncryptionMethod
        {
            get { return JWTConstant.AES_256_GCM; }
        }

        /// <summary>
        /// Claim list part of the token return in Dictionary of string
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> Claims
        {
            get { return this.claims; }
        }
        /// <summary>
        /// Claim list part of the token return in List of string
        /// </summary>
        [JsonIgnore]
        public IList<Claim> ClaimList
        {
            get
            {
                return this.claims.Keys.SelectMany(key => this.claims[key].Split(',').Select(value => new Claim(key, value))).ToList();
            }
        }

        /// <summary>
        /// Expiry element of claims, claim name = JWTConstant.CLAIM_EXPIRATION_TIME
        /// </summary>
        [JsonIgnore]
        public ulong ExpiresOn
        {
            get
            {
                return UInt64.Parse(this.claims[JWTConstant.CLAIM_EXPIRATION_TIME]);
            }
            protected set
            {
                if (this.claims.ContainsKey(JWTConstant.CLAIM_EXPIRATION_TIME))
                    this.claims[JWTConstant.CLAIM_EXPIRATION_TIME] = value.ToString();
                else
                    this.claims.Add(JWTConstant.CLAIM_EXPIRATION_TIME, value.ToString());
            }
        }

        /// <summary>
        /// Issuer element of claims, claim name = JWTConstant.CLAIM_ISSUER
        /// </summary>
        [JsonIgnore]
        public string Issuer
        {
            get
            {
                return this.claims.ContainsKey(JWTConstant.CLAIM_ISSUER) ? this.claims[JWTConstant.CLAIM_ISSUER] : string.Empty;
            }
            set
            {
                this.claims.Add(JWTConstant.CLAIM_ISSUER, value);
            }
        }

        /// <summary>
        /// Audience element of claims, claim name = JWTConstant.CLAIM_AUDIENCE
        /// </summary>
        [JsonIgnore]
        public string Audience
        {
            get
            {
                return this.claims.ContainsKey(JWTConstant.CLAIM_AUDIENCE) ? this.claims[JWTConstant.CLAIM_AUDIENCE] : string.Empty;
            }
            set
            {
                this.claims.Add(JWTConstant.CLAIM_AUDIENCE, value);
            }
        }

        /// <summary>
        /// SymmetricKey element of claims, claim name = JWTConstant.CLAIM_SYMMETRIC_KEY
        /// </summary>
        [JsonIgnore]
        public string SymmetricKey
        {
            get
            {
                return this.claims.ContainsKey(JWTConstant.CLAIM_AUDIENCE) ? this.claims[JWTConstant.CLAIM_SYMMETRIC_KEY] : string.Empty;
            }
            set
            {
                this.claims.Add(JWTConstant.CLAIM_SYMMETRIC_KEY, value);
            }
        }
        /// <summary>
        /// Add a new claim with a claim type and value
        /// </summary>
        /// <param name="claimType"></param>
        /// <param name="value"></param>
        public void AddClaim(string claimType, string value)
        {
            if (this.claims.ContainsKey(claimType))
                this.claims[claimType] = this.claims[claimType] + "," + value;
            else
                this.claims.Add(claimType, value);
        }
        /// <summary>
        /// Index property
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string this[string key]
        {
            get
            {
                switch (key)
                {
                    case JWTConstant.TYPE_HEADER:
                        return this.Type;
                    case JWTConstant.CLAIM_ISSUER:
                        return this.Issuer;
                    case JWTConstant.CLAIM_AUDIENCE:
                        return this.Audience;
                    case JWTConstant.ENCRYPTION_METHOD_HEADER:
                        return this.EncryptionMethod;
                    case JWTConstant.ENCRYPTION_ALGORITHM_HEADER:
                        return this.EncryptionAlgorithm;
                    case JWTConstant.CLAIM_EXPIRATION_TIME:
                        return this.ExpiresOn.ToString();
                    default:
                        return this.claims[key];
                }
            }
        }

        /// <summary>
        /// Serialize this token to plain text (not Base64Url)
        /// </summary>
        /// <returns></returns>
        public virtual string SerializeToPlainText()
        {
            string header = JsonConvert.SerializeObject(this);
            string claims = JsonConvert.SerializeObject(this.claims);
            return String.Format("{0}.{1}", header, claims);
        }

        /// <summary>
        /// Serialize this token to Base64Url string, so that the string can be used in url
        /// </summary>
        /// <returns></returns>
        public virtual string SerializeToBase64UrlString()
        {
            string header = JsonConvert.SerializeObject(this).GetBase64UrlFromNormalString();
            string claims = JsonConvert.SerializeObject(this.claims).GetBase64UrlFromNormalString();

            return String.Format("{0}.{1}", header, claims);
        }
    }
}
