using Aparete.JWT;
using Aparte.AuthServer;
using Aparte.Repository;
using Aparte.Security.Cryptography;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace AparteApi.Controllers
{
    /// <summary>
    /// Act as Authentication Server
    /// Client side library, Gene.Client.Services.AuthorizeClientService is used to call these methods
    /// </summary>
    public class AccountController : ApiController
    {
        /// <summary>
        /// Only to exchange public keys between JEdixWinClient and this AuthServer
        /// Client library call this method with JWT with JEDIX_WIN_CLIENT_NAME, get back Api Server Public Key
        /// Steps
        /// 1. Exchange Public Keys between Client and the Authentication Server (for later exchange of a symmetric key to be used by Client and Server)
        /// -- This happens before Client Application Verification
        /// 2. Verify Client Application 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExchangePublicKeys()
        {
            var content = Request.Headers.Authorization.Parameter;
            var jwt = new JWT();
            jwt.Issuer = Aparete.JWT.KeyFile.AUTHENTICATION_SERVER_NAME;
            jwt.Audience = Aparete.JWT.KeyFile.JEDIX_WIN_CLIENT_NAME;
            jwt.AddClaim(JWTConstant.CLAIM_PUBLIC_KEY, AuthenticationServer.PUBLIC_KEY);
            return Ok<string>(jwt.SerializeToBase64UrlString());
        }

        /// <summary>
        /// Method called from Client to verify it as authentic
        /// TODO: Currenty not really verifying the Client. The Client Secrete should be verified
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifyClient([FromBody]Aparte.Credentials.ApplicationCredential application)
        {
            var token = new JWEAsymmetric();
            token.AddClaim(JWTConstant.CLAIM_SYMMETRIC_KEY, Audiences.Item(KeyFile.JEDIX_WIN_CLIENT_NAME).SecretSymmetricKey);
            token.AsymmetricKey = Audiences.Item(KeyFile.JEDIX_WIN_CLIENT_NAME).PublicKey;
            return Ok<string>(token.SerializeToBase64UrlString());
        }

        /// <summary>
        /// Method called from Client to verify a user with user id and password
        /// Query database to test the client credential matches with the record
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Token([FromBody]Aparte.Credentials.UserCredential user)
        {
            //Authentication codes here
            var context = new ApiContext();
            var pw = AsymmetricEncryption.Decrypt(user.Password, AuthenticationServer.PRIVATE_KEY);
            var systemUser = Aparte.WebApi.BeginApiSession.ValidatePassword(context, user.UserId, pw);
            if (systemUser == null)
                return null;//this.Unauthorized(null);
            var encryptedToken = ProduceToken(systemUser.PK, KeyFile.JEDIX_WIN_CLIENT_NAME, context);
            return Ok<string>(encryptedToken);
        }

        /// <summary>
        /// This web action is sent from JEdixWeb
        /// </summary>
        /// <param name="pkSystemUser"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RequestToken()
        {
            var content = Request.Content.ReadAsStringAsync().Result;
            dynamic json = JsonConvert.DeserializeObject(content);
            long pkSystemUser = json.PKSystemUser.Value;
            string clientId = json.ClientId.Value;
            string publicKey = json.PublicKey;
            if (!Audiences.Exists(clientId))
            {
                var key = Aparte.Security.Cryptography.SymmetricKey.GenerateSymmetricKey();
                try
                {
                    Audiences.Add(clientId, publicKey, key);
                }
                catch (Exception ex)
                {

                }

            }
            var encryptedToken = ProduceToken(pkSystemUser, clientId, new ApiContext());
            return Ok<string>(encryptedToken);
        }

        private string ProduceToken(long? pkSystemUser, string client_id, ApiContext context)
        {
            var apiSession = Aparte.WebApi.BeginApiSession.Execute(context, pkSystemUser);
            Aparte.WebApi.GetTenant.Execute(context, apiSession);
            context = null;

            var token = new JWEAsymmetric();
            try
            {
                token.AsymmetricKey = Audiences.Item(client_id).PublicKey;
                token.SetExpiry(apiSession.TokenExpiry);
                token.AddClaim(JWTConstant.PK_SYSTEM_USER, apiSession.PKSystemUser.ToString());
                token.AddClaim(JWTConstant.USER_NAME, apiSession.UserName);
                token.AddClaim(JWTConstant.USER_CODE, apiSession.UserCode);
                token.AddClaim(JWTConstant.ACCESS_TOKEN, apiSession.AccessToken.ToString());
                token.AddClaim(JWTConstant.REFRESH_TOKEN, apiSession.RefreshToken.ToString());
            }
            catch (Exception ex)
            {

            }

            var encryptedToken = token.SerializeToBase64UrlString();
            UserList.Add(apiSession.PKSystemUser, apiSession.UserName, apiSession.UserCode, apiSession.AccessToken, apiSession.RefreshToken, apiSession.TokenExpiry);
            return encryptedToken;
        }

        /// <summary>
        /// Method called from Client to obtain a new Token with a parameter of Refresh Token
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RefreshToken([FromBody]Aparte.Credentials.UserToken userToken)
        {
            var accessToken = AsymmetricEncryption.Decrypt(userToken.AccessToken, AuthenticationServer.PRIVATE_KEY);
            var refreshToken = AsymmetricEncryption.Decrypt(userToken.RefreshToken, AuthenticationServer.PRIVATE_KEY);

            var context = new ApiContext();
            var apiSession = Aparte.WebApi.RefreshApiSession.Execute(context, userToken.PKSystemUser, accessToken, refreshToken);
            var token = new JWEAsymmetric();
            token.AsymmetricKey = Audiences.Item(KeyFile.JEDIX_WIN_CLIENT_NAME).PublicKey;
            token.SetExpiry(apiSession.TokenExpiry);
            token.AddClaim(JWTConstant.PK_SYSTEM_USER, apiSession.PKSystemUser.ToString());
            token.AddClaim(JWTConstant.ACCESS_TOKEN, apiSession.AccessToken.ToString());
            var encryptedToken = token.SerializeToBase64UrlString();

            UserList.Add(apiSession.PKSystemUser, apiSession.UserName, apiSession.UserCode, apiSession.AccessToken, apiSession.RefreshToken, apiSession.TokenExpiry);
            return Ok<string>(encryptedToken);
        }
    }
}
