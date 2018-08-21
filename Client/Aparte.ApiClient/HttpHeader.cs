using Aparete.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.ApiClient
{
    public class HttpHeader
    {
        /// <summary>
        /// Http Header for exchanging public keys with auth server
        /// Authorization Scheme: Basic
        /// </summary>
        /// <returns></returns>
        public static AuthenticationHeaderValue ExchangePublicKeyHeader()
        {
            var jwtHeader = InitializeJWTHeader<JWT>();
            jwtHeader.AddClaim(JWTConstant.CLAIM_PUBLIC_KEY, WinClient.PUBLIC_KEY);
            var header = new AuthenticationHeaderValue(HttpHeaderScheme.Basic.ToString(), jwtHeader.SerializeToBase64UrlString());
            return header;
        }

        // <summary>
        /// Http Header for Verify this client with auth server, by sending Client Secret
        /// Authorization Scheme: Digest
        /// </summary>
        /// <returns></returns>
        public static AuthenticationHeaderValue VerifyClientHeader(string serverPublicKey)
        {
            var jwtHeader = InitializeJWTHeader<JWEAsymmetric>();
            jwtHeader.AsymmetricKey = serverPublicKey;
            jwtHeader.AddClaim(JWTConstant.CLAIM_CLIENT_SECRET, KeyFile.JEDIX_WIN_CLIENT_SECRET);
            var header = new AuthenticationHeaderValue(HttpHeaderScheme.Digest.ToString(), jwtHeader.SerializeToBase64UrlString());
            return header;
        }
        /// <summary>
        /// Http Header for Verify the login user with auth server, by sending Client Secret
        /// Authorization Scheme: Digest
        /// </summary>
        /// <param name="serverPublicKey"></param>
        /// <returns></returns>
        public static AuthenticationHeaderValue UserVerificationHeader(string serverPublicKey)
        {
            var jwtHeader = InitializeJWTHeader<JWEAsymmetric>();
            jwtHeader.AsymmetricKey = serverPublicKey;
            jwtHeader.AddClaim(JWTConstant.CLAIM_CLIENT_SECRET, KeyFile.JEDIX_WIN_CLIENT_SECRET);
            var header = new AuthenticationHeaderValue(HttpHeaderScheme.Digest.ToString(), jwtHeader.SerializeToBase64UrlString());
            return header;
        }

        public static string ODataVerificationHeader(string serverPublicKey, long? pkSystemUser, long? pkTenant, Dictionary<string, string> parameters)
        {
            var jwtHeader = InitializeJWTHeader<JWEAsymmetric>();
            jwtHeader.AsymmetricKey = serverPublicKey;
            jwtHeader.AddClaim(JWTConstant.CLAIM_CLIENT_SECRET, KeyFile.JEDIX_WIN_CLIENT_SECRET);
            jwtHeader.AddClaim(JWTConstant.ACCESS_TOKEN, WinClient.AccessToken);
            jwtHeader.AddClaim(JWTConstant.PK_SYSTEM_USER, pkSystemUser.ToString());
            jwtHeader.AddClaim(JWTConstant.FK_TENANT, pkTenant.ToString());
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    jwtHeader.AddClaim(p.Key, p.Value);
                }
            }
            return jwtHeader.SerializeToBase64UrlString();
        }

        public static T InitializeJWTHeader<T>() where T : JWT, new()
        {
            var jwtHeader = new T();
            jwtHeader.Issuer = KeyFile.JEDIX_WIN_CLIENT_NAME;
            jwtHeader.Audience = KeyFile.AUTHENTICATION_SERVER_NAME;
            return jwtHeader;
        }
    }
}
