namespace Aparete.JWT
{
    /// <summary>
    /// Cryptography: Cryptosystem Algorithm Types
    /// HSxxx: HMAC(Hash-based Message Authentication Code)
    /// RSxxx: RSA(Rivest–Shamir–Adleman) is the first Public-Key cryptosystems
    /// ESxxx: ECDSA(Elliptic Curve Digital Signature Algorithm) -  Digital Signature Algorithm
    /// </summary>
    public enum Alg
    {
        /// <summary>
        /// HMAC using SHA256 hash algorithm
        /// </summary>
        HS256,
        /// <summary>
        /// HMAC using SHA384 hash algorithm
        /// </summary>
        HS384,
        /// <summary>
        /// HMAC using SHA512 hash algorithm
        /// </summary>
        HS512,
        /// <summary>
        /// RSA using SHA256 hash algorithm
        /// </summary>
        RS256,
        /// <summary>
        /// RSA using SHA384 hash algorithm
        /// </summary>
        RS384,
        /// <summary>
        /// RSA using SHA512 hash algorithm
        /// </summary>
        RS512,
        /// <summary>
        /// ECDSA using SHA256 hash algorithm
        /// </summary>
        ES256,
        /// <summary>
        /// ECDSAC using SHA384 hash algorithm
        /// </summary>
        ES384,
        /// <summary>
        /// ECDSA using SHA512 hash algorithm
        /// </summary>
        ES512
    }

    /// <summary>
    /// JWTConstant class
    /// To hold a list of constant values
    /// Pre-defined values are in small letters
    /// </summary>
    public static class JWTConstant
    {
        /// <summary>
        /// Value: JWT
        /// </summary>
        public const string JSON_WEB_TOKEN = "JWT";
        /// <summary>
        /// Value: JWEA
        /// </summary>
        public const string JWEAsymmetric = "JWEA";
        /// <summary>
        /// Value: type
        /// </summary>
        public const string TYPE_HEADER = "typ";
        /// <summary>
        /// Value: alg
        /// </summary>
        public const string ENCRYPTION_ALGORITHM_HEADER = "alg";
        /// <summary>
        /// Value: enc
        /// </summary>
        public const string ENCRYPTION_METHOD_HEADER = "enc";
        /// <summary>
        /// Value: RSA-OAEP
        /// </summary>
        public const string RSA_OAEP = "RSA-OAEP";
        /// <summary>
        /// Value: A256GCM
        /// </summary>
        public const string AES_256_GCM = "A256GCM";
        /// <summary>
        /// Value: HS256
        /// </summary>
        public const string HMAC_SHA256 = "HS256";
        /// <summary>
        /// Value: alg
        /// </summary>
        public const string SIGNING_ALGORITHM_HEADER = "alg";
        /// <summary>
        /// Value: Claims
        /// </summary>
        public const string CLAIMS = "Claims";
        /// <summary>
        /// Value: exp
        /// </summary>
        public const string CLAIM_EXPIRATION_TIME = "exp";
        /// <summary>
        /// Value: iss
        /// </summary>
        public const string CLAIM_ISSUER = "iss";
        /// <summary>
        /// Value: aud
        /// </summary>
        public const string CLAIM_AUDIENCE = "aud";
        /// <summary>
        /// Value: pubkey
        /// </summary>
        public const string CLAIM_PUBLIC_KEY = "pubkey";
        /// <summary>
        /// Value: clientsecret
        /// </summary>
        public const string CLAIM_CLIENT_SECRET = "clientsecret";
        //------------------------------------------------
        /// <summary>
        /// Value: symmetrickey
        /// </summary>
        public const string CLAIM_SYMMETRIC_KEY = "symmetrickey";
        //------------------------------------------------
        /// <summary>
        /// Value: userid
        /// </summary>
        public const string USER_ID = "userid";
        /// <summary>
        /// Value: username
        /// </summary>
        public const string USER_NAME = "username";
        /// <summary>
        /// Value: usercode
        /// </summary>
        public const string USER_CODE = "usercode";
        /// <summary>
        /// Value: accesstoken
        /// </summary>
        public const string ACCESS_TOKEN = "accesstoken";
        /// <summary>
        /// Value: refreshtoken
        /// </summary>
        public const string REFRESH_TOKEN = "refreshtoken";
        /// <summary>
        /// Value: tokenexpiry
        /// </summary>
        public const string TOKEN_EXPIRY = "tokenexpiry";
        //------------------------------------------------
        /// <summary>
        /// Value: pksystemuser
        /// </summary>
        public const string PK_SYSTEM_USER = "pksystemuser";
        /// <summary>
        /// Value: pktenant
        /// </summary>
        public const string FK_TENANT = "pktenant";
    }

}