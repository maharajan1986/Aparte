using Aparete.JWT;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Aparte.AuthServer
{
    public class SecurityHandler : DelegatingHandler
    {
        /// <summary>
        /// The main method called from IIS. All the WebApi call comes here.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {            
            if (request.GetRouteData() != null)
            {
                var template = request.GetRouteData().Route.RouteTemplate;
                if (template == "{*url}") return AuthResponse.RouteMismatch(request);
            }
            if (request.Headers == null) return AuthResponse.NoHeader(request);

            //OData Metadata requests - only for local host to update the T4 template
            if (request.RequestUri.AbsolutePath == "/aparte/$metadata" && request.Headers.Host == "localhost:58578")
                return await base.SendAsync(request, cancellationToken);
            
            //When no authorization.
            if (request.Headers.Authorization == null) return AuthResponse.NoAuthorizationHeader(request);

            //Check request has defined authorization schema. (Basic, Bearer, Digest)
            if (!AuthResponse.IsAuthorizationScheme(request)) return AuthResponse.WrongAuthorizationScheme(request);

            //If basic authorization schema
            if (AuthResponse.IsBasicScheme(request.Headers.Authorization.Scheme))
            {
                var authHeaderValue = SecurityHandler.GetBasicAuthorizationHeader(request);
                if (authHeaderValue == null) return AuthResponse.BasicSchemeError(request);

                return await base.SendAsync(request, cancellationToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }

        private static JWT GetBasicAuthorizationHeader(HttpRequestMessage request)
        {
            var content = request.Headers.Authorization.Parameter;
            if (content == null) return null;

            var jwt = JWT.ParseFromBase64Url(content);
            var jwtDestination = jwt.Audience;
            var jwtIssuer = jwt.Issuer;

            if (jwtDestination != KeyFile.AUTHENTICATION_SERVER_NAME) return null;

            if (jwtIssuer != KeyFile.JEDIX_WIN_CLIENT_NAME)
            {
                //using (var dbContext = new ApiDbContext())
                //{
                //    var issuer = dbContext.Clients.Where(a => a.Name == jwtIssuer).FirstOrDefault();
                //    if (issuer == null)
                //        return null;
                //}
            }
            var publicKey = jwt[JWTConstant.CLAIM_PUBLIC_KEY];
            if (!Audiences.Exists(jwtIssuer))
            {
                var key = Security.Cryptography.SymmetricKey.GenerateSymmetricKey();
                Audiences.Add(jwtIssuer, publicKey, key);
            }
            return jwt;
        }
    }
}
