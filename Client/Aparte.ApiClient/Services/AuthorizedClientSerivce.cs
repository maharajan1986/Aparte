using Aparete.Json;
using Aparete.JWT;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Aparte.ApiClient.Services
{
    /// <summary>
    /// Tasks to be performed by client application to get authenticated
    /// </summary>
    public class AuthorizeClientService
    {
        /// <summary>
        /// Only to get Api Server public key, by sending JEdixWin Client Name
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<bool> ExchangePublicKey(CancellationToken token = new CancellationToken())
        {
            using (var clientKeyExchange = new AparteHttpClient(WinClient.ApiServiceUri, HttpHeader.ExchangePublicKeyHeader()))
            {
                var content = JsonSerializer.GetStringContent(new { Name = KeyFile.JEDIX_WIN_CLIENT_NAME });
                using (var response = await clientKeyExchange.PostAsync("api/Account/ExchangePublicKeys", content, token).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var jwsString = await response.Content.ReadAsStringAsync();
                        var jws = JWT.ParseFromBase64Url(jwsString);
                        WinClient.ServerPublicKey = jws[JWTConstant.CLAIM_PUBLIC_KEY];
                    }
                }
            }
            return ApiClient.WinClient.IsClientAuthorized;
        }
        /// <summary>
        /// Important Step to get verified if this JEdixWin is authentic by Api Server.
        /// PKI Encrypted Header is created by HttpHeader.VerifyClientHeader(ServerPublicKey)
        /// which contains KeyFile.JEDIX_WIN_CLIENT_SECRET encrypted by ServerPublicKey
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<bool> VerifyClient(CancellationToken token = new CancellationToken())
        {
            if (!WinClient.IsClientAuthorized)
            {
                using (var clientKeyExchange = new AparteHttpClient(WinClient.ApiServiceUri, HttpHeader.VerifyClientHeader(WinClient.ServerPublicKey)))
                {
                    var content = JsonSerializer.GetStringContent(null);
                    using (var response = await clientKeyExchange.PostAsync("api/Account/VerifyClient", content, token).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var jwsString = await response.Content.ReadAsStringAsync();
                            var jws = JWEAsymmetric.Parse(jwsString.Replace("\"",""), WinClient.PRIVATE_KEY);
                            WinClient.SharedSymmetricKey = jws[JWTConstant.CLAIM_SYMMETRIC_KEY];
                            WinClient.IsClientAuthorized = true;
                        }
                        else
                            WinClient.IsClientAuthorized = false;
                    }
                }
            }
            return WinClient.IsClientAuthorized;
        }

        /// <summary>
        /// Method to get User AccessToken by connecting to Token Method "api/Account/Token" of Api Server with user credential
        /// Password is encrypted with Api Server Public Key
        /// This method requires
        /// 1. JEdixAuthServerPublicKey
        /// 2. JEdixWin is authorized - Gene.Client.WinClient.IsClientAuthorized
        /// If authenticated,
        /// 1. Token is decrypted and stored in static properties of static class "Win"
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> GetAccessToken(string userId, string password, CancellationToken cancellationToken = new CancellationToken())
        {
            if (WinClient.IsClientAuthorized)
            {
                using (var client = new AparteHttpClient(WinClient.ApiServiceUri, HttpHeader.UserVerificationHeader(WinClient.ServerPublicKey)))
                {
                    var encryptedPassword = Security.Cryptography.AsymmetricEncryption.Encrypt(password, WinClient.ServerPublicKey);
                    var loginContent = JsonSerializer.GetStringContent(new { UserId = userId, Password = encryptedPassword });
                    using (var response = await client.PostAsync("api/Account/Token", loginContent, cancellationToken).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var token = await response.Content.ReadAsStringAsync();
                            token = token.Replace("\"", "");
                            var decryptedToken = JWEAsymmetric.Parse(token, WinClient.PRIVATE_KEY);
                            WinClient.PKSystemUser = GetInt64(decryptedToken.Claims[JWTConstant.PK_SYSTEM_USER]);
                            WinClient.SystemUserName = decryptedToken.Claims[JWTConstant.USER_NAME];
                            WinClient.AccessToken = decryptedToken.Claims[JWTConstant.ACCESS_TOKEN];
                            WinClient.RefreshToken = decryptedToken.Claims[JWTConstant.REFRESH_TOKEN];
                            WinClient.TokenExpiryInUtc = decryptedToken.ExpiryDateTimeInUtc();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Method to re-obtain User Access Token with Refresh Token
        /// </summary>
        /// <param name="pkSystemUser"></param>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> RefreshToken(long? pkSystemUser, string accessToken, string refreshToken, CancellationToken cancellationToken = new CancellationToken())
        {
            if (WinClient.IsClientAuthorized)
            {
                using (var client = new AparteHttpClient(WinClient.ApiServiceUri, HttpHeader.UserVerificationHeader(WinClient.ServerPublicKey)))
                {
                    var encryptedAccessToken = Security.Cryptography.AsymmetricEncryption.Encrypt(accessToken, WinClient.ServerPublicKey);
                    var encryptedRefreshToken = Security.Cryptography.AsymmetricEncryption.Encrypt(refreshToken, WinClient.ServerPublicKey);
                    var body = new { PKSystemUser = pkSystemUser, AccessToken = encryptedAccessToken, RefreshToken = encryptedRefreshToken };
                    var content = JsonSerializer.GetStringContent(body);
                    using (var response = await client.PostAsync("api/Account/RefreshToken", content, cancellationToken).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var token = await response.Content.ReadAsStringAsync();
                            var decryptedToken = JWEAsymmetric.Parse(token, WinClient.PRIVATE_KEY);
                            WinClient.AccessToken = decryptedToken.Claims[JWTConstant.ACCESS_TOKEN];
                            WinClient.TokenExpiryInUtc = decryptedToken.ExpiryDateTimeInUtc();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static long? GetInt64(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return Int64.Parse(value);
        }
    }
}
